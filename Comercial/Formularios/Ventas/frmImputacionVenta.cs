using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Ventas
{
    public partial class frmImputacionVenta : Form
    {
        private decimal _total;
        private int _plan;
        public BindingList<Clases.ClassVentas.CobroFormasPago> unDT = new BindingList<Clases.ClassVentas.CobroFormasPago>();
        public frmImputacionVenta(decimal total, int plan)
        {
            _total = total;
            _plan = plan;
            InitializeComponent();
        }

        private void frmImputacionVenta_Load(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void estadoInicial()
        {
            lblTotal.Text = _total.ToString("");
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            var planDB = instConfig.traerPlanesPagoPorId(_plan);
            if (planDB.Rows.Count == 0) return;
            var medioDB = instConfig.traerMediosDePagoPorId(int.Parse(planDB.Rows[0]["fk_medioPago"].ToString()));
            if (medioDB.Rows.Count == 0) return;
            dgvFormasPago.DataSource = unDT;
            unDT.Add(new Clases.ClassVentas.CobroFormasPago
            {
                idMedio = int.Parse(medioDB.Rows[0]["id"].ToString()),
                Medio = medioDB.Rows[0]["Nombre"].ToString(),
                idPlan = int.Parse(planDB.Rows[0]["id"].ToString()),
                Plan = planDB.Rows[0]["nombre"].ToString(),
                Importe = _total,
                Referencia1 = "",
                Referencia2 = "",
                Referencia3 = "",
                necesitaDatos = bool.Parse(medioDB.Rows[0]["conDatos"].ToString())
            });
            calcularTotal();
        }

        private void calcularTotal()
        {
            decimal total = unDT.Sum(x => x.Importe);
            nudImputar.Value = total <= 0 ? 0 : total;
            btnCobrar.Enabled = total >= 0;
            dgvFormasPago.Columns["idMedio"].Visible = false;
            dgvFormasPago.Columns["idPlan"].Visible = false;
            dgvFormasPago.Columns["necesitaDatos"].Visible = false;
        }
        private void btnCobrar_Click(object sender, EventArgs e)
        {

            if (!validarFormulario()) return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool validarFormulario()
        {
            errorProvider1.Clear();
            foreach (Clases.ClassVentas.CobroFormasPago item in unDT)
            {
                if (item.necesitaDatos && item.Referencia1 == string.Empty && item.Referencia2 == string.Empty && item.Referencia3 == string.Empty)
                {
                    errorProvider1.SetError(dgvFormasPago, "Faltan datos de cobros en algunas de las formas de pagos seleccionadas");
                    return false;
                }

                if (nudImputar.Value <= 0)
                {
                    errorProvider1.SetError(nudImputar, "El importe a imputar debe ser mayor a 0");
                    return false;
                }

            }

            return true;
        }

        private void btnAgregarFormasPago_Click(object sender, EventArgs e)
        {
            if (nudImputar.Value < _total)
            {

                using (frmImputacionFormasPagos unFrmImputFP = new frmImputacionFormasPagos(_total - nudImputar.Value, 0, "", "", "", false))
                {
                    if (unFrmImputFP.ShowDialog() == DialogResult.OK)
                    {
                        if (!unFrmImputFP._edicion)
                        {
                            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                            var planPagoDB = instConfig.traerPlanesPagoPorId(unFrmImputFP._planPago ?? 0);
                            if (planPagoDB == null) return;
                            var medioPagoDB = instConfig.traerMediosDePagoPorId(int.Parse(planPagoDB.Rows[0]["fk_medioPago"].ToString()));
                            if (medioPagoDB == null) return;
                            unDT.Add(new Clases.ClassVentas.CobroFormasPago
                            {
                                idMedio = int.Parse(medioPagoDB.Rows[0]["id"].ToString()),
                                Medio = medioPagoDB.Rows[0]["Nombre"].ToString(),
                                idPlan = int.Parse(planPagoDB.Rows[0]["id"].ToString()),
                                Plan = planPagoDB.Rows[0]["nombre"].ToString(),
                                Importe = unFrmImputFP._total,
                                Referencia1 = unFrmImputFP._dato1,
                                Referencia2 = unFrmImputFP._dato2,
                                Referencia3 = unFrmImputFP._dato3,
                                necesitaDatos = bool.Parse(medioPagoDB.Rows[0]["conDatos"].ToString())
                            });
                            calcularTotal();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "Ya no tiene saldo a imputar", "COBROS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void frmImputacionVenta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
            {
                btnAgregarFormasPago_Click(null, null);
            }
            if (e.KeyData == Keys.F3 && btnCobrar.Enabled == true)
            {
                btnCobrar_Click(null, null);
            }
        }

        private void dgvFormasPago_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow fila = dgvFormasPago.Rows[e.RowIndex];
            Clases.ClassVentas.CobroFormasPago pagoSeleccionado = (Clases.ClassVentas.CobroFormasPago)fila.DataBoundItem;
            using (frmImputacionFormasPagos unFrmImputFP = new frmImputacionFormasPagos(pagoSeleccionado.Importe, pagoSeleccionado.idPlan, pagoSeleccionado.Referencia1, pagoSeleccionado.Referencia2, pagoSeleccionado.Referencia3, true))
            {
                if (unFrmImputFP.ShowDialog() == DialogResult.OK)
                {
                    if (unFrmImputFP._edicion)
                    {
                        Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                        pagoSeleccionado.Importe = unFrmImputFP._total;
                        var planPagoDB = instConfig.traerPlanesPagoPorId(unFrmImputFP._planPago ?? 0);
                        if (planPagoDB == null) return;
                        var medioPagoDB = instConfig.traerMediosDePagoPorId(int.Parse(planPagoDB.Rows[0]["fk_medioPago"].ToString()));
                        if (medioPagoDB == null) return;
                        pagoSeleccionado.idMedio = int.Parse(medioPagoDB.Rows[0]["id"].ToString());
                        pagoSeleccionado.Medio = medioPagoDB.Rows[0]["Nombre"].ToString();
                        pagoSeleccionado.idPlan = int.Parse(planPagoDB.Rows[0]["id"].ToString());
                        pagoSeleccionado.Plan = planPagoDB.Rows[0]["nombre"].ToString();
                        pagoSeleccionado.Referencia1 = unFrmImputFP._dato1;
                        pagoSeleccionado.Referencia2 = unFrmImputFP._dato2;
                        pagoSeleccionado.Referencia3 = unFrmImputFP._dato3;
                        dgvFormasPago.Refresh();
                        calcularTotal();
                    }
                }
            }
        }

        private void dgvFormasPago_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            calcularTotal();
        }
    }   

}
