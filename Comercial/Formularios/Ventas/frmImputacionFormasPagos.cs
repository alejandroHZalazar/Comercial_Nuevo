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
    public partial class frmImputacionFormasPagos : Form
    {
        public decimal _total;
        public int? _planPago;
        public string _dato1;
        public string _dato2;
        public string _dato3;
        public bool _edicion;
        public frmImputacionFormasPagos(decimal total, int? planPago, string dato1, string dato2, string dato3, bool edicion)
        {
            _total = total;
            _planPago = planPago;
            _dato1 = dato1;
            _dato2 = dato2;
            _dato3 = dato3;
            _edicion = edicion;
            InitializeComponent();
        }

        private void frmImputacionFormasPagos_Load(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void estadoInicial()
        {
            lblTotal.Text = _total.ToString("C");
            nudImputar.Value = _total;            
            cargarCombos();
        }        

        private void cargarCombos()
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            cboMedioPago.DataSource = instConfig.traerPlanesPago();
            cboMedioPago.ValueMember = "id";
            cboMedioPago.DisplayMember = "Nombre";
        }

        private void frmImputacionFormasPagos_Shown(object sender, EventArgs e)
        {
            if (_edicion)
            {
                cboMedioPago.SelectedValue = _planPago;
                txtDato1.Text = _dato1;
                txtDato2.Text = _dato2;
                txtDato3.Text = _dato3;
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (validarFormulario())
            {
                _total = nudImputar.Value;
                _planPago = int.Parse(cboMedioPago.SelectedValue.ToString());
                _dato1 = txtDato1.Text.Trim();
                _dato2 = txtDato2.Text.Trim();
                _dato3 = txtDato3.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool validarFormulario()
        {
            errorProvider1.Clear();            

            if (cboMedioPago.SelectedIndex <0)
            {
                errorProvider1.SetError(cboMedioPago, "Debe seleccionar un medio de pago");
                return false;
            }

            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            var planPagoDB = instConfig.traerPlanesPagoPorId(int.Parse(cboMedioPago.SelectedValue.ToString()));
            if (planPagoDB == null) return false;
            var medioPagoDB = instConfig.traerMediosDePagoPorId(int.Parse(planPagoDB.Rows[0]["fk_medioPago"].ToString()));
            if (medioPagoDB == null) return false;
            if (bool.Parse(medioPagoDB.Rows[0]["conDatos"].ToString()) && txtDato1.Text == string.Empty && txtDato2.Text == string.Empty && txtDato3.Text == string.Empty)
            {
                errorProvider1.SetError(txtDato1, "El medio de pago seleccionado exige datos de cobro");
                return false;
            }

            return true;
            
        }

        private void frmImputacionFormasPagos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
            {
                btnGrabar_Click(null, null);
            }
        }
    }
}
