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
    public partial class frmABMMediosPago : Form
    {
        int accion = 0;
        int medioId = 0;
        public frmABMMediosPago()
        {
            InitializeComponent();
        }

        private void frmABMMediosPago_Load(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void estadoInicial()
        {
            if (dgvMediosPago.Width == 339)
            {
                dgvMediosPago.Width += gbDatos.Width + 5;
            }
            dgvMediosPago.BringToFront();
            cargarGrilla();
            verificarBotones();
            dgvMediosPago.Enabled = true;
            dgvMediosPago.Focus();

        }

        private void cargarGrilla()
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            dgvMediosPago.DataSource = instConfig.traerMediosDePago();
        }

        private void verificarBotones()
        {
            btnAgregar.Enabled = true;         
            btnEditar.Enabled = btnEliminar.Enabled = btnAgregarPlan.Enabled =(dgvMediosPago.RowCount > 0);      

        }

        private void frmABMMediosPago_KeyDown(object sender, KeyEventArgs e)
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

            if (e.KeyData == Keys.F7 && btnAgregarPlan.Enabled == true)
            {
                btnAgregarPlan_Click(null, null);
            }
        }

        private void btnAgregarPlan_Click(object sender, EventArgs e)
        {
            frmABMPlanesPagos unFrmPlan = new frmABMPlanesPagos(int.Parse(dgvMediosPago.CurrentRow.Cells["id"].Value.ToString()), dgvMediosPago.CurrentRow.Cells["Nombre"].Value.ToString());
            unFrmPlan.ShowDialog();
            estadoInicial();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            estadoAM();
            accion = 1;
            dgvMediosPago.Enabled = false;
            txtNombre.Text = string.Empty;            
            txtNombre.Focus();
        }

        private void estadoAM()
        {
            dgvMediosPago.Enabled = false;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnAgregarPlan.Enabled = false;
            dgvMediosPago.Width = 339;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                string salida;
                Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                if (accion == 1)
                {
                    salida = instConfig.ABMMediosPago(txtNombre.Text.Trim(), 1, 0);
                }
                else
                {
                    salida = instConfig.ABMMediosPago(txtNombre.Text.Trim(), 2, medioId);
                }

                if (salida == "1")
                {
                    estadoInicial();

                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Gestión Medios de Pago", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            return true;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            estadoAM();
            accion = 2;
            DataTable medioPago = instConfig.traerMediosDePagoPorId(int.Parse(dgvMediosPago.CurrentRow.Cells["id"].Value.ToString()));

            txtNombre.Text = medioPago.Rows[0]["Nombre"].ToString();            

            medioId = int.Parse(medioPago.Rows[0]["id"].ToString());
            txtNombre.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            string salida = instConfig.ABMMediosPago("", 3, int.Parse(dgvMediosPago.CurrentRow.Cells["id"].Value.ToString()));
            if (salida == "1")
            {
                estadoInicial();
            }
            else
            {
                MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Gestión Medios de Pago", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
