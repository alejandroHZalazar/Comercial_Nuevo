using Comercial.Clases;
using Comercial.Formularios.Clientes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Comercial.Formularios.Ventas
{
    public partial class frmVentasMinorista : Form
    {
        // ── Estado de venta ──────────────────────────────────────────────────
        int unCliente  = 0;
        int unProducto = 0;
        bool esfraccionado   = false;
        decimal _subtotalForzado = 0;
        int idVendedor = 0;
        int CajaId     = 0;

        // Promociones detectadas al confirmar la venta (igual que frmVentas)
        private List<Clases.PromoAplicacion> _promosAplicadas = new List<Clases.PromoAplicacion>();

        // Listas en memoria (cargadas en background)
        private List<string> resgClientes = new List<string>();

        // ── Instancias de clases de negocio ─────────────────────────────────
        Clases.ClassVentas        instVentas = new Clases.ClassVentas();
        Clases.ClassProductos     instProd   = new Clases.ClassProductos();
        Clases.ClassClientes      instClie   = new Clases.ClassClientes();
        Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();

        // ── Parámetros del sistema (mismo patrón class-level que frmVentas) ──
        int cantDec   = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();

        int llevaCC = Clases.ClassParametros.buscarParametro("clientes", "llevaCC") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("clientes", "llevaCC"));

        int tieneConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "tieneConsumidorFinal") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "tieneConsumidorFinal"));

        int clienteConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal"));

        int tieneMediosPagos = Clases.ClassParametros.buscarParametro("ventas", "mediosPagos") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "mediosPagos"));

        int imputaEnVenta = Clases.ClassParametros.buscarParametro("ventas", "pagosEnVenta") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "pagosEnVenta"));

        int tieneProductosBalanza = Clases.ClassParametros.buscarParametro("productos", "tieneProductosBalanza") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("productos", "tieneProductosBalanza"));

        string prefijoBalanza          = Clases.ClassParametros.buscarParametro("productos", "prefijoBalanza");
        string posicionProductoBalanza = Clases.ClassParametros.buscarParametro("productos", "posicionProducto");
        string posicionPrecio          = Clases.ClassParametros.buscarParametro("productos", "posicionPrecio");
        string divisorPrecio           = Clases.ClassParametros.buscarParametro("productos", "divisorPrecio");

        int tieneCaja = Clases.ClassParametros.buscarParametro("caja", "haceCaja") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("caja", "haceCaja"));

        int haceNotaVentaTK = Clases.ClassParametros.buscarParametro("ventas", "notaVentaTK") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "notaVentaTK"));

        int anchoTk = Clases.ClassParametros.buscarParametro("ventas", "anchoTk") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "anchoTk"));

        int productosDolarizados = Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos") == "" ? 0
            : int.Parse(Clases.ClassParametros.buscarParametro("productos", "dolarizaProductos"));

        decimal valorDolar = Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar") == "" ? 0
            : decimal.Parse(Clases.ClassParametros.buscarParametro("productos", "cotizacionDolar"));

        // ── Constructor ──────────────────────────────────────────────────────
        public frmVentasMinorista()
        {
            InitializeComponent();
        }

        // ── Load ─────────────────────────────────────────────────────────────
        private void frmVentasMinorista_Load(object sender, EventArgs e)
        {
            // Icono: reutilizar el mismo recurso embebido de frmVentas
            try
            {
                var res = new System.ComponentModel.ComponentResourceManager(typeof(frmVentas));
                this.Icon = (System.Drawing.Icon)res.GetObject("$this.Icon");
            }
            catch { /* si falla, continuar sin icono */ }

            // Título con usuario logueado (mismo patrón que frmPrincipal)
            string nombreUser = Environment.GetEnvironmentVariable("nombreUser") ?? string.Empty;
            this.Text = string.IsNullOrEmpty(nombreUser)
                ? "Venta — Kiosco"
                : "Venta — Kiosco  |  Usuario: " + nombreUser;

            // Vendedor fijo desde parámetros (no hay combo de vendedor en kiosco)
            string adminStr = Clases.ClassParametros.buscarParametro("configuracion", "admin");
            idVendedor = string.IsNullOrEmpty(adminStr) ? 0 : int.Parse(adminStr);

            // Verificar caja (igual que frmVentas.verificarParametros)
            if (tieneCaja == 1)
            {
                Clases.ClassCaja instCaja = new Clases.ClassCaja();
                DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));
                bool cajaAbierta = cajaEstado.Rows.Count == 0 ? false
                    : cajaEstado.Rows[0]["estado"].ToString() == "ABIERTA";
                CajaId = cajaEstado.Rows.Count == 0 ? 0 : int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());
                if (!cajaAbierta)
                {
                    MessageBox.Show(this, "Debe Abrir Caja", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }
            }

            if (haceNotaVentaTK == 1 && anchoTk == 0)
            {
                MessageBox.Show(this, "Debe establecer el ancho del ticket", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Configurar cliente
            if (tieneConsumidorFinal == 1 && clienteConsumidorFinal == 0)
            {
                MessageBox.Show(this, "Debe crear y parametrizar el cliente CONSUMIDOR FINAL", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Cargar lista de clientes en background (siempre, para búsqueda y nuevo cliente)
            backgroundWorkerClientes.RunWorkerAsync();

            // Combo filtro
            cboFiltro.Items.AddRange(new object[] { "Cód. Proveedor", "Cód. Barras" });
            cboFiltro.SelectedIndex = Clases.ClassParametros.indiceBusqNotaPed();
            if (cboFiltro.SelectedIndex < 0) cboFiltro.SelectedIndex = 1; // default barras para kiosco

            estadoInicial();
        }

        // ── Estado inicial ────────────────────────────────────────────────────
        private void estadoInicial()
        {
            _subtotalForzado  = 0;
            esfraccionado     = false;
            dgvProductos.Rows.Clear();
            lbDesc.Visible    = false;
            lbCliente.Visible = false;
            nudDescuento.Value = 0;
            nudRecargo.Value   = 0;
            txtTotGeneral.Text = "0";
            txtFiltro.Text     = string.Empty;
            txtDesc.Text       = string.Empty;
            txtCliente.Text    = string.Empty;
            unProducto = 0;
            _promosAplicadas.Clear();

            // Restaurar cliente por defecto
            if (tieneConsumidorFinal == 1 && clienteConsumidorFinal > 0)
                cargarDatosClientePorId(clienteConsumidorFinal);
            else
            {
                unCliente = 0;
                lblClienteNombre.Text = string.Empty;
            }

            txtFiltro.Focus();
        }

        // ── Búsqueda de producto ──────────────────────────────────────────────
        private void buscarProducto()
        {
            DataTable producto = null;
            bool productoDeBalanzaEncontrado = false;
            decimal cantidadPorAsterisco   = 1;
            bool    usoCantidadPorAsterisco = false;

            if (txtFiltro.Text != string.Empty)
            {
                if (cboFiltro.SelectedIndex == 0)
                {
                    // Búsqueda por código proveedor
                    producto = instProd.traeProductosPpal(" where baja = 0 and codProveedor = '" + txtFiltro.Text.Trim() + "'");
                }
                else if (cboFiltro.SelectedIndex == 1)
                {
                    if (tieneProductosBalanza == 0)
                    {
                        // Soporte formato "cantidad*codBarras" (ej: "3*7790001234567") — copia exacta de frmVentas
                        string textoBusqueda = txtFiltro.Text.Trim();
                        int posAsterisco = textoBusqueda.IndexOf('*');

                        if (posAsterisco > 0)
                        {
                            string parteIzquierda    = textoBusqueda.Substring(0, posAsterisco).Trim();
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
                        // Ruta balanza — copia exacta de frmVentas
                        if (prefijoBalanza == string.Empty || posicionProductoBalanza == string.Empty ||
                            posicionPrecio == string.Empty || divisorPrecio == string.Empty)
                        {
                            MessageBox.Show(this, "Para operar con productos de balanza debe parametrizar: Prefijo, Posición de Producto, Posicion de Importe y divisior de importe", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (Clases.ClassProductosBalanza.esCodigoBalanza(txtFiltro.Text.Trim(), prefijoBalanza))
                        {
                            var productoBalanzaId = Clases.ClassProductosBalanza.ExtraerPorPosicion(txtFiltro.Text.Trim(), posicionProductoBalanza);
                            if (productoBalanzaId == null) return;

                            producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + productoBalanzaId.Trim());
                            if (producto.Rows.Count > 0)
                            {
                                productoDeBalanzaEncontrado = true;
                                var precioStr = Clases.ClassProductosBalanza.ExtraerPorPosicion(txtFiltro.Text.Trim(), posicionPrecio);
                                if (!string.IsNullOrEmpty(precioStr))
                                {
                                    decimal precioBalanza;
                                    if (decimal.TryParse(precioStr, System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture, out precioBalanza))
                                    {
                                        decimal divisor;
                                        if (decimal.TryParse(divisorPrecio, System.Globalization.NumberStyles.Any,
                                                System.Globalization.CultureInfo.InvariantCulture, out divisor) && divisor > 0)
                                            precioBalanza /= divisor;
                                        if (precioBalanza > 0)
                                            _subtotalForzado = precioBalanza;
                                    }
                                }
                            }
                        }
                        else
                        {
                            producto = instProd.traeProductosPpal(" where baja = 0 and codBarras = " + txtFiltro.Text.Trim());
                        }
                    }
                }
                else
                {
                    producto = instProd.traeProductosPpal(" where baja = 0 and id = " + txtFiltro.Text.Trim());
                }
            }

            if (producto != null && producto.Rows.Count > 0)
            {
                unProducto    = int.Parse(producto.Rows[0]["ID"].ToString());
                esfraccionado = bool.Parse(producto.Rows[0]["fraccionado"].ToString());

                if (!productoDeBalanzaEncontrado)
                {
                    // Kiosco: comportamiento siempre "auto-agregar" (igual a tieneLectoraCB==1 en frmVentas)
                    if (esfraccionado && usoCantidadPorAsterisco)
                    {
                        // Valor antes del '*' = IMPORTE (no cantidad) → guardar en _subtotalForzado
                        _subtotalForzado = cantidadPorAsterisco;
                        agregarProducto(1, false);
                    }
                    else
                    {
                        agregarProducto(cantidadPorAsterisco, usoCantidadPorAsterisco);
                    }
                }
                else
                {
                    if (_subtotalForzado <= 0) return;
                    agregarProducto(1, false);
                }
            }
            else
            {
                MessageBox.Show(this, "No existe el producto: " + txtFiltro.Text.Trim(), "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtFiltro.Clear();
            txtFiltro.Focus();
        }

        // ── Agregar producto a la grilla ──────────────────────────────────────
        private void agregarProducto(decimal cantidadPorAsterisco, bool usoCantidadPorAsterisco)
        {
            bool band = false;

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if (int.Parse(fila.Cells["id"].Value.ToString()) == unProducto)
                {
                    if (!esfraccionado)
                    {
                        // Producto no fraccionado: incrementar cantidad
                        fila.Cells["Cantidad"].Value = decimal.Parse(fila.Cells["Cantidad"].Value.ToString()) + cantidadPorAsterisco;
                        band = true;
                        break;
                    }
                    else
                    {
                        if (_subtotalForzado > 0)
                        {
                            // Fraccionado con subtotal forzado: acumular importe, recalcular cantidad
                            decimal precioConIvaFila = decimal.Parse(fila.Cells["precioConIva"].Value.ToString());
                            decimal subtotalActual   = decimal.Parse(fila.Cells["Subtotal"].Value.ToString());
                            decimal nuevoSubtotal    = subtotalActual + _subtotalForzado;
                            decimal nuevaCantidad    = precioConIvaFila > 0
                                ? Math.Round(nuevoSubtotal / precioConIvaFila, cantStock) : 0;
                            fila.Tag = "subtotalManual";
                            fila.Cells["Cantidad"].Value = nuevaCantidad;
                            fila.Cells["Subtotal"].Value = nuevoSubtotal;
                            _subtotalForzado = 0;
                            band = true;
                        }
                        else
                        {
                            band = false;
                        }
                        break;
                    }
                }
            }

            if (!band)
            {
                DataTable producto = instProd.traerProductosParaEditar(unProducto);
                if (producto.Rows.Count > 0)
                {
                    bool esDolarizado = productosDolarizados == 1 && Convert.ToBoolean(producto.Rows[0]["dolarizado"]);

                    decimal costo = !esDolarizado
                        ? Math.Round(decimal.Parse(producto.Rows[0]["costo"].ToString()),  cantDec)
                        : Math.Round(decimal.Parse(producto.Rows[0]["costo"].ToString())  * valorDolar, cantDec);

                    decimal precio = !esDolarizado
                        ? Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()), cantDec)
                        : Math.Round(decimal.Parse(producto.Rows[0]["precio"].ToString()) * valorDolar, cantDec);

                    // IVA=0 en kiosco → precioConIva = precio
                    decimal precioConIva = Math.Round(precio * (1 + 0m / 100), cantDec);

                    decimal cantidad = _subtotalForzado > 0 && precioConIva > 0
                        ? Math.Round(_subtotalForzado / precioConIva, cantStock)
                        : Math.Round(cantidadPorAsterisco, cantStock);

                    // Columnas en orden (ver AddRange en Designer):
                    // [0] codBarras  [1] descripcion  [2] Cantidad
                    // [3] precioSinIva (Precio Unit.)  [4] precioConIva (Precio Final)
                    // [5] Subtotal  [6] DescRec  [7] subtotalSIVA
                    // [8] id  [9] pedido  [10] costo  [11] fraccionado  [12] dolarizado
                    int nuevoIdx = dgvProductos.Rows.Add(
                        producto.Rows[0]["codBarras"].ToString(),   // 0
                        producto.Rows[0]["descripcion"].ToString(), // 1
                        cantidad,                                   // 2
                        precio,                                     // 3 precioSinIva = Precio Unit. (original)
                        precioConIva,                               // 4 precioConIva = Precio Final (ajustado)
                        Math.Round(precioConIva * cantidad, cantDec), // 5 Subtotal
                        0m,                                         // 6 DescRec
                        precio,                                     // 7 subtotalSIVA (= precioSinIva, IVA=0)
                        unProducto,                                 // 8 id
                        0,                                          // 9 pedido
                        costo,                                      // 10
                        esfraccionado,                              // 11
                        esDolarizado                                // 12
                    );

                    // Subtotal forzado (fraccionado/balanza)
                    if (_subtotalForzado > 0)
                    {
                        dgvProductos.Rows[nuevoIdx].Tag = "subtotalManual";
                        dgvProductos.Rows[nuevoIdx].Cells["Subtotal"].Value = _subtotalForzado;
                        _subtotalForzado = 0;
                    }
                }
            }

            if (dgvProductos.RowCount > 0)
                dgvProductos.FirstDisplayedScrollingRowIndex = dgvProductos.RowCount - 1;

            procesoTotales();
            unProducto = 0;
        }

        // ── Totales (IVA=0 hardcoded, preserva Tag="subtotalManual") ─────────
        private void procesoTotales()
        {
            decimal totalFinal = 0;

            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                decimal precioSinIva = decimal.Parse(fila.Cells["precioSinIva"].Value.ToString());
                decimal cantidad     = decimal.Parse(fila.Cells["Cantidad"].Value.ToString());
                decimal descRec      = 0;
                decimal.TryParse(fila.Cells["DescRec"].Value?.ToString(), out descRec);

                decimal precioUnitarioAjustado = Math.Round(
                    precioSinIva * (1 + descRec / 100), cantDec, MidpointRounding.AwayFromZero);

                fila.Cells["subtotalSIVA"].Value = precioUnitarioAjustado;
                fila.Cells["precioConIva"].Value = precioUnitarioAjustado; // IVA=0

                bool subtotalManual = fila.Tag != null && fila.Tag.ToString() == "subtotalManual";
                decimal subtotal;
                if (subtotalManual)
                {
                    subtotal = decimal.Parse(fila.Cells["Subtotal"].Value.ToString());
                }
                else
                {
                    subtotal = Math.Round(precioUnitarioAjustado * cantidad, cantDec, MidpointRounding.AwayFromZero);
                    fila.Cells["Subtotal"].Value = subtotal;
                }

                totalFinal += subtotal;
            }

            txtTotGeneral.Text = totalFinal.ToString("F2");
        }

        // ── Descuento / Recargo global ────────────────────────────────────────
        private void nudDescuento_ValueChanged(object sender, EventArgs e)
        {
            nudRecargo.Value = 0;
            foreach (DataGridViewRow fila in dgvProductos.Rows)
                fila.Cells["DescRec"].Value = nudDescuento.Value * -1;
            procesoTotales();
        }

        private void nudRecargo_ValueChanged(object sender, EventArgs e)
        {
            nudDescuento.Value = 0;
            foreach (DataGridViewRow fila in dgvProductos.Rows)
                fila.Cells["DescRec"].Value = nudRecargo.Value;
            procesoTotales();
        }

        // ── Vender ────────────────────────────────────────────────────────────
        private async void vender()
        {
            long salida = -1;
            decimal unCosto = 0;
            decimal imputacion = 0;
            BindingList<Clases.ClassVentas.CobroFormasPago> dtFormasPAgo = new BindingList<Clases.ClassVentas.CobroFormasPago>();

            if (dgvProductos.RowCount == 0)
            {
                MessageBox.Show(this, "No existen productos en la grilla", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataGridViewRow fila in dgvProductos.Rows)
                unCosto += Math.Round(
                    decimal.Parse(fila.Cells["costo"].Value.ToString()) *
                    decimal.Parse(fila.Cells["Cantidad"].Value.ToString()), 4);

            string detalleFormasPago = string.Empty;

            // Resolver plan efectivo predeterminado (igual que frmVentas lines 1328-1334)
            int? planPagoId = null;
            {
                string planStr = Clases.ClassParametros.buscarParametro("Cobros", "idPlanEfectivo");
                if (!string.IsNullOrEmpty(planStr))
                {
                    var planPagoDT = instConfig.traerPlanesPagoPorId(int.Parse(planStr));
                    if (planPagoDT.Rows.Count > 0)
                        planPagoId = int.Parse(planPagoDT.Rows[0]["id"].ToString());
                }
            }

            if (tieneMediosPagos == 1 || imputaEnVenta == 1 || llevaCC == 1)
            {
                frmImputacionVenta unFrmImputacion = new frmImputacionVenta(
                    decimal.Parse(txtTotGeneral.Text), planPagoId ?? 0, llevaCC);
                unFrmImputacion.ShowDialog();
                dtFormasPAgo = unFrmImputacion.unDT;

                if (unFrmImputacion.DialogResult == DialogResult.OK)
                {
                    imputacion = System.Linq.Enumerable
                        .Where(dtFormasPAgo, x => x.idPlan != 0)
                        .Sum(x => x.Importe);

                    if (tieneMediosPagos == 1)
                    {
                        var pagosReales = System.Linq.Enumerable
                            .Where(dtFormasPAgo, x => x.idPlan != 0)
                            .ToList();

                        foreach (Clases.ClassVentas.CobroFormasPago item in pagosReales)
                        {
                            detalleFormasPago += item.idMedio + "#";
                            detalleFormasPago += item.idPlan  + "*";
                            detalleFormasPago += item.Importe + "!";
                            detalleFormasPago += item.Referencia1 + "?";
                            detalleFormasPago += item.Referencia2 + ";";
                            detalleFormasPago += item.Referencia3 + "¿";
                            detalleFormasPago  = detalleFormasPago.Replace(',', '.');
                        }

                        // Aplicar recargo del plan → activa nudRecargo_ValueChanged → procesoTotales
                        if (pagosReales.Count > 0)
                            nudRecargo.Value = pagosReales[0].Recargo;
                    }
                }
                else
                {
                    return; // usuario canceló
                }
            }

            // Construir detalle DESPUÉS del recargo (procesoTotales ya actualizó subtotales)
            // Formato idéntico a frmVentas lines 1381-1396
            string detalle = string.Empty;
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                detalle += fila.Cells["id"].Value.ToString() + "#";
                detalle += decimal.Parse(fila.Cells["precioSinIva"].Value.ToString()) + "*";
                detalle += decimal.Parse(fila.Cells["precioConIva"].Value.ToString()) + "!";
                detalle += decimal.Parse(fila.Cells["Cantidad"].Value.ToString())     + "?";
                detalle += fila.Cells["pedido"].Value.ToString()                      + ";";
                detalle += fila.Cells["Subtotal"].Value.ToString()                    + "¿";
                detalle += bool.Parse(fila.Cells["fraccionado"].Value.ToString()) == false ? "0¡" : "1¡";
                detalle += decimal.Parse(fila.Cells["DescRec"].Value.ToString())      + "&";
                detalle += decimal.Parse(fila.Cells["subtotalSIVA"].Value.ToString()) + "$";
                detalle  = detalle.Replace(',', '.');
            }

            salida = instVentas.grabarVenta(
                decimal.Parse(txtTotGeneral.Text),
                unCosto,
                unCliente,
                int.Parse(Environment.GetEnvironmentVariable("idUser")),
                0m,         // IVA = 0
                null,       // descuento global = null
                null,       // recargo global = null
                idVendedor,
                0m,         // comisión = 0
                0m,         // IIBB = 0
                detalle,
                llevaCC, imputaEnVenta, tieneMediosPagos,
                imputacion, detalleFormasPago,
                tieneCaja, CajaId
            );

            // Guardar componentes de promociones detectadas automáticamente (igual que frmVentas)
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
                if (haceNotaVentaTK == 0)
                    imprimirVenta(salida);
                else
                    imprimirNotaVentaTk(salida);

                estadoInicial();
            }
            else
            {
                MessageBox.Show(this, "Ha ocurrido un error en el proceso", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Impresión (copia exacta de frmVentas lines 1157-1214) ─────────────
        private void imprimirVenta(long unaVenta)
        {
            Clases.ClassReportesITextSharp instItext = new ClassReportesITextSharp();

            DialogResult result = MessageBox.Show(
                "¿Desea descargar en formato PDF?\n(Sí = PDF / No = Excel)",
                "Exportar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return;

            if (result == DialogResult.Yes)
                instItext.GenerarVentasPDF(unaVenta);
            else
                instItext.GenerarVentasExcel(unaVenta);
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
                    PrecioUnit  = Convert.ToDecimal(fila["precioConIva"]),
                    Cantidad    = Convert.ToDecimal(fila["cantidad"]),
                    subtotal    = Convert.ToDecimal(fila["subtotal"]),
                });
            }

            var ticket = new TicketPrinter(
                items,
                ventaDT.Rows[0]["Vendedor"].ToString(),
                ventaDT.Rows[0]["nombreComercial"].ToString(),
                decimal.Parse(ventaDT.Rows[0]["totalVenta"].ToString()),
                DateTime.Parse(ventaDT.Rows[0]["fecha"].ToString()),
                ventaDT.Rows[0]["nroVenta"].ToString(),
                decimal.Parse(ventaDT.Rows[0]["descuento"].ToString()),
                decimal.Parse(ventaDT.Rows[0]["recargo"].ToString()),
                anchoTk == 80 ? 42 : 32);
            ticket.Imprimir();
        }

        // ── Cargar cliente por ID (nuevo cliente o restaurar CF) ─────────────
        private void cargarDatosClientePorId(int id)
        {
            DataTable cliente = instClie.traerDatosVenta(" and c.id = " + id);
            if (cliente.Rows.Count > 0)
            {
                unCliente             = int.Parse(cliente.Rows[0]["ID"].ToString());
                lblClienteNombre.Text = cliente.Rows[0]["Nombre_Comercial"].ToString();
            }
            lbCliente.Visible = false;
            txtCliente.Text   = string.Empty;
        }

        // ── Nuevo cliente ─────────────────────────────────────────────────────
        private void btnAltaCliente_Click(object sender, EventArgs e)
        {
            frmAltaModifClientes unFrmAlta = new frmAltaModifClientes();
            unFrmAlta.unaAccion = 1;
            unFrmAlta.idCliente = 0;
            unFrmAlta.ShowDialog();
            if (unFrmAlta.DialogResult == DialogResult.OK)
            {
                // Refrescar lista en background para que aparezca en búsquedas futuras
                if (!backgroundWorkerClientes.IsBusy)
                    backgroundWorkerClientes.RunWorkerAsync();
                // Autoseleccionar el cliente recién creado
                cargarDatosClientePorId(instClie.traerUltimoCliente());
            }
        }

        // ── Búsqueda de cliente ───────────────────────────────────────────────
        private void txtCliente_TextChanged(object sender, EventArgs e)
        {
            if (txtCliente.Focused && txtCliente.Text != string.Empty)
                filtrarCargarListBoxClie();
            else
                lbCliente.Visible = false;
        }

        private void filtrarCargarListBoxClie()
        {
            lbCliente.Items.Clear();
            if (txtCliente.Text.Trim().Length == 0) { lbCliente.Visible = false; return; }

            var result = resgClientes.FindAll(l => l.ToUpper().Contains(txtCliente.Text.Trim().ToUpper()));
            lbCliente.Items.Clear();
            if (result.Count > 0)
            {
                result.ForEach(x => lbCliente.Items.Add(x));
                lbCliente.Visible = true;
            }
            else
                lbCliente.Visible = false;
        }

        private void cargarDatosCliente()
        {
            if (lbCliente.SelectedItem == null) return;
            string sel   = lbCliente.SelectedItem.ToString();
            string idStr = sel.Substring(0, sel.IndexOf("-")).Trim();
            DataTable cliente = instClie.traerDatosVenta(" and c.id = " + idStr);
            if (cliente.Rows.Count > 0)
            {
                unCliente = int.Parse(cliente.Rows[0]["ID"].ToString());
                lblClienteNombre.Text = cliente.Rows[0]["Nombre_Comercial"].ToString();
            }
            lbCliente.Visible = false;
            txtCliente.Text   = string.Empty;
            txtFiltro.Focus();
        }

        // ── Búsqueda por descripción (timer debounce 400ms) ───────────────────
        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
            timerBusq.Stop();
            if (txtDesc.Text.Trim().Length >= 2)
                timerBusq.Start();
            else
                lbDesc.Visible = false;
        }

        private void timerBusq_Tick(object sender, EventArgs e)
        {
            timerBusq.Stop();
            buscarPorDescripcion();
        }

        private void buscarPorDescripcion()
        {
            string texto = txtDesc.Text.Trim();
            if (texto.Length < 2) { lbDesc.Visible = false; return; }

            DataTable dt = instProd.traeProductosPpal(
                " where baja = 0 and descripcion like '%" + texto.Replace("'", "''") + "%' limit 20");

            lbDesc.Items.Clear();
            foreach (DataRow row in dt.Rows)
                lbDesc.Items.Add(row["Descripcion"].ToString() + "|" + row["ID"].ToString());

            lbDesc.Visible = lbDesc.Items.Count > 0;
        }

        private void prepararDesdeListBox()
        {
            if (lbDesc.SelectedItem == null) return;
            string item   = lbDesc.SelectedItem.ToString();
            int    idxSep = item.LastIndexOf('|');
            if (idxSep < 0) return;

            unProducto    = int.Parse(item.Substring(idxSep + 1));
            esfraccionado = false; // descripción no usa * → no hay _subtotalForzado activo

            agregarProducto(1, false);
            lbDesc.Visible = false;
            txtDesc.Clear();
            txtFiltro.Focus();
        }

        // ── BackgroundWorker — cargar lista de clientes ───────────────────────
        private void backgroundWorkerClientes_DoWork(object sender, DoWorkEventArgs e)
        {
            // Instancia propia: el hilo secundario no puede compartir la conexión de instClie
            Clases.ClassClientes instClieBW = new Clases.ClassClientes();
            DataTable clientes = instClieBW.buscarAVender();
            var lista = new List<string>();
            foreach (DataRow fila in clientes.Rows)
                lista.Add(fila["ID"].ToString() + "-  " + fila["Nombre_Comercial"].ToString() + "  -" + fila["Dir"].ToString());
            e.Result = lista;
        }

        private void backgroundWorkerClientes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null && e.Result is List<string> lista)
                resgClientes = lista;
        }

        // ── Event handlers ────────────────────────────────────────────────────
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            // Detección automática de promociones (igual que frmVentas)
            if (Clases.ClassParametros.buscarParametro("promociones", "activo") == "1")
            {
                if (!verificarYAplicarPromociones())
                    return; // el operador eligió "Volver a la venta"
            }
            vender();
        }

        private void txtFiltro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && txtFiltro.Text != string.Empty)
            {
                buscarProducto();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtFiltro.Text != string.Empty)
                buscarProducto();
        }

        private void lbDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Back)
                txtDesc.Focus();
            if (e.KeyData == Keys.Enter && lbDesc.Items.Count > 0)
                prepararDesdeListBox();
        }

        private void lbDesc_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            prepararDesdeListBox();
        }

        private void txtDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down && lbDesc.Items.Count > 0)
            {
                lbDesc.SetSelected(0, true);
                lbDesc.Focus();
            }
        }

        private void txtCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down && lbCliente.Items.Count > 0)
            {
                lbCliente.SetSelected(0, true);
                lbCliente.Focus();
            }
        }

        private void lbCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                cargarDatosCliente();
        }

        private void lbCliente_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cargarDatosCliente();
        }

        private void dgvProductos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                procesoTotales();
        }

        private void dgvProductos_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            procesoTotales();
        }

        private void dgvProductos_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e?.Value != null && e.Value.ToString().IndexOf('.') != -1)
            {
                try
                {
                    e.Value = Clases.ClassValidacion.cambiarPuntoPorComa(e.Value.ToString());
                    e.ParsingApplied = true;
                }
                catch
                {
                    e.ParsingApplied = false;
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow != null)
                dgvProductos.Rows.Remove(dgvProductos.CurrentRow);
        }

        private void frmVentasMinorista_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
                btnGrabar_Click(null, null);

            if (e.KeyData == Keys.F2 && txtFiltro.Text != string.Empty)
                buscarProducto();

            if (e.KeyData == Keys.Delete && dgvProductos.CurrentRow != null &&
                !dgvProductos.IsCurrentCellInEditMode)
                btnEliminar_Click(null, null);
        }

        // ── Promociones ───────────────────────────────────────────────────────

        /// <summary>
        /// Detecta las promociones que se pueden armar con los productos del carrito,
        /// muestra el diálogo de confirmación y aplica los cambios en la grilla.
        /// Retorna false si el operador elige "Volver a la venta".
        /// Copia exacta de frmVentas.verificarYAplicarPromociones().
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
                List<Clases.PromoInfo> promos = Clases.ClassPromoEngine.MapearDesdeDataSet(dsPromos);

                if (promos == null || promos.Count == 0)
                    return true; // sin promos configuradas, continuar normal

                // 3. Ejecutar el motor de detección
                Clases.ResultadoPromos resultado = Clases.ClassPromoEngine.Calcular(promos, carrito);

                if (resultado.Aplicaciones.Count == 0 && resultado.Alertas.Count == 0)
                    return true; // nada que hacer

                // 4. Mostrar diálogo de confirmación
                frmAvisoPromociones dlg = new frmAvisoPromociones();
                dlg.Resultado = resultado;
                DialogResult dr = dlg.ShowDialog(this);

                if (dr != DialogResult.OK)
                    return false; // operador canceló

                if (resultado.Aplicaciones.Count == 0)
                    return true; // solo alertas; confirma sin cambios en grilla

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
        /// Adapta el Rows.Add a la estructura de columnas de frmVentasMinorista.
        /// </summary>
        private void aplicarPromosEnGrilla(List<Clases.PromoAplicacion> aplicaciones)
        {
            foreach (var ap in aplicaciones)
            {
                // A. Descontar componentes usados de la grilla (idéntico a frmVentas)
                var filasAEliminar = new List<DataGridViewRow>();
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

                // B. Agregar fila del producto-promo (estructura de columnas minorista)
                DataTable dtProd = instProd.traerProductosParaEditar(ap.Promo.IdProducto);
                if (dtProd.Rows.Count > 0)
                {
                    DataRow r = dtProd.Rows[0];
                    bool esDol = productosDolarizados == 1 && Convert.ToBoolean(r["dolarizado"]);

                    decimal costo = !esDol
                        ? Math.Round(decimal.Parse(r["costo"].ToString()), cantDec)
                        : Math.Round(decimal.Parse(r["costo"].ToString()) * valorDolar, cantDec);
                    decimal precio = !esDol
                        ? Math.Round(decimal.Parse(r["precio"].ToString()), cantDec)
                        : Math.Round(decimal.Parse(r["precio"].ToString()) * valorDolar, cantDec);

                    // IVA=0 → precioConIva = precio
                    decimal cantidad = ap.Veces;
                    decimal subtotal = Math.Round(precio * cantidad, cantDec);

                    // Columnas minorista:
                    // [0] codBarras  [1] descripcion  [2] Cantidad
                    // [3] precioSinIva (Precio Unit.)  [4] precioConIva (Precio Final)
                    // [5] Subtotal  [6] DescRec  [7] subtotalSIVA
                    // [8] id  [9] pedido  [10] costo  [11] fraccionado  [12] dolarizado
                    dgvProductos.Rows.Add(
                        r["codBarras"],             // 0
                        r["descripcion"],           // 1
                        cantidad,                   // 2  ap.Veces = nro de veces que dispara la promo
                        precio,                     // 3  precioSinIva (Precio Unit.)
                        precio,                     // 4  precioConIva (Precio Final, IVA=0)
                        subtotal,                   // 5  Subtotal
                        0m,                         // 6  DescRec
                        precio,                     // 7  subtotalSIVA
                        ap.Promo.IdProducto,        // 8  id
                        0,                          // 9  pedido
                        costo,                      // 10 costo
                        ap.Promo.EsFraccionado,     // 11 fraccionado
                        esDol                       // 12 dolarizado
                    );
                }
            }

            procesoTotales();
        }
    }
}
