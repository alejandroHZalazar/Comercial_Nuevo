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
    public partial class frmIngresoDinero : Form
    {
        private int _cajaId;
        private int _ingresoConcepto;
        private int _medioPagoEfectivo;
        public frmIngresoDinero(int cajaId, int ingresoConcepto, int medioPagoEfectivo)
        {
            _cajaId = cajaId;
            _ingresoConcepto = ingresoConcepto;
            _medioPagoEfectivo = medioPagoEfectivo;
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                Clases.ClassCaja instCaja = new Clases.ClassCaja();
                var salida = instCaja.AddMovimiento(_cajaId, _ingresoConcepto, _medioPagoEfectivo, nudDineroIngreso.Value, rtbObservacion.Text.Trim());

                if (salida == "1")
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Ingreso Dinero a Caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool formularioValido()
        {
            errorProvider1.Clear();

            if (nudDineroIngreso.Value <= 0)
            {
                errorProvider1.SetError(nudDineroIngreso, "Debe ingresar Valor de ingreso");
                return false;
            }

            if (rtbObservacion.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(rtbObservacion, "Debe ingresar observación");
                return false;
            }

            return true;
        }

        private void frmIngresoDinero_Load(object sender, EventArgs e)
        {
            nudDineroIngreso.Focus();
            nudDineroIngreso.Select(0, nudDineroIngreso.Text.Length);
        }

        private void nudDineroIngreso_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void rtbObservacion_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
