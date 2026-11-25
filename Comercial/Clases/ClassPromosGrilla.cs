using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Clases
{
    /// <summary>
    /// Enlaza el motor de promociones por cantidad / porcentaje con la grilla de detalle de Ventas, Venta minorista y
    /// Pedidos. Es el equivalente de promo-descuentos.js de Comercial Web:
    ///   * evalúa en segundo plano (150 ms de espera) cada vez que cambia la grilla y SOLO muestra el resultado si la
    ///     grilla no cambió mientras tanto;
    ///   * PORC: pone el descuento en la columna Desc/Rec de la fila;
    ///   * NxM: divide la fila en dos (cobradas + bonificadas al 100 %), para que lo que se ve coincida con lo que se graba;
    ///   * un Desc/Rec manual (distinto de cero y sin promoción) siempre gana; fraccionados, filas de pedido y filas con
    ///     subtotal manual no participan;
    ///   * avisos flotantes no bloqueantes: solo cuando cambia el conjunto de promociones aplicadas, y el aviso de
    ///     "falta 1 unidad" una sola vez por producto + promoción + cantidad.
    /// Con el módulo apagado (parámetro promociones/descuentos distinto de '1' o sin tablas) no agrega columnas ni
    /// consulta nada: Activo = false y todos los métodos no hacen nada.
    /// </summary>
    class ClassPromosGrilla
    {
        public class Configuracion
        {
            public Form Propietario;
            public DataGridView Grilla;
            public string ColId = "id";
            public string ColCantidad = "Cantidad";
            public string ColPrecioSinIva = "precioSinIva";
            public string ColDescRec = "DescRec";
            public string ColFraccionado = "fraccionado";
            /// <summary>Columna con el id de pedido de origen (filas que vienen de un pedido no participan). Null si no hay.</summary>
            public string ColPedido = null;
            /// <summary>Valor de Desc/Rec que le corresponde a una fila sin promoción (descuento/recargo general vigente).</summary>
            public Func<decimal> Baseline = () => 0m;
            /// <summary>Recalcula los totales de la pantalla (procesoTotales).</summary>
            public Action Recalcular = () => { };
        }

        const string COL_ROL = "promoRol";        // PORC | COB | BON
        const string COL_INFO = "promoInfo";      // PromoAplicada (para la auditoría)
        const string COL_TEXTO = "promoTexto";    // texto visible

        const string ROL_PORC = "PORC", ROL_COB = "COB", ROL_BON = "BON";

        readonly Configuracion cfg;
        readonly DataGridView grid;
        readonly Timer timer = new Timer { Interval = 150 };

        public bool Activo { get; private set; }

        bool escribiendo;
        int seq;
        string ultimaFirma = string.Empty;
        HashSet<string> proxVistas = new HashSet<string>();

        class Linea
        {
            public int Clave;
            public DataGridViewRow Fila;
            public int Producto;
            public decimal Cantidad;      // cantidad lógica (cobradas + bonificadas)
            public decimal Precio;
            public List<DataGridViewRow> Miembros;   // filas cobradas/bonificadas que forman la línea (null = fila común o con %)
        }

        public ClassPromosGrilla(Configuracion c)
        {
            cfg = c;
            grid = c.Grilla;
            Activo = ClassPromocionesDescuentos.habilitado();
            if (!Activo) return;

            agregarColumna(COL_ROL, false, 0);
            agregarColumna(COL_INFO, false, 0);
            agregarColumna(COL_TEXTO, true, 190);
            grid.Columns[COL_TEXTO].HeaderText = "Promoción";
            grid.Columns[COL_TEXTO].ReadOnly = true;

            timer.Tick += (s, e) => { timer.Stop(); evaluarAsync(); };
        }

        void agregarColumna(string nombre, bool visible, int ancho)
        {
            if (grid.Columns.Contains(nombre)) return;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn { Name = nombre, Visible = visible, SortMode = DataGridViewColumnSortMode.NotSortable };
            if (visible)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.Width = ancho;
            }
            grid.Columns.Add(col);
        }

        // ── Utilidades de celdas ─────────────────────────────────────────────
        static decimal dec(object v)
        {
            if (v == null || v == DBNull.Value) return 0m;
            if (v is decimal) return (decimal)v;
            decimal d;
            return decimal.TryParse(Convert.ToString(v), NumberStyles.Any, CultureInfo.CurrentCulture, out d) ? d : 0m;
        }

        static bool verdadero(object v)
        {
            if (v == null || v == DBNull.Value) return false;
            if (v is bool) return (bool)v;
            bool b;
            return bool.TryParse(Convert.ToString(v), out b) && b;
        }

        decimal cant(DataGridViewRow r) { return dec(r.Cells[cfg.ColCantidad].Value); }
        decimal precio(DataGridViewRow r) { return dec(r.Cells[cfg.ColPrecioSinIva].Value); }
        decimal descRec(DataGridViewRow r) { return dec(r.Cells[cfg.ColDescRec].Value); }
        int producto(DataGridViewRow r) { return (int)dec(r.Cells[cfg.ColId].Value); }
        string rol(DataGridViewRow r) { return Activo ? Convert.ToString(r.Cells[COL_ROL].Value) : string.Empty; }
        PromoInfo info(DataGridViewRow r) { return r.Cells[COL_INFO].Value as PromoInfo; }

        // La información de promoción viaja en la fila (se copia al dividir una NxM)
        public class PromoInfo
        {
            public ClassPromocionesDescuentos.PromoAplicada Aplicada;
        }

        IEnumerable<DataGridViewRow> filas()
        {
            foreach (DataGridViewRow r in grid.Rows)
                if (!r.IsNewRow) yield return r;
        }

        /// <summary>¿Es una fila de unidades bonificadas por una NxM? (no admite edición ni sumar cantidades)</summary>
        public bool EsBonificada(DataGridViewRow fila)
        {
            return Activo && fila != null && rol(fila) == ROL_BON;
        }

        /// <summary>¿La fila tiene una promoción aplicada? (Desc/Rec globales no deben pisarla.)</summary>
        public bool TienePromo(DataGridViewRow fila)
        {
            return Activo && fila != null && !string.IsNullOrEmpty(rol(fila));
        }

        static string claveGrupo(int prod, ClassPromocionesDescuentos.PromoAplicada a)
        {
            return prod + "|" + (a == null ? 0 : a.FkRegla);
        }

        // ── Disparo ──────────────────────────────────────────────────────────
        /// <summary>Pide una evaluación (se ejecuta 150 ms después del último cambio). Llamar al final de procesoTotales.</summary>
        public void Solicitar()
        {
            if (!Activo || escribiendo) return;
            timer.Stop();
            timer.Start();
        }

        public void Reset()
        {
            if (!Activo) return;
            if (grid.InvokeRequired) { grid.BeginInvoke(new Action(Reset)); return; }
            timer.Stop();
            seq++;
            ultimaFirma = string.Empty;
            proxVistas = new HashSet<string>();
            Formularios.Ventas.frmAvisoPromo.CerrarTodos();
        }

        // ── Elegibilidad y firma de la grilla ────────────────────────────────
        bool esManual(DataGridViewRow r)
        {
            string ro = rol(r);
            if (ro == ROL_PORC)
            {
                PromoInfo i = info(r);
                decimal esperado = i == null || i.Aplicada.Porcentaje == null ? 0m : -i.Aplicada.Porcentaje.Value;
                return Math.Abs(descRec(r) - esperado) > 0.001m;
            }
            if (ro == ROL_COB) return Math.Abs(descRec(r) - cfg.Baseline()) > 0.001m;
            if (ro == ROL_BON) return false;
            return descRec(r) != 0m;
        }

        // Condiciones que no dependen de las marcas de promoción
        bool elegibleBase(DataGridViewRow r)
        {
            if (verdadero(r.Cells[cfg.ColFraccionado].Value)) return false;
            if (Convert.ToString(r.Tag) == "subtotalManual") return false;
            if (cfg.ColPedido != null && dec(r.Cells[cfg.ColPedido].Value) != 0m) return false;
            if (cant(r) <= 0m || precio(r) <= 0m || producto(r) <= 0) return false;
            return true;
        }

        bool elegible(DataGridViewRow r)
        {
            if (rol(r) == ROL_BON) return false;
            if (!elegibleBase(r)) return false;
            if (esManual(r)) return false;       // Desc/Rec manual: gana el manual
            return true;
        }

        // Las filas cobradas (COB) y bonificadas (BON) de un mismo producto y promoción forman UNA línea lógica:
        // se evalúan juntas, con la suma de sus cantidades. La fila cobrada es el "ancla" de la línea; si el producto
        // tiene todas sus unidades bonificadas (caso típico al acumular variantes) el ancla es la primera bonificada.
        Dictionary<string, List<DataGridViewRow>> cubetas(List<DataGridViewRow> todas)
        {
            Dictionary<string, List<DataGridViewRow>> d = new Dictionary<string, List<DataGridViewRow>>();
            foreach (DataGridViewRow r in todas)
            {
                string ro = rol(r);
                if (ro != ROL_COB && ro != ROL_BON) continue;
                string k = claveGrupo(producto(r), info(r) == null ? null : info(r).Aplicada);
                if (!d.ContainsKey(k)) d[k] = new List<DataGridViewRow>();
                d[k].Add(r);
            }
            return d;
        }

        DataGridViewRow ancla(List<DataGridViewRow> miembros)
        {
            foreach (DataGridViewRow m in miembros) if (rol(m) == ROL_COB) return m;
            return miembros[0];
        }

        string firma()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataGridViewRow r in filas())
                sb.Append(producto(r)).Append('|').Append(cant(r)).Append('|').Append(precio(r)).Append('|')
                  .Append(descRec(r)).Append('|').Append(rol(r)).Append('|').Append(Convert.ToString(r.Tag)).Append(';');
            return sb.ToString();
        }

        // Líneas lógicas: las filas cobradas/bonificadas de un producto se evalúan juntas (con la suma de sus cantidades)
        List<Linea> instantanea()
        {
            List<DataGridViewRow> todas = filas().ToList();

            // Fila con promoción manipulada a mano: deja de ser promoción
            bool previo = escribiendo;
            escribiendo = true;
            try
            {
                foreach (DataGridViewRow r in todas)
                    if (!string.IsNullOrEmpty(rol(r)) && rol(r) != ROL_BON && esManual(r)) limpiarRol(r, false);
            }
            finally { escribiendo = previo; }

            Dictionary<string, List<DataGridViewRow>> cub = cubetas(todas);

            List<Linea> res = new List<Linea>();
            HashSet<string> emitidas = new HashSet<string>();
            foreach (DataGridViewRow r in todas)
            {
                string ro = rol(r);
                if (ro == ROL_COB || ro == ROL_BON)
                {
                    string k = claveGrupo(producto(r), info(r) == null ? null : info(r).Aplicada);
                    if (!emitidas.Add(k)) continue;

                    List<DataGridViewRow> miembros = cub[k];
                    DataGridViewRow anc = ancla(miembros);
                    if (!elegibleBase(anc)) continue;      // marcas que ya no corresponden: se limpian al reconstruir
                    res.Add(new Linea
                    {
                        Clave = res.Count, Fila = anc, Producto = producto(anc), Precio = precio(anc),
                        Cantidad = miembros.Sum(m => cant(m)), Miembros = miembros
                    });
                }
                else if (elegible(r))
                {
                    res.Add(new Linea { Clave = res.Count, Fila = r, Producto = producto(r), Cantidad = cant(r), Precio = precio(r) });
                }
            }
            return res;
        }

        // ── Evaluación ───────────────────────────────────────────────────────
        void evaluarAsync()
        {
            if (!Activo || grid.IsDisposed) return;
            if (escribiendo || grid.IsCurrentCellInEditMode) { timer.Start(); return; }   // no se pisa una edición en curso

            List<Linea> snap = instantanea();
            int mi = ++seq;
            string f0 = firma();

            List<LineaPromoEntrada> entrada = snap.Select(l => new LineaPromoEntrada
            {
                Clave = l.Clave, ProductoId = l.Producto, Cantidad = l.Cantidad, PrecioSinIva = l.Precio
            }).ToList();

            Task.Factory.StartNew(() =>
            {
                try { return new ClassPromocionesDescuentos().evaluar(entrada); }
                catch { return null; }   // ante un error de base se conserva lo que hay (como la web)
            }).ContinueWith(t =>
            {
                if (grid.IsDisposed || t.IsFaulted || t.Result == null) return;
                if (mi != seq || firma() != f0) return;                 // llegó algo más nuevo: este resultado ya no vale
                if (grid.IsCurrentCellInEditMode) { Solicitar(); return; }
                aplicar(snap, t.Result);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Evalúa AHORA (sin esperar) y deja la grilla al día. Devuelve true si la grilla cambió, para que el
        /// formulario avise "las promociones cambiaron" antes de grabar.
        /// </summary>
        public bool EvaluarAhora()
        {
            if (!Activo) return false;
            timer.Stop();
            seq++;

            List<Linea> snap = instantanea();
            ClassPromocionesDescuentos.ResultadoEvaluacion res;
            try
            {
                res = new ClassPromocionesDescuentos().evaluar(snap.Select(l => new LineaPromoEntrada
                {
                    Clave = l.Clave, ProductoId = l.Producto, Cantidad = l.Cantidad, PrecioSinIva = l.Precio
                }));
            }
            catch { return false; }
            return aplicar(snap, res);
        }

        // Estado actual de promociones de cada línea lógica vs el que dice el motor
        bool igualAlMotor(List<Linea> snap, ClassPromocionesDescuentos.ResultadoEvaluacion res)
        {
            Dictionary<int, LineaPromoResultado> porClave = res.Lineas.ToDictionary(r => r.Clave);

            foreach (Linea l in snap)
            {
                LineaPromoResultado esperado;
                porClave.TryGetValue(l.Clave, out esperado);

                if (esperado == null)
                {
                    // Sin promoción para esta línea: no debe tener marcas
                    if (l.Miembros != null || !string.IsNullOrEmpty(rol(l.Fila))) return false;
                    continue;
                }

                PromoInfo i = info(l.Fila);
                if (i == null || i.Aplicada.FkRegla != esperado.ReglaId) return false;

                if (esperado.Tipo == TipoPromo.Porcentaje)
                {
                    if (l.Miembros != null || rol(l.Fila) != ROL_PORC) return false;
                    if (Math.Abs(descRec(l.Fila) - (-esperado.DescuentoPct)) > 0.001m) return false;
                }
                else
                {
                    // NxM: las unidades bonificadas de la línea deben ser exactamente las que dice el motor
                    if (l.Miembros == null) return false;
                    decimal bon = l.Miembros.Where(m => rol(m) == ROL_BON).Sum(m => cant(m));
                    if (Math.Abs(bon - esperado.CantBonificada) > 0.0001m) return false;
                }
            }
            return true;
        }

        void limpiarRol(DataGridViewRow r, bool restaurarDescRec)
        {
            r.Cells[COL_ROL].Value = null;
            r.Cells[COL_INFO].Value = null;
            r.Cells[COL_TEXTO].Value = null;
            if (restaurarDescRec) r.Cells[cfg.ColDescRec].Value = cfg.Baseline();
        }

        // Devuelve la grilla a una fila por producto: une las cobradas y bonificadas en el ancla (suma de cantidades) y borra
        // las marcas. Nunca se pierde una unidad: una bonificada sin cobrada pasa a ser una fila común.
        void desagrupar()
        {
            List<DataGridViewRow> todas = filas().ToList();

            List<DataGridViewRow> aBorrar = new List<DataGridViewRow>();
            foreach (List<DataGridViewRow> miembros in cubetas(todas).Values)
            {
                DataGridViewRow anc = ancla(miembros);
                decimal total = miembros.Sum(m => cant(m));
                bool eraBonificada = rol(anc) == ROL_BON;

                if (miembros.Count > 1 || cant(anc) != total) anc.Cells[cfg.ColCantidad].Value = total;
                foreach (DataGridViewRow m in miembros) if (m != anc) aBorrar.Add(m);

                limpiarRol(anc, true);
                if (eraBonificada) desbloquear(anc);
            }
            foreach (DataGridViewRow b in aBorrar) grid.Rows.Remove(b);

            foreach (DataGridViewRow r in filas())
                if (rol(r) == ROL_PORC) limpiarRol(r, true);
        }

        // La fila deja de ser "bonificada": vuelve a ser editable como cualquier otra
        void desbloquear(DataGridViewRow r)
        {
            r.Cells[cfg.ColCantidad].ReadOnly = false;
            r.Cells[cfg.ColPrecioSinIva].ReadOnly = false;
            r.Cells[cfg.ColDescRec].ReadOnly = false;
            r.DefaultCellStyle.BackColor = System.Drawing.Color.Empty;
        }

        /// <summary>Quita todas las promociones de la grilla (una fila por producto). Usado antes de los combos.</summary>
        public void Desagrupar()
        {
            if (!Activo) return;
            timer.Stop();
            seq++;
            escribiendo = true;
            try { desagrupar(); }
            finally { escribiendo = false; }
            cfg.Recalcular();
        }

        static ClassPromocionesDescuentos.PromoAplicada copiaAplicada(LineaPromoResultado r, decimal ahorro, decimal cantBonificada)
        {
            return new ClassPromocionesDescuentos.PromoAplicada
            {
                FkRegla = r.ReglaId, Nombre = r.Nombre, Tipo = r.Tipo, Porcentaje = r.Porcentaje,
                Lleva = r.Lleva, Paga = r.Paga, CantBonificada = cantBonificada, Ahorro = ahorro
            };
        }

        void marcarBonificada(DataGridViewRow r)
        {
            r.Cells[cfg.ColCantidad].ReadOnly = true;
            r.Cells[cfg.ColPrecioSinIva].ReadOnly = true;
            r.Cells[cfg.ColDescRec].ReadOnly = true;
            r.DefaultCellStyle.BackColor = System.Drawing.Color.Honeydew;
        }

        // Aplica el resultado del motor. Devuelve true si hubo que modificar la grilla.
        bool aplicar(List<Linea> snap, ClassPromocionesDescuentos.ResultadoEvaluacion res)
        {
            bool cambio = false;
            bool igual = igualAlMotor(snap, res) && hayMarcasSoloDeSnap(snap);

            escribiendo = true;
            try
            {
                if (!igual)
                {
                    cambio = true;
                    desagrupar();

                    foreach (LineaPromoResultado r in res.Lineas.OrderByDescending(x => x.Clave))
                    {
                        Linea l = snap[r.Clave];
                        DataGridViewRow fila = l.Fila;
                        if (fila.Index < 0) continue;

                        if (r.Tipo == TipoPromo.Porcentaje)
                        {
                            fila.Cells[cfg.ColDescRec].Value = -r.DescuentoPct;
                            fila.Cells[COL_ROL].Value = ROL_PORC;
                            fila.Cells[COL_INFO].Value = new PromoInfo { Aplicada = copiaAplicada(r, r.Ahorro, 0m) };
                        }
                        else
                        {
                            decimal total = cant(fila);
                            decimal bonif = r.CantBonificada;
                            decimal cobradas = total - bonif;

                            if (cobradas > 0m)
                            {
                                fila.Cells[cfg.ColCantidad].Value = cobradas;
                                fila.Cells[COL_ROL].Value = ROL_COB;
                                fila.Cells[COL_INFO].Value = new PromoInfo { Aplicada = copiaAplicada(r, 0m, bonif) };

                                // La fila bonificada se inserta YA COMPLETA (todas sus celdas en una sola operación):
                                // si se copiara celda por celda, cada escritura dispararía CellValueChanged -> procesoTotales
                                // con una fila a medio llenar (precioSinIva nulo) y fallaría con NullReferenceException.
                                object[] valores = new object[grid.Columns.Count];
                                foreach (DataGridViewColumn c in grid.Columns)
                                    valores[c.Index] = fila.Cells[c.Index].Value;
                                valores[grid.Columns[cfg.ColCantidad].Index] = bonif;
                                valores[grid.Columns[cfg.ColDescRec].Index] = -100m;
                                valores[grid.Columns[COL_ROL].Index] = ROL_BON;
                                valores[grid.Columns[COL_INFO].Index] = new PromoInfo { Aplicada = copiaAplicada(r, r.Ahorro, bonif) };
                                valores[grid.Columns[COL_TEXTO].Index] = null;

                                int idx = fila.Index + 1;
                                grid.Rows.Insert(idx, valores);
                                DataGridViewRow bon = grid.Rows[idx];
                                bon.Tag = null;
                                marcarBonificada(bon);
                            }
                            else
                            {
                                // Toda la fila es bonificada (otra fila del mismo producto lleva las cobradas)
                                fila.Cells[cfg.ColCantidad].Value = bonif;
                                fila.Cells[cfg.ColDescRec].Value = -100m;
                                fila.Cells[COL_ROL].Value = ROL_BON;
                                fila.Cells[COL_INFO].Value = new PromoInfo { Aplicada = copiaAplicada(r, r.Ahorro, bonif) };
                                marcarBonificada(fila);
                            }
                        }
                    }

                }

                actualizarTextos(snap, res);
                if (cambio) cfg.Recalcular();
            }
            finally
            {
                escribiendo = false;
            }

            avisar(res);
            return cambio;
        }

        // Si hay filas con marcas que no pertenecen a ninguna línea evaluada, hay que rehacer
        bool hayMarcasSoloDeSnap(List<Linea> snap)
        {
            HashSet<DataGridViewRow> conocidas = new HashSet<DataGridViewRow>();
            foreach (Linea l in snap)
            {
                conocidas.Add(l.Fila);
                if (l.Miembros != null) foreach (DataGridViewRow m in l.Miembros) conocidas.Add(m);
            }
            foreach (DataGridViewRow r in filas())
                if (!string.IsNullOrEmpty(rol(r)) && !conocidas.Contains(r)) return false;
            return true;
        }

        void actualizarTextos(List<Linea> snap, ClassPromocionesDescuentos.ResultadoEvaluacion res)
        {
            Dictionary<int, ClassPromocionesDescuentos.AvisoProxima> prox = new Dictionary<int, ClassPromocionesDescuentos.AvisoProxima>();
            foreach (var x in res.Proximas) prox[x.Clave] = x;
            Dictionary<DataGridViewRow, int> claveDe = new Dictionary<DataGridViewRow, int>();
            foreach (Linea l in snap) claveDe[l.Fila] = l.Clave;

            foreach (DataGridViewRow r in filas())
            {
                string ro = rol(r);
                PromoInfo i = info(r);
                string texto = null;
                if (ro == ROL_PORC && i != null)
                    texto = i.Aplicada.Nombre + "  −" + i.Aplicada.Ahorro.ToString("N2");
                else if (ro == ROL_COB && i != null)
                    texto = i.Aplicada.Nombre + " (" + i.Aplicada.Lleva + "x" + i.Aplicada.Paga + ")";
                else if (ro == ROL_BON && i != null)
                    texto = "BONIFICADA — " + i.Aplicada.Nombre + "  −" + i.Aplicada.Ahorro.ToString("N2");
                else
                {
                    int clave;
                    ClassPromocionesDescuentos.AvisoProxima p;
                    if (claveDe.TryGetValue(r, out clave) && prox.TryGetValue(clave, out p)) texto = p.TextoCorto;
                }
                if (Convert.ToString(r.Cells[COL_TEXTO].Value) != (texto ?? string.Empty))
                    r.Cells[COL_TEXTO].Value = texto;
            }
        }

        // Avisos: solo promociones realmente aplicadas, y solo cuando cambia el conjunto aplicado
        void avisar(ClassPromocionesDescuentos.ResultadoEvaluacion res)
        {
            string f = string.Join("¦", res.Resumen.Select(x => x.FkProducto + "|" + x.Texto));
            if (f != ultimaFirma)
            {
                ultimaFirma = f;
                if (res.Resumen.Count > 0)
                {
                    string texto = string.Join("\r\n", res.Resumen.Select(x => "• " + x.Descripcion + ": " + x.Texto + "."));
                    Formularios.Ventas.frmAvisoPromo.Mostrar(cfg.Propietario, "aplicadas", "Se aplicaron las siguientes promociones", texto, false, 8000);
                }
            }

            // Aviso de proximidad: una sola vez por producto + promoción + cantidad
            HashSet<string> vistasAhora = new HashSet<string>();
            List<ClassPromocionesDescuentos.AvisoProxima> nuevas = new List<ClassPromocionesDescuentos.AvisoProxima>();
            foreach (var x in res.Proximas)
            {
                string k = x.FkProducto + "|" + x.FkRegla + "|" + x.CantidadActual;
                vistasAhora.Add(k);
                if (!proxVistas.Contains(k)) nuevas.Add(x);
            }
            proxVistas = vistasAhora;
            if (nuevas.Count > 0)
            {
                string texto = string.Join("\r\n\r\n", nuevas.Select(x => x.Descripcion + "\r\n" + x.Texto));
                Formularios.Ventas.frmAvisoPromo.Mostrar(cfg.Propietario, "proxima", "Promoción disponible", texto, true, 7000);
            }
        }

        // ── Auditoría ────────────────────────────────────────────────────────
        /// <summary>
        /// Promoción de cada fila de la grilla, en el MISMO orden en que se arma el detalle a grabar
        /// (null = fila sin promoción). Vacía si el módulo está apagado.
        /// </summary>
        public List<ClassPromocionesDescuentos.PromoAplicada> AuditoriaPorFila()
        {
            List<ClassPromocionesDescuentos.PromoAplicada> lista = new List<ClassPromocionesDescuentos.PromoAplicada>();
            if (!Activo) return lista;
            foreach (DataGridViewRow r in filas())
            {
                PromoInfo i = string.IsNullOrEmpty(rol(r)) ? null : info(r);
                lista.Add(i == null ? null : i.Aplicada);
            }
            return lista;
        }

        // ── Pedidos: recupera las promociones de un pedido guardado al editarlo ──
        /// <summary>
        /// Asocia a las filas ya cargadas las promociones registradas en la auditoría del pedido
        /// (se busca la fila por producto, cantidad y descuento). Sin auditoría, no hace nada.
        /// </summary>
        public void RecuperarDePedido(DataTable auditoria)
        {
            if (!Activo || auditoria == null || auditoria.Rows.Count == 0) return;

            HashSet<DataGridViewRow> usadas = new HashSet<DataGridViewRow>();
            escribiendo = true;
            try
            {
                foreach (DataRow a in auditoria.Rows)
                {
                    int prod = Convert.ToInt32(a["fk_producto"]);
                    decimal cantidad = Convert.ToDecimal(a["cantidad"]);
                    decimal dr = -Convert.ToDecimal(a["descuento"]) + Convert.ToDecimal(a["recargo"]);

                    DataGridViewRow fila = filas().FirstOrDefault(r => !usadas.Contains(r) && producto(r) == prod &&
                                                                       cant(r) == cantidad && Math.Abs(descRec(r) - dr) < 0.001m);
                    if (fila == null) continue;
                    usadas.Add(fila);

                    string tipo = a["tipo"].ToString();
                    ClassPromocionesDescuentos.PromoAplicada ap = new ClassPromocionesDescuentos.PromoAplicada
                    {
                        FkRegla = a["fk_regla"] == DBNull.Value ? 0 : Convert.ToInt32(a["fk_regla"]),
                        Nombre = a["nombre"].ToString(),
                        Tipo = tipo,
                        Porcentaje = a["porcentaje"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(a["porcentaje"]),
                        Lleva = a["lleva"] == DBNull.Value ? (int?)null : Convert.ToInt32(a["lleva"]),
                        Paga = a["paga"] == DBNull.Value ? (int?)null : Convert.ToInt32(a["paga"]),
                        CantBonificada = a["cantBonificada"] == DBNull.Value ? 0m : Convert.ToDecimal(a["cantBonificada"]),
                        Ahorro = Convert.ToDecimal(a["ahorro"])
                    };
                    if (ap.FkRegla == 0) continue;

                    fila.Cells[COL_INFO].Value = new PromoInfo { Aplicada = ap };
                    if (tipo == TipoPromo.Porcentaje) fila.Cells[COL_ROL].Value = ROL_PORC;
                    else if (Convert.ToDecimal(a["descuento"]) >= 100m) { fila.Cells[COL_ROL].Value = ROL_BON; marcarBonificada(fila); }
                    else fila.Cells[COL_ROL].Value = ROL_COB;
                }
            }
            finally
            {
                escribiendo = false;
            }
        }
    }
}
