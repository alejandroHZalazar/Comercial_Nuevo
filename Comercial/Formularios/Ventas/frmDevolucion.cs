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
    public partial class frmDevolucion : Form
    {
        public int unCliente;
        int unProducto = 0;
        bool cargado = false;
        int unTipoBusq = 1;
        decimal unCosto = 0;
        bool esfraccionado = false;

        private List<string> resgClientes = new List<string>();
        private List<string> resgProducto = new List<string>();
        Clases.ClassClientes instClie = new Clases.ClassClientes();

        Clases.ClassProductos instProd = new Clases.ClassProductos();
        Clases.ClassProveedores instProv = new Clases.ClassProveedores();
        Clases.ClassVentas instVentas = new Clases.ClassVentas();
        Clases.classUsuarios instUser = new Clases.classUsuarios();
        int cantDec = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();
        int tieneProductosBalanza = Clases.ClassParametros.buscarParametro("productos", "tieneProductosBalanza") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "tieneProductosBalanza"));
        string prefijoBalanza = Clases.ClassParametros.buscarParametro("productos", "prefijoBalanza");
        string posicionProductoBalanza = Clases.ClassParametros.buscarParametro("productos", "posicionProducto");
        string posicionPeso = Clases.ClassParametros.buscarParametro("productos", "posicionPeso");
        string divisorPeso = Clases.ClassParametros.buscarParametro("productos", "divisorPeso");
        int tieneConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "tieneConsumidorFinal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "tieneConsumidorFinal"));
        int clienteConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal"));
        int comisiona = Clases.ClassParametros.buscarParametro("ventas", "comisiona") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "comisiona"));
        decimal valorDolar = Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar") == "" ? 0 : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar"));
        int productosDolarizados = Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos"));
        int tieneLectoraCB = Clases.ClassParametros.buscarParametro("productos", "MecanismoLectora") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "MecanismoLectora"));
        public frmDevolucion()
        {
            InitializeComponent();
        }

        private void frmDevolucion_Load(object sender, EventArgs e)
        {
            cargarClientes();
            cargarProductos();
            cargarCombos();
            estadoInicial();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        public void cargarVenta(int unaVenta, decimal unIva, decimal unDescuento, decimal unRecargo, int unVendedor, decimal unaComision)
        {
            
            
            DataTable cliente = instClie.traeClientesPpal(" and c.id = " + unCliente);
            if (cliente.Rows.Count > 0)
            {
                lblClienteNombre.Text = cliente.Rows[0]["Nombre_Comercial"].ToString();
                estadoConCliente();
            }
            DataTable pedido = instVentas.traerDetalleVentaADevolver (unaVenta );
            dgvProductos.Rows.Clear();
            if (pedido.Rows.Count > 0)
            {
                
                foreach (DataRow fila in pedido.Rows)
                {
                    var precio = !Convert.ToBoolean(fila["fraccionado"]) ? 0 : Math.Round(decimal.Parse(fila["Precio S/IVA"].ToString()), cantDec);
                    dgvProductos.Rows.Add(fila["Cod_Barras"].ToString(), fila["Cod_Proveedor"].ToString(), fila["Descripcion"].ToString(), Math.Round(decimal.Parse(fila["Stock"].ToString()), cantStock), Math.Round(decimal.Parse(fila["Precio S/IVA"].ToString()), cantDec), Math.Round(decimal.Parse(fila["Precio C/IVA"].ToString()), cantDec), Math.Round(decimal.Parse(fila["Cantidad"].ToString()), cantStock), Math.Round(decimal.Parse(fila["Subtotal"].ToString()), cantDec), Math.Round(decimal.Parse(fila["Precio S/IVA"].ToString()), cantDec), fila["fk_producto"].ToString(), 0, Math.Round(decimal.Parse(fila["costo"].ToString()), cantDec), Convert.ToBoolean(fila["fraccionado"]), Convert.ToBoolean(fila["dolarizado"]));
                }
            }
            
            nudDescuento.Value = unDescuento;
            nudRecargo.Value = unRecargo;
            cboIVA.Text = unIva.ToString();
            cboVendedores.SelectedValue = unVendedor;
            nudComision.Value = unaComision;
            procesoTotales();
           
        }

        private void estadoInicial()
        {
            lblDescripcion.Text = string.Empty;
            lblClienteNombre.Text = string.Empty;
            dgvProductos.Rows.Clear();
            lbDesc.Visible = false;
            lblCliente.Visible = false;
            cboIVA.Enabled = false;
            gbFiltro.Enabled = false;
            dgvProductos.Enabled = false;
            nudDescuento.Enabled = false;
            nudRecargo.Enabled = false;
            btnGrabar.Enabled = false;
            cboFiltro.SelectedIndex = Clases.ClassParametros.indiceBusqNotaPed();
            txtCliente.Text = string.Empty;
            cboIVA.SelectedIndex = 0;
            txtTotalSinIva.Text = "0";
            nudDescuento.Value = 0;
            nudRecargo.Value = 0;
            unCliente = 0;
            unProducto = 0;
            txtSubSinIVA.Text = "0";
            txtCliente.Focus();
            nudComision.Value = -1;
            lblDir.Text = string.Empty;
            lblTel.Text = string.Empty;
            lblEncargado.Text = string.Empty;
            nudCantidad.DecimalPlaces = cantStock;
             cboVendedores.SelectedIndex = -1;
            txtDescuento.Text = "0";
            txtTotGeneral.Text = "0";
            verificarParametros();
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
                   
                    cargarDatosCliente(1, clienteConsumidorFinal);
                    txtFiltro.Focus();
                }
            }
            else
            {
                txtCliente.Focus();
            }           


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
                this.unCliente = int.Parse(cliente.Rows[0]["ID"].ToString());
                lblClienteNombre.Text = cliente.Rows[0]["Nombre_Comercial"].ToString();
                lblDir.Text = cliente.Rows[0]["Dir"].ToString() + ". " + cliente.Rows[0]["Localidad"].ToString() + ". " + cliente.Rows[0]["Provincia"].ToString();
                lblTel.Text = cliente.Rows[0]["Tel"].ToString();
                lblEncargado.Text = cliente.Rows[0]["contacto"].ToString();
                lbDesc.Visible = false;
                txtCliente.Text = string.Empty;
                estadoConCliente();
                txtFiltro.Focus();

            }
        }

        private void estadoConCliente()
        {
            lblDescripcion.Text = string.Empty;
            lbDesc.Visible = false;
            cboIVA.Enabled = true;
            gbFiltro.Enabled = true;
            dgvProductos.Enabled = true;
            nudDescuento.Enabled = true;
            nudRecargo.Enabled = true;
            btnGrabar.Enabled = true;
            cboIVA.SelectedIndex = 0;
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
            decimal totalCIVA = 0;
            decimal totalSIVA = 0;
            decimal totalDescuento = 0;

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                fila.Cells["precioConIva"].Value = Math.Round(decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()) * (1 + Decimal.Parse(cboIVA.Text) / 100), cantDec);
                fila.Cells["Subtotal"].Value = !bool.Parse(fila.Cells["fraccionado"].Value.ToString()) ? Math.Round(decimal.Parse(fila.Cells["precioConIva"].Value.ToString()) * decimal.Parse(fila.Cells["Cantidad"].Value.ToString()), cantDec) : Math.Round(decimal.Parse(fila.Cells["Subtotal"].Value.ToString()), cantDec);
                totalCIVA += decimal.Parse(fila.Cells["Subtotal"].Value.ToString());
                totalSIVA += !bool.Parse(fila.Cells["fraccionado"].Value.ToString()) ? Math.Round(decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()) * decimal.Parse(fila.Cells["Cantidad"].Value.ToString()), cantDec) : Math.Round(decimal.Parse(fila.Cells["Subtotal"].Value.ToString()), cantDec);

            }

            if (dgvProductos.RowCount > 0)
            {
                txtSubSinIVA.Text = totalSIVA.ToString();
                totalDescuento = Math.Round(totalSIVA * (nudDescuento.Value / 100), cantDec);
                txtTotalSinIva.Text = Math.Round((totalSIVA - totalDescuento), cantDec).ToString();


                txtDescuento.Text = Math.Round(totalDescuento, cantDec).ToString();
                txtTotGeneral.Text = (Math.Round((totalSIVA - totalDescuento) * (1 + Decimal.Parse(cboIVA.Text) / 100), cantDec)).ToString();
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
                    producto = instProd.traeProductosPpal(" where baja = 0 and codProveedor = '" + txtFiltro.Text.Trim()+"'");
                }
                else if (cboFiltro.SelectedIndex == 1)
                {
                    if(tieneProductosBalanza == 0)
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
                        dgvProductos.Rows.Add(producto.Rows[0]["codBarras"].ToString(), producto.Rows[0]["codProveedor"].ToString(), producto.Rows[0]["descripcion"].ToString(), Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock), precio, precio, Math.Round(nudCantidad.Value, cantStock), 0, precio, unProducto, 0, costo, esfraccionado, Convert.ToBoolean(producto.Rows[0]["dolarizado"]));
                    }
                    else
                    {
                        var cantidad = Math.Round(nudCantidad.Value / decimal.Parse(producto.Rows[0]["precio"].ToString()), cantStock);
                        var precio = !esDolarizado ? Math.Round(nudCantidad.Value, cantDec) : Math.Round(nudCantidad.Value * valorDolar, 2);
                        dgvProductos.Rows.Add(producto.Rows[0]["codBarras"].ToString(), producto.Rows[0]["codProveedor"].ToString(), producto.Rows[0]["descripcion"].ToString(), Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock), Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec), Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec), cantidad, precio, Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec), unProducto, 0, costo, esfraccionado, Convert.ToBoolean(producto.Rows[0]["dolarizado"]));
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

                procesoTotales();

                nudRecargo.Value = 0;
            }
        }

        private void nudRecargo_ValueChanged(object sender, EventArgs e)
        {
            if (nudRecargo.Value != 0 | nudRecargo.Focused)
            {


                procesoTotales();

                nudDescuento.Value = 0;
            }
        }

        private void frmDevolucion_KeyDown(object sender, KeyEventArgs e)
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
            return true;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (formularioValido ())
            {
                devolver();
            }
          
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

        private void devolver()
        {
            long salida = -1;
            if (dgvProductos.RowCount > 0)
            {
                int progreso = 0;
                
                unCosto = 0;
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    unCosto += Math.Round(decimal.Parse(fila.Cells["costo"].Value.ToString()) * decimal.Parse(fila.Cells["Cantidad"].Value.ToString()), 4);

                }
                salida = instVentas.grabarCabeceraDevolucion (decimal.Parse(txtTotGeneral.Text), unCosto, unCliente, int.Parse(Environment.GetEnvironmentVariable("idUser")), decimal.Parse(cboIVA.Text), nudDescuento.Value, nudRecargo.Value, int.Parse(cboVendedores.SelectedValue.ToString()),nudComision .Value /100);

                if (salida != -1)
                {
                    string detalle = string.Empty;
                    int reglones = 0;
                    foreach (DataGridViewRow fila in dgvProductos.Rows)
                    {
                        detalle += fila.Cells["id"].Value.ToString() + "#";
                        detalle += decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()) + "*";
                        detalle += decimal.Parse(fila.Cells["precioConIva"].Value.ToString()) + "!";
                        detalle += decimal.Parse(fila.Cells["Cantidad"].Value.ToString()) + "?";
                        detalle = detalle.Replace(',', '.');
                        reglones++;
                        if (reglones == 10)
                        {
                            salida = instVentas.grabarProcesoDetalleDevolucion(salida, detalle);
                            if (salida != -1)
                            {
                                detalle = string.Empty;
                                reglones = 0;
                            }
                            else
                            {
                                MessageBox.Show(this, "Ha ocurrido un error en el proceso", "DEVOLUCION", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }

                        }
                        progreso++;

                    }

                    if (reglones != 0)
                    {
                        salida = instVentas.grabarProcesoDetalleDevolucion(salida, detalle);
                        if (salida != -1)
                        {
                            detalle = string.Empty;
                            reglones = 0;
                        }
                        else
                        {
                            MessageBox.Show(this, "Ha ocurrido un error en el proceso", "DEVOLUCION", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }

                        //salida = instVentas.grabarProcesoDetalleDevolucion (salida, detalle);

                        //if (salida != -1)
                        //{
                        imprimirDevolucion(salida);
                      estadoInicial();
                    // ;
                        
                    //}
                    //else
                    //{
                    //    MessageBox.Show(this, "Ha ocurrido un error en el proceso", "DEVOLUCION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                       
                    //}

                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error en el proceso", "DEVOLUCION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
            }
        }

        private void imprimirDevolucion(long unaDev)
        {
            Reportes.frmReport unFrmReport = new Reportes.frmReport();

            unFrmReport.nombreReporte = "ReportDevolucion.rdlc";
            List<string> var = new List<string>();
            var.Add(unaDev.ToString());
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

        private void dgvProductos_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            procesoTotales();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            estadoInicial();
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            frmVentasADevolver unFrmVentasDevolver = new frmVentasADevolver(this);
            unFrmVentasDevolver.llamador = this;
            unFrmVentasDevolver.ShowDialog();
        }

        private void nudComision_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }
        }
    }
}
