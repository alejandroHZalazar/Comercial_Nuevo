using MySqlConnector;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        public DataTable imprimirDevolucion(long unId)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_DevolucionPrint", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaDevolucion", unId);

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

        public long grabarDevolucion(decimal unTotal, decimal unCosto, int unCliente, int unCajero, decimal unIva, decimal unDescuento, decimal unRecargo, int unVendedor, decimal unaComision, decimal unImpuesto,
                                        string unDetalle, int unLlevaCC)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_DevolucionAddDevolucion";

                cmd.Parameters.AddWithValue("unTotal", unTotal);
                cmd.Parameters.AddWithValue("unCosto", unCosto);
                cmd.Parameters.AddWithValue("unCliente", unCliente);
                cmd.Parameters.AddWithValue("unCajero", unCajero);
                cmd.Parameters.AddWithValue("unIva", unIva);
                cmd.Parameters.AddWithValue("unDescuento", unDescuento);
                cmd.Parameters.AddWithValue("unRecargo", unRecargo);
                cmd.Parameters.AddWithValue("unVendedor", unVendedor);
                cmd.Parameters.AddWithValue("unaComision", unaComision);
                cmd.Parameters.AddWithValue("unImpuesto", unImpuesto);
                cmd.Parameters.AddWithValue("detalle", unDetalle);
                cmd.Parameters.AddWithValue("llevaCC", unLlevaCC);

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

        public long grabarVenta(decimal unTotal, decimal unCosto, int unCliente, int unCajero, decimal unIva, decimal? unDescuento, 
                                decimal? unRecargo, int unVendedor, decimal comision, decimal unImpuesto, string unDetalle, int llevaCC, 
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

        /// <summary>
        /// Obtiene el 'linea' (PK) de la fila de ventasDetalle para el producto dado en la venta dada.
        /// Útil para relacionar la línea de una promo con sus componentes.
        /// </summary>
        public long traerLineaDetalleVenta(long idVenta, int idProducto)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT linea FROM ventasDetalle WHERE fk_venta = @venta AND fk_producto = @prod ORDER BY linea DESC LIMIT 1",
                    instDatos.abrirConexion());
                cmd.Parameters.AddWithValue("@venta", idVenta);
                cmd.Parameters.AddWithValue("@prod", idProducto);
                object res = cmd.ExecuteScalar();
                instDatos.cerrarConexion();
                return res != null ? long.Parse(res.ToString()) : -1;
            }
            catch
            {
                instDatos.cerrarConexion();
                return -1;
            }
        }

        /// <summary>
        /// Guarda los componentes elegidos de una promo al vender y descuenta stock.
        /// </summary>
        public int guardarComponentesPromocion(long fkVentaDetalle, string detalleComponentes)
        {
            ClassPromociones instPromo = new ClassPromociones();
            return instPromo.guardarComponentesVenta(fkVentaDetalle, detalleComponentes);
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

        public DataTable traerVentaDetalleProductoCsv(DateTime desde, DateTime hasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Ventas_DetalleProductosCSV", instDatos.abrirConexion());
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

        public string traerFormaPagoFacturas(long unaVenta)
        {
            MySqlCommand nComando = new MySqlCommand(
                "SELECT fn_Ventas_FormasPagoFactura(@unaVenta)",
                instDatos.abrirConexion()
            );

            nComando.Parameters.AddWithValue("@unaVenta", unaVenta);

            object res = nComando.ExecuteScalar();

            string valor = "";

            if (res != null && res != DBNull.Value)
                valor = res.ToString();

            instDatos.cerrarConexion();

            return valor;
        }

        public DataTable TraerCabeceraNC(long unaDevolucion)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Devolucion_TraerCabeceraNotaCredito", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("UnaDevolucion", unaDevolucion);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable TraerDetalleNC(long unaDevolucion)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Devolucion_TraerDetalleNotaCredito", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("UnaDevolucion", unaDevolucion);

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

            private decimal _Recargo;
            public decimal Recargo
            {
                get => _Recargo;
                set { _Recargo = value; OnPropertyChanged(nameof(Recargo)); }
            }

            private void OnPropertyChanged(string nombre)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
            }
        }
    }

    public class ProductoBusqueda
    {
        public string Descripcion { get; set; }
        public int IdProveedor { get; set; }
    }

}
