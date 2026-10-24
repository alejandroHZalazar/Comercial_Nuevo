-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 06: vista vw_RubrosJerarquia
-- nivel: 0 = rubro principal. ruta: "Rubro > Subrubro > Subrubro".
-- =====================================================================
CREATE OR REPLACE VIEW vw_RubrosJerarquia AS
SELECT
    r.id,
    r.descripcion,
    r.fk_rubroPadre,
    rp.descripcion AS rubroPadre,
    (SELECT MAX(a.profundidad) FROM RubrosArbol a WHERE a.fk_descendiente = r.id) AS nivel,
    (SELECT GROUP_CONCAT(ra.descripcion ORDER BY a.profundidad DESC SEPARATOR ' > ')
       FROM RubrosArbol a
       INNER JOIN Rubros ra ON ra.id = a.fk_ancestro
      WHERE a.fk_descendiente = r.id) AS ruta,
    IF(r.fk_rubroPadre IS NULL, 1, 0) AS esRaiz,
    IF(EXISTS (SELECT 1 FROM Rubros h WHERE h.fk_rubroPadre = r.id), 0, 1) AS esHoja
FROM Rubros r
LEFT JOIN Rubros rp ON rp.id = r.fk_rubroPadre;