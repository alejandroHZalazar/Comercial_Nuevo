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
    public partial class frmGestionCaja : Form
    {
        public frmGestionCaja()
        {
            InitializeComponent();
        }

        private void frmGestionCaja_Load(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void estadoInicial()
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

            bool cajaAbierta = cajaEstado.Rows.Count == 0 ? false : (cajaEstado.Rows[0]["estado"].ToString() == "ABIERTA" ? true : false);

            btnAbrirCaja.Enabled = !cajaAbierta;
            btnCerrarCaja.Enabled = btnIngresoDinero.Enabled = btnEgresoDinero.Enabled = btnGastos.Enabled = btnArqueo.Enabled = cajaAbierta;
            lblEstado.Text = cajaEstado.Rows.Count == 0 ? "Cerrada" : cajaEstado.Rows[0]["estado"].ToString();
            lblUsuario.Text = Environment.GetEnvironmentVariable("nombreUser");
            lblEfectivo.Text = lblTotalDebe.Text = lblTotalHaber.Text = "$ 0,00";
            dgvMovCaja.DataSource = null;
            if (cajaAbierta) mostrarResumen();
        }

        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));
            frmAperturaCaja unFrmApertura = new frmAperturaCaja(cajaEstado.Rows.Count == 0?0: decimal.Parse(cajaEstado.Rows[0]["saldo_cierre"].ToString()), cajaEstado.Rows.Count == 0 ? DateTime.Now.ToString() : cajaEstado.Rows[0]["fecha_cierre"].ToString());
            unFrmApertura.ShowDialog();
            estadoInicial();
        }

        private void mostrarResumen()
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            dgvMovCaja.DataSource = instCaja.traerResumenCaja(0, int.Parse(Environment.GetEnvironmentVariable("idUser")));
            dgvMovCaja.ColumnHeadersDefaultCellStyle.Font = new Font(dgvMovCaja.Font, FontStyle.Bold);
            dgvMovCaja.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMovCaja.Columns["Importe Debe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvMovCaja.Columns["Importe Haber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvMovCaja.Columns["orden"].Visible = false;
            obtenerTotales();
        }

        private void obtenerTotales()
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            decimal totalDebe = 0;
            decimal totalHaber = 0;

            foreach (DataGridViewRow fila in dgvMovCaja.Rows)
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
            lblEfectivo.Text = instCaja.traerSaldoCaja(0, int.Parse(Environment.GetEnvironmentVariable("idUser"))).ToString("C");
            dgvMovCaja.Columns["Importe Debe"].DefaultCellStyle.Format = "C";
            dgvMovCaja.Columns["Importe Haber"].DefaultCellStyle.Format = "C";
        }

        private void btnIngresoDinero_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            var ingresoId = Clases.ClassParametros.buscarParametro("caja", "IngresoDinero");
            var medioEfectivoId = Clases.ClassParametros.buscarParametro("caja", "MedioPagoEfectivo");
            DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

            if (cajaEstado.Rows.Count == 0) return;
            var cajaId = int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());            
            
            if (!string.IsNullOrWhiteSpace(ingresoId) && !string.IsNullOrWhiteSpace(medioEfectivoId))
            {
                Formularios.Contable.frmIngresoDinero unFrmIngresoDinero = new Formularios.Contable.frmIngresoDinero(cajaId, int.Parse(ingresoId), int.Parse(medioEfectivoId));
                unFrmIngresoDinero.ShowDialog();
                estadoInicial();
            }
            else
            {
                MessageBox.Show(this, "Debe habilitar el modulo de Caja o parametrizar su valor y parametrizar el valor del medio de Pago efectivo y el Concepto de Pago de Ingresos de dinero", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEgresoDinero_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            var egresoId = Clases.ClassParametros.buscarParametro("caja", "EgresoDinero");
            var medioEfectivoId = Clases.ClassParametros.buscarParametro("caja", "MedioPagoEfectivo");
            DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

            if (cajaEstado.Rows.Count == 0) return;
            var cajaId = int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());

            if (!string.IsNullOrWhiteSpace(egresoId) && !string.IsNullOrWhiteSpace(medioEfectivoId))
            {
                Formularios.Contable.frmEgresoDinero unFrmEgresoDinero = new Formularios.Contable.frmEgresoDinero(cajaId, int.Parse(egresoId), int.Parse(medioEfectivoId));
                unFrmEgresoDinero.ShowDialog();
                estadoInicial();
            }
            else
            {
                MessageBox.Show(this, "Debe habilitar el modulo de Caja o parametrizar su valor y parametrizar el valor del medio de Pago efectivo y el Concepto de Pago de Egreso de dinero", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGastos_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            var gastoId = Clases.ClassParametros.buscarParametro("caja", "Gastos");
            var medioEfectivoId = Clases.ClassParametros.buscarParametro("caja", "MedioPagoEfectivo");
            DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

            if (cajaEstado.Rows.Count == 0) return;
            var cajaId = int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());

            if (!string.IsNullOrWhiteSpace(gastoId) && !string.IsNullOrWhiteSpace(medioEfectivoId))
            {
                Formularios.Contable.frmIngresoGastos unFrmIngresoGastos = new Formularios.Contable.frmIngresoGastos(cajaId, int.Parse(gastoId), int.Parse(medioEfectivoId));
                unFrmIngresoGastos.ShowDialog();
                estadoInicial();
            }
            else
            {
                MessageBox.Show(this, "Debe habilitar el modulo de Caja o parametrizar su valor y parametrizar el valor del medio de Pago efectivo y el Concepto de Gastos", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnArqueo_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            var arqueoIngresoId = Clases.ClassParametros.buscarParametro("caja", "ArqueoIngreso");
            var arqueoEgresoId = Clases.ClassParametros.buscarParametro("caja", "ArqueoEgreso");
            var medioEfectivoId = Clases.ClassParametros.buscarParametro("caja", "MedioPagoEfectivo");
            var cajaActual = instCaja.traerSaldoCaja(0, int.Parse(Environment.GetEnvironmentVariable("idUser")));
            DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

            if (cajaEstado.Rows.Count == 0)
            {
                MessageBox.Show(this, "No existe caja abierta", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cajaId = int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());

            if (string.IsNullOrEmpty(arqueoIngresoId))
            {
                MessageBox.Show(this, "No existe parametro para ingreso por arqueo", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(arqueoEgresoId))
            {
                MessageBox.Show(this, "No existe parametro para egreso por arqueo", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(medioEfectivoId))
            {
                MessageBox.Show(this, "No existe parametro para medio de pago dinero efectivo", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Formularios.Contable.frmArqueoCaja unFrmArqueo = new frmArqueoCaja(cajaId, int.Parse(arqueoIngresoId), int.Parse(arqueoEgresoId), int.Parse(medioEfectivoId), cajaActual);
            unFrmArqueo.ShowDialog();
            estadoInicial();
            
        }

        private void btnCerrarCaja_Click(object sender, EventArgs e)
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));
            if (cajaEstado.Rows.Count == 0) return;
            var cajaActual = instCaja.traerSaldoCaja(0, int.Parse(Environment.GetEnvironmentVariable("idUser")));
            frmCierreCaja unFrmCierre = new frmCierreCaja(cajaActual,cajaEstado.Rows[0]["fecha_apertura"].ToString(),int.Parse(cajaEstado.Rows[0]["caja_id"].ToString()));
            unFrmCierre.ShowDialog();
            estadoInicial();
        }
    }
}
