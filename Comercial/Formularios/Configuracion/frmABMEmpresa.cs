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
    public partial class frmABMEmpresa : Form
    {
        public frmABMEmpresa()
        {
            InitializeComponent();
        }

        private void frmABMEmpresa_Load(object sender, EventArgs e)
        {
            Clases.ClassParametros instParam = new Clases.ClassParametros();
            dgvParametros.DataSource = instParam.traerParametro();
            dgvParametros.Columns["id"].Visible = false;
            dgvParametros.Columns["Modulo"].ReadOnly = true;
            dgvParametros.Columns["Parametro"].ReadOnly = true;
        }

        private void dgvParametros_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvParametros.Columns[e.ColumnIndex].Name != "Valor")
                return;
            DataGridViewRow row = dgvParametros.Rows[e.RowIndex];

            if (row.Cells["id"].Value == null)
                return;

            int id = Convert.ToInt32(row.Cells["id"].Value);
            string nuevoValor = row.Cells["Valor"].Value?.ToString();

            Clases.ClassParametros instParam = new Clases.ClassParametros();
            instParam.ActualizarParametro(id, nuevoValor);
        }
    }
}
