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
    public partial class frmABMTipoGastos : Form
    {
        private int accion = 0;
        private int unTipoGasto = 0;
        public frmABMTipoGastos()
        {
            InitializeComponent();
        }

        private void frmABMTipoGastos_Load(object sender, EventArgs e)
        {
            estadoInicial();
        }
        private void estadoInicial()
        {
            if (dgvTipoGastos.Width == 339)
            {
                dgvTipoGastos.Width += gbDatos.Width + 5;
            }
            dgvTipoGastos.BringToFront();
            cargarGrilla();
            verificarBotones();
            dgvTipoGastos.Enabled = true;
            dgvTipoGastos.Focus();
        }
        private void cargarGrilla()
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            dgvTipoGastos.DataSource = instConfig.traerTipoGastos();
        }

        private void verificarBotones()
        {
            btnAgregar.Enabled = true;
            btnEditar.Enabled = btnEliminar.Enabled = (dgvTipoGastos.RowCount > 0);

        }

        private void frmABMTipoGastos_KeyDown(object sender, KeyEventArgs e)
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
            dgvTipoGastos.Enabled = false;
            txtNombre.Text = string.Empty;
            txtNombre.Focus();
        }

        private void estadoAM()
        {
            dgvTipoGastos.Enabled = false;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            dgvTipoGastos.Width = 339;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                string salida;
                Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                if (accion == 1)
                {
                    salida = instConfig.ABMTiposGastos(txtNombre.Text.Trim(), 1, 0);
                }
                else
                {
                    salida = instConfig.ABMTiposGastos(txtNombre.Text.Trim(), 2, unTipoGasto);
                }

                if (salida == "1")
                {
                    estadoInicial();

                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Gestión Tipos de Gastos", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            
            estadoAM();
            accion = 2;            

            txtNombre.Text = dgvTipoGastos.CurrentRow.Cells["Nombre"].Value.ToString();
            unTipoGasto = int.Parse(dgvTipoGastos.CurrentRow.Cells["Nro"].Value.ToString());
            txtNombre.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            string salida = instConfig.ABMTiposGastos("", 3, int.Parse(dgvTipoGastos.CurrentRow.Cells["Nro"].Value.ToString()));
            if (salida == "1")
            {
                estadoInicial();
            }
            else
            {
                MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Gestión Tipos de Gastos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
