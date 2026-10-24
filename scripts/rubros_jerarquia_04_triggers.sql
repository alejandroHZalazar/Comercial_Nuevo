-- =====================================================================
-- RUBROS JERARQUICOS (nivel N) - 04: triggers
-- trg_Rubros_ai: mantiene RubrosArbol al insertar un rubro (aunque lo cree el SP viejo
--                sp_ConfiguracionABMrubros desde un .exe anterior).
-- trg_Rubros_bu: impide que un rubro sea su propio padre.
-- Requiere permiso TRIGGER para el usuario.
-- =====================================================================
DROP TRIGGER IF EXISTS trg_Rubros_ai;
DROP TRIGGER IF EXISTS trg_Rubros_bu;
DELIMITER $$
CREATE TRIGGER trg_Rubros_ai AFTER INSERT ON Rubros FOR EACH ROW
BEGIN
    INSERT INTO RubrosArbol (fk_ancestro, fk_descendiente, profundidad)
    VALUES (NEW.id, NEW.id, 0);

    IF NEW.fk_rubroPadre IS NOT NULL THEN
        INSERT INTO RubrosArbol (fk_ancestro, fk_descendiente, profundidad)
        SELECT fk_ancestro, NEW.id, profundidad + 1
        FROM RubrosArbol
        WHERE fk_descendiente = NEW.fk_rubroPadre;
    END IF;
END$$

CREATE TRIGGER trg_Rubros_bu BEFORE UPDATE ON Rubros FOR EACH ROW
BEGIN
    IF NEW.fk_rubroPadre IS NOT NULL AND NEW.fk_rubroPadre = NEW.id THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Un rubro no puede ser su propio padre';
    END IF;
END$$
DELIMITER ;