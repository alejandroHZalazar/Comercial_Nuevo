using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Comercial.Formularios.Estadisticas
{
    public partial class frmRankingVentas : Form
    {
        int cantDec = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();
        string nombreCSV;

        public frmRankingVentas()
        {
            InitializeComponent();
        }

        private void btnRankingProductos_Click(object sender, EventArgs e)
        {
            dgvRanking.DataSource = null;
            dgvRanking.Rows.Clear();
            Clases.ClassEstadisticas instEstadist = new Clases.ClassEstadisticas();
            dgvRanking.DataSource = instEstadist.traerVentasRankingProductos(dtpDesde.Value,dtpHasta.Value,cbProveedor.Checked?(int?)cboProveedor.SelectedValue:null);
            nombreCSV = "Ranging Productos";
        }

        private void btnRankigCliente_Click(object sender, EventArgs e)
        {
            dgvRanking.DataSource = null;
            dgvRanking.Rows.Clear();
            Clases.ClassEstadisticas instEstadist = new Clases.ClassEstadisticas();
            dgvRanking.DataSource = instEstadist.traerVentasRankingClientes(dtpDesde.Value, dtpHasta.Value, cbProveedor.Checked ? (int?)cboProveedor.SelectedValue : null);
            redondearGrilla();
            nombreCSV = "Ranging Clientes";
        }

        private void redondearGrilla()
        {
            if (dgvRanking.RowCount > 0)
            {
               // dgvRanking.Columns["Ventas"].DefaultCellStyle.Format = "N" + cantDec.ToString();
                
            }
        }

        private void frmRankingVentas_Load(object sender, EventArgs e)
        {
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();
            cboProveedor.DataSource = instProv.traeProveedores();
            cboProveedor.ValueMember = "id";
            cboProveedor.DisplayMember = "nombreComercial";
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvRanking.RowCount == 0) return;

            Clases.ClassUtil instUtil = new Clases.ClassUtil();
            instUtil.ExportarDataGridViewACsv(dgvRanking, nombreCSV);
        }
    }
}
