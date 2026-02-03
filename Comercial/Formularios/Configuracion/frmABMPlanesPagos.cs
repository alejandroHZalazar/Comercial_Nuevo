using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Configuracion
{
    public partial class frmABMPlanesPagos : Form
    {
        private int _id;
        private string _nombre;
        private int accion = 0;
        private int planesId = 0;
        public frmABMPlanesPagos(int unIdMedio, string unNombreMedio)
        {
            _id = unIdMedio;
            _nombre = unNombreMedio;
            InitializeComponent();
        }

        private void frmABMPlanesPagos_Load(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void estadoInicial()
        {
            if (dgvPlanesPago.Width == 339)
            {
                dgvPlanesPago.Width += gbDatos.Width + 5;
            }
            lblMedioPago.Text = _nombre;
            dgvPlanesPago.BringToFront();
            cargarGrilla();
            verificarBotones();
            dgvPlanesPago.Enabled = true;
            dgvPlanesPago.Focus();

        }

        private void cargarGrilla()
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            dgvPlanesPago.DataSource = instConfig.traerPlanesPagoPorMedio(_id);
        }

        private void verificarBotones()
        {
            btnAgregar.Enabled = true;
            if (dgvPlanesPago.RowCount > 0)
            {
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
            }
            else
            {
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
            }

        }
        private void frmABMPlanesPagos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2 & btnAgregar.Enabled == true)
            {
                btnAgregar_Click(null, null);
            }

            if (e.KeyData == Keys.F3 & btnEditar.Enabled == true)
            {
                btnEditar_Click(null, null);
            }

            if (e.KeyData == Keys.F4 & btnEliminar.Enabled == true)
            {
                btnEliminar_Click(null, null);
            }

            if (e.KeyData == Keys.F5 & btnAgregar.Enabled == false)
            {
                btnGrabar_Click(null, null);
            }

            if (e.KeyData == Keys.F6 & btnAgregar.Enabled == false)
            {
                btnCancelar_Click(null, null);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            estadoAM();
            accion = 1;
            dgvPlanesPago.Enabled = false;
            txtNombre.Text = string.Empty;
            nudRecargo.Value = 0;
            txtNombre.Focus();
        }

        private void estadoAM()
        {
            dgvPlanesPago.Enabled = false;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            dgvPlanesPago.Width = 339;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                string salida;
                Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                if (accion == 1)
                {
                    salida = instConfig.ABMPlanesPago(_id,txtNombre.Text.Trim(), nudRecargo.Value, 1, 0);
                }
                else
                {
                    salida = instConfig.ABMPlanesPago(_id,txtNombre.Text.Trim(), nudRecargo.Value, 2, planesId);
                }

                if (salida == "1")
                {
                    estadoInicial();

                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Gestión Planes de Pago", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool formularioValido()
        {
            errorProvider1.Clear();

            if (txtNombre.Text == string.Empty)
            {
                errorProvider1.SetError(txtNombre, "Debe escribir un nombre");
                return false;
            }

            if (nudRecargo.Value < 0)
            {
                errorProvider1.SetError(nudRecargo, "El recargo debe ser mayor o igual a 0");
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            estadoAM();
            accion = 2;

            txtNombre.Text = dgvPlanesPago.CurrentRow.Cells["Nombre"].Value.ToString();
            nudRecargo.Value = decimal.Parse(dgvPlanesPago.CurrentRow.Cells["Recargo"].Value.ToString());

            planesId = int.Parse(dgvPlanesPago.CurrentRow.Cells["Nro"].Value.ToString());
            txtNombre.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            string salida = instConfig.ABMPlanesPago(0, "",0,3, int.Parse(dgvPlanesPago.CurrentRow.Cells["Nro"].Value.ToString()));
            if (salida == "1")
            {
                estadoInicial();
            }
            else
            {
                MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Gestión Planes de Pago", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
