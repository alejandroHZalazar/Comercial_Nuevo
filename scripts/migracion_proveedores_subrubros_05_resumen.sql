-- =====================================================================
-- MIGRACION PROVEEDORES -> SUBRUBROS - 05: RESUMEN de filas a crear, excluidos y casos ambiguos
-- (solo SELECT, no modifica nada). Requiere MySQL 8.0 (usa WITH). Sin nombre de base.
-- Ejecutar ANTES de la migracion (03) para revisar el alcance; despues de migrar,
-- "a crear" pasa a 0 y todo queda en "ya existen" (la migracion es idempotente).
-- =====================================================================
SET @incluirBajas = 1;   -- mismo valor que en 01, 02, 03 y 04

-- 5.a) Resumen de cantidades
WITH base0 AS (
    SELECT p.id AS idProducto, p.fk_rubro, p.fk_proveedor,
           r.id AS rubroOk, r.descripcion AS rubroDescripcion, r.fk_rubroPadre AS rubroPadreDelRubro,
           pr.id AS proveedorOk,
           TRIM(REPLACE(REPLACE(REPLACE(pr.nombreComercial,
               CHAR(9 USING latin1), ' '), CHAR(10 USING latin1), ' '), CHAR(13 USING latin1), ' ')) AS nombreCompleto
    FROM Productos p
    LEFT JOIN Rubros r       ON r.id  = p.fk_rubro
    LEFT JOIN Proveedores pr ON pr.id = p.fk_proveedor
    WHERE (@incluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
),
base AS (
    SELECT b.*,
           CONVERT(LEFT(b.nombreCompleto, 150) USING latin1) COLLATE latin1_swedish_ci AS nombre,
           CHAR_LENGTH(b.nombreCompleto) > 150 AS truncado
    FROM base0 b
),
excluidos AS (
    SELECT idProducto,
           CASE WHEN IFNULL(fk_rubro, 0) = 0      THEN 'Sin rubro'
                WHEN rubroOk IS NULL              THEN 'Rubro inexistente'
                WHEN IFNULL(fk_proveedor, 0) = 0  THEN 'Sin proveedor'
                WHEN proveedorOk IS NULL          THEN 'Proveedor inexistente'
                ELSE 'Proveedor sin nombre' END AS motivo
    FROM base
    WHERE rubroOk IS NULL OR proveedorOk IS NULL OR nombre IS NULL OR nombre = ''
),
ya_subrubro AS (
    SELECT idProducto
    FROM base
    WHERE rubroOk IS NOT NULL AND proveedorOk IS NOT NULL AND nombre IS NOT NULL AND nombre <> ''
      AND rubroPadreDelRubro IS NOT NULL
      AND TRIM(rubroDescripcion) COLLATE latin1_swedish_ci = nombre
),
combos AS (
    SELECT fk_rubro AS fk_rubroPadre, nombre,
           COUNT(*) AS productos, COUNT(DISTINCT fk_proveedor) AS proveedores,
           MAX(truncado) AS truncado, MAX(rubroPadreDelRubro IS NOT NULL) AS rubroPadreEsSubrubro
    FROM base
    WHERE rubroOk IS NOT NULL AND proveedorOk IS NOT NULL AND nombre IS NOT NULL AND nombre <> ''
      AND NOT (rubroPadreDelRubro IS NOT NULL AND TRIM(rubroDescripcion) COLLATE latin1_swedish_ci = nombre)
    GROUP BY fk_rubro, nombre
),
existentes AS (
    SELECT fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci AS nombre,
           MIN(id) AS id, COUNT(*) AS repetidos
    FROM Rubros
    WHERE fk_rubroPadre IS NOT NULL
    GROUP BY fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci
)
SELECT 1 AS orden, 'Productos considerados' AS concepto, COUNT(*) AS cantidad FROM base
UNION ALL
SELECT 2, 'Productos involucrados en la migracion', IFNULL(SUM(productos), 0) FROM combos
UNION ALL
SELECT 3, CONCAT('Productos EXCLUIDOS - ', motivo), COUNT(*) FROM excluidos GROUP BY motivo
UNION ALL
SELECT 4, 'Productos omitidos: su rubro ya es el subrubro de su proveedor', COUNT(*) FROM ya_subrubro
UNION ALL
SELECT 5, 'Combinaciones rubro padre + proveedor (subrubros necesarios)', COUNT(*) FROM combos
UNION ALL
SELECT 6, 'Subrubros que SE CREARIAN (filas nuevas en Rubros)', COUNT(*)
FROM combos c LEFT JOIN existentes e ON e.fk_rubroPadre = c.fk_rubroPadre AND e.nombre = c.nombre
WHERE e.id IS NULL
UNION ALL
SELECT 7, 'Subrubros que YA EXISTEN y se reutilizan', COUNT(*)
FROM combos c INNER JOIN existentes e ON e.fk_rubroPadre = c.fk_rubroPadre AND e.nombre = c.nombre
UNION ALL
SELECT 8, 'AMBIGUO: combinaciones con subrubro preexistente repetido', COUNT(*)
FROM combos c INNER JOIN existentes e ON e.fk_rubroPadre = c.fk_rubroPadre AND e.nombre = c.nombre
WHERE e.repetidos > 1
UNION ALL
SELECT 9, 'AMBIGUO: combinaciones que fusionan mas de un proveedor con el mismo nombre', COUNT(*)
FROM combos WHERE proveedores > 1
UNION ALL
SELECT 10, 'AMBIGUO: combinaciones con nombre de proveedor truncado a 150 caracteres', COUNT(*)
FROM combos WHERE truncado = 1
UNION ALL
SELECT 11, 'AMBIGUO: combinaciones bajo un rubro que ya es un subrubro (queda a nivel 3 o mas)', COUNT(*)
FROM combos WHERE rubroPadreEsSubrubro = 1
UNION ALL
SELECT 12, 'Informativo: proveedores presentes en mas de un rubro (subrubros independientes)', COUNT(*)
FROM (SELECT nombre FROM combos GROUP BY nombre HAVING COUNT(*) > 1) t
ORDER BY orden, concepto;

-- 5.b) Detalle de las combinaciones ambiguas (si da 0 filas, no hay casos ambiguos)
WITH base0 AS (
    SELECT p.fk_rubro, p.fk_proveedor,
           r.id AS rubroOk, r.descripcion AS rubroDescripcion, r.fk_rubroPadre AS rubroPadreDelRubro,
           pr.id AS proveedorOk,
           TRIM(REPLACE(REPLACE(REPLACE(pr.nombreComercial,
               CHAR(9 USING latin1), ' '), CHAR(10 USING latin1), ' '), CHAR(13 USING latin1), ' ')) AS nombreCompleto
    FROM Productos p
    LEFT JOIN Rubros r       ON r.id  = p.fk_rubro
    LEFT JOIN Proveedores pr ON pr.id = p.fk_proveedor
    WHERE (@incluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
),
base AS (
    SELECT b.*,
           CONVERT(LEFT(b.nombreCompleto, 150) USING latin1) COLLATE latin1_swedish_ci AS nombre,
           CHAR_LENGTH(b.nombreCompleto) > 150 AS truncado
    FROM base0 b
),
combos AS (
    SELECT fk_rubro AS fk_rubroPadre, nombre,
           COUNT(*) AS productos, COUNT(DISTINCT fk_proveedor) AS proveedores,
           GROUP_CONCAT(DISTINCT fk_proveedor ORDER BY fk_proveedor) AS idsProveedor,
           MAX(truncado) AS truncado, MAX(rubroPadreDelRubro IS NOT NULL) AS rubroPadreEsSubrubro
    FROM base
    WHERE rubroOk IS NOT NULL AND proveedorOk IS NOT NULL AND nombre IS NOT NULL AND nombre <> ''
      AND NOT (rubroPadreDelRubro IS NOT NULL AND TRIM(rubroDescripcion) COLLATE latin1_swedish_ci = nombre)
    GROUP BY fk_rubro, nombre
),
existentes AS (
    SELECT fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci AS nombre,
           MIN(id) AS id, COUNT(*) AS repetidos, GROUP_CONCAT(id ORDER BY id) AS ids
    FROM Rubros
    WHERE fk_rubroPadre IS NOT NULL
    GROUP BY fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci
)
SELECT rp.descripcion AS rubroPadre, c.fk_rubroPadre, c.nombre AS subrubro, c.productos,
       c.idsProveedor, IFNULL(e.repetidos, 0) AS subrubrosPreexistentes, e.ids AS idsPreexistentes,
       CONCAT_WS('; ',
           IF(e.repetidos > 1,           'subrubro preexistente repetido', NULL),
           IF(c.proveedores > 1,         'varios proveedores con el mismo nombre', NULL),
           IF(c.truncado = 1,            'nombre truncado a 150 caracteres', NULL),
           IF(c.rubroPadreEsSubrubro = 1,'el rubro padre ya es un subrubro', NULL)) AS motivoAmbiguedad
FROM combos c
INNER JOIN Rubros rp ON rp.id = c.fk_rubroPadre
LEFT JOIN existentes e ON e.fk_rubroPadre = c.fk_rubroPadre AND e.nombre = c.nombre
WHERE e.repetidos > 1 OR c.proveedores > 1 OR c.truncado = 1 OR c.rubroPadreEsSubrubro = 1
ORDER BY rp.descripcion, c.nombre;