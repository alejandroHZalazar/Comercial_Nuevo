using Comercial.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    /// <summary>
    /// Diálogo para seleccionar uno o varios productos mediante checkbox.
    /// Busca por descripción, código de barras y código de proveedor.
    /// </summary>
    public class frmPickProducto : Form
    {
        /// <summary>Primer id seleccionado (compatibilidad con código existente).</summary>
        public int IdProductoSeleccionado { get; private set; } = 0;

        /// <summary>Todos los ids seleccionados (marcados con el check).</summary>
        public List<int> IdsSeleccionados { get; private set; } = new List<int>();

        /// <summary>
        /// IDs que deben aparecer pre-chequeados al abrir el diálogo.
        /// Asignar antes de llamar a ShowDialog().
        /// </summary>
        public List<int> IdsPreseleccionados { get; set; } = new List<int>();

        private DataGridView dgv;
        private TextBox      txtFiltro;
        private Label        lblSeleccionados;
        private Button       btnOK;
        private Button       btnCancelar;
        private DataTable    dtProductos;

        private const string COL_SEL = "Sel";

        public frmPickProducto()
        {
            Text            = "Seleccionar Productos";
            Width           = 900;
            Height          = 540;
            StartPosition   = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            KeyPreview      = true;
            KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) confirmar(); };

            // — Barra de búsqueda —
            var lblBuscar = new Label
            {
                Text     = "Buscar:",
                Location = new Point(10, 13),
                AutoSize = true,
                Font     = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            txtFiltro = new TextBox
            {
                Location = new Point(65, 10),
                Width    = 500,
                Font     = new Font("Segoe UI", 10)
            };
            txtFiltro.TextChanged += (s, e) => filtrar();

            var lblHint = new Label
            {
                Text      = "Busca en descripción · cód. barras · cód. proveedor  |  ✓ clic para marcar",
                Location  = new Point(572, 13),
                AutoSize  = true,
                ForeColor = Color.Gray,
                Font      = new Font("Segoe UI", 7.5f)
            };

            // — Grid —
            dgv = new DataGridView
            {
                Location                    = new Point(10, 38),
                Size                        = new Size(864, 395),
                AllowUserToAddRows          = false,
                AllowUserToDeleteRows       = false,
                ReadOnly                    = false,
                SelectionMode               = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect                 = false,
                RowHeadersVisible           = false,
                AutoGenerateColumns         = false,
                AutoSizeColumnsMode         = DataGridViewAutoSizeColumnsMode.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                BackgroundColor             = SystemColors.Window,
                BorderStyle                 = BorderStyle.Fixed3D
            };

            // Columnas (orden visual)
            dgv.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = COL_SEL,
                Name             = COL_SEL,
                HeaderText       = "✓",
                Width            = 32,
                Resizable        = DataGridViewTriState.False
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                Name             = "id",
                Visible          = false
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "descripcion",
                Name             = "descripcion",
                HeaderText       = "Descripción",
                ReadOnly         = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "codBarras",
                Name             = "codBarras",
                HeaderText       = "Cód. Barras",
                ReadOnly         = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "codProveedor",
                Name             = "codProveedor",
                HeaderText       = "Cód. Proveedor",
                ReadOnly         = true
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName   = "precio",
                Name               = "precio",
                HeaderText         = "Precio",
                ReadOnly           = true,
                DefaultCellStyle   = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format    = "N2"
                }
            });

            // Confirmar el check inmediatamente al hacer clic en la celda checkbox
            dgv.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgv.IsCurrentCellDirty &&
                    dgv.CurrentCell?.OwningColumn?.Name == COL_SEL)
                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            // Clic en cualquier celda de datos → toggle del check de esa fila
            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex != dgv.Columns[COL_SEL].Index)
                {
                    DataRow dr = ((DataRowView)dgv.Rows[e.RowIndex].DataBoundItem).Row;
                    dr[COL_SEL] = !(dr[COL_SEL] as bool? ?? false);
                }
                actualizarContador();
            };

            dgv.CellValueChanged += (s, e) =>
            {
                if (e.RowIndex >= 0 &&
                    dgv.Columns[e.ColumnIndex].Name == COL_SEL)
                    actualizarContador();
            };

            // — Pie de formulario —
            lblSeleccionados = new Label
            {
                Text      = "0 producto(s) seleccionado(s)",
                Location  = new Point(10, 443),
                Size      = new Size(500, 22),
                ForeColor = Color.DimGray,
                Font      = new Font("Segoe UI", 9)
            };

            btnOK = new Button
            {
                Text     = "Agregar seleccionados",
                Location = new Point(630, 440),
                Width    = 155,
                Height   = 30,
                Font     = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOK.Click += (s, e) => confirmar();

            btnCancelar = new Button
            {
                Text         = "Cancelar",
                Location     = new Point(793, 440),
                Width        = 81,
                Height       = 30,
                Font         = new Font("Segoe UI", 9),
                DialogResult = DialogResult.Cancel
            };
            btnCancelar.Click += (s, e) => Close();

            Controls.AddRange(new Control[]
            {
                lblBuscar, txtFiltro, lblHint,
                dgv,
                lblSeleccionados, btnOK, btnCancelar
            });

            Load += (s, e) => cargarProductos();
        }

        // ── Carga inicial ─────────────────────────────────────────────────────
        private void cargarProductos()
        {
            ClassProductos instProd = new ClassProductos();
            dtProductos = instProd.traeProductosPpal(
                " where baja = 0 and IFNULL(p.esPromocion, 0) = 0 order by descripcion");

            // Columna de selección en el DataTable (persiste al filtrar)
            if (!dtProductos.Columns.Contains(COL_SEL))
                dtProductos.Columns.Add(COL_SEL, typeof(bool));
            foreach (DataRow row in dtProductos.Rows)
            {
                int idFila = int.Parse(row["id"].ToString());
                row[COL_SEL] = IdsPreseleccionados != null && IdsPreseleccionados.Contains(idFila);
            }

            dgv.DataSource = dtProductos.DefaultView;

            ajustarAnchoColumnas();
            actualizarContador();
            txtFiltro.Focus();
        }

        // ── Filtrado ──────────────────────────────────────────────────────────
        private void filtrar()
        {
            if (dtProductos == null) return;
            string f = txtFiltro.Text.Trim().Replace("'", "''");
            dtProductos.DefaultView.RowFilter = string.IsNullOrEmpty(f)
                ? ""
                : $"descripcion LIKE '%{f}%' OR codBarras LIKE '%{f}%' OR codProveedor LIKE '%{f}%'";
        }

        // ── Auto-ancho de columnas según contenido ────────────────────────────
        private void ajustarAnchoColumnas()
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Name == COL_SEL || !col.Visible) continue;
                dgv.AutoResizeColumn(col.Index, DataGridViewAutoSizeColumnMode.AllCells);
                // Mínimo razonable para descripción
                if (col.Name == "descripcion" && col.Width < 280)
                    col.Width = 280;
            }
            dgv.Columns[COL_SEL].Width = 32;
        }

        // ── Contador de marcados (sobre todo el DataTable, no solo los visibles) ──
        private void actualizarContador()
        {
            if (dtProductos == null) return;
            int n = dtProductos.Rows.Cast<DataRow>()
                                    .Count(r => r[COL_SEL] as bool? == true);
            lblSeleccionados.Text = $"{n} producto(s) seleccionado(s)";
            lblSeleccionados.ForeColor = n > 0 ? Color.DarkGreen : Color.DimGray;
        }

        // ── Confirmar selección ───────────────────────────────────────────────
        private void confirmar()
        {
            if (dtProductos == null) return;

            IdsSeleccionados.Clear();
            foreach (DataRow row in dtProductos.Rows)
                if (row[COL_SEL] as bool? == true)
                    IdsSeleccionados.Add(int.Parse(row["id"].ToString()));

            if (IdsSeleccionados.Count == 0) return;

            IdProductoSeleccionado = IdsSeleccionados[0];
            DialogResult = DialogResult.OK;
            Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmPickProducto
            // 
            this.ClientSize = new System.Drawing.Size(300, 261);
            this.Name = "frmPickProducto";
            this.Load += new System.EventHandler(this.frmPickProducto_Load);
            this.ResumeLayout(false);

        }

        private void frmPickProducto_Load(object sender, EventArgs e)
        {

        }
    }
}
