using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public static void guardarParametro(string modulo, string parametro, string valor)
        {
            try
            {
                classDatos datos = new classDatos();

                using (MySqlConnection conn = datos.abrirConexion())
                {
                    // 1️⃣ Verificar si existe
                    using (MySqlCommand checkCmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM parametros WHERE modulo = @modulo AND parametro = @parametro",
                        conn))
                    {
                        checkCmd.Parameters.AddWithValue("@modulo", modulo);
                        checkCmd.Parameters.AddWithValue("@parametro", parametro);

                        int existe = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (existe > 0)
                        {
                            // 2️⃣ UPDATE
                            using (MySqlCommand updateCmd = new MySqlCommand(
                                "UPDATE parametros SET valor = @valor WHERE modulo = @modulo AND parametro = @parametro",
                                conn))
                            {
                                updateCmd.Parameters.AddWithValue("@valor", valor);
                                updateCmd.Parameters.AddWithValue("@modulo", modulo);
                                updateCmd.Parameters.AddWithValue("@parametro", parametro);

                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // 3️⃣ INSERT
                            using (MySqlCommand insertCmd = new MySqlCommand(
                                "INSERT INTO parametros (modulo, parametro, valor) VALUES (@modulo, @parametro, @valor)",
                                conn))
                            {
                                insertCmd.Parameters.AddWithValue("@modulo", modulo);
                                insertCmd.Parameters.AddWithValue("@parametro", parametro);
                                insertCmd.Parameters.AddWithValue("@valor", valor);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar parámetro: " + ex.Message);
            }
        }

        public void ActualizarParametro(int unId, string unNuevoValor)
        {
            classDatos datos = new classDatos();
            using (MySqlConnection conn = datos.abrirConexion())
            {
                using (MySqlCommand updateCmd = new MySqlCommand(
                               "UPDATE parametros SET valor = @valor WHERE id = @id",
                               conn))
                {
                    updateCmd.Parameters.AddWithValue("@valor", unNuevoValor);
                    updateCmd.Parameters.AddWithValue("@id", unId);


                    updateCmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable traerParametro()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("SELECT id, modulo Modulo, parametro Parametro, valor Valor FROM parametros where parametro <> 'imagen'", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

    }
}

