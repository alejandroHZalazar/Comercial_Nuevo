using Comercial.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Comercial.Formularios.Ventas
{
    public partial class frmVentasReportes : Form
    {
        int haceNotaVentaTK = Clases.ClassParametros.buscarParametro("ventas", "notaVentaTK") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "notaVentaTK"));
        int anchoTk = Clases.ClassParametros.buscarParametro("ventas", "anchoTk") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "anchoTk"));
        public frmVentasReportes()
        {
            InitializeComponent();
        }

        Clases.ClassClientes instClie = new Clases.ClassClientes();
        Clases.ClassVentas instVentas = new Clases.ClassVentas();

        int cantDec = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();

        private void frmVentasReportes_Load(object sender, EventArgs e)
        {
            estadoInicial();
            cboCliente.DataSource = instClie.buscarAVender();
            cboCliente.DisplayMember = "Completo";
            cboCliente.ValueMember = "ID";
            cboCliente.SelectedIndex = 0;
        }

        private void estadoInicial()
        {
            dgvDetalle.DataSource = null;
            dgvVentasCabecera.DataSource = null;
            dgvDetalle.Rows.Clear();
            dgvVentasCabecera.Rows.Clear();
            if (haceNotaVentaTK == 1 && anchoTk == 0)
            {
                MessageBox.Show(this, "Debe establecer el ancho del ticket", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            estadoInicial();
            string unFiltro = " v.fecha between '" + dtpDesde.Value.Year + "-" + dtpDesde.Value.Month + "-" + dtpDesde.Value.Day + "' and '" + dtpHasta.Value.Year + "-" + dtpHasta.Value.Month + "-" + dtpHasta.Value.Day + " 23:59:59'";

            if (cbCliente .Checked == true)
            { 
                unFiltro = unFiltro + " and v.fk_cliente = " + cboCliente .SelectedValue .ToString ();
            }

            dgvVentasCabecera.DataSource = instVentas.traerTodos(unFiltro);

            if (dgvVentasCabecera .Rows .Count > 0)
            {
                dgvVentasCabecera.Columns["id"].Visible = false;
                dgvVentasCabecera.Rows[0].Selected = true;
            }

            redondearEncabezado();
        }

        private void redondearEncabezado()
        {
            if (dgvVentasCabecera.RowCount > 0)
            {
                dgvVentasCabecera.Columns["IVA"].DefaultCellStyle.Format = "N" + cantDec.ToString();
                dgvVentasCabecera.Columns["Total"].DefaultCellStyle.Format = "N" + cantDec.ToString();
                dgvVentasCabecera.Columns["Costo"].DefaultCellStyle.Format = "N" + cantDec.ToString();
                dgvVentasCabecera.Columns["descuento"].DefaultCellStyle.Format = "N" + cantDec.ToString();
            }
            
        }

        private void dgvVentasCabecera_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvDetalle.DataSource = instVentas.traerTodosDetalles(long.Parse(dgvVentasCabecera.CurrentRow.Cells["id"].Value.ToString()));
            redondearDetalle();
        }

        private void redondearDetalle()
        {
            if(dgvDetalle .RowCount > 0)
            {
                dgvDetalle.Columns["Precio_S_IVA"].DefaultCellStyle.Format = "N" + cantDec.ToString();
                dgvDetalle.Columns["Precio_C_IVA"].DefaultCellStyle.Format = "N" + cantDec.ToString();
                dgvDetalle.Columns["Cantidad"].DefaultCellStyle.Format = "N" + cantStock .ToString();
                dgvDetalle.Columns["Subtotal"].DefaultCellStyle.Format = "N" + cantDec.ToString();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (dgvVentasCabecera.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar una venta para imprimir.", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvVentasCabecera .Rows .Count > 0)
            {
                if (haceNotaVentaTK == 0)
                {
                    Reportes.frmReport unFrmReport = new Reportes.frmReport();

                    unFrmReport.nombreReporte = "ReportVenta.rdlc";
                    List<string> var = new List<string>();
                    var.Add(dgvVentasCabecera.CurrentRow.Cells["id"].Value.ToString());
                    var.Add(Clases.ClassValidacion.traerEmpresa());
                    var.Add("Tel: " + Clases.ClassValidacion.traerEmpresaTelefono());
                    var.Add(Clases.ClassValidacion.traerEmpresaDireccion());
                    var.Add(Clases.ClassValidacion.traerEmpresaCiudad());
                    var.Add("CUIT: " + Clases.ClassValidacion.traerEmpresaCuit());
                    var.Add(dgvVentasCabecera.CurrentRow.Cells["IVA"].Value.ToString());
                    var.Add(cantDec.ToString());
                    var.Add(cantStock.ToString());
                    var.Add(Clases.ClassValidacion.traerRazonSocial());
                    unFrmReport.variable = var;
                    unFrmReport.ShowDialog();
                }
                else
                {                    
                        imprimirNotaVentaTk(long.Parse(dgvVentasCabecera.CurrentRow.Cells["id"].Value.ToString()));
                    
                }
            }
        }

        private void imprimirNotaVentaTk(long venta)
        {
            var ventaDT = instVentas.imprimirVenta(venta);

            if (ventaDT.Rows.Count == 0) return;

            var items = new List<ItemVenta>();

            foreach (DataRow fila in ventaDT.Rows)
            {
                items.Add(new ItemVenta
                {
                    Descripcion = fila["descripcion"].ToString(),
                    PrecioUnit = Convert.ToDecimal(fila["precioConIva"]),
                    Cantidad = Convert.ToDecimal(fila["cantidad"]),
                    subtotal = Convert.ToDecimal(fila["subtotal"]),
                });
            }

            var ticket = new TicketPrinter(items, ventaDT.Rows[0]["Vendedor"].ToString(), ventaDT.Rows[0]["nombreComercial"].ToString(), 
                                            decimal.Parse (ventaDT.Rows[0]["totalVenta"].ToString()), DateTime.Parse(ventaDT.Rows[0]["fecha"].ToString()), 
                                            ventaDT.Rows[0]["nroVenta"].ToString(), decimal.Parse(ventaDT.Rows[0]["descuento"].ToString())
                                            , decimal.Parse(ventaDT.Rows[0]["recargo"].ToString()), anchoTk == 80 ? 42 : 32); // 42 = 80mm
            ticket.Imprimir();
        }
    }
}