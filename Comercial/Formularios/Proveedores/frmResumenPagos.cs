using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Proveedores
{
    public partial class frmResumenPagos : Form
    {
        DataTable resumenPagos;
        public frmResumenPagos()
        {
            InitializeComponent();
        }

        private void frmResumenPagos_Load(object sender, EventArgs e)
        {

        }

        private void btnBuscarDetalle_Click(object sender, EventArgs e)
        {
            dgvPagos.DataSource = null;
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();
            resumenPagos = instProv.traerResumenPagos(dtpDesdeDetalle.Value, dtpHastaDetalle.Value);
            if (resumenPagos.Rows.Count == 0) return;
            dgvPagos.DataSource = resumenPagos;
            calcularTotal();
        }

        private void calcularTotal()
        {
            var total = 0m;
            foreach (DataGridViewRow fila in dgvPagos.Rows)
            {

                decimal valor;
                if (decimal.TryParse(fila.Cells["Importe"].Value.ToString(), out valor))
                {
                    total += valor;
                }
            }

            txtTotal.Text = total.ToString("C");
        }

        private void btnDescargarDetalle_Click(object sender, EventArgs e)
        {
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();
            if (resumenPagos == null || resumenPagos.Rows.Count == 0) resumenPagos = instProv.traerResumenPagos(dtpDesdeDetalle.Value, dtpHastaDetalle.Value);
            if (resumenPagos.Rows.Count == 0) return;
            instProv.ExportarPagosCsv(resumenPagos);
        }
    }
}

