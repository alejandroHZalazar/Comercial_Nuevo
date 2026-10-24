-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 05: inicializacion
-- Los rubros existentes quedan como rubros principales (fk_rubroPadre = NULL).
-- Los productos NO se tocan (Productos.fk_rubro sigue igual).
-- Valida que existan la columna, la tabla RubrosArbol y el SP antes de ejecutar.
-- =====================================================================
SET @ok = (SELECT COUNT(*) FROM information_schema.ROUTINES
           WHERE ROUTINE_SCHEMA = DATABASE() AND ROUTINE_TYPE = 'PROCEDURE'
             AND ROUTINE_NAME = 'sp_RubrosReconstruirArbol')
        + (SELECT COUNT(*) FROM information_schema.TABLES
           WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubrosarbol')
        + (SELECT COUNT(*) FROM information_schema.COLUMNS
           WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
             AND COLUMN_NAME = 'fk_rubroPadre');
SET @s = IF(@ok = 3, 'CALL sp_RubrosReconstruirArbol()',
            'SELECT ''Faltan objetos: ejecutar antes los scripts 01 a 03'' AS info');
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;