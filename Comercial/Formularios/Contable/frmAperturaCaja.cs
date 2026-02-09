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
    public partial class frmAperturaCaja : Form
    {
        private decimal _dinero;
        private string _fechaUltimaCaja;
        public frmAperturaCaja(decimal dinero, string fechaUltimaCaja)
        {
            _dinero = dinero;
            _fechaUltimaCaja = fechaUltimaCaja;
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void frmAperturaCaja_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = Environment.GetEnvironmentVariable("nombreUser");
            lblCajaAnterior.Text = _dinero.ToString();
            lblFechaUltimoCierre.Text = _fechaUltimaCaja;
        }

        private void btnAbrirCaja_Click(object sender, EventArgs e)
        {
            string salida;
            Clases.ClassCaja instCaja = new Clases.ClassCaja();

            string observacionUsuario = rtbObservacion.Text.Trim();

            string observacionFinal = "Apertura Caja:" + Environment.NewLine + observacionUsuario;

            salida = instCaja.AperturaCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")), observacionFinal, _dinero);
           
            if (salida == "1")
            {
                this.Close();

            }
            else
            {
                MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Apertura de Caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
