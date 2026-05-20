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
    public partial class frmDevolucion : Form
    {
        public int unCliente;
        int unProducto = 0;
        bool cargado = false;
        int unTipoBusq = 1;
        decimal unCosto = 0;
        bool esfraccionado = false;

        private List<string> resgClientes = new List<string>();
        private List<ProductoBusqueda> resgProducto = new List<ProductoBusqueda>();
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
        int filtraPorProveedor = Clases.ClassParametros.buscarParametro("ventas", "filtraPorProveedor") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "filtraPorProveedor"));
        int llevaCC = Clases.ClassParametros.buscarParametro("clientes", "llevaCC") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("clientes", "llevaCC"));
        int facturaElectronica = Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica"));
        int puntoVenta = Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName) == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName));
        int bonificacionPorLinea = Clases.ClassParametros.buscarParametro("ventas", "bonificacionesPorDetalle") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "bonificacionesPorDetalle"));
        int tieneCaja = Clases.ClassParametros.buscarParametro("caja", "haceCaja") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("caja", "haceCaja"));
        int CajaId = 0;
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

        public void cargarVenta(int unaVenta, decimal unIva, decimal unDescuento, decimal unRecargo, int unVendedor, decimal unaComision, decimal iibb)
        {


            DataTable cliente = instClie.traeClientesPpal(" and c.id = " + unCliente);
            if (cliente.Rows.Count > 0)
            {
                lblClienteNombre.Text = cliente.Rows[0]["Nombre_Comercial"].ToString();
                estadoConCliente();
            }
            DataTable pedido = instVentas.traerDetalleVentaADevolver(unaVenta);
            dgvProductos.Rows.Clear();
            if (pedido.Rows.Count > 0)
            {

                foreach (DataRow fila in pedido.Rows)
                {
                    var precio = !Convert.ToBoolean(fila["fraccionado"]) ? 0 : Math.Round(decimal.Parse(fila["Precio S/IVA"].ToString()), cantDec);
                    var descRec = Math.Round((decimal)fila["Recargo"] - (decimal)fila["Descuento"], cantDec);
                    dgvProductos.Rows.Add(false,fila["Cod_Barras"].ToString(), fila["Cod_Proveedor"].ToString(), fila["Descripcion"].ToString(), Math.Round(decimal.Parse(fila["Stock"].ToString()), cantStock), Math.Round(decimal.Parse(fila["Precio S/IVA"].ToString()), cantDec), descRec,"", Math.Round(decimal.Parse(fila["Precio C/IVA"].ToString()), cantDec), Math.Round(decimal.Parse(fila["Cantidad"].ToString()), cantStock), Math.Round(decimal.Parse(fila["Subtotal"].ToString()), cantDec), Math.Round(decimal.Parse(fila["Precio S/IVA"].ToString()), cantDec), fila["fk_producto"].ToString(), 0, Math.Round(decimal.Parse(fila["costo"].ToString()), cantDec), Convert.ToBoolean(fila["fraccionado"]), Convert.ToBoolean(fila["dolarizado"]));
                }
            }

            nudDescuento.Value = unDescuento;
            nudRecargo.Value = unRecargo;
            cboIVA.Text = unIva.ToString();
            cboIngBrutos.Text = iibb.ToString();
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
            txtIVA.Text = "0";
            txtIB.Text = "0";
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

            cbProveedor.Visible = cboProveedor.Visible = filtraPorProveedor == 1;

            cboProveedor.DataSource = instProv.traeProveedoresCabecera();
            cboProveedor.ValueMember = "Cod";
            cboProveedor.DisplayMember = "Proveedor";
            cboProveedor.SelectedIndex = 0;

            cboIngBrutos.DataSource = instProv.traerPorcentajeImpuestos();
            cboIngBrutos.ValueMember = "id";
            cboIngBrutos.DisplayMember = "valor";

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
                    resgProducto.Add(new ProductoBusqueda
                    {
                        Descripcion = fila["Descripcion"].ToString(),
                        IdProveedor = Convert.ToInt32(fila["fk_proveedor"])
                    });
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
            decimal totalSIVA = 0;
            decimal totalIVA = 0;
            decimal totalFinal = 0;
            decimal totalDescRec = 0;

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                decimal precioSinIva = decimal.Parse(fila.Cells["precioSinIva"].Value.ToString());
                decimal cantidad = decimal.Parse(fila.Cells["Cantidad"].Value.ToString());
                decimal iva = decimal.Parse(cboIVA.Text);
                decimal descRec = 0;

                decimal.TryParse(fila.Cells["DescRec"].Value?.ToString(), out descRec);

                decimal precioUnitarioAjustado = Math.Round(
                    precioSinIva * (1 + (descRec / 100)),
                    cantDec,
                    MidpointRounding.AwayFromZero
                );

                fila.Cells["SubtotalSinIVA"].Value = precioUnitarioAjustado;

                // 🔹 2. Subtotal SIN IVA
                decimal subtotalSinIVA = Math.Round(
                    precioUnitarioAjustado * cantidad,
                    cantDec,
                    MidpointRounding.AwayFromZero
                );

                // 🔹 3. IVA por ítem
                decimal ivaItem = Math.Round(
                    subtotalSinIVA * (iva / 100),
                    cantDec,
                    MidpointRounding.AwayFromZero
                );

                // 🔹 4. Precio unitario CON IVA
                decimal precioConIva = Math.Round(
                    precioUnitarioAjustado * (1 + iva / 100),
                    cantDec,
                    MidpointRounding.AwayFromZero
                );

                fila.Cells["precioConIva"].Value = precioConIva;

                // 🔹 5. Subtotal final
                decimal subtotal = Math.Round(
                    precioConIva * cantidad,
                    cantDec,
                    MidpointRounding.AwayFromZero
                );

                fila.Cells["Subtotal"].Value = subtotal;


                // 🔹 ACUMULAR (SIEMPRE desde valores ya redondeados)
                totalSIVA += precioSinIva * cantidad;
                totalIVA += ivaItem;
                totalFinal += subtotal;

                totalDescRec += Math.Round(
                    (precioSinIva * (descRec / 100)) * cantidad,
                    cantDec,
                    MidpointRounding.AwayFromZero
                );

            }

            if (dgvProductos.RowCount > 0)
            {
                txtSubSinIVA.Text = totalSIVA.ToString("F2");
                txtDescuento.Text = totalDescRec.ToString("F2");
                var totalGeneralSInIVA = totalSIVA + totalDescRec;
                txtTotalSinIva.Text = totalGeneralSInIVA.ToString("F2");

                txtIVA.Text = totalIVA.ToString("F2");

                decimal totalIB = Math.Round(
                    totalGeneralSInIVA * (Decimal.Parse(cboIngBrutos.Text) / 100),
                    cantDec,
                    MidpointRounding.AwayFromZero
                );

                txtIB.Text = totalIB.ToString("F2");

                decimal totalCalculado = totalFinal + totalIB;
                decimal totalEsperado = Math.Round(totalGeneralSInIVA + totalIVA + totalIB, cantDec);

                // 🔥 Diferencia
                decimal diferencia = totalEsperado - totalCalculado;

                // 👉 Ajustar SOLO si hay diferencia mínima
                if (Math.Abs(diferencia) > 0 && Math.Abs(diferencia) <= 0.05m)
                {
                    totalCalculado += diferencia;
                }

                txtTotGeneral.Text = totalCalculado.ToString("F2");
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
                        dgvProductos.Rows.Add(false, producto.Rows[0]["codBarras"].ToString(), producto.Rows[0]["codProveedor"].ToString(), producto.Rows[0]["descripcion"].ToString(), Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock), precio,0, precio, precio, Math.Round(nudCantidad.Value, cantStock), 0, precio, unProducto, 0, costo, esfraccionado, Convert.ToBoolean(producto.Rows[0]["dolarizado"]));

                    }
                    else
                    {
                        var cantidad = Math.Round(nudCantidad.Value / decimal.Parse(producto.Rows[0]["precio"].ToString()), cantStock);
                        var precio = !esDolarizado ? Math.Round(nudCantidad.Value, cantDec) : Math.Round(nudCantidad.Value * valorDolar, 2);
                        dgvProductos.Rows.Add(producto.Rows[0]["codBarras"].ToString(), producto.Rows[0]["codProveedor"].ToString(), producto.Rows[0]["descripcion"].ToString(), Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock),          precio,0,precio,precio, Math.Round(cantidad, cantStock),           0, precio, unProducto, 0, costo, esfraccionado, Convert.ToBoolean(producto.Rows[0]["dolarizado"]));
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
            nudRecargo.Value = 0;

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if ((bool)fila.Cells["Sel"].Value == true || bonificacionPorLinea == 0)
                {
                    fila.Cells["DescRec"].Value = nudDescuento.Value * -1;
                    fila.Cells["Sel"].Value = false;
                }
            }
            procesoTotales();
        }

        private void nudRecargo_ValueChanged(object sender, EventArgs e)
        {
            nudDescuento.Value = 0;

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if ((bool)fila.Cells["Sel"].Value == true || bonificacionPorLinea == 0)
                {
                    fila.Cells["DescRec"].Value = nudRecargo.Value;
                    fila.Cells["Sel"].Value = false;
                }
            }

            procesoTotales();
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
            if (formularioValido())
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
                return;
            }

            string texto = txtDesc.Text.Trim().ToUpper();

            var result = resgProducto
                .Where(p => p.Descripcion.ToUpper().Contains(texto));

            // 🔹 Si el checkbox está marcado, filtramos por proveedor
            if (cbProveedor.Checked && cboProveedor.SelectedValue != null)
            {
                int idProveedorSeleccionado = Convert.ToInt32(cboProveedor.SelectedValue);

                result = result.Where(p => p.IdProveedor == idProveedorSeleccionado);
            }

            var listaFinal = result.ToList();

            if (listaFinal.Count > 0)
            {
                listaFinal.ForEach(x => lbDesc.Items.Add(x.Descripcion));
                lbDesc.Visible = true;
            }
            else
            {
                lbDesc.Visible = false;
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

        private async void devolver()
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

                string detalle = string.Empty;
                int reglones = 0;
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    detalle += fila.Cells["id"].Value.ToString() + "#";
                    detalle += decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()) + "*";
                    detalle += decimal.Parse(fila.Cells["precioConIva"].Value.ToString()) + "!";
                    detalle += decimal.Parse(fila.Cells["Cantidad"].Value.ToString()) + "?";
                    detalle += decimal.Parse(fila.Cells["DescRec"].Value.ToString()) + "&";
                    detalle += decimal.Parse(fila.Cells["SubtotalSinIVA"].Value.ToString()) + "$";
                    detalle = detalle.Replace(',', '.');
                    reglones++;
                    progreso++;

                }
                if (tieneCaja == 1)
                {
                    Clases.ClassCaja instCaja = new Clases.ClassCaja();
                    DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

                    bool cajaAbierta = cajaEstado.Rows.Count == 0 ? false : (cajaEstado.Rows[0]["estado"].ToString() == "ABIERTA" ? true : false);
                    CajaId = cajaEstado.Rows.Count == 0 ? 0 : int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());
                   
                }

                salida = instVentas.grabarDevolucion(decimal.Parse(txtTotGeneral.Text), unCosto, unCliente, int.Parse(Environment.GetEnvironmentVariable("idUser")), decimal.Parse(cboIVA.Text), nudDescuento.Value, nudRecargo.Value,
                                                     int.Parse(cboVendedores.SelectedValue.ToString()), nudComision.Value / 100, decimal.Parse(cboIngBrutos.Text), detalle, llevaCC, tieneCaja, CajaId);
                if (salida != -1)
                {
                    detalle = string.Empty;
                    reglones = 0;
                }
                else
                {
                    MessageBox.Show(this, "Ha ocurrido un error en el proceso", "DEVOLUCION", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                if (salida == -1) return;

                if (facturaElectronica == 1)
                {
                    ClassFacturacionElectronica instFactElect = new ClassFacturacionElectronica();
                    Formularios.Facturacion.frmIngresarDatosNC unFrmIngresarDatosNC = new Facturacion.frmIngresarDatosNC(decimal.Parse(txtTotGeneral.Text), decimal.Parse(cboIVA.Text), decimal.Parse(cboIngBrutos.Text),true);
                    unFrmIngresarDatosNC.ShowDialog();
                    if (unFrmIngresarDatosNC.DialogResult == DialogResult.OK && unFrmIngresarDatosNC._compAsociado > 0)
                    {
                        var status = await instFactElect.emitirNotaCredito(salida, unFrmIngresarDatosNC._compAsociado, unFrmIngresarDatosNC._fechaCompAsoc, null, null, null, null);

                        if (!status)
                        {
                            MessageBox.Show(this,"Ha ocurrido un error en el proceso de emisión de Nota de Crédito", "DEVOLUCION", MessageBoxButtons.OK, MessageBoxIcon.Error);                           
                        }
                    }
                    else
                    {
                        imprimirDevolucion(salida);
                    }
                }
                else
                {
                    imprimirDevolucion(salida);
                }
                estadoInicial();
            }
        }

        private void imprimirDevolucion(long unaDev)
        {
            //Reportes.frmReport unFrmReport = new Reportes.frmReport();

            //unFrmReport.nombreReporte = "ReportDevolucion.rdlc";
            //List<string> var = new List<string>();
            //var.Add(unaDev.ToString());
            //var.Add(Clases.ClassValidacion.traerEmpresa());
            //var.Add("Tel: " + Clases.ClassValidacion.traerEmpresaTelefono());
            //var.Add(Clases.ClassValidacion.traerEmpresaDireccion());
            //var.Add(Clases.ClassValidacion.traerEmpresaCiudad());
            //var.Add("CUIT: " + Clases.ClassValidacion.traerEmpresaCuit());
            //var.Add(cboIVA.Text);
            //var.Add(cantDec.ToString());
            //var.Add(cantStock.ToString());
            //var.Add(Clases.ClassValidacion.traerRazonSocial());
            //unFrmReport.variable = var;
            //unFrmReport.ShowDialog();

            Clases.ClassReportesITextSharp instItextS = new ClassReportesITextSharp();
            DialogResult result = MessageBox.Show("¿Desea descargar en formato PDF?\n(Sí = PDF / No = Excel)", "Exportar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel) return;

            if (result == DialogResult.Yes)
            {
                instItextS.GenerarDevolucionPDF(unaDev);
            }
            else
            {
                instItextS.GenerarDevolucionExcel(unaDev);
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

        private void cbProveedor_CheckedChanged(object sender, EventArgs e)
        {
            txtDesc.Text = string.Empty;
        }

        private void cboProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDesc.Text = string.Empty;
        }

        private void cboIngBrutos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargado)
            {
                procesoTotales();
            }
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                fila.Cells["Sel"].Value = true;
            }
        }

        private void btnNinguno_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                fila.Cells["Sel"].Value = false;
            }
        }

        private void nudDescuento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                nudDescuento_ValueChanged(null, null);
            }
        }

        private void nudRecargo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Enter)
            {
                nudRecargo_ValueChanged(null, null);
            }
        }
    }
}
