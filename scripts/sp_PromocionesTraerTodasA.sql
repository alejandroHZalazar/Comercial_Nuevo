DROP PROCEDURE IF EXISTS sp_PromocionesTraerTodas;
DELIMITER $$

CREATE PROCEDURE sp_PromocionesTraerTodas()
BEGIN
    -- Result set 1: cabeceras de promos activas y vigentes
    SELECT
        p.id          AS idPromocion,
        p.fk_producto,
        pr.descripcion,
        pr.precio,
        pr.precioSinIva,
        pr.iva,
        pr.dolarizado,
        pr.esFraccionado
    FROM promociones p
    JOIN productos pr ON pr.id = p.fk_producto
    WHERE p.activa = 1
      AND (p.fechaDesde IS NULL OR p.fechaDesde <= CURDATE())
      AND (p.fechaHasta IS NULL OR p.fechaHasta >= CURDATE());

    -- Result set 2: slots de esas promos
    SELECT
        ps.id          AS idSlot,
        ps.fk_promocion,
        ps.cantidadRequerida,
        ps.descripcion
    FROM promociones_slots ps
    JOIN promociones p ON p.id = ps.fk_promocion
    WHERE p.activa = 1
      AND (p.fechaDesde IS NULL OR p.fechaDesde <= CURDATE())
      AND (p.fechaHasta IS NULL OR p.fechaHasta >= CURDATE());

    -- Result set 3: productos elegibles por slot (con nombre)
    SELECT
        psp.fk_slot,
        psp.fk_producto,
        pr.descripcion AS productoDesc
    FROM promociones_slot_productos psp
    JOIN productos pr ON pr.id = psp.fk_producto
    JOIN promociones_slots ps ON ps.id = psp.fk_slot
    JOIN promociones p ON p.id = ps.fk_promocion
    WHERE p.activa = 1
      AND (p.fechaDesde IS NULL OR p.fechaDesde <= CURDATE())
      AND (p.fechaHasta IS NULL OR p.fechaHasta >= CURDATE());
END$$

DELIMITER ;
