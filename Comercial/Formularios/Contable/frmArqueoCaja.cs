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
    public partial class frmArqueoCaja : Form
    {
        private int _cajaId;
        private int _ingresoArqueoId;
        private int _egresoArqueoId;
        private int _medioPagoEfectivo;
        private decimal _cajaActual;
        public frmArqueoCaja(int cajaId, int ingresoArqueoId, int egresoArqueoId, int medioPagoEfectivo, decimal cajaActual)
        {
            _cajaId = cajaId;
            _ingresoArqueoId = ingresoArqueoId;
            _egresoArqueoId = egresoArqueoId;
            _medioPagoEfectivo = medioPagoEfectivo;
            _cajaActual = cajaActual;
            InitializeComponent();
        }

        private void frmArqueoCaja_Load(object sender, EventArgs e)
        {
            lblSistema.Text = _cajaActual.ToString("C");
            nudDineroFisico.Focus();
            nudDineroFisico.Select(0, nudDineroFisico.Text.Length);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                Clases.ClassCaja instCaja = new Clases.ClassCaja();
                var salida = "";
                if (_cajaActual < nudDineroFisico.Value)
                {
                    salida = instCaja.AddMovimiento(_cajaId, _ingresoArqueoId, _medioPagoEfectivo, nudDineroFisico.Value - _cajaActual, rtbObservacion.Text.Trim());
                }
                if (_cajaActual > nudDineroFisico.Value)
                {
                    salida = instCaja.AddMovimiento(_cajaId, _egresoArqueoId, _medioPagoEfectivo, _cajaActual - nudDineroFisico.Value, rtbObservacion.Text.Trim());
                }

                if (salida == "1")
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Arqueo de Caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool formularioValido()
        {
            errorProvider1.Clear();

            if (nudDineroFisico.Value <= 0)
            {
                errorProvider1.SetError(nudDineroFisico, "Debe ingresar Valor de Arqueo");
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
