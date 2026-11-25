-- ============================================================================
-- Promociones NxM: acumular variantes de una misma ficha
-- ----------------------------------------------------------------------------
-- QUE HACE
--   Agrega a promociones_reglas la columna agrupaVariantes (TINYINT(1) NOT NULL DEFAULT 0).
--     0 = (valor actual de todas las promociones) cada producto se evalua por separado.
--     1 = (solo reglas NXM) las cantidades de las distintas variantes de una misma ficha que figuran en la lista de
--         productos de la promocion se suman para completar el grupo "lleva N, paga M".
--
-- COMPATIBILIDAD
--   * Es la UNICA modificacion: no toca ninguna otra tabla, indice ni procedimiento almacenado.
--   * Las promociones existentes quedan en 0 (DEFAULT 0): su comportamiento no cambia.
--   * Idempotente: si la columna ya existe no hace nada. Compatible con MySQL 5.5 y 8.x.
--   * No usa el nombre de la base: se ejecuta sobre la base activa (ale, panalera, ...). Hacer backup antes.
--   * Hasta que se ejecute, la aplicacion de escritorio funciona igual que antes y el check
--     "Acumular las variantes de una misma ficha" del ABM de promociones queda deshabilitado.
--   * Usa la tabla productosVariantes (variantes_01_tablas.sql de LitoralMarket), que ya existe cuando las
--     variantes estan habilitadas (parametro ecommerce/HabilitarVariantesProducto = 1). No se crea nada de eso aqui.
--   * No hace falta crear indices: productosVariantes ya tiene PRIMARY KEY (fk_producto), que es por donde se consulta.
-- ============================================================================

SET @existe = (SELECT COUNT(*)
               FROM information_schema.COLUMNS
               WHERE TABLE_SCHEMA = DATABASE()
                 AND TABLE_NAME   = 'promociones_reglas'
                 AND COLUMN_NAME  = 'agrupaVariantes');

SET @sql = IF(@existe = 0,
    'ALTER TABLE promociones_reglas ADD COLUMN agrupaVariantes TINYINT(1) NOT NULL DEFAULT 0 COMMENT ''Solo NXM: 1 = acumula las variantes de una misma ficha'' AFTER paga',
    'SELECT ''La columna agrupaVariantes ya existe: no se modifica nada.'' AS resultado');

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Verificacion (solo lectura): debe mostrar la columna y todas las promociones existentes con 0
SELECT COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE, COLUMN_DEFAULT
FROM information_schema.COLUMNS
WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'promociones_reglas' AND COLUMN_NAME = 'agrupaVariantes';

SELECT id, nombre, tipo, lleva, paga, agrupaVariantes FROM promociones_reglas ORDER BY id;

-- ----------------------------------------------------------------------------
-- REVERSION (solo si hiciera falta volver atras; las promociones que acumulaban pasarian a evaluarse por producto):
--   ALTER TABLE promociones_reglas DROP COLUMN agrupaVariantes;
-- ----------------------------------------------------------------------------
