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
    public partial class frmPedidos : Form
    {
        private List<string> resgClientes = new List<string>();
        private List<string> resgProducto = new List<string>();
        Clases.ClassClientes instClie = new Clases.ClassClientes();
        Clases.ClassProductos instProd = new Clases.ClassProductos();
        Clases.ClassProveedores instProv = new Clases.ClassProveedores();
        Clases.classUsuarios instUser = new Clases.classUsuarios();
        Clases.ClassPedidos instPed = new Clases.ClassPedidos();
        Clases .ClassColores instColor = new Clases.ClassColores ();

        int clientePed = 0;
        int unProducto = 0;
        bool cargado = false;
        int unTipoBusq = 1;
        public int unPedActual = 0;
        bool esfraccionado = false;
        int cantDec = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();
        int bonificacionPorLinea = Clases.ClassParametros.buscarParametro("ventas", "bonificacionesPorDetalle") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "bonificacionesPorDetalle"));
        // Descuento/recargo de cabecera al editar un pedido existente (null = usar por línea)
        decimal? _descuentoCabecera = null;
        decimal? _recargoCabecera = null;
        int productosDolarizados = Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos"));
        decimal valorDolar = Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar") == "" ? 0 : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar"));
        int tieneProductosBalanza = Clases.ClassParametros.buscarParametro("productos", "tieneProductosBalanza") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "tieneProductosBalanza"));
        string prefijoBalanza = Clases.ClassParametros.buscarParametro("productos", "prefijoBalanza");
        string posicionProductoBalanza = Clases.ClassParametros.buscarParametro("productos", "posicionProducto");
        string posicionPrecio = Clases.ClassParametros.buscarParametro("productos", "posicionPrecio");
        string divisorPrecio = Clases.ClassParametros.buscarParametro("productos", "divisorPrecio");
        int tieneLectoraCB = Clases.ClassParametros.buscarParametro("productos", "MecanismoLectora") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "MecanismoLectora"));
        int tieneConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "tieneConsumidorFinal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "tieneConsumidorFinal"));
        int clienteConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal"));
        // Subtotal forzado para productos fraccionados con formato "total*codBarras"
        decimal _subtotalForzado = 0;
        public frmPedidos()
        {
            InitializeComponent();
        }

       

        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
            if (txtCliente .Focused & txtCliente.Text != string.Empty)
            {
                filtrarCargarListBoxClie();
            }
            else
            {
                lblCliente .Visible = false;
            }
        }

        private void filtrarCargarListBoxClie()
        {
            lblCliente .Items.Clear();

            if (txtCliente .Text.Trim().Length == 0)
            {
                lblCliente .Visible = false;
            }

            var result = resgClientes .FindAll(l => l.ToUpper().Contains(txtCliente .Text.Trim().ToUpper()));

            lblCliente .Items.Clear();

            if (result.Count > 0)
            {
                result.ForEach(x => lblCliente .Items.Add(x));
                lblCliente .Visible = true;
            }
        }

        private void frmPedidos_Load(object sender, EventArgs e)
        {
            cargarClientes();
            cargarProductos();
            cargarCombos();
            estadoInicial();
            Control.CheckForIllegalCrossThreadCalls = false;
            backgroundWorkerTarea.RunWorkerCompleted += backgroundWorkerTarea_RunWorkerCompleted;
        }

        private void verificarParametros()
        {
            if (tieneConsumidorFinal == 1)
            {
                cargarDatosCliente(1, clienteConsumidorFinal);
                this.ActiveControl = txtFiltro;
            }
        }

        private void cargarCombos()
        {
            cboIVA.DataSource = instProv.traePorcentajeIVA();
            cboIVA.ValueMember = "id";
            cboIVA.DisplayMember = "valor";
            cboVendedores.DataSource  = instUser .traerTodosUsuarios ();
            cboVendedores.ValueMember = "id";
            cboVendedores.DisplayMember = "nombre";
            cboVendedores.SelectedValue = int.Parse(Environment.GetEnvironmentVariable("idUser"));
            
           cargado = true;
        }

        private void estadoInicial()
        {
            lblDescripcion.Text = string.Empty;
            lblClienteNombre.Text = string.Empty;
            dgvPedido.Rows.Clear();
            lbDesc.Visible = false;
            lblCliente.Visible = false;
            cboIVA.Enabled = false;
            gbFiltro.Enabled = false;
            dgvPedido.Enabled = false;
            nudDescuento.Enabled = false;
            nudRecargo.Enabled = false;
            btnGrabar.Enabled = false;
            cboFiltro.SelectedIndex = Clases.ClassParametros.indiceBusqNotaPed();
            txtCliente.Text = string.Empty;
            cboIVA.SelectedIndex = 0;
            txtSinIVA .Text = "0";
            txtTotalConIVA.Text = "0";
            txtDescuento.Text = "0";
            txtTotGeneral .Text = "0";
            nudDescuento.Value = 0;
            nudRecargo.Value = 0;
            clientePed  = 0;
            unPedActual = 0;
            unProducto = 0;
            rtbObserv.Text = string.Empty;
            btnAltaCliente.Enabled = true;
            txtCliente.Focus();
            lblDir.Text = string.Empty;
            lblTel.Text = string.Empty;
            lblEncargado.Text = string.Empty;
            nudCantidad.DecimalPlaces = cantStock;
            _descuentoCabecera = null;
            _recargoCabecera = null;
            dgvPedido.Columns["Sel"].Visible = bonificacionPorLinea == 1;
            panelSelGrilla.Visible = bonificacionPorLinea == 1;
            verificarParametros();
        }

        private void cargarProductos()
        {
            resgProducto .Clear();
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
            
            DataTable clientes = instClie.buscarAVender ();

            if (clientes.Rows.Count > 0)
            {
                foreach (DataRow fila in clientes.Rows)
                {
                    resgClientes .Add(fila["ID"].ToString() + "-  "+fila["Nombre_Comercial"].ToString() + "  -" + fila["Dir"].ToString());
                }
            }

        }

        private void lblCliente_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cargarDatosCliente(0,0);
        }

        private void lblCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter & lblCliente .Items.Count > 0)
            {
                cargarDatosCliente(0,0);

            }
        }

        public void cargarDatosCabecera(string unVendedor, string unaObserv, decimal? unDescuento, decimal? unRecargo, int unIVA)
        {
            cboVendedores.Text = unVendedor;
            rtbObserv.Text = unaObserv;
            // Si hay descuento/recargo global en la cabecera se aplica por línea al cargar el detalle
            _descuentoCabecera = (unDescuento.HasValue && unDescuento.Value != 0) ? unDescuento : null;
            _recargoCabecera   = (unRecargo.HasValue   && unRecargo.Value   != 0) ? unRecargo   : null;
            nudDescuento.Value = 0;
            nudRecargo.Value   = 0;
            cboIVA.SelectedValue = unIVA;
        }

        public void cargarDatosCliente(int unTipo, int unCliente)
        {
            DataTable cliente;
                if (unTipo == 0)
            {
               cliente  = instClie.traerDatosVenta (" and c.id = " + lblCliente.SelectedItem.ToString ().Substring (0,lblCliente .SelectedItem .ToString ().IndexOf ("-")));
            }
                else
            {
                cliente = instClie.traerDatosVenta (" and c.id = " + unCliente.ToString());
            }

            if (cliente.Rows.Count > 0)
            {
                clientePed  = int.Parse(cliente.Rows[0]["ID"].ToString());
                lblClienteNombre .Text = cliente.Rows[0]["Nombre_Comercial"].ToString();
                lblDir .Text = cliente.Rows[0]["Dir"].ToString() + ". " + cliente.Rows[0]["Localidad"].ToString() + ". " + cliente.Rows[0]["Provincia"].ToString();
                lblTel .Text = cliente.Rows[0]["Tel"].ToString();
                lblEncargado .Text = cliente.Rows[0]["contacto"].ToString();
                lbDesc.Visible = false;
                txtCliente .Text = string.Empty;
                estadoConCliente();
                txtFiltro.Focus();

            }
        }

        private void estadoConCliente()
        {
            lblDescripcion.Text = string.Empty;
            dgvPedido .Rows.Clear();
            lbDesc.Visible = false;
            cboIVA.Enabled = true;
            gbFiltro.Enabled = true;
            dgvPedido.Enabled = true;
            nudDescuento.Enabled = true;
            nudRecargo.Enabled = true;
            btnGrabar.Enabled = true;
            cboIVA.SelectedIndex = 0;
            btnAltaCliente.Enabled = false;
        }

        private void cboIVA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargado)
            {
                procesoTotalesyColorear();
            }
        }

        private void txtFiltro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter & txtFiltro .Text != string .Empty )
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

        private void dgvPedido_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            procesoTotalesyColorear();
            

        }



        private void dgvPedido_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            procesoTotalesyColorear();
        }

        private void procesoTotalesyColorear()
        {
            decimal totalSinIva  = 0;   // suma de (subtotalSIVA * cantidad)
            decimal totalConIVA  = 0;   // suma de Subtotal (c/IVA)
            decimal totalDescRec = 0;   // suma de (precioSinIVA - subtotalSIVA) * cantidad

            foreach (DataGridViewRow fila in dgvPedido.Rows)
            {
                bool subtotalManual = fila.Tag != null && fila.Tag.ToString() == "subtotalManual";

                decimal precioSinIva = decimal.Parse(fila.Cells["PrecioSinIVA"].Value.ToString());
                decimal cantidad     = decimal.Parse(fila.Cells["Cantidad"].Value.ToString());
                decimal iva          = decimal.Parse(cboIVA.Text);
                decimal descRec      = 0;
                decimal.TryParse(fila.Cells["DescRec"].Value?.ToString(), out descRec);

                // Subtotal S/IVA = Precio S/IVA * (1 + descRec/100)
                decimal subSinIVA = Math.Round(
                    precioSinIva * (1 + descRec / 100),
                    cantDec, MidpointRounding.AwayFromZero);
                fila.Cells["subtotalSIVA"].Value = subSinIVA;

                // Precio C/IVA = Subtotal S/IVA * (1 + iva/100)
                decimal precioConIva = Math.Round(
                    subSinIVA * (1 + iva / 100),
                    cantDec, MidpointRounding.AwayFromZero);
                fila.Cells["PrecioConIva"].Value = precioConIva;

                decimal subtotal;
                if (subtotalManual)
                {
                    // Producto fraccionado con total ingresado por el usuario (formato total*cb):
                    // se respeta el subtotal exacto en lugar de recalcularlo.
                    subtotal = decimal.Parse(fila.Cells["Subtotal"].Value.ToString());
                }
                else
                {
                    // Subtotal = Precio C/IVA * Cantidad
                    subtotal = Math.Round(
                        precioConIva * cantidad,
                        cantDec, MidpointRounding.AwayFromZero);
                    fila.Cells["Subtotal"].Value = subtotal;
                }

                totalSinIva  += Math.Round(subSinIVA * cantidad, cantDec, MidpointRounding.AwayFromZero);
                totalConIVA  += subtotal;
                totalDescRec += Math.Round((precioSinIva - subSinIVA) * cantidad, cantDec, MidpointRounding.AwayFromZero);
            }

            txtTotalConIVA.Text = Math.Round(totalConIVA,  2).ToString();
            txtSinIVA.Text      = Math.Round(totalSinIva,  cantDec).ToString();
            txtDescuento.Text   = Math.Round(totalDescRec, cantDec).ToString();
            txtTotGeneral.Text  = Math.Round(totalConIVA,  cantDec).ToString();
        }

        private void buscarProducto()
        {
            DataTable producto = null;
            bool productoDeBalanzaEncontrado = false;

            // Cantidad especificada con el formato "cantidad*codigoBarras" (solo en búsqueda por CB)
            decimal cantidadForzada = 0;

            if (txtFiltro.Text != string.Empty)
            {
                if (cboFiltro.SelectedIndex == 0)
                {
                    producto = instProd.traeProductosPpal(" where baja = 0 and codProveedor = '" + txtFiltro.Text.Trim()+"'");
                }
                else if (cboFiltro.SelectedIndex == 1)
                {
                    // Formato cantidad*codBarras
                    string textoBusqueda = txtFiltro.Text.Trim();
                    if (textoBusqueda.Contains("*"))
                    {
                        int pos = textoBusqueda.IndexOf('*');
                        string parteQty = textoBusqueda.Substring(0, pos).Trim();
                        textoBusqueda  = textoBusqueda.Substring(pos + 1).Trim();
                        decimal qty;
                        cantidadForzada = (decimal.TryParse(parteQty, out qty) && qty > 0) ? qty : 1;
                    }

                    if (tieneProductosBalanza == 0)
                    {
                        producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + textoBusqueda);
                    }
                    else
                    {
                        if (prefijoBalanza == string.Empty || posicionProductoBalanza == string.Empty || posicionPrecio == string.Empty || divisorPrecio == string.Empty)
                        {
                            MessageBox.Show(this, "Para operar con productos de balanza debe parametrizar: Prefijo, Posición de Producto, Posicion de Importe y divisior de importe", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (Clases.ClassProductosBalanza.esCodigoBalanza(textoBusqueda, prefijoBalanza))
                        {
                            var productoBalanzaId = Clases.ClassProductosBalanza.ExtraerPorPosicion(textoBusqueda, posicionProductoBalanza);

                            if (productoBalanzaId == null) return;

                            producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + productoBalanzaId.Trim());
                            if (producto.Rows.Count > 0)
                            {
                                productoDeBalanzaEncontrado = true;

                                // Extraer el TOTAL del código de barras de balanza
                                // (precio del paquete pesado, ya pre-calculado por la balanza).
                                // Se divide por divisorPrecio para obtener los decimales.
                                var precioStr = Clases.ClassProductosBalanza.ExtraerPorPosicion(textoBusqueda, posicionPrecio);
                                if (!string.IsNullOrEmpty(precioStr))
                                {
                                    decimal precioBalanza;
                                    if (decimal.TryParse(precioStr, System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture, out precioBalanza))
                                    {
                                        decimal divisor;
                                        if (decimal.TryParse(divisorPrecio, System.Globalization.NumberStyles.Any,
                                                System.Globalization.CultureInfo.InvariantCulture, out divisor) && divisor > 0)
                                            precioBalanza = precioBalanza / divisor;

                                        if (precioBalanza > 0)
                                            cantidadForzada = precioBalanza;
                                    }
                                }
                            }
                        }
                        else
                        {
                            producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + textoBusqueda);
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

                // Subtotal forzado:
                //   - Producto fraccionado con formato "total*codBarras" (usuario tipea el total)
                //   - Producto leído por código de balanza (el total viene en el propio CB)
                // En ambos casos cantidadForzada contiene el TOTAL; la qty la calcula agregarProducto().
                if (cantidadForzada > 0 && (esfraccionado || productoDeBalanzaEncontrado))
                {
                    _subtotalForzado = cantidadForzada;
                    cantidadForzada  = 0;
                }

                if (tieneLectoraCB == 1)
                {
                    // Lectora de código de barras: agregar directamente (cantidad forzada o 1)
                    nudCantidad.Value = cantidadForzada > 0 ? cantidadForzada : 1;
                    agregarProducto();
                    nudCantidad.Value = 0;
                    unProducto = 0;
                    txtFiltro.Focus();
                }
                else
                {
                    if (cantidadForzada > 0)
                        nudCantidad.Value = cantidadForzada;
                    nudCantidad.Focus();
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

                dgvPedido.CurrentCell = dgvPedido.Rows[dgvPedido.RowCount - 1].Cells["Observ"];
                dgvPedido.BeginEdit(true);

                
            }
        }

        private void agregarProducto()
        {
            // Si es lectora de CB: buscar si el producto ya existe en la grilla y acumular
            if (tieneLectoraCB == 1)
            {
                foreach (DataGridViewRow fila in dgvPedido.Rows)
                {
                    if (int.Parse(fila.Cells["id"].Value.ToString()) == unProducto)
                    {
                        if (_subtotalForzado > 0)
                        {
                            // Fraccionado con total ingresado: acumular subtotal exacto
                            // y recalcular cantidad equivalente. Marcar fila como manual
                            // ANTES de tocar las celdas para que procesoTotalesyColorear
                            // no recalcule el Subtotal.
                            decimal precioConIvaFila = decimal.Parse(fila.Cells["PrecioConIva"].Value.ToString());
                            decimal subtotalActual   = decimal.Parse(fila.Cells["Subtotal"].Value.ToString());
                            decimal nuevoSubtotal    = subtotalActual + _subtotalForzado;
                            decimal nuevaCantidad    = precioConIvaFila > 0
                                ? Math.Round(nuevoSubtotal / precioConIvaFila, cantStock)
                                : 0;

                            fila.Tag = "subtotalManual";
                            fila.Cells["Cantidad"].Value = nuevaCantidad;
                            fila.Cells["Subtotal"].Value = nuevoSubtotal;
                            _subtotalForzado = 0;
                        }
                        else
                        {
                            decimal cantActual = decimal.Parse(fila.Cells["Cantidad"].Value.ToString());
                            fila.Cells["Cantidad"].Value = Math.Round(cantActual + nudCantidad.Value, cantStock);
                        }

                        dgvPedido.FirstDisplayedScrollingRowIndex = fila.Index;
                        procesoTotalesyColorear();
                        return;
                    }
                }
            }

            DataTable producto = instProd.traerProductosParaEditar(unProducto);
            if (producto.Rows.Count > 0)
            {
                bool esDolarizado = productosDolarizados == 1 && Convert.ToBoolean(producto.Rows[0]["dolarizado"]);
                decimal precio = esDolarizado
                    ? Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()) * valorDolar, cantDec)
                    : Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec);
                decimal costo = esDolarizado
                    ? Math.Round(decimal.Parse(producto.Rows[0]["costo"].ToString()) * valorDolar, cantDec)
                    : Math.Round(decimal.Parse(producto.Rows[0]["costo"].ToString()), cantDec);

                // Calcular precioConIva para poder derivar la cantidad si es fraccionado con total
                decimal iva = decimal.TryParse(cboIVA.Text, out decimal ivaVal) ? ivaVal : 0;
                decimal precioConIva = Math.Round(precio * (1 + iva / 100), cantDec);

                decimal cantidad;
                if (_subtotalForzado > 0)
                {
                    cantidad = precioConIva > 0
                        ? Math.Round(_subtotalForzado / precioConIva, cantStock)
                        : 1;
                }
                else
                {
                    cantidad = Math.Round(nudCantidad.Value, cantStock);
                }

                // Sel | CodBarras | CodProv | Desc | Observ | Stock | PrecioSinIVA | DescRec | subtotalSIVA | PrecioConIva | Cantidad | Subtotal | PrecioOrig | id | costo | fraccionado
                int nuevoIdx = dgvPedido.Rows.Add(
                    false,
                    producto.Rows[0]["codBarras"].ToString(),
                    producto.Rows[0]["codProveedor"].ToString(),
                    producto.Rows[0]["descripcion"].ToString(),
                    " ",
                    Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock),
                    precio,
                    0,
                    precio,
                    precioConIva,
                    cantidad,
                    0,
                    precio,
                    unProducto,
                    costo,
                    Convert.ToBoolean(producto.Rows[0]["fraccionado"]));

                if (_subtotalForzado > 0)
                {
                    // Marcar como manual ANTES de imponer el subtotal: así el recálculo
                    // gatillado por CellValueChanged respeta el valor exacto.
                    dgvPedido.Rows[nuevoIdx].Tag = "subtotalManual";
                    dgvPedido.Rows[nuevoIdx].Cells["Subtotal"].Value = _subtotalForzado;
                    _subtotalForzado = 0;
                }
            }

            dgvPedido.FirstDisplayedScrollingRowIndex = dgvPedido.RowCount - 1;
            procesoTotalesyColorear();
        }

        public void cargarDetallePedidoEditar(int unPedido)
        {
            DataTable detalle = instPed.traerDetalleParaEditar(unPedido.ToString());

            if (detalle.Rows.Count > 0)
            {
                dgvPedido.Rows.Clear();

                foreach (DataRow fila in detalle.Rows)
                {
                    decimal precioSinIva = Math.Round(decimal.Parse(fila["precioSinIva"].ToString()), cantDec);

                    // Determinar DescRec: si hay descuento global en cabecera lo aplica a todas las líneas;
                    // si no, lee descuento/recargo propio de la línea (campos nuevos en PedidoDetalle)
                    decimal descRec = 0;
                    if (_descuentoCabecera.HasValue || _recargoCabecera.HasValue)
                    {
                        decimal desc = _descuentoCabecera ?? 0;
                        decimal rec  = _recargoCabecera   ?? 0;
                        descRec = (desc * -1) + rec;
                    }
                    else if (detalle.Columns.Contains("descuento") && detalle.Columns.Contains("recargo"))
                    {
                        decimal lineDesc = 0, lineRec = 0;
                        if (fila["descuento"] != DBNull.Value) decimal.TryParse(fila["descuento"].ToString(), out lineDesc);
                        if (fila["recargo"]   != DBNull.Value) decimal.TryParse(fila["recargo"].ToString(),   out lineRec);
                        descRec = (lineDesc * -1) + lineRec;
                    }

                    decimal subSinIVA = Math.Round(precioSinIva * (1 + descRec / 100), cantDec, MidpointRounding.AwayFromZero);

                    // Sel | CodBarras | CodProv | Desc | Observ | Stock | PrecioSinIVA | DescRec | subtotalSIVA | PrecioConIva | Cantidad | Subtotal | PrecioOrig | id | costo | fraccionado
                    dgvPedido.Rows.Add(
                        false,
                        fila["codBarras"].ToString(),
                        fila["codProveedor"].ToString(),
                        fila["descripcion"].ToString(),
                        fila["observ"].ToString(),
                        Math.Round(decimal.Parse(fila["stock"].ToString()), cantStock),
                        precioSinIva,
                        descRec,
                        subSinIVA,
                        Math.Round(decimal.Parse(fila["precioConIva"].ToString()), cantDec),
                        Math.Round(decimal.Parse(fila["cantidad"].ToString()), cantStock),
                        0,
                        Math.Round(decimal.Parse(fila["precioOrig"].ToString()), cantDec),
                        fila["fk_producto"].ToString(),
                        Math.Round(decimal.Parse(fila["costo"].ToString()), cantDec),
                        Convert.ToBoolean(fila["fraccionado"]));

                    // Para productos fraccionados, el subtotal exacto guardado en la BD
                    // debe preservarse tal cual; no debe recalcularse por procesoTotalesyColorear.
                    if (Convert.ToBoolean(fila["fraccionado"]) && fila["subtotal"] != DBNull.Value)
                    {
                        int idx = dgvPedido.RowCount - 1;
                        dgvPedido.Rows[idx].Tag = "subtotalManual";
                        dgvPedido.Rows[idx].Cells["Subtotal"].Value =
                            Math.Round(decimal.Parse(fila["subtotal"].ToString()), cantDec);
                    }
                }

                dgvPedido.FirstDisplayedScrollingRowIndex = dgvPedido.RowCount - 1;
                procesoTotalesyColorear();
            }
        }

        private void nudCantidad_Enter(object sender, EventArgs e)
        {
            Clases.ClassValidacion.seleccionarTodoNumericUpDown(nudCantidad);
        }

        private void nudDescuento_Enter(object sender, EventArgs e)
        {
            Clases.ClassValidacion.seleccionarTodoNumericUpDown(nudDescuento );
        }

        private void nudRecargo_Enter(object sender, EventArgs e)
        {
            Clases.ClassValidacion.seleccionarTodoNumericUpDown(nudRecargo);
        }

        private void txtCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                if (lblCliente .Items.Count > -1)
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

        private void aplicarDescuentoPorLinea()
        {
            nudRecargo.Value = 0;
            foreach (DataGridViewRow fila in dgvPedido.Rows)
            {
                if ((bool)(fila.Cells["Sel"].Value ?? false) || bonificacionPorLinea == 0)
                {
                    fila.Cells["DescRec"].Value = nudDescuento.Value * -1;
                    fila.Cells["Sel"].Value = false;
                }
            }
            procesoTotalesyColorear();
        }

        private void aplicarRecargoPorLinea()
        {
            nudDescuento.Value = 0;
            foreach (DataGridViewRow fila in dgvPedido.Rows)
            {
                if ((bool)(fila.Cells["Sel"].Value ?? false) || bonificacionPorLinea == 0)
                {
                    fila.Cells["DescRec"].Value = nudRecargo.Value;
                    fila.Cells["Sel"].Value = false;
                }
            }
            procesoTotalesyColorear();
        }

        private void nudDescuento_ValueChanged(object sender, EventArgs e)
        {
            if (nudDescuento.Value != 0 || nudDescuento.Focused)
                aplicarDescuentoPorLinea();
        }

        private void nudRecargo_ValueChanged(object sender, EventArgs e)
        {
            if (nudRecargo.Value != 0 || nudRecargo.Focused)
                aplicarRecargoPorLinea();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            backgroundWorkerTarea.RunWorkerAsync();

           
        }

        private void frmPedidos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData== Keys.F5 )
            {
                btnGrabar_Click(null, null);
            }
        }

        private void txtDesc_TextChanged_1(object sender, EventArgs e)
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
            try
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
            catch { }
        }

        private void nudDescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                aplicarDescuentoPorLinea();
        }

        private void nudRecargo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                aplicarRecargoPorLinea();
        }

        private void btnSelTodos_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow fila in dgvPedido.Rows)
                fila.Cells["Sel"].Value = true;
        }

        private void btnSelNinguno_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow fila in dgvPedido.Rows)
                fila.Cells["Sel"].Value = false;
        }

        private void dgvPedido_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
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
            if (dgvPedido.RowCount > 0)
            {
                int progreso = 0;
                pbProceso.Maximum = dgvPedido.RowCount;
                Clases.ClassPedidos instPedidos = new Clases.ClassPedidos();
                int salida = 0;

                // Para pedidos nuevos: null en cabecera indica que los descuentos están por línea
                salida = instPedidos.pedidosAddCabecera(decimal.Parse(txtTotGeneral.Text), clientePed, decimal.Parse(cboIVA.Text), null, null, int.Parse(cboVendedores.SelectedValue.ToString()), rtbObserv.Text.Trim());

                if (salida != -1)
                {
                    foreach (DataGridViewRow fila in dgvPedido.Rows)
                    {
                        decimal descRecVal = 0;
                        decimal.TryParse(fila.Cells["DescRec"].Value?.ToString(), out descRecVal);
                        decimal descuento = descRecVal < 0 ? Math.Abs(descRecVal) : 0;
                        decimal recargo   = descRecVal > 0 ? descRecVal : 0;
                        decimal subSinIva = 0;
                        decimal.TryParse(fila.Cells["subtotalSIVA"].Value?.ToString(), out subSinIva);

                        salida = instPedidos.pedidosAddDetalle(salida, int.Parse(fila.Cells["id"].Value.ToString()), fila.Cells["Cod_Barras"].Value.ToString(), fila.Cells["Cod_Proveedor"].Value.ToString(), fila.Cells["Descripcion"].Value.ToString(), decimal.Parse(fila.Cells["PrecioSinIVA"].Value.ToString()), decimal.Parse(fila.Cells["PrecioConIva"].Value.ToString()), decimal.Parse(fila.Cells["Cantidad"].Value.ToString()), decimal.Parse(fila.Cells["Subtotal"].Value.ToString()), decimal.Parse(fila.Cells["PrecioOrig"].Value.ToString()), decimal.Parse(fila.Cells["costo"].Value.ToString()), fila.Cells["Observ"].Value.ToString(), descuento, recargo, subSinIva);

                        if (salida == -1)
                        {
                            MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        

                        progreso++;
                        backgroundWorkerTarea.ReportProgress(progreso, DateTime.Now);
                    }

                    salida = instPed.elimnarPedido(unPedActual);

                    MessageBox.Show(this, "Pedido Cargado con éxito!!", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pbProceso.Value = 0;
                    estadoInicial();
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error durante el procesamiento", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void backgroundWorkerTarea_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProceso.Value = (e.ProgressPercentage);
        }

        // Corre en el hilo de UI: es el momento seguro para establecer el foco
        // después de que el worker termina (estadoInicial se llama desde el hilo worker,
        // donde Focus() no tiene efecto confiable en WinForms).
        private void backgroundWorkerTarea_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tieneConsumidorFinal == 1)
                txtFiltro.Focus();
            else
                txtCliente.Focus();
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            frmPedidosParaEditar unFrmPedidosEditar = new frmPedidosParaEditar(this);
            unFrmPedidosEditar.llamador = this;
            unFrmPedidosEditar.ShowDialog();
        }

        private void dgvPedido_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPedido.IsCurrentCellDirty)
            {
                dgvPedido.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPedido_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void dgvPedido_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPedido.Rows[dgvPedido.RowCount - 1].Cells["Observ"].Selected == true)
            {
                if (unTipoBusq == 1)
                {
                    txtFiltro.Focus();
                }
                else { txtDesc.Focus(); }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            estadoInicial();
        }
    }
}
