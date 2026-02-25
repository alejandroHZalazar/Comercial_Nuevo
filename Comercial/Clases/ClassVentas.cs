using MySqlConnector;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;

namespace Comercial.Clases
{
    public class ClassVentas
    {
        classDatos instDatos = new classDatos();

        public DataTable traerPedidosPendientes(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_PedidosTraerPendientesCabecera", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerVentasParaDevolver(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Ventas_TraerParaDevolver", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable imprimirVenta(long unId)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_VentasPrint", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaVenta", unId);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable imprimirVentaTk(long unId)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_VentasPrintNuevo", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaVenta", unId);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerDetalleVentaADevolver(long unaVenta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Ventas_TraerParaDevolverDetalle", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaVenta", unaVenta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerTodos (string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Ventas", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerTodosDevolucion(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_devoluciones", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerTodosDetalles(long unaVenta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_VentasDetalles", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaVenta", unaVenta );

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerTodosDetallesDevoluciones(long unaDev)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_DevolucionesDetalles", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaDevolucion", unaDev);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public long grabarCabeceraVenta (decimal unTotal, decimal unCosto, int unCliente, int unCajero, decimal unIva, decimal unDescuento, decimal unRecargo, int unVendedor, decimal comision, decimal unImpuesto)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_VentasGrabarCabecera";

                cmd.Parameters.AddWithValue("unTotal", unTotal);
                cmd.Parameters.AddWithValue("unCosto", unCosto);
                cmd.Parameters.AddWithValue("unCliente", unCliente);
                cmd.Parameters.AddWithValue("unCajero", unCajero);
                cmd.Parameters.AddWithValue("unIva", unIva);
                cmd.Parameters.AddWithValue("unDescuento", unDescuento );
                cmd.Parameters.AddWithValue("unRecargo", unRecargo );
                cmd.Parameters.AddWithValue("unVendedor", unVendedor);
                cmd.Parameters.AddWithValue("unaComision", comision);
                cmd.Parameters.AddWithValue("unImpuesto", unImpuesto);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int64);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                long valor = long.Parse(cmd.Parameters["salida"].Value.ToString());
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

        public long grabarCabeceraDevolucion(decimal unTotal, decimal unCosto, int unCliente, int unCajero, decimal unIva, decimal unDescuento, decimal unRecargo, int unVendedor, decimal unaComision)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_DevolucionGrabarCabecera";

                cmd.Parameters.AddWithValue("unTotal", unTotal);
                cmd.Parameters.AddWithValue("unCosto", unCosto);
                cmd.Parameters.AddWithValue("unCliente", unCliente);
                cmd.Parameters.AddWithValue("unCajero", unCajero);
                cmd.Parameters.AddWithValue("unIva", unIva);
                cmd.Parameters.AddWithValue("unDescuento", unDescuento);
                cmd.Parameters.AddWithValue("unRecargo", unRecargo);
                cmd.Parameters.AddWithValue("unVendedor", unVendedor);
                cmd.Parameters.AddWithValue("unaComision", unaComision);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int64);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                long valor = long.Parse(cmd.Parameters["salida"].Value.ToString());
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

        public decimal traerPrecioProductosVentas (int unTipo, int unProducto, int unPedido)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_cambioPreciosVentas";
                cmd.Parameters.AddWithValue("tipo", unTipo );
                cmd.Parameters.AddWithValue("producto", unProducto);
                cmd.Parameters.AddWithValue("pedido", unPedido );
                MySqlParameter precio = new MySqlParameter("precioOut", MySqlDbType.Decimal);
                precio.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(precio);

                cmd.ExecuteScalar();
                decimal valor = decimal.Parse(cmd.Parameters["precioOut"].Value.ToString());
                return valor;
            }
            catch
            {
                return -1;
            }
        }

        public long grabarProcesoDetallVenta(long unaVenta, string  unDetalle)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_VentasAddDetalle";

                cmd.Parameters.AddWithValue("unaVenta", unaVenta);
                cmd.Parameters.AddWithValue("detalle", unDetalle );
                

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int64);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                long valor = long.Parse(cmd.Parameters["salida"].Value.ToString());
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

        public long grabarProcesoDetalleDevolucion(long unaDevolucion, string unDetalle)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_DevolucionAddDetalle";
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("unaDevolucion", unaDevolucion);
                cmd.Parameters.AddWithValue("detalle", unDetalle);


                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int64);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                long valor = 0;
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

        public long ultimaVenta()
        {
            MySqlCommand nComando = new MySqlCommand("select MAX(id) from ventas", instDatos.abrirConexion());
            long valor = long.Parse(nComando.ExecuteScalar().ToString());
            instDatos.cerrarConexion();
            return valor;
        }

        public long grabarVenta(decimal unTotal, decimal unCosto, int unCliente, int unCajero, decimal unIva, decimal unDescuento, 
                                decimal unRecargo, int unVendedor, decimal comision, decimal unImpuesto, string unDetalle, int llevaCC, 
                                int imputaEnVenta, int tieneMediosPagos, decimal ImporteCobro, string DetallePlanPago, int haceCaja, int idCaja)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_VentasAddVenta ";

                cmd.Parameters.AddWithValue("unTotal", unTotal);
                cmd.Parameters.AddWithValue("unCosto", unCosto);
                cmd.Parameters.AddWithValue("unCliente", unCliente);
                cmd.Parameters.AddWithValue("unCajero", unCajero);
                cmd.Parameters.AddWithValue("unIva", unIva);
                cmd.Parameters.AddWithValue("unDescuento", unDescuento);
                cmd.Parameters.AddWithValue("unRecargo", unRecargo);
                cmd.Parameters.AddWithValue("unVendedor", unVendedor);
                cmd.Parameters.AddWithValue("unaComision", comision);
                cmd.Parameters.AddWithValue("unImpuesto", unImpuesto);
                cmd.Parameters.AddWithValue("llevaCC", llevaCC);
                cmd.Parameters.AddWithValue("detalle", unDetalle);
                cmd.Parameters.AddWithValue("detallePlanPago", DetallePlanPago);
                cmd.Parameters.AddWithValue("imputaEnVenta", imputaEnVenta);
                cmd.Parameters.AddWithValue("tieneMediosPagos", tieneMediosPagos);
                cmd.Parameters.AddWithValue("ImporteCobro", ImporteCobro);
                cmd.Parameters.AddWithValue("haceCaja", haceCaja);
                cmd.Parameters.AddWithValue("idCaja", idCaja);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int64);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                long valor = long.Parse(cmd.Parameters["salida"].Value.ToString());
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

        public DataTable traerPlanesPago()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id Nro, nombre Nombre, recargo Recargo from planes_pago", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public int traerIdPlanPagoporNombre(string unPlanPago)
        {
            MySqlCommand nComando = new MySqlCommand("select id from planes_pago where nombre = '" + unPlanPago + "'", instDatos.abrirConexion());
            int valor = int.Parse(nComando.ExecuteScalar().ToString());
            instDatos.cerrarConexion();
            return valor;
        }

        public DataTable traerVentaDetalleCsv(DateTime desde, DateTime hasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Ventas_DetalleCSV", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("desde", desde);
            a1.SelectCommand.Parameters.AddWithValue("hasta", hasta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable TraerCabeceraFactura(long unaVenta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Ventas_TraerCabeceraFactura", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaVenta", unaVenta);           

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable TraerDetalleFactura(long unaVenta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Ventas_TraerDetalleFactura", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaVenta", unaVenta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable TraerVentasNoFacturadas(DateTime fechaDesde, DateTime fechaHasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Ventas_TraerSinFacturarPorFecha", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("desde", fechaDesde);
            a1.SelectCommand.Parameters.AddWithValue("hasta", fechaHasta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }
        public class CobroFormasPago
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private int _idMetodo;
            public int idMedio
            {
                get => _idMetodo;
                set { _idMetodo = value; OnPropertyChanged(nameof(idMedio)); }
            }

            private string _Metodo;
            public string Medio
            {
                get => _Metodo;
                set { _Metodo = value; OnPropertyChanged(nameof(Medio)); }
            }

            private int _idPlan;
            public int idPlan
            {
                get => _idPlan;
                set { _idPlan = value; OnPropertyChanged(nameof(idPlan)); }
            }

            private string _Plan;
            public string Plan
            {
                get => _Plan;
                set { _Plan = value; OnPropertyChanged(nameof(Plan)); }
            }

            private decimal _Importe;
            public decimal Importe
            {
                get => _Importe;
                set { _Importe = value; OnPropertyChanged(nameof(Importe)); }
            }

            private string _Referencia1;
            public string Referencia1
            {
                get => _Referencia1;
                set { _Referencia1 = value; OnPropertyChanged(nameof(Referencia1)); }
            }

            private string _Referencia2;
            public string Referencia2
            {
                get => _Referencia2;
                set { _Referencia2 = value; OnPropertyChanged(nameof(Referencia2)); }
            }

            private string _Referencia3;
            public string Referencia3
            {
                get => _Referencia3;
                set { _Referencia3 = value; OnPropertyChanged(nameof(Referencia3)); }
            }

            private bool _necesitaDatos;

            public bool necesitaDatos
            {
                get => _necesitaDatos;
                set { _necesitaDatos = value; OnPropertyChanged(nameof(necesitaDatos)); }
            }

            private void OnPropertyChanged(string nombre)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
            }
        }

            public void ExportarVentasCsv(DataTable detalle)
            {

                string carpetaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                string archivo = "Ventas_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".csv";

                string path = Path.Combine(carpetaDescargas, archivo);

                var sb = new StringBuilder();


                // Columnas
                sb.AppendLine("Nro;Fecha;Total;Costo;Cliente;Cajero;IVA;Descuento;Recargo;Vendedor;Comision;Impuesto;Medio Pago 1;Medio Pago 2;Medio Pago 3");

                // Detalle
                foreach (DataRow row in detalle.Rows)
                {
                    sb.AppendLine($"{row["Nro"]};{row["Fecha"]};{row["Total"]};{row["Costo"]};{row["Cliente"]};{row["Cajero"]};{row["IVA"]};{row["Descuento"]};{row["Recargo"]};{row["Vendedor"]};{row["Comision"]};{row["Impuesto"]};{row["Medio Pago 1"]};{row["Medio Pago 2"]};{row["Medio Pago 3"]}");
                }               

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            }

        

    }
}
