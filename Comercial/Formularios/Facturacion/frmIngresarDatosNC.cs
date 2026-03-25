using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Facturacion
{
    public partial class frmIngresarDatosNC : Form
    {
        public decimal? _importe;
        public decimal? _iva;
        public decimal? _impuesto;
        public bool _esDevolucion;
        public int _compAsociado;
        public string _fechaCompAsoc;
        public frmIngresarDatosNC(decimal? importe, decimal? IVA, decimal? impuesto, bool esDevolucion)
        {
            InitializeComponent();
            _importe = importe;
            _iva = IVA;
            _impuesto = impuesto;
            _esDevolucion = esDevolucion;
        }

        private void frmIngresarDatosNC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                btnSiguiente_Click(null, null);
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                _importe = nudImporte.Value;
                _iva = (decimal)cboIva.SelectedValue;
                _impuesto = (decimal)cboIIBB.SelectedValue;
                _compAsociado = int.Parse(txtFacturaAsociada.Text == string.Empty ? "0" : txtFacturaAsociada.Text);
                _fechaCompAsoc = dtpFecha.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void frmIngresarDatosNC_Load(object sender, EventArgs e)
        {
            cargarCombos();

            nudImporte.Enabled = cboIva.Enabled = cboIIBB.Enabled = !_esDevolucion;

            if (_esDevolucion)
            {               
                cboIva.SelectedValue = _iva;
                cboIIBB.SelectedValue = _impuesto;
            }

            nudImporte.Value = _importe ?? 0;

        }

        private void cargarCombos()
        {
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();

            cboIva.DataSource = instProv.traePorcentajeIVA();
            cboIva.ValueMember = "valor";
            cboIva.DisplayMember = "valor";

            cboIIBB.DataSource = instProv.traerPorcentajeImpuestos();
            cboIIBB.ValueMember = "valor";
            cboIIBB.DisplayMember = "valor";
        }

        private bool formularioValido()
        {
            errorProvider1.Clear();           

            if (dtpFecha.Value > DateTime.Now)
            {
                errorProvider1.SetError(dtpFecha, "Debe Ingresar una fecha menor o igual a la actual");
                return false;
            }

            if (nudImporte.Value <= 0)
            {
                errorProvider1.SetError(nudImporte, "Debe Ingresar un monto decimal mayor a 0");
                return false;
            }

            if (cboIva.SelectedIndex < 0)
            {
                errorProvider1.SetError(cboIva, "Debe seleccionar el porcentaje de IVA");
                return false;
            }

            if (cboIIBB.SelectedIndex < 0)
            {
                errorProvider1.SetError(cboIIBB, "Debe seleccionar el porcentaje de Impuestos Internos");
                return false;
            }

            return true;
        }
    }
}
