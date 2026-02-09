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
    public partial class frmABMDocumentosTipos : Form
    {
        int accion = 0;
        int tipoDocId = 0;
        public frmABMDocumentosTipos()
        {
            InitializeComponent();
        }

        private void frmABMDocumentosTipos_KeyDown(object sender, KeyEventArgs e)
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

        private void frmABMDocumentosTipos_Load(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void estadoInicial()
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            dgvTiposDocumentos.DataSource = instConfig.traerTiposDocumentos();
            txtNombre.Text = string.Empty;

            if (dgvTiposDocumentos.Width == 355)
            {
                dgvTiposDocumentos.Width += gbDatos.Width + 5;
            }
            dgvTiposDocumentos.BringToFront();
            verificarBotones();
            dgvTiposDocumentos.Enabled = true;
            dgvTiposDocumentos.Focus();
        }

        private void verificarBotones()
        {
            btnAgregar.Enabled = true;
            if (dgvTiposDocumentos.RowCount > 0)
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            estadoAM();
            accion = 1;
            txtNombre.Text = string.Empty;
            txtAbrev.Text = string.Empty;
            txtNombre.Focus();
        }
        private void estadoAM()
        {
            dgvTiposDocumentos.Enabled = false;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            dgvTiposDocumentos.Width = 355;

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                if (accion == 1)
                {
                    instConfig.ABMTiposDocumentos(0, txtNombre.Text.Trim(), txtAbrev.Text.Trim(), 1);
                }
                if (accion == 2)
                {
                    instConfig.ABMTiposDocumentos(tipoDocId, txtNombre.Text.Trim(), txtAbrev.Text.Trim(), 2);
                }
                estadoInicial();
            }
        }

        private bool formularioValido()
        {
            if (txtNombre.Text == string.Empty)
            {
                errorProvider1.SetError(txtNombre, "Debe indicar el nombre");
                return false;
            }

            if (txtAbrev.Text == string.Empty)
            {
                errorProvider1.SetError(txtAbrev, "Debe indicar una Abreviatura de 2 letras");
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
            txtNombre.Text = dgvTiposDocumentos.CurrentRow.Cells["Nombre"].Value.ToString();
            txtAbrev.Text = dgvTiposDocumentos.CurrentRow.Cells["Abrev"].Value.ToString();

            tipoDocId = int.Parse(dgvTiposDocumentos.CurrentRow.Cells["Nro"].Value.ToString());
            txtNombre.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            instConfig.ABMTiposDocumentos(int.Parse(dgvTiposDocumentos.CurrentRow.Cells["Nro"].Value.ToString()),"", "", 3);
            estadoInicial();
        }
    }
}
