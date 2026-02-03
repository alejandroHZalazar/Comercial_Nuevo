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
    public partial class frmABMConceptosCaja : Form
    {
        int accion = 0;
        int conceptoId = 0;
        public frmABMConceptosCaja()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void frmABMConceptosCaja_Load(object sender, EventArgs e)
        {
            estadoInicial();
           
        }

        private void estadoInicial()
        {
            if (dgvConceptos.Width == 401)
            {
                dgvConceptos.Width += gbDatos.Width + 5;
            }
            dgvConceptos.BringToFront();
            cargarGrilla();
            verificarBotones();
            dgvConceptos.Enabled = true;
            dgvConceptos.Focus();
            
        }

        private void verificarBotones()
        {
            btnAgregar.Enabled = true;
            if (dgvConceptos.RowCount > 0)
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

        private void cargarGrilla()
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            dgvConceptos.DataSource = instConfig.traerConceptosCaja();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            estadoAM();
            accion = 1;
            dgvConceptos.Enabled = false;
            txtNombre.Text = string.Empty;
            cboTipoMovimiento.SelectedIndex = -1;
            cbAfectaEfectivo.Checked = false;
            txtNombre.Focus();
        }

        private void estadoAM()
        {
            dgvConceptos.Enabled = false;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            dgvConceptos.Width = 401;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                string salida;
                Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                if (accion == 1)
                {
                    salida = instConfig.ABMConceptosCaja(txtNombre.Text.Trim(), cboTipoMovimiento.SelectedItem.ToString(),cbAfectaEfectivo.Checked,1,0);
                }
                else
                {
                    salida = instConfig.ABMConceptosCaja(txtNombre.Text.Trim(), cboTipoMovimiento.SelectedItem.ToString(), cbAfectaEfectivo.Checked, 2, conceptoId);
                }

                if (salida == "1")
                {
                    estadoInicial();                    

                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Gestión Conceptos Caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (cboTipoMovimiento.SelectedIndex == -1)
            {
                errorProvider1.SetError(cboTipoMovimiento, "Debe seleccionar una Tipo de Movimiento");
                return false;
            }

            return true;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            estadoAM();
            accion = 2;
            DataTable concepto = instConfig.traerConceptosCajaPorId(int.Parse(dgvConceptos.CurrentRow.Cells["id"].Value.ToString()));

            txtNombre.Text = concepto.Rows[0]["Nombre"].ToString();
            cboTipoMovimiento.SelectedItem = concepto.Rows[0]["Tipo"].ToString();
            cbAfectaEfectivo.Checked = bool.Parse(concepto.Rows[0]["afecta_efectivo"].ToString());

            conceptoId = int.Parse(concepto.Rows[0]["id"].ToString());
            txtNombre.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            string salida = instConfig.ABMConceptosCaja("","",false,3, int.Parse(dgvConceptos.CurrentRow.Cells["id"].Value.ToString()));
            if (salida == "1")
            {
                estadoInicial();
            }
            else
            {
                MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Gestión Conceptos Caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmABMConceptosCaja_KeyDown(object sender, KeyEventArgs e)
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

            if (e.KeyData == Keys.F7)
            {
                btnAgregarTiposGastos_Click(null, null);
            }
        }

        private void btnAgregarTiposGastos_Click(object sender, EventArgs e)
        {
            frmABMTipoGastos unFrmABMTipoGastos = new frmABMTipoGastos();
            unFrmABMTipoGastos.ShowDialog();
            estadoInicial();
        }
    }
}
