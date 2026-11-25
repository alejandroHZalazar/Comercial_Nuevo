using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace Comercial.Clases
{
    class ClassProductos
    {
        classDatos instDatos = new classDatos();

        // ── Variantes de producto (modelo LitoralMarket / Comercial Web) ─────────────
        // Ficha = fila de productosPrincipales. Solo se aplica con parametros(ecommerce, HabilitarVariantesProducto)='1'
        // y la tabla existente; con el parámetro apagado no se consulta ninguna tabla de variantes.
        private static bool _variantesCalculado;
        private static bool _variantesHabilitado;
        private static string _variantesTabla;
        private static DateTime _variantesVence = DateTime.MinValue;
        private static readonly object _variantesLock = new object();

        /// <summary>true solo con el parámetro en '1' y la tabla productosPrincipales existente (cacheado 60 s).</summary>
        public static bool variantesHabilitadas()
        {
            lock (_variantesLock)
            {
                if (_variantesCalculado && _variantesVence > DateTime.UtcNow) return _variantesHabilitado;

                bool habilitado = false;
                string tabla = null;
                classDatos datos = new classDatos();
                try
                {
                    MySqlCommand cmd = new MySqlCommand("select valor from parametros where modulo = 'ecommerce' and parametro = 'HabilitarVariantesProducto'", datos.abrirConexion());
                    object valor = cmd.ExecuteScalar();
                    if (valor != null && valor != DBNull.Value && valor.ToString().Trim() == "1")
                    {
                        // Nombre exacto tomado de la base (en Linux MySQL distingue mayúsculas en nombres de tabla)
                        MySqlCommand cmdT = new MySqlCommand("select TABLE_NAME from information_schema.TABLES where TABLE_SCHEMA = DATABASE() and LOWER(TABLE_NAME) = 'productosprincipales'", datos.abrirConexion());
                        object t = cmdT.ExecuteScalar();
                        if (t != null && t != DBNull.Value)
                        {
                            tabla = t.ToString();
                            habilitado = true;
                        }
                    }
                }
                catch
                {
                    habilitado = false;
                    tabla = null;
                }
                finally
                {
                    datos.cerrarConexion();
                }

                _variantesHabilitado = habilitado;
                _variantesTabla = tabla;
                _variantesCalculado = true;
                _variantesVence = DateTime.UtcNow.AddSeconds(60);
                return habilitado;
            }
        }

        /// <summary>Condición SQL (empieza con " and ") que excluye las fichas; cadena vacía si las variantes no están habilitadas.</summary>
        public static string condicionSinFichas(string alias)
        {
            if (!variantesHabilitadas()) return string.Empty;
            return " and not exists(select 1 from `" + _variantesTabla.Replace("`", "``") + "` f where f.fk_producto = " + alias + ".id)";
        }

        /// <summary>Ids (de la lista dada) que son fichas principales. Vacío si las variantes no están habilitadas.</summary>
        public static HashSet<int> fichasPrincipales(IEnumerable<int> ids)
        {
            HashSet<int> resultado = new HashSet<int>();
            List<int> lista = new List<int>();
            foreach (int i in ids) if (i > 0 && !lista.Contains(i)) lista.Add(i);
            if (lista.Count == 0 || !variantesHabilitadas()) return resultado;

            classDatos datos = new classDatos();
            try
            {
                MySqlCommand cmd = new MySqlCommand("select fk_producto from `" + _variantesTabla.Replace("`", "``") + "` where fk_producto in (" + string.Join(",", lista) + ")", datos.abrirConexion());
                using (MySqlDataReader rd = cmd.ExecuteReader())
                    while (rd.Read()) resultado.Add(Convert.ToInt32(rd[0]));
            }
            finally
            {
                datos.cerrarConexion();
            }
            return resultado;
        }

        /// <summary>
        /// Lanza InvalidOperationException si alguno de los productos es una ficha principal
        /// (no se pueden vender ni pedir fichas, solo sus variantes). No hace nada con variantes apagadas.
        /// </summary>
        public static void validarNoSonFichas(IEnumerable<int> ids)
        {
            HashSet<int> fichas = fichasPrincipales(ids);
            if (fichas.Count == 0) return;

            List<string> nombres = new List<string>();
            classDatos datos = new classDatos();
            try
            {
                MySqlCommand cmd = new MySqlCommand("select descripcion from Productos where id in (" + string.Join(",", fichas) + ")", datos.abrirConexion());
                using (MySqlDataReader rd = cmd.ExecuteReader())
                    while (rd.Read()) nombres.Add("'" + rd[0].ToString() + "'");
            }
            finally
            {
                datos.cerrarConexion();
            }
            throw new InvalidOperationException("No se pueden vender ni pedir fichas principales: " + string.Join(", ", nombres) +
                                                ". Seleccione una de sus variantes (medida/color).");
        }

        /// <summary>
        /// Quita de un DataTable las filas cuya columna de id corresponde a una ficha principal.
        /// Para SP que no reciben filtro (listas por proveedor). No hace nada con variantes apagadas.
        /// </summary>
        public static DataTable quitarFichas(DataTable tabla, string columnaId)
        {
            if (tabla == null || tabla.Rows.Count == 0 || !tabla.Columns.Contains(columnaId) || !variantesHabilitadas()) return tabla;

            List<int> ids = new List<int>();
            foreach (DataRow fila in tabla.Rows)
            {
                int id;
                if (int.TryParse(fila[columnaId].ToString(), out id)) ids.Add(id);
            }
            HashSet<int> fichas = fichasPrincipales(ids);
            if (fichas.Count == 0) return tabla;

            for (int i = tabla.Rows.Count - 1; i >= 0; i--)
            {
                int id;
                if (int.TryParse(tabla.Rows[i][columnaId].ToString(), out id) && fichas.Contains(id))
                    tabla.Rows.RemoveAt(i);
            }
            tabla.AcceptChanges();
            return tabla;
        }

        /// <summary>
        /// Listado del ABM de productos con variantes habilitadas: una fila por producto principal.
        /// Las variantes encontradas se reemplazan por su ficha (se agrega la ficha si no estaba) y cada ficha
        /// se marca con su cantidad de variantes habilitadas (activa = 1). Sin variantes habilitadas devuelve
        /// la tabla sin cambios y no consulta ninguna tabla de variantes.
        /// </summary>
        public DataTable resolverListadoVariantes(DataTable productos, out Dictionary<int, int> variantesPorFicha)
        {
            variantesPorFicha = new Dictionary<int, int>();
            if (productos == null || productos.Rows.Count == 0 || !variantesHabilitadas()) return productos;

            classDatos datos = new classDatos();
            try
            {
                string tPrincipales = "`" + _variantesTabla.Replace("`", "``") + "`";
                MySqlConnection cn = datos.abrirConexion();

                // Nombre exacto de productosVariantes tomado de la base
                string tVar;
                using (MySqlCommand c = new MySqlCommand("select TABLE_NAME from information_schema.TABLES where TABLE_SCHEMA = DATABASE() and LOWER(TABLE_NAME) = 'productosvariantes'", cn))
                {
                    object o = c.ExecuteScalar();
                    if (o == null || o == DBNull.Value) return productos;
                    tVar = "`" + o.ToString().Replace("`", "``") + "`";
                }

                List<int> ids = new List<int>();
                foreach (DataRow r in productos.Rows) ids.Add(Convert.ToInt32(r["id"]));

                // 1) ¿cuáles de los encontrados son variantes? (y de qué ficha)
                HashSet<int> idsVariantes = new HashSet<int>();
                HashSet<int> fichasDeVariantes = new HashSet<int>();
                using (MySqlCommand c = new MySqlCommand("select fk_producto, fk_principal from " + tVar + " where fk_producto in (" + string.Join(",", ids) + ")", cn))
                using (MySqlDataReader rd = c.ExecuteReader())
                    while (rd.Read())
                    {
                        idsVariantes.Add(Convert.ToInt32(rd[0]));
                        fichasDeVariantes.Add(Convert.ToInt32(rd[1]));
                    }

                // 2) Las variantes se ven por su ficha: se quitan y se agrega la ficha si no estaba
                DataTable resultado = productos.Clone();
                HashSet<int> yaEstan = new HashSet<int>();
                foreach (DataRow r in productos.Rows)
                {
                    int id = Convert.ToInt32(r["id"]);
                    if (idsVariantes.Contains(id)) continue;
                    resultado.ImportRow(r);
                    yaEstan.Add(id);
                }

                List<int> faltan = new List<int>();
                foreach (int f in fichasDeVariantes) if (!yaEstan.Contains(f)) faltan.Add(f);
                if (faltan.Count > 0)
                {
                    DataTable fichasExtra = traeProductosPpal(" where p.baja = 0 and p.id in (" + string.Join(",", faltan) + ")", true);
                    foreach (DataRow r in fichasExtra.Rows)
                    {
                        resultado.ImportRow(r);
                        yaEstan.Add(Convert.ToInt32(r["id"]));
                    }
                }

                // 3) Fichas del listado con su cantidad de variantes habilitadas
                List<int> idsLista = new List<int>(yaEstan);
                if (idsLista.Count > 0)
                {
                    List<int> fichas = new List<int>();
                    using (MySqlCommand c = new MySqlCommand("select fk_producto from " + tPrincipales + " where fk_producto in (" + string.Join(",", idsLista) + ")", cn))
                    using (MySqlDataReader rd = c.ExecuteReader())
                        while (rd.Read()) fichas.Add(Convert.ToInt32(rd[0]));

                    if (fichas.Count > 0)
                    {
                        Dictionary<int, int> habilitadas = new Dictionary<int, int>();
                        using (MySqlCommand c = new MySqlCommand("select fk_principal, count(*) from " + tVar + " where fk_principal in (" + string.Join(",", fichas) + ") and activa = 1 group by fk_principal", cn))
                        using (MySqlDataReader rd = c.ExecuteReader())
                            while (rd.Read()) habilitadas[Convert.ToInt32(rd[0])] = Convert.ToInt32(rd[1]);

                        foreach (int f in fichas)
                        {
                            int n;
                            variantesPorFicha[f] = habilitadas.TryGetValue(f, out n) ? n : 0;
                        }
                    }
                }

                if (faltan.Count > 0 && resultado.Columns.Contains("descripcion"))
                {
                    resultado.DefaultView.Sort = "descripcion";
                    resultado = resultado.DefaultView.ToTable();
                }
                return resultado;
            }
            catch
            {
                // Ante cualquier problema con las tablas de variantes se conserva el listado original
                variantesPorFicha = new Dictionary<int, int>();
                return productos;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        /// <summary>
        /// Agrega la exclusión de fichas a un filtro de sp_ProductosConsultaPpal (alias p).
        /// Respeta un where existente (lo encierra entre paréntesis) y un order by / limit final.
        /// </summary>
        private static string filtroPpalSinFichas(string filtro)
        {
            string condicion = condicionSinFichas("p");
            if (condicion == string.Empty) return filtro;

            string cuerpo = filtro ?? string.Empty;
            string cola = string.Empty;

            // Cola final (order by / limit) que debe quedar después del where
            // (se toma la última aparición: la cola real va al final del filtro)
            System.Text.RegularExpressions.Match mc = System.Text.RegularExpressions.Regex.Match(
                cuerpo, @"\s(order\s+by|limit)\s", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.RightToLeft);
            if (mc.Success)
            {
                cola = cuerpo.Substring(mc.Index);
                cuerpo = cuerpo.Substring(0, mc.Index);
            }

            System.Text.RegularExpressions.Match w = System.Text.RegularExpressions.Regex.Match(
                cuerpo, @"\bwhere\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (w.Success)
            {
                string antes = cuerpo.Substring(0, w.Index);
                string condiciones = cuerpo.Substring(w.Index + w.Length);
                cuerpo = antes + " where (" + condiciones + ")" + condicion;
            }
            else
            {
                cuerpo = cuerpo + " where 1 = 1" + condicion;
            }

            return cuerpo + cola;
        }

        /// <summary>
        /// Agrega la exclusión de fichas (alias p) al final de un filtro recibido por un SP.
        /// conWhere = true: el SP concatena el filtro tal cual (el filtro trae su propio where, o está vacío).
        /// conWhere = false: el SP ya trae un where propio y el filtro son condiciones.
        /// Sin variantes habilitadas devuelve el filtro sin cambios.
        /// </summary>
        private static string agregarSinFichas(string filtro, bool conWhere)
        {
            string condicion = condicionSinFichas("p");
            if (condicion == string.Empty) return filtro;
            if (conWhere && string.IsNullOrWhiteSpace(filtro)) return " where 1 = 1" + condicion;
            return filtro + condicion;
        }

        /// <param name="incluirFichas">true solo para el ABM de productos, donde la ficha principal sí debe verse.</param>
        public DataTable traeProductosPpal(string unFiltro, bool incluirFichas = false)
        {
            if (!incluirFichas) unFiltro = filtroPpalSinFichas(unFiltro);

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
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", agregarSinFichas(unFiltro, true));

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traeMovimientoProductos(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosTraerMovimientos", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", agregarSinFichas(unFiltro, false));

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traeProductosStock(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosTraerStock", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", agregarSinFichas(unFiltro, false));

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerListaPrecios(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ProductosListaPrecios", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", agregarSinFichas(unFiltro, true));

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


        /// <summary>
        /// Lee como booleano un campo de Productos (fraccionado, dolarizado, esPromocion...) tal como lo devuelve la base:
        /// bool, 0/1 (numérico o texto), "True"/"False" o NULL. NULL = false, el mismo criterio que ya aplican
        /// sp_ProductosTraerParaEditar (IFNULL(p.fraccionado, 0)) y sp_pedidosTraerPendientesDetalle. Nunca modifica datos.
        /// </summary>
        public static bool esVerdadero(object valor)
        {
            if (valor == null || valor == DBNull.Value) return false;
            if (valor is bool) return (bool)valor;

            string t = valor.ToString().Trim();
            if (t.Length == 0 || t == "0") return false;
            if (t == "1") return true;

            bool b;
            return bool.TryParse(t, out b) && b;
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

        /// <summary>
        /// Devuelve los porcentajes ganancia/descuento del producto y el flag
        /// preciosPorProducto del proveedor asociado. Se usa solo para visualizar
        /// esos valores en frmConsultaProductos cuando el proveedor administra
        /// precios por producto (Proveedores.preciosPorProducto = 1).
        /// </summary>
        public DataTable traerConfigPreciosPorProducto(int unProducto)
        {
            string sql = @"select p.ganancia, p.descuento,
                                  coalesce(pr.preciosPorProducto, 0) as preciosPorProducto
                           from Productos p
                           left join Proveedores pr on pr.id = p.fk_proveedor
                           where p.id = @id";
            MySqlDataAdapter a1 = new MySqlDataAdapter(sql, instDatos.abrirConexion());
            a1.SelectCommand.Parameters.AddWithValue("@id", unProducto);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            instDatos.cerrarConexion();
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
            // Con el filtro vacío se conserva el comportamiento actual (no se agrega nada)
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", string.IsNullOrWhiteSpace(unFiltro) ? unFiltro : agregarSinFichas(unFiltro, false));

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

        public void actualziarMasivaPrecios(string unProducto, decimal unPrecio, string unaDescripcion, string unCodBarras, int unProveedor,
                                            decimal? unDescuento = null, decimal? unaGanancia = null)
        {
            MySqlCommand nComando = new MySqlCommand("sp_Productos_CambiarPreciosMasivos", instDatos.abrirConexion());
            nComando.CommandType = CommandType.StoredProcedure;

            nComando.Parameters.AddWithValue("@unProducto", unProducto);
            nComando.Parameters.AddWithValue("@unPrecio", unPrecio);
            nComando.Parameters.AddWithValue("@unCodBarras", unCodBarras);
            nComando.Parameters.AddWithValue("@unaDescripcion", unaDescripcion);
            nComando.Parameters.AddWithValue("@unProveedor", unProveedor);
            // NULL cuando el proveedor NO usa precios por producto → se guardan como NULL en ProductosACrear
            nComando.Parameters.AddWithValue("@unDescuento", unDescuento.HasValue ? (object)unDescuento.Value : DBNull.Value);
            nComando.Parameters.AddWithValue("@unaGanancia", unaGanancia.HasValue ? (object)unaGanancia.Value : DBNull.Value);

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
                                decimal unPrecio, decimal unStock, decimal unaCantMinima, int unaAccion, int unProducto, decimal unPrecioProveedor, bool esFraccionado, bool esDolarizado, bool esPromocion,
                                decimal? unGanancia = null, decimal? unDescuento = null, decimal? unaCantidadMinimaVenta = null)
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
                // NULL cuando el proveedor NO usa precios por producto → el SP no altera esos campos
                cmd.Parameters.AddWithValue("unGanancia", unGanancia.HasValue ? (object)unGanancia.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("unDescuento", unDescuento.HasValue ? (object)unDescuento.Value : DBNull.Value);
                // NULL cuando no se envía → el SP setea el campo en 1 por defecto
                cmd.Parameters.AddWithValue("unaCantidadMinimaVenta", unaCantidadMinimaVenta.HasValue ? (object)unaCantidadMinimaVenta.Value : DBNull.Value);


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

        /// <summary>
        /// Alta de un producto simple sin códigos: la base reserva el código y lo graba como código de barras y de
        /// proveedor en una sola transacción. Devuelve el id (&lt;= 0 = no se grabó; ver mensaje).
        /// </summary>
        public int altaConCodigoAutomatico(int unRubro, string unaDescripcion, int unProveedor, decimal unCosto,
                                           decimal unPrecio, decimal unStock, decimal unaCantMinima, decimal unPrecioProveedor,
                                           bool esFraccionado, bool esDolarizado, bool esPromocion,
                                           decimal? unGanancia, decimal? unDescuento, decimal? unaCantidadMinimaVenta,
                                           out string codigo, out string mensaje)
        {
            codigo = null;
            mensaje = string.Empty;

            if (!ClassCodigosBarras.procedimientosInstalados())
            {
                mensaje = ClassCodigosBarras.FaltaScript;
                return -1;
            }

            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_ProductosAltaCodigoAutomatico", instDatos.abrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("unRubro", unRubro);
                cmd.Parameters.AddWithValue("unaDescripcion", unaDescripcion);
                cmd.Parameters.AddWithValue("unProveedor", unProveedor);
                cmd.Parameters.AddWithValue("unCosto", unCosto);
                cmd.Parameters.AddWithValue("unPrecio", unPrecio);
                cmd.Parameters.AddWithValue("unStock", unStock);
                cmd.Parameters.AddWithValue("unaCMinima", unaCantMinima);
                cmd.Parameters.AddWithValue("unPrecioProveedor", unPrecioProveedor);
                cmd.Parameters.AddWithValue("esFraccionado", esFraccionado);
                cmd.Parameters.AddWithValue("esDolarizado", esDolarizado);
                cmd.Parameters.AddWithValue("esUnaPromocion", esPromocion);
                cmd.Parameters.AddWithValue("unGanancia", unGanancia.HasValue ? (object)unGanancia.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("unDescuento", unDescuento.HasValue ? (object)unDescuento.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("unaCantidadMinimaVenta", unaCantidadMinimaVenta.HasValue ? (object)unaCantidadMinimaVenta.Value : DBNull.Value);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                MySqlParameter salidaCodigo = new MySqlParameter("unCodigo", MySqlDbType.VarChar, 45) { Direction = ParameterDirection.Output };
                MySqlParameter salidaMensaje = new MySqlParameter("mensaje", MySqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(salida);
                cmd.Parameters.Add(salidaCodigo);
                cmd.Parameters.Add(salidaMensaje);

                cmd.ExecuteNonQuery();

                codigo = salidaCodigo.Value as string;
                mensaje = salidaMensaje.Value as string ?? string.Empty;
                return Convert.ToInt32(salida.Value);
            }
            catch (MySqlException ex)
            {
                mensaje = ex.Message;
                return -1;
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
                // Incluye el rubro seleccionado y todos sus subrubros
                if (rubroId != 0)
                    filtro += " AND " + ClassConfiguracion.filtroRubro("p.fk_Rubro", rubroId);

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

                // Variantes: nunca la ficha principal (vacío si las variantes no están habilitadas)
                filtro += condicionSinFichas("p");

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
        // Cantidad de etiquetas a imprimir de este producto (entero 1..500)
        public int Cantidad { get; set; } = 1;

        
    }
}
