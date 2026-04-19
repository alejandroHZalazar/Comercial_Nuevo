DELIMITER $$

-- -------------------------------------------------------
-- sp_PromocionesConsulta: lista promociones con datos del producto
-- -------------------------------------------------------
DROP PROCEDURE IF EXISTS sp_PromocionesConsulta$$
CREATE PROCEDURE sp_PromocionesConsulta(IN unFiltro VARCHAR(500))
BEGIN
    SET @sql = CONCAT(
        'SELECT pr.id, pr.fk_producto, p.descripcion AS producto, p.codBarras, p.precio, ',
        'pr.activa, pr.fechaDesde, pr.fechaHasta ',
        'FROM promociones pr ',
        'INNER JOIN productos p ON p.id = pr.fk_producto ',
        'WHERE 1=1 ', unFiltro,
        ' ORDER BY p.descripcion'
    );
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
END$$

-- -------------------------------------------------------
-- sp_PromocionesABM: alta/baja/modificacion de promo
--   accion 1=insert, 2=update, 3=delete logico (desactiva)
-- -------------------------------------------------------
DROP PROCEDURE IF EXISTS sp_PromocionesABM$$
CREATE PROCEDURE sp_PromocionesABM(
    IN unaAccion      INT,
    IN unIdPromocion  INT,
    IN unFkProducto   INT,
    IN unActiva       TINYINT,
    IN unaFechaDesde  DATE,
    IN unaFechaHasta  DATE,
    OUT salida        INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET salida = -1;
        ROLLBACK;
    END;

    START TRANSACTION;

    IF unaAccion = 1 THEN
        INSERT INTO promociones (fk_producto, activa, fechaDesde, fechaHasta)
        VALUES (unFkProducto, unActiva, unaFechaDesde, unaFechaHasta);
        SET salida = LAST_INSERT_ID();

        UPDATE productos SET esPromocion = 1 WHERE id = unFkProducto;

    ELSEIF unaAccion = 2 THEN
        UPDATE promociones
        SET activa = unActiva, fechaDesde = unaFechaDesde, fechaHasta = unaFechaHasta
        WHERE id = unIdPromocion;
        SET salida = unIdPromocion;

    ELSEIF unaAccion = 3 THEN
        UPDATE promociones SET activa = 0 WHERE id = unIdPromocion;
        UPDATE productos SET esPromocion = 0
        WHERE id = (SELECT fk_producto FROM promociones WHERE id = unIdPromocion);
        SET salida = unIdPromocion;
    END IF;

    COMMIT;
END$$

-- -------------------------------------------------------
-- sp_PromocionesTraerCompleta: devuelve slots + productos
--   elegibles de la promo de un producto dado
-- -------------------------------------------------------
DROP PROCEDURE IF EXISTS sp_PromocionesTraerCompleta$$
CREATE PROCEDURE sp_PromocionesTraerCompleta(IN unFkProducto INT)
BEGIN
    -- Cabecera de la promo
    SELECT pr.id AS idPromocion, pr.fk_producto, p.descripcion AS producto,
           pr.activa, pr.fechaDesde, pr.fechaHasta
    FROM promociones pr
    INNER JOIN productos p ON p.id = pr.fk_producto
    WHERE pr.fk_producto = unFkProducto
    LIMIT 1;

    -- Slots
    SELECT s.id AS idSlot, s.numero, s.cantidadRequerida, s.descripcion
    FROM promociones_slots s
    INNER JOIN promociones pr ON pr.id = s.fk_promocion
    WHERE pr.fk_producto = unFkProducto
    ORDER BY s.numero;

    -- Productos elegibles por slot
    SELECT sp.id, sp.fk_slot, sp.fk_producto,
           p.descripcion AS productoDesc, p.codBarras
    FROM promociones_slot_productos sp
    INNER JOIN productos p ON p.id = sp.fk_producto
    INNER JOIN promociones_slots s ON s.id = sp.fk_slot
    INNER JOIN promociones pr ON pr.id = s.fk_promocion
    WHERE pr.fk_producto = unFkProducto
    ORDER BY s.numero, p.descripcion;
END$$

-- -------------------------------------------------------
-- sp_SlotsABM: recrea todos los slots de una promo
--   Recibe idPromocion y borra+inserta slots y sus productos
--   El detalle de slots viene como string:
--     slotNum|cantReq|descripcion|prod1,prod2,...;slotNum2|...
-- -------------------------------------------------------
DROP PROCEDURE IF EXISTS sp_SlotsABM$$
CREATE PROCEDURE sp_SlotsABM(
    IN unIdPromocion INT,
    IN unDetalleSlots TEXT,
    OUT salida INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET salida = -1;
        ROLLBACK;
    END;

    DECLARE slotEntry TEXT DEFAULT '';
    DECLARE slotNum INT DEFAULT 0;
    DECLARE cantReq DECIMAL(18,4) DEFAULT 0;
    DECLARE slotDesc VARCHAR(100) DEFAULT '';
    DECLARE productosStr TEXT DEFAULT '';
    DECLARE prodId INT DEFAULT 0;
    DECLARE nuevoSlotId INT DEFAULT 0;
    DECLARE posSlot INT DEFAULT 0;
    DECLARE posProd INT DEFAULT 0;
    DECLARE slotPart TEXT DEFAULT '';
    DECLARE prodPart TEXT DEFAULT '';

    START TRANSACTION;

    -- Borrar slots existentes (cascade sobre slot_productos)
    DELETE FROM promociones_slot_productos
    WHERE fk_slot IN (SELECT id FROM promociones_slots WHERE fk_promocion = unIdPromocion);
    DELETE FROM promociones_slots WHERE fk_promocion = unIdPromocion;

    -- Parsear string de slots (separados por ';')
    SET @slotsStr = unDetalleSlots;
    WHILE LENGTH(@slotsStr) > 0 DO
        -- Extraer un slot entry
        SET posSlot = LOCATE(';', @slotsStr);
        IF posSlot > 0 THEN
            SET slotEntry = SUBSTRING(@slotsStr, 1, posSlot - 1);
            SET @slotsStr = SUBSTRING(@slotsStr, posSlot + 1);
        ELSE
            SET slotEntry = @slotsStr;
            SET @slotsStr = '';
        END IF;

        IF LENGTH(slotEntry) > 0 THEN
            -- Parsear campos del slot: num|cant|desc|prod1,prod2,...
            SET slotNum = CAST(SUBSTRING_INDEX(slotEntry, '|', 1) AS UNSIGNED);
            SET slotPart = SUBSTRING(slotEntry, LOCATE('|', slotEntry) + 1);
            SET cantReq = CAST(SUBSTRING_INDEX(slotPart, '|', 1) AS DECIMAL(18,4));
            SET slotPart = SUBSTRING(slotPart, LOCATE('|', slotPart) + 1);
            SET slotDesc = SUBSTRING_INDEX(slotPart, '|', 1);
            SET productosStr = SUBSTRING(slotPart, LOCATE('|', slotPart) + 1);

            -- Insertar slot
            INSERT INTO promociones_slots (fk_promocion, numero, cantidadRequerida, descripcion)
            VALUES (unIdPromocion, slotNum, cantReq, slotDesc);
            SET nuevoSlotId = LAST_INSERT_ID();

            -- Parsear productos del slot (separados por ',')
            WHILE LENGTH(productosStr) > 0 DO
                SET posProd = LOCATE(',', productosStr);
                IF posProd > 0 THEN
                    SET prodId = CAST(SUBSTRING(productosStr, 1, posProd - 1) AS UNSIGNED);
                    SET productosStr = SUBSTRING(productosStr, posProd + 1);
                ELSE
                    SET prodId = CAST(productosStr AS UNSIGNED);
                    SET productosStr = '';
                END IF;

                IF prodId > 0 THEN
                    INSERT INTO promociones_slot_productos (fk_slot, fk_producto)
                    VALUES (nuevoSlotId, prodId);
                END IF;
            END WHILE;
        END IF;
    END WHILE;

    SET salida = unIdPromocion;
    COMMIT;
END$$

DELIMITER ;
