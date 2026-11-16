-- =====================================================================
-- VARIANTES DE PRODUCTO (escritorio) - 01: sp_ProductosVariantesGuardar
-- Crea o actualiza una FICHA PRINCIPAL y sus VARIANTES en UNA sola transaccion.
-- Es el equivalente de VarianteAdminService.GuardarFichaAsync de Comercial Web.
--
-- Requisitos (ya existentes en la base): tablas variantesMedidas, variantesColores,
-- productosPrincipales y productosVariantes (variantes_01_tablas.sql de LitoralMarket), MySQL 8 (JSON_TABLE).
-- NO modifica sp_ProductosABM ni ninguna tabla. Se puede volver a ejecutar (DROP IF EXISTS).
-- Las validaciones de negocio (descripciones, codigos repetidos, precios) las hace la aplicacion;
-- este SP solo escribe, todo o nada.
--
-- unasVariantes (JSON): [{"productoId":0,"medidaId":1,"colorId":0,"habilitada":1,"codBarras":"123",
--   "codProveedor":"X-1","descripcion":"NOMBRE - M","costo":10,"precio":20,"stock":5,"orden":1}, ...]
--   productoId = 0 -> variante nueva; > 0 -> variante existente de la ficha.
--
-- salida: > 0 id de la ficha | -1 error (ver mensaje) | -8 el producto existe pero no es una ficha
--         -9 se intento cambiar medidas/colores de una ficha que ya tiene variantes
-- =====================================================================
DROP PROCEDURE IF EXISTS sp_ProductosVariantesGuardar;
DELIMITER $$
CREATE PROCEDURE sp_ProductosVariantesGuardar(
    IN  unProducto              INT,            -- 0 = ficha nueva
    IN  unCodProveedor          VARCHAR(45),
    IN  unCodBarras             VARCHAR(45),
    IN  unRubro                 INT,
    IN  unaDescripcion          VARCHAR(150),   -- nombre base de la ficha
    IN  unProveedor             INT,
    IN  unCosto                 DECIMAL(18,4),  -- costo comun (o referencia si los precios son distintos)
    IN  unPrecio                DECIMAL(18,4),  -- precio comun (o referencia)
    IN  unaCMinima              DECIMAL(18,4),
    IN  unPrecioProveedor       DECIMAL(18,4),
    IN  esFraccionado           TINYINT(1),
    IN  esDolarizado            TINYINT(1),
    IN  esUnaPromocion          TINYINT(1),
    IN  unGanancia              DECIMAL(18,2),  -- NULL = no modificar
    IN  unDescuento             DECIMAL(18,2),  -- NULL = no modificar
    IN  unaCantidadMinimaVenta  DECIMAL(18,2),
    IN  usaMed                  TINYINT(1),
    IN  usaCol                  TINYINT(1),
    IN  preciosIg               TINYINT(1),
    IN  unasVariantes           LONGTEXT,
    OUT salida                  INT,
    OUT mensaje                 VARCHAR(500)
)
proc: BEGIN
    DECLARE vFicha   INT DEFAULT 0;
    DECLARE vId      INT DEFAULT 0;
    DECLARE vRn      INT DEFAULT 1;
    DECLARE vMax     INT DEFAULT 0;
    DECLARE vSafe    INT DEFAULT @@SQL_SAFE_UPDATES;

    DECLARE cProd    INT;
    DECLARE cMed     INT;
    DECLARE cCol     INT;
    DECLARE cHab     TINYINT;
    DECLARE cCodB    VARCHAR(45);
    DECLARE cCodP    VARCHAR(45);
    DECLARE cDesc    VARCHAR(150);
    DECLARE cCosto   DECIMAL(18,4);
    DECLARE cPrecio  DECIMAL(18,4);
    DECLARE cStock   DECIMAL(18,4);
    DECLARE cOrden   INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 mensaje = MESSAGE_TEXT;
        ROLLBACK;
        SET SQL_SAFE_UPDATES = vSafe;
        SET salida = -1;
    END;

    SET mensaje = '';
    SET SQL_SAFE_UPDATES = 0;

    DROP TEMPORARY TABLE IF EXISTS tmpVariantesGuardar;
    CREATE TEMPORARY TABLE tmpVariantesGuardar (
        rn           INT AUTO_INCREMENT PRIMARY KEY,
        productoId   INT,
        medidaId     INT,
        colorId      INT,
        habilitada   TINYINT,
        codBarras    VARCHAR(45),
        codProveedor VARCHAR(45),
        descripcion  VARCHAR(150),
        costo        DECIMAL(18,4),
        precio       DECIMAL(18,4),
        stock        DECIMAL(18,4),
        orden        INT
    );

    INSERT INTO tmpVariantesGuardar (productoId, medidaId, colorId, habilitada, codBarras, codProveedor, descripcion, costo, precio, stock, orden)
    SELECT IFNULL(jt.productoId, 0), IFNULL(jt.medidaId, 0), IFNULL(jt.colorId, 0), IFNULL(jt.habilitada, 1),
           jt.codBarras, jt.codProveedor, jt.descripcion, IFNULL(jt.costo, 0), IFNULL(jt.precio, 0), IFNULL(jt.stock, 0), IFNULL(jt.orden, 0)
    FROM JSON_TABLE(unasVariantes, '$[*]' COLUMNS (
            productoId   INT           PATH '$.productoId',
            medidaId     INT           PATH '$.medidaId',
            colorId      INT           PATH '$.colorId',
            habilitada   INT           PATH '$.habilitada',
            codBarras    VARCHAR(45)   PATH '$.codBarras',
            codProveedor VARCHAR(45)   PATH '$.codProveedor',
            descripcion  VARCHAR(150)  PATH '$.descripcion',
            costo        DECIMAL(18,4) PATH '$.costo',
            precio       DECIMAL(18,4) PATH '$.precio',
            stock        DECIMAL(18,4) PATH '$.stock',
            orden        INT           PATH '$.orden'
         )) AS jt;

    SELECT COUNT(*) INTO vMax FROM tmpVariantesGuardar;
    IF vMax = 0 THEN
        SET mensaje = 'No se recibieron variantes.';
        SET salida = -1;
        SET SQL_SAFE_UPDATES = vSafe;
        LEAVE proc;
    END IF;

    START TRANSACTION;

    -- ---------------------------------------------------------------
    -- 1) Ficha principal
    -- ---------------------------------------------------------------
    IF unProducto = 0 THEN
        INSERT INTO Productos
            (codProveedor, codBarras, fk_Rubro, iva, descripcion, fk_proveedor, baja,
             fraccionado, dolarizado, esPromocion, ganancia, descuento, cantidadMinimaVenta)
        VALUES
            (unCodProveedor, unCodBarras, unRubro,
             (SELECT valor FROM parametros WHERE modulo = 'productos' AND parametro = 'sinIVA'),
             unaDescripcion, unProveedor, 0,
             esFraccionado, esDolarizado, esUnaPromocion, unGanancia, unDescuento,
             COALESCE(unaCantidadMinimaVenta, 1));

        SET vFicha = LAST_INSERT_ID();

        INSERT INTO costosProductos (fk_producto, costo) VALUES (vFicha, unCosto);
        INSERT INTO preciosProductos (fk_producto, precio) VALUES (vFicha, unPrecio);
        INSERT INTO stockProductos (fk_producto, cantidad, cantidadMinima) VALUES (vFicha, 0, unaCMinima);   -- la ficha nunca tiene stock propio
        INSERT INTO preciosProveedores (fk_producto, precio) VALUES (vFicha, unPrecioProveedor);

        INSERT INTO productosPrincipales (fk_producto, usaMedidas, usaColores, preciosIguales)
        VALUES (vFicha, usaMed, usaCol, preciosIg);
    ELSE
        SET vFicha = unProducto;

        IF NOT EXISTS (SELECT 1 FROM productosPrincipales WHERE fk_producto = vFicha) THEN
            ROLLBACK;
            SET SQL_SAFE_UPDATES = vSafe;
            SET salida = -8;
            SET mensaje = 'El producto existe como producto simple: para usar variantes cree un producto nuevo.';
            LEAVE proc;
        END IF;

        IF EXISTS (SELECT 1 FROM productosVariantes WHERE fk_principal = vFicha)
           AND EXISTS (SELECT 1 FROM productosPrincipales
                       WHERE fk_producto = vFicha AND (usaMedidas <> usaMed OR usaColores <> usaCol)) THEN
            ROLLBACK;
            SET SQL_SAFE_UPDATES = vSafe;
            SET salida = -9;
            SET mensaje = 'No se pueden cambiar las dimensiones (medidas/colores) de un producto que ya tiene variantes.';
            LEAVE proc;
        END IF;

        UPDATE Productos
           SET codProveedor = unCodProveedor,
               codBarras = unCodBarras,
               fk_Rubro = unRubro,
               descripcion = unaDescripcion,
               fk_proveedor = unProveedor,
               fraccionado = esFraccionado,
               dolarizado = esDolarizado,
               esPromocion = esUnaPromocion,
               ganancia = COALESCE(unGanancia, ganancia),
               descuento = COALESCE(unDescuento, descuento),
               cantidadMinimaVenta = COALESCE(unaCantidadMinimaVenta, 1)
         WHERE id = vFicha;

        UPDATE costosProductos SET costo = unCosto WHERE fk_producto = vFicha;
        UPDATE preciosProductos SET precio = unPrecio WHERE fk_producto = vFicha;
        UPDATE stockProductos SET cantidadMinima = unaCMinima WHERE fk_producto = vFicha;
        UPDATE preciosProveedores SET precio = unPrecioProveedor WHERE fk_producto = vFicha;

        UPDATE productosPrincipales
           SET usaMedidas = usaMed, usaColores = usaCol, preciosIguales = preciosIg
         WHERE fk_producto = vFicha;
    END IF;

    -- ---------------------------------------------------------------
    -- 2) Variantes existentes enviadas: codigos, descripcion, habilitacion, orden y precios
    -- ---------------------------------------------------------------
    UPDATE Productos p
      JOIN tmpVariantesGuardar t ON t.productoId = p.id AND t.productoId > 0
      JOIN productosVariantes v ON v.fk_producto = p.id AND v.fk_principal = vFicha
       SET p.codBarras = t.codBarras, p.codProveedor = t.codProveedor, p.descripcion = t.descripcion;

    UPDATE productosVariantes v
      JOIN tmpVariantesGuardar t ON t.productoId = v.fk_producto AND t.productoId > 0
       SET v.activa = t.habilitada, v.orden = IF(t.orden > 0, t.orden, v.orden)
     WHERE v.fk_principal = vFicha;

    UPDATE costosProductos c
      JOIN tmpVariantesGuardar t ON t.productoId = c.fk_producto AND t.productoId > 0
      JOIN productosVariantes v ON v.fk_producto = t.productoId AND v.fk_principal = vFicha
       SET c.costo = t.costo;

    UPDATE preciosProductos pr
      JOIN tmpVariantesGuardar t ON t.productoId = pr.fk_producto AND t.productoId > 0
      JOIN productosVariantes v ON v.fk_producto = t.productoId AND v.fk_principal = vFicha
       SET pr.precio = t.precio;

    -- Variantes existentes sin fila de costo/precio: se crean
    INSERT INTO costosProductos (fk_producto, costo)
    SELECT t.productoId, t.costo
      FROM tmpVariantesGuardar t
      JOIN productosVariantes v ON v.fk_producto = t.productoId AND v.fk_principal = vFicha
     WHERE t.productoId > 0
       AND NOT EXISTS (SELECT 1 FROM costosProductos c WHERE c.fk_producto = t.productoId);

    INSERT INTO preciosProductos (fk_producto, precio)
    SELECT t.productoId, t.precio
      FROM tmpVariantesGuardar t
      JOIN productosVariantes v ON v.fk_producto = t.productoId AND v.fk_principal = vFicha
     WHERE t.productoId > 0
       AND NOT EXISTS (SELECT 1 FROM preciosProductos pr WHERE pr.fk_producto = t.productoId);

    -- ---------------------------------------------------------------
    -- 3) Atributos comunes y baja en TODAS las variantes existentes (tambien las no enviadas)
    -- ---------------------------------------------------------------
    UPDATE Productos p
      JOIN productosVariantes v ON v.fk_producto = p.id AND v.fk_principal = vFicha
      JOIN Productos f ON f.id = vFicha
       SET p.fk_Rubro = f.fk_Rubro,
           p.fk_proveedor = f.fk_proveedor,
           p.iva = f.iva,
           p.esPromocion = f.esPromocion,
           p.fraccionado = f.fraccionado,
           p.dolarizado = f.dolarizado,
           p.ganancia = f.ganancia,
           p.descuento = f.descuento,
           p.cantidadMinimaVenta = f.cantidadMinimaVenta,
           p.baja = IF(IFNULL(f.baja, 0) = 1 OR v.activa = 0, 1, 0);

    -- Stock minimo comun (el stock actual no se toca)
    UPDATE stockProductos s
      JOIN productosVariantes v ON v.fk_producto = s.fk_producto AND v.fk_principal = vFicha
       SET s.cantidadMinima = unaCMinima;

    -- ---------------------------------------------------------------
    -- 4) Variantes nuevas: producto + enlace + stock + precio + costo + precio de proveedor
    -- ---------------------------------------------------------------
    WHILE vRn <= vMax DO
        SET cProd = NULL;
        SELECT productoId, medidaId, colorId, habilitada, codBarras, codProveedor, descripcion, costo, precio, stock, orden
          INTO cProd, cMed, cCol, cHab, cCodB, cCodP, cDesc, cCosto, cPrecio, cStock, cOrden
          FROM tmpVariantesGuardar WHERE rn = vRn;

        IF cProd = 0 THEN
            INSERT INTO Productos
                (codProveedor, codBarras, fk_Rubro, iva, descripcion, fk_proveedor, baja,
                 fraccionado, dolarizado, esPromocion, ganancia, descuento, cantidadMinimaVenta)
            SELECT cCodP, cCodB, f.fk_Rubro, f.iva, cDesc, f.fk_proveedor,
                   IF(IFNULL(f.baja, 0) = 1 OR cHab = 0, 1, 0),
                   f.fraccionado, f.dolarizado, f.esPromocion, f.ganancia, f.descuento, f.cantidadMinimaVenta
              FROM Productos f WHERE f.id = vFicha;

            SET vId = LAST_INSERT_ID();

            INSERT INTO productosVariantes (fk_producto, fk_principal, fk_medida, fk_color, activa, orden)
            VALUES (vId, vFicha, cMed, cCol, cHab, IF(cOrden > 0, cOrden, vRn));
            INSERT INTO stockProductos (fk_producto, cantidad, cantidadMinima) VALUES (vId, cStock, unaCMinima);
            INSERT INTO preciosProductos (fk_producto, precio) VALUES (vId, cPrecio);
            INSERT INTO costosProductos (fk_producto, costo) VALUES (vId, cCosto);
            INSERT INTO preciosProveedores (fk_producto, precio) VALUES (vId, cCosto);
        END IF;

        SET vRn = vRn + 1;
    END WHILE;

    COMMIT;

    DROP TEMPORARY TABLE IF EXISTS tmpVariantesGuardar;
    SET SQL_SAFE_UPDATES = vSafe;
    SET salida = vFicha;
END$$
DELIMITER ;
