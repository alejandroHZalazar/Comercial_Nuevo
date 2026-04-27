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
        private List<ProductoBusqueda> resgProducto = new List<ProductoBusqueda>();
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
        int facturaEnVenta = Clases.ClassParametros.buscarParametro("ventas", "facturaEnVenta") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaEnVenta"));
        int facturaFiscal = Clases.ClassParametros.buscarParametro("ventas", "facturaFiscal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaFiscal"));
        int facturaElectronica = Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica"));
        string marcaFiscal = Clases.ClassParametros.buscarParametro("ventas", "marcaFiscal");
        int productosDolarizados = Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos"));
        decimal valorDolar = Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar") == "" ? 0 : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar"));
        int haceNotaVentaTK = Clases.ClassParametros.buscarParametro("ventas", "notaVentaTK") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "notaVentaTK"));
        int anchoTk = Clases.ClassParametros.buscarParametro("ventas", "anchoTk") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "anchoTk"));
        int puntoVenta = Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName) == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName));
        int tieneLectoraCB = Clases.ClassParametros.buscarParametro("productos", "MecanismoLectora") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("productos", "MecanismoLectora"));
        int filtraPorProveedor = Clases.ClassParametros.buscarParametro("ventas", "filtraPorProveedor") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "filtraPorProveedor"));
        int bonificacionPorLinea  = Clases.ClassParametros.buscarParametro("ventas", "bonificacionesPorDetalle") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "bonificacionesPorDetalle"));

        bool ordenFacturar = false;

        // Promociones detectadas automáticamente al presionar Vender.
        // Se persisten en BD después de grabarVenta exitoso.
        private System.Collections.Generic.List<Clases.PromoAplicacion> _promosAplicadas =
            new System.Collections.Generic.List<Clases.PromoAplicacion>();

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
            _promosAplicadas.Clear();
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
            dgvProductos.Columns["Sel"].Visible = bonificacionPorLinea == 1;
            panelSelGrilla.Visible = bonificacionPorLinea == 1;
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

            cbProveedor.Visible = cboProveedor.Visible = filtraPorProveedor == 1;

            cboProveedor.DataSource = instProv.traeProveedoresCabecera();
            cboProveedor.ValueMember = "Cod";
            cboProveedor.DisplayMember = "Proveedor";
            cboProveedor.SelectedIndex = 0;

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

        public void cargarPedidoPendiente(int unPed, decimal unIva, decimal? unDescuento, decimal? unRecargo)
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

                // Si la cabecera tiene descuento/recargo global se aplica igual a todas las filas:
                //   descRec = (descuento * -1) + recargo
                // Si es null significa que el pedido usa bonificación por línea:
                //   se lee descuento/recargo de cada fila de PedidoDetalle
                bool tieneGlobal = unDescuento.HasValue || unRecargo.HasValue;
                decimal descRecGlobal = tieneGlobal
                    ? ((unDescuento ?? 0) * -1) + (unRecargo ?? 0)
                    : 0;

                foreach (DataRow fila in pedido.Rows)
                {
                    decimal descRec;
                    if (tieneGlobal)
                    {
                        descRec = descRecGlobal;
                    }
                    else
                    {
                        // Leer bonificación propia de la línea (campos nuevos en PedidoDetalle)
                        decimal lineDesc = 0, lineRec = 0;
                        if (pedido.Columns.Contains("descuento") && fila["descuento"] != DBNull.Value)
                            decimal.TryParse(fila["descuento"].ToString(), out lineDesc);
                        if (pedido.Columns.Contains("recargo") && fila["recargo"] != DBNull.Value)
                            decimal.TryParse(fila["recargo"].ToString(), out lineRec);
                        descRec = (lineDesc * -1) + lineRec;
                    }

                    dgvProductos.Rows.Add(false, fila["Cod_Barras"].ToString(), fila["Cod_Proveedor"].ToString(), fila["Descripcion"].ToString(), Math.Round(decimal.Parse(fila["Stock"].ToString()), cantStock), Math.Round(decimal.Parse(fila["Precio S/IVA"].ToString()), cantDec), descRec, "", Math.Round(decimal.Parse(fila["Precio C/IVA"].ToString()), cantDec), Math.Round(decimal.Parse(fila["Cantidad"].ToString()), cantStock), Math.Round(decimal.Parse(fila["Subtotal"].ToString()), cantDec), Math.Round(decimal.Parse(fila["precioOrig"].ToString()), cantDec), fila["fk_producto"].ToString(), unPedido, Math.Round(decimal.Parse(fila["costo"].ToString()), cantDec), Convert.ToBoolean(fila["fraccionado"]), Convert.ToBoolean(fila["dolarizado"]));
                }
            }
            unPedido = 0;
            nudDescuento.Value = 0;
            nudRecargo.Value = 0;
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

                // 🔹 1. Precio unitario SIN IVA ajustado (TU regla)
                decimal precioUnitarioAjustado = Math.Round(
                    precioSinIva * (1 + (descRec / 100)),
                    cantDec,
                    MidpointRounding.AwayFromZero
                );

                fila.Cells["subtotalSIVA"].Value = precioUnitarioAjustado;

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

            // Cantidad ingresada por el usuario mediante el separador '*' (ej: "3*7790001234567")
            decimal cantidadPorAsterisco = 1;
            bool    usoCantidadPorAsterisco = false;

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
                        // Soporte de formato "cantidad*codBarras" (ej: "3*7790001234567")
                        string textoBusqueda = txtFiltro.Text.Trim();
                        int posAsterisco = textoBusqueda.IndexOf('*');

                        if (posAsterisco > 0)
                        {
                            string parteIzquierda  = textoBusqueda.Substring(0, posAsterisco).Trim();
                            string codigoBarrasParte = textoBusqueda.Substring(posAsterisco + 1).Trim();

                            decimal cantParsed;
                            if (decimal.TryParse(parteIzquierda,
                                    System.Globalization.NumberStyles.Any,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    out cantParsed) && cantParsed > 0)
                            {
                                cantidadPorAsterisco    = cantParsed;
                                usoCantidadPorAsterisco = true;
                            }

                            textoBusqueda = codigoBarrasParte;
                        }

                        producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + textoBusqueda);
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
                        if (usoCantidadPorAsterisco)
                            nudCantidad.Value = cantidadPorAsterisco;
                        nudCantidad.Focus();
                    }
                    else
                    {
                        nudCantidad.Value = usoCantidadPorAsterisco ? cantidadPorAsterisco : 1;
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
                    // if (!esfraccionado)
                    //   {
                    var precio = !esDolarizado ? Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec) : Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()) * valorDolar, cantDec);
                    dgvProductos.Rows.Add(false,producto.Rows[0]["codBarras"].ToString(), producto.Rows[0]["codProveedor"].ToString(), producto.Rows[0]["descripcion"].ToString(), Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock), precio,0, precio, precio, Math.Round(nudCantidad.Value, cantStock), 0, precio, unProducto, 0, costo, esfraccionado);
                    // }
                    //else
                    //{
                    //    var cantidad = Math.Round(nudCantidad.Value / decimal.Parse(producto.Rows[0]["precio"].ToString()), cantStock);
                    //    var precio = !esDolarizado ? Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec) : Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()) * valorDolar, cantDec);
                    //    dgvProductos.Rows.Add(producto.Rows[0]["codBarras"].ToString(), producto.Rows[0]["codProveedor"].ToString(), producto.Rows[0]["descripcion"].ToString(), Math.Round(decimal.Parse(producto.Rows[0]["cantidad"].ToString()), cantStock), precio, precio, cantidad, precio, precio, unProducto, 0, costo, esfraccionado);
                    //}
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

                foreach(DataGridViewRow fila in dgvProductos.Rows)
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

        private void frmVentas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                btnGrabar_Click(null, null);
            }

            if (e.KeyData == Keys.F6)
            {
                try
                {
                    ordenFacturar = true;
                    btnGrabar_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "ERROR EN EL PROCESO DE FACTURACION. " + ex.Message, "FACTURACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ordenFacturar = false;
                }
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
                // Detección automática de promociones
                if (Clases.ClassParametros.buscarParametro("promociones", "activo") == "1")
                {
                    if (!verificarYAplicarPromociones())
                    {
                        ordenFacturar = false;
                        return; // el operador eligió "Volver a la venta"
                    }
                }

                vender();
                ordenFacturar = false;
                if (buscoPend)
                {
                    btnPedido_Click(null, null);
                }
            }
            ordenFacturar = false;
        }

        /// <summary>
        /// Detecta las promociones que se pueden armar con los productos del carrito,
        /// muestra el diálogo de confirmación y aplica los cambios en la grilla.
        /// Retorna false si el operador elige "Volver a la venta".
        /// </summary>
        private bool verificarYAplicarPromociones()
        {
            try
            {
                // 1. Construir el carrito desde la grilla
                var carrito = new System.Collections.Generic.Dictionary<int, decimal>();
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    int idProd = int.Parse(fila.Cells["id"].Value.ToString());
                    decimal cant = decimal.Parse(fila.Cells["Cantidad"].Value.ToString());
                    if (carrito.ContainsKey(idProd))
                        carrito[idProd] += cant;
                    else
                        carrito[idProd] = cant;
                }

                // 2. Cargar promos activas desde la BD
                Clases.ClassPromociones instPromo = new Clases.ClassPromociones();
                DataSet dsPromos = instPromo.obtenerTodasPromocionesActivas();
                System.Collections.Generic.List<Clases.PromoInfo> promos =
                    Clases.ClassPromoEngine.MapearDesdeDataSet(dsPromos);

                if (promos == null || promos.Count == 0)
                    return true; // sin promos configuradas, continuar normal

                // 3. Ejecutar el motor de detección
                Clases.ResultadoPromos resultado = Clases.ClassPromoEngine.Calcular(promos, carrito);

                if (resultado.Aplicaciones.Count == 0 && resultado.Alertas.Count == 0)
                    return true; // nada que hacer

                // Si solo hay alertas (ninguna promo se pudo armar), también avisan
                // pero no hay nada para aplicar. Mostramos el diálogo de todas formas
                // para que el operador vea las alertas.

                // 4. Mostrar diálogo de confirmación
                Formularios.Ventas.frmAvisoPromociones dlg = new Formularios.Ventas.frmAvisoPromociones();
                dlg.Resultado = resultado;
                DialogResult dr = dlg.ShowDialog(this);

                if (dr != DialogResult.OK)
                    return false; // operador canceló

                if (resultado.Aplicaciones.Count == 0)
                    return true; // solo había alertas; confirma sin cambios en grilla

                // 5. Aplicar en grilla
                aplicarPromosEnGrilla(resultado.Aplicaciones);
                _promosAplicadas = resultado.Aplicaciones;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al verificar promociones: " + ex.Message,
                    "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true; // ante error, continuar con la venta normal
            }
        }

        /// <summary>
        /// Modifica la grilla: descuenta los componentes usados y agrega las líneas de promo.
        /// </summary>
        private void aplicarPromosEnGrilla(System.Collections.Generic.List<Clases.PromoAplicacion> aplicaciones)
        {
            foreach (var ap in aplicaciones)
            {
                // A. Descontar componentes de la grilla
                var filasAEliminar = new System.Collections.Generic.List<DataGridViewRow>();

                foreach (var comp in ap.Componentes)
                {
                    foreach (DataGridViewRow fila in dgvProductos.Rows)
                    {
                        if (int.Parse(fila.Cells["id"].Value.ToString()) == comp.IdProducto)
                        {
                            decimal actual = decimal.Parse(fila.Cells["Cantidad"].Value.ToString());
                            decimal nueva  = actual - comp.Cantidad;
                            if (nueva <= 0)
                                filasAEliminar.Add(fila);
                            else
                                fila.Cells["Cantidad"].Value = Math.Round(nueva, cantStock);
                            break;
                        }
                    }
                }

                foreach (DataGridViewRow fila in filasAEliminar)
                    dgvProductos.Rows.Remove(fila);

                // B. Agregar línea del producto-promo
                DataTable dtProd = instProd.traerProductosParaEditar(ap.Promo.IdProducto);
                if (dtProd.Rows.Count > 0)
                {
                    DataRow r = dtProd.Rows[0];
                    bool esDol = productosDolarizados == 1 && Convert.ToBoolean(r["dolarizado"]);
                    decimal costo  = !esDol
                        ? Math.Round(decimal.Parse(r["costo"].ToString()), cantDec)
                        : Math.Round(decimal.Parse(r["costo"].ToString()) * valorDolar, cantDec);
                    decimal precio = !esDol
                        ? Math.Round(decimal.Parse(r["precio"].ToString()), cantDec)
                        : Math.Round(decimal.Parse(r["precio"].ToString()) * valorDolar, cantDec);

                    dgvProductos.Rows.Add(
                        false,
                        r["codBarras"],
                        r["codProveedor"],
                        r["descripcion"],
                        Math.Round(decimal.Parse(r["cantidad"].ToString()), cantStock),
                        precio,
                        0,
                        precio,
                        precio,
                        ap.Veces,
                        0,
                        precio,
                        ap.Promo.IdProducto,
                        0,
                        costo,
                        Convert.ToBoolean(r["fraccionado"]),
                        esDol
                    );
                }
            }

            procesoTotales();
        }

        private void imprimirVenta(long unaVenta)
        {
            Clases.ClassReportesITextSharp instItextSahrp = new ClassReportesITextSharp();
            //Reportes.frmReport unFrmReport = new Reportes.frmReport();

            //unFrmReport.nombreReporte = "ReportVenta.rdlc";
            //List<string> var = new List<string>();
            //var.Add(unaVenta.ToString());
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

            DialogResult result = MessageBox.Show("¿Desea descargar en formato PDF?\n(Sí = PDF / No = Excel)", "Exportar",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            if (result == DialogResult.Yes)
            {

                instItextSahrp.GenerarVentasPDF(unaVenta);
            }
            else
            {
                instItextSahrp.GenerarVentasExcel(unaVenta);
            }
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

        private void backgroundWorkerTarea_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private async void vender()
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

                if (imputaEnVenta == 1 || llevaCC == 1)
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
                    detalle += decimal.Parse(fila.Cells["DescRec"].Value.ToString()) + "&";
                    detalle += decimal.Parse(fila.Cells["subtotalSIVA"].Value.ToString()) + "$";
                    detalle = detalle.Replace(',', '.');
                    progreso++;

                }

                salida = instVentas.grabarVenta(decimal.Parse(txtTotGeneral.Text), unCosto, unCliente, int.Parse(Environment.GetEnvironmentVariable("idUser")), decimal.Parse(cboIVA.Text),null, null, int.Parse(cboVendedores.SelectedValue.ToString()), nudComision.Value / 100, decimal.Parse(cboIngBrutos.Text), detalle, llevaCC, imputaEnVenta, tieneMediosPagos, imputacion, detalleFormasPago, tieneCaja, CajaId);

                // Guardar componentes de promociones detectadas automáticamente
                if (salida != -1 && _promosAplicadas.Count > 0)
                {
                    foreach (var ap in _promosAplicadas)
                    {
                        long lineaDetalle = instVentas.traerLineaDetalleVenta(salida, ap.Promo.IdProducto);
                        if (lineaDetalle > 0)
                        {
                            string detalleComp = string.Join(";",
                                System.Linq.Enumerable.Select(ap.Componentes,
                                    c => c.IdSlot + "#" + c.IdProducto + "*" +
                                         c.Cantidad.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                            instVentas.guardarComponentesPromocion(lineaDetalle, detalleComp);
                        }
                    }
                    _promosAplicadas.Clear();
                }

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
                        if (facturaEnVenta == 1 && ordenFacturar)
                        {
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
                            else if (facturaElectronica == 1)
                            {

                                ClassFacturacionElectronica instFactElect = new ClassFacturacionElectronica();
                                var status = await instFactElect.emitirFacturaElectronica(salida);
                                if (!status)
                                {
                                    MessageBox.Show(this,"Ha ocurrido un error en el proceso de facturación", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
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
                    }


                    estadoInicial();


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

        private void cbProveedor_CheckedChanged(object sender, EventArgs e)
        {
            txtDesc.Text = string.Empty;
        }

        private void cboProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDesc.Text = string.Empty;
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
