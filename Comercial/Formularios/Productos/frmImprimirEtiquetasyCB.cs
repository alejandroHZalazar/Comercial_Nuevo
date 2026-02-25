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
            cboRubros.DataSource = instConfig.traeRubrosconTdos();
            cboRubros.ValueMember = "id";
            cboRubros.DisplayMember = "descripcion";
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

            cbTodos.Checked = false;
            cbNinguno.Checked = false;
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
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                fila.Cells["sel"].Value = estado;
            }

            dgvProductos.RefreshEdit();
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

                    lista.Add(p);
                }
            }

            return lista;
        }

        private async void btnImprimirEtiqueta_Click(object sender, EventArgs e)
        {
            Clases.ClassReportesITextSharp instItextSharp = new Clases.ClassReportesITextSharp();

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
