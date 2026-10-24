-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 02: tabla de clausura RubrosArbol
-- Guarda todos los pares ancestro-descendiente (incluido cada rubro consigo mismo, profundidad 0).
-- =====================================================================
CREATE TABLE IF NOT EXISTS `RubrosArbol` (
  `fk_ancestro`     INT NOT NULL,
  `fk_descendiente` INT NOT NULL,
  `profundidad`     INT NOT NULL,
  PRIMARY KEY (`fk_ancestro`, `fk_descendiente`),
  KEY `idx_rubrosarbol_desc` (`fk_descendiente`, `fk_ancestro`),
  CONSTRAINT `fk_ra_ancestro` FOREIGN KEY (`fk_ancestro`)     REFERENCES `Rubros` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_ra_desc`     FOREIGN KEY (`fk_descendiente`) REFERENCES `Rubros` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;