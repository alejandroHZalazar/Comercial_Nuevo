-- =====================================================================
-- ASIGNACION DE SUBRUBRO DE PROVEEDOR A PRODUCTOS - 04: REVERSION (solo si hace falta deshacer)
-- !!! Devuelve Productos.fk_rubro al rubro anterior registrado en ProductosRubroMigracion !!!
-- Revierte el lote indicado (por defecto, el ultimo). Solo toca productos que siguen en el
-- subrubro asignado por la migracion (si otro usuario ya los reclasifico, no los pisa).
-- Si Workbench informara el error 1175 (safe update mode), ejecutar antes: SET SQL_SAFE_UPDATES = 0;
-- =====================================================================
SET @lote = (SELECT MAX(lote) FROM ProductosRubroMigracion);   -- o fijar un lote: SET @lote = '20261101120000';

START TRANSACTION;

UPDATE ProductosRubroMigracion b
STRAIGHT_JOIN Productos p ON p.id = b.fk_producto
   SET p.fk_rubro = b.fk_rubroAnterior
 WHERE b.lote = @lote
   AND p.fk_rubro = b.fk_rubroNuevo;

SELECT ROW_COUNT() AS productosRevertidos, @lote AS lote;

COMMIT;