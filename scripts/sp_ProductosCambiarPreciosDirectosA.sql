DROP PROCEDURE IF EXISTS sp_ProductosCambiarPreciosDirectos;
DELIMITER $$

CREATE PROCEDURE sp_ProductosCambiarPreciosDirectos(
    IN unCodProveedor  VARCHAR(45),
    IN unPrecioProv    DECIMAL(18,4),
    IN unPrecioSIVA    DECIMAL(18,4),
    IN unCosto         DECIMAL(18,4)
)
BEGIN
    DECLARE idProducto INT DEFAULT NULL;

    -- Obtener el id del producto por código de proveedor
    SELECT id INTO idProducto
    FROM Productos
    WHERE codProveedor = unCodProveedor
    LIMIT 1;

    IF idProducto IS NOT NULL THEN

        -- Actualizar precio de proveedor solo si viene con valor > 0
        IF unPrecioProv > 0 THEN
            UPDATE preciosProveedores
            SET    precio = unPrecioProv
            WHERE  fk_producto = idProducto;
        END IF;

        -- Actualizar precio de lista (sin IVA) solo si viene con valor > 0
        IF unPrecioSIVA > 0 THEN
            UPDATE preciosProductos
            SET    precio = unPrecioSIVA
            WHERE  fk_producto = idProducto;
        END IF;

        -- Actualizar costo solo si viene con valor > 0
        IF unCosto > 0 THEN
            UPDATE costosProductos
            SET    costo = unCosto
            WHERE  fk_producto = idProducto;
        END IF;

    END IF;
END$$

DELIMITER ;
