-- =====================================================================
-- ASIGNACION DE SUBRUBRO DE PROVEEDOR A PRODUCTOS - 01: DIAGNOSTICO PREVIO (solo SELECT)
-- Un producto es ASIGNABLE solo si existe EXACTAMENTE UN subrubro cuyo padre es el rubro actual
-- del producto (Productos.fk_rubro) y cuya descripcion coincide con Proveedores.nombreComercial
-- del proveedor del producto (comparacion sin mayusculas, sin espacios/tab/saltos en los extremos).
-- Usa una tabla temporal de sesion (no queda ningun objeto en la base). Sin nombre de base.
-- =====================================================================
SET @incluirBajas = 1;   -- 1 = incluye productos dados de baja; 0 = solo activos (igual en 02 y 03)

DROP TEMPORARY TABLE IF EXISTS tmp_asignacion;
CREATE TEMPORARY TABLE tmp_asignacion AS
SELECT x.id                       AS idProducto,
       x.descripcion              AS producto,
       x.fk_rubro                 AS idRubroActual,
       r.descripcion              AS rubroActual,
       x.fk_proveedor             AS idProveedor,
       x.proveedor                AS proveedor,
       sub.id                     AS idSubrubro,
       IFNULL(sub.cantidad, 0)    AS subrubrosCoincidentes,
       sub.ids                    AS idsSubrubros,
       CASE
           WHEN IFNULL(x.fk_rubro, 0) = 0                  THEN 'SIN RUBRO'
           WHEN r.id IS NULL                               THEN 'RUBRO INEXISTENTE'
           WHEN IFNULL(x.fk_proveedor, 0) = 0              THEN 'SIN PROVEEDOR'
           WHEN x.idProv IS NULL                           THEN 'PROVEEDOR INEXISTENTE'
           WHEN x.nombre IS NULL OR x.nombre = ''          THEN 'PROVEEDOR SIN NOMBRE'
           WHEN r.fk_rubroPadre IS NOT NULL
                AND TRIM(r.descripcion) COLLATE latin1_swedish_ci = x.nombre
                                                           THEN 'YA ASIGNADO AL SUBRUBRO DEL PROVEEDOR'
           WHEN sub.id IS NULL                             THEN 'SIN SUBRUBRO COINCIDENTE'
           WHEN sub.cantidad > 1                           THEN 'AMBIGUO (VARIOS SUBRUBROS)'
           ELSE 'ASIGNABLE'
       END AS estado
FROM (
    SELECT p.id, p.descripcion, p.fk_rubro, p.fk_proveedor,
           pr.id AS idProv, pr.nombreComercial AS proveedor,
           CONVERT(LEFT(TRIM(REPLACE(REPLACE(REPLACE(pr.nombreComercial,
               CHAR(9 USING latin1), ' '), CHAR(10 USING latin1), ' '), CHAR(13 USING latin1), ' ')), 150)
               USING latin1) COLLATE latin1_swedish_ci AS nombre
    FROM Productos p
    LEFT JOIN Proveedores pr ON pr.id = p.fk_proveedor
    WHERE (@incluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
) x
LEFT JOIN Rubros r ON r.id = x.fk_rubro
LEFT JOIN (
    SELECT fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci AS nombre,
           MIN(id) AS id, COUNT(*) AS cantidad, GROUP_CONCAT(id ORDER BY id) AS ids
    FROM Rubros
    WHERE fk_rubroPadre IS NOT NULL
    GROUP BY fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci
) sub ON sub.fk_rubroPadre = x.fk_rubro AND sub.nombre = x.nombre;

-- 1.a) Resumen por estado (los ASIGNABLE son los unicos que actualizaria el script 02)
SELECT estado, COUNT(*) AS productos
FROM tmp_asignacion
GROUP BY estado
ORDER BY estado;

-- 1.b) Productos que NO se actualizarian (sin proveedor, sin subrubro, ambiguos, etc.)
SELECT estado, idProducto, producto, idRubroActual, rubroActual, idProveedor, proveedor,
       idSubrubro, subrubrosCoincidentes, idsSubrubros
FROM tmp_asignacion
WHERE estado <> 'ASIGNABLE'
ORDER BY estado, idProducto;

-- 1.c) Productos ASIGNABLES: producto, rubro actual, proveedor, subrubro encontrado e identificadores
SELECT idProducto, producto, idRubroActual, rubroActual, idProveedor, proveedor,
       idSubrubro AS idSubrubroEncontrado
FROM tmp_asignacion
WHERE estado = 'ASIGNABLE'
ORDER BY rubroActual, proveedor, idProducto;

-- 1.d) Resumen de lo que se asignaria: rubro actual + proveedor -> subrubro, con cantidad de productos
SELECT idRubroActual, rubroActual, idProveedor, proveedor, idSubrubro, COUNT(*) AS productos
FROM tmp_asignacion
WHERE estado = 'ASIGNABLE'
GROUP BY idRubroActual, rubroActual, idProveedor, proveedor, idSubrubro
ORDER BY rubroActual, proveedor;

DROP TEMPORARY TABLE IF EXISTS tmp_asignacion;