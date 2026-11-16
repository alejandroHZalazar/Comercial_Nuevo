using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    /// <summary>
    /// Alta / modificación de una promoción por cantidad (NxM: "lleva N, paga M") o por porcentaje de descuento.
    /// Mismas reglas y validaciones que Comercial Web (ClassPromocionesDescuentos.guardar).
    /// </summary>
    public class frmAltaModifPromocionDescuento : Form
    {
        private readonly Clases.ClassPromocionesDescuentos inst = new Clases.ClassPromocionesDescuentos();

        /// <summary>0 = promoción nueva; > 0 = edición.</summary>
        public int IdRegla = 0;

        private readonly List<int> productoIds = new List<int>();

        private TextBox txtNombre;
        private ComboBox cboTipo;
        private Label lblPorc, lblLleva, lblPaga;
        private NumericUpDown nudPorc, nudLleva, nudPaga;
        private DateTimePicker dtpDesde, dtpHasta;
        private CheckBox chkActiva;
        private DataGridView dgv;
        private Label lblCantidad;
        private ErrorProvider errores = new ErrorProvider();

        public frmAltaModifPromocionDescuento()
        {
            Text = "Promoción por cantidad / descuento";
            Font = new Font("Segoe UI", 9F);
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(860, 600);
            MinimumSize = new Size(760, 520);
            KeyPreview = true;

            construir();

            Load += (s, e) => cargar();
            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F5) guardar();
                else if (e.KeyCode == Keys.Escape) { DialogResult = DialogResult.Cancel; Close(); }
            };
        }

        private void construir()
        {
            GroupBox gbDatos = new GroupBox { Text = "Datos de la promoción", Dock = DockStyle.Top, Height = 168, Padding = new Padding(10) };

            gbDatos.Controls.Add(new Label { Text = "Nombre", AutoSize = true, Location = new Point(12, 28) });
            txtNombre = new TextBox { Location = new Point(70, 24), Width = 360, MaxLength = 100, CharacterCasing = CharacterCasing.Upper };
            chkActiva = new CheckBox { Text = "Activa", Checked = true, AutoSize = true, Location = new Point(450, 26) };
            gbDatos.Controls.AddRange(new Control[] { txtNombre, chkActiva });

            gbDatos.Controls.Add(new Label { Text = "Tipo", AutoSize = true, Location = new Point(12, 62) });
            cboTipo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(70, 58), Width = 200 };
            cboTipo.Items.Add(new Opcion("Porcentaje de descuento", Clases.TipoPromo.Porcentaje));
            cboTipo.Items.Add(new Opcion("Lleva N, paga M (NxM)", Clases.TipoPromo.NxM));
            cboTipo.SelectedIndex = 0;
            cboTipo.SelectedIndexChanged += (s, e) => actualizarTipo();

            lblPorc = new Label { Text = "% de descuento", AutoSize = true, Location = new Point(290, 62) };
            nudPorc = new NumericUpDown { Location = new Point(390, 58), Width = 80, DecimalPlaces = 2, Minimum = 0, Maximum = 100, Value = 10 };
            lblLleva = new Label { Text = "Lleva", AutoSize = true, Location = new Point(290, 62) };
            nudLleva = new NumericUpDown { Location = new Point(335, 58), Width = 60, Minimum = 2, Maximum = 99, Value = 2 };
            lblPaga = new Label { Text = "Paga", AutoSize = true, Location = new Point(410, 62) };
            nudPaga = new NumericUpDown { Location = new Point(452, 58), Width = 60, Minimum = 1, Maximum = 98, Value = 1 };
            gbDatos.Controls.AddRange(new Control[] { cboTipo, lblPorc, nudPorc, lblLleva, nudLleva, lblPaga, nudPaga });

            gbDatos.Controls.Add(new Label { Text = "Vigente desde", AutoSize = true, Location = new Point(12, 100) });
            dtpDesde = new DateTimePicker { Location = new Point(100, 96), Width = 160, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
            gbDatos.Controls.Add(new Label { Text = "hasta", AutoSize = true, Location = new Point(280, 100) });
            dtpHasta = new DateTimePicker { Location = new Point(320, 96), Width = 160, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
            gbDatos.Controls.AddRange(new Control[] { dtpDesde, dtpHasta });
            gbDatos.Controls.Add(new Label
            {
                Text = "Hora de Argentina. Fuera de la vigencia la promoción nunca se aplica, aunque esté activa.",
                AutoSize = true, ForeColor = Color.DimGray, Location = new Point(12, 132)
            });

            GroupBox gbProd = new GroupBox { Text = "Productos alcanzados", Dock = DockStyle.Fill, Padding = new Padding(8) };

            Panel barra = new Panel { Dock = DockStyle.Top, Height = 38 };
            Button btnAgregar = new Button { Text = "Agregar productos...", Size = new Size(170, 30), Location = new Point(0, 4) };
            Button btnQuitar = new Button { Text = "Quitar seleccionados", Size = new Size(170, 30), Location = new Point(180, 4) };
            lblCantidad = new Label { AutoSize = true, Location = new Point(370, 12), ForeColor = Color.DimGray };
            barra.Controls.AddRange(new Control[] { btnAgregar, btnQuitar, lblCantidad });
            Clases.IconosVariantes.Aplicar(btnAgregar, Properties.Resources.plus);
            Clases.IconosVariantes.Aplicar(btnQuitar, Properties.Resources.rubbish_bin__1_);
            btnAgregar.Click += (s, e) => agregarProductos();
            btnQuitar.Click += (s, e) => quitarSeleccionados();

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false, RowHeadersVisible = false, MultiSelect = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle, EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DimGray;
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(242, 242, 242) };
            dgv.KeyDown += (s, e) => { if (e.KeyCode == Keys.Delete) { quitarSeleccionados(); e.Handled = true; } };

            gbProd.Controls.Add(dgv);
            gbProd.Controls.Add(barra);
            dgv.BringToFront();

            Panel pie = new Panel { Dock = DockStyle.Bottom, Height = 54 };
            Button btnGuardar = new Button { Text = "Guardar (F5)", Size = new Size(130, 34), Anchor = AnchorStyles.Right | AnchorStyles.Bottom };
            Button btnCancelar = new Button { Text = "Cancelar", Size = new Size(110, 34), Anchor = AnchorStyles.Right | AnchorStyles.Bottom };
            pie.Controls.AddRange(new Control[] { btnGuardar, btnCancelar });
            pie.Resize += (s, e) =>
            {
                btnCancelar.Location = new Point(pie.ClientSize.Width - btnCancelar.Width - 12, 10);
                btnGuardar.Location = new Point(btnCancelar.Left - btnGuardar.Width - 8, 10);
            };
            btnCancelar.Location = new Point(ClientSize.Width - btnCancelar.Width - 12, 10);
            btnGuardar.Location = new Point(btnCancelar.Left - btnGuardar.Width - 8, 10);
            Clases.IconosVariantes.Aplicar(btnGuardar, Properties.Resources.save__1_);
            Clases.IconosVariantes.Aplicar(btnCancelar, Properties.Resources.back_arrow);
            btnGuardar.Click += (s, e) => guardar();
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(gbProd);
            Controls.Add(pie);
            Controls.Add(gbDatos);
            gbProd.BringToFront();
        }

        private class Opcion
        {
            public string Texto; public string Valor;
            public Opcion(string t, string v) { Texto = t; Valor = v; }
            public override string ToString() { return Texto; }
        }

        private string tipoActual { get { return ((Opcion)cboTipo.SelectedItem).Valor; } }

        private void actualizarTipo()
        {
            bool porc = tipoActual == Clases.TipoPromo.Porcentaje;
            lblPorc.Visible = nudPorc.Visible = porc;
            lblLleva.Visible = nudLleva.Visible = lblPaga.Visible = nudPaga.Visible = !porc;
        }

        private void cargar()
        {
            actualizarTipo();

            if (IdRegla > 0)
            {
                Clases.ClassPromocionesDescuentos.ReglaDetalle r = inst.obtener(IdRegla);
                if (r == null)
                {
                    MessageBox.Show(this, "La promoción no existe.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.Cancel; Close(); return;
                }

                txtNombre.Text = r.Nombre;
                cboTipo.SelectedIndex = r.Tipo == Clases.TipoPromo.NxM ? 1 : 0;
                if (r.Porcentaje.HasValue) nudPorc.Value = r.Porcentaje.Value;
                if (r.Lleva.HasValue) nudLleva.Value = r.Lleva.Value;
                if (r.Paga.HasValue) nudPaga.Value = r.Paga.Value;
                dtpDesde.Value = r.Desde;
                dtpHasta.Value = r.Hasta;
                chkActiva.Checked = r.Activa;
                productoIds.AddRange(r.ProductoIds);
            }
            else
            {
                DateTime ahora = Clases.ClassPromocionesDescuentos.ahoraArgentina();
                dtpDesde.Value = new DateTime(ahora.Year, ahora.Month, ahora.Day, ahora.Hour, ahora.Minute, 0);
                DateTime fin = ahora.Date.AddDays(30);
                dtpHasta.Value = new DateTime(fin.Year, fin.Month, fin.Day, 23, 59, 0);
            }
            refrescarProductos();
        }

        private void refrescarProductos()
        {
            DataTable t = inst.productosConPrecio(productoIds);
            dgv.DataSource = t;
            if (dgv.Columns.Contains("id")) dgv.Columns["id"].Visible = false;
            Dictionary<string, string> cab = new Dictionary<string, string>
            {
                { "codProveedor", "Cód. proveedor" }, { "codBarras", "Cód. barras" }, { "descripcion", "Descripción" },
                { "proveedor", "Proveedor" }, { "rubro", "Rubro" }, { "precio", "Precio s/IVA" }
            };
            foreach (var kv in cab)
                if (dgv.Columns.Contains(kv.Key)) dgv.Columns[kv.Key].HeaderText = kv.Value;
            if (dgv.Columns.Contains("precio"))
            {
                dgv.Columns["precio"].DefaultCellStyle.Format = "N2";
                dgv.Columns["precio"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            lblCantidad.Text = productoIds.Count + " producto(s)";
        }

        private void agregarProductos()
        {
            using (frmPickProducto pick = new frmPickProducto())
            {
                pick.IdsPreseleccionados = new List<int>(productoIds);
                if (pick.ShowDialog(this) != DialogResult.OK) return;

                // La selección devuelta reemplaza a la anterior de los productos que el buscador lista
                foreach (int id in pick.IdsSeleccionados)
                    if (!productoIds.Contains(id)) productoIds.Add(id);
            }
            refrescarProductos();
        }

        private void quitarSeleccionados()
        {
            if (dgv.SelectedRows.Count == 0) return;
            foreach (DataGridViewRow fila in dgv.SelectedRows)
                productoIds.Remove(Convert.ToInt32(fila.Cells["id"].Value));
            refrescarProductos();
        }

        private void guardar()
        {
            dgv.EndEdit();
            errores.Clear();

            Clases.ClassPromocionesDescuentos.ReglaDetalle dto = new Clases.ClassPromocionesDescuentos.ReglaDetalle
            {
                Id = IdRegla,
                Nombre = txtNombre.Text,
                Tipo = tipoActual,
                Porcentaje = tipoActual == Clases.TipoPromo.Porcentaje ? (decimal?)nudPorc.Value : null,
                Lleva = tipoActual == Clases.TipoPromo.NxM ? (int?)nudLleva.Value : null,
                Paga = tipoActual == Clases.TipoPromo.NxM ? (int?)nudPaga.Value : null,
                Desde = dtpDesde.Value,
                Hasta = dtpHasta.Value,
                Activa = chkActiva.Checked,
                ProductoIds = new List<int>(productoIds)
            };

            try
            {
                Cursor = Cursors.WaitCursor;
                inst.guardar(dto, Environment.GetEnvironmentVariable("nombreUser"));
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo guardar la promoción: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
