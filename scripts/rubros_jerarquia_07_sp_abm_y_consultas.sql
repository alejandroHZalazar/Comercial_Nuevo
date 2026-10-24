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