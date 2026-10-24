-- =====================================================================
-- MIGRACION PROVEEDORES -> SUBRUBROS - 03: MIGRACION (transaccional, re-ejecutable)
-- Crea, bajo cada rubro padre (Productos.fk_rubro), un subrubro con el nombre del proveedor
-- (Proveedores.nombreComercial) si todavia no existe; si existe lo reutiliza.
-- NO modifica Productos (fk_rubro y fk_proveedor se conservan) ni rubros preexistentes.
-- Requiere la jerarquia de rubros aplicada (Rubros.fk_rubroPadre, RubrosArbol, sp_RubrosReconstruirArbol).
-- Sin nombre de base: se ejecuta sobre la base seleccionada.
-- Se crea un procedimiento temporal solo para poder usar transaccion + manejo de errores; se elimina al terminar.
-- Ejecutar antes los scripts 01 y 02 y revisar el resultado.
-- =====================================================================
SET @incluirBajas = 1;   -- 1 = incluye productos dados de baja; 0 = solo activos (igual que en 01 y 02)

DROP PROCEDURE IF EXISTS tmp_MigrarProveedoresASubrubros;
DELIMITER $$
CREATE PROCEDURE tmp_MigrarProveedoresASubrubros(IN pIncluirBajas TINYINT)
proc: BEGIN
    DECLARE vMaxIdAntes     INT DEFAULT 0;
    DECLARE vRubrosAntes    INT DEFAULT 0;
    DECLARE vRubrosDespues  INT DEFAULT 0;
    DECLARE vCombinaciones  INT DEFAULT 0;
    DECLARE vYaExistian     INT DEFAULT 0;
    DECLARE vCreados        INT DEFAULT 0;
    DECLARE vFaltan         INT DEFAULT 0;
    DECLARE vDuplicados     INT DEFAULT 0;
    DECLARE vChecksumAntes  BIGINT DEFAULT 0;
    DECLARE vChecksumDesp   BIGINT DEFAULT 0;
    DECLARE vMensaje        TEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 vMensaje = MESSAGE_TEXT;
        ROLLBACK;
        DROP TEMPORARY TABLE IF EXISTS tmp_subrubros_proveedor;
        SELECT 'ERROR: migracion revertida (ROLLBACK)' AS resultado, vMensaje AS detalle;
    END;

    -- Precondiciones: jerarquia de rubros aplicada
    IF (SELECT COUNT(*) FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
          AND COLUMN_NAME = 'fk_rubroPadre') = 0
       OR (SELECT COUNT(*) FROM information_schema.TABLES
           WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubrosarbol') = 0
       OR (SELECT COUNT(*) FROM information_schema.ROUTINES
           WHERE ROUTINE_SCHEMA = DATABASE() AND ROUTINE_TYPE = 'PROCEDURE'
             AND ROUTINE_NAME = 'sp_RubrosReconstruirArbol') = 0 THEN
        SELECT 'NO SE MIGRO: falta aplicar la jerarquia de rubros (scripts rubros_jerarquia_00_APLICAR_TODO)' AS resultado;
        LEAVE proc;
    END IF;

    SELECT COUNT(*), IFNULL(MAX(id), 0) INTO vRubrosAntes, vMaxIdAntes FROM Rubros;
    SELECT IFNULL(SUM(CRC32(CONCAT_WS('|', id, IFNULL(fk_rubro, ''), IFNULL(fk_proveedor, '')))), 0)
      INTO vChecksumAntes FROM Productos;

    START TRANSACTION;

    -- Combinaciones (rubro padre, nombre de proveedor normalizado) de los productos involucrados
    DROP TEMPORARY TABLE IF EXISTS tmp_subrubros_proveedor;
    CREATE TEMPORARY TABLE tmp_subrubros_proveedor AS
        SELECT s.fk_rubro AS fk_rubroPadre, s.nombre,
               COUNT(*) AS productos, COUNT(DISTINCT s.fk_proveedor) AS proveedores
        FROM (
            SELECT p.id, p.fk_rubro, p.fk_proveedor,
                   CONVERT(LEFT(TRIM(REPLACE(REPLACE(REPLACE(pr.nombreComercial,
                       CHAR(9 USING latin1), ' '), CHAR(10 USING latin1), ' '), CHAR(13 USING latin1), ' ')), 150)
                       USING latin1) COLLATE latin1_swedish_ci AS nombre,
                   r.descripcion AS rubroDescripcion, r.fk_rubroPadre AS rubroPadreDelRubro
            FROM Productos p
            INNER JOIN Rubros r       ON r.id  = p.fk_rubro
            INNER JOIN Proveedores pr ON pr.id = p.fk_proveedor
            WHERE (pIncluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
        ) s
        WHERE s.nombre IS NOT NULL AND s.nombre <> ''
          AND NOT (s.rubroPadreDelRubro IS NOT NULL
                   AND TRIM(s.rubroDescripcion) COLLATE latin1_swedish_ci = s.nombre)
        GROUP BY s.fk_rubro, s.nombre;

    SELECT COUNT(*) INTO vCombinaciones FROM tmp_subrubros_proveedor;

    SELECT COUNT(*) INTO vYaExistian
    FROM tmp_subrubros_proveedor c
    WHERE EXISTS (SELECT 1 FROM Rubros x
                  WHERE x.fk_rubroPadre = c.fk_rubroPadre
                    AND TRIM(x.descripcion) COLLATE latin1_swedish_ci = c.nombre);

    -- Alta de los subrubros que no existen (el trigger trg_Rubros_ai mantiene RubrosArbol)
    INSERT INTO Rubros (descripcion, fk_rubroPadre)
    SELECT c.nombre, c.fk_rubroPadre
    FROM tmp_subrubros_proveedor c
    WHERE NOT EXISTS (SELECT 1 FROM Rubros x
                      WHERE x.fk_rubroPadre = c.fk_rubroPadre
                        AND TRIM(x.descripcion) COLLATE latin1_swedish_ci = c.nombre)
    ORDER BY c.fk_rubroPadre, c.nombre;

    SELECT COUNT(*) INTO vCreados FROM Rubros WHERE id > vMaxIdAntes;

    -- Reconstruye la clausura por seguridad (idempotente; cubre bases sin el trigger)
    CALL sp_RubrosReconstruirArbol();

    -- Validaciones antes de confirmar
    SELECT COUNT(*) INTO vFaltan
    FROM tmp_subrubros_proveedor c
    WHERE NOT EXISTS (SELECT 1 FROM Rubros x
                      WHERE x.fk_rubroPadre = c.fk_rubroPadre
                        AND TRIM(x.descripcion) COLLATE latin1_swedish_ci = c.nombre);

    SELECT COUNT(*) INTO vDuplicados
    FROM tmp_subrubros_proveedor c
    WHERE (SELECT COUNT(*) FROM Rubros x
           WHERE x.fk_rubroPadre = c.fk_rubroPadre
             AND TRIM(x.descripcion) COLLATE latin1_swedish_ci = c.nombre) > 1;

    SELECT IFNULL(SUM(CRC32(CONCAT_WS('|', id, IFNULL(fk_rubro, ''), IFNULL(fk_proveedor, '')))), 0)
      INTO vChecksumDesp FROM Productos;

    IF vFaltan > 0 OR vChecksumAntes <> vChecksumDesp OR vCreados <> (vCombinaciones - vYaExistian) THEN
        ROLLBACK;
        DROP TEMPORARY TABLE IF EXISTS tmp_subrubros_proveedor;
        SELECT 'NO SE MIGRO: validaciones fallidas (ROLLBACK)' AS resultado,
               vCombinaciones AS combinaciones, vYaExistian AS yaExistian, vCreados AS creados,
               vFaltan AS combinacionesSinSubrubro, (vChecksumAntes <> vChecksumDesp) AS productosModificados;
        LEAVE proc;
    END IF;

    COMMIT;
    SELECT COUNT(*) INTO vRubrosDespues FROM Rubros;
    DROP TEMPORARY TABLE IF EXISTS tmp_subrubros_proveedor;

    SELECT 'MIGRACION OK' AS resultado,
           vCombinaciones AS combinacionesRubroProveedor,
           vYaExistian    AS subrubrosYaExistentes_reutilizados,
           vCreados       AS subrubrosCreados,
           vDuplicados    AS combinacionesConSubrubroRepetidoPreexistente,
           vRubrosAntes   AS rubrosAntes,
           vRubrosDespues AS rubrosDespues;
END$$
DELIMITER ;

CALL tmp_MigrarProveedoresASubrubros(@incluirBajas);
DROP PROCEDURE IF EXISTS tmp_MigrarProveedoresASubrubros;