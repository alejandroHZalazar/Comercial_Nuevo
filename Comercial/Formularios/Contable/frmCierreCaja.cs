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
    public partial class frmCierreCaja : Form
    {
        private decimal _dinero;
        private string _fechaInicioCaja;
        private int _cajaId;
        public frmCierreCaja(decimal dinero, string fechaInicioCaja, int cajaId)
        {
            _dinero = dinero;
            _fechaInicioCaja = fechaInicioCaja;
            _cajaId = cajaId;
            InitializeComponent();
        }           

        private void btnCierreCaja_Click(object sender, EventArgs e)
        {
            string salida;
            Clases.ClassCaja instCaja = new Clases.ClassCaja();

            string observacionUsuario = rtbObservacion.Text.Trim();

            string observacionFinal = Environment.NewLine + "Cierre Caja:" + Environment.NewLine + observacionUsuario;

            salida = instCaja.CierreCaja(_cajaId, _dinero,observacionFinal);
           
            if (salida == "1")
            {
                this.Close();

            }
            else
            {
                MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Apertura de Caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmCierreCaja_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = Environment.GetEnvironmentVariable("nombreUser");
            lblFechaApertura.Text = _fechaInicioCaja;
            lblSaldoFinal.Text = _dinero.ToString("C");
        }
    }
}
