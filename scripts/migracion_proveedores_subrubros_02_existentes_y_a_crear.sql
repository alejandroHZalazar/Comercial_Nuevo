-- =====================================================================
-- MIGRACION PROVEEDORES -> SUBRUBROS - 02: subrubros que ya existen y los que se crearian
-- (solo SELECT, no modifica nada). Un subrubro por (rubro padre, nombre del proveedor).
-- Nombre normalizado: sin espacios/tabuladores/saltos en los extremos, maximo 150 caracteres,
-- comparado sin distinguir mayusculas. Dos proveedores con el mismo nombre comparten subrubro.
-- Se omite el caso en que el propio rubro ya es el subrubro de ese proveedor (evita anidarlo dos veces).
-- =====================================================================
SET @incluirBajas = 1;   -- debe coincidir con el valor usado en el diagnostico y en la migracion

SELECT c.fk_rubroPadre                               AS idRubroPadre,
       r.descripcion                                 AS rubroPadre,
       c.nombre                                      AS subrubro,
       c.productos,
       c.proveedores,
       ex.id                                         AS idSubrubroExistente,
       IFNULL(ex.repetidos, 0)                       AS subrubrosRepetidosExistentes,
       IF(ex.id IS NULL, 'CREAR', 'EXISTE - SE REUTILIZA') AS accion
FROM (
    SELECT s.fk_rubro AS fk_rubroPadre, s.nombre,
           COUNT(*) AS productos, COUNT(DISTINCT s.fk_proveedor) AS proveedores
    FROM (
        SELECT p.id, p.fk_rubro, p.fk_proveedor,
               CONVERT(LEFT(TRIM(REPLACE(REPLACE(REPLACE(pr.nombreComercial,
                   CHAR(9 USING latin1), ' '), CHAR(10 USING latin1), ' '), CHAR(13 USING latin1), ' ')), 150)
                   USING latin1) COLLATE latin1_swedish_ci AS nombre,
               r2.descripcion AS rubroDescripcion, r2.fk_rubroPadre AS rubroPadreDelRubro
        FROM Productos p
        INNER JOIN Rubros r2      ON r2.id = p.fk_rubro
        INNER JOIN Proveedores pr ON pr.id = p.fk_proveedor
        WHERE (@incluirBajas = 1 OR IFNULL(p.baja, 0) = 0)
    ) s
    WHERE s.nombre IS NOT NULL AND s.nombre <> ''
      AND NOT (s.rubroPadreDelRubro IS NOT NULL
               AND TRIM(s.rubroDescripcion) COLLATE latin1_swedish_ci = s.nombre)
    GROUP BY s.fk_rubro, s.nombre
) c
INNER JOIN Rubros r ON r.id = c.fk_rubroPadre
LEFT JOIN (
    SELECT x.fk_rubroPadre, TRIM(x.descripcion) COLLATE latin1_swedish_ci AS nombre,
           MIN(x.id) AS id, COUNT(*) AS repetidos
    FROM Rubros x
    WHERE x.fk_rubroPadre IS NOT NULL
    GROUP BY x.fk_rubroPadre, TRIM(x.descripcion) COLLATE latin1_swedish_ci
) ex ON ex.fk_rubroPadre = c.fk_rubroPadre AND ex.nombre = c.nombre
ORDER BY r.descripcion, c.nombre;