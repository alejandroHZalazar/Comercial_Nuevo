-- =====================================================================
-- MIGRACION PROVEEDORES -> SUBRUBROS - 04: VERIFICACION POSTERIOR (solo SELECT)
-- Comprueba que cada combinacion (rubro padre, proveedor) tenga exactamente un subrubro.
-- =====================================================================
SET @incluirBajas = 1;   -- mismo valor que en la migracion

-- Combinaciones (rubro padre, nombre de proveedor) con el subrubro que les corresponde
DROP TEMPORARY TABLE IF EXISTS tmp_verif;
CREATE TEMPORARY TABLE tmp_verif AS
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
        WHERE (@incluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
    ) s
    WHERE s.nombre IS NOT NULL AND s.nombre <> ''
      AND NOT (s.rubroPadreDelRubro IS NOT NULL
               AND TRIM(s.rubroDescripcion) COLLATE latin1_swedish_ci = s.nombre)
    GROUP BY s.fk_rubro, s.nombre;

-- 4.a) Resumen: todas las combinaciones deben tener exactamente 1 subrubro
SELECT COUNT(*)              AS combinaciones,
       SUM(t.cantidad = 1)   AS conUnSubrubro,
       SUM(t.cantidad = 0)   AS sinSubrubro,
       SUM(t.cantidad > 1)   AS conSubrubroRepetido
FROM (
    SELECT (SELECT COUNT(*) FROM Rubros x
            WHERE x.fk_rubroPadre = c.fk_rubroPadre
              AND TRIM(x.descripcion) COLLATE latin1_swedish_ci = c.nombre) AS cantidad
    FROM tmp_verif c
) t;
-- 4.b) Detalle de las combinaciones con problemas (debe dar 0 filas)
SELECT c.fk_rubroPadre, c.nombre AS proveedor,
       (SELECT COUNT(*) FROM Rubros x
        WHERE x.fk_rubroPadre = c.fk_rubroPadre
          AND TRIM(x.descripcion) COLLATE latin1_swedish_ci = c.nombre) AS subrubrosEncontrados
FROM tmp_verif c
HAVING subrubrosEncontrados <> 1;

-- 4.c) Subrubros repetidos bajo el mismo padre en TODA la tabla (debe dar 0 filas)
SELECT fk_rubroPadre, TRIM(descripcion) AS descripcion, COUNT(*) AS repetidos, GROUP_CONCAT(id) AS ids
FROM Rubros
WHERE fk_rubroPadre IS NOT NULL
GROUP BY fk_rubroPadre, TRIM(descripcion) COLLATE latin1_swedish_ci
HAVING COUNT(*) > 1;

-- 4.d) Cada subrubro de la migracion existe en RubrosArbol bajo su padre (debe dar 0 filas)
SELECT r.id, r.descripcion, r.fk_rubroPadre
FROM Rubros r
WHERE r.fk_rubroPadre IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM RubrosArbol a
                  WHERE a.fk_ancestro = r.fk_rubroPadre AND a.fk_descendiente = r.id AND a.profundidad = 1);

-- 4.e) Detalle final: rubro padre > subrubro (proveedor), productos del proveedor en ese rubro
SELECT rp.descripcion AS rubroPadre, v.nombre AS subrubroProveedor, s.id AS idSubrubro,
       v.productos, v.proveedores
FROM tmp_verif v
INNER JOIN Rubros rp ON rp.id = v.fk_rubroPadre
LEFT JOIN Rubros s ON s.fk_rubroPadre = v.fk_rubroPadre
                  AND TRIM(s.descripcion) COLLATE latin1_swedish_ci = v.nombre
ORDER BY rp.descripcion, v.nombre;

-- 4.f) Control de Productos: estos totales deben ser IGUALES a los previos a la migracion
SELECT COUNT(*) AS productos, SUM(fk_rubro) AS sumaRubro, SUM(fk_proveedor) AS sumaProveedor
FROM Productos;

DROP TEMPORARY TABLE IF EXISTS tmp_verif;