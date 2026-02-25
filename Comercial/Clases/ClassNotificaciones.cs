using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Comercial.Clases
{
    public class ClassNotificaciones
    {
        public async void VerificarDolarAsync()
        {
            try
            {
                decimal dolarSistema = Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar") == "" ? 0 : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar"));
                decimal dolarApi = await ObtenerDolarDesdeApi();
                decimal diferenciaDolar = Clases.ClassParametros.buscarParametro("productos", "alertaDiferenciaDolar") == "" ? 0 : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "alertaDiferenciaDolar"));

                if (Math.Abs(dolarSistema - dolarApi) > diferenciaDolar)
                {
                    CrearNotificacion(
                        "DOLAR",
                        $"El dólar cambió. Sistema: {dolarSistema} - Actual: {dolarApi}",
                        "ABRIR_CONFIG_DOLAR",
                        0
                    );
                }
            }
            catch
            {

            }
        }

        public void VerificarStockMinimo()
        {
            Clases.classDatos instDatos = new classDatos();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ProductosExistenDebajoCantidadMinima";                


                MySqlParameter pExiste = new MySqlParameter("pExiste", MySqlDbType.Bool);
                pExiste.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(pExiste);

                cmd.ExecuteScalar();
                bool valor = bool.Parse(cmd.Parameters["pExiste"].Value.ToString());

                if (valor)
                {
                     CrearNotificacion(
                           "STOCK_MINIMO",
                           "Existen Producto Por Debajo de la cantidad Mínima",
                           "ABRIR_PRODUCTO",
                           0
                       );
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }  
        }

        private async Task<decimal> ObtenerDolarDesdeApi()
        {
            using (HttpClient client = new HttpClient())
            {
                var json = await client.GetStringAsync("https://cdn.moneyconvert.net/api/latest.json");
                var data = JObject.Parse(json);

                decimal dolarBlue = data["rates"]["DOLARBLUE"].Value<decimal>();
                return dolarBlue;
            }
        }

        public void CrearNotificacion(string unTipo, string unMensaje, string unaAccion, int unaReferencia)
        {
            Clases.classDatos instDatos = new classDatos();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_Notificaciones_Add";

                cmd.Parameters.AddWithValue("unTipo", unTipo);
                cmd.Parameters.AddWithValue("unMensaje", unMensaje);
                cmd.Parameters.AddWithValue("unaAccion", unaAccion);
                cmd.Parameters.AddWithValue("unaReferencia", unaReferencia);

                cmd.ExecuteScalar();

                
                                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public DataTable traerNotificacionesPendientes()
        {
            Clases.classDatos instDatos = new classDatos();
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from Notificaciones where Leida = 0", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public bool MarcarComoLeida (int unaNotificacion)
        {
            Clases.classDatos instDatos = new classDatos();
            string query = "UPDATE Notificaciones SET Leida = 1 WHERE Id = @id";

            using (var conn = instDatos.abrirConexion())
            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", unaNotificacion);

                int filasAfectadas = cmd.ExecuteNonQuery();

                return filasAfectadas > 0;
            }
        }
        
    }
}
