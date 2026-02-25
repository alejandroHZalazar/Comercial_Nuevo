using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Configuracion
{
    
    public partial class frmCotizacionDolar : Form
    {
        decimal valorDolar = Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar") == "" ? 0 : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar"));
        public frmCotizacionDolar()
        {
            InitializeComponent();
        }

        private void frmCotizacionDolar_Load(object sender, EventArgs e)
        {
            nudCotizacion.Value = valorDolar;
            nudCotizacion.Focus();
            nudCotizacion.Select(0, nudCotizacion.Text.Length);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Clases.ClassParametros.guardarParametro("productos", "cotizacionDolar", nudCotizacion.Value.ToString());
            this.Close();
        }
    }
}
