-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 03: sp_RubrosReconstruirArbol
-- Reconstruye RubrosArbol a partir de Rubros.fk_rubroPadre (bucle, no requiere MySQL 8).
-- Compatible con "safe update mode" de MySQL Workbench (el DELETE usa una columna clave y LIMIT).
-- =====================================================================
DROP PROCEDURE IF EXISTS sp_RubrosReconstruirArbol;
DELIMITER $$
CREATE PROCEDURE sp_RubrosReconstruirArbol()
BEGIN
    DECLARE filas INT DEFAULT 1;
    DECLARE nivel INT DEFAULT 0;

    DELETE FROM RubrosArbol WHERE fk_ancestro >= 0 LIMIT 2000000000;
    INSERT INTO RubrosArbol (fk_ancestro, fk_descendiente, profundidad)
        SELECT id, id, 0 FROM Rubros;

    WHILE filas > 0 AND nivel < 50 DO
        SET nivel = nivel + 1;
        INSERT INTO RubrosArbol (fk_ancestro, fk_descendiente, profundidad)
            SELECT a.fk_ancestro, r.id, nivel
            FROM RubrosArbol a
            INNER JOIN Rubros r ON r.fk_rubroPadre = a.fk_descendiente
            WHERE a.profundidad = nivel - 1;
        SET filas = ROW_COUNT();
    END WHILE;
END$$
DELIMITER ;