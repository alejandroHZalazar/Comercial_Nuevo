using Comercial.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    /// <summary>
    /// Diálogo para seleccionar uno o varios productos de la lista.
    /// Permite selección múltiple con Ctrl/Shift.
    /// Retorna los ids seleccionados en IdsSeleccionados (y el primero en IdProductoSeleccionado).
    /// </summary>
    public class frmPickProducto : Form
    {
        /// <summary>Primer producto seleccionado (compatibilidad con código existente).</summary>
        public int IdProductoSeleccionado { get; private set; } = 0;

        /// <summary>Todos los productos seleccionados.</summary>
        public List<int> IdsSeleccionados { get; private set; } = new List<int>();

        private DataGridView dgv;
        private TextBox txtFiltro;
        private Label lblSeleccionados;
        private Button btnOK;
        private Button btnCancelar;
        private DataTable dtProductos;

        public frmPickProducto()
        {
            Text = "Seleccionar Productos";
            Width = 620; Height = 480;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            KeyPreview = true;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) confirmar(); };

            Label lbl = new Label { Text = "Buscar:", Location = new System.Drawing.Point(10, 12), AutoSize = true };

            txtFiltro = new TextBox { Location = new System.Drawing.Point(65, 9), Width = 300 };
            txtFiltro.TextChanged += (s, e) => filtrar();

            Label lblHint = new Label
            {
                Text = "Ctrl+clic o Shift+clic para seleccionar varios",
                Location = new System.Drawing.Point(375, 12),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Gray,
                Font = new System.Drawing.Font("Segoe UI", 7.5f)
            };

            dgv = new DataGridView
            {
                Location = new System.Drawing.Point(10, 38),
                Size = new System.Drawing.Size(585, 350),
                AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.DoubleClick += (s, e) => confirmar();
            dgv.SelectionChanged += (s, e) => actualizarContador();

            lblSeleccionados = new Label
            {
                Text = "0 producto(s) seleccionado(s)",
                Location = new System.Drawing.Point(10, 398),
                Size = new System.Drawing.Size(260, 20),
                ForeColor = System.Drawing.Color.DimGray
            };

            btnOK = new Button
            {
                Text = "Agregar seleccionados",
                Location = new System.Drawing.Point(390, 393),
                Width = 130, Height = 28
            };
            btnOK.Click += (s, e) => confirmar();

            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(528, 393),
                Width = 70, Height = 28,
                DialogResult = DialogResult.Cancel
            };
            btnCancelar.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { lbl, txtFiltro, lblHint, dgv, lblSeleccionados, btnOK, btnCancelar });

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
            dv.RowFilter = string.IsNullOrEmpty(f) ? "" :
                string.Format("descripcion LIKE '%{0}%' OR codBarras LIKE '%{0}%' OR codProveedor LIKE '%{0}%'", f);
            dgv.DataSource = dv;

            if (dgv.Columns.Contains("id"))           dgv.Columns["id"].Visible = false;
            if (dgv.Columns.Contains("descripcion"))  dgv.Columns["descripcion"].HeaderText = "Descripción";
            if (dgv.Columns.Contains("codBarras"))    dgv.Columns["codBarras"].HeaderText = "Cód. Barras";
            if (dgv.Columns.Contains("codProveedor")) dgv.Columns["codProveedor"].HeaderText = "Cód. Proveedor";
            if (dgv.Columns.Contains("precio"))       dgv.Columns["precio"].HeaderText = "Precio";

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                string n = col.Name.ToLower();
                if (n == "cantidad" || n == "cantidadminima" || n == "costo" || n == "fraccionado" ||
                    n == "dolarizado" || n == "baja" || n == "fk_rubro" || n == "fk_proveedor" ||
                    n == "p_proveedor" || n == "espromocion")
                    col.Visible = false;
            }
        }

        private void actualizarContador()
        {
            int n = dgv.SelectedRows.Count;
            lblSeleccionados.Text = n + " producto(s) seleccionado(s)";
        }

        private void confirmar()
        {
            if (dgv.SelectedRows.Count == 0) return;

            IdsSeleccionados.Clear();
            foreach (DataGridViewRow row in dgv.SelectedRows)
                IdsSeleccionados.Add(int.Parse(row.Cells["id"].Value.ToString()));

            IdProductoSeleccionado = IdsSeleccionados[0];
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
