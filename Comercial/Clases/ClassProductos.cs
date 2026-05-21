using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace Comercial.Clases
{
    class ClassProductos
    {
        classDatos instDatos = new classDatos();

        public DataTable traeProductosPpal(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosConsultaPpal", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traeParaCambiarPrecio(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosTraerParaCambioPrecio", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traeMovimientoProductos(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosTraerMovimientos", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traeProductosStock(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosTraerStock", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerListaPrecios(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosListaPrecios", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerLotesIngreso(DateTime desde, DateTime hasta, string unComprobante, int unTipo)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Productos_TraerLotesIngreso", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unTipo", unTipo);
            a1.SelectCommand.Parameters.AddWithValue("desde", desde);
            a1.SelectCommand.Parameters.AddWithValue("hasta", hasta);
            a1.SelectCommand.Parameters.AddWithValue("unComprobante", unComprobante);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public int traerDefaultBusq()
        {
            MySqlCommand nComando = new MySqlCommand("select valor from parametros where modulo = 'productos' and parametro = 'indiceBusqueda'", instDatos.abrirConexion());
            int valor = int.Parse(nComando.ExecuteScalar().ToString());
            instDatos.cerrarConexion();
            return valor;
        }


        public static int cantDecimales()
        {
            classDatos instDatos = new Clases.classDatos();
            MySqlCommand nComando = new MySqlCommand("select valor from parametros where modulo = 'productos' and parametro = 'decimales'", instDatos.abrirConexion());
            int valor = int.Parse(nComando.ExecuteScalar().ToString());
            instDatos.cerrarConexion();
            return valor;
        }

        public static int cantDecimalesStock()
        {
            classDatos instDatos = new Clases.classDatos();
            MySqlCommand nComando = new MySqlCommand("select valor from parametros where modulo = 'productos' and parametro = 'decimalesCant'", instDatos.abrirConexion());
            int valor = int.Parse(nComando.ExecuteScalar().ToString());
            instDatos.cerrarConexion();
            return valor;
        }


        public DataTable traerProductosParaEditar(int unProducto)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosTraerParaEditar", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unProducto", unProducto);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerDetalleLoteIngreso(string unComprobante)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Productos_TraerLotesIngresoDetalles", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unComprobante", unComprobante);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerParaAjustar(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosTraerpParaAjustar", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public decimal calcularPrecioLista(decimal unCosto, int unProveedor)
        {
            MySqlCommand nComando = new MySqlCommand("select ganancia from Proveedores where id = " + unProveedor, instDatos.abrirConexion());
            decimal valor = decimal.Parse(nComando.ExecuteScalar().ToString());
            valor = Math.Round(unCosto * (1 + valor / 100), 2);
            instDatos.cerrarConexion();
            return valor;
        }

        public void agregarStock(string unProducto, decimal unaCantidad)
        {
            MySqlCommand nComando = new MySqlCommand("update stockProductos set cantidad = cantidad +" + unaCantidad.ToString().Replace(',', '.') + " where fk_producto = " + unProducto, instDatos.abrirConexion());
            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }

        public void actualizarEsPromocion(int idProducto, bool esPromocion)
        {
            MySqlCommand cmd = new MySqlCommand(
                "UPDATE Productos SET esPromocion = @val WHERE id = @id",
                instDatos.abrirConexion());
            cmd.Parameters.AddWithValue("@val", esPromocion ? 1 : 0);
            cmd.Parameters.AddWithValue("@id", idProducto);
            cmd.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }

        public bool traerEsPromocion(int idProducto)
        {
            MySqlCommand cmd = new MySqlCommand(
                "SELECT COALESCE(esPromocion, 0) FROM Productos WHERE id = @id",
                instDatos.abrirConexion());
            cmd.Parameters.AddWithValue("@id", idProducto);
            object val = cmd.ExecuteScalar();
            instDatos.cerrarConexion();
            return val != null && val.ToString() == "1";
        }

        public void actualizarEstadoProducto(string unProducto, bool baja)
        {
            MySqlCommand nComando = new MySqlCommand("update Productos set baja = " + baja.ToString() + " where id = " + unProducto, instDatos.abrirConexion());
            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }

        public void actualziarMasivaPrecios(string unProducto, decimal unPrecio, string unaDescripcion, string unCodBarras, int unProveedor)
        {
            MySqlCommand nComando = new MySqlCommand("sp_Productos_CambiarPreciosMasivos", instDatos.abrirConexion());
            nComando.CommandType = CommandType.StoredProcedure;

            nComando.Parameters.AddWithValue("@unProducto", unProducto);
            nComando.Parameters.AddWithValue("@unPrecio", unPrecio);
            nComando.Parameters.AddWithValue("@unCodBarras", unCodBarras);
            nComando.Parameters.AddWithValue("@unaDescripcion", unaDescripcion);
            nComando.Parameters.AddWithValue("@unProveedor", unProveedor);

            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }

        public void actualizarPrecios(int unProducto, decimal unPrecio, decimal unCosto, decimal unPrecioProv)
        {
            MySqlCommand nComando = new MySqlCommand("sp_productosActualizarPrecios", instDatos.abrirConexion());
            nComando.CommandType = CommandType.StoredProcedure;

            nComando.Parameters.AddWithValue("@unProducto", unProducto);
            nComando.Parameters.AddWithValue("@unPrecio", unPrecio);
            nComando.Parameters.AddWithValue("@unCosto", unCosto);
            nComando.Parameters.AddWithValue("@unPrecioProv", unPrecioProv);

            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }


        /// <summary>
        /// Actualiza precio proveedor, precio de lista y/o costo de un producto directamente
        /// (por codProveedor). Solo actualiza las tablas cuyo valor sea mayor a cero.
        /// </summary>
        public void actualizarPreciosDirectos(string codProveedor, decimal precioProv, decimal precioSIVA, decimal costo)
        {
            MySqlCommand nComando = new MySqlCommand("sp_ProductosCambiarPreciosDirectos", instDatos.abrirConexion());
            nComando.CommandType = CommandType.StoredProcedure;
            nComando.Parameters.AddWithValue("@unCodProveedor", codProveedor);
            nComando.Parameters.AddWithValue("@unPrecioProv",   precioProv);
            nComando.Parameters.AddWithValue("@unPrecioSIVA",   precioSIVA);
            nComando.Parameters.AddWithValue("@unCosto",        costo);
            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }

        public void ajustarStock(int unProducto, decimal unStock, decimal unaDif)
        {
            MySqlCommand nComando = new MySqlCommand("sp_Productos_AjusteStock", instDatos.abrirConexion());
            nComando.CommandType = CommandType.StoredProcedure;

            nComando.Parameters.AddWithValue("@unIdProducto", unProducto);
            nComando.Parameters.AddWithValue("@unStock", unStock);
            nComando.Parameters.AddWithValue("@unaDif", unaDif);


            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }


        public void registrarMovimiento(string unProducto, int unTipoMov, string unaDesc, decimal unStockAnt, decimal unStockAct, decimal unCosto, decimal unaVenta, decimal unaCantidad, DateTime unaFecha, string unComprobante, decimal unPrecioProveedor)

        {
            MySqlCommand nComando = new MySqlCommand("insert into productosMovimientos (fk_producto, tipoMovimiento, descripcion, stockAnt, stockAct, costo, venta,cantidad,fechaEntrega,nroComprobante,precio_Proveedor,fechaMov) values (@producto,@movimiento,@descripcion,@stockAnt,@stockAct,@costo,@venta,@cantidad,@fecha,@comprobante,@precioProveedor,@fechaMov)", instDatos.abrirConexion());

            nComando.Parameters.AddWithValue("@producto", unProducto);
            nComando.Parameters.AddWithValue("@movimiento", unTipoMov);
            nComando.Parameters.AddWithValue("@descripcion", unaDesc);
            nComando.Parameters.AddWithValue("@stockAnt", unStockAnt);
            nComando.Parameters.AddWithValue("@stockAct", unStockAct);
            nComando.Parameters.AddWithValue("@costo", unCosto);
            nComando.Parameters.AddWithValue("@venta", unaVenta);
            nComando.Parameters.AddWithValue("@cantidad", unaCantidad);
            nComando.Parameters.AddWithValue("@fecha", unaFecha);
            nComando.Parameters.AddWithValue("@comprobante", unComprobante);
            nComando.Parameters.AddWithValue("@precioProveedor", unPrecioProveedor);
            nComando.Parameters.AddWithValue("@fechaMov", DateTime.Now);

            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }

        public int ABMProductos(string unCodProveedor, string unCodBarras, int unRubro, string unaDescripcion, int unProveedor, decimal unCosto,
                                decimal unPrecio, decimal unStock, decimal unaCantMinima, int unaAccion, int unProducto, decimal unPrecioProveedor, bool esFraccionado, bool esDolarizado, bool esPromocion)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ProductosABM";

                cmd.Parameters.AddWithValue("unCodProveedor", unCodProveedor);
                cmd.Parameters.AddWithValue("unCodBarras", unCodBarras);
                cmd.Parameters.AddWithValue("unRubro", unRubro);
                cmd.Parameters.AddWithValue("unaDescripcion", unaDescripcion);
                cmd.Parameters.AddWithValue("unProveedor", unProveedor);
                cmd.Parameters.AddWithValue("unCosto", unCosto);
                cmd.Parameters.AddWithValue("unPrecio", unPrecio);
                cmd.Parameters.AddWithValue("unStock", unStock);
                cmd.Parameters.AddWithValue("unaCMinima", unaCantMinima);
                cmd.Parameters.AddWithValue("unaAccion", unaAccion);
                cmd.Parameters.AddWithValue("unProducto", unProducto);
                cmd.Parameters.AddWithValue("unPrecioProveedor", unPrecioProveedor);
                cmd.Parameters.AddWithValue("esFraccionado", esFraccionado);
                cmd.Parameters.AddWithValue("esDolarizado", esDolarizado);
                cmd.Parameters.AddWithValue("esUnaPromocion", esPromocion);


                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteNonQuery(); 
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

        public DataTable traerProductosProveedores()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select p.codProveedor, ppr.precio * 1.10 from Productos as p inner join preciosProveedores as ppr on ppr.fk_producto = p.id where p.fk_proveedor = 7 and p.id > 385 order by p.id", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public List<ProductoResultadoBarras> BuscarProductosEtiquetasCodigosBarra(
                                                                                    int proveedorId,
                                                                                    int rubroId,
                                                                                    string tipoFiltro,
                                                                                    string valorFiltro)
        {
            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(
                    "sp_ProductosTraerProductosEtiquetasCodigosBarra",
                    instDatos.abrirConexion());

                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                string filtro = "";

                // 🔹 Filtro proveedor
                if (proveedorId != 0)
                    filtro += $" AND p.fk_proveedor = {proveedorId}";

                // 🔹 Filtro rubro
                if (rubroId != 0)
                    filtro += $" AND p.fk_Rubro = {rubroId}";

                // 🔹 Filtro por texto
                if (!string.IsNullOrWhiteSpace(valorFiltro))
                {
                    string valorSeguro = MySqlHelper.EscapeString(valorFiltro);

                    switch (tipoFiltro)
                    {
                        case "Cod. Proveedor":
                            filtro += $" AND p.codProveedor LIKE '%{valorSeguro}%'";
                            break;

                        case "Cod. Barras":
                            filtro += $" AND p.codBarras LIKE '%{valorSeguro}%'";
                            break;

                        case "Descripcion":
                            filtro += $" AND p.descripcion LIKE '%{valorSeguro}%'";
                            break;
                    }
                }

                filtro += " ORDER BY p.descripcion";

                da.SelectCommand.Parameters.AddWithValue("unFiltro", filtro);

                DataTable tabla = new DataTable();
                da.Fill(tabla);

                List<ProductoResultadoBarras> lista = new List<ProductoResultadoBarras>();

                foreach (DataRow fila in tabla.Rows)
                {
                    ProductoResultadoBarras p = new ProductoResultadoBarras
                    {
                        Id = Convert.ToInt32(fila["id"]),
                        CodProveedor = fila["codProveedor"]?.ToString(),
                        CodBarras = fila["codBarras"]?.ToString(),
                        Descripcion = fila["descripcion"]?.ToString(),
                        Precio = Convert.ToDecimal(fila["precio"]),
                        NombreComercial = fila["nombreComercial"]?.ToString(),
                        Rubro = fila["Rubro"]?.ToString(),
                        Sel = false
                    };

                    lista.Add(p);
                }

                return lista;
            }
            catch
            {

                return null;
            }
        }

    }

    public class ProductoResultadoBarras
    {
        public bool Sel { get; set; }
        public int Id { get; set; }
        public string CodProveedor { get; set; }
        public string CodBarras { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string NombreComercial { get; set; }
        public string Rubro { get; set; }

        
    }
}
