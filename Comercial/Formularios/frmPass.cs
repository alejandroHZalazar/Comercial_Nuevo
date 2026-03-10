using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios
{
    public partial class frmPass : Form
    {
        public frmPass()
        {
            InitializeComponent();
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ValidarPass();
            }
        }

        private void ValidarPass()
        {
            if (txtPass.Text.ToUpper() == "$$$RETAIL109" )
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
