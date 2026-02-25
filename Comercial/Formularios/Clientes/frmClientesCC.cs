using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Clientes
{
    public partial class frmClientesCC : Form
    {
        int _cliente;
        public frmClientesCC(int cliente)
        {
            _cliente = cliente;
            InitializeComponent();
        }

        private void frmClientesCC_Load(object sender, EventArgs e)
        {
            cargarGrilla();
        }

        private void cargarGrilla()
        {
            Clases.ClassClientes instClie = new Clases.ClassClientes();
            dgvCC.DataSource = instClie.traerCC(_cliente);
            dgvCC.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCC.Columns["Debe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;            
            dgvCC.Columns["Debe"].DefaultCellStyle.Format = "C2";
            dgvCC.Columns["Haber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCC.Columns["Haber"].DefaultCellStyle.Format = "C2";
            dgvCC.Columns["Saldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCC.Columns["Saldo"].DefaultCellStyle.Format = "C2";
            txtSaldo.Text = calcularTotal().ToString("C");
        }

        private decimal calcularTotal()
        {
            decimal saldo = 0;

            foreach (DataGridViewRow fila in dgvCC.Rows)
            {
                if (decimal.Parse(fila.Cells["Saldo"].Value.ToString()) > 0)
                {
                    if (decimal.Parse(fila.Cells["Debe"].Value.ToString()) > 0 )
                    {
                        saldo += decimal.Parse(fila.Cells["Saldo"].Value.ToString());
                    }
                    else
                    {
                        saldo -= decimal.Parse(fila.Cells["Saldo"].Value.ToString());
                    }
                }
            }

            return saldo;
        }

        private void imprimirCobro (DataTable recibo)
        {
            Clases.ClassReportesITextSharp instReport = new Clases.ClassReportesITextSharp();
            var logo = Clases.ClassParametros.buscarParametro("login", "logo");
            var nombreEmpresa = Clases.ClassParametros.buscarParametro("empresa", "nombre");
            var direccionEmpresa = Clases.ClassParametros.buscarParametro("empresa", "direccion");
            var telEmpresa = Clases.ClassParametros.buscarParametro("empresa", "telefono");
            var cuilEmpresa = Clases.ClassParametros.buscarParametro("empresa", "cuit");
            instReport.GenerarYMostrarRecibo(recibo.Rows[0]["Recibo"].ToString(), logo, nombreEmpresa, direccionEmpresa, telEmpresa, cuilEmpresa, DateTime.Parse(recibo.Rows[0]["Fecha"].ToString()), recibo.Rows[0]["Cliente"].ToString(), recibo.Rows[0]["cuil"].ToString(), recibo.Rows[0]["Observaciones"].ToString(), decimal.Parse(recibo.Rows[0]["ImporteTotal"].ToString()));
        }
        private void btnCobrar_Click(object sender, EventArgs e)
        {
            BindingList<Clases.ClassVentas.CobroFormasPago> dtFormasPAgo = new BindingList<Clases.ClassVentas.CobroFormasPago>();
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            Clases.ClassClientes instClie = new Clases.ClassClientes();
            var planPagoDT = instConfig.traerPlanesPagoPorId(int.Parse(Clases.ClassParametros.buscarParametro("Cobros", "idPlanEfectivo")));
            if (planPagoDT.Rows.Count == 0) return;
            var planPagoId = int.Parse(planPagoDT.Rows[0]["id"].ToString());
            Formularios.Ventas.frmImputacionVenta unFrmImputacion = new Formularios.Ventas.frmImputacionVenta(calcularTotal() <= 0? 0: calcularTotal(), planPagoId);
            unFrmImputacion.ShowDialog();
            if (unFrmImputacion.DialogResult == DialogResult.OK)
            {
                dtFormasPAgo = unFrmImputacion.unDT;
                var imputacion = dtFormasPAgo.Sum(x => x.Importe);
                if (imputacion <= 0) return;
                var salida = instClie.CobrarCliente(_cliente, imputacion);

                if (salida != -1)
                {
                    MessageBox.Show(this, "Cobro Registrado con éxito!!", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                    var recibo = instClie.traerDatosRecibo(salida);
                    if (recibo.Rows.Count > 0)
                    {
                        imprimirCobro(recibo);
                     }
                    cargarGrilla();
                }
                else
                {
                    MessageBox.Show(this, "Problemas para registrar cobro", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNC_Click(object sender, EventArgs e)
        {
            frmNC unFrmNC = new frmNC(_cliente, calcularTotal() <= 0 ? 0 : calcularTotal());
            unFrmNC.ShowDialog();
            cargarGrilla();
        }

        private void btnND_Click(object sender, EventArgs e)
        {
            frmAddND unFrmNC = new frmAddND(_cliente);
            unFrmNC.ShowDialog();
            cargarGrilla();
        }

        private void frmClientesCC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
            {
                btnCobrar_Click(null, null);
            }

            if (e.KeyData == Keys.F3)
            {
                btnNC_Click(null, null);
            }

            if (e.KeyData ==  Keys.F4)
            {
                btnND_Click(null, null);
            }
        }

        private void dgvCC_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // evita encabezados
            {
                var fila = dgvCC.Rows[e.RowIndex];

                if (fila.Cells["Movimiento"].Value.ToString() != "Cobro") return;

                var id = int.Parse(fila.Cells["Numero Referencia"].Value.ToString());

                Clases.ClassClientes instClie = new Clases.ClassClientes();
                var recibo = instClie.traerDatosRecibo(id);
                if (recibo.Rows.Count > 0)
                {
                    imprimirCobro(recibo);
                }
            }
        }
    }
}
