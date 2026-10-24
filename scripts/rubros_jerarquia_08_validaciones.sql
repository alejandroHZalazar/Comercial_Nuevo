-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 08: validaciones posteriores (solo SELECT)
-- =====================================================================

-- 1) Una fila de autoreferencia por rubro; mientras no haya subrubros, todos son raices
SELECT (SELECT COUNT(*) FROM Rubros) AS rubros,
       (SELECT COUNT(*) FROM RubrosArbol WHERE profundidad = 0) AS autoreferencias,
       (SELECT COUNT(*) FROM Rubros WHERE fk_rubroPadre IS NULL) AS raices;

-- 2) Rubros que no alcanzan una raiz (ciclos o huerfanos). Debe dar 0 filas
SELECT r.id, r.descripcion
FROM Rubros r
WHERE NOT EXISTS (SELECT 1 FROM RubrosArbol a
                  INNER JOIN Rubros ra ON ra.id = a.fk_ancestro AND ra.fk_rubroPadre IS NULL
                  WHERE a.fk_descendiente = r.id);

-- 3) Productos con un rubro que ya no existe. Debe dar 0 filas
SELECT p.id, p.descripcion, p.fk_rubro
FROM Productos p LEFT JOIN Rubros r ON r.id = p.fk_rubro
WHERE r.id IS NULL;

-- 4) Productos por rubro (comparar con el conteo previo a la migracion)
SELECT fk_rubro, COUNT(*) AS productos FROM Productos GROUP BY fk_rubro ORDER BY fk_rubro;

-- 5) Vista de jerarquia
SELECT id, descripcion, fk_rubroPadre, rubroPadre, nivel, ruta, esRaiz, esHoja
FROM vw_RubrosJerarquia ORDER BY ruta;