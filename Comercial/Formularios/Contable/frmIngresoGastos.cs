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
    public partial class frmIngresoGastos : Form
    {
        private int _cajaId;
        private int _gastoConcepto;
        private int _medioPagoEfectivo;
        public frmIngresoGastos(int cajaId, int gastoConcepto, int medioPagoEfectivo)
        {
            _cajaId = cajaId;
            _gastoConcepto = gastoConcepto;
            _medioPagoEfectivo = medioPagoEfectivo;
            InitializeComponent();
        }

        private void frmIngresoGastos_Load(object sender, EventArgs e)
        {
            nudDineroGasto.Focus();
            nudDineroGasto.Select(0, nudDineroGasto.Text.Length);
            cargarCombos();
        }

        private void cargarCombos()
        {
            Clases.ClassCaja instCaja = new Clases.ClassCaja();
            cboTipoGastos.DataSource = instCaja.traerTipoGastos();
            cboTipoGastos.ValueMember = "id";
            cboTipoGastos.DisplayMember = "nombre";
            cboTipoGastos.SelectedIndex = -1;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                Clases.ClassCaja instCaja = new Clases.ClassCaja();
                var salida = instCaja.AddGasto(_cajaId, _gastoConcepto, _medioPagoEfectivo, nudDineroGasto.Value, rtbObservacion.Text.Trim(),int.Parse(cboTipoGastos.SelectedValue.ToString()));

                if (salida == "1")
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Ingreso Gastos a Caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool formularioValido()
        {
            errorProvider1.Clear();

            if (nudDineroGasto.Value <= 0)
            {
                errorProvider1.SetError(nudDineroGasto, "Debe ingresar Valor de ingreso");
                return false;
            }

            if (rtbObservacion.Text.Trim() == string.Empty)
            {
                errorProvider1.SetError(rtbObservacion, "Debe ingresar observación");
                return false;
            }

            if (cboTipoGastos.SelectedIndex == -1)
            {
                errorProvider1.SetError(cboTipoGastos, "Debe indicar un tipo de gasto");
                return false;
            }

            return true;
        }
    }
}
