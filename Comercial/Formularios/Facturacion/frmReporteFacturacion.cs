using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Facturacion
{
    public partial class frmReporteFacturacion : Form
    {
        DataTable resumenDiario;
        public frmReporteFacturacion()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Clases.ClassReportesFiscal instRepFiscal = new Clases.ClassReportesFiscal();
            DataTable resumen = instRepFiscal.traerResumenFacturacion(dtpDesde.Value, dtpHasta.Value);
            if (resumen.Rows.Count == 0) return;
            dgvResumenGeneral.DataSource = resumen;

            resumenDiario = instRepFiscal.traerResumenFacturacionDiario(dtpDesde.Value, dtpHasta.Value);
            if (resumenDiario.Rows.Count == 0) return;
            dgvResumenDiario.DataSource = resumenDiario;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (resumenDiario.Rows.Count == 0) return;
            Clases.ClassReportesFiscal instRepFiscal = new Clases.ClassReportesFiscal();
            instRepFiscal.ExportarResumenDiario(resumenDiario);
        }

        private void btnBuscarDetalle_Click(object sender, EventArgs e)
        {
            Clases.ClassReportesFiscal instRepFiscal = new Clases.ClassReportesFiscal();
            DataTable detalle = instRepFiscal.traerDetalleFacturacion(dtpDesdeDetalle.Value, dtpHastaDetalle.Value);

            if (detalle.Rows.Count == 0) return;

            dgvDetalle.DataSource = detalle;
        }
    }
}
