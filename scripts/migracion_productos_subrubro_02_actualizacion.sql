-- =====================================================================
-- ASIGNACION DE SUBRUBRO DE PROVEEDOR A PRODUCTOS - 02: ACTUALIZACION (transaccional, re-ejecutable)
-- Actualiza SOLO Productos.fk_rubro (del rubro padre actual al subrubro de su proveedor) cuando existe
-- una coincidencia exacta y unica. No crea subrubros, no modifica fk_proveedor ni otros datos.
-- El rubro original queda representado por la jerarquia (Rubros.fk_rubroPadre del subrubro) y ademas
-- se guarda en la tabla de auditoria ProductosRubroMigracion (permite revertir con el script 04).
-- Sin nombre de base. Ejecutar antes el script 01 y revisar el resultado.
-- Si Workbench informara el error 1175 (safe update mode), ejecutar antes: SET SQL_SAFE_UPDATES = 0;
-- =====================================================================
SET @incluirBajas = 1;   -- igual que en 01 y 03

CREATE TABLE IF NOT EXISTS `ProductosRubroMigracion` (
  `id`               INT NOT NULL AUTO_INCREMENT,
  `fk_producto`      INT NOT NULL,
  `fk_rubroAnterior` INT NOT NULL,
  `fk_rubroNuevo`    INT NOT NULL,
  `lote`             VARCHAR(20) NOT NULL,
  `fecha`            DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `idx_prm_producto` (`fk_producto`),
  KEY `idx_prm_lote` (`lote`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

DROP PROCEDURE IF EXISTS tmp_AsignarSubrubroProveedor;
DELIMITER $$
CREATE PROCEDURE tmp_AsignarSubrubroProveedor(IN pIncluirBajas TINYINT)
proc: BEGIN
    DECLARE vLote            VARCHAR(20);
    DECLARE vConsiderados    INT DEFAULT 0;
    DECLARE vAsignables      INT DEFAULT 0;
    DECLARE vActualizados    INT DEFAULT 0;
    DECLARE vInconsistentes  INT DEFAULT 0;
    DECLARE vProductosAntes  INT DEFAULT 0;
    DECLARE vProductosDesp   INT DEFAULT 0;
    DECLARE vChecksumAntes   BIGINT DEFAULT 0;
    DECLARE vChecksumDesp    BIGINT DEFAULT 0;
    DECLARE vMensaje         TEXT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 vMensaje = MESSAGE_TEXT;
        ROLLBACK;
        DROP TEMPORARY TABLE IF EXISTS tmp_asignables;
        SELECT 'ERROR: actualizacion revertida (ROLLBACK)' AS resultado, vMensaje AS detalle;
    END;

    IF (SELECT COUNT(*) FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
          AND COLUMN_NAME = 'fk_rubroPadre') = 0 THEN
        SELECT 'NO SE ACTUALIZO: falta aplicar la jerarquia de rubros' AS resultado;
        LEAVE proc;
    END IF;

    SET vLote = DATE_FORMAT(NOW(), '%Y%m%d%H%i%s');

    SELECT COUNT(*), IFNULL(SUM(CRC32(CONCAT_WS('|', id, IFNULL(fk_proveedor, '')))), 0)
      INTO vProductosAntes, vChecksumAntes FROM Productos;
    SELECT COUNT(*) INTO vConsiderados FROM Productos WHERE (pIncluirBajas = 1 OR IFNULL(baja, 0) = 0);

    START TRANSACTION;

    -- Solo coincidencias inequivocas: un unico subrubro con el nombre del proveedor bajo el rubro actual
    DROP TEMPORARY TABLE IF EXISTS tmp_asignables;
    CREATE TEMPORARY TABLE tmp_asignables AS
        SELECT x.id AS idProducto, x.fk_rubro AS idRubroActual, sub.id AS idSubrubro
        FROM (
            SELECT p.id, p.fk_rubro, pr.id AS idProv,
                   CONVERT(LEFT(TRIM(REPLACE(REPLACE(REPLACE(pr.nombreComercial,
                       CHAR(9 USING latin1), ' '), CHAR(10 USING latin1), ' '), CHAR(13 USING latin1), ' ')), 150)
                       USING latin1) COLLATE latin1_swedish_ci AS nombre
            FROM Productos p
            INNER JOIN Proveedores pr ON pr.id = p.fk_proveedor
            WHERE (pIncluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
        ) x
        INNER JOIN Rubros r ON r.id = x.fk_rubro
        INNER JOIN (
            SELECT fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci AS nombre,
                   MIN(id) AS id, COUNT(*) AS cantidad
            FROM Rubros
            WHERE fk_rubroPadre IS NOT NULL
            GROUP BY fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci
        ) sub ON sub.fk_rubroPadre = x.fk_rubro AND sub.nombre = x.nombre AND sub.cantidad = 1
        WHERE x.nombre IS NOT NULL AND x.nombre <> ''
          AND NOT (r.fk_rubroPadre IS NOT NULL
                   AND TRIM(r.descripcion) COLLATE latin1_swedish_ci = x.nombre);

    SELECT COUNT(*) INTO vAsignables FROM tmp_asignables;

    IF vAsignables = 0 THEN
        ROLLBACK;
        DROP TEMPORARY TABLE IF EXISTS tmp_asignables;
        SELECT 'SIN CAMBIOS: no hay productos asignables (ya asignados o sin subrubro coincidente)' AS resultado,
               vConsiderados AS productosConsiderados;
        LEAVE proc;
    END IF;

    -- Auditoria: rubro anterior y subrubro nuevo de cada producto
    INSERT INTO ProductosRubroMigracion (fk_producto, fk_rubroAnterior, fk_rubroNuevo, lote)
    SELECT idProducto, idRubroActual, idSubrubro, vLote FROM tmp_asignables;

    -- Actualizacion: solo fk_rubro; la condicion sobre el rubro actual evita pisar cambios concurrentes
    UPDATE tmp_asignables t
    STRAIGHT_JOIN Productos p ON p.id = t.idProducto
       SET p.fk_rubro = t.idSubrubro
     WHERE p.fk_rubro = t.idRubroActual;
    SET vActualizados = ROW_COUNT();

    -- Validaciones antes de confirmar
    SELECT COUNT(*) INTO vInconsistentes
    FROM ProductosRubroMigracion b
    INNER JOIN Productos p ON p.id = b.fk_producto
    LEFT JOIN Rubros s ON s.id = p.fk_rubro
    WHERE b.lote = vLote
      AND (p.fk_rubro <> b.fk_rubroNuevo OR s.id IS NULL
           OR s.fk_rubroPadre IS NULL OR s.fk_rubroPadre <> b.fk_rubroAnterior);

    SELECT COUNT(*), IFNULL(SUM(CRC32(CONCAT_WS('|', id, IFNULL(fk_proveedor, '')))), 0)
      INTO vProductosDesp, vChecksumDesp FROM Productos;

    IF vActualizados <> vAsignables OR vInconsistentes > 0
       OR vProductosAntes <> vProductosDesp OR vChecksumAntes <> vChecksumDesp THEN
        ROLLBACK;
        DROP TEMPORARY TABLE IF EXISTS tmp_asignables;
        SELECT 'NO SE ACTUALIZO: validaciones fallidas (ROLLBACK)' AS resultado,
               vAsignables AS asignables, vActualizados AS actualizados, vInconsistentes AS inconsistentes,
               (vChecksumAntes <> vChecksumDesp) AS proveedorModificado;
        LEAVE proc;
    END IF;

    COMMIT;
    DROP TEMPORARY TABLE IF EXISTS tmp_asignables;

    SELECT 'ASIGNACION OK' AS resultado,
           vLote                          AS lote,
           vConsiderados                  AS productosConsiderados,
           vActualizados                  AS productosActualizados,
           vConsiderados - vActualizados  AS productosOmitidos,
           vInconsistentes                AS inconsistencias,
           vChecksumAntes                 AS controlProveedorAntes,
           vChecksumDesp                  AS controlProveedorDespues;
END$$
DELIMITER ;

CALL tmp_AsignarSubrubroProveedor(@incluirBajas);
DROP PROCEDURE IF EXISTS tmp_AsignarSubrubroProveedor;