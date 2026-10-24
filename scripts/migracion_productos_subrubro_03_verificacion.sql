-- =====================================================================
-- ASIGNACION DE SUBRUBRO DE PROVEEDOR A PRODUCTOS - 03: VERIFICACION POSTERIOR (solo SELECT)
-- Verifica el ultimo lote registrado en ProductosRubroMigracion (o el indicado en @lote).
-- =====================================================================
SET @lote = (SELECT MAX(lote) FROM ProductosRubroMigracion);   -- o fijar un lote: SET @lote = '20261101120000';

-- 3.a) Resumen del lote: cada producto debe apuntar al subrubro de su proveedor, hijo de su rubro anterior
SELECT b.lote,
       COUNT(*)                                     AS asignacionesRegistradas,
       SUM(c.consistente = 1)                       AS consistentes,
       SUM(c.consistente = 0)                       AS inconsistentes
FROM ProductosRubroMigracion b
INNER JOIN (
    SELECT b2.id AS idRegistro,
           IFNULL(p.fk_rubro = b2.fk_rubroNuevo
            AND s.fk_rubroPadre = b2.fk_rubroAnterior
            AND TRIM(s.descripcion) COLLATE latin1_swedish_ci =
                CONVERT(LEFT(TRIM(REPLACE(REPLACE(REPLACE(pr.nombreComercial,
                    CHAR(9 USING latin1), ' '), CHAR(10 USING latin1), ' '), CHAR(13 USING latin1), ' ')), 150)
                    USING latin1) COLLATE latin1_swedish_ci, 0) AS consistente
    FROM ProductosRubroMigracion b2
    INNER JOIN Productos p       ON p.id  = b2.fk_producto
    LEFT JOIN Rubros s           ON s.id  = p.fk_rubro
    LEFT JOIN Proveedores pr     ON pr.id = p.fk_proveedor
    WHERE b2.lote = @lote
) c ON c.idRegistro = b.id
GROUP BY b.lote;

-- 3.b) Asignaciones con problemas (debe dar 0 filas)
SELECT b.fk_producto, p.descripcion AS producto, b.fk_rubroAnterior, b.fk_rubroNuevo,
       p.fk_rubro AS rubroActualDelProducto, s.descripcion AS subrubroActual,
       s.fk_rubroPadre AS padreDelSubrubro, pr.nombreComercial AS proveedor
FROM ProductosRubroMigracion b
INNER JOIN Productos p   ON p.id = b.fk_producto
LEFT JOIN Rubros s       ON s.id = p.fk_rubro
LEFT JOIN Proveedores pr ON pr.id = p.fk_proveedor
WHERE b.lote = @lote
  AND (p.fk_rubro <> b.fk_rubroNuevo OR s.id IS NULL OR pr.id IS NULL
       OR s.fk_rubroPadre IS NULL OR s.fk_rubroPadre <> b.fk_rubroAnterior
       OR TRIM(s.descripcion) COLLATE latin1_swedish_ci <>
          CONVERT(LEFT(TRIM(REPLACE(REPLACE(REPLACE(pr.nombreComercial,
              CHAR(9 USING latin1), ' '), CHAR(10 USING latin1), ' '), CHAR(13 USING latin1), ' ')), 150)
              USING latin1) COLLATE latin1_swedish_ci);

-- 3.c) Detalle de lo asignado: producto, rubro anterior, proveedor, subrubro asignado y ruta completa
SELECT b.fk_producto AS idProducto, p.descripcion AS producto,
       b.fk_rubroAnterior AS idRubroAnterior, ra.descripcion AS rubroAnterior,
       p.fk_proveedor AS idProveedor, pr.nombreComercial AS proveedor,
       p.fk_rubro AS idSubrubroAsignado, v.ruta AS rutaCompleta
FROM ProductosRubroMigracion b
INNER JOIN Productos p     ON p.id = b.fk_producto
LEFT JOIN Rubros ra        ON ra.id = b.fk_rubroAnterior
LEFT JOIN Proveedores pr   ON pr.id = p.fk_proveedor
LEFT JOIN vw_RubrosJerarquia v ON v.id = p.fk_rubro
WHERE b.lote = @lote
ORDER BY ra.descripcion, pr.nombreComercial, b.fk_producto;

-- 3.d) Productos con subrubro asignado por migracion vs. total
SELECT COUNT(*) AS productosTotales,
       SUM(p.id IN (SELECT fk_producto FROM ProductosRubroMigracion)) AS conSubrubroAsignadoPorMigracion
FROM Productos p;

-- 3.e) Control de fk_proveedor: debe coincidir con "controlProveedorAntes/Despues" del script 02
SELECT COUNT(*) AS productos,
       IFNULL(SUM(CRC32(CONCAT_WS('|', id, IFNULL(fk_proveedor, '')))), 0) AS controlProveedor
FROM Productos;

-- Tambien: volver a correr el script 01; el estado ASIGNABLE debe dar 0 productos.