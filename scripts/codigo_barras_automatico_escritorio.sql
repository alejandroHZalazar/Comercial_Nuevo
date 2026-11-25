-- ============================================================================
-- CODIGOS DE BARRAS Y DE PROVEEDOR AUTOMATICOS (escritorio)
-- ----------------------------------------------------------------------------
-- Misma logica que Comercial Web: con parametros(productos, generaCodigoBarras) = '1', un producto que se da de
-- alta SIN codigo de barras y SIN codigo de proveedor recibe el mismo valor en ambos (001 ... 999, 1000 ...).
-- El proximo numero vive en parametros(productos, proximoCodigoBarras) y es el MISMO contador que usa la web.
--
-- QUE HACE ESTE SCRIPT
--   1) Agrega las dos filas de parametros si faltan (queda APAGADO; no cambia las que ya existen).
--   2) Crea sp_CodigoBarras_Reservar y sp_ProductosAltaCodigoAutomatico.
--   NO modifica sp_ProductosABM, ni tablas, ni productos existentes. Se puede ejecutar mas de una vez.
--
-- PARA VARIANTES: ejecutar tambien variantes_escritorio_01_sp_guardar.sql (version actualizada), solo en las bases
--   con variantes (requiere MySQL 8).
--
-- Ejecutar sobre cada base donde se quiera usar. Hacer backup antes.
-- Para ACTIVAR:
--   UPDATE parametros SET valor = '1' WHERE modulo = 'productos' AND parametro = 'generaCodigoBarras';
-- ============================================================================

-- 1) Interruptor (apagado por defecto). El id se calcula porque en varias bases parametros.id no es AUTO_INCREMENT.
INSERT INTO parametros (id, modulo, parametro, valor)
SELECT t.nuevo, 'productos', 'generaCodigoBarras', '0'
FROM (SELECT IFNULL(MAX(id), 0) + 1 AS nuevo,
             IFNULL(SUM(modulo = 'productos' AND parametro = 'generaCodigoBarras'), 0) AS existe
      FROM parametros) t
WHERE t.existe = 0;

-- 2) Proximo numero a probar (contador). Su fila se bloquea durante cada alta para evitar duplicados.
INSERT INTO parametros (id, modulo, parametro, valor)
SELECT t.nuevo, 'productos', 'proximoCodigoBarras', '1'
FROM (SELECT IFNULL(MAX(id), 0) + 1 AS nuevo,
             IFNULL(SUM(modulo = 'productos' AND parametro = 'proximoCodigoBarras'), 0) AS existe
      FROM parametros) t
WHERE t.existe = 0;

-- ----------------------------------------------------------------------------
-- 3) Reserva UN codigo libre. NO abre ni confirma transacciones: debe llamarse DENTRO de la transaccion del alta,
--    para que el bloqueo del contador dure hasta el COMMIT y el contador vuelva atras si el alta falla.
--    El codigo devuelto debe grabarse en Productos antes de pedir otro.
--    Libre = no existe como codBarras NI como codProveedor en ningun producto (incluidos los dados de baja).
-- ----------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS sp_CodigoBarras_Reservar;
DELIMITER $$
CREATE PROCEDURE sp_CodigoBarras_Reservar(
    OUT unCodigo VARCHAR(45)
)
BEGIN
    DECLARE vFilas    INT DEFAULT 0;
    DECLARE vValor    VARCHAR(45);
    DECLARE vNumero   BIGINT DEFAULT 1;
    DECLARE vIntentos INT DEFAULT 0;
    DECLARE vLibre    TINYINT DEFAULT 0;

    SELECT COUNT(*), MAX(COALESCE(valor, '1')) INTO vFilas, vValor
      FROM parametros
     WHERE modulo = 'productos' AND parametro = 'proximoCodigoBarras'
       FOR UPDATE;

    IF vFilas = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Falta el parametro productos/proximoCodigoBarras. Ejecute codigo_barras_automatico_escritorio.sql';
    END IF;

    IF TRIM(vValor) REGEXP '^[0-9]{1,18}$' THEN
        SET vNumero = GREATEST(CAST(TRIM(vValor) AS UNSIGNED), 1);
    END IF;

    WHILE vLibre = 0 DO
        SET unCodigo = IF(vNumero < 1000, LPAD(vNumero, 3, '0'), CAST(vNumero AS CHAR));

        IF NOT EXISTS (SELECT 1 FROM Productos WHERE codBarras = unCodigo)
           AND NOT EXISTS (SELECT 1 FROM Productos WHERE codProveedor = unCodigo) THEN
            SET vLibre = 1;
        END IF;

        SET vNumero = vNumero + 1;
        SET vIntentos = vIntentos + 1;

        IF vIntentos > 1000000 THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No se encontraron codigos de barras libres.';
        END IF;
    END WHILE;

    UPDATE parametros SET valor = CAST(vNumero AS CHAR)
     WHERE modulo = 'productos' AND parametro = 'proximoCodigoBarras';
END$$
DELIMITER ;

-- ----------------------------------------------------------------------------
-- 4) Alta de un producto simple con codigo automatico, todo en una transaccion.
--    Mismas inserciones que sp_ProductosABM (accion 1): si cambia el alta alli, actualizar tambien aqui.
--    La aplicacion lo llama solo si el parametro esta activo y ambos codigos vienen vacios.
--    salida: > 0 id del producto | -1 error (ver mensaje)
-- ----------------------------------------------------------------------------
DROP PROCEDURE IF EXISTS sp_ProductosAltaCodigoAutomatico;
DELIMITER $$
CREATE PROCEDURE sp_ProductosAltaCodigoAutomatico(
    IN  unRubro                 INT,
    IN  unaDescripcion          VARCHAR(150),
    IN  unProveedor             INT,
    IN  unCosto                 DECIMAL(18,4),
    IN  unPrecio                DECIMAL(18,4),
    IN  unStock                 DECIMAL(18,4),
    IN  unaCMinima              DECIMAL(18,4),
    IN  unPrecioProveedor       DECIMAL(18,4),
    IN  esFraccionado           TINYINT(1),
    IN  esDolarizado            TINYINT(1),
    IN  esUnaPromocion          TINYINT(1),
    IN  unGanancia              DECIMAL(18,2),
    IN  unDescuento             DECIMAL(18,2),
    IN  unaCantidadMinimaVenta  DECIMAL(18,2),
    OUT salida                  INT,
    OUT unCodigo                VARCHAR(45),
    OUT mensaje                 VARCHAR(500)
)
BEGIN
    DECLARE prod INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 mensaje = MESSAGE_TEXT;
        ROLLBACK;
        SET salida = -1;
        SET unCodigo = NULL;
    END;

    SET mensaje = '';

    START TRANSACTION;

    -- Primera lectura de la transaccion: bloquea el contador
    CALL sp_CodigoBarras_Reservar(unCodigo);

    INSERT INTO Productos
        (codProveedor, codBarras, fk_Rubro, iva, descripcion, fk_proveedor, baja,
         fraccionado, dolarizado, esPromocion, ganancia, descuento, cantidadMinimaVenta)
    VALUES
        (unCodigo, unCodigo, unRubro,
         (SELECT valor FROM parametros WHERE modulo = 'productos' AND parametro = 'sinIVA'),
         unaDescripcion, unProveedor, 0,
         esFraccionado, esDolarizado, esUnaPromocion, unGanancia, unDescuento,
         COALESCE(unaCantidadMinimaVenta, 1));

    SET prod = LAST_INSERT_ID();

    INSERT INTO costosProductos (fk_producto, costo) VALUES (prod, unCosto);
    INSERT INTO preciosProductos (fk_producto, precio) VALUES (prod, unPrecio);
    INSERT INTO stockProductos (fk_producto, cantidad, cantidadMinima) VALUES (prod, unStock, unaCMinima);
    INSERT INTO preciosProveedores (fk_producto, precio) VALUES (prod, unPrecioProveedor);

    COMMIT;

    SET salida = prod;
END$$
DELIMITER ;

-- ----------------------------------------------------------------------------
-- Verificacion (solo lectura). parametros y Productos deben ser InnoDB para que el bloqueo y el rollback funcionen.
-- ----------------------------------------------------------------------------
SELECT TABLE_NAME, ENGINE FROM information_schema.TABLES
 WHERE TABLE_SCHEMA = DATABASE()
   AND LOWER(TABLE_NAME) IN ('parametros', 'productos', 'costosproductos', 'preciosproductos', 'stockproductos', 'preciosproveedores');

SELECT modulo, parametro, valor FROM parametros
 WHERE modulo = 'productos' AND parametro IN ('generaCodigoBarras', 'proximoCodigoBarras');
