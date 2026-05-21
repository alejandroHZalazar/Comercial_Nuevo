using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    public partial class frmReportesProductos : Form
    {
        Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
        int cantDec = Clases.ClassProductos.cantDecimales();

        public frmReportesProductos()
        {
            InitializeComponent();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            string filtro = string.Empty;
            if (cbRubro.Checked)
            {
                filtro = " where p.fk_rubro = " + cboRubro.SelectedValue.ToString();
            }
            if (cbProveedor.Checked)
            {
                if (cbRubro.Checked)
                    filtro += " and p.fk_proveedor = " + cboProveedor.SelectedValue.ToString();
                else
                    filtro = " where p.fk_proveedor = " + cboProveedor.SelectedValue.ToString();
            }

            if (filtro == string.Empty)
                filtro = " where p.baja = 0 ";
            else
                filtro += " and p.baja = 0";

            var instReportes = new Clases.ClassReportesITextSharp();
            instReportes.GenerarListaPreciosPDF(filtro, cantDec);
        }

        private void frmReportesProductos_Load(object sender, EventArgs e)
        {
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();
            cboRubro.DataSource = instConfig.traeRubros();
            cboRubro.ValueMember = "id";
            cboRubro.DisplayMember = "descripcion";
            cboRubro.SelectedIndex = 0;

            cboProveedor.DataSource = instProv.traeProveedores();
            cboProveedor.DisplayMember = "nombreComercial";
            cboProveedor.ValueMember = "id";
            cboProveedor.SelectedIndex = 0;

        }
    }
}
