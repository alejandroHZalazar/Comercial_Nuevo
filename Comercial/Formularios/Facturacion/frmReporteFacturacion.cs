using System;
using System.Data;
using System.Diagnostics;
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

            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            link.DataPropertyName = "linkPDF";   // nombre del campo que viene de MySQL
            link.HeaderText = "Link";
            link.Name = "linkPDF";

            dgvDetalle.Columns.Remove("linkPDF");
            dgvDetalle.Columns.Add(link);
        }

        private void dgvDetalle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalle.Columns[e.ColumnIndex] is DataGridViewLinkColumn)
            {
                string url = dgvDetalle.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }
    }
}
