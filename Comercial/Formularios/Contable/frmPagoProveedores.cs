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
    public partial class frmPagoProveedores : Form
    {
        int _cajaid;
        int _medioPagoEfectivoId;
        int _conceptoPago;
        public frmPagoProveedores(int cajaId, int medioPagoEfectivo, int conceptoPago)
        {
            _cajaid = cajaId;
            _medioPagoEfectivoId = medioPagoEfectivo;
            _conceptoPago = conceptoPago;
            InitializeComponent();
        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            cargarCombos();

        }

        private void cargarCombos()
        {
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();
            cboProveedor.DataSource = instProv.traeProveedores();
            cboProveedor.ValueMember = "id";
            cboProveedor.DisplayMember = "nombreComercial";
        }

        private void btnPagoProveedores_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                Clases.ClassCaja instCaja = new Clases.ClassCaja();
                var salida = instCaja.AddCajaPagoProveedores(int.Parse(cboProveedor.SelectedValue.ToString()), cboProveedor.Text, nudPagoProveedor.Value, rtbObservacion.Text.Trim(), _cajaid, _conceptoPago, _medioPagoEfectivoId);

                if (salida != -1)
                {
                    MessageBox.Show(this, "Pago registrado con éxito!!", "CAJA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error en la gestion de pagos a proveedores", "CAJA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool formularioValido()
        {
            errorProvider1.Clear();
            if (cboProveedor.SelectedIndex < 0)
            {
                errorProvider1.SetError(cboProveedor, "Debe seleccionar un Proveedor");
                return false;
            }

            if (nudPagoProveedor.Value <= 0)
            {
                errorProvider1.SetError(nudPagoProveedor, "Debe ingresar un importe");
                return false;
            }

            return true;
        }
    }
}
