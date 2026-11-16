using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    /// <summary>
    /// ABM de los maestros de variantes: medidas (talles, tamaños) y colores.
    /// Se usa desde el editor de variantes de producto. Renombrar una medida o un color
    /// regenera la descripción de los productos que la usan.
    /// </summary>
    public class frmABMVariantesAtributos : Form
    {
        private readonly Clases.ClassVariantes inst = new Clases.ClassVariantes();

        // Una pestaña por tipo de atributo (misma estructura, con el color hexadecimal solo en colores)
        private class Pestana
        {
            public bool EsColor;
            public DataGridView Grilla;
            public TextBox Nombre;
            public NumericUpDown Orden;
            public CheckBox Activa;
            public TextBox Hex;
            public Panel Muestra;
            public int IdActual;
            public Label Estado;
        }

        private readonly Pestana medidas = new Pestana { EsColor = false };
        private readonly Pestana colores = new Pestana { EsColor = true };

        public frmABMVariantesAtributos()
        {
            Text = "Medidas y colores";
            Font = new Font("Segoe UI", 9F);
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(640, 480);
            MinimumSize = new Size(560, 440);
            KeyPreview = true;

            TabControl tabs = new TabControl { Dock = DockStyle.Fill };
            TabPage tpM = new TabPage("Medidas");
            TabPage tpC = new TabPage("Colores");
            tabs.TabPages.Add(tpM);
            tabs.TabPages.Add(tpC);
            Controls.Add(tabs);

            construir(tpM, medidas);
            construir(tpC, colores);

            Load += (s, e) => { recargar(medidas); recargar(colores); };
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };
        }

        private void construir(TabPage tab, Pestana p)
        {
            string que = p.EsColor ? "color" : "medida";

            // Panel de edición (abajo)
            Panel editor = new Panel { Dock = DockStyle.Bottom, Height = 150, Padding = new Padding(8) };
            tab.Controls.Add(editor);

            Label lblN = new Label { Text = "Nombre", AutoSize = true, Location = new Point(10, 14) };
            p.Nombre = new TextBox { Location = new Point(70, 10), Width = 250, MaxLength = Clases.ClassVariantes.MaxNombreAtributo };
            Label lblO = new Label { Text = "Orden", AutoSize = true, Location = new Point(335, 14) };
            p.Orden = new NumericUpDown { Location = new Point(380, 10), Width = 70, Minimum = 0, Maximum = 9999 };
            p.Activa = new CheckBox { Text = "Habilitada", Checked = true, AutoSize = true, Location = new Point(470, 12) };
            editor.Controls.AddRange(new Control[] { lblN, p.Nombre, lblO, p.Orden, p.Activa });

            if (p.EsColor)
            {
                Label lblH = new Label { Text = "Color", AutoSize = true, Location = new Point(10, 48) };
                p.Hex = new TextBox { Location = new Point(70, 44), Width = 90, MaxLength = 7 };
                p.Muestra = new Panel { Location = new Point(168, 44), Size = new Size(26, 24), BorderStyle = BorderStyle.FixedSingle };
                Button btnElegir = new Button { Text = "Elegir...", Location = new Point(202, 43), Size = new Size(92, 26) };
                Label lblAyuda = new Label { Text = "(opcional, formato #RRGGBB)", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(302, 48) };
                p.Hex.TextChanged += (s, e) => actualizarMuestra(p);
                Clases.IconosVariantes.Aplicar(btnElegir, Properties.Resources.settings);
                btnElegir.Click += (s, e) =>
                {
                    using (ColorDialog cd = new ColorDialog { FullOpen = true })
                    {
                        if (cd.ShowDialog(this) == DialogResult.OK)
                            p.Hex.Text = "#" + cd.Color.R.ToString("X2") + cd.Color.G.ToString("X2") + cd.Color.B.ToString("X2");
                    }
                };
                editor.Controls.AddRange(new Control[] { lblH, p.Hex, p.Muestra, btnElegir, lblAyuda });
            }

            Button btnNuevo = new Button { Text = "Nueva " + que, Location = new Point(10, 100), Size = new Size(110, 30) };
            Button btnGuardar = new Button { Text = "Guardar", Location = new Point(130, 100), Size = new Size(110, 30) };
            p.Estado = new Label { AutoSize = true, Location = new Point(255, 108), ForeColor = Color.DimGray, Text = "Nueva " + que };
            Clases.IconosVariantes.Aplicar(btnNuevo, Properties.Resources.plus);
            Clases.IconosVariantes.Aplicar(btnGuardar, Properties.Resources.save__1_);
            btnNuevo.Click += (s, e) => limpiar(p);
            btnGuardar.Click += (s, e) => guardar(p);
            editor.Controls.AddRange(new Control[] { btnNuevo, btnGuardar, p.Estado });

            // Grilla (arriba)
            p.Grilla = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                EnableHeadersVisualStyles = false
            };
            p.Grilla.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            p.Grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            p.Grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DimGray;
            p.Grilla.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(242, 242, 242) };
            p.Grilla.SelectionChanged += (s, e) => cargarSeleccion(p);
            tab.Controls.Add(p.Grilla);
            p.Grilla.BringToFront();
        }

        private void recargar(Pestana p)
        {
            int idSel = p.IdActual;
            DataTable t = p.EsColor ? inst.traeColores(false) : inst.traeMedidas(false);
            p.Grilla.DataSource = t;
            if (p.Grilla.Columns.Contains("id")) p.Grilla.Columns["id"].Visible = false;
            if (p.Grilla.Columns.Contains("nombre")) p.Grilla.Columns["nombre"].HeaderText = "Nombre";
            if (p.Grilla.Columns.Contains("codigoHex")) p.Grilla.Columns["codigoHex"].HeaderText = "Color";
            if (p.Grilla.Columns.Contains("orden")) p.Grilla.Columns["orden"].HeaderText = "Orden";
            if (p.Grilla.Columns.Contains("activa")) p.Grilla.Columns["activa"].HeaderText = "Habilitada";
            if (p.Grilla.Columns.Contains("enUso")) p.Grilla.Columns["enUso"].HeaderText = "Variantes que la usan";

            // Vuelve a seleccionar el registro que se estaba editando
            bool encontrado = false;
            foreach (DataGridViewRow fila in p.Grilla.Rows)
                if (idSel > 0 && Convert.ToInt32(fila.Cells["id"].Value) == idSel)
                {
                    fila.Selected = true;
                    p.Grilla.CurrentCell = fila.Cells[p.Grilla.Columns["nombre"].Index];
                    encontrado = true;
                    break;
                }
            if (!encontrado) limpiar(p);
        }

        private void cargarSeleccion(Pestana p)
        {
            if (p.Grilla.CurrentRow == null || p.Grilla.CurrentRow.Cells["id"].Value == null) return;
            DataGridViewRow f = p.Grilla.CurrentRow;
            p.IdActual = Convert.ToInt32(f.Cells["id"].Value);
            p.Nombre.Text = Convert.ToString(f.Cells["nombre"].Value);
            p.Orden.Value = Math.Max(p.Orden.Minimum, Math.Min(p.Orden.Maximum, Convert.ToDecimal(f.Cells["orden"].Value)));
            p.Activa.Checked = Convert.ToInt32(f.Cells["activa"].Value) == 1;
            if (p.EsColor) p.Hex.Text = Convert.ToString(f.Cells["codigoHex"].Value);
            p.Estado.Text = "Modificando: " + p.Nombre.Text;
        }

        private void limpiar(Pestana p)
        {
            p.IdActual = 0;
            p.Nombre.Text = string.Empty;
            p.Orden.Value = 0;
            p.Activa.Checked = true;
            if (p.EsColor) p.Hex.Text = string.Empty;
            p.Grilla.ClearSelection();
            p.Estado.Text = "Nueva " + (p.EsColor ? "color" : "medida");
            p.Nombre.Focus();
        }

        private static void actualizarMuestra(Pestana p)
        {
            try
            {
                string h = p.Hex.Text.Trim();
                p.Muestra.BackColor = System.Text.RegularExpressions.Regex.IsMatch(h, "^#[0-9A-Fa-f]{6}$")
                    ? ColorTranslator.FromHtml(h)
                    : SystemColors.Control;
            }
            catch { p.Muestra.BackColor = SystemColors.Control; }
        }

        private void guardar(Pestana p)
        {
            int idGuardado;
            string error = p.EsColor
                ? inst.guardarColor(p.IdActual, p.Nombre.Text, p.Hex.Text, (int)p.Orden.Value, p.Activa.Checked, out idGuardado)
                : inst.guardarMedida(p.IdActual, p.Nombre.Text, (int)p.Orden.Value, p.Activa.Checked, out idGuardado);

            if (error != null)
            {
                MessageBox.Show(this, error, "Medidas y colores", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            p.IdActual = idGuardado;
            recargar(p);
        }
    }
}
