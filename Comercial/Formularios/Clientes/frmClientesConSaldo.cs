using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Clientes
{
    public partial class frmClientesConSaldo : Form
    {
        bool cargado = false;
        public frmClientesConSaldo()
        {
            InitializeComponent();
        }

        private void frmClientesConSaldo_Load(object sender, EventArgs e)
        {
            cargarCombos();
        }

        private void cargarCombos()
        {
            Clases.ClassLocalidades instLoc = new Clases.ClassLocalidades();
            cboProvincia.DataSource = instLoc.traeProvincias();
            cboProvincia.DisplayMember = "nombre";
            cboProvincia.ValueMember = "id";            

            Clases.ClassClientes instClie = new Clases.ClassClientes();
            cboZona.DataSource = instClie.traerZonas();
            cboZona.DisplayMember = "nombre";
            cboZona.ValueMember = "id";

            Clases.classUsuarios instUser = new Clases.classUsuarios();
            cboVendedor.DataSource = instUser.traerVendedores();
            cboVendedor.DisplayMember = "nombre";
            cboVendedor.ValueMember = "id";

            cargado = true;

            cboProvincia_SelectedIndexChanged(null, null);

        }

        private void cboProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargado) return;
            Clases.ClassLocalidades instLoc = new Clases.ClassLocalidades();
            cboLocalidad.DataSource = instLoc.traeLocalidades((int)cboProvincia.SelectedValue);
            cboLocalidad.DisplayMember = "nombre";
            cboLocalidad.ValueMember = "id";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Clases.ClassClientes instCLie = new Clases.ClassClientes();
            dgvSaldos.DataSource = instCLie.traerConSaldo(cbProvincia.Checked ? (int?)cboProvincia.SelectedValue : null, cbLocalidad.Checked ? (int?)cboLocalidad.SelectedValue : null, cbZona.Checked ? (int?)cboZona.SelectedValue : null, cbVendedor.Checked ? (int?)cboVendedor.SelectedValue : null);

            txtSaldo.Text = "0,00";

            if (dgvSaldos.RowCount == 0) return;

            decimal suma = 0;

            foreach (DataGridViewRow row in dgvSaldos.Rows)
            {
                if (row.Cells["Saldo"].Value != null &&
                    decimal.TryParse(row.Cells["Saldo"].Value.ToString(), out decimal valor))
                {
                    suma += valor;
                }
            }
            var culture = new CultureInfo("es-AR");
            txtSaldo.Text = suma.ToString("N2", culture);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvSaldos.RowCount == 0) return;

            Clases.ClassUtil instUtil = new Clases.ClassUtil();
            instUtil.ExportarDataGridViewACsv(dgvSaldos, "Saldos_Clientes");
        }
    }
}
