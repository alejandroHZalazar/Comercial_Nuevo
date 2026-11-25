using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Comercial.Clases
{
    /// <summary>
    /// Promociones por cantidad (NxM) y por porcentaje de descuento. Port del PromocionDescuentosService de Comercial Web:
    /// mismas tablas (promociones_reglas, promociones_reglas_productos, ventas_promo_detalle, pedidos_promo_detalle),
    /// mismo parámetro de habilitación (promociones/descuentos = '1') y mismas reglas de negocio.
    /// Sin el parámetro en '1' o sin las tablas, no se consulta nada y todo queda como antes.
    /// </summary>
    class ClassPromocionesDescuentos
    {
        static readonly CultureInfo Ar = new CultureInfo("es-AR");

        classDatos instDatos = new classDatos();

        // ── Modelos ───────────────────────────────────────────────────────────
        public class ReglaDetalle
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Tipo { get; set; } = TipoPromo.Porcentaje;
            public decimal? Porcentaje { get; set; }
            public int? Lleva { get; set; }
            public int? Paga { get; set; }
            public DateTime Desde { get; set; }
            public DateTime Hasta { get; set; }
            public bool Activa { get; set; } = true;
            /// <summary>Solo NxM: acumula las variantes de una misma ficha (requiere la columna agrupaVariantes).</summary>
            public bool AgrupaVariantes { get; set; }
            public List<int> ProductoIds { get; set; } = new List<int>();
        }

        public class AvisoResumen
        {
            public int FkProducto { get; set; }
            public string Descripcion { get; set; }
            public string Texto { get; set; }
        }

        public class AvisoProxima
        {
            public int Clave { get; set; }
            public int FkProducto { get; set; }
            public int FkRegla { get; set; }
            public decimal CantidadActual { get; set; }
            public string Descripcion { get; set; }
            public string Texto { get; set; }
            public string TextoCorto { get; set; }
        }

        public class ResultadoEvaluacion
        {
            public List<LineaPromoResultado> Lineas { get; set; } = new List<LineaPromoResultado>();
            public List<AvisoResumen> Resumen { get; set; } = new List<AvisoResumen>();
            public List<AvisoProxima> Proximas { get; set; } = new List<AvisoProxima>();
        }

        // Datos de una línea al grabar
        public class PromoGuardarLinea
        {
            public int Indice { get; set; }
            public int FkProducto { get; set; }
            public decimal Cantidad { get; set; }
            public decimal PrecioSinIva { get; set; }
            public decimal DescRec { get; set; }          // negativo = descuento (como la columna Desc/Rec)
            public int? FkPromoRegla { get; set; }        // regla que la pantalla declara haber aplicado
            public bool Excluida { get; set; }            // fraccionado, de pedido, bloqueada
        }

        public class PromoAplicada
        {
            public int FkRegla { get; set; }
            public string Nombre { get; set; }
            public string Tipo { get; set; }
            public decimal? Porcentaje { get; set; }
            public int? Lleva { get; set; }
            public int? Paga { get; set; }
            public decimal CantBonificada { get; set; }
            public decimal Ahorro { get; set; }
        }

        // Cómo debe quedar grabada una línea (una línea original puede dar 1 o 2 segmentos)
        public class PromoGuardarSegmento
        {
            public int Indice { get; set; }
            public decimal Cantidad { get; set; }
            public decimal DescuentoPct { get; set; }
            public bool Bonificada { get; set; }
            public PromoAplicada Promo { get; set; }
        }

        // ── Estado / parámetro ────────────────────────────────────────────────
        private static bool _calculado;
        private static bool _habilitado;
        private static bool _tablas;
        private static bool _columnaAgrupa;     // existe promociones_reglas.agrupaVariantes (scripts/promociones_reglas_agrupa_variantes.sql)
        private static DateTime _vence = DateTime.MinValue;
        private static readonly object _lock = new object();

        private static void calcularEstado()
        {
            lock (_lock)
            {
                if (_calculado && _vence > DateTime.UtcNow) return;

                bool tablas = false;
                bool habilitado = false;
                bool columnaAgrupa = false;
                classDatos datos = new classDatos();
                try
                {
                    MySqlConnection cn = datos.abrirConexion();
                    using (MySqlCommand c = new MySqlCommand(
                        "select count(*) from information_schema.TABLES where TABLE_SCHEMA = DATABASE() and LOWER(TABLE_NAME) in " +
                        "('promociones_reglas','promociones_reglas_productos','ventas_promo_detalle','pedidos_promo_detalle')", cn))
                        tablas = Convert.ToInt32(c.ExecuteScalar()) == 4;

                    if (tablas)
                    {
                        using (MySqlCommand c = new MySqlCommand(
                            "select count(*) from information_schema.COLUMNS where TABLE_SCHEMA = DATABASE() " +
                            "and LOWER(TABLE_NAME) = 'promociones_reglas' and COLUMN_NAME = 'agrupaVariantes'", cn))
                            columnaAgrupa = Convert.ToInt32(c.ExecuteScalar()) > 0;
                    }

                    if (tablas)
                    {
                        using (MySqlCommand c = new MySqlCommand("select valor from parametros where modulo = 'promociones' and parametro = 'descuentos'", cn))
                        {
                            object v = c.ExecuteScalar();
                            habilitado = v != null && v != DBNull.Value && v.ToString().Trim() == "1";
                        }
                    }
                }
                catch
                {
                    tablas = false;
                    habilitado = false;
                    columnaAgrupa = false;
                }
                finally
                {
                    datos.cerrarConexion();
                }

                _tablas = tablas;
                _habilitado = habilitado;
                _columnaAgrupa = columnaAgrupa;
                _calculado = true;
                _vence = DateTime.UtcNow.AddSeconds(60);
            }
        }

        /// <summary>Parámetro promociones/descuentos en '1' Y las 4 tablas existentes (cacheado 60 s).</summary>
        public static bool habilitado()
        {
            calcularEstado();
            return _habilitado;
        }

        /// <summary>
        /// Existe la columna promociones_reglas.agrupaVariantes (se crea con scripts/promociones_reglas_agrupa_variantes.sql).
        /// Sin ella no se lee ni se guarda la opción y todo funciona como antes.
        /// </summary>
        public static bool agrupaVariantesDisponible()
        {
            calcularEstado();
            return _tablas && _columnaAgrupa;
        }

        /// <summary>Las 4 tablas existen (sirve para el ABM aunque el parámetro esté apagado).</summary>
        public static bool tablasListas()
        {
            calcularEstado();
            return _tablas;
        }

        /// <summary>Hora local de Argentina (la vigencia se guarda en esa hora).</summary>
        public static DateTime ahoraArgentina()
        {
            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time"));
            }
            catch
            {
                return DateTime.Now;
            }
        }

        private DataTable consulta(string sql, params MySqlParameter[] parametros)
        {
            try
            {
                MySqlDataAdapter a = new MySqlDataAdapter(sql, instDatos.abrirConexion());
                if (parametros != null) a.SelectCommand.Parameters.AddRange(parametros);
                DataTable t = new DataTable();
                a.Fill(t);
                return t;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        private static string lista(IEnumerable<int> ids)
        {
            return string.Join(",", ids.Select(i => i.ToString(CultureInfo.InvariantCulture)));
        }

        // ── Opción "acumular variantes de una misma ficha" ────────────────────
        // Sin la columna (script sin ejecutar) no se la nombra en ninguna consulta: todo queda como antes.
        private static string columnaAgrupaSql()
        {
            return agrupaVariantesDisponible() ? ", agrupaVariantes" : string.Empty;
        }

        private static bool leerAgrupa(DataRow r)
        {
            return r.Table.Columns.Contains("agrupaVariantes") && r["agrupaVariantes"] != DBNull.Value &&
                   Convert.ToInt32(r["agrupaVariantes"]) == 1;
        }

        // Id de la ficha de cada variante (productosVariantes.fk_producto -> fk_principal). 0 = no es variante.
        private Dictionary<int, int> fichasDeVariantes(List<int> productoIds)
        {
            Dictionary<int, int> d = new Dictionary<int, int>();
            if (productoIds.Count == 0) return d;
            DataTable t = consulta("select fk_producto, fk_principal from productosVariantes where fk_producto in (" + lista(productoIds) + ")");
            foreach (DataRow r in t.Rows) d[Convert.ToInt32(r["fk_producto"])] = Convert.ToInt32(r["fk_principal"]);
            return d;
        }

        // ── Reglas vigentes para un conjunto de productos ─────────────────────
        private List<ReglaPromo> reglasVigentes(List<int> productoIds, DateTime ahora)
        {
            if (productoIds.Count == 0) return new List<ReglaPromo>();

            DataTable asoc = consulta(
                "select rp.fk_regla, rp.fk_producto from promociones_reglas_productos rp " +
                "join promociones_reglas r on r.id = rp.fk_regla " +
                "where rp.fk_producto in (" + lista(productoIds) + ") and r.activa = 1 and r.vigenteDesde <= @ahora and r.vigenteHasta >= @ahora",
                new MySqlParameter("@ahora", ahora));
            if (asoc.Rows.Count == 0) return new List<ReglaPromo>();

            List<int> reglaIds = asoc.AsEnumerable().Select(r => Convert.ToInt32(r["fk_regla"])).Distinct().ToList();
            DataTable reglas = consulta(
                "select id, nombre, tipo, porcentaje, lleva, paga, vigenteDesde, vigenteHasta, activa" + columnaAgrupaSql() +
                " from promociones_reglas where id in (" + lista(reglaIds) + ")");

            List<ReglaPromo> res = new List<ReglaPromo>();
            foreach (DataRow r in reglas.Rows)
            {
                int id = Convert.ToInt32(r["id"]);
                res.Add(new ReglaPromo
                {
                    AgrupaVariantes = leerAgrupa(r),
                    Id = id,
                    Nombre = r["nombre"].ToString(),
                    Tipo = r["tipo"].ToString(),
                    Porcentaje = r["porcentaje"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r["porcentaje"]),
                    Lleva = r["lleva"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["lleva"]),
                    Paga = r["paga"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["paga"]),
                    Desde = Convert.ToDateTime(r["vigenteDesde"]),
                    Hasta = Convert.ToDateTime(r["vigenteHasta"]),
                    Activa = Convert.ToInt32(r["activa"]) == 1,
                    ProductoIds = new HashSet<int>(asoc.AsEnumerable().Where(a => Convert.ToInt32(a["fk_regla"]) == id).Select(a => Convert.ToInt32(a["fk_producto"])))
                });
            }
            return res;
        }

        // Los productos fraccionados (importe por peso/medida) quedan fuera del motor: su importe es libre.
        private HashSet<int> fraccionados(List<int> productoIds)
        {
            if (productoIds.Count == 0) return new HashSet<int>();
            DataTable t = consulta("select id from Productos where id in (" + lista(productoIds) + ") and IFNULL(fraccionado, 0) = 1");
            return new HashSet<int>(t.AsEnumerable().Select(r => Convert.ToInt32(r["id"])));
        }

        // Líneas válidas (sin fraccionados) + reglas vigentes: una sola carga que usan Evaluar y Proximas.
        private void preparar(IEnumerable<LineaPromoEntrada> entrada, out List<LineaPromoEntrada> validas, out List<ReglaPromo> reglas, out DateTime ahora)
        {
            ahora = ahoraArgentina();
            reglas = new List<ReglaPromo>();
            List<LineaPromoEntrada> lineas = entrada.Where(l => l.Cantidad > 0 && l.PrecioSinIva > 0 && l.ProductoId > 0).ToList();
            validas = new List<LineaPromoEntrada>();
            if (lineas.Count == 0) return;

            HashSet<int> fracc = fraccionados(lineas.Select(l => l.ProductoId).Distinct().ToList());
            validas = lineas.Where(l => !fracc.Contains(l.ProductoId)).ToList();
            if (validas.Count == 0) return;

            reglas = reglasVigentes(validas.Select(l => l.ProductoId).Distinct().ToList(), ahora);

            // Solo si alguna regla vigente acumula variantes (y las variantes están habilitadas) se consulta a qué ficha
            // pertenece cada producto; en cualquier otro caso no se toca ninguna tabla de variantes.
            if (reglas.Any(r => r.AcumulaPorFicha) && ClassProductos.variantesHabilitadas())
            {
                Dictionary<int, int> fichas = fichasDeVariantes(validas.Select(l => l.ProductoId).Distinct().ToList());
                foreach (LineaPromoEntrada l in validas)
                {
                    int ficha;
                    l.FichaId = fichas.TryGetValue(l.ProductoId, out ficha) ? ficha : 0;
                }
            }
        }

        // ── Evaluación desde la grilla ────────────────────────────────────────
        public ResultadoEvaluacion evaluar(IEnumerable<LineaPromoEntrada> lineas)
        {
            ResultadoEvaluacion resp = new ResultadoEvaluacion();
            if (!habilitado()) return resp;

            List<LineaPromoEntrada> validas; List<ReglaPromo> reglas; DateTime ahora;
            preparar(lineas, out validas, out reglas, out ahora);
            if (validas.Count == 0) return resp;

            List<LineaPromoResultado> res = MotorPromociones.Evaluar(validas, reglas, ahora);

            // Avisos de proximidad (informativos)
            List<LineaPromoProxima> proximas = MotorPromociones.Proximas(validas, reglas, ahora);
            if (proximas.Count > 0)
            {
                // Avisos por ficha: el nombre es el de la ficha y la unidad que falta puede ser de cualquiera de sus variantes
                Func<LineaPromoProxima, int> idNombre = p => p.EsGrupo ? p.FichaId : p.ProductoId;
                Dictionary<int, string> nomProx = nombres(proximas.Select(idNombre).Distinct().ToList());
                resp.Proximas = proximas.Select(p => new AvisoProxima
                {
                    Clave = p.Clave,
                    FkProducto = idNombre(p),
                    FkRegla = p.ReglaId,
                    CantidadActual = p.CantidadActual,
                    Descripcion = nomProx.ContainsKey(idNombre(p)) ? nomProx[idNombre(p)] : "",
                    Texto = "¡Aprovechá la promoción " + p.Nombre + "! Agregando 1 unidad más" +
                            (p.EsGrupo ? " (de cualquiera de las variantes)" : "") + ", llevás " + p.Lleva + " y pagás " + p.Paga + ".",
                    TextoCorto = "¡+1 y llevá " + p.Lleva + " pagando " + p.Paga + "!"
                }).ToList();
            }
            if (res.Count == 0) return resp;

            resp.Lineas = res;

            // Resumen para el usuario: una entrada por producto, solo con promociones realmente aplicadas
            Dictionary<int, string> nom = nombres(res.Select(r => r.ProductoId).Distinct().ToList());
            foreach (IGrouping<int, LineaPromoResultado> g in res.GroupBy(r => r.ProductoId))
            {
                LineaPromoResultado r0 = g.First();
                decimal ahorro = g.Sum(x => x.Ahorro);
                decimal bonif = g.Sum(x => x.CantBonificada);
                string texto = r0.Tipo == TipoPromo.NxM
                    ? "promoción " + r0.Lleva + "x" + r0.Paga + (g.Any(x => x.PorFicha) ? " acumulando variantes" : "") + " (" + bonif.ToString("0.####", Ar) + " unidad" + (bonif == 1m ? "" : "es") +
                      " bonificada" + (bonif == 1m ? "" : "s") + ", ahorro " + ahorro.ToString("C2", Ar) + " sin IVA)"
                    : r0.Porcentaje.Value.ToString("0.##", Ar) + "% de descuento (ahorro " + ahorro.ToString("C2", Ar) + " sin IVA)";

                resp.Resumen.Add(new AvisoResumen
                {
                    FkProducto = g.Key,
                    Descripcion = nom.ContainsKey(g.Key) ? nom[g.Key] : "",
                    Texto = texto
                });
            }
            return resp;
        }

        private Dictionary<int, string> nombres(List<int> ids)
        {
            Dictionary<int, string> d = new Dictionary<int, string>();
            if (ids.Count == 0) return d;
            DataTable t = consulta("select id, descripcion from Productos where id in (" + lista(ids) + ")");
            foreach (DataRow r in t.Rows) d[Convert.ToInt32(r["id"])] = r["descripcion"] == DBNull.Value ? "" : r["descripcion"].ToString();
            return d;
        }

        // ── Guardado: validación (manda el servidor/la capa de datos) + expansión NxM ──
        public const string MensajePromosCambiaron =
            "Las promociones vigentes cambiaron mientras se armaba la operación. " +
            "Se recalcularon los importes en pantalla: revíselos y vuelva a confirmar.";

        /// <summary>
        /// Re-evalúa las líneas al grabar. Si una línea declara una promoción distinta de la que corresponde (o ya no
        /// corresponde ninguna) lanza InvalidOperationException. Devuelve cómo grabar cada línea
        /// (PORC: 1 segmento con el %; NxM: unidades cobradas + unidades bonificadas al 100 %).
        /// Sin el módulo habilitado devuelve cada línea sin cambios.
        /// </summary>
        public List<PromoGuardarSegmento> resolverParaGuardar(IList<PromoGuardarLinea> lineas)
        {
            if (!habilitado())
                return lineas.Select(l => new PromoGuardarSegmento { Indice = l.Indice, Cantidad = l.Cantidad }).ToList();

            // Elegibles: no excluidas y sin descuento manual (descuento ≠ 0 sin promoción declarada = manual: gana el manual)
            List<PromoGuardarLinea> elegibles = lineas.Where(l => !l.Excluida && l.Cantidad > 0 && l.PrecioSinIva > 0 &&
                                                                  (l.DescRec == 0m || l.FkPromoRegla.HasValue)).ToList();

            List<LineaPromoEntrada> entrada = elegibles.Select(l => new LineaPromoEntrada
            {
                Clave = l.Indice, ProductoId = l.FkProducto, Cantidad = l.Cantidad, PrecioSinIva = l.PrecioSinIva
            }).ToList();

            List<LineaPromoEntrada> validas; List<ReglaPromo> reglas; DateTime ahora;
            preparar(entrada, out validas, out reglas, out ahora);
            List<LineaPromoResultado> res = validas.Count == 0 ? new List<LineaPromoResultado>() : MotorPromociones.Evaluar(validas, reglas, ahora);
            Dictionary<int, LineaPromoResultado> porIndice = res.ToDictionary(r => r.Clave);

            foreach (PromoGuardarLinea l in elegibles)
            {
                LineaPromoResultado esperado;
                if (!porIndice.TryGetValue(l.Indice, out esperado))
                {
                    if (l.FkPromoRegla.HasValue) throw new InvalidOperationException(MensajePromosCambiaron);   // ya no corresponde
                    continue;
                }
                // Corresponde promoción: la pantalla debe haber informado la misma, con el mismo descuento efectivo
                if (l.FkPromoRegla != esperado.ReglaId || Math.Abs(-esperado.DescuentoPct - l.DescRec) > 0.001m)
                    throw new InvalidOperationException(MensajePromosCambiaron);
            }

            List<PromoGuardarSegmento> segmentos = new List<PromoGuardarSegmento>();
            foreach (PromoGuardarLinea l in lineas)
            {
                LineaPromoResultado r;
                if (!porIndice.TryGetValue(l.Indice, out r) || !elegibles.Contains(l))
                {
                    segmentos.Add(new PromoGuardarSegmento { Indice = l.Indice, Cantidad = l.Cantidad });
                    continue;
                }

                if (r.Tipo == TipoPromo.Porcentaje)
                {
                    segmentos.Add(new PromoGuardarSegmento { Indice = l.Indice, Cantidad = l.Cantidad, DescuentoPct = r.DescuentoPct, Promo = aplicada(r) });
                }
                else   // NxM: cobradas (precio normal) + bonificadas (100 %)
                {
                    decimal bonif = r.CantBonificada;
                    decimal cobradas = l.Cantidad - bonif;
                    PromoAplicada promo = aplicada(r);
                    if (cobradas > 0m)
                        segmentos.Add(new PromoGuardarSegmento
                        {
                            Indice = l.Indice, Cantidad = cobradas, DescuentoPct = 0m,
                            Promo = new PromoAplicada
                            {
                                FkRegla = promo.FkRegla, Nombre = promo.Nombre, Tipo = promo.Tipo,
                                Lleva = promo.Lleva, Paga = promo.Paga, CantBonificada = bonif, Ahorro = 0m
                            }
                        });
                    segmentos.Add(new PromoGuardarSegmento { Indice = l.Indice, Cantidad = bonif, DescuentoPct = 100m, Bonificada = true, Promo = promo });
                }
            }
            return segmentos;
        }

        private static PromoAplicada aplicada(LineaPromoResultado r)
        {
            return new PromoAplicada
            {
                FkRegla = r.ReglaId, Nombre = r.Nombre, Tipo = r.Tipo, Porcentaje = r.Porcentaje,
                Lleva = r.Lleva, Paga = r.Paga, CantBonificada = r.CantBonificada, Ahorro = r.Ahorro
            };
        }

        // ── ABM ───────────────────────────────────────────────────────────────
        private static string resumen(string tipo, object pct, object lleva, object paga, bool agrupaVariantes = false)
        {
            if (tipo == TipoPromo.NxM) return lleva + "x" + paga + (agrupaVariantes ? " (acumula variantes)" : string.Empty);
            decimal p = pct == DBNull.Value || pct == null ? 0m : Convert.ToDecimal(pct);
            return p.ToString("0.##", Ar) + " % de descuento";
        }

        /// <summary>Lista de reglas: id, nombre, tipo, resumen, desde, hasta, activa, vigente, cantProductos.</summary>
        public DataTable listar()
        {
            DataTable salida = new DataTable();
            salida.Columns.Add("id", typeof(int));
            salida.Columns.Add("nombre", typeof(string));
            salida.Columns.Add("tipo", typeof(string));
            salida.Columns.Add("resumen", typeof(string));
            salida.Columns.Add("desde", typeof(DateTime));
            salida.Columns.Add("hasta", typeof(DateTime));
            salida.Columns.Add("activa", typeof(bool));
            salida.Columns.Add("vigente", typeof(bool));
            salida.Columns.Add("cantProductos", typeof(int));
            if (!tablasListas()) return salida;

            DataTable t = consulta(
                "select r.id, r.nombre, r.tipo, r.porcentaje, r.lleva, r.paga, r.vigenteDesde, r.vigenteHasta, r.activa, " +
                (agrupaVariantesDisponible() ? "r.agrupaVariantes, " : string.Empty) +
                "(select count(*) from promociones_reglas_productos p where p.fk_regla = r.id) as cant " +
                "from promociones_reglas r order by r.vigenteHasta desc, r.nombre");

            DateTime ahora = ahoraArgentina();
            foreach (DataRow r in t.Rows)
            {
                bool activa = Convert.ToInt32(r["activa"]) == 1;
                DateTime d = Convert.ToDateTime(r["vigenteDesde"]);
                DateTime h = Convert.ToDateTime(r["vigenteHasta"]);
                salida.Rows.Add(Convert.ToInt32(r["id"]), r["nombre"].ToString(), r["tipo"].ToString(),
                                resumen(r["tipo"].ToString(), r["porcentaje"], r["lleva"], r["paga"], leerAgrupa(r)),
                                d, h, activa, activa && d <= ahora && h >= ahora, Convert.ToInt32(r["cant"]));
            }
            return salida;
        }

        public ReglaDetalle obtener(int id)
        {
            if (!tablasListas()) return null;
            DataTable t = consulta("select id, nombre, tipo, porcentaje, lleva, paga, vigenteDesde, vigenteHasta, activa" + columnaAgrupaSql() +
                                   " from promociones_reglas where id = @id",
                                   new MySqlParameter("@id", id));
            if (t.Rows.Count == 0) return null;
            DataRow r = t.Rows[0];

            DataTable p = consulta("select fk_producto from promociones_reglas_productos where fk_regla = @id", new MySqlParameter("@id", id));
            return new ReglaDetalle
            {
                Id = Convert.ToInt32(r["id"]),
                Nombre = r["nombre"].ToString(),
                Tipo = r["tipo"].ToString(),
                Porcentaje = r["porcentaje"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r["porcentaje"]),
                Lleva = r["lleva"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["lleva"]),
                Paga = r["paga"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["paga"]),
                Desde = Convert.ToDateTime(r["vigenteDesde"]),
                Hasta = Convert.ToDateTime(r["vigenteHasta"]),
                Activa = Convert.ToInt32(r["activa"]) == 1,
                AgrupaVariantes = leerAgrupa(r),
                ProductoIds = p.AsEnumerable().Select(x => Convert.ToInt32(x["fk_producto"])).ToList()
            };
        }

        /// <summary>
        /// Alta (Id = 0) o modificación. Valida igual que la web (en MySQL 5.5 los CHECK no se aplican) y lanza
        /// InvalidOperationException con el mensaje a mostrar. Devuelve el id de la regla.
        /// </summary>
        public int guardar(ReglaDetalle dto, string usuario)
        {
            if (!tablasListas())
                throw new InvalidOperationException("Faltan crear las tablas de promociones (ejecute scripts/promociones_reglas.sql).");

            string nombre = (dto.Nombre ?? "").Trim().ToUpperInvariant();
            if (nombre.Length == 0) throw new InvalidOperationException("Ingrese el nombre de la promoción.");
            if (nombre.Length > 100) throw new InvalidOperationException("El nombre no puede superar los 100 caracteres.");

            string tipo = (dto.Tipo ?? "").Trim().ToUpperInvariant();
            decimal? pct = null; int? lleva = null, paga = null;
            if (tipo == TipoPromo.Porcentaje)
            {
                if (!dto.Porcentaje.HasValue || dto.Porcentaje.Value <= 0m || dto.Porcentaje.Value > 100m)
                    throw new InvalidOperationException("El porcentaje de descuento debe ser mayor que 0 y como máximo 100.");
                if (decimal.Round(dto.Porcentaje.Value, 2) != dto.Porcentaje.Value)
                    throw new InvalidOperationException("El porcentaje admite como máximo 2 decimales.");
                pct = dto.Porcentaje.Value;
            }
            else if (tipo == TipoPromo.NxM)
            {
                if (!dto.Lleva.HasValue || !dto.Paga.HasValue)
                    throw new InvalidOperationException("Indique cuántas unidades lleva y cuántas paga el cliente.");
                if (dto.Lleva.Value < 2 || dto.Lleva.Value > 99)
                    throw new InvalidOperationException("La cantidad que lleva el cliente debe estar entre 2 y 99.");
                if (dto.Paga.Value < 1 || dto.Paga.Value >= dto.Lleva.Value)
                    throw new InvalidOperationException("La cantidad que paga debe ser al menos 1 y menor que la que lleva.");
                lleva = dto.Lleva; paga = dto.Paga;
            }
            else throw new InvalidOperationException("Tipo de promoción inválido.");

            // Acumular variantes de una misma ficha: solo NxM, y solo si existe la columna y las variantes están habilitadas
            bool agrupa = false;
            if (tipo == TipoPromo.NxM && dto.AgrupaVariantes)
            {
                if (!agrupaVariantesDisponible())
                    throw new InvalidOperationException("Para acumular variantes falta ejecutar scripts/promociones_reglas_agrupa_variantes.sql.");
                if (!ClassVariantes.habilitado())
                    throw new InvalidOperationException("Para acumular variantes deben estar habilitadas las variantes de producto.");
                agrupa = true;
            }
            bool guardaAgrupa = agrupaVariantesDisponible();   // sin la columna no se la nombra en el SQL

            if (dto.Hasta < dto.Desde)
                throw new InvalidOperationException("La fecha de finalización no puede ser anterior a la de inicio.");

            List<int> prodIds = dto.ProductoIds.Where(i => i > 0).Distinct().ToList();
            if (prodIds.Count == 0) throw new InvalidOperationException("Seleccione al menos un producto.");
            DataTable ex = consulta("select count(*) from Productos where id in (" + lista(prodIds) + ")");
            if (Convert.ToInt32(ex.Rows[0][0]) != prodIds.Count)
                throw new InvalidOperationException("Hay productos seleccionados que ya no existen.");

            // Segundos fuera: la vigencia se maneja al minuto
            DateTime desde = new DateTime(dto.Desde.Year, dto.Desde.Month, dto.Desde.Day, dto.Desde.Hour, dto.Desde.Minute, 0);
            DateTime hasta = new DateTime(dto.Hasta.Year, dto.Hasta.Month, dto.Hasta.Day, dto.Hasta.Hour, dto.Hasta.Minute, 0);

            MySqlConnection cn = instDatos.abrirConexion();
            MySqlTransaction tx = null;
            try
            {
                tx = cn.BeginTransaction();
                int id = dto.Id;

                if (id > 0)
                {
                    MySqlCommand existe = new MySqlCommand("select count(*) from promociones_reglas where id = @id", cn, tx);
                    existe.Parameters.AddWithValue("@id", id);
                    if (Convert.ToInt32(existe.ExecuteScalar()) == 0) throw new InvalidOperationException("La promoción no existe.");

                    MySqlCommand upd = new MySqlCommand(
                        "update promociones_reglas set nombre=@nombre, tipo=@tipo, porcentaje=@pct, lleva=@lleva, paga=@paga, " +
                        (guardaAgrupa ? "agrupaVariantes=@agrupa, " : string.Empty) +
                        "vigenteDesde=@desde, vigenteHasta=@hasta, activa=@activa, usuario=@usuario where id=@id", cn, tx);
                    cargarParametros(upd, nombre, tipo, pct, lleva, paga, desde, hasta, dto.Activa, usuario);
                    upd.Parameters.AddWithValue("@agrupa", agrupa ? 1 : 0);
                    upd.Parameters.AddWithValue("@id", id);
                    upd.ExecuteNonQuery();
                }
                else
                {
                    MySqlCommand ins = new MySqlCommand(
                        "insert into promociones_reglas (nombre, tipo, porcentaje, lleva, paga, " + (guardaAgrupa ? "agrupaVariantes, " : string.Empty) +
                        "vigenteDesde, vigenteHasta, activa, fechaAlta, usuario) " +
                        "values (@nombre, @tipo, @pct, @lleva, @paga, " + (guardaAgrupa ? "@agrupa, " : string.Empty) +
                        "@desde, @hasta, @activa, @alta, @usuario)", cn, tx);
                    cargarParametros(ins, nombre, tipo, pct, lleva, paga, desde, hasta, dto.Activa, usuario);
                    ins.Parameters.AddWithValue("@agrupa", agrupa ? 1 : 0);
                    ins.Parameters.AddWithValue("@alta", ahoraArgentina());
                    ins.ExecuteNonQuery();
                    id = (int)ins.LastInsertedId;
                }

                MySqlCommand del = new MySqlCommand("delete from promociones_reglas_productos where fk_regla = @id", cn, tx);
                del.Parameters.AddWithValue("@id", id);
                del.ExecuteNonQuery();

                foreach (int prod in prodIds)
                {
                    MySqlCommand ip = new MySqlCommand("insert into promociones_reglas_productos (fk_regla, fk_producto) values (@r, @p)", cn, tx);
                    ip.Parameters.AddWithValue("@r", id);
                    ip.Parameters.AddWithValue("@p", prod);
                    ip.ExecuteNonQuery();
                }

                tx.Commit();
                return id;
            }
            catch
            {
                try { if (tx != null) tx.Rollback(); } catch { }
                throw;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        private static void cargarParametros(MySqlCommand c, string nombre, string tipo, decimal? pct, int? lleva, int? paga,
                                             DateTime desde, DateTime hasta, bool activa, string usuario)
        {
            c.Parameters.AddWithValue("@nombre", nombre);
            c.Parameters.AddWithValue("@tipo", tipo);
            c.Parameters.AddWithValue("@pct", pct.HasValue ? (object)pct.Value : DBNull.Value);
            c.Parameters.AddWithValue("@lleva", lleva.HasValue ? (object)lleva.Value : DBNull.Value);
            c.Parameters.AddWithValue("@paga", paga.HasValue ? (object)paga.Value : DBNull.Value);
            c.Parameters.AddWithValue("@desde", desde);
            c.Parameters.AddWithValue("@hasta", hasta);
            c.Parameters.AddWithValue("@activa", activa ? 1 : 0);
            c.Parameters.AddWithValue("@usuario", string.IsNullOrEmpty(usuario) ? (object)DBNull.Value : usuario);
        }

        /// <summary>Elimina la regla y sus productos (la auditoría de ventas/pedidos conserva el snapshot).</summary>
        public void eliminar(int id)
        {
            if (!tablasListas()) return;

            MySqlConnection cn = instDatos.abrirConexion();
            MySqlTransaction tx = null;
            try
            {
                tx = cn.BeginTransaction();
                MySqlCommand d1 = new MySqlCommand("delete from promociones_reglas_productos where fk_regla = @id", cn, tx);
                d1.Parameters.AddWithValue("@id", id);
                d1.ExecuteNonQuery();
                MySqlCommand d2 = new MySqlCommand("delete from promociones_reglas where id = @id", cn, tx);
                d2.Parameters.AddWithValue("@id", id);
                d2.ExecuteNonQuery();
                tx.Commit();
            }
            catch
            {
                try { if (tx != null) tx.Rollback(); } catch { }
                throw;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        /// <summary>Invierte el interruptor "activa" y devuelve el nuevo valor.</summary>
        public bool toggleActiva(int id)
        {
            try
            {
                MySqlConnection cn = instDatos.abrirConexion();
                MySqlCommand upd = new MySqlCommand("update promociones_reglas set activa = if(activa = 1, 0, 1) where id = @id", cn);
                upd.Parameters.AddWithValue("@id", id);
                if (upd.ExecuteNonQuery() == 0) throw new InvalidOperationException("La promoción no existe.");

                MySqlCommand sel = new MySqlCommand("select activa from promociones_reglas where id = @id", cn);
                sel.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(sel.ExecuteScalar()) == 1;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        // ── Auditoría de lo aplicado en ventas y pedidos ──────────────────────
        /// <summary>
        /// Registra en ventas_promo_detalle la promoción de cada línea de la venta. 'porFila' va en el MISMO orden en que
        /// se armó el detalle (null = sin promoción): sp_VentasAddVenta inserta las líneas en ese orden, así que la fila
        /// N corresponde a la N-ésima línea (por 'linea') de la venta. Sin el módulo habilitado no hace nada.
        /// </summary>
        public void grabarAuditoriaVenta(long venta, IList<PromoAplicada> porFila)
        {
            grabarAuditoria("ventas_promo_detalle", "fk_ventaDetalle",
                            "select linea from ventasDetalle where fk_venta = @id order by linea", venta, porFila);
        }

        /// <summary>Ídem para pedidos (pedidos_promo_detalle / pedidoDetalle.linea).</summary>
        public void grabarAuditoriaPedido(long pedido, IList<PromoAplicada> porFila)
        {
            grabarAuditoria("pedidos_promo_detalle", "fk_pedidoDetalle",
                            "select linea from pedidoDetalle where fk_pedido = @id order by linea", pedido, porFila);
        }

        private void grabarAuditoria(string tabla, string campoLinea, string sqlLineas, long id, IList<PromoAplicada> porFila)
        {
            if (!habilitado() || porFila == null || !porFila.Any(p => p != null)) return;

            List<long> lineas = consulta(sqlLineas, new MySqlParameter("@id", id)).AsEnumerable().Select(r => Convert.ToInt64(r[0])).ToList();
            if (lineas.Count != porFila.Count)
                throw new InvalidOperationException("El detalle grabado (" + lineas.Count + " líneas) no coincide con el detalle de la pantalla (" +
                                                    porFila.Count + " líneas): no se pudo registrar el detalle de promociones.");

            MySqlConnection cn = instDatos.abrirConexion();
            MySqlTransaction tx = null;
            try
            {
                tx = cn.BeginTransaction();
                for (int i = 0; i < porFila.Count; i++)
                {
                    PromoAplicada p = porFila[i];
                    if (p == null) continue;

                    MySqlCommand ins = new MySqlCommand(
                        "insert into " + tabla + " (" + campoLinea + ", fk_regla, nombre, tipo, porcentaje, lleva, paga, cantBonificada, ahorro) " +
                        "values (@linea, @regla, @nombre, @tipo, @pct, @lleva, @paga, @bonif, @ahorro)", cn, tx);
                    ins.Parameters.AddWithValue("@linea", lineas[i]);
                    ins.Parameters.AddWithValue("@regla", p.FkRegla);
                    ins.Parameters.AddWithValue("@nombre", p.Nombre ?? string.Empty);
                    ins.Parameters.AddWithValue("@tipo", p.Tipo);
                    ins.Parameters.AddWithValue("@pct", p.Porcentaje.HasValue ? (object)p.Porcentaje.Value : DBNull.Value);
                    ins.Parameters.AddWithValue("@lleva", p.Lleva.HasValue ? (object)p.Lleva.Value : DBNull.Value);
                    ins.Parameters.AddWithValue("@paga", p.Paga.HasValue ? (object)p.Paga.Value : DBNull.Value);
                    ins.Parameters.AddWithValue("@bonif", p.Tipo == TipoPromo.NxM ? (object)p.CantBonificada : DBNull.Value);
                    ins.Parameters.AddWithValue("@ahorro", p.Ahorro);
                    ins.ExecuteNonQuery();
                }
                tx.Commit();
            }
            catch
            {
                try { if (tx != null) tx.Rollback(); } catch { }
                throw;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        /// <summary>
        /// Promociones registradas en las líneas de un pedido (para recuperarlas al editarlo): linea, fk_producto, cantidad,
        /// descuento, recargo + los datos de la promoción. Vacía si el módulo no está habilitado.
        /// </summary>
        public DataTable auditoriaPedido(int pedido)
        {
            if (!habilitado()) return new DataTable();
            return consulta(
                "select d.linea, d.fk_producto, d.cantidad, IFNULL(d.descuento,0) as descuento, IFNULL(d.recargo,0) as recargo, " +
                "a.fk_regla, a.nombre, a.tipo, a.porcentaje, a.lleva, a.paga, a.cantBonificada, a.ahorro " +
                "from pedidoDetalle d join pedidos_promo_detalle a on a.fk_pedidoDetalle = d.linea " +
                "where d.fk_pedido = @p order by d.linea", new MySqlParameter("@p", pedido));
        }

        // ── Productos de una regla, con precio vigente sin IVA (dolarización incluida) ──
        /// <summary>id, codProveedor, codBarras, descripcion, proveedor, rubro, precio.</summary>
        public DataTable productosConPrecio(List<int> ids)
        {
            if (ids == null || ids.Count == 0) return new DataTable();

            DataTable t = consulta(
                "select p.id, p.codProveedor, p.codBarras, p.descripcion, pr.nombreComercial as proveedor, r.descripcion as rubro, " +
                "IFNULL(pp.precio, 0) as precio, IFNULL(p.dolarizado, 0) as dolarizado " +
                "from Productos p " +
                "left join Proveedores pr on pr.id = p.fk_proveedor " +
                "left join Rubros r on r.id = p.fk_rubro " +
                "left join preciosProductos pp on pp.fk_producto = p.id " +
                "where p.id in (" + lista(ids) + ") order by p.descripcion");

            bool dolariza = false; decimal cotiz = 0m;
            DataTable prm = consulta(
                "select parametro, valor from parametros where modulo = 'productos' and parametro in ('dolarizaProductos','cotizacionDolar')");
            foreach (DataRow r in prm.Rows)
            {
                string v = Convert.ToString(r["valor"]);
                if (r["parametro"].ToString() == "dolarizaProductos") dolariza = v == "1";
                else decimal.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out cotiz);
            }

            DataTable salida = new DataTable();
            salida.Columns.Add("id", typeof(int));
            salida.Columns.Add("codProveedor", typeof(string));
            salida.Columns.Add("codBarras", typeof(string));
            salida.Columns.Add("descripcion", typeof(string));
            salida.Columns.Add("proveedor", typeof(string));
            salida.Columns.Add("rubro", typeof(string));
            salida.Columns.Add("precio", typeof(decimal));
            foreach (DataRow r in t.Rows)
            {
                decimal precio = Convert.ToDecimal(r["precio"]);
                if (dolariza && Convert.ToInt32(r["dolarizado"]) == 1 && cotiz > 0m) precio *= cotiz;
                salida.Rows.Add(Convert.ToInt32(r["id"]), r["codProveedor"].ToString(), r["codBarras"].ToString(), r["descripcion"].ToString(),
                                r["proveedor"].ToString(), r["rubro"].ToString(), precio);
            }
            return salida;
        }
    }
}
