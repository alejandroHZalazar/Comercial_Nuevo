using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Comercial.Clases.ClassReportesITextSharp;

namespace Comercial.Formularios.Productos
{
    public partial class frmImprimirEtiquetasyCB : Form
    {
        private bool _cambiandoCheck = false;
        public frmImprimirEtiquetasyCB()
        {
            InitializeComponent();
            dgvProductos.CellValidating += dgvProductos_CellValidating;
            dgvProductos.CellValueChanged += dgvProductos_CellValueChanged;
            dgvProductos.CurrentCellDirtyStateChanged += dgvProductos_CurrentCellDirtyStateChanged;
            dgvProductos.DataError += (s, e) => { e.ThrowException = false; };
        }

        private void frmImprimirEtiquetasyCB_Load(object sender, EventArgs e)
        {
            estadoInicial();

        }

        private void estadoInicial()
        {
            dgvProductos.Rows.Clear();
            txtFiltro.Text = string.Empty;
            cargarCombos();
            cbTodos.Checked = false;
            cbNinguno.Checked = false;
            progressBar1.Visible = false;
            progressBar1.Style = ProgressBarStyle.Continuous;
        }

        private void cargarCombos()
        {
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();
            cboProveedores.DataSource = instProv.traeProveedoresconTodos();
            cboProveedores.ValueMember = "id";
            cboProveedores.DisplayMember = "nombreComercial";
            cboProveedores.SelectedValue = 0;

            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            cboRubros.DataSource = instConfig.traeRubrosJerarquiaConTodos();
            cboRubros.ValueMember = "id";
            cboRubros.DisplayMember = "ruta";
            cboRubros.DropDownWidth = 450;
            cboRubros.SelectedValue = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Clases.ClassProductos instProd = new Clases.ClassProductos();
            var productos = instProd.BuscarProductosEtiquetasCodigosBarra(int.Parse(cboProveedores.SelectedValue.ToString()), int.Parse(cboRubros.SelectedValue.ToString()), cboFiltro.Text, txtFiltro.Text.Trim());

            if (productos == null) return;
            dgvProductos.DataSource = null;
            dgvProductos.DataSource = productos;
            dgvProductos.Columns["id"].ReadOnly = true;
            dgvProductos.Columns["CodProveedor"].ReadOnly = true;
            dgvProductos.Columns["CodBarras"].ReadOnly = true;
            dgvProductos.Columns["Descripcion"].ReadOnly = true;
            dgvProductos.Columns["Precio"].ReadOnly = true;
            dgvProductos.Columns["NombreComercial"].ReadOnly = true;
            dgvProductos.Columns["Rubro"].ReadOnly = true;

            dgvProductos.Columns["Cantidad"].HeaderText = "Cant.";

            cbTodos.Checked = false;
            cbNinguno.Checked = false;

            // Si hay cantidad "para todos" vigente, se aplica a la nueva grilla
            if (txtCantGlobal.Text.Trim().Length > 0)
                txtCantGlobal_TextChanged(null, null);
            else
                actualizarTotal();
        }

        private void cbTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (_cambiandoCheck) return;

            _cambiandoCheck = true;

            if (cbTodos.Checked)
            {
                cbNinguno.Checked = false;
                MarcarFilas(true);
            }

            _cambiandoCheck = false;
        }

        private void cbNinguno_CheckedChanged(object sender, EventArgs e)
        {
            if (_cambiandoCheck) return;

            _cambiandoCheck = true;

            if (cbNinguno.Checked)
            {
                cbTodos.Checked = false;
                MarcarFilas(false);
            }

            _cambiandoCheck = false;
        }

        private void MarcarFilas(bool estado)
        {
            _actualizandoCant = true;
            try
            {
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    fila.Cells["sel"].Value = estado;
                }
            }
            finally
            {
                _actualizandoCant = false;
            }
            actualizarTotal();

            dgvProductos.RefreshEdit();
        }

        // ── Cantidad de etiquetas: entero 1..CANT_MAX por producto, tope total por impresión ──
        private const int CANT_MAX = 500;
        private const int TOTAL_MAX = 5000;
        private bool _globalInvalida = false;
        private bool _actualizandoCant = false;

        private static bool cantidadValida(string texto, out int valor)
        {
            valor = 0;
            string t = (texto ?? string.Empty).Trim();
            if (t.Length == 0) return false;
            foreach (char c in t) if (c < '0' || c > '9') return false;   // vacío, decimales, negativos, texto
            return int.TryParse(t, out valor) && valor >= 1 && valor <= CANT_MAX;
        }

        private static string mensajeCantidadInvalida()
        {
            return "Cantidad de etiquetas inválida: ingrese un número entero entre 1 y " + CANT_MAX + " (sin decimales, ceros ni negativos).";
        }

        // Suma de las cantidades de las filas seleccionadas
        private int totalEtiquetas()
        {
            int total = 0;
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if (!Convert.ToBoolean(fila.Cells["sel"].Value)) continue;
                int c;
                if (cantidadValida(Convert.ToString(fila.Cells["Cantidad"].Value), out c)) total += c;
            }
            return total;
        }

        private void actualizarTotal()
        {
            if (_actualizandoCant) return;
            int total = totalEtiquetas();

            string texto = "Total: " + total.ToString("N0") + (total == 1 ? " etiqueta" : " etiquetas");
            bool error = false;
            if (_globalInvalida)
            {
                texto = "Cantidad inválida (1 a " + CANT_MAX + ")";
                error = true;
            }
            else if (total > TOTAL_MAX)
            {
                texto += " (máx. " + TOTAL_MAX.ToString("N0") + ")";
                error = true;
            }
            lblTotalEtiquetas.Text = texto;
            lblTotalEtiquetas.ForeColor = error ? Color.Firebrick : SystemColors.ControlText;
        }

        // La cantidad "para todos" reemplaza las cantidades individuales de todos los productos de la grilla.
        private void txtCantGlobal_TextChanged(object sender, EventArgs e)
        {
            string t = txtCantGlobal.Text.Trim();
            if (t.Length == 0)
            {
                _globalInvalida = false;
                txtCantGlobal.BackColor = SystemColors.Window;
                actualizarTotal();
                return;
            }

            int v;
            if (!cantidadValida(t, out v))
            {
                _globalInvalida = true;
                txtCantGlobal.BackColor = Color.MistyRose;
                actualizarTotal();
                return;
            }

            _globalInvalida = false;
            txtCantGlobal.BackColor = SystemColors.Window;
            _actualizandoCant = true;
            try
            {
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                    fila.Cells["Cantidad"].Value = v;
            }
            finally
            {
                _actualizandoCant = false;
            }
            dgvProductos.Refresh();
            actualizarTotal();
        }

        private void dgvProductos_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvProductos.Columns[e.ColumnIndex].Name != "Cantidad") return;

            int v;
            if (!cantidadValida(Convert.ToString(e.FormattedValue), out v))
            {
                e.Cancel = true;   // Esc devuelve el valor anterior
                MessageBox.Show(this, mensajeCantidadInvalida(), "IMPRESION DE ETIQUETAS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvProductos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgvProductos.Columns[e.ColumnIndex].Name;
            if (col == "Sel" || col == "sel" || col == "Cantidad")
                actualizarTotal();
        }

        private void dgvProductos_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Los checkbox se confirman al instante para que el total se actualice
            if (dgvProductos.IsCurrentCellDirty && dgvProductos.CurrentCell is DataGridViewCheckBoxCell)
                dgvProductos.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        // Valida la selección antes de imprimir; sin selección no hace nada (rige el aviso existente).
        private bool seleccionValida(string titulo)
        {
            if (_globalInvalida)
            {
                MessageBox.Show(this, mensajeCantidadInvalida(), titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int seleccionados = 0;
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if (!Convert.ToBoolean(fila.Cells["sel"].Value)) continue;
                seleccionados++;
                int c;
                if (!cantidadValida(Convert.ToString(fila.Cells["Cantidad"].Value), out c))
                {
                    MessageBox.Show(this, "Hay productos con cantidad inválida. " + mensajeCantidadInvalida(), titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            int total = totalEtiquetas();
            if (seleccionados > 0 && total > TOTAL_MAX)
            {
                MessageBox.Show(this, "El total (" + total.ToString("N0") + ") supera el máximo de " + TOTAL_MAX.ToString("N0") + " etiquetas por impresión.", titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private List<ProductoEtiqueta> ObtenerProductosSeleccionados()
        {
            List<ProductoEtiqueta> lista = new List<ProductoEtiqueta>();

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                bool seleccionado = Convert.ToBoolean(fila.Cells["sel"].Value);

                if (seleccionado)
                {
                    ProductoEtiqueta p = new ProductoEtiqueta
                    {
                        Descripcion = fila.Cells["descripcion"].Value?.ToString(),
                        Precio = Convert.ToDecimal(fila.Cells["precio"].Value),
                        CodigoBarras = fila.Cells["codBarras"].Value?.ToString()
                    };

                    // Una entrada por etiqueta a imprimir: cada producto se repite "Cantidad" veces
                    int cantidad;
                    if (!cantidadValida(Convert.ToString(fila.Cells["Cantidad"].Value), out cantidad)) cantidad = 1;
                    for (int k = 0; k < cantidad; k++)
                        lista.Add(p);
                }
            }

            return lista;
        }

        private async void btnImprimirEtiqueta_Click(object sender, EventArgs e)
        {
            Clases.ClassReportesITextSharp instItextSharp = new Clases.ClassReportesITextSharp();

            if (!seleccionValida("IMPRESION DE ETIQUETAS")) return;
            var productos = ObtenerProductosSeleccionados();

            if (productos.Count == 0)
            {
                MessageBox.Show(this, "Debe seleccionar al menos un producto.",
                    "IMPRESION DE ETIQUETAS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string carpetaDescargas = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            string pathArchivo = Path.Combine(
                carpetaDescargas,
                "Etiquetas_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

            progressBar1.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Maximum = productos.Count;

            await Task.Run(() =>
                instItextSharp.GenerarEtiquetasPDF(productos, pathArchivo,
                    (valor) =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            progressBar1.Value = valor;
                        }));
                    })
            );

            progressBar1.Visible = false;

            MessageBox.Show(this,"PDF generado correctamente", "IMPRESION DE ETIQUETAS",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private async void btnImprimirCodBarras_Click(object sender, EventArgs e)
        {
            Clases.ClassReportesITextSharp instItextSharp = new Clases.ClassReportesITextSharp();

            if (!seleccionValida("IMPRESION DE CODIGOS DE BARRAS")) return;
            var productos = ObtenerProductosSeleccionados();

            if (productos.Count == 0)
            {
                MessageBox.Show(this, "Debe seleccionar al menos un producto.",
                    "IMPRESION DE CODIGOS DE BARRAS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            string carpetaDescargas = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            string pathArchivo = Path.Combine(
                carpetaDescargas,
                "CODBARRAS_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

            progressBar1.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Maximum = productos.Count;

            await Task.Run(() =>
                instItextSharp.GenerarCodBarrasPDF(productos, pathArchivo,
                    (valor) =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            progressBar1.Value = valor;
                        }));
                    })
            );

            progressBar1.Visible = false;

            MessageBox.Show(this, "COD. BARRAS generado correctamente", "IMPRESION CODIGOS DE BARRAS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
