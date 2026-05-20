using Comercial.Clases;
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

    public partial class frmNC : Form
    {
        int facturaElectronica = Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica"));
        int puntoVenta = Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName) == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName));
        int tieneCaja = Clases.ClassParametros.buscarParametro("caja", "haceCaja") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("caja", "haceCaja"));
        int _cliente;
        decimal _importe;
        int CajaId = 0;
        public frmNC(int cliente, decimal importe)
        {
            _cliente = cliente;
            _importe = importe;
            InitializeComponent();
        }

        private void frmNC_Load(object sender, EventArgs e)
        {
            nudImputar.Value = _importe;
            nudImputar.Select(0, nudImputar.Text.Length);
        }

        private async void btnGrabar_Click(object sender, EventArgs e)
        {
            if (fomularioValido())
            {
                if (facturaElectronica == 1)
                {
                    ClassFacturacionElectronica instFactElect = new ClassFacturacionElectronica();
                    Formularios.Facturacion.frmIngresarDatosNC unFrmIngresarDatosNC = new Facturacion.frmIngresarDatosNC(nudImputar.Value,null, null, false);
                    unFrmIngresarDatosNC.ShowDialog();
                    if (unFrmIngresarDatosNC.DialogResult == DialogResult.OK && unFrmIngresarDatosNC._compAsociado > 0)
                    {
                        var status = await instFactElect.emitirNotaCredito(0, unFrmIngresarDatosNC._compAsociado, unFrmIngresarDatosNC._fechaCompAsoc, unFrmIngresarDatosNC._importe, _cliente, unFrmIngresarDatosNC._iva, unFrmIngresarDatosNC._impuesto);

                        if (!status)
                        {
                            MessageBox.Show(this, "Ha ocurrido un error en el proceso de emisión de Nota de Crédito", "DEVOLUCION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                Clases.ClassClientes instClie = new Clases.ClassClientes();

                if (tieneCaja == 1)
                {
                    Clases.ClassCaja instCaja = new Clases.ClassCaja();
                    DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

                    bool cajaAbierta = cajaEstado.Rows.Count == 0 ? false : (cajaEstado.Rows[0]["estado"].ToString() == "ABIERTA" ? true : false);
                    CajaId = cajaEstado.Rows.Count == 0 ? 0 : int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());
                }

                    var salida = instClie.NC_Cliente(_cliente, nudImputar.Value, rbtObserv.Text.Trim(), tieneCaja, CajaId);
                if (salida != -1)
                {
                    MessageBox.Show(this, "Nota de Crédito ingresada con éxito!!", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error al momento de generar la nota de crédito", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            
        }

        private bool fomularioValido()
        {
            errorProvider1.Clear();

            if (nudImputar.Value <= 0)
            {
                errorProvider1.SetError(nudImputar, "Debe ingresar un valor mayor a 0");
                return false;
            }

            if (rbtObserv.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(rbtObserv, "Debe ingresar una observación");
                return false;
            }
            return true;
        }
        private void frmNC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
            {
                btnGrabar_Click(null, null);
            }
        }
    }
}
