-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 01: columna padre, indice y clave foranea
-- Sin nombre de base: se ejecuta sobre la base seleccionada (DATABASE()).
-- Re-ejecutable: valida la existencia de la tabla y de cada objeto antes de crearlo.
-- =====================================================================

SET @tieneRubros = (SELECT COUNT(*) FROM information_schema.TABLES
                    WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros');

-- Columna fk_rubroPadre (NULL = rubro principal)
SET @s = IF(@tieneRubros = 0, 'SELECT ''No existe la tabla Rubros en esta base'' AS info',
        IF((SELECT COUNT(*) FROM information_schema.COLUMNS
            WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
              AND COLUMN_NAME = 'fk_rubroPadre') = 0,
           'ALTER TABLE `Rubros` ADD COLUMN `fk_rubroPadre` INT NULL DEFAULT NULL AFTER `descripcion`',
           'SELECT ''columna fk_rubroPadre ya existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Indice
SET @s = IF(@tieneRubros = 0, 'SELECT ''No existe la tabla Rubros en esta base'' AS info',
        IF((SELECT COUNT(*) FROM information_schema.STATISTICS
            WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
              AND INDEX_NAME = 'idx_rubros_padre') = 0,
           'ALTER TABLE `Rubros` ADD INDEX `idx_rubros_padre` (`fk_rubroPadre`)',
           'SELECT ''indice idx_rubros_padre ya existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Clave foranea (RESTRICT: no se puede borrar un rubro que tiene subrubros)
SET @s = IF(@tieneRubros = 0, 'SELECT ''No existe la tabla Rubros en esta base'' AS info',
        IF((SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS
            WHERE TABLE_SCHEMA = DATABASE() AND LOWER(TABLE_NAME) = 'rubros'
              AND CONSTRAINT_NAME = 'fk_rubros_padre' AND CONSTRAINT_TYPE = 'FOREIGN KEY') = 0,
           'ALTER TABLE `Rubros` ADD CONSTRAINT `fk_rubros_padre` FOREIGN KEY (`fk_rubroPadre`) REFERENCES `Rubros` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT',
           'SELECT ''clave foranea fk_rubros_padre ya existe'' AS info'));
PREPARE st FROM @s; EXECUTE st; DEALLOCATE PREPARE st;

-- Nota: no se usa CHECK (MySQL no permite CHECK sobre columnas AUTO_INCREMENT, error 3818).
-- La regla "un rubro no puede ser su propio padre" la cubren sp_RubrosABM y el trigger trg_Rubros_bu (script 04).