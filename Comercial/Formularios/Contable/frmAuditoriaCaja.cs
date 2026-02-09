using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Contable
{
    public partial class frmAuditoriaCaja : Form
    {
        DataTable grillaDT;
        DataTable detalleDT;
        decimal totalDebeG;
        decimal totalHaberG;
        public frmAuditoriaCaja()
        {
            InitializeComponent();
        }

        private void frmAuditoriaCaja_Load(object sender, EventArgs e)
        {
            estadoInicialResumen();
        }

        private void estadoInicialResumen()
        {
            Clases.classUsuarios instUser = new Clases.classUsuarios();

            cboUsuario.DataSource = instUser.traerTodosUsuarios();
            cboUserDetalle.DataSource = instUser.traerUsuariosActivos();

            cboUsuario.ValueMember = "id";
            cboUsuario.DisplayMember = "nombre";
            cboUserDetalle.ValueMember = "id";
            cboUserDetalle.DisplayMember = "nombre";
            panelResumen.Visible = false;
            dgvDetalle.DataSource = null;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();

            dgvCajas.DataSource = instCaja.traerEncabezadoCajaPorUsuarioyFechas(int.Parse(cboUsuario.SelectedValue.ToString()), dtpDesdeResumen.Value, dtpHastaResumen.Value);

            panelResumen.Visible = dgvCajas.RowCount > 0;

            if (dgvCajas.RowCount > 0)
            {
                estadoConDatos();
            }
        }

        private void estadoConDatos()
        {
            dgvCajas.Columns["observaciones"].Visible = false;
        }

        private void dgvCajas_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            grillaDT = instCaja.traerResumenCaja(int.Parse(dgvCajas.CurrentRow.Cells["Nro"].Value.ToString()), 0);
            dgvMovimiento.DataSource = grillaDT;
            if (dgvMovimiento.RowCount == 0) return;
            dgvMovimiento.Columns["orden"].Visible = false;
            rtbObservaciones.Text = dgvCajas.CurrentRow.Cells["observaciones"].Value.ToString();
            obtenerTotales();
        }

        private void obtenerTotales()
        {            
            decimal totalDebe = 0;
            decimal totalHaber = 0;

            foreach (DataGridViewRow fila in dgvMovimiento.Rows)
            {
                if (fila.Cells["Importe Debe"].Value != null)
                {
                    decimal valor;
                    if (decimal.TryParse(fila.Cells["Importe Debe"].Value.ToString(), out valor))
                    {
                        totalDebe += valor;
                    }
                }

                if (fila.Cells["Importe Haber"].Value != null)
                {
                    decimal valor;
                    if (decimal.TryParse(fila.Cells["Importe Haber"].Value.ToString(), out valor))
                    {
                        totalHaber += valor;
                    }
                }
            }

            lblTotalDebe.Text = totalDebe.ToString("C");
            lblTotalHaber.Text = totalHaber.ToString("C");
            totalDebeG = totalDebe;
            totalHaberG = totalHaber;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvMovimiento.RowCount == 0) return;
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            instCaja.ExportarResumenCajaCsv(int.Parse(dgvCajas.CurrentRow.Cells["Nro"].Value.ToString()), dgvCajas.CurrentRow.Cells["Usuario"].Value.ToString(), DateTime.Parse(dgvCajas.CurrentRow.Cells["Apertura"].Value.ToString()),
                                            DateTime.Parse(dgvCajas.CurrentRow.Cells["Cierre"].Value.ToString()), grillaDT, totalDebeG, totalHaberG, dgvCajas.CurrentRow.Cells["observaciones"].Value.ToString());
        }

        private void btnBuscarDetalle_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();

            detalleDT = instCaja.traerDetalleMovimientoPorUsuarioyFechas(int.Parse(cboUserDetalle.SelectedValue.ToString()), dtpDesdeDetalle.Value, dtpHastaDetalle.Value);
            dgvDetalle.DataSource = detalleDT;

            if (dgvDetalle.RowCount > 0)
            {
                ObtenerTotalesDetalle();
            }
        }

        private void ObtenerTotalesDetalle()
        {
            if (dgvDetalle.RowCount == 0) return;

            decimal totalIngreso = 0;
            decimal totalEgreso = 0;

            foreach (DataGridViewRow fila in dgvDetalle.Rows)
            {
                if (fila.Cells["Tipo"].Value != null && fila.Cells["Tipo"].Value.ToString() == "Ingreso")
                {
                    decimal valor;
                    if (decimal.TryParse(fila.Cells["Importe"].Value.ToString(), out valor))
                    {
                        totalIngreso += valor;
                    }
                }

                if (fila.Cells["Tipo"].Value != null && fila.Cells["Tipo"].Value.ToString() == "Egreso")
                {
                    decimal valor;
                    if (decimal.TryParse(fila.Cells["Importe"].Value.ToString(), out valor))
                    {
                        totalEgreso += valor;
                    }
                }
            }

            lblIngresos.Text = totalIngreso.ToString("C");
            lblEgresos.Text = totalEgreso.ToString("C");            

        }

        private void btnDescargarDetalle_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            instCaja.ExportarDetalleCajaCsv(detalleDT);
        }
    }
}
