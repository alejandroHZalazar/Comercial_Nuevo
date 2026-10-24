-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 09: ROLLBACK
-- !!! NO EJECUTAR salvo que se quiera DESHACER la jerarquia de rubros !!!
-- Si ya existen subrubros (fk_rubroPadre con valores) NO elimina la columna ni la clave foranea,
-- para no perder la jerarquia: hay que reubicar esos rubros como principales antes.
-- Valida la existencia de cada objeto. No modifica Productos.
-- =====================================================================
DROP TRIGGER IF EXISTS trg_Rubros_bu;
DROP TRIGGER IF EXISTS trg_Rubros_ai;
DROP VIEW IF EXISTS vw_RubrosJerarquia;
DROP PROCEDURE IF EXISTS sp_RubrosDescendientes;
DROP PROCEDURE IF EXISTS sp_RubrosAntecesores;
DROP PROCEDURE IF EXISTS sp_RubrosABM;
DROP PROCEDURE IF EXISTS sp_RubrosReconstruirArbol;
DROP TABLE IF EXISTS RubrosArbol;

-- Cantidad de subrubros (solo si la columna existe)
SET @tieneColumna = (SELECT COUNT(*) FROM information_schema.COLUMNS
                     WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
                       AND COLUMN_NAME = 'fk_rubroPadre');
SET @subrubros = 0;
SET @s = IF(@tieneColumna = 1,
            'SELECT COUNT(*) INTO @subrubros FROM Rubros WHERE fk_rubroPadre IS NOT NULL',
            'SELECT 0 INTO @subrubros');
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Clave foranea
SET @s = IF(@subrubros > 0, 'SELECT ''Hay subrubros: no se elimina la jerarquia (reubicarlos antes)'' AS info',
        IF((SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS
            WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
              AND CONSTRAINT_NAME = 'fk_rubros_padre' AND CONSTRAINT_TYPE = 'FOREIGN KEY') = 1,
           'ALTER TABLE `Rubros` DROP FOREIGN KEY `fk_rubros_padre`',
           'SELECT ''clave foranea fk_rubros_padre no existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Indice
SET @s = IF(@subrubros > 0, 'SELECT ''Hay subrubros: no se elimina el indice'' AS info',
        IF((SELECT COUNT(*) FROM information_schema.STATISTICS
            WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
              AND INDEX_NAME = 'idx_rubros_padre') > 0,
           'ALTER TABLE `Rubros` DROP INDEX `idx_rubros_padre`',
           'SELECT ''indice idx_rubros_padre no existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Columna
SET @s = IF(@subrubros > 0, 'SELECT ''Hay subrubros: no se elimina la columna fk_rubroPadre'' AS info',
        IF(@tieneColumna = 1,
           'ALTER TABLE `Rubros` DROP COLUMN `fk_rubroPadre`',
           'SELECT ''columna fk_rubroPadre no existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;