using MySqlConnector;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace Comercial.Clases
{
    public class ClassCaja
    {
        Clases.classDatos instDatos = new classDatos();

        public DataTable traerEstadoCaja(int unUsuario)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Caja_VerificarEstadoUltimaCaja", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unUsuarioId", unUsuario);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public string AperturaCaja(int unUsuarioId, string unaObservacion, decimal unSaldoInicial)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_Caja_Apertura";

                cmd.Parameters.AddWithValue("usuarioId", unUsuarioId);
                cmd.Parameters.AddWithValue("unaObservacion", unaObservacion);
                cmd.Parameters.AddWithValue("unSaldoInicial", unSaldoInicial);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
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

        public DataTable traerResumenCaja(int unaCaja, int unUsuario)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_caja_ResumenCaja", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaCajaId", unaCaja);
            a1.SelectCommand.Parameters.AddWithValue("unUsuario", unUsuario);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public decimal traerSaldoCaja(int unaCaja, int unUsuario)
        {
            string query = "SELECT fn_saldo_caja_actual(@p_caja_id, @unUsuario)";

            using (var cmd = new MySqlCommand(query, instDatos.abrirConexion()))
            {
                cmd.Parameters.AddWithValue("@p_caja_id", unaCaja);
                cmd.Parameters.AddWithValue("@unUsuario", unUsuario);

                var resultado = cmd.ExecuteScalar();
                if (resultado == null) return 0;
                return decimal.Parse(resultado.ToString());
            }
        }

        public string AddMovimiento(int cajaId, int unConceptoId, int unMedioPagoId, decimal unImporte, string unaObservacion)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_caja_AddMovimiento";

                cmd.Parameters.AddWithValue("unaCajaId", cajaId);
                cmd.Parameters.AddWithValue("conceptoCajaId", unConceptoId);
                cmd.Parameters.AddWithValue("medioPago", unMedioPagoId);
                cmd.Parameters.AddWithValue("unImporte", unImporte);
                cmd.Parameters.AddWithValue("unaObserv", unaObservacion);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
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

        public DataTable traerTipoGastos()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id, nombre from tipo_gastos", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public string AddGasto(int cajaId, int unConceptoId, int unMedioPagoId, decimal unImporte, string unaObservacion, int tipoGasto)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_caja_AddGasto";

                cmd.Parameters.AddWithValue("unaCajaId", cajaId);
                cmd.Parameters.AddWithValue("conceptoCajaId", unConceptoId);
                cmd.Parameters.AddWithValue("medioPago", unMedioPagoId);
                cmd.Parameters.AddWithValue("unImporte", unImporte);
                cmd.Parameters.AddWithValue("unaObserv", unaObservacion);
                cmd.Parameters.AddWithValue("untipoGasto", tipoGasto);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
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
        public string CierreCaja (int cajaId, decimal unSaldoCierre, string unaObservacion)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_Caja_Cierre";

                cmd.Parameters.AddWithValue("unaCaja", cajaId);
                cmd.Parameters.AddWithValue("saldoFinal", unSaldoCierre);
                cmd.Parameters.AddWithValue("unaObservacion", unaObservacion);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
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

        public int AddCajaPagoProveedores(int unProveedor, string nombreProveedor, decimal unImporte, string unaObservacion, int cajaId, int unConceptoId, int unMedioPago)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_Proveedores_AddPAgoCaja";

                cmd.Parameters.AddWithValue("unProveedor", unProveedor);
                cmd.Parameters.AddWithValue("nomProveedor", nombreProveedor);
                cmd.Parameters.AddWithValue("unImporte", unImporte);
                cmd.Parameters.AddWithValue("unaObserv", unaObservacion);
                cmd.Parameters.AddWithValue("unaCajaId", cajaId);
                cmd.Parameters.AddWithValue("conceptoCajaId", unConceptoId);
                cmd.Parameters.AddWithValue("medioPago", unMedioPago);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                int valor = Int32.Parse(cmd.Parameters["salida"].Value.ToString());
                return valor;
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

        public DataTable traerEncabezadoCajaPorUsuarioyFechas(int unUsuario, DateTime desde, DateTime hasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_caja_TraerEncabezadoPorUsuarioyFecha", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unUsuario", unUsuario);
            a1.SelectCommand.Parameters.AddWithValue("desde", desde);
            a1.SelectCommand.Parameters.AddWithValue("hasta", hasta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public void ExportarResumenCajaCsv(int numeroCaja, string usuario, DateTime fechaInicio, DateTime? fechaCierre, DataTable dtDetalle, decimal totalDebe, decimal totalHaber, string observaciones)
        {

            string carpetaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Downloads");

            string archivo = $"Caja_{numeroCaja}.csv";

            string path = Path.Combine(carpetaDescargas, archivo);

            var sb = new StringBuilder();

            // Encabezado
            sb.AppendLine($"Numero Caja;{numeroCaja}");
            sb.AppendLine($"Usuario;{usuario}");
            sb.AppendLine($"Fecha Inicio;{fechaInicio:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Fecha Cierre;{fechaCierre:dd/MM/yyyy HH:mm}");
            sb.AppendLine();

            // Columnas
            sb.AppendLine("Debe;Importe Debe;Haber;Importe Haber");

            // Detalle
            foreach (DataRow row in dtDetalle.Rows)
            {
                sb.AppendLine($"{row["Debe"]};{row["Importe Debe"]};{row["Haber"]};{row["Importe Haber"]}");
            }

            sb.AppendLine();
            sb.AppendLine($"Total Debe;{totalDebe};Total Haber;{totalHaber}");
            sb.AppendLine();
            sb.AppendLine("Observaciones;");
            sb.AppendLine(observaciones);

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        public DataTable traerDetalleMovimientoPorUsuarioyFechas(int unUsuario, DateTime desde, DateTime hasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_caja_detalleMovimiento", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unUsuario", unUsuario);
            a1.SelectCommand.Parameters.AddWithValue("desde", desde);
            a1.SelectCommand.Parameters.AddWithValue("hasta", hasta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public void ExportarDetalleCajaCsv(DataTable dtDetalle)
        {

            string carpetaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            string archivo = $"CajaDetalle_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            string path = Path.Combine(carpetaDescargas, archivo);

            var sb = new StringBuilder();           

            // Columnas
            sb.AppendLine("Nro Caja;Fecha;Concepto;Tipo;Medio Pago;Importe;Observaciones");

            // Detalle
            foreach (DataRow row in dtDetalle.Rows)
            {
                sb.AppendLine($"{row["Nro Caja"]};{row["Fecha"]};{row["Concepto"]};{row["Tipo"]};{row["Medio Pago"]};{row["Importe"]};{row["Observaciones"]}");
            }           

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
