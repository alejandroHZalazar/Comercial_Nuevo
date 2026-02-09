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
    public partial class frmEgresoDinero : Form
    {
        private int _cajaId;
        private int _egresoConcepto;
        private int _medioPagoEfectivo;
        public frmEgresoDinero(int cajaId, int egresoConcepto, int medioPagoEfectivo)
        {
            _cajaId = cajaId;
            _egresoConcepto = egresoConcepto;
            _medioPagoEfectivo = medioPagoEfectivo;
            InitializeComponent();
        }

        private void frmEgresoDinero_Load(object sender, EventArgs e)
        {
            nudDineroEgreso.Focus();
            nudDineroEgreso.Select(0, nudDineroEgreso.Text.Length);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                Clases.ClassCaja instCaja = new Clases.ClassCaja();
                var salida = instCaja.AddMovimiento(_cajaId, _egresoConcepto, _medioPagoEfectivo, nudDineroEgreso.Value, rtbObservacion.Text.Trim());

                if (salida == "1")
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Retiro Dinero a Caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool formularioValido()
        {
            errorProvider1.Clear();

            if (nudDineroEgreso.Value <= 0)
            {
                errorProvider1.SetError(nudDineroEgreso, "Debe ingresar Valor de retiro");
                return false;
            }

            if (rtbObservacion.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(rtbObservacion, "Debe ingresar observación");
                return false;
            }

            return true;
        }
    }
}
