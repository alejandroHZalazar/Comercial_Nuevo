DELIMITER $$

-- -------------------------------------------------------
-- sp_VentasAddPromoComponentes
-- Recibe la linea de ventasDetalle de la promo y el detalle
-- de componentes seleccionados en formato:
--   idSlot#idProducto*cantidad;idSlot2#idProducto2*cantidad2;...
-- Inserta en ventas_promo_componentes y descuenta stock.
-- -------------------------------------------------------
DROP PROCEDURE IF EXISTS sp_VentasAddPromoComponentes$$
CREATE PROCEDURE sp_VentasAddPromoComponentes(
    IN unFkVentaDetalle BIGINT,
    IN unDetalle        TEXT,
    OUT salida          INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET salida = -1;
        ROLLBACK;
    END;

    DECLARE itemEntry TEXT DEFAULT '';
    DECLARE idSlot    INT DEFAULT 0;
    DECLARE idProd    INT DEFAULT 0;
    DECLARE cant      DECIMAL(18,4) DEFAULT 0;
    DECLARE posItem   INT DEFAULT 0;
    DECLARE partB     TEXT DEFAULT '';
    DECLARE partC     TEXT DEFAULT '';

    START TRANSACTION;

    SET @det = unDetalle;
    WHILE LENGTH(@det) > 0 DO
        SET posItem = LOCATE(';', @det);
        IF posItem > 0 THEN
            SET itemEntry = SUBSTRING(@det, 1, posItem - 1);
            SET @det = SUBSTRING(@det, posItem + 1);
        ELSE
            SET itemEntry = @det;
            SET @det = '';
        END IF;

        IF LENGTH(itemEntry) > 0 THEN
            -- Formato: idSlot#idProducto*cantidad
            SET idSlot = CAST(SUBSTRING_INDEX(itemEntry, '#', 1) AS UNSIGNED);
            SET partB  = SUBSTRING(itemEntry, LOCATE('#', itemEntry) + 1);
            SET idProd = CAST(SUBSTRING_INDEX(partB, '*', 1) AS UNSIGNED);
            SET cant   = CAST(SUBSTRING(partB, LOCATE('*', partB) + 1) AS DECIMAL(18,4));

            IF idSlot > 0 AND idProd > 0 AND cant > 0 THEN
                INSERT INTO ventas_promo_componentes (fk_ventaDetalle, fk_slot, fk_producto, cantidad)
                VALUES (unFkVentaDetalle, idSlot, idProd, cant);

                UPDATE stockProductos SET cantidad = cantidad - cant WHERE fk_producto = idProd;
            END IF;
        END IF;
    END WHILE;

    SET salida = 1;
    COMMIT;
END$$

DELIMITER ;
