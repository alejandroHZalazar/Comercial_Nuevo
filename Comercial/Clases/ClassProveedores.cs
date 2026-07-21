using MySqlConnector;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace Comercial.Clases
{
    class ClassProveedores
    {
        classDatos instDatos = new classDatos();

        public DataTable traeProveedores()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id, nombreComercial from Proveedores order by nombreComercial", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traeProveedoresconTodos()
        {
            string sql = @"
                        SELECT 0 AS id, 'TODOS' AS nombreComercial
                        UNION ALL
                        SELECT id, nombreComercial 
                        FROM Proveedores
                        ORDER BY id = 0 DESC, nombreComercial";

            MySqlDataAdapter rows = new MySqlDataAdapter(sql, instDatos.abrirConexion());

            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();

            return dt;
        }
        public DataTable traerPedidosPendientes(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProveedoresTraerNotaPedidoPendientes", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro );

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerListaProdAPedir(int unProveedor, DateTime unDesde, DateTime unHasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProveedoresListarProductosAPedir", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unProveedor", unProveedor );
            a1.SelectCommand.Parameters.AddWithValue("desde", unDesde );
            a1.SelectCommand.Parameters.AddWithValue("hasta", unHasta);
            a1.SelectCommand.CommandTimeout = 0;

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerOrdenCompra(long idOrden)
        {
            string sql = @"SELECT lpad(o.id, 8, 0) AS id, o.fecha, o.total, o.iva, o.recargo, o.descuento,
                                  d.codBarras, d.codProveedor, d.descripcion, d.precioProveedor, d.cantidad, d.subtotal,
                                  p.imagen, pr.nombreComercial, pr.direccion
                           FROM ordenCompra o
                           INNER JOIN ordenCompraDetalle d ON o.id = d.fk_ordenCompra
                           INNER JOIN parametros p ON p.modulo = 'login' AND p.parametro = 'imagen'
                           INNER JOIN Proveedores pr ON pr.id = o.fk_proveedor
                           WHERE o.id = @idOrden";
            MySqlDataAdapter a1 = new MySqlDataAdapter(sql, instDatos.abrirConexion());
            a1.SelectCommand.Parameters.AddWithValue("@idOrden", idOrden);
            DataTable t2 = new DataTable();
            a1.Fill(t2);
            instDatos.cerrarConexion();
            return t2;
        }

        /// <summary>
        /// Devuelve los coeficientes ganancia/descuento a aplicar, resolviendo la modalidad
        /// en una sola consulta según Proveedores.preciosPorProducto:
        ///   - 1  → toma los valores de Productos (producto abierto).
        ///   - 0 / NULL → toma los valores del Proveedor (comportamiento actual).
        /// También devuelve la columna preciosPorProducto por si el llamador la necesita.
        /// </summary>
        public DataTable traerCoeficientes(int unProveedor, int unProducto)
        {
            string sql = @"select case when coalesce(pr.preciosPorProducto, 0) = 1 then p.ganancia  else pr.ganancia  end as ganancia,
                                  case when coalesce(pr.preciosPorProducto, 0) = 1 then p.descuento else pr.descuento end as descuento,
                                  coalesce(pr.preciosPorProducto, 0) as preciosPorProducto
                           from Proveedores pr
                           left join Productos p on p.id = @idProducto
                           where pr.id = @idProveedor";
            MySqlDataAdapter rows = new MySqlDataAdapter(sql, instDatos.abrirConexion());
            rows.SelectCommand.Parameters.AddWithValue("@idProducto", unProducto);
            rows.SelectCommand.Parameters.AddWithValue("@idProveedor", unProveedor);
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        /// <summary>
        /// Determina en una sola consulta si el proveedor administra precios por producto
        /// (Proveedores.preciosPorProducto = 1). NULL o 0 → false (precios por proveedor).
        /// </summary>
        public bool usaPreciosPorProducto(int idProveedor)
        {
            MySqlCommand cmd = new MySqlCommand(
                "select coalesce(preciosPorProducto, 0) from Proveedores where id = @id",
                instDatos.abrirConexion());
            cmd.Parameters.AddWithValue("@id", idProveedor);
            object res = cmd.ExecuteScalar();
            instDatos.cerrarConexion();
            return res != null && res != DBNull.Value && Convert.ToInt32(res) == 1;
        }

        public string traerNombreProveedor (int unId)
        {
            classDatos datos = new classDatos();
            MySqlCommand nComando = new MySqlCommand("select nombreComercial from Proveedores where id = " + unId , datos.abrirConexion());
            string valor = nComando.ExecuteScalar().ToString();
            datos.cerrarConexion();
            return valor;
        }


        public DataTable traerDetallePedidosPendientes (long unaNorta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProveedoresTraerDetalleNotaPedido", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unaNotaPedido", unaNorta );

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }



        public DataTable traeProveedoresPorId( string unId)
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from Proveedores where id = " + unId , instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traeProveedoresCabecera()
        {
            // preciosPorProducto (tinyint(1)) se devuelve crudo: el conector lo mapea a Boolean
            // y el DataGridView genera automáticamente una columna checkbox (NULL/0 = destildado).
            string sql = @"select id as 'Cod', nombreComercial as 'Proveedor', direccion as 'Direccion',
                                  preciosPorProducto as 'Precios por Producto'
                           from Proveedores where baja = 0 order by nombreComercial";
            MySqlDataAdapter rows = new MySqlDataAdapter(sql, instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traePorcentajeIVA()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from ivaPorcentajes", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerPorcentajeImpuestos()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from impuestos order by valor", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerProductosProveedor (int unProveedor)
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select descripcion from Productos where baja = 0 and fk_proveedor = " + unProveedor , instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traeCantMinPorProveedor(int unProveedor)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosCantMinimaPorProveedor", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unProveedor", unProveedor );

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public long ABMProveedores(int unId, string unNombreComercial, string unCuil, string unaDireccion, string unEmail, string unTel, string unCel, decimal unaGanacia, int unAccion, decimal unDescuento, bool? unPreciosPorProducto = null)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ProveedoresABM";

                cmd.Parameters.AddWithValue("unId", unId );
                cmd.Parameters.AddWithValue("unNombreComercial", unNombreComercial );
                cmd.Parameters.AddWithValue("unCuil", unCuil );
                cmd.Parameters.AddWithValue("unaDireccion", unaDireccion );
                cmd.Parameters.AddWithValue("unEmail", unEmail);
                cmd.Parameters.AddWithValue("unTel", unTel );
                cmd.Parameters.AddWithValue("unCel", unCel );
                cmd.Parameters.AddWithValue("unaGanancia", unaGanacia );
                cmd.Parameters.AddWithValue("unaAccion", unAccion);
                cmd.Parameters.AddWithValue("unDescuento", unDescuento );
                // NULL cuando no se especifica → el proveedor conserva la modalidad actual (por proveedor)
                cmd.Parameters.AddWithValue("unPreciosPorProducto",
                    unPreciosPorProducto.HasValue ? (object)(unPreciosPorProducto.Value ? 1 : 0) : DBNull.Value);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int64  );
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

        public void marcarlineaNotaPedido (string unaLinea)
        {
            MySqlCommand nComando = new MySqlCommand("update ordenCompraDetalle set procesado = 1 where linea = " + unaLinea, instDatos.abrirConexion());
            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }

        public void eliminarOrdenCompra(string unaOrden)
        {
            MySqlCommand nComando = new MySqlCommand("delete from ordenCompra where id = " + unaOrden, instDatos.abrirConexion());
            nComando.ExecuteNonQuery();
            instDatos.cerrarConexion();
        }

        public long insertOrdenCompraCabecera(int unProveedor, decimal unTotal, decimal unIva, decimal unRecargo, decimal unDescuento)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ProveedoresAltaCabeceraOrdenCompra";

                cmd.Parameters.AddWithValue("unProveedor", unProveedor );
                cmd.Parameters.AddWithValue("unTotal", unTotal);
                cmd.Parameters.AddWithValue("unIva", unIva );
                cmd.Parameters.AddWithValue("unRecargo", unRecargo );
                cmd.Parameters.AddWithValue("unDescuento", unDescuento);

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


        public long insertOrdenCompraDetalle(long unaOrdenCompra, int unProducto, string unCodBarras, string unCodProveedor, string unaDescripcion, decimal unPrecioProveedor, decimal unaCantidad, decimal unSubtotal)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ordenCompraDetalle";

                cmd.Parameters.AddWithValue("unaOrdenCompra", unaOrdenCompra );
                cmd.Parameters.AddWithValue("unProducto", unProducto );
                cmd.Parameters.AddWithValue("unCodBarras", unCodBarras );
                cmd.Parameters.AddWithValue("unCodProveedor", unCodProveedor);
                cmd.Parameters.AddWithValue("unaDescripcion", unaDescripcion);
                cmd.Parameters.AddWithValue("unPrecioProveedor", unPrecioProveedor  );
                cmd.Parameters.AddWithValue("unaCantidad", unaCantidad );
                cmd.Parameters.AddWithValue("unSubtotal", unSubtotal);
                

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

        public DataTable traerResumenPagos(DateTime desde, DateTime hasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProveedoresResumenPagos", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("desde", desde);
            a1.SelectCommand.Parameters.AddWithValue("hasta", hasta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public void ExportarPagosCsv(DataTable detalle)
        {

            string carpetaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            string archivo = "Pagos_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".csv";

            string path = Path.Combine(carpetaDescargas, archivo);

            var sb = new StringBuilder();


            // Columnas
            sb.AppendLine("Proveedor;Fecha;Importe;Observaciones");

            // Detalle
            foreach (DataRow row in detalle.Rows)
            {
                sb.AppendLine($"{row["Proveedor"]};{row["Fecha"]};{row["Importe"]};{row["Observaciones"]}");
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
