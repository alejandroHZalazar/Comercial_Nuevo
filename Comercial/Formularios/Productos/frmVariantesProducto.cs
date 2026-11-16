using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    /// <summary>
    /// Editor de productos con variantes (medidas y/o colores), en 4 pasos:
    ///   1) qué varía, 2) precios iguales o distintos, 3) elegir medidas y colores, 4) grilla de combinaciones.
    /// Guarda la ficha y todas sus variantes juntas (todo o nada). Se abre desde frmAltaModifProductos.
    /// </summary>
    public class frmVariantesProducto : Form
    {
        private readonly Clases.ClassVariantes inst = new Clases.ClassVariantes();
        private readonly Clases.ClassVariantes.FichaDatos ficha;
        private readonly Clases.ClassVariantes.FichaVariantes actual;   // ficha ya guardada (vacía si es nueva)
        private readonly BindingList<Clases.ClassVariantes.VarianteFila> filas = new BindingList<Clases.ClassVariantes.VarianteFila>();

        private DataTable tablaMedidas;
        private DataTable tablaColores;
        private int paso = 1;
        private bool cargando = true;

        // Cabecera y barra inferior
        private Label lblPaso;
        private Panel pnlContenido;
        private Button btnAtras, btnSiguiente, btnGuardar, btnCancelar, btnMaestros;
        private Panel[] pasos = new Panel[4];

        // Paso 1
        private CheckBox chkMedidas, chkColores;
        // Paso 2
        private RadioButton rbIguales, rbDistintos;
        private Label lblComunes;
        private Label lblPropagacion;
        private ComboBox cboPropagacion;
        // Paso 3
        private CheckedListBox clbMedidas, clbColores;
        private TextBox txtNuevaMedida, txtNuevoColor;
        private Button btnAgregarMedida, btnAgregarColor;
        private GroupBox gbMedidas, gbColores;
        // Paso 4
        private DataGridView dgv;
        private TextBox txtBulkCosto, txtBulkPrecio, txtBulkStock;
        private Label lblResumen;

        internal frmVariantesProducto(Clases.ClassVariantes.FichaDatos datos)
        {
            ficha = datos;
            actual = datos.ProductoId > 0 ? inst.obtenerFicha(datos.ProductoId) : new Clases.ClassVariantes.FichaVariantes();

            Text = ficha.ProductoId > 0 ? "Variantes del producto" : "Alta de producto con variantes";
            Font = new Font("Segoe UI", 9F);
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 560);
            MinimumSize = new Size(760, 500);
            KeyPreview = true;

            construir();

            Load += (s, e) => iniciar();
            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) { DialogResult = DialogResult.Cancel; Close(); }
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        // Construcción de la interfaz
        // ══════════════════════════════════════════════════════════════════════
        private void construir()
        {
            lblPaso = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };

            Panel barra = new Panel { Dock = DockStyle.Bottom, Height = 54, Padding = new Padding(10) };
            btnMaestros = new Button { Text = "Medidas y colores...", Size = new Size(150, 32), Location = new Point(10, 11), Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
            btnCancelar = new Button { Text = "Cancelar", Size = new Size(95, 32), Anchor = AnchorStyles.Right | AnchorStyles.Bottom };
            btnGuardar = new Button { Text = "Guardar", Size = new Size(100, 32), Anchor = AnchorStyles.Right | AnchorStyles.Bottom };
            btnSiguiente = new Button { Text = "Siguiente >", Size = new Size(100, 32), Anchor = AnchorStyles.Right | AnchorStyles.Bottom };
            btnAtras = new Button { Text = "< Atrás", Size = new Size(95, 32), Anchor = AnchorStyles.Right | AnchorStyles.Bottom };
            barra.Controls.AddRange(new Control[] { btnMaestros, btnAtras, btnSiguiente, btnGuardar, btnCancelar });
            barra.Resize += (s, e) => ubicarBotones(barra);
            ubicarBotones(barra);

            Clases.IconosVariantes.Aplicar(btnMaestros, Properties.Resources.tag);
            Clases.IconosVariantes.Aplicar(btnAtras, Properties.Resources._return);
            Clases.IconosVariantes.Aplicar(btnSiguiente, Properties.Resources.play_button);
            Clases.IconosVariantes.Aplicar(btnGuardar, Properties.Resources.save__1_);
            Clases.IconosVariantes.Aplicar(btnCancelar, Properties.Resources.back_arrow);
            btnSiguiente.ImageAlign = ContentAlignment.MiddleRight;
            btnSiguiente.TextImageRelation = TextImageRelation.TextBeforeImage;

            btnAtras.Click += (s, e) => irAPaso(paso - 1);
            btnSiguiente.Click += (s, e) => siguiente();
            btnGuardar.Click += (s, e) => guardar();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            btnMaestros.Click += (s, e) => abrirMaestros();

            pnlContenido = new Panel { Dock = DockStyle.Fill, Padding = new Padding(14) };

            for (int i = 0; i < 4; i++)
                pasos[i] = new Panel { Dock = DockStyle.Fill, Visible = false };

            construirPaso1(pasos[0]);
            construirPaso2(pasos[1]);
            construirPaso3(pasos[2]);
            construirPaso4(pasos[3]);
            foreach (Panel p in pasos) pnlContenido.Controls.Add(p);

            Controls.Add(pnlContenido);
            Controls.Add(barra);
            Controls.Add(lblPaso);
            pnlContenido.BringToFront();
        }

        private void ubicarBotones(Panel barra)
        {
            int x = barra.ClientSize.Width - 10;
            foreach (Button b in new[] { btnCancelar, btnGuardar, btnSiguiente, btnAtras })
            {
                x -= b.Width;
                b.Location = new Point(x, 11);
                x -= 8;
            }
        }

        private void construirPaso1(Panel p)
        {
            p.Controls.Add(new Label { Text = "¿Qué varía en este producto?", Font = new Font("Segoe UI", 10F, FontStyle.Bold), AutoSize = true, Location = new Point(4, 4) });
            chkMedidas = new CheckBox { Text = "Medidas (talles, tamaños, capacidades...)", AutoSize = true, Location = new Point(14, 44) };
            chkColores = new CheckBox { Text = "Colores", AutoSize = true, Location = new Point(14, 76) };
            Label nota = new Label
            {
                AutoSize = false,
                Size = new Size(620, 60),
                Location = new Point(14, 118),
                ForeColor = Color.DimGray,
                Text = "Cada combinación será un producto con su propio stock, costo y precio. " +
                       "En ventas, pedidos y reportes se usa cada variante; la ficha principal solo se ve en Gestión de productos."
            };
            p.Controls.AddRange(new Control[] { chkMedidas, chkColores, nota });
        }

        private void construirPaso2(Panel p)
        {
            p.Controls.Add(new Label { Text = "Costos y precios", Font = new Font("Segoe UI", 10F, FontStyle.Bold), AutoSize = true, Location = new Point(4, 4) });
            rbIguales = new RadioButton { Text = "Todas las variantes tienen el mismo costo y precio", AutoSize = true, Location = new Point(14, 44), Checked = true };
            rbDistintos = new RadioButton { Text = "Cada variante tiene su propio costo y precio", AutoSize = true, Location = new Point(14, 74) };
            lblComunes = new Label { AutoSize = false, Size = new Size(700, 40), Location = new Point(32, 104), ForeColor = Color.DimGray };
            lblPropagacion = new Label
            {
                AutoSize = true,
                Location = new Point(14, 164),
                Text = "Si cambió el costo o precio común, aplicarlo a las variantes existentes:"
            };
            cboPropagacion = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(14, 188), Width = 380 };
            cboPropagacion.Items.Add(new OpcionPropagacion("Solo las que hoy siguen al valor común", Clases.ClassVariantes.PropagarSoloQuienesSiguen));
            cboPropagacion.Items.Add(new OpcionPropagacion("Todas las variantes", Clases.ClassVariantes.PropagarTodas));
            cboPropagacion.Items.Add(new OpcionPropagacion("Ninguna (conservar los precios actuales)", Clases.ClassVariantes.PropagarNinguna));
            cboPropagacion.SelectedIndex = 0;
            rbIguales.CheckedChanged += (s, e) => actualizarPaso2();
            p.Controls.AddRange(new Control[] { rbIguales, rbDistintos, lblComunes, lblPropagacion, cboPropagacion });
        }

        private class OpcionPropagacion
        {
            public string Texto; public int Valor;
            public OpcionPropagacion(string t, int v) { Texto = t; Valor = v; }
            public override string ToString() { return Texto; }
        }

        private void construirPaso3(Panel p)
        {
            p.Controls.Add(new Label { Text = "Elija las medidas y los colores", Font = new Font("Segoe UI", 10F, FontStyle.Bold), AutoSize = true, Location = new Point(4, 4) });

            gbMedidas = new GroupBox { Text = "Medidas", Location = new Point(4, 34), Size = new Size(420, 340), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom };
            clbMedidas = new CheckedListBox { Location = new Point(10, 22), Size = new Size(400, 256), CheckOnClick = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            txtNuevaMedida = new TextBox { Location = new Point(10, 306), Width = 280, MaxLength = Clases.ClassVariantes.MaxNombreAtributo, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
            btnAgregarMedida = new Button { Text = "Crear medida", Location = new Point(298, 304), Size = new Size(110, 26), Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
            gbMedidas.Controls.Add(new Label { Text = "Nueva medida:", AutoSize = true, Location = new Point(10, 286), Anchor = AnchorStyles.Left | AnchorStyles.Bottom });
            gbMedidas.Controls.AddRange(new Control[] { clbMedidas, txtNuevaMedida, btnAgregarMedida });

            gbColores = new GroupBox { Text = "Colores", Location = new Point(440, 34), Size = new Size(420, 340), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom };
            clbColores = new CheckedListBox { Location = new Point(10, 22), Size = new Size(400, 256), CheckOnClick = true, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            txtNuevoColor = new TextBox { Location = new Point(10, 306), Width = 280, MaxLength = Clases.ClassVariantes.MaxNombreAtributo, Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
            btnAgregarColor = new Button { Text = "Crear color", Location = new Point(298, 304), Size = new Size(110, 26), Anchor = AnchorStyles.Left | AnchorStyles.Bottom };
            gbColores.Controls.Add(new Label { Text = "Nuevo color:", AutoSize = true, Location = new Point(10, 286), Anchor = AnchorStyles.Left | AnchorStyles.Bottom });
            gbColores.Controls.AddRange(new Control[] { clbColores, txtNuevoColor, btnAgregarColor });

            Clases.IconosVariantes.Aplicar(btnAgregarMedida, Properties.Resources.plus);
            Clases.IconosVariantes.Aplicar(btnAgregarColor, Properties.Resources.plus);
            btnAgregarMedida.Click += (s, e) => crearAtributo(true);
            btnAgregarColor.Click += (s, e) => crearAtributo(false);

            p.Controls.AddRange(new Control[] { gbMedidas, gbColores });
            p.Resize += (s, e) =>
            {
                int ancho = Math.Max(300, (p.ClientSize.Width - 20) / 2);
                gbMedidas.Width = ancho;
                gbColores.Left = gbMedidas.Right + 12;
                gbColores.Width = ancho;
                gbMedidas.Height = gbColores.Height = Math.Max(200, p.ClientSize.Height - 40);
            };
        }

        private void construirPaso4(Panel p)
        {
            Panel top = new Panel { Dock = DockStyle.Top, Height = 74 };
            lblResumen = new Label { AutoSize = true, Location = new Point(4, 4), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            Button btnCodigos = new Button { Text = "Generar códigos de proveedor", Location = new Point(4, 32), Size = new Size(190, 28) };
            Label lblAplicar = new Label { Text = "Aplicar a las filas seleccionadas (o a todas):", AutoSize = true, Location = new Point(210, 38) };
            txtBulkCosto = new TextBox { Location = new Point(470, 34), Width = 70 };
            txtBulkPrecio = new TextBox { Location = new Point(548, 34), Width = 70 };
            txtBulkStock = new TextBox { Location = new Point(626, 34), Width = 60 };
            Button btnAplicar = new Button { Text = "Aplicar", Location = new Point(694, 32), Size = new Size(90, 28) };
            Label hCosto = new Label { Text = "Costo", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(470, 18) };
            Label hPrecio = new Label { Text = "Precio", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(548, 18) };
            Label hStock = new Label { Text = "Stock inicial", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(626, 18) };
            top.Controls.AddRange(new Control[] { lblResumen, btnCodigos, lblAplicar, txtBulkCosto, txtBulkPrecio, txtBulkStock, btnAplicar, hCosto, hPrecio, hStock });
            Clases.IconosVariantes.Aplicar(btnCodigos, Properties.Resources.list_with_bullets);
            Clases.IconosVariantes.Aplicar(btnAplicar, Properties.Resources.pencil_edit_button__1_);
            btnCodigos.Click += (s, e) => generarCodigos();
            btnAplicar.Click += (s, e) => aplicarMasivo();

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DimGray;
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(242, 242, 242) };
            dgv.DataError += (s, e) => { e.ThrowException = false; MessageBox.Show(this, "El valor ingresado no es válido para esa columna.", "Variantes", MessageBoxButtons.OK, MessageBoxIcon.Warning); };
            dgv.CurrentCellDirtyStateChanged += (s, e) => { if (dgv.IsCurrentCellDirty && dgv.CurrentCell is DataGridViewCheckBoxCell) dgv.CommitEdit(DataGridViewDataErrorContexts.Commit); };
            dgv.DataBindingComplete += (s, e) => marcarSoloLectura();
            dgv.CellValueChanged += (s, e) => { if (!cargando) actualizarResumen(); };

            p.Controls.Add(dgv);
            p.Controls.Add(top);
            dgv.BringToFront();
        }

        // ══════════════════════════════════════════════════════════════════════
        // Carga inicial
        // ══════════════════════════════════════════════════════════════════════
        private void iniciar()
        {
            cargarMaestros();

            if (actual.EsFicha)
            {
                chkMedidas.Checked = actual.UsaMedidas;
                chkColores.Checked = actual.UsaColores;
                chkMedidas.Enabled = chkColores.Enabled = actual.Variantes.Count == 0;   // con variantes, las dimensiones no se cambian
                rbIguales.Checked = actual.PreciosIguales;
                rbDistintos.Checked = !actual.PreciosIguales;
                foreach (var v in actual.Variantes) filas.Add(v);
                marcarSeleccionDesdeFilas();
            }
            else
            {
                chkMedidas.Checked = true;
            }

            cargando = false;
            irAPaso(actual.EsFicha ? 4 : 1);
        }

        private void cargarMaestros()
        {
            // Las medidas/colores deshabilitados solo se muestran si alguna variante existente los usa
            HashSet<int> usadasM = new HashSet<int>(actual.Variantes.Select(v => v.MedidaId));
            HashSet<int> usadasC = new HashSet<int>(actual.Variantes.Select(v => v.ColorId));

            tablaMedidas = inst.traeMedidas(false);
            tablaColores = inst.traeColores(false);
            quitarInactivosNoUsados(tablaMedidas, usadasM);
            quitarInactivosNoUsados(tablaColores, usadasC);

            llenarLista(clbMedidas, tablaMedidas);
            llenarLista(clbColores, tablaColores);
        }

        private static void quitarInactivosNoUsados(DataTable t, HashSet<int> usadas)
        {
            for (int i = t.Rows.Count - 1; i >= 0; i--)
                if (Convert.ToInt32(t.Rows[i]["activa"]) == 0 && !usadas.Contains(Convert.ToInt32(t.Rows[i]["id"])))
                    t.Rows.RemoveAt(i);
        }

        private static void llenarLista(CheckedListBox clb, DataTable t)
        {
            HashSet<int> marcados = idsMarcados(clb);
            clb.Items.Clear();
            foreach (DataRow r in t.Rows)
            {
                string nombre = r["nombre"].ToString() + (Convert.ToInt32(r["activa"]) == 0 ? " (deshabilitada)" : "");
                int id = Convert.ToInt32(r["id"]);
                clb.Items.Add(new ItemAtributo(id, nombre), marcados.Contains(id));
            }
        }

        private class ItemAtributo
        {
            public int Id; public string Texto;
            public ItemAtributo(int id, string t) { Id = id; Texto = t; }
            public override string ToString() { return Texto; }
        }

        private static HashSet<int> idsMarcados(CheckedListBox clb)
        {
            return new HashSet<int>(clb.CheckedItems.Cast<ItemAtributo>().Select(i => i.Id));
        }

        private void marcarSeleccionDesdeFilas()
        {
            HashSet<int> m = new HashSet<int>(filas.Select(f => f.MedidaId));
            HashSet<int> c = new HashSet<int>(filas.Select(f => f.ColorId));
            for (int i = 0; i < clbMedidas.Items.Count; i++) clbMedidas.SetItemChecked(i, m.Contains(((ItemAtributo)clbMedidas.Items[i]).Id));
            for (int i = 0; i < clbColores.Items.Count; i++) clbColores.SetItemChecked(i, c.Contains(((ItemAtributo)clbColores.Items[i]).Id));
        }

        private string nombreMedida(int id)
        {
            DataRow[] r = tablaMedidas.Select("id = " + id);
            return r.Length > 0 ? r[0]["nombre"].ToString() : null;
        }

        private string nombreColor(int id)
        {
            DataRow[] r = tablaColores.Select("id = " + id);
            return r.Length > 0 ? r[0]["nombre"].ToString() : null;
        }

        // ══════════════════════════════════════════════════════════════════════
        // Navegación entre pasos
        // ══════════════════════════════════════════════════════════════════════
        private static readonly string[] titulos =
        {
            "Paso 1 de 4 — Qué varía",
            "Paso 2 de 4 — Costos y precios",
            "Paso 3 de 4 — Medidas y colores",
            "Paso 4 de 4 — Combinaciones"
        };

        private void irAPaso(int n)
        {
            if (n < 1 || n > 4) return;
            paso = n;
            for (int i = 0; i < 4; i++) pasos[i].Visible = (i == n - 1);
            lblPaso.Text = titulos[n - 1] + "   |   " + (ficha.Descripcion ?? string.Empty).Trim();

            btnAtras.Enabled = n > 1;
            btnSiguiente.Visible = n < 4;
            btnGuardar.Visible = n == 4;

            if (n == 2) actualizarPaso2();
            if (n == 3)
            {
                gbMedidas.Enabled = chkMedidas.Checked;
                gbColores.Enabled = chkColores.Checked;
            }
            if (n == 4) prepararGrilla();
        }

        private void siguiente()
        {
            if (paso == 1)
            {
                if (!chkMedidas.Checked && !chkColores.Checked)
                {
                    MessageBox.Show(this, "Elija si el producto varía por medidas, colores o ambos.", "Variantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (paso == 3)
            {
                if (!aplicarSeleccion()) return;
            }
            irAPaso(paso + 1);
        }

        private void actualizarPaso2()
        {
            bool iguales = rbIguales.Checked;
            lblComunes.Text = iguales
                ? "Costo común: " + ficha.Costo.ToString("N2") + "     Precio de venta común: " + ficha.Precio.ToString("N2") +
                  "\r\n(se cargan en la ficha del producto; cada variante puede igual tener un valor propio en la grilla)"
                : "Cada variante tendrá su costo y precio en la grilla. La ficha guardará como referencia el menor valor de las variantes habilitadas.";

            bool hayExistentes = actual.EsFicha && actual.Variantes.Count > 0;
            lblPropagacion.Visible = cboPropagacion.Visible = iguales && hayExistentes;
        }

        // Pasa de la selección de medidas/colores a las filas de la grilla:
        // agrega las combinaciones que faltan, quita las nuevas que ya no corresponden y
        // deshabilita las existentes que ya no corresponden.
        private bool aplicarSeleccion()
        {
            List<int> ms = chkMedidas.Checked ? clbMedidas.CheckedItems.Cast<ItemAtributo>().Select(i => i.Id).ToList() : new List<int> { 0 };
            List<int> cs = chkColores.Checked ? clbColores.CheckedItems.Cast<ItemAtributo>().Select(i => i.Id).ToList() : new List<int> { 0 };

            if (ms.Count == 0 || cs.Count == 0)
            {
                MessageBox.Show(this, "Elija al menos una opción en cada dimensión.", "Variantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Filas que ya no corresponden a la selección
            foreach (var f in filas.ToList())
            {
                bool coincide = ms.Contains(f.MedidaId) && cs.Contains(f.ColorId);
                if (coincide) continue;
                if (f.ProductoId.HasValue) f.Habilitada = false;
                else filas.Remove(f);
            }

            // Combinaciones nuevas
            foreach (int m in ms)
                foreach (int c in cs)
                    if (!filas.Any(f => f.MedidaId == m && f.ColorId == c))
                        filas.Add(nuevaFila(m, c));

            return true;
        }

        private Clases.ClassVariantes.VarianteFila nuevaFila(int m, int c)
        {
            bool iguales = rbIguales.Checked;
            return new Clases.ClassVariantes.VarianteFila
            {
                MedidaId = m,
                ColorId = c,
                MedidaNombre = m > 0 ? nombreMedida(m) : null,
                ColorNombre = c > 0 ? nombreColor(c) : null,
                Habilitada = true,
                Costo = iguales ? ficha.Costo : 0m,
                Precio = iguales ? ficha.Precio : 0m
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        // Grilla de combinaciones
        // ══════════════════════════════════════════════════════════════════════
        private void prepararGrilla()
        {
            // Los nombres pueden haber cambiado desde el ABM de medidas y colores
            foreach (var f in filas)
            {
                f.MedidaNombre = f.MedidaId > 0 ? nombreMedida(f.MedidaId) : null;
                f.ColorNombre = f.ColorId > 0 ? nombreColor(f.ColorId) : null;
            }

            dgv.DataSource = null;
            dgv.Columns.Clear();

            if (chkMedidas.Checked) dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "MedidaNombre", DataPropertyName = "MedidaNombre", HeaderText = "Medida", ReadOnly = true, FillWeight = 70 });
            if (chkColores.Checked) dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColorNombre", DataPropertyName = "ColorNombre", HeaderText = "Color", ReadOnly = true, FillWeight = 70 });
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Habilitada", DataPropertyName = "Habilitada", HeaderText = "Habilitada", FillWeight = 45 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "CodBarras", DataPropertyName = "CodBarras", HeaderText = "Cód. barras", FillWeight = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "CodProveedor", DataPropertyName = "CodProveedor", HeaderText = "Cód. proveedor", FillWeight = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Costo", DataPropertyName = "Costo", HeaderText = "Costo", FillWeight = 60, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Precio", DataPropertyName = "Precio", HeaderText = "Precio venta", FillWeight = 60, DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stock", DataPropertyName = "Stock", HeaderText = "Stock", FillWeight = 50, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });

            dgv.DataSource = filas;
            marcarSoloLectura();
            actualizarResumen();
        }

        // El stock de una variante existente no se edita aquí (se ajusta con el ajuste de stock)
        private void marcarSoloLectura()
        {
            if (!dgv.Columns.Contains("Stock")) return;
            foreach (DataGridViewRow r in dgv.Rows)
            {
                var f = r.DataBoundItem as Clases.ClassVariantes.VarianteFila;
                if (f == null) continue;
                DataGridViewCell c = r.Cells["Stock"];
                c.ReadOnly = f.ProductoId.HasValue;
                c.Style.BackColor = f.ProductoId.HasValue ? Color.Gainsboro : Color.Empty;
                r.DefaultCellStyle.ForeColor = f.Habilitada ? Color.Empty : Color.Gray;
            }
        }

        private void actualizarResumen()
        {
            int nuevas = filas.Count(f => !f.ProductoId.HasValue);
            int existentes = filas.Count - nuevas;
            int habilitadas = filas.Count(f => f.Habilitada);
            lblResumen.Text = filas.Count + " combinaciones (" + habilitadas + " habilitadas)" +
                              (existentes > 0 ? " — " + nuevas + " nueva(s)" : string.Empty);
            marcarSoloLectura();
        }

        private static string slug(string s)
        {
            string n = (s ?? string.Empty).Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char ch in n)
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            return Regex.Replace(sb.ToString().ToUpperInvariant(), "[^A-Z0-9]", string.Empty);
        }

        // Código de proveedor sugerido: NOMBRE-MEDIDA-COLOR (solo en filas habilitadas sin código)
        private void generarCodigos()
        {
            dgv.EndEdit();
            string b = slug(ficha.Descripcion);
            b = (b.Length > 10 ? b.Substring(0, 10) : b);
            if (b.Length == 0) b = "PROD";

            int n = 0;
            foreach (var f in filas)
            {
                if (!string.IsNullOrWhiteSpace(f.CodProveedor) || !f.Habilitada) continue;
                List<string> partes = new List<string> { b };
                if (chkMedidas.Checked && f.MedidaId > 0) partes.Add(recortar(slug(f.MedidaNombre), 8));
                if (chkColores.Checked && f.ColorId > 0) partes.Add(recortar(slug(f.ColorNombre), 8));
                string cod = string.Join("-", partes);
                f.CodProveedor = cod.Length > Clases.ClassVariantes.MaxCodigo ? cod.Substring(0, Clases.ClassVariantes.MaxCodigo) : cod;
                n++;
            }
            dgv.Refresh();
            lblResumen.Text += "   Códigos generados: " + n + ".";
        }

        private static string recortar(string s, int max)
        {
            return s.Length > max ? s.Substring(0, max) : s;
        }

        private void aplicarMasivo()
        {
            dgv.EndEdit();
            decimal costo, precio, stock;
            bool hayCosto = decimal.TryParse(txtBulkCosto.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out costo);
            bool hayPrecio = decimal.TryParse(txtBulkPrecio.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out precio);
            bool hayStock = decimal.TryParse(txtBulkStock.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out stock);

            if (!hayCosto && !hayPrecio && !hayStock)
            {
                MessageBox.Show(this, "Escriba un valor para aplicar.", "Variantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var objetivo = dgv.SelectedRows.Count > 0
                ? dgv.SelectedRows.Cast<DataGridViewRow>().Select(r => r.DataBoundItem).OfType<Clases.ClassVariantes.VarianteFila>().ToList()
                : filas.ToList();

            foreach (var f in objetivo)
            {
                if (hayCosto) f.Costo = costo;
                if (hayPrecio) f.Precio = precio;
                if (hayStock && !f.ProductoId.HasValue) f.Stock = stock;   // el stock de las existentes no se toca
            }
            dgv.Refresh();
        }

        // ══════════════════════════════════════════════════════════════════════
        // Maestros
        // ══════════════════════════════════════════════════════════════════════
        private void crearAtributo(bool esMedida)
        {
            TextBox txt = esMedida ? txtNuevaMedida : txtNuevoColor;
            int id;
            string error = esMedida
                ? inst.guardarMedida(0, txt.Text, 0, true, out id)
                : inst.guardarColor(0, txt.Text, null, 0, true, out id);

            if (error != null)
            {
                MessageBox.Show(this, error, "Variantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txt.Text = string.Empty;
            recargarMaestros();

            CheckedListBox clb = esMedida ? clbMedidas : clbColores;
            for (int i = 0; i < clb.Items.Count; i++)
                if (((ItemAtributo)clb.Items[i]).Id == id) clb.SetItemChecked(i, true);
        }

        private void abrirMaestros()
        {
            using (frmABMVariantesAtributos f = new frmABMVariantesAtributos())
                f.ShowDialog(this);
            recargarMaestros();
            if (paso == 4) prepararGrilla();
        }

        // Recarga las listas conservando lo marcado
        private void recargarMaestros()
        {
            HashSet<int> usadasM = new HashSet<int>(actual.Variantes.Select(v => v.MedidaId));
            HashSet<int> usadasC = new HashSet<int>(actual.Variantes.Select(v => v.ColorId));
            tablaMedidas = inst.traeMedidas(false);
            tablaColores = inst.traeColores(false);
            quitarInactivosNoUsados(tablaMedidas, usadasM);
            quitarInactivosNoUsados(tablaColores, usadasC);
            llenarLista(clbMedidas, tablaMedidas);
            llenarLista(clbColores, tablaColores);
        }

        // ══════════════════════════════════════════════════════════════════════
        // Guardado
        // ══════════════════════════════════════════════════════════════════════
        private void guardar()
        {
            dgv.EndEdit();

            int modo = Clases.ClassVariantes.PropagarNinguna;
            if (cboPropagacion.Visible && cboPropagacion.SelectedItem != null)
                modo = ((OpcionPropagacion)cboPropagacion.SelectedItem).Valor;

            Cursor = Cursors.WaitCursor;
            Clases.ClassVariantes.ResultadoGuardado res;
            try
            {
                res = inst.guardarFicha(ficha, chkMedidas.Checked, chkColores.Checked, rbIguales.Checked, modo, filas.ToList());
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            if (!res.Ok)
            {
                MessageBox.Show(this, "No se pudo guardar:\r\n\r\n• " + string.Join("\r\n• ", res.Errores),
                    "Variantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ficha.ProductoId = res.ProductoId;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
