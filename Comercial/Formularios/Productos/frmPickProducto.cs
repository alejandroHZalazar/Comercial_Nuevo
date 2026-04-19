using Comercial.Clases;
using System;
using System.Data;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    /// <summary>
    /// Diálogo simple para seleccionar un producto de la lista.
    /// Retorna idProducto seleccionado en la propiedad IdProductoSeleccionado.
    /// </summary>
    public class frmPickProducto : Form
    {
        public int IdProductoSeleccionado { get; private set; } = 0;

        private DataGridView dgv;
        private TextBox txtFiltro;
        private Button btnOK;
        private Button btnCancelar;
        private DataTable dtProductos;

        public frmPickProducto()
        {
            Text = "Seleccionar Producto";
            Width = 600; Height = 450;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;

            Label lbl = new Label { Text = "Buscar:", Location = new System.Drawing.Point(10, 12), AutoSize = true };

            txtFiltro = new TextBox { Location = new System.Drawing.Point(65, 9), Width = 300 };
            txtFiltro.TextChanged += (s, e) => filtrar();

            dgv = new DataGridView
            {
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(565, 340),
                AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false, RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.DoubleClick += (s, e) => confirmar();

            btnOK = new Button { Text = "Seleccionar", Location = new System.Drawing.Point(390, 390), Width = 90, Height = 28 };
            btnOK.Click += (s, e) => confirmar();

            btnCancelar = new Button { Text = "Cancelar", Location = new System.Drawing.Point(490, 390), Width = 80, Height = 28 };
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lbl, txtFiltro, dgv, btnOK, btnCancelar });

            Load += (s, e) => cargarProductos();
        }

        private void cargarProductos()
        {
            ClassProductos instProd = new ClassProductos();
            dtProductos = instProd.traeProductosPpal(" where baja = 0 order by descripcion");
            filtrar();
        }

        private void filtrar()
        {
            if (dtProductos == null) return;
            string f = txtFiltro.Text.Trim();
            DataView dv = dtProductos.DefaultView;
            dv.RowFilter = string.IsNullOrEmpty(f) ? "" : $"descripcion LIKE '%{f}%' OR codBarras LIKE '%{f}%' OR codProveedor LIKE '%{f}%'";
            dgv.DataSource = dv;

            if (dgv.Columns.Contains("id")) dgv.Columns["id"].Visible = false;
            if (dgv.Columns.Contains("descripcion")) dgv.Columns["descripcion"].HeaderText = "Descripción";
            if (dgv.Columns.Contains("codBarras")) dgv.Columns["codBarras"].HeaderText = "Cód. Barras";
            if (dgv.Columns.Contains("codProveedor")) dgv.Columns["codProveedor"].HeaderText = "Cód. Proveedor";
            if (dgv.Columns.Contains("precio")) dgv.Columns["precio"].HeaderText = "Precio";
            // Ocultar columnas técnicas
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                string n = col.Name.ToLower();
                if (n == "cantidad" || n == "cantidadminima" || n == "costo" || n == "fraccionado" || n == "dolarizado" || n == "baja" || n == "fk_rubro" || n == "fk_proveedor" || n == "p_proveedor" || n == "espromocion")
                    col.Visible = false;
            }
        }

        private void confirmar()
        {
            if (dgv.CurrentRow == null) return;
            var row = dgv.CurrentRow;
            IdProductoSeleccionado = int.Parse(row.Cells["id"].Value.ToString());
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
