-- =====================================================================
-- MIGRACION PROVEEDORES -> SUBRUBROS - 01: DIAGNOSTICO (solo SELECT, no modifica nada)
-- Requiere haber aplicado la jerarquia de rubros (Rubros.fk_rubroPadre).
-- Sin nombre de base: se ejecuta sobre la base seleccionada.
-- =====================================================================
SET @incluirBajas = 1;   -- 1 = incluye productos dados de baja (Productos.baja = 1); 0 = solo activos

-- 1.a) Rubro padre x proveedor: cantidad de productos involucrados
SELECT p.fk_rubro                      AS idRubroPadre,
       r.descripcion                   AS rubroPadre,
       pr.id                           AS idProveedor,
       pr.nombreComercial              AS proveedor,
       COUNT(*)                        AS productos,
       SUM(IFNULL(p.baja, 0) = 1)      AS productosDeBaja
FROM Productos p
INNER JOIN Rubros r      ON r.id  = p.fk_rubro
INNER JOIN Proveedores pr ON pr.id = p.fk_proveedor
WHERE (@incluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
GROUP BY p.fk_rubro, r.descripcion, pr.id, pr.nombreComercial
ORDER BY r.descripcion, pr.nombreComercial;

-- 1.b) Productos que quedan EXCLUIDOS de la migracion y el motivo
SELECT p.id AS idProducto, p.descripcion AS producto, p.fk_rubro, p.fk_proveedor,
       CASE
           WHEN IFNULL(p.fk_rubro, 0) = 0         THEN 'Sin rubro'
           WHEN r.id IS NULL                      THEN 'Rubro inexistente'
           WHEN IFNULL(p.fk_proveedor, 0) = 0     THEN 'Sin proveedor'
           WHEN pr.id IS NULL                     THEN 'Proveedor inexistente'
           WHEN TRIM(IFNULL(pr.nombreComercial, '')) = '' THEN 'Proveedor sin nombre'
       END AS motivo
FROM Productos p
LEFT JOIN Rubros r       ON r.id  = p.fk_rubro
LEFT JOIN Proveedores pr ON pr.id = p.fk_proveedor
WHERE (@incluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
  AND (r.id IS NULL OR pr.id IS NULL OR TRIM(IFNULL(pr.nombreComercial, '')) = '')
ORDER BY motivo, p.id;

-- 1.c) Totales
SELECT COUNT(*)                                   AS productosTotales,
       SUM(r.id IS NOT NULL AND pr.id IS NOT NULL
           AND TRIM(IFNULL(pr.nombreComercial, '')) <> '') AS productosInvolucrados,
       SUM(NOT (r.id IS NOT NULL AND pr.id IS NOT NULL
           AND TRIM(IFNULL(pr.nombreComercial, '')) <> '')) AS productosExcluidos
FROM Productos p
LEFT JOIN Rubros r       ON r.id  = p.fk_rubro
LEFT JOIN Proveedores pr ON pr.id = p.fk_proveedor
WHERE (@incluirBajas = 1 OR IFNULL(p.baja, 0) = 0);