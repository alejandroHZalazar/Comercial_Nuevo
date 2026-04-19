using MySqlConnector;
using System;
using System.Data;

namespace Comercial.Clases
{
    public class ClassPromociones
    {
        classDatos instDatos = new classDatos();

        /// <summary>
        /// Lista promociones. unFiltro es fragmento SQL opcional (ej: " AND pr.activa = 1").
        /// </summary>
        public DataTable obtenerPromociones(string unFiltro)
        {
            MySqlDataAdapter da = new MySqlDataAdapter("sp_PromocionesConsulta", instDatos.abrirConexion());
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro ?? "");
            DataTable dt = new DataTable();
            da.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        /// <summary>
        /// Devuelve tres DataTable: [0] cabecera promo, [1] slots, [2] productos por slot.
        /// </summary>
        public DataSet obtenerPromocionCompleta(int fkProducto)
        {
            MySqlDataAdapter da = new MySqlDataAdapter("sp_PromocionesTraerCompleta", instDatos.abrirConexion());
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("unFkProducto", fkProducto);
            DataSet ds = new DataSet();
            da.Fill(ds);
            instDatos.cerrarConexion();
            return ds;
        }

        /// <summary>
        /// Alta o modificación de la promoción.
        /// accion: 1=insert, 2=update, 3=delete lógico.
        /// Retorna id de la promo o -1 en error.
        /// </summary>
        public int ABMPromocion(int accion, int idPromocion, int fkProducto, bool activa, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_PromocionesABM", instDatos.abrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("unaAccion", accion);
                cmd.Parameters.AddWithValue("unIdPromocion", idPromocion);
                cmd.Parameters.AddWithValue("unFkProducto", fkProducto);
                cmd.Parameters.AddWithValue("unActiva", activa ? 1 : 0);
                cmd.Parameters.AddWithValue("unaFechaDesde", fechaDesde.HasValue ? (object)fechaDesde.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("unaFechaHasta", fechaHasta.HasValue ? (object)fechaHasta.Value : DBNull.Value);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteNonQuery();
                return int.Parse(cmd.Parameters["salida"].Value.ToString());
            }
            catch
            {
                return -1;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        /// <summary>
        /// Recrea todos los slots de una promoción.
        /// detalleSlots formato: "num|cantReq|desc|prod1,prod2;num2|cantReq2|desc2|prod3,prod4"
        /// </summary>
        public int guardarSlots(int idPromocion, string detalleSlots)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_SlotsABM", instDatos.abrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("unIdPromocion", idPromocion);
                cmd.Parameters.AddWithValue("unDetalleSlots", detalleSlots);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteNonQuery();
                return int.Parse(cmd.Parameters["salida"].Value.ToString());
            }
            catch
            {
                return -1;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        /// <summary>
        /// Guarda los componentes elegidos al vender una promo y descuenta stock.
        /// detalle formato: "idSlot#idProducto*cantidad;idSlot2#idProducto2*cantidad2"
        /// </summary>
        public int guardarComponentesVenta(long fkVentaDetalle, string detalle)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_VentasAddPromoComponentes", instDatos.abrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("unFkVentaDetalle", fkVentaDetalle);
                cmd.Parameters.AddWithValue("unDetalle", detalle);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteNonQuery();
                return int.Parse(cmd.Parameters["salida"].Value.ToString());
            }
            catch
            {
                return -1;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        /// <summary>
        /// Retorna true si el producto con id dado es una promoción activa.
        /// </summary>
        public bool esPromocionActiva(int idProducto)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT COUNT(*) FROM promociones WHERE fk_producto = @id AND activa = 1",
                    instDatos.abrirConexion());
                cmd.Parameters.AddWithValue("@id", idProducto);
                int count = int.Parse(cmd.ExecuteScalar().ToString());
                instDatos.cerrarConexion();
                return count > 0;
            }
            catch
            {
                instDatos.cerrarConexion();
                return false;
            }
        }
    }
}
