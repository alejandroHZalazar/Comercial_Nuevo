using Comercial.Enums;
using Comercial.Properties;
using Comercial.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Comercial.Clases
{
    public class ClassFacturacionElectronica
    {
        string userToken = Clases.ClassParametros.buscarParametro("facturacionElectronica", "userToken");
        string apiKey = Clases.ClassParametros.buscarParametro("facturacionElectronica", "apiKey");
        string apiToken = Clases.ClassParametros.buscarParametro("facturacionElectronica", "apiToken");
        int clienteConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal"));
        string enviarFacturaMail = Clases.ClassParametros.buscarParametro("facturacionElectronica", "enviarFacturaPorMail") == "" ? "N" : Clases.ClassParametros.buscarParametro("facturacionElectronica", "enviarFacturaPorMail");
        int puntoVenta = Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName) == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName));
        string rubroFE = Clases.ClassParametros.buscarParametro("facturacionElectronica", "rubro") == "" ? "Productos Varios" : Clases.ClassParametros.buscarParametro("facturacionElectronica", "rubro");
        int tributoIIBB = Clases.ClassParametros.buscarParametro("facturacionElectronica", "tributoIIBB") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("facturacionElectronica", "tributoIIBB"));
        int regimenIIBB = Clases.ClassParametros.buscarParametro("facturacionElectronica", "regimenIIBB") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("facturacionElectronica", "regimenIIBB"));
        string codigoDetalle = Clases.ClassParametros.buscarParametro("facturacionElectronica", "CodigoDetalle") == "" ? "CodInterno" : Clases.ClassParametros.buscarParametro("facturacionElectronica", "CodigoDetalle");
        public class FacturaRequest
        {
            public string usertoken { get; set; }
            public string apikey { get; set; }
            public string apitoken { get; set; }

            public Cliente cliente { get; set; }
            public Comprobante comprobante { get; set; }
        }

        public class Cliente
        {
            public string documento_tipo { get; set; }

            public string documento_nro { get; set; }

            public string razon_social { get; set; }

            public string nombre_fantasia { get; set; }

            public string email { get; set; }

            public string domicilio { get; set; }

            public string provincia { get; set; }

            public string codigo { get; set; }

            public string envia_por_mail { get; set; }

            public string condicion_pago { get; set; }

            public string condicion_pago_otra { get; set; }

            public string condicion_iva { get; set; }

            public string condicion_iva_operacion { get; set; }

            public string reclama_deuda { get; set; }

            public int reclama_deuda_dias { get; set; }

            public int reclama_deuda_repite_dias { get; set; }

            public string rg5329 { get; set; }
        }

        public class Comprobante
        {
            public string fecha { get; set; }
            public string tipo { get; set; }
            public string punto_venta { get; set; }
            public string operacion { get; set; }
            public string idioma { get; set; }
            public string vencimiento { get; set; }
            public string periodo_facturado_desde { get; set; }
            public string periodo_facturado_hasta { get; set; }
            public string rubro { get; set; }
            public string rubro_grupo_contable { get; set; }
            public string moneda { get; set; }
            public string cotizacion { get; set; }
            public List<DetalleFactura> detalle { get; set; }
            public decimal total { get; set; }
            public List<Tributo> tributos { get; set; }
            public List<ComprobanteAsociado> comprobantes_asociados { get; set; }
            public decimal bonificacion { get; set; }
            public Pagos pagos { get; set; }
        }

        public class DetalleFactura
        {
            public decimal cantidad { get; set; }
            public string afecta_stock { get; set; }
            public Producto producto { get; set; }
            public decimal bonificacion_porcentaje { get; set; }
        }

        public class Tributo
        {
            public int tipo { get; set; }
            public int regimen { get; set; }
            public decimal base_imponible { get; set; }
            public decimal alicuota { get; set; }
            public decimal total { get; set; }
        }

        public class Producto
        {
            public string descripcion { get; set; }
            public int unidad_bulto { get; set; }
            public string lista_precios { get; set; }
            public decimal precio_unitario_sin_iva { get; set; }
            public string codigo { get; set; }
            public decimal alicuota { get; set; }
            public int unidad_medida { get; set; }
            public string actualiza_precio { get; set; }
            public string rg5329 { get; set; }
        }

        public class Pagos
        {
            public List<FormaPago> formas_pago { get; set; }
            public decimal total { get; set; }
        }

        public class FormaPago
        {
            public string descripcion { get; set; }
            public decimal importe { get; set; }
        }

        public class ComprobanteAsociado
        {
            public string tipo_comprobante { get; set; }   // FACTURA A

            public int punto_venta { get; set; }           // hasta 5 dígitos

            public int numero { get; set; }                // hasta 8 dígitos

            public string comprobante_fecha { get; set; }  // "dd/MM/yyyy"

            public long cuit { get; set; }                 // 11 dígitos
        }

        public static decimal CalcularTotal(List<DetalleFactura> detalles)
        {
            decimal total = 0;

            foreach (var item in detalles)
            {
                decimal baseItem = item.producto.precio_unitario_sin_iva * item.cantidad;
                decimal iva = baseItem * (item.producto.alicuota / 100);

                total += baseItem + iva;
            }

            return total;
        }

        public async Task<(bool ok, string content, string error)> emitirComprobante(FacturaRequest comprobante)
        {
            string urlApi = "https://www.tusfacturas.app/app/api/v2/facturacion/nuevo";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(comprobante);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    client.Timeout = TimeSpan.FromSeconds(60);

                    var response = await client.PostAsync(urlApi, content);

                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        // Error HTTP (400, 500, etc.)
                        return (false, responseContent, $"HTTP {(int)response.StatusCode}");
                    }

                    // OK
                    return (true, responseContent, null);
                }
                catch (TaskCanceledException ex)
                {
                    // Timeout
                    return (false, null, "Timeout en la petición");
                }
                catch (HttpRequestException ex)
                {
                    // Error de red
                    return (false, null, $"Error HTTP Request: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Error general
                    return (false, null, $"Error general: {ex.Message}");
                }
            }
        }
        public class FacturaResponse
        {
            public string error { get; set; }
            public List<string> errores { get; set; }
            public string rta { get; set; }
            public string cae { get; set; }
            public string requiere_fec { get; set; }
            public string vencimiento_cae { get; set; }
            public string vencimiento_pago { get; set; }
            public string comprobante_pdf_url { get; set; }
            public string comprobante_ticket_url { get; set; }
            public string afip_qr { get; set; }
            public string afip_codigo_barras { get; set; }
            public string envio_x_mail { get; set; }
            public string external_reference { get; set; }
            public string comprobante_nro { get; set; }
            public string comprobante_tipo { get; set; }
            public Micrositios micrositios { get; set; }
        }
        public class Micrositios
        {
            public string cliente { get; set; }
            public string descarga { get; set; }
        }
        public class ErrorDetail
        {
            public string code { get; set; }
            public string text { get; set; }
        }

        public async Task<bool> emitirNotaCredito(long unaDevolucion, int FacturaAsociada, string fechaFacturaAsociada, decimal? unImporte, int? unCliente, decimal? IVA, decimal? iibb)
        {
            try
            {
                ClassVentas instVentas = new ClassVentas();
                if (string.IsNullOrEmpty(userToken) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiToken)) return false;
                if (puntoVenta == 0) return false;

                if (unaDevolucion > 0)
                {
                    DataTable cabeceraDt = instVentas.TraerCabeceraNC(unaDevolucion);
                    if (cabeceraDt.Rows.Count == 0) return false;
                    DataRow cabecera = cabeceraDt.Rows[0];

                    Cliente cliente = CrearCliente(cabecera, null);

                    string claveNotaCredito = $"Tipo_Comprobante_NC{cabecera["letra"].ToString().ToUpper()}";
                    string tipoComprobante = Resource.ResourceManager.GetString(claveNotaCredito);

                    string claveComprobanteAsoc = $"Tipo_Comprobante_F{cabecera["letra"].ToString().ToUpper()}";
                    string tipoComprobanteAsoc = Resource.ResourceManager.GetString(claveComprobanteAsoc);

                    // Mismo comprobante asociado (la factura original) para todos los lotes que se emitan.
                    var comprobanteAsociado = CrearComprobanteAsociado(
                        tipoComprobanteAsoc,
                        FacturaAsociada,
                        fechaFacturaAsociada,
                        long.Parse(cabecera["cuil"].ToString())
                    );

                    DataTable detalle = instVentas.TraerDetalleNC(unaDevolucion);
                    if (detalle.Rows.Count == 0) return false;

                    // ── División en lotes: la API rechaza comprobantes con más de 130 líneas ──
                    int tamanioLote = CalcularTamanioLote(detalle);
                    var lotes = DividirEnLotes(detalle, tamanioLote);

                    var comprobantesEmitidos = new List<(string numero, string pdf)>();
                    string errorParte = null;

                    for (int i = 0; i < lotes.Count; i++)
                    {
                        var (detalleLote, neto, ivaTotal) = ConstruirDetalleLote(lotes[i], cabecera);
                        var (bonificacionGeneral, ivaConGeneral, tributos, totalFinal) = CalcularAgregadosLote(neto, ivaTotal, cabecera);

                        FacturaRequest facturaRequest = new FacturaRequest();
                        facturaRequest.usertoken = this.userToken;
                        facturaRequest.apikey = this.apiKey;
                        facturaRequest.apitoken = this.apiToken;
                        facturaRequest.cliente = cliente;
                        facturaRequest.comprobante = CrearComprobante(tipoComprobante);
                        facturaRequest.comprobante.comprobantes_asociados = new List<ComprobanteAsociado> { comprobanteAsociado };
                        facturaRequest.comprobante.detalle = detalleLote;
                        facturaRequest.comprobante.bonificacion = bonificacionGeneral;
                        facturaRequest.comprobante.tributos = tributos;
                        facturaRequest.comprobante.total = totalFinal;

                        FacturaResponse respuesta = await emitirConReintentos(facturaRequest, unaDevolucion);
                        if (respuesta == null)
                        {
                            // Un CAE ya emitido no se puede anular: se conservan los lotes ya emitidos
                            // y se detiene el proceso, sin reintentar automáticamente el resto.
                            errorParte = $"Se emitieron {comprobantesEmitidos.Count} de {lotes.Count} notas de crédito. Falló la parte {i + 1}.";
                            break;
                        }

                        // ✅ ÉXITO → guardar este comprobante inmediatamente (no esperar a los demás lotes)
                        Fiscal unTk = new Fiscal();

                        string cae = respuesta.cae.Trim();
                        string vencimientoCAE = respuesta.vencimiento_cae;
                        string numero = respuesta.comprobante_nro;
                        string pdf = respuesta.comprobante_pdf_url;
                        string qr = respuesta.afip_qr;

                        ComprobanteFiscal unComprobante = new ComprobanteFiscal
                        {
                            TipoComprobante = "Nota de Crédito",
                            Letra = cabecera["letra"].ToString(),
                            PuntoVenta = puntoVenta,
                            Numero = numero.Split('-')[1].TrimStart('0'),
                            FechaEmision = DateTime.Now,
                            CreatedAt = DateTime.Now,
                            NroReferencia = int.Parse(unaDevolucion.ToString()),
                            FkCliente = int.Parse(cabecera["Cliente"].ToString()),
                            RazonSocial = cabecera["razonSocial"].ToString(),
                            Cuit = cabecera["Cliente"].ToString() == clienteConsumidorFinal.ToString()
                                        ? "99999999"
                                        : cabecera["cuil"].ToString(),
                            ImporteTotal = totalFinal,
                            Estado = "Emitido",
                            Cae = cae,
                            FechaVencimientoCae = DateTime.ParseExact(vencimientoCAE, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            urlComprobante = pdf,
                            qrAfip = qr
                        };

                        unTk.almacenarComprobanteFiscal(unComprobante);
                        comprobantesEmitidos.Add((numero, pdf));
                    }

                    foreach (var c in comprobantesEmitidos)
                    {
                        System.Diagnostics.Process.Start(new ProcessStartInfo
                        {
                            FileName = c.pdf,
                            UseShellExecute = true
                        });
                    }

                    // Con más de 1 lote se informa un resumen; con 1 solo lote se preserva el
                    // comportamiento anterior (sin mensaje extra en éxito).
                    if (lotes.Count > 1)
                    {
                        string resumen = errorParte == null
                            ? $"Se generaron {comprobantesEmitidos.Count} notas de crédito:\n" + string.Join("\n", comprobantesEmitidos.ConvertAll(c => c.numero))
                            : errorParte + (comprobantesEmitidos.Count > 0
                                ? "\nComprobantes ya emitidos:\n" + string.Join("\n", comprobantesEmitidos.ConvertAll(c => c.numero))
                                : string.Empty);

                        MessageBox.Show(resumen, "FACTURACION", MessageBoxButton.OK,
                            errorParte == null ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    }

                    return errorParte == null;
                }
                else
                {
                    decimal totalSinIVACalculado = Math.Round(
                        (unImporte ?? 0) /
                        (
                            1 +
                            ((IVA ?? 0) / 100m) +
                            ((iibb ?? 0) / 100m)
                        ),
                        2
                    );

                    FacturaRequest facturaRequest = new FacturaRequest();
                    facturaRequest.cliente = new Cliente();
                    facturaRequest.comprobante = new Comprobante();
                    facturaRequest.comprobante.comprobantes_asociados = new List<ComprobanteAsociado>();

                    facturaRequest.usertoken = this.userToken;
                    facturaRequest.apikey = this.apiKey;
                    facturaRequest.apitoken = this.apiToken;

                    ClassClientes instClie = new ClassClientes();
                    DataTable Cliente = instClie.traerDatosFiscales(unCliente ?? 0);
                    if (Cliente.Rows.Count == 0) return false;

                    //--------Cliente (reutiliza CrearCliente y sobrescribe los campos propios de NC sin venta)------------------
                    // CrearCliente espera columna "Cliente" como id del cliente; el DataTable de traerDatosFiscales la trae igual.
                    // Validación de provincia previa para mantener el return false ante valor inválido.
                    if (!Enum.TryParse(Cliente.Rows[0]["Provincia"].ToString().Replace(" ", "_"), out ProvinciasEnum _provNC)) return false;

                    facturaRequest.cliente = CrearCliente(Cliente.Rows[0], null);
                    // En NC sin venta original se usa condicion_pago "201" y sin condicion_pago_otra
                    // (CrearCliente setea "214" y "Sin Especificar" cuando unaVenta == null).
                    facturaRequest.cliente.condicion_pago      = "201";
                    facturaRequest.cliente.condicion_pago_otra = null;

                    //-------------------Comprobante-------------------------

                    string claveNotaCredito = $"Tipo_Comprobante_NC{Cliente.Rows[0]["letra"].ToString().ToUpper()}";
                    string tipoComprobante = Resource.ResourceManager.GetString(claveNotaCredito);

                    string claveComprobanteAsoc = $"Tipo_Comprobante_F{Cliente.Rows[0]["letra"].ToString().ToUpper()}";
                    string tipoComprobanteAsoc = Resource.ResourceManager.GetString(claveComprobanteAsoc);

                    facturaRequest.comprobante = CrearComprobante(tipoComprobante);
                    facturaRequest.comprobante.comprobantes_asociados = new List<ComprobanteAsociado>();
                    var unCOmprobanteAsociado = CrearComprobanteAsociado(tipoComprobanteAsoc, FacturaAsociada, fechaFacturaAsociada, long.Parse(Cliente.Rows[0]["cuil"].ToString()));
                    facturaRequest.comprobante.comprobantes_asociados.Add(unCOmprobanteAsociado);

                    //-------------------Detalle Comprobante-----------------------


                    facturaRequest.comprobante.detalle = new List<DetalleFactura>();
                    DetalleFactura item = new DetalleFactura
                    {

                        cantidad = 1,
                        afecta_stock = "S",
                        producto = new Producto
                        {
                            descripcion = "Productos Varios",
                            unidad_bulto = 1,
                            lista_precios = "Lista de Precios",
                            codigo = "1",
                            precio_unitario_sin_iva = totalSinIVACalculado,
                            alicuota = IVA == 00 ? 21 : IVA ?? 0,
                            unidad_medida = 7,
                            actualiza_precio = "N",
                            rg5329 = "N"
                        }
                    };

                    facturaRequest.comprobante.detalle.Add(item);

                    if (iibb > 0)
                    {
                        facturaRequest.comprobante.tributos = new List<Tributo>();
                        Tributo unTributo = new Tributo
                        {
                            tipo = tributoIIBB,
                            regimen = regimenIIBB,
                            base_imponible = totalSinIVACalculado,
                            alicuota = iibb ?? 0,
                            total = Math.Round(
                                totalSinIVACalculado * ((iibb ?? 0) / 100m),
                                2
                            )
                        };

                        facturaRequest.comprobante.tributos.Add(unTributo);
                    }

                    FacturaResponse respuesta = await emitirConReintentos(facturaRequest, unaDevolucion);
                    if (respuesta == null) return false;

                    // ✅ ÉXITO → guardar comprobante
                    Fiscal unTk = new Fiscal();

                    string cae = respuesta.cae.Trim();
                    string vencimientoCAE = respuesta.vencimiento_cae;
                    string numero = respuesta.comprobante_nro;
                    string pdf = respuesta.comprobante_pdf_url;
                    string qr = respuesta.afip_qr;

                    ComprobanteFiscal unComprobante = new ComprobanteFiscal
                    {
                        TipoComprobante = "Nota de Crédito",
                        Letra = Cliente.Rows[0]["letra"].ToString(),
                        PuntoVenta = puntoVenta,
                        Numero = numero.Split('-')[1].TrimStart('0'),
                        FechaEmision = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        NroReferencia = int.Parse(unaDevolucion.ToString()),
                        FkCliente = int.Parse(Cliente.Rows[0]["Cliente"].ToString()),
                        RazonSocial = Cliente.Rows[0]["razonSocial"].ToString(),
                        Cuit = Cliente.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString()
                                    ? "99999999"
                                    : Cliente.Rows[0]["cuil"].ToString(),
                        ImporteTotal = unImporte ?? 0,
                        Estado = "Emitido",
                        Cae = cae,
                        FechaVencimientoCae = DateTime.ParseExact(vencimientoCAE, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        urlComprobante = pdf,
                        qrAfip = qr
                    };

                    unTk.almacenarComprobanteFiscal(unComprobante);

                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = pdf,
                        UseShellExecute = true
                    });

                    return true;
                }
            }
            catch (Exception u)
            {
                return false;
            }
        }

        public async Task<bool> emitirFacturaElectronica(long unaVenta)
        {
            try
            {
                ClassVentas instVentas = new ClassVentas();

                if (string.IsNullOrEmpty(userToken) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiToken)) return false;
                if (puntoVenta == 0) return false;

                DataTable cabeceraDt = instVentas.TraerCabeceraFactura(unaVenta);
                if (cabeceraDt.Rows.Count == 0) return false;
                DataRow cabecera = cabeceraDt.Rows[0];

                Cliente cliente = CrearCliente(cabecera, unaVenta);

                string claveFactura = $"Tipo_Comprobante_F{cabecera["letra"].ToString().ToUpper()}";
                string tipoComprobante = Resource.ResourceManager.GetString(claveFactura);

                DataTable detalle = instVentas.TraerDetalleFactura(unaVenta);
                if (detalle.Rows.Count == 0) return false;

                // ── División en lotes: la API rechaza comprobantes con más de 130 líneas ──
                int tamanioLote = CalcularTamanioLote(detalle);
                var lotes = DividirEnLotes(detalle, tamanioLote);

                var comprobantesEmitidos = new List<(string numero, string pdf)>();
                string errorParte = null;

                for (int i = 0; i < lotes.Count; i++)
                {
                    var (detalleLote, neto, ivaTotal) = ConstruirDetalleLote(lotes[i], cabecera);
                    var (bonificacionGeneral, ivaConGeneral, tributos, totalFinal) = CalcularAgregadosLote(neto, ivaTotal, cabecera);

                    FacturaRequest facturaRequest = new FacturaRequest();
                    facturaRequest.usertoken = this.userToken;
                    facturaRequest.apikey = this.apiKey;
                    facturaRequest.apitoken = this.apiToken;
                    facturaRequest.cliente = cliente;
                    facturaRequest.comprobante = CrearComprobante(tipoComprobante);
                    facturaRequest.comprobante.detalle = detalleLote;
                    facturaRequest.comprobante.bonificacion = bonificacionGeneral;
                    facturaRequest.comprobante.tributos = tributos;
                    facturaRequest.comprobante.total = totalFinal;

                    FacturaResponse respuesta = await emitirConReintentos(facturaRequest, unaVenta, mostrarMensajeError: true);
                    if (respuesta == null)
                    {
                        // Un CAE ya emitido no se puede anular: se conservan los lotes ya emitidos
                        // y se detiene el proceso, sin reintentar automáticamente el resto.
                        errorParte = $"Se emitieron {comprobantesEmitidos.Count} de {lotes.Count} comprobantes. Falló la parte {i + 1}.";
                        break;
                    }

                    // ✅ ÉXITO → guardar este comprobante inmediatamente (no esperar a los demás lotes)
                    Fiscal unTk = new Fiscal();

                    string cae = respuesta.cae.Trim();
                    string vencimientoCAE = respuesta.vencimiento_cae;
                    string numero = respuesta.comprobante_nro;
                    string pdf = respuesta.comprobante_pdf_url;
                    string qr = respuesta.afip_qr;

                    ComprobanteFiscal unComprobante = new ComprobanteFiscal
                    {
                        TipoComprobante = "Factura",
                        Letra = cabecera["letra"].ToString(),
                        PuntoVenta = puntoVenta,
                        Numero = numero.Split('-')[1].TrimStart('0'),
                        FechaEmision = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        NroReferencia = int.Parse(unaVenta.ToString()),
                        FkCliente = int.Parse(cabecera["Cliente"].ToString()),
                        RazonSocial = cabecera["razonSocial"].ToString(),
                        Cuit = cabecera["Cliente"].ToString() == clienteConsumidorFinal.ToString()
                                    ? "99999999"
                                    : cabecera["cuil"].ToString(),
                        // Con un único comprobante se preserva exactamente el valor histórico
                        // (totalVenta de cabecera); con varios, cada fila lleva el total de SU parte.
                        ImporteTotal = lotes.Count == 1 ? decimal.Parse(cabecera["totalVenta"].ToString()) : totalFinal,
                        Estado = "Emitido",
                        Cae = cae,
                        FechaVencimientoCae = DateTime.ParseExact(vencimientoCAE, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        urlComprobante = pdf,
                        qrAfip = qr
                    };

                    unTk.almacenarComprobanteFiscal(unComprobante);
                    comprobantesEmitidos.Add((numero, pdf));
                }

                foreach (var c in comprobantesEmitidos)
                {
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = c.pdf,
                        UseShellExecute = true
                    });
                }

                // Con más de 1 lote se informa un resumen; con 1 solo lote se preserva el
                // comportamiento anterior (sin mensaje extra en éxito; en error ya avisó emitirConReintentos).
                if (lotes.Count > 1)
                {
                    string resumen = errorParte == null
                        ? $"Se generaron {comprobantesEmitidos.Count} comprobantes:\n" + string.Join("\n", comprobantesEmitidos.ConvertAll(c => c.numero))
                        : errorParte + (comprobantesEmitidos.Count > 0
                            ? "\nComprobantes ya emitidos:\n" + string.Join("\n", comprobantesEmitidos.ConvertAll(c => c.numero))
                            : string.Empty);

                    MessageBox.Show(resumen, "FACTURACION", MessageBoxButton.OK,
                        errorParte == null ? MessageBoxImage.Information : MessageBoxImage.Warning);
                }

                return errorParte == null;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Emite el comprobante con hasta 2 intentos. Si AFIP/API responde
        /// "sumatorias finales (...)" o "El total a enviar a AFIP (...)",
        /// se extrae el total correcto vía <see cref="ObtenerTotalDesdeError"/>
        /// y se reintenta una sola vez. En cualquier error definitivo se
        /// registra con <see cref="Fiscal.AddErrorFE"/> y retorna null.
        /// </summary>
        /// <param name="facturaRequest">payload a enviar (se ajusta el total en el reintento)</param>
        /// <param name="idReferencia">id de venta o devolución, usado al registrar errores</param>
        /// <param name="mostrarMensajeError">si true, muestra MessageBox con el detalle del error definitivo</param>
        /// <returns>FacturaResponse en caso de éxito; null si falló definitivamente</returns>
        private async Task<FacturaResponse> emitirConReintentos(
            FacturaRequest facturaRequest,
            long idReferencia,
            bool mostrarMensajeError = false)
        {
            for (int intento = 0; intento < 2; intento++)
            {
                var (ok, jsonRespuesta, error) = await emitirComprobante(facturaRequest);

                if (!ok || string.IsNullOrEmpty(jsonRespuesta))
                    return null;

                FacturaResponse respuesta = JsonConvert.DeserializeObject<FacturaResponse>(jsonRespuesta);

                if (respuesta.error == "N")
                    return respuesta;

                string errores = string.Join(" | ", respuesta.errores);

                // 🔁 reintento sólo en el primer intento ante diferencias de totales
                if (intento == 0 &&
                    (errores.Contains("sumatorias finales") ||
                     errores.Contains("El total a enviar a AFIP")))
                {
                    decimal? totalCorrecto = ObtenerTotalDesdeError(errores);
                    if (totalCorrecto.HasValue)
                    {
                        facturaRequest.comprobante.total = totalCorrecto.Value;
                        continue;
                    }
                }

                // ❌ error definitivo
                new Fiscal().AddErrorFE(idReferencia, errores);
                if (mostrarMensajeError)
                    MessageBox.Show(errores, "FACTURACION", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return null;
        }

        private decimal? ObtenerTotalDesdeError(string error)
        {
            try
            {
                if (string.IsNullOrEmpty(error))
                    return null;

                // 🔹 CASO 1: sumatorias finales (YA EXISTENTE)
                string clave1 = "sumatorias finales (";
                int index1 = error.IndexOf(clave1);

                if (index1 != -1)
                {
                    int inicioNumero = index1 + clave1.Length;
                    int finNumero = error.IndexOf(")", inicioNumero);

                    if (finNumero != -1)
                    {
                        string numeroStr = error.Substring(inicioNumero, finNumero - inicioNumero).Trim();

                        if (decimal.TryParse(numeroStr, System.Globalization.NumberStyles.Any,
                                             System.Globalization.CultureInfo.InvariantCulture,
                                             out decimal total))
                        {
                            return total;
                        }
                    }
                }

                // 🔹 CASO 2: El total a enviar a AFIP (NUEVO)
                string clave2 = "El total a enviar a AFIP (";
                int index2 = error.IndexOf(clave2);

                if (index2 != -1)
                {
                    int inicioNumero = index2 + clave2.Length;
                    int finNumero = error.IndexOf(")", inicioNumero);

                    if (finNumero != -1)
                    {
                        string numeroStr = error.Substring(inicioNumero, finNumero - inicioNumero).Trim();

                        if (decimal.TryParse(numeroStr, System.Globalization.NumberStyles.Any,
                                             System.Globalization.CultureInfo.InvariantCulture,
                                             out decimal total))
                        {
                            return total;
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        // La API (tusfacturas.app) rechaza comprobantes con más de 130 líneas de detalle.
        // Ventas/devoluciones más grandes se dividen en varios comprobantes (ver EmitirVentaOdevolucion*).
        private const int MAX_DETALLES_POR_COMPROBANTE = 130;

        /// <summary>
        /// Divide las filas de detalle en lotes de a lo sumo <paramref name="tamanioLote"/> filas,
        /// preservando el orden. Cada lote se convierte luego en un comprobante independiente.
        /// </summary>
        private static List<List<DataRow>> DividirEnLotes(DataTable detalle, int tamanioLote)
        {
            var lotes = new List<List<DataRow>>();
            var filas = detalle.AsEnumerable().ToList();
            for (int i = 0; i < filas.Count; i += tamanioLote)
                lotes.Add(filas.Skip(i).Take(tamanioLote).ToList());
            return lotes;
        }

        /// <summary>
        /// Tamaño de lote a usar para no superar nunca las 130 líneas por comprobante.
        /// Si algún renglón de la operación tiene recargo, se reserva 1 lugar (129 productos)
        /// porque cada lote con recargo agrega una línea sintética "Recargo" adicional.
        /// </summary>
        private static int CalcularTamanioLote(DataTable detalle)
        {
            bool hayAlgunRecargo = detalle.AsEnumerable().Any(f => (decimal)f["recargo"] > 0);
            return hayAlgunRecargo ? MAX_DETALLES_POR_COMPROBANTE - 1 : MAX_DETALLES_POR_COMPROBANTE;
        }

        /// <summary>
        /// Construye el detalle de UN comprobante (lote) a partir de sus filas y acumula neto/IVA.
        /// Misma lógica de línea que ya existía (precio, bonificación por línea, IVA, recargo),
        /// aplicada solo sobre las filas del lote — compartida entre Factura y Nota de Crédito.
        /// </summary>
        private (List<DetalleFactura> detalle, decimal neto, decimal ivaTotal) ConstruirDetalleLote(List<DataRow> filasLote, DataRow cabecera)
        {
            decimal neto = 0m;
            decimal ivaTotal = 0m;
            decimal recargoTotal = 0m;
            decimal ivaCab = (decimal)cabecera["IVA"];

            var detalleLote = new List<DetalleFactura>();

            foreach (DataRow fila in filasLote)
            {
                decimal cantidad = Math.Round((decimal)fila["cantidad"], 2);
                decimal precioBase = (decimal)fila["precioSinIva"];

                decimal precioUnitario = Math.Round(
                    ivaCab == 0 ? precioBase / 1.21m : precioBase
                , 3);

                decimal bonif = Math.Round((decimal)fila["descuento"], 2);
                decimal alicuota = ivaCab == 0 ? 21 : ivaCab;

                decimal subtotal = precioUnitario * cantidad;

                if (bonif > 0)
                    subtotal -= subtotal * (bonif / 100m);

                neto += subtotal;

                decimal ivaLinea = Math.Round(subtotal * (alicuota / 100m), 2);
                ivaTotal += ivaLinea;

                if ((decimal)fila["recargo"] > 0)
                {
                    decimal recargoLinea = subtotal * ((decimal)fila["recargo"] / 100m);
                    recargoTotal += recargoLinea;
                }

                detalleLote.Add(new DetalleFactura
                {
                    cantidad = cantidad,
                    afecta_stock = "S",
                    bonificacion_porcentaje = bonif,
                    producto = new Producto
                    {
                        descripcion = fila["descripcion"].ToString(),
                        unidad_bulto = 1,
                        lista_precios = "Lista de Precios",
                        codigo = codigoDetalle == "CodProveedor"
                                    ? fila["codProveedor"].ToString()
                                    : codigoDetalle == "CodBarras"
                                        ? fila["codBarras"].ToString()
                                        : fila["Producto"].ToString(),
                        precio_unitario_sin_iva = precioUnitario,
                        alicuota = alicuota,
                        unidad_medida = 7,
                        actualiza_precio = "N",
                        rg5329 = "N"
                    }
                });
            }

            if (recargoTotal > 0)
            {
                decimal recargoRedondeado = Math.Round(recargoTotal, 3);
                neto += recargoTotal;

                detalleLote.Add(new DetalleFactura
                {
                    cantidad = 1,
                    afecta_stock = "S",
                    producto = new Producto
                    {
                        descripcion = "Recargo",
                        unidad_bulto = 1,
                        lista_precios = "Lista de Precios",
                        codigo = "1",
                        precio_unitario_sin_iva = recargoRedondeado,
                        alicuota = ivaCab == 0 ? 21 : ivaCab,
                        unidad_medida = 7,
                        actualiza_precio = "N",
                        rg5329 = "N"
                    }
                });
            }

            return (detalleLote, neto, ivaTotal);
        }

        /// <summary>
        /// Calcula, para UN lote, la bonificación general (misma % de la cabecera aplicada sobre el
        /// neto del lote), el IVA ajustado, los tributos (IIBB) y el total del comprobante.
        /// Misma fórmula que ya existía para el comprobante completo, aplicada por lote.
        /// </summary>
        private (decimal bonificacionGeneral, decimal ivaConGeneral, List<Tributo> tributos, decimal total) CalcularAgregadosLote(decimal neto, decimal ivaTotal, DataRow cabecera)
        {
            decimal ivaCab = (decimal)cabecera["IVA"];
            decimal descGeneralPct = cabecera.Table.Columns.Contains("descuento") && cabecera["descuento"] != DBNull.Value
                ? (decimal)cabecera["descuento"] : 0m;
            decimal alicuotaGeneral = ivaCab == 0 ? 21 : ivaCab;
            decimal bonificacionGeneral = descGeneralPct > 0 ? Math.Round(neto * (descGeneralPct / 100m), 2) : 0m;
            decimal netoConGeneral = neto - bonificacionGeneral;

            List<Tributo> tributos = null;
            decimal totalTributos = 0m;

            if ((decimal)cabecera["impuesto"] > 0)
            {
                decimal alicuotaTributo = Math.Round((decimal)cabecera["impuesto"], 2);
                decimal totalTributo = Math.Round(netoConGeneral * (alicuotaTributo / 100m), 2);

                totalTributos = totalTributo;

                tributos = new List<Tributo>
                {
                    new Tributo
                    {
                        tipo = tributoIIBB,
                        regimen = regimenIIBB,
                        base_imponible = netoConGeneral,
                        alicuota = alicuotaTributo,
                        total = totalTributo
                    }
                };
            }

            decimal ivaConGeneral = ivaTotal - Math.Round(bonificacionGeneral * (alicuotaGeneral / 100m), 2);
            decimal total = Math.Round(netoConGeneral + ivaConGeneral + totalTributos, 2);

            return (bonificacionGeneral, ivaConGeneral, tributos, total);
        }

        private ComprobanteAsociado CrearComprobanteAsociado(string tipoComp, int numeroAsoc, string fechaAsociado, long cuitAsoc)
        {
            return new ComprobanteAsociado
            {
                tipo_comprobante = tipoComp,
                punto_venta = puntoVenta,
                numero = numeroAsoc,
                cuit = cuitAsoc,
                comprobante_fecha = fechaAsociado
            };
        }
        private Cliente CrearCliente(DataRow row, long? unaVenta)
        {
            bool esConsumidorFinal = row["Cliente"].ToString() == clienteConsumidorFinal.ToString();
            Clases.ClassVentas instVentas = new Clases.ClassVentas();

            if (!Enum.TryParse(row["Provincia"].ToString().Replace(" ", "_"), out ProvinciasEnum provincia))
                throw new Exception("Provincia inválida");

            return new Cliente
            {
                documento_tipo = esConsumidorFinal ? "DNI" : "CUIT",
                documento_nro = esConsumidorFinal ? "99999999" : row["cuil"].ToString(),
                razon_social = row["razonSocial"].ToString(),
                domicilio = row["Direccion"].ToString(),
                provincia = ((int)provincia).ToString(),
                codigo = "Clie" + (esConsumidorFinal ? "99999999" : row["cuil"].ToString()),
                envia_por_mail = enviarFacturaMail,
                email = enviarFacturaMail == "S" ? row["email"].ToString() : null,
                condicion_pago = "214",
                condicion_pago_otra = unaVenta == null ? "Sin Especificar" : instVentas.traerFormaPagoFacturas(unaVenta ?? 0),
                condicion_iva = row["abrevFE"].ToString(),
                rg5329 = "N"
            };
        }

        private Comprobante CrearComprobante(string tipo)
        {
            var now = DateTime.Now;
            string fecha = now.ToString("dd/MM/yyyy");

            return new Comprobante
            {
                fecha = fecha,
                tipo = tipo,
                operacion = "V",
                idioma = "1",
                punto_venta = puntoVenta.ToString(),
                moneda = "PES",
                cotizacion = "1",
                vencimiento = fecha,
                periodo_facturado_desde = fecha,
                periodo_facturado_hasta = fecha,
                rubro = rubroFE,
                rubro_grupo_contable = $"{now.Month}/{now.Year}"
            };
        }
    }
}
