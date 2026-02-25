using Comercial.Clases;
using Comercial.Formularios.Clientes;
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
    public partial class frmVentas : Form
    {
        public int unCliente;
        int unProducto = 0;
        bool cargado = false;
        int unTipoBusq = 1;
        int unPedido = 0;
        decimal unCosto = 0;
        int pedidoCargado;
        public string filtro;
        public bool buscoPend = false;
        public int pos = 0;
        bool esfraccionado = false;


        private List<string> resgClientes = new List<string>();
        private List<string> resgProducto = new List<string>();
        Clases.ClassClientes instClie = new Clases.ClassClientes();

        Clases.ClassPedidos instPed = new Clases.ClassPedidos();
        Clases.ClassProductos instProd = new Clases.ClassProductos();
        Clases.ClassProveedores instProv = new Clases.ClassProveedores();
        Clases.ClassVentas instVentas = new Clases.ClassVentas();
        Clases.classUsuarios instUser = new Clases.classUsuarios();
        int cantDec = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();
        int llevaCC = Clases.ClassParametros.buscarParametro("clientes", "llevaCC") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("clientes", "llevaCC"));
        int comisiona = Clases.ClassParametros.buscarParametro("ventas", "comisiona") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "comisiona"));
        int tieneConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "tieneConsumidorFinal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "tieneConsumidorFinal"));
        int clienteConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal"));
        int tieneMediosPagos = Clases.ClassParametros.buscarParametro("ventas", "mediosPagos") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "mediosPagos"));
        int imputaEnVenta = Clases.ClassParametros.buscarParametro("ventas", "pagosEnVenta") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "pagosEnVenta"));
        int tieneProductosBalanza = Clases.ClassParametros.buscarParametro("productos", "tieneProductosBalanza") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "tieneProductosBalanza"));
        string prefijoBalanza = Clases.ClassParametros.buscarParametro("productos", "prefijoBalanza");
        string posicionProductoBalanza = Clases.ClassParametros.buscarParametro("productos", "posicionProducto");
        string posicionPeso = Clases.ClassParametros.buscarParametro("productos", "posicionPeso");
        string divisorPeso = Clases.ClassParametros.buscarParametro("productos", "divisorPeso");
        int tieneCaja = Clases.ClassParametros.buscarParametro("caja", "haceCaja") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("caja", "haceCaja"));
        int CajaId = 0;
        int facturaFiscal = Clases.ClassParametros.buscarParametro("ventas", "facturaFiscal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaFiscal"));
        int facturaElectronica = Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica"));
        string marcaFiscal = Clases.ClassParametros.buscarParametro("ventas", "marcaFiscal");
        int productosDolarizados = Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos"));
        decimal valorDolar = Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar") == "" ? 0 : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar"));
        int haceNotaVentaTK = Clases.ClassParametros.buscarParametro("ventas", "notaVentaTK") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "notaVentaTK"));
        int anchoTk = Clases.ClassParametros.buscarParametro("ventas", "anchoTk") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "anchoTk"));
        int puntoVenta = Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName) == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName));
        int tieneLectoraCB = Clases.ClassParametros.buscarParametro("productos", "MecanismoLectora") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "MecanismoLectora"));
        public frmVentas()
        {
            InitializeComponent();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            cargarClientes();
            cargarProductos();
            cargarCombos();
        }

        private void verificarParametros()
        {
            if (comisiona == 0)
            {
                cboVendedores.SelectedValue = int.Parse(Environment.GetEnvironmentVariable("idUser"));
                cboVendedores.Enabled = false;
                nudComision.Value = 0;
                nudComision.Enabled = false;
            }

            if (tieneConsumidorFinal == 1)
            {
                if (clienteConsumidorFinal == 0)
                {
                    MessageBox.Show(this, "Debe crear y parametrizar el cliente CONSUMIDOR FINAL", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();

                }
                else
                {
                    DataTable cliente = instClie.traerDatosVenta(" and c.id = " + clienteConsumidorFinal.ToString());
                    cargarDatosClientesFormulario(cliente);
                    txtFiltro.Focus();
                }
            }
            else
            {
                txtCliente.Focus();
            }

            if (tieneMediosPagos == 0)
            {
                lblMedioPago.Visible = cboMedioPago.Visible = false;
            }

            if (tieneCaja == 1)
            {
                Clases.ClassCaja instCaja = new Clases.ClassCaja();
                DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

                bool cajaAbierta = cajaEstado.Rows.Count == 0 ? false : (cajaEstado.Rows[0]["estado"].ToString() == "ABIERTA" ? true : false);
                CajaId = cajaEstado.Rows.Count == 0 ? 0 : int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());
                if (!cajaAbierta)
                {

                    MessageBox.Show(this, "Debe Abrir Caja", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
            }

            if (haceNotaVentaTK == 1 && anchoTk == 0)
            {
                MessageBox.Show(this, "Debe establecer el ancho del ticket", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }


        }
        private void estadoInicial()
        {
            lblDescripcion.Text = string.Empty;
            lblClienteNombre.Text = string.Empty;
            dgvProductos.Rows.Clear();
            lbDesc.Visible = false;
            lblCliente.Visible = false;
            cboIVA.Enabled = false;
            cboIngBrutos.Enabled = false;
            gbFiltro.Enabled = false;
            dgvProductos.Enabled = false;
            nudDescuento.Enabled = false;
            nudRecargo.Enabled = false;
            cboMedioPago.Enabled = false;
            btnGrabar.Enabled = false;
            cboFiltro.SelectedIndex = Clases.ClassParametros.indiceBusqNotaPed();
            txtCliente.Text = string.Empty;
            cboIVA.SelectedIndex = 0;
            cboIngBrutos.SelectedIndex = 0;
            txtSubSinIVA.Text = "0";
            nudDescuento.Value = 0;
            nudRecargo.Value = 0;
            unCliente = 0;
            unProducto = 0;
            txtSubSinIVA.Text = "0";
            cboTipo.Enabled = false;
            cboTipo.SelectedIndex = 0;
            btnAltaCliente.Enabled = true;
            nudComision.Value = -1;
            lblDir.Text = string.Empty;
            lblTel.Text = string.Empty;
            lblCondIVA.Text = string.Empty;
            lblEncargado.Text = string.Empty;
            nudCantidad.DecimalPlaces = cantStock;
            btnCambioPrecio.Enabled = false;
            btnCambioPrecio.Text = "Precios Actualizados";
            pedidoCargado = 0;
            txtTotalSinIva.Text = "0";
            txtIVA.Text = "0";
            txtIB.Text = "0";
            txtTotGeneral.Text = "0";
            cboVendedores.SelectedIndex = -1;
            cboMedioPago.SelectedIndex = (tieneMediosPagos == 1 ? 0 : -1);

            verificarParametros();
        }

        private void cargarCombos()
        {
            cboIVA.DataSource = instProv.traePorcentajeIVA();
            cboIVA.ValueMember = "id";
            cboIVA.DisplayMember = "valor";

            cboVendedores.DataSource = instUser.traerTodosUsuarios();
            cboVendedores.ValueMember = "id";
            cboVendedores.DisplayMember = "nombre";
            cboVendedores.SelectedIndex = -1;

            cboIngBrutos.DataSource = instProv.traerPorcentajeImpuestos();
            cboIngBrutos.ValueMember = "id";
            cboIngBrutos.DisplayMember = "valor";

            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();

            cboMedioPago.DataSource = instVentas.traerPlanesPago();
            cboMedioPago.ValueMember = "Recargo";
            cboMedioPago.DisplayMember = "Nombre";

            cargado = true;
        }

        private void cargarProductos()
        {
            resgProducto.Clear();
            DataTable productos = instProd.traeProductosPpal(" where baja = 0 order by descripcion");

            if (productos.Rows.Count > 0)
            {
                foreach (DataRow fila in productos.Rows)
                {
                    resgProducto.Add(fila["Descripcion"].ToString());
                }
            }
        }

        private void cargarClientes()
        {
            resgClientes.Clear();
            DataTable clientes = instClie.buscarAVender();

            if (clientes.Rows.Count > 0)
            {
                foreach (DataRow fila in clientes.Rows)
                {
                    resgClientes.Add(fila["ID"].ToString() + "-  " + fila["Nombre_Comercial"].ToString() + "  -" + fila["Dir"].ToString());
                }
            }

        }


        private void btnPedido_Click(object sender, EventArgs e)
        {
            Formularios.Ventas.frmPedidosPendientes unFrmPendientes = new frmPedidosPendientes(this);
            unFrmPendientes.llamador = this;
            unFrmPendientes.buscarYa = buscoPend;
            unFrmPendientes.position = pos;
            unFrmPendientes.filtro = this.filtro;
            unFrmPendientes.ShowDialog();
            if (unFrmPendientes.DialogResult == DialogResult.OK)
            {
                buscoPend = true;
            }
            else
            {
                buscoPend = false;
            }
        }

        public void cargarPedidoPendiente(int unPed, decimal unIva, decimal unDescuento, decimal unRecargo)
        {
            unPedido = 0;
            pedidoCargado = unPed;
            DataTable cliente = instClie.traeClientesPpal(" and c.id = " + unCliente);
            if (cliente.Rows.Count > 0)
            {
                lblClienteNombre.Text = cliente.Rows[0]["Nombre_Comercial"].ToString();
                estadoConCliente();
            }
            DataTable pedido = instPed.traerDetallePendientes(unPed);
            dgvProductos.Rows.Clear();
            if (pedido.Rows.Count > 0)
            {
                unPedido = unPed;
                foreach (DataRow fila in pedido.Rows)
                {
                    dgvProductos.Rows.Add(fila["Cod_Barras"].ToString(), fila["Cod_Proveedor"].ToString(), fila["Descripcion"].ToString(), Math.Round(decimal.Parse(fila["Stock"].ToString()), cantStock), Math.Round(decimal.Parse(fila["Precio S/IVA"].ToString()), cantDec), Math.Round(decimal.Parse(fila["Precio C/IVA"].ToString()), cantDec), Math.Round(decimal.Parse(fila["Cantidad"].ToString()), cantStock), Math.Round(decimal.Parse(fila["Subtotal"].ToString()), cantDec), Math.Round(decimal.Parse(fila["precioOrig"].ToString()), cantDec), fila["fk_producto"].ToString(), unPedido, Math.Round(decimal.Parse(fila["costo"].ToString()), cantDec), Convert.ToBoolean(fila["fraccionado"]), Convert.ToBoolean(fila["dolarizado"]));
                }
            }
            unPedido = 0;
            nudDescuento.Value = unDescuento;
            nudRecargo.Value = unRecargo;
            cboIVA.Text = unIva.ToString();
            procesoTotales();
            btnCambioPrecio.Enabled = true;
        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {
            if (txtCliente.Focused & txtCliente.Text != string.Empty)
            {
                filtrarCargarListBoxClie();
            }
            else
            {
                lblCliente.Visible = false;
            }
        }

        private void filtrarCargarListBoxClie()
        {
            lblCliente.Items.Clear();

            if (txtCliente.Text.Trim().Length == 0)
            {
                lblCliente.Visible = false;
            }

            var result = resgClientes.FindAll(l => l.ToUpper().Contains(txtCliente.Text.Trim().ToUpper()));

            lblCliente.Items.Clear();

            if (result.Count > 0)
            {
                result.ForEach(x => lblCliente.Items.Add(x));
                lblCliente.Visible = true;
            }
        }

        private void lblCliente_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cargarDatosCliente(0, 0);
        }

        private void lblCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cargarDatosCliente(0, 0);
            }
        }

        public void cargarDatosCliente(int tipo, int unCliente)
        {
            DataTable cliente;
            if (tipo == 0)
            {
                cliente = instClie.traerDatosVenta(" and c.id = " + lblCliente.SelectedItem.ToString().Substring(0, lblCliente.SelectedItem.ToString().IndexOf("-")));
            }
            else
            {
                cliente = instClie.traerDatosVenta(" and c.id = " + unCliente.ToString());
            }

            if (cliente.Rows.Count > 0)
            {

                cargarDatosClientesFormulario(cliente);
            }
        }

        private void cargarDatosClientesFormulario(DataTable unCliente)
        {
            this.unCliente = int.Parse(unCliente.Rows[0]["ID"].ToString());
            lblClienteNombre.Text = unCliente.Rows[0]["Nombre_Comercial"].ToString();
            lblDir.Text = unCliente.Rows[0]["Dir"].ToString() + ". " + unCliente.Rows[0]["Localidad"].ToString() + ". " + unCliente.Rows[0]["Provincia"].ToString();
            lblTel.Text = unCliente.Rows[0]["Tel"].ToString();
            lblCondIVA.Text = unCliente.Rows[0]["CondIVA"].ToString();
            lblEncargado.Text = unCliente.Rows[0]["contacto"].ToString();
            lbDesc.Visible = false;
            txtCliente.Text = string.Empty;
            estadoConCliente();
            txtFiltro.Focus();
        }

        private void estadoConCliente()
        {
            lblDescripcion.Text = string.Empty;
            lbDesc.Visible = false;
            cboIVA.Enabled = true;
            cboIngBrutos.Enabled = true;
            gbFiltro.Enabled = true;
            dgvProductos.Enabled = true;
            nudDescuento.Enabled = true;
            nudRecargo.Enabled = true;
            cboMedioPago.Enabled = true;
            btnGrabar.Enabled = true;
            cboIVA.SelectedIndex = 0;
            cboIngBrutos.SelectedIndex = 0;
            cboTipo.Enabled = true;
            btnPedido.Enabled = true;
            btnAltaCliente.Enabled = tieneConsumidorFinal == 0 ? false : true; ;
            txtFiltro.Focus();
        }

        private void cboIVA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargado)
            {
                procesoTotales();
            }
        }

        private void txtFiltro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter & txtFiltro.Text != string.Empty)
            {
                buscarProducto();
                unTipoBusq = 1;
                txtFiltro.Text = string.Empty;
            }
        }

        private void lbDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Back)
            {
                txtDesc.Focus();
            }

            if (e.KeyData == Keys.Enter & lbDesc.Items.Count > 0)
            {
                prepararProducto();

            }
        }

        private void nudCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }
        }

        private void nudCantidad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnAgregar_Click(null, null);
            }
        }

        private void dgvProductos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            procesoTotales();
        }

        private void procesoTotales()
        {

            decimal totalSIVA = 0;
            decimal totalDescuento = 0;
            decimal totalRecargo = 0;

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                fila.Cells["precioConIva"].Value = Math.Round(decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()) * (1 + Decimal.Parse(cboIVA.Text) / 100), cantDec);
                //fila.Cells["Subtotal"].Value = !bool.Parse(fila.Cells["fraccionado"].Value.ToString())? Math.Round(decimal.Parse(fila.Cells["precioConIva"].Value.ToString()) * decimal.Parse(fila.Cells["Cantidad"].Value.ToString()), cantDec):Math.Round(decimal.Parse(fila.Cells["Subtotal"].Value.ToString()),cantDec);
                fila.Cells["Subtotal"].Value = !bool.Parse(fila.Cells["fraccionado"].Value.ToString()) ? decimal.Parse(fila.Cells["precioConIva"].Value.ToString()) * decimal.Parse(fila.Cells["Cantidad"].Value.ToString()) : Math.Round(decimal.Parse(fila.Cells["precioConIva"].Value.ToString()), cantDec);
                totalSIVA += !bool.Parse(fila.Cells["fraccionado"].Value.ToString()) ? Math.Round(decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()) * decimal.Parse(fila.Cells["Cantidad"].Value.ToString()), cantDec) : Math.Round(decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()), cantDec);

            }

            if (dgvProductos.RowCount > 0)
            {
                txtSubSinIVA.Text = totalSIVA.ToString();
                totalDescuento = Math.Round(totalSIVA * (nudDescuento.Value / 100), cantDec);
                totalRecargo = Math.Round(totalSIVA * (nudRecargo.Value / 100), cantDec);
                txtTotalSinIva.Text = Math.Round((totalSIVA - totalDescuento + totalRecargo), cantDec).ToString();


                txtDescuento.Text = Math.Round(totalDescuento * -1 + totalRecargo, cantDec).ToString();
                txtIVA.Text = (Math.Round((totalSIVA - totalDescuento + totalRecargo) * (Decimal.Parse(cboIVA.Text) / 100), cantDec)).ToString();
                txtIB.Text = (Math.Round((totalSIVA - totalDescuento + totalRecargo) * (Decimal.Parse(cboIngBrutos.Text) / 100), cantDec)).ToString();
                // txtTotGeneral.Text = (Math.Round((totalSIVA - totalDescuento + totalRecargo ) * (1 + Decimal.Parse(cboIVA.Text) / 100), cantDec)).ToString();
                txtTotGeneral.Text = (Math.Round(decimal.Parse(txtTotalSinIva.Text) + decimal.Parse(txtIVA.Text) + decimal.Parse(txtIB.Text), cantDec)).ToString();
            }
        }

        private void buscarProducto()
        {
            DataTable producto = null;

            bool productoDeBalanzaEncontrado = false;

            if (txtFiltro.Text != string.Empty)
            {
                if (cboFiltro.SelectedIndex == 0)
                {
                    producto = instProd.traeProductosPpal(" where baja = 0 and codProveedor = '" + txtFiltro.Text.Trim() + "'");
                }
                else if (cboFiltro.SelectedIndex == 1)
                {
                    if (tieneProductosBalanza == 0)
                    {
                        producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + txtFiltro.Text.Trim());
                    }
                    else
                    {
                        if (prefijoBalanza == string.Empty || posicionProductoBalanza == string.Empty || posicionPeso == string.Empty || divisorPeso == string.Empty)
                        {
                            MessageBox.Show(this, "Para operar con productos de balanza debe parametrizar: Prefijo, Posición de Producto, Posicion de Importe y divisior de importe", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (Clases.ClassProductosBalanza.esCodigoBalanza(txtFiltro.Text.Trim(), prefijoBalanza))
                        {
                            var productoBalanzaId = Clases.ClassProductosBalanza.ExtraerPorPosicion(txtFiltro.Text.Trim(), posicionProductoBalanza);

                            if (productoBalanzaId == null) return;

                            producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + productoBalanzaId.Trim());
                            if (producto.Rows.Count > 0) productoDeBalanzaEncontrado = true;
                        }
                        else
                        {
                            producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + txtFiltro.Text.Trim());
                        }

                    }
                }
                else

                    producto = instProd.traeProductosPpal(" where baja = 0 and id = " + txtFiltro.Text.Trim());

            }

            if (producto.Rows.Count > 0)
            {
                unProducto = int.Parse(producto.Rows[0]["ID"].ToString());
                lblDescripcion.Text = producto.Rows[0]["Descripcion"].ToString();
                esfraccionado = bool.Parse(producto.Rows[0]["fraccionado"].ToString());
                if (!productoDeBalanzaEncontrado)
                {
                    if (tieneLectoraCB == 0)
                    {
                        nudCantidad.Focus();
                    }
                    else
                    {
                        nudCantidad.Value = 1;
                        btnAgregar_Click(null, null);
                    }

                }
                else
                {
                    var pesoProductoBalanza = Clases.ClassProductosBalanza.ExtraerPorPosicion(txtFiltro.Text.Trim(), posicionPeso);
                    if (pesoProductoBalanza == null) return;
                    var importeProductoCalculado = Math.Round((decimal.Parse(pesoProductoBalanza) / decimal.Parse(divisorPeso)) * decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec);
                    if (importeProductoCalculado == 0) return;
                    nudCantidad.Value = importeProductoCalculado;
                    btnAgregar_Click(null, null);
                }

            }
            else
            {
                MessageBox.Show(this, "No existe el producto n°: " + txtFiltro.Text.Trim(), "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lbDesc_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            prepararProducto();
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Clases.ClassValidacion.soloNumeros(e);
        }

        private void prepararProducto()
        {
            DataTable producto = instProd.traeProductosPpal(" where baja = 0 and descripcion = '" + lbDesc.SelectedItem.ToString() + "'");

            if (producto.Rows.Count > 0)
            {
                unProducto = int.Parse(producto.Rows[0]["ID"].ToString());
                lblDescripcion.Text = producto.Rows[0]["Descripcion"].ToString();
                nudCantidad.Focus();
                lbDesc.Visible = false;
                txtDesc.Text = string.Empty;
                unTipoBusq = 2;
                esfraccionado = bool.Parse(producto.Rows[0]["fraccionado"].ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (nudCantidad.Value > 0 & unProducto != 0)
            {
                agregarProducto();
                nudCantidad.Value = 0;
                unProducto = 0;
                if (unTipoBusq == 1)
                {
                    txtFiltro.Focus();
                }
                else { txtDesc.Focus(); }
            }
        }

        private void agregarProducto()
        {
            bool band = false;
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {

                if (int.Parse(fila.Cells["id"].Value.ToString()) == unProducto)
                {

                    bool esDolarizado = productosDolarizados == 1 && Convert.ToBoolean(fila.Cells["dolarizado"].Value);
                    if (!esfraccionado)
                    {
                        fila.Cells["cantidad"].Value = decimal.Parse(fila.Cells["cantidad"].Value.ToString()) + nudCantidad.Value;
                        band = true;
                        break;
                    }
                    else
                    {
                        //var cantidad = Math.Round(nudCantidad.Value / decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()), cantStock);
                        //fila.Cells["cantidad"].Value = decimal.Parse(fila.Cells["cantidad"].Value.ToString()) + cantidad;
                        //var precio = !esDolarizado ? decimal.Parse(fila.Cells["Subtotal"].Value.ToString()) + nudCantidad.Value : decimal.Parse(fila.Cells["Subtotal"].Value.ToString()) + Math.Round(nudCantidad.Value * valorDolar, cantDec);
                        //fila.Cells["precioSinIva"].Value = precio;
                        //fila.Cells["Subtotal"].Value = precio;
                        band = false;
                        break;
                    }
                    
                }

            }

            if (band == false)
            {
                DataTable producto = instProd.traerProductosParaEditar(unProducto);
                if (producto.Rows.Count > 0)
                {
                    bool esDolarizado = productosDolarizados == 1 && Convert.ToBoolean(producto.Rows[0]["dolarizado"]);
                    var costo = !esDolarizado ? Math.Round(decimal.Parse(producto.Rows[0]["costo"].ToString()), cantDec) : Math.Round(decimal.Parse(producto.Rows[0]["costo"].ToString()) * valorDolar, cantDec);
                    if (!esfraccionado)
                    {
                        var precio = !esDolarizado ? Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec) : Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()) * valorDolar, cantDec);
                        dgvProductos.Rows.Add(producto.Rows[0]["codBarras"].ToString(), producto.Rows[0]["codProveedor"].ToString(), producto.Rows[0]["descripcion"].ToString(), Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock), precio, precio, Math.Round(nudCantidad.Value, cantStock), 0, precio, unProducto, 0, costo, esfraccionado);
                    }
                    else
                    {
                        var cantidad = Math.Round(nudCantidad.Value / decimal.Parse(producto.Rows[0]["precio"].ToString()), cantStock);
                        var precio = !esDolarizado ? Math.Round(nudCantidad.Value, cantDec) : Math.Round(nudCantidad.Value * valorDolar, 2);
                        dgvProductos.Rows.Add(producto.Rows[0]["codBarras"].ToString(), producto.Rows[0]["codProveedor"].ToString(), producto.Rows[0]["descripcion"].ToString(), Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock), precio, precio, cantidad, precio, precio, unProducto, 0, costo, esfraccionado);
                    }
                }
            }

            dgvProductos.FirstDisplayedScrollingRowIndex = dgvProductos.RowCount - 1;

            procesoTotales();
        }

        private void nudCantidad_Enter(object sender, EventArgs e)
        {
            Clases.ClassValidacion.seleccionarTodoNumericUpDown(nudCantidad);
        }

        private void nudDescuento_Enter(object sender, EventArgs e)
        {
            Clases.ClassValidacion.seleccionarTodoNumericUpDown(nudDescuento);
        }

        private void nudRecargo_Enter(object sender, EventArgs e)
        {
            Clases.ClassValidacion.seleccionarTodoNumericUpDown(nudRecargo);
        }

        private void txtCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                if (lblCliente.Items.Count > -1)
                {
                    lblCliente.SetSelected(0, true);
                    lblCliente.Focus();
                }
            }
        }

        private void nudDescuento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }
        }

        private void nudRecargo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }
        }

        private void nudDescuento_ValueChanged(object sender, EventArgs e)
        {
            if (nudDescuento.Value != 0 | nudDescuento.Focused)
            {
                nudRecargo.Value = 0;
                procesoTotales();

            }
        }

        private void nudRecargo_ValueChanged(object sender, EventArgs e)
        {
            if (nudRecargo.Value != 0 | nudRecargo.Focused)
            {
                nudDescuento.Value = 0;
                procesoTotales();
            }
        }

        private void frmVentas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                btnGrabar_Click(null, null);
            }
        }

        private bool formularioValido()
        {
            errorProvider1.Clear();

            if (cboVendedores.SelectedIndex < 0)
            {
                errorProvider1.SetError(cboVendedores, "Seleccione un vendedor");
                return false;
            }
            if (nudComision.Value == -1)
            {
                errorProvider1.SetError(nudComision, "indique comision");
                return false;
            }

            if (tieneMediosPagos == 1 && cboMedioPago.SelectedIndex < 0)
            {
                errorProvider1.SetError(cboMedioPago, "Debe seleccionar medio de pago");
                return false;
            }
            return true;
        }


        private void btnGrabar_Click(object sender, EventArgs e)
        {


            if (formularioValido())
            {
                vender();
                if (buscoPend)
                {
                    btnPedido_Click(null, null);
                }
            }


        }

        private void imprimirVenta(long unaVenta)
        {
            Reportes.frmReport unFrmReport = new Reportes.frmReport();

            unFrmReport.nombreReporte = "ReportVenta.rdlc";
            List<string> var = new List<string>();
            var.Add(unaVenta.ToString());
            var.Add(Clases.ClassValidacion.traerEmpresa());
            var.Add("Tel: " + Clases.ClassValidacion.traerEmpresaTelefono());
            var.Add(Clases.ClassValidacion.traerEmpresaDireccion());
            var.Add(Clases.ClassValidacion.traerEmpresaCiudad());
            var.Add("CUIT: " + Clases.ClassValidacion.traerEmpresaCuit());
            var.Add(cboIVA.Text);
            var.Add(cantDec.ToString());
            var.Add(cantStock.ToString());
            var.Add(Clases.ClassValidacion.traerRazonSocial());
            unFrmReport.variable = var;
            unFrmReport.ShowDialog();
        }

        private void imprimirNotaVentaTk(long venta)
        {
            var ventaDT = instVentas.imprimirVentaTk(venta);

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

            var ticket = new TicketPrinter(items, ventaDT.Rows[0]["Vendedor"].ToString(), ventaDT.Rows[0]["nombreComercial"].ToString(), decimal.Parse(ventaDT.Rows[0]["totalVenta"].ToString()),
                                            DateTime.Parse(ventaDT.Rows[0]["fecha"].ToString()), ventaDT.Rows[0]["nroVenta"].ToString(), decimal.Parse(ventaDT.Rows[0]["descuento"].ToString())
                                            , decimal.Parse(ventaDT.Rows[0]["recargo"].ToString()), anchoTk == 80 ? 42 : 32); // 42 = 80mm
            ticket.Imprimir();
        }
        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
            if (txtDesc.Focused & txtDesc.Text != string.Empty)
            {
                filtrarCargarListBox();
            }
            else
            {
                lbDesc.Visible = false;
            }
        }

        private void filtrarCargarListBox()
        {
            lbDesc.Items.Clear();

            if (txtDesc.Text.Trim().Length == 0)
            {
                lbDesc.Visible = false;
            }

            var result = resgProducto.FindAll(l => l.ToUpper().Contains(txtDesc.Text.Trim().ToUpper()));

            lbDesc.Items.Clear();

            if (result.Count > 0)
            {
                result.ForEach(x => lbDesc.Items.Add(x));
                lbDesc.Visible = true;
            }
        }

        private void txtDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                if (lbDesc.Items.Count > -1)
                {
                    lbDesc.SetSelected(0, true);
                    lbDesc.Focus();
                }
            }
        }

        private void dgvProductos_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            decimal prec = 0;

            if (e != null)
            {
                if (e.Value != null)
                {
                    if (e.Value.ToString().IndexOf('.') != -1)
                    {
                        try
                        {
                            prec = Clases.ClassValidacion.cambiarPuntoPorComa(e.Value.ToString());
                            e.Value = prec;
                            e.ParsingApplied = true;
                        }
                        catch
                        {
                            e.ParsingApplied = false;
                        }
                    }
                }
            }
        }

        private void backgroundWorkerTarea_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void vender()
        {
            long salida = -1;
            int? planPagoId = null;
            decimal imputacion = 0;
            BindingList<Clases.ClassVentas.CobroFormasPago> dtFormasPAgo = new BindingList<Clases.ClassVentas.CobroFormasPago>();

            if (dgvProductos.RowCount > 0)
            {
                int progreso = 0;

                unCosto = 0;
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    unCosto += Math.Round(decimal.Parse(fila.Cells["costo"].Value.ToString()) * decimal.Parse(fila.Cells["Cantidad"].Value.ToString()), 4);

                }

                string detalleFormasPago = string.Empty;
                if (tieneMediosPagos == 1)
                {
                    planPagoId = instVentas.traerIdPlanPagoporNombre(cboMedioPago.Text);
                }
                else
                {
                    Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                    var planPagoDT = instConfig.traerPlanesPagoPorId(int.Parse(Clases.ClassParametros.buscarParametro("Cobros", "idPlanEfectivo")));
                    if (planPagoDT.Rows.Count == 0) return;
                    planPagoId = int.Parse(planPagoDT.Rows[0]["id"].ToString());
                }

                if (imputaEnVenta == 1)
                {
                    frmImputacionVenta unFrmImputacion = new frmImputacionVenta(decimal.Parse(txtTotGeneral.Text), planPagoId ?? 0);
                    unFrmImputacion.ShowDialog();
                    dtFormasPAgo = unFrmImputacion.unDT;

                    if (unFrmImputacion.DialogResult == DialogResult.OK)
                    {
                        imputacion = dtFormasPAgo.Sum(x => x.Importe);
                        if (tieneMediosPagos == 1)
                        {

                            foreach (Clases.ClassVentas.CobroFormasPago item in dtFormasPAgo)
                            {
                                detalleFormasPago += item.idMedio + "#";
                                detalleFormasPago += item.idPlan + "*";
                                detalleFormasPago += item.Importe + "!";
                                detalleFormasPago += item.Referencia1 + "?";
                                detalleFormasPago += item.Referencia2 + ";";
                                detalleFormasPago += item.Referencia3 + "¿";
                                detalleFormasPago = detalleFormasPago.Replace(',', '.');
                            }
                        }
                    }
                    else
                    {
                        return;
                    }

                }

                // salida = instVentas.grabarCabeceraVenta(decimal.Parse(txtTotGeneral.Text), unCosto, unCliente, int.Parse(Environment.GetEnvironmentVariable("idUser")), decimal.Parse(cboIVA.Text), nudDescuento.Value, nudRecargo.Value,int.Parse (cboVendedores .SelectedValue .ToString ()),nudComision.Value  /100, decimal.Parse(cboIngBrutos .Text));

                string detalle = string.Empty;
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    detalle += fila.Cells["id"].Value.ToString() + "#";
                    detalle += decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()) + "*";
                    detalle += decimal.Parse(fila.Cells["precioConIva"].Value.ToString()) + "!";
                    detalle += decimal.Parse(fila.Cells["Cantidad"].Value.ToString()) + "?";
                    detalle += fila.Cells["pedido"].Value.ToString() + ";";
                    detalle += fila.Cells["Subtotal"].Value.ToString() + "¿";
                    detalle += bool.Parse(fila.Cells["fraccionado"].Value.ToString()) == false ? "0" + "¡" : "1" + "¡";
                    detalle = detalle.Replace(',', '.');
                    progreso++;

                }

                salida = instVentas.grabarVenta(decimal.Parse(txtTotGeneral.Text), unCosto, unCliente, int.Parse(Environment.GetEnvironmentVariable("idUser")), decimal.Parse(cboIVA.Text), nudDescuento.Value, nudRecargo.Value, int.Parse(cboVendedores.SelectedValue.ToString()), nudComision.Value / 100, decimal.Parse(cboIngBrutos.Text), detalle, llevaCC, imputaEnVenta, tieneMediosPagos, imputacion, detalleFormasPago, tieneCaja, CajaId);

                if (salida != -1)
                {
                    if (cboTipo.SelectedIndex == 1)
                    {
                        //procesoFacturacion();
                        estadoInicial();
                    }
                    else
                    {
                        instPed.marcarPedido(pedidoCargado, 1);
                        if (facturaFiscal == 1)
                        {
                            if (puntoVenta == 0)
                            {
                                MessageBox.Show(this, "Debe configurar el punto de venta para este equipo", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                estadoInicial();
                            }
                            DataTable cliente = instClie.traeTodosDatos(unCliente);
                            if (cliente.Rows.Count == 0) return;
                            bool esConsumidorFinal = cliente.Rows[0]["abrev"].ToString() == "C" ? true : false;
                            string letra = cliente.Rows[0]["letra"].ToString();
                            
                                Fiscal unTk = new Fiscal();

                            ComprobanteFiscal status;
                            if (marcaFiscal.ToUpper() == "EPSON")
                            {
                                status = unTk.imprimirFacturaEpson(salida);
                            }
                            else
                            {
                                status = unTk.imprimirFacturaHasar(salida);
                            }
                            if (status == null) estadoInicial();

                                var salida_Fiscal = unTk.almacenarComprobanteFiscal(status);

                                if (salida_Fiscal == -1)
                                {
                                    MessageBox.Show(this, "Error al almacenar el comprobante fiscal", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    estadoInicial();
                                }

                            
                        }
                        else
                        {
                            if (haceNotaVentaTK == 0)
                            {
                                imprimirVenta(salida);
                            }
                            else
                            {
                                imprimirNotaVentaTk(salida);
                            }
                        }


                        estadoInicial();


                    }
                }

                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error en el proceso", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else
            {
                MessageBox.Show(this, "No existen productos en la grilla", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void backgroundWorkerTarea_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void btnAltaCliente_Click(object sender, EventArgs e)
        {
            frmAltaModifClientes unFrmAltaModifClientes = new frmAltaModifClientes();
            unFrmAltaModifClientes.unaAccion = 1;
            unFrmAltaModifClientes.idCliente = 0;
            unFrmAltaModifClientes.ShowDialog();
            if (unFrmAltaModifClientes.DialogResult == DialogResult.OK)
            {
                cargarDatosCliente(1, instClie.traerUltimoCliente());
            }
        }

        private void dgvProductos_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            procesoTotales();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            estadoInicial();
        }

        private void btnCambioPrecio_Click(object sender, EventArgs e)
        {
            if (dgvProductos.RowCount > 0)
            {
                decimal precio;
                valorDolar = Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar") == "" ? 0 : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar"));

                if (btnCambioPrecio.Text == "Precios Actualizados")
                {
                    foreach (DataGridViewRow fila in dgvProductos.Rows)
                    {
                        precio = instVentas.traerPrecioProductosVentas(1, int.Parse(fila.Cells["id"].Value.ToString()), unPedido);
                        if (precio > -1)
                        {
                            if (!Convert.ToBoolean(fila.Cells["fraccionado"].Value))
                            {
                                precio = !Convert.ToBoolean(fila.Cells["dolarizado"].Value) ? precio : precio * valorDolar;
                                fila.Cells["precioSinIva"].Value = Math.Round(precio, cantDec);
                            }
                        }

                    }

                    btnCambioPrecio.Text = "Precios del Pedido";
                }

                else if (btnCambioPrecio.Text == "Precios del Pedido")
                {
                    foreach (DataGridViewRow fila in dgvProductos.Rows)
                    {
                        precio = instVentas.traerPrecioProductosVentas(2, int.Parse(fila.Cells["id"].Value.ToString()), pedidoCargado);
                        if (precio > -1)
                        {
                            fila.Cells["precioSinIva"].Value = Math.Round(precio, cantDec);
                        }

                    }
                    btnCambioPrecio.Text = "Precios Actualizados";
                }

                procesoTotales();
            }
        }

        private void nudComision_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }
        }

        private void lblCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboIngBrutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            procesoTotales();
        }

        private void frmVentas_Shown(object sender, EventArgs e)
        {
            estadoInicial();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void cboMedioPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargado && tieneMediosPagos == 1)
            {
                nudRecargo.Value = decimal.Parse(cboMedioPago.SelectedValue.ToString());
            }
        }
    }
}
