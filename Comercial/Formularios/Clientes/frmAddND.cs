using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Clientes
{
    public partial class frmAddND : Form
    {
        int _cliente;        
        public frmAddND(int cliente)
        {
            _cliente = cliente;
            InitializeComponent();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (fomularioValido())
            {
                Clases.ClassClientes instClie = new Clases.ClassClientes();
                var salida = instClie.Add_ND_Cliente(_cliente, nudImputar.Value, rbtObserv.Text.Trim());
                if (salida != -1)
                {
                    MessageBox.Show(this, "Nota de Débito ingresada con éxito!!", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error al momento de generar la nota de débito", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private bool fomularioValido()
        {
            errorProvider1.Clear();

            if (nudImputar.Value <= 0)
            {
                errorProvider1.SetError(nudImputar, "Debe ingresar un valor mayor a 0");
                return false;
            }

            if (rbtObserv.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(rbtObserv, "Debe ingresar una observación");
                return false;
            }
            return true;
        }
        private void frmAddND_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
            {
                btnGrabar_Click(null, null);
            }
        }

        private void frmAddND_Load(object sender, EventArgs e)
        {
            nudImputar.Select(0, nudImputar.Text.Length);
        }
    }
}
