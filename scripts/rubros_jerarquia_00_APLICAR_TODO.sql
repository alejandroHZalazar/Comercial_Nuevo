-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - APLICAR TODO (scripts 01 a 07)
-- Sin nombre de base: ejecutar con la base deseada seleccionada. Luego correr 08_validaciones.
-- Es re-ejecutable (valida existencia de cada objeto). NO incluye 08 ni el rollback 09.
-- =====================================================================

-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 01: columna padre, indice y clave foranea
-- Sin nombre de base: se ejecuta sobre la base seleccionada (DATABASE()).
-- Re-ejecutable: valida la existencia de la tabla y de cada objeto antes de crearlo.
-- =====================================================================

SET @tieneRubros = (SELECT COUNT(*) FROM information_schema.TABLES
                    WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros');

-- Columna fk_rubroPadre (NULL = rubro principal)
SET @s = IF(@tieneRubros = 0, 'SELECT ''No existe la tabla Rubros en esta base'' AS info',
        IF((SELECT COUNT(*) FROM information_schema.COLUMNS
            WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
              AND COLUMN_NAME = 'fk_rubroPadre') = 0,
           'ALTER TABLE `Rubros` ADD COLUMN `fk_rubroPadre` INT NULL DEFAULT NULL AFTER `descripcion`',
           'SELECT ''columna fk_rubroPadre ya existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Indice
SET @s = IF(@tieneRubros = 0, 'SELECT ''No existe la tabla Rubros en esta base'' AS info',
        IF((SELECT COUNT(*) FROM information_schema.STATISTICS
            WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
              AND INDEX_NAME = 'idx_rubros_padre') = 0,
           'ALTER TABLE `Rubros` ADD INDEX `idx_rubros_padre` (`fk_rubroPadre`)',
           'SELECT ''indice idx_rubros_padre ya existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Clave foranea (RESTRICT: no se puede borrar un rubro que tiene subrubros)
SET @s = IF(@tieneRubros = 0, 'SELECT ''No existe la tabla Rubros en esta base'' AS info',
        IF((SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS
            WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
              AND CONSTRAINT_NAME = 'fk_rubros_padre' AND CONSTRAINT_TYPE = 'FOREIGN KEY') = 0,
           'ALTER TABLE `Rubros` ADD CONSTRAINT `fk_rubros_padre` FOREIGN KEY (`fk_rubroPadre`) REFERENCES `Rubros` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT',
           'SELECT ''clave foranea fk_rubros_padre ya existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Nota: no se usa CHECK (MySQL no permite CHECK sobre columnas AUTO_INCREMENT, error 3818).
-- La regla "un rubro no puede ser su propio padre" la cubren sp_RubrosABM y el trigger trg_Rubros_bu (script 04).

-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 02: tabla de clausura RubrosArbol
-- Guarda todos los pares ancestro-descendiente (incluido cada rubro consigo mismo, profundidad 0).
-- =====================================================================
CREATE TABLE IF NOT EXISTS `RubrosArbol` (
  `fk_ancestro`     INT NOT NULL,
  `fk_descendiente` INT NOT NULL,
  `profundidad`     INT NOT NULL,
  PRIMARY KEY (`fk_ancestro`, `fk_descendiente`),
  KEY `idx_rubrosarbol_desc` (`fk_descendiente`, `fk_ancestro`),
  CONSTRAINT `fk_ra_ancestro` FOREIGN KEY (`fk_ancestro`)     REFERENCES `Rubros` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_ra_desc`     FOREIGN KEY (`fk_descendiente`) REFERENCES `Rubros` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 03: sp_RubrosReconstruirArbol
-- Reconstruye RubrosArbol a partir de Rubros.fk_rubroPadre (bucle, no requiere MySQL 8).
-- Compatible con "safe update mode" de MySQL Workbench (el DELETE usa una columna clave y LIMIT).
-- =====================================================================
DROP PROCEDURE IF EXISTS sp_RubrosReconstruirArbol;
DELIMITER $$
CREATE PROCEDURE sp_RubrosReconstruirArbol()
BEGIN
    DECLARE filas INT DEFAULT 1;
    DECLARE nivel INT DEFAULT 0;

    DELETE FROM RubrosArbol WHERE fk_ancestro >= 0 LIMIT 2000000000;
    INSERT INTO RubrosArbol (fk_ancestro, fk_descendiente, profundidad)
        SELECT id, id, 0 FROM Rubros;

    WHILE filas > 0 AND nivel < 50 DO
        SET nivel = nivel + 1;
        INSERT INTO RubrosArbol (fk_ancestro, fk_descendiente, profundidad)
            SELECT a.fk_ancestro, r.id, nivel
            FROM RubrosArbol a
            INNER JOIN Rubros r ON r.fk_rubroPadre = a.fk_descendiente
            WHERE a.profundidad = nivel - 1;
        SET filas = ROW_COUNT();
    END WHILE;
END$$
DELIMITER ;

-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 04: triggers
-- trg_Rubros_ai: mantiene RubrosArbol al insertar un rubro (aunque lo cree el SP viejo
--                sp_ConfiguracionABMrubros desde un .exe anterior).
-- trg_Rubros_bu: impide que un rubro sea su propio padre.
-- Requiere permiso TRIGGER para el usuario.
-- =====================================================================
DROP TRIGGER IF EXISTS trg_Rubros_ai;
DROP TRIGGER IF EXISTS trg_Rubros_bu;
DELIMITER $$
CREATE TRIGGER trg_Rubros_ai AFTER INSERT ON Rubros FOR EACH ROW
BEGIN
    INSERT INTO RubrosArbol (fk_ancestro, fk_descendiente, profundidad)
    VALUES (NEW.id, NEW.id, 0);

    IF NEW.fk_rubroPadre IS NOT NULL THEN
        INSERT INTO RubrosArbol (fk_ancestro, fk_descendiente, profundidad)
        SELECT fk_ancestro, NEW.id, profundidad + 1
        FROM RubrosArbol
        WHERE fk_descendiente = NEW.fk_rubroPadre;
    END IF;
END$$

CREATE TRIGGER trg_Rubros_bu BEFORE UPDATE ON Rubros FOR EACH ROW
BEGIN
    IF NEW.fk_rubroPadre IS NOT NULL AND NEW.fk_rubroPadre = NEW.id THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Un rubro no puede ser su propio padre';
    END IF;
END$$
DELIMITER ;

-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 05: inicializacion
-- Los rubros existentes quedan como rubros principales (fk_rubroPadre = NULL).
-- Los productos NO se tocan (Productos.fk_rubro sigue igual).
-- Valida que existan la columna, la tabla RubrosArbol y el SP antes de ejecutar.
-- =====================================================================
SET @ok = (SELECT COUNT(*) FROM information_schema.ROUTINES
           WHERE ROUTINE_SCHEMA = DATABASE() AND ROUTINE_TYPE = 'PROCEDURE'
             AND ROUTINE_NAME = 'sp_RubrosReconstruirArbol')
        + (SELECT COUNT(*) FROM information_schema.TABLES
           WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubrosarbol')
        + (SELECT COUNT(*) FROM information_schema.COLUMNS
           WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
             AND COLUMN_NAME = 'fk_rubroPadre');
SET @s = IF(@ok = 3, 'CALL sp_RubrosReconstruirArbol()',
            'SELECT ''Faltan objetos: ejecutar antes los scripts 01 a 03'' AS info');
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 06: vista vw_RubrosJerarquia
-- nivel: 0 = rubro principal. ruta: "Rubro > Subrubro > Subrubro".
-- =====================================================================
CREATE OR REPLACE VIEW vw_RubrosJerarquia AS
SELECT
    r.id,
    r.descripcion,
    r.fk_rubroPadre,
    rp.descripcion AS rubroPadre,
    (SELECT MAX(a.profundidad) FROM RubrosArbol a WHERE a.fk_descendiente = r.id) AS nivel,
    (SELECT GROUP_CONCAT(ra.descripcion ORDER BY a.profundidad DESC SEPARATOR ' > ')
       FROM RubrosArbol a
       INNER JOIN Rubros ra ON ra.id = a.fk_ancestro
      WHERE a.fk_descendiente = r.id) AS ruta,
    IF(r.fk_rubroPadre IS NULL, 1, 0) AS esRaiz,
    IF(EXISTS (SELECT 1 FROM Rubros h WHERE h.fk_rubroPadre = r.id), 0, 1) AS esHoja
FROM Rubros r
LEFT JOIN Rubros rp ON rp.id = r.fk_rubroPadre;

-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 07: sp_RubrosABM, sp_RubrosDescendientes, sp_RubrosAntecesores
-- sp_RubrosABM: unTipo 1 = alta, 2 = modificacion, 3 = baja. unPadre NULL o 0 = rubro principal.
-- Codigos de salida: >0 id | -1 error | -2 ciclo | -3 tiene subrubros | -4 tiene productos
--                    -5 nombre repetido bajo el mismo padre | -6 padre inexistente | -7 descripcion vacia
-- El SP anterior sp_ConfiguracionABMrubros NO se modifica (compatibilidad con .exe anteriores).
-- =====================================================================
DROP PROCEDURE IF EXISTS sp_RubrosABM;
DROP PROCEDURE IF EXISTS sp_RubrosDescendientes;
DROP PROCEDURE IF EXISTS sp_RubrosAntecesores;
DELIMITER $$

CREATE PROCEDURE sp_RubrosABM(
    IN  unId             INT,
    IN  unTipo           INT,
    IN  unaDescripcion   VARCHAR(150),
    IN  unPadre          INT,
    OUT salida           INT
)
proc: BEGIN
    DECLARE vCant INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SET salida = -1;
    END;

    SET salida = 0;
    IF unPadre = 0 THEN SET unPadre = NULL; END IF;

    START TRANSACTION;

    IF unTipo IN (1, 2) THEN
        IF unaDescripcion IS NULL OR TRIM(unaDescripcion) = '' THEN
            SET salida = -7; ROLLBACK; LEAVE proc;
        END IF;

        IF unPadre IS NOT NULL THEN
            SELECT COUNT(*) INTO vCant FROM Rubros WHERE id = unPadre;
            IF vCant = 0 THEN SET salida = -6; ROLLBACK; LEAVE proc; END IF;
        END IF;

        SELECT COUNT(*) INTO vCant FROM Rubros
         WHERE descripcion = unaDescripcion
           AND id <> IFNULL(unId, 0)
           AND ((fk_rubroPadre IS NULL AND unPadre IS NULL) OR fk_rubroPadre = unPadre);
        IF vCant > 0 THEN SET salida = -5; ROLLBACK; LEAVE proc; END IF;
    END IF;

    IF unTipo = 1 THEN
        INSERT INTO Rubros (descripcion, fk_rubroPadre) VALUES (unaDescripcion, unPadre);
        SET salida = LAST_INSERT_ID();

    ELSEIF unTipo = 2 THEN
        IF unPadre IS NOT NULL THEN
            -- el nuevo padre no puede ser el mismo rubro ni uno de sus descendientes
            SELECT COUNT(*) INTO vCant FROM RubrosArbol
             WHERE fk_ancestro = unId AND fk_descendiente = unPadre;
            IF vCant > 0 THEN SET salida = -2; ROLLBACK; LEAVE proc; END IF;
        END IF;
        UPDATE Rubros SET descripcion = unaDescripcion, fk_rubroPadre = unPadre WHERE id = unId;
        CALL sp_RubrosReconstruirArbol();
        SET salida = unId;

    ELSEIF unTipo = 3 THEN
        SELECT COUNT(*) INTO vCant FROM Rubros WHERE fk_rubroPadre = unId;
        IF vCant > 0 THEN SET salida = -3; ROLLBACK; LEAVE proc; END IF;

        SELECT COUNT(*) INTO vCant FROM Productos WHERE fk_rubro = unId;
        IF vCant > 0 THEN SET salida = -4; ROLLBACK; LEAVE proc; END IF;

        DELETE FROM Rubros WHERE id = unId;     -- RubrosArbol se limpia por ON DELETE CASCADE
        SET salida = unId;
    END IF;

    COMMIT;
END$$

CREATE PROCEDURE sp_RubrosDescendientes(IN unId INT)
BEGIN
    SELECT v.*
    FROM RubrosArbol a
    INNER JOIN vw_RubrosJerarquia v ON v.id = a.fk_descendiente
    WHERE a.fk_ancestro = unId
    ORDER BY v.ruta;
END$$

CREATE PROCEDURE sp_RubrosAntecesores(IN unId INT)
BEGIN
    SELECT v.*
    FROM RubrosArbol a
    INNER JOIN vw_RubrosJerarquia v ON v.id = a.fk_ancestro
    WHERE a.fk_descendiente = unId
    ORDER BY a.profundidad DESC;
END$$

DELIMITER ;