using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Estadisticas
{
    public partial class frmExportarVentas : Form
    {
        DataTable ventasCSV;
        public frmExportarVentas()
        {
            InitializeComponent();
        }

        private void btnBuscarDetalle_Click(object sender, EventArgs e)
        {
            Clases.ClassVentas instVentas = new Clases.ClassVentas();
            ventasCSV = instVentas.traerVentaDetalleCsv(dtpDesdeDetalle.Value, dtpHastaDetalle.Value);

            if (ventasCSV.Rows.Count == 0) return;
            dgvDetalle.DataSource = ventasCSV;
        }

        private void btnDescargarDetalle_Click(object sender, EventArgs e)
        {
            Clases.ClassVentas instVentas = new Clases.ClassVentas();
            if (ventasCSV.Rows.Count == 0)
            {
                ventasCSV = instVentas.traerVentaDetalleCsv(dtpDesdeDetalle.Value, dtpHastaDetalle.Value);
            }
            if (ventasCSV.Rows.Count == 0) return;
            instVentas.ExportarVentasCsv(ventasCSV);
        }

        private void frmExportarVentas_Load(object sender, EventArgs e)
        {

        }
    }
}
