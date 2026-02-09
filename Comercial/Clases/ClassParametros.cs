using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Comercial.Clases
{
    class ClassParametros
    {
        classDatos instDatos = new classDatos();
        public string traePathLogo()
        {
            MySqlCommand nComando = new MySqlCommand("select valor from parametros where modulo = 'login' and parametro = 'logo'", instDatos.abrirConexion());
            string valor = nComando.ExecuteScalar().ToString();
            instDatos.cerrarConexion();
            return valor;
        }

        public string traerRazonSocial()
        {
            MySqlCommand nComando = new MySqlCommand("select valor from parametros where modulo = 'empresa' and parametro = 'razonSocial'", instDatos.abrirConexion());
            string valor = nComando.ExecuteScalar().ToString();
            instDatos.cerrarConexion();
            return valor;
        }

        public static string traerColorDefecto()
        {
            classDatos datos = new classDatos();
            MySqlCommand nComando = new MySqlCommand("select valor from parametros where modulo = 'color' and parametro = 'defecto'", datos.abrirConexion());
            string valor = nComando.ExecuteScalar().ToString();
            datos.cerrarConexion();
            return valor;
        }

        public static int indiceBusqNotaPed()
        {
            classDatos datos = new classDatos();
            MySqlCommand nComando = new MySqlCommand("select valor from parametros where modulo = 'notaPedido' and parametro = 'indiceBusqueda'", datos.abrirConexion());
            int valor = int.Parse(nComando.ExecuteScalar().ToString());
            datos.cerrarConexion();
            return valor;
        }

        public static string buscarParametro(string unModulo, string unParametro)
        {
            try
            {
                classDatos datos = new classDatos();
                using (MySqlCommand nComando = new MySqlCommand(
                    "SELECT valor FROM parametros WHERE modulo = @modulo AND parametro = @parametro",
                    datos.abrirConexion()))
                {
                    nComando.Parameters.AddWithValue("@modulo", unModulo);
                    nComando.Parameters.AddWithValue("@parametro", unParametro);

                    object resultado = nComando.ExecuteScalar();

                    if (resultado != null && resultado != DBNull.Value)
                        return resultado.ToString();
                    else
                        return ""; // no encontró valor
                }
            }
            catch (Exception ex)
            {
                // Ideal loguearlo
                return "";
            }
        }
    }
}

