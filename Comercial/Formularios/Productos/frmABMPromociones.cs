using Comercial.Clases;
using System;
using System.Data;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    public partial class frmABMPromociones : Form
    {
        ClassPromociones instPromo = new ClassPromociones();

        public frmABMPromociones()
        {
            InitializeComponent();
        }

        private void frmABMPromociones_Load(object sender, EventArgs e)
        {
            cargarPromociones();
        }

        private void cargarPromociones()
        {
            DataTable dt = instPromo.obtenerPromociones("");
            dgvPromociones.DataSource = dt;

            if (dgvPromociones.Columns.Contains("id"))
                dgvPromociones.Columns["id"].Visible = false;
            if (dgvPromociones.Columns.Contains("fk_producto"))
                dgvPromociones.Columns["fk_producto"].Visible = false;

            if (dgvPromociones.Columns.Contains("producto"))
                dgvPromociones.Columns["producto"].HeaderText = "Producto";
            if (dgvPromociones.Columns.Contains("codBarras"))
                dgvPromociones.Columns["codBarras"].HeaderText = "Cód. Barras";
            if (dgvPromociones.Columns.Contains("precio"))
                dgvPromociones.Columns["precio"].HeaderText = "Precio";
            if (dgvPromociones.Columns.Contains("activa"))
                dgvPromociones.Columns["activa"].HeaderText = "Activa";
            if (dgvPromociones.Columns.Contains("fechaDesde"))
                dgvPromociones.Columns["fechaDesde"].HeaderText = "Desde";
            if (dgvPromociones.Columns.Contains("fechaHasta"))
                dgvPromociones.Columns["fechaHasta"].HeaderText = "Hasta";
        }

        private void btnNueva_Click(object sender, EventArgs e)
        {
            frmAltaModifPromocion frm = new frmAltaModifPromocion();
            frm.unAccion = 1;
            frm.ShowDialog();
            cargarPromociones();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvPromociones.CurrentRow == null) return;

            int fkProducto = int.Parse(dgvPromociones.CurrentRow.Cells["fk_producto"].Value.ToString());
            int idPromo = int.Parse(dgvPromociones.CurrentRow.Cells["id"].Value.ToString());

            frmAltaModifPromocion frm = new frmAltaModifPromocion();
            frm.unAccion = 2;
            frm.unIdPromocion = idPromo;
            frm.unFkProducto = fkProducto;
            frm.ShowDialog();
            cargarPromociones();
        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            if (dgvPromociones.CurrentRow == null) return;

            int idPromo = int.Parse(dgvPromociones.CurrentRow.Cells["id"].Value.ToString());
            int fkProducto = int.Parse(dgvPromociones.CurrentRow.Cells["fk_producto"].Value.ToString());
            bool activa = Convert.ToBoolean(dgvPromociones.CurrentRow.Cells["activa"].Value);

            int resultado = instPromo.ABMPromocion(2, idPromo, fkProducto, !activa, null, null);
            if (resultado == -1)
                MessageBox.Show("Error al cambiar estado de la promoción.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Error);

            cargarPromociones();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPromociones.CurrentRow == null) return;

            int idPromo = int.Parse(dgvPromociones.CurrentRow.Cells["id"].Value.ToString());
            string nombre = dgvPromociones.CurrentRow.Cells["producto"].Value.ToString();

            if (MessageBox.Show($"¿Desea eliminar la promoción '{nombre}'?", "Promociones",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int resultado = instPromo.ABMPromocion(3, idPromo, 0, false, null, null);
                if (resultado == -1)
                    MessageBox.Show("Error al eliminar la promoción.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cargarPromociones();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvPromociones_DoubleClick(object sender, EventArgs e)
        {
            btnEditar_Click(sender, e);
        }

        private void frmABMPromociones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F4)
            {
                btnNueva_Click(null, null);
            }

            if (e.KeyData == Keys.F5)
            {
                btnEditar_Click(null, null);
            }
            
        }
    }
}
