using Comercial.Enums;
using Comercial.Properties;
using Comercial.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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

        public async Task<bool> emitirNotaCredito (long unaDevolucion, int FacturaAsociada, string fechaFacturaAsociada, decimal? unImporte, int? unCliente, decimal? IVA, decimal? iibb)
        {
            try
            {
                ClassVentas instVentas = new ClassVentas();
                if (string.IsNullOrEmpty(userToken) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiToken)) return false;
                if (puntoVenta == 0) return false;

                if (unaDevolucion > 0)
                {
                    DataTable cabecera = instVentas.TraerCabeceraNC(unaDevolucion);
                    if (cabecera.Rows.Count == 0) return false;
                    FacturaRequest facturaRequest = new FacturaRequest();
                    facturaRequest.cliente = new Cliente();
                    facturaRequest.comprobante = new Comprobante();
                    

                    facturaRequest.usertoken = this.userToken;
                    facturaRequest.apikey = this.apiKey;
                    facturaRequest.apitoken = this.apiToken;
                   
                    
                    facturaRequest.cliente = CrearCliente(cabecera.Rows[0]);

                    //-------------------Comprobante-------------------------

                    string claveNotaCredito = $"Tipo_Comprobante_NC{cabecera.Rows[0]["letra"].ToString().ToUpper()}";
                    string tipoComprobante = Resource.ResourceManager.GetString(claveNotaCredito);

                    string claveComprobanteAsoc = $"Tipo_Comprobante_F{cabecera.Rows[0]["letra"].ToString().ToUpper()}";
                    string tipoComprobanteAsoc = Resource.ResourceManager.GetString(claveComprobanteAsoc);

                    facturaRequest.comprobante = CrearComprobante(tipoComprobante, (decimal)cabecera.Rows[0]["totalDevolucion"]);
                    //facturaRequest.comprobante.bonificacion = Math.Round((decimal)cabecera.Rows[0]["TotalSInIva"] * ((decimal)cabecera.Rows[0]["descuento"] / 100), 2);
                    facturaRequest.comprobante.comprobantes_asociados = new List<ComprobanteAsociado>();
                    var unCOmprobanteAsociado = CrearComprobanteAsociado(tipoComprobanteAsoc, FacturaAsociada, fechaFacturaAsociada, long.Parse(cabecera.Rows[0]["cuil"].ToString()));
                    facturaRequest.comprobante.comprobantes_asociados.Add(unCOmprobanteAsociado);
                    //-------------------Detalle Comprobante-----------------------

                    DataTable detalle = instVentas.TraerDetalleNC(unaDevolucion);
                    if (detalle.Rows.Count == 0) return false;
                    facturaRequest.comprobante.detalle = new List<DetalleFactura>();
                    decimal recargoTotal = 0;
                    decimal totalSinIva = 0;
                    foreach (DataRow fila in detalle.Rows)
                    {
                        DetalleFactura item = new DetalleFactura
                        {
                            cantidad = Math.Round((decimal)(fila["cantidad"]), 2),
                            afecta_stock = "S",
                            bonificacion_porcentaje = Math.Round((decimal)fila["descuento"], 2),
                            producto = new Producto
                            {
                                descripcion = fila["descripcion"].ToString(),
                                unidad_bulto = 1,
                                lista_precios = "Lista de Precios",
                                codigo = fila["Producto"].ToString(),
                                precio_unitario_sin_iva = (decimal)cabecera.Rows[0]["IVA"] == 0 ? Math.Round((decimal)fila["precioSinIva"] / (decimal)1.21, 2) : (decimal)fila["precioSinIva"],
                                alicuota = (decimal)cabecera.Rows[0]["IVA"] == 00 ? 21 : (decimal)cabecera.Rows[0]["IVA"],
                                unidad_medida = 7,
                                actualiza_precio = "N",
                                rg5329 = "N"
                            }
                        };

                        if ((decimal)fila["recargo"] > 0) recargoTotal += ((decimal)fila["precioSinIva"] * ((decimal)fila["recargo"] / 100)) * (decimal)fila["cantidad"];

                        var lineaSinIva = (decimal)fila["subtotalSinIVA"] == 0 ? (decimal)cabecera.Rows[0]["TotalSInIva"] - ((decimal)cabecera.Rows[0]["TotalSInIva"] * ((decimal)cabecera.Rows[0]["descuento"] / 100)) + ((decimal)cabecera.Rows[0]["TotalSInIva"] * ((decimal)cabecera.Rows[0]["recargo"] / 100))
                                                                            : (decimal)fila["subtotalSinIVA"];

                        totalSinIva += (lineaSinIva * (decimal)fila["cantidad"]);                        

                        facturaRequest.comprobante.detalle.Add(item);
                    }

                    if (recargoTotal > 0)
                    {

                        DetalleFactura item = new DetalleFactura
                        {
                            cantidad = 1,
                            afecta_stock = "S",
                            producto = new Producto
                            {
                                descripcion = "Recargo",
                                unidad_bulto = 1,
                                lista_precios = "Lista de Precios",
                                codigo = "1",
                                precio_unitario_sin_iva = Math.Round(recargoTotal, 2),
                                alicuota = (decimal)cabecera.Rows[0]["IVA"] == 00 ? 21 : (decimal)cabecera.Rows[0]["IVA"],
                                unidad_medida = 7,
                                actualiza_precio = "N",
                                rg5329 = "N"
                            }
                        };

                        facturaRequest.comprobante.detalle.Add(item);
                    }


                    if ((decimal)cabecera.Rows[0]["impuesto"] > 0)
                    {                        
                        facturaRequest.comprobante.tributos = new List<Tributo>();
                        Tributo unTributo = new Tributo
                        {
                            tipo = tributoIIBB,
                            regimen = regimenIIBB,
                            base_imponible = totalSinIva,
                            alicuota = Math.Round((decimal)cabecera.Rows[0]["impuesto"], 2),
                            total = Math.Round(totalSinIva * ((decimal)cabecera.Rows[0]["impuesto"] / 100), 2)
                        };

                        facturaRequest.comprobante.tributos.Add(unTributo);                       
                    }

                   // var response = await emitirComprobante(facturaRequest);
                    var (ok, jsonRespuesta, error) = await emitirComprobante(facturaRequest);

                    if (!ok)
                    {
                        if (!string.IsNullOrEmpty(jsonRespuesta))
                        {
                            // error HTTP
                            string errorHttp = "Error API: " + jsonRespuesta;
                            return false;
                        }
                        else
                        {
                            // error técnico (timeout, red, etc.)
                            string errorHttp = "Error técnico: " + error;
                        }

                        return false;
                    }
                                       

                    FacturaResponse respuesta = JsonConvert.DeserializeObject<FacturaResponse>(jsonRespuesta);

                    Fiscal unTk = new Fiscal();

                    if (respuesta.error == "N")
                    {
                        string cae = respuesta.cae.Trim();
                        string vencimientoCAE = respuesta.vencimiento_cae;
                        string numero = respuesta.comprobante_nro;
                        string pdf = respuesta.comprobante_pdf_url;
                        string qr = respuesta.afip_qr;


                        ComprobanteFiscal unComprobante = new ComprobanteFiscal();

                        unComprobante.TipoComprobante = "Nota de Crédito";
                        unComprobante.Letra = cabecera.Rows[0]["letra"].ToString();
                        unComprobante.PuntoVenta = puntoVenta;
                        unComprobante.Numero = numero.Split('-')[1].TrimStart('0');
                        unComprobante.FechaEmision = DateTime.Now;
                        unComprobante.CreatedAt = DateTime.Now;
                        unComprobante.NroReferencia = int.Parse(unaDevolucion.ToString());
                        unComprobante.FkCliente = int.Parse(cabecera.Rows[0]["Cliente"].ToString());
                        unComprobante.RazonSocial = cabecera.Rows[0]["razonSocial"].ToString();
                        unComprobante.Cuit = cabecera.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "99999999" : cabecera.Rows[0]["cuil"].ToString();
                        unComprobante.ImporteTotal = decimal.Parse(cabecera.Rows[0]["totalDevolucion"].ToString());
                        unComprobante.Estado = "Emitido";
                        unComprobante.Cae = cae;
                        unComprobante.FechaVencimientoCae = DateTime.ParseExact(vencimientoCAE, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        unComprobante.urlComprobante = pdf;
                        unComprobante.qrAfip = qr;

                        // guardar en base de datos
                        unTk.almacenarComprobanteFiscal(unComprobante);

                        System.Diagnostics.Process.Start(new ProcessStartInfo
                        {
                            FileName = pdf,
                            UseShellExecute = true
                        });

                        return true;
                    }
                    else
                    {
                        string errores = string.Join(" | ", respuesta.errores);

                        // guardar error en log
                        unTk.AddErrorFE(unaDevolucion, errores);

                        return false;
                    }
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

                    //--------Cliente------------------
                    facturaRequest.cliente.documento_tipo = Cliente.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "DNI" : "CUIT";
                    facturaRequest.cliente.documento_nro = Cliente.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "99999999" : Cliente.Rows[0]["cuil"].ToString();
                    facturaRequest.cliente.razon_social = Cliente.Rows[0]["razonSocial"].ToString();
                    facturaRequest.cliente.domicilio = Cliente.Rows[0]["Direccion"].ToString();
                    var provinciaDb = Cliente.Rows[0]["Provincia"].ToString();

                    ProvinciasEnum provincia;

                    if (!Enum.TryParse(provinciaDb.Replace(" ", "_"), out provincia)) return false;

                    var codProvincia = (int)provincia;
                    facturaRequest.cliente.provincia = codProvincia.ToString();

                    facturaRequest.cliente.codigo = "Clie" + Cliente.Rows[0]["Cliente"].ToString();
                    facturaRequest.cliente.envia_por_mail = enviarFacturaMail;
                    if (enviarFacturaMail == "S") facturaRequest.cliente.email = Cliente.Rows[0]["email"].ToString();
                    facturaRequest.cliente.condicion_pago = "201";
                    facturaRequest.cliente.condicion_iva = Cliente.Rows[0]["abrevFE"].ToString();
                    facturaRequest.cliente.rg5329 = "N";

                    //-------------------Comprobante-------------------------

                    string claveNotaCredito = $"Tipo_Comprobante_NC{Cliente.Rows[0]["letra"].ToString().ToUpper()}";
                    string tipoComprobante = Resource.ResourceManager.GetString(claveNotaCredito);

                    string claveComprobanteAsoc = $"Tipo_Comprobante_F{Cliente.Rows[0]["letra"].ToString().ToUpper()}";
                    string tipoComprobanteAsoc = Resource.ResourceManager.GetString(claveComprobanteAsoc);

                    facturaRequest.comprobante = CrearComprobante(tipoComprobante, unImporte ?? 0);                    
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

                    var (ok, jsonRespuesta, error) = await emitirComprobante(facturaRequest);

                    if (!ok)
                    {
                        if (!string.IsNullOrEmpty(jsonRespuesta))
                        {
                            // error HTTP
                            string errorHttp = "Error API: " + jsonRespuesta;
                            return false;
                        }
                        else
                        {
                            // error técnico (timeout, red, etc.)
                            string errorHttp = "Error técnico: " + error;
                        }

                        return false;
                    }

                    FacturaResponse respuesta = JsonConvert.DeserializeObject<FacturaResponse>(jsonRespuesta);

                    Fiscal unTk = new Fiscal();

                    if (respuesta.error == "N")
                    {
                        string cae = respuesta.cae.Trim();
                        string vencimientoCAE = respuesta.vencimiento_cae;
                        string numero = respuesta.comprobante_nro;
                        string pdf = respuesta.comprobante_pdf_url;
                        string qr = respuesta.afip_qr;


                        ComprobanteFiscal unComprobante = new ComprobanteFiscal();

                        unComprobante.TipoComprobante = "Nota de Crédito";
                        unComprobante.Letra = Cliente.Rows[0]["letra"].ToString();
                        unComprobante.PuntoVenta = puntoVenta;
                        unComprobante.Numero = numero.Split('-')[1].TrimStart('0');
                        unComprobante.FechaEmision = DateTime.Now;
                        unComprobante.CreatedAt = DateTime.Now;
                        unComprobante.NroReferencia = int.Parse(unaDevolucion.ToString());
                        unComprobante.FkCliente = int.Parse(Cliente.Rows[0]["Cliente"].ToString());
                        unComprobante.RazonSocial = Cliente.Rows[0]["razonSocial"].ToString();
                        unComprobante.Cuit = Cliente.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "99999999" : Cliente.Rows[0]["cuil"].ToString();
                        unComprobante.ImporteTotal = unImporte ?? 0;
                        unComprobante.Estado = "Emitido";
                        unComprobante.Cae = cae;
                        unComprobante.FechaVencimientoCae = DateTime.ParseExact(vencimientoCAE, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        unComprobante.urlComprobante = pdf;
                        unComprobante.qrAfip = qr;

                        // guardar en base de datos
                        unTk.almacenarComprobanteFiscal(unComprobante);

                        System.Diagnostics.Process.Start(new ProcessStartInfo
                        {
                            FileName = pdf,
                            UseShellExecute = true
                        });

                        return true;

                    }

                    else
                    {
                        string errores = string.Join(" | ", respuesta.errores);

                        // guardar error en log
                        unTk.AddErrorFE(unaDevolucion, errores);

                        return false;
                    }
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

                DataTable cabecera = instVentas.TraerCabeceraFactura(unaVenta);

                if (cabecera.Rows.Count == 0) return false;

                FacturaRequest facturaRequest = new FacturaRequest();
                facturaRequest.cliente = new Cliente();
                facturaRequest.comprobante = new Comprobante();

                facturaRequest.usertoken = this.userToken;
                facturaRequest.apikey = this.apiKey;
                facturaRequest.apitoken = this.apiToken;

                //--------Cliente------------------                

                facturaRequest.cliente = CrearCliente(cabecera.Rows[0]);

                //-------------------Comprobante-------------------------

                string claveFactura = $"Tipo_Comprobante_F{cabecera.Rows[0]["letra"].ToString().ToUpper()}";
                string tipoComprobante = Resource.ResourceManager.GetString(claveFactura);
                facturaRequest.comprobante = CrearComprobante(tipoComprobante, (decimal)cabecera.Rows[0]["totalVenta"]);
               // facturaRequest.comprobante.bonificacion = Math.Round((decimal)cabecera.Rows[0]["TotalSInIva"] * ((decimal)cabecera.Rows[0]["descuento"]/100), 2);
                //-------------------Detalle Comprobante-----------------------

                DataTable detalle = instVentas.TraerDetalleFactura(unaVenta);
                if (detalle.Rows.Count == 0) return false;
                facturaRequest.comprobante.detalle = new List<DetalleFactura>();
                decimal recargoTotal = 0;
                decimal totalSinIva = 0;
                foreach (DataRow fila in detalle.Rows)
                {
                    DetalleFactura item = new DetalleFactura
                    {
                        cantidad = Math.Round((decimal)(fila["cantidad"]), 2),
                        afecta_stock = "S",
                        bonificacion_porcentaje = Math.Round((decimal)fila["descuento"],2),
                        producto = new Producto
                        {
                            descripcion = fila["descripcion"].ToString(),
                            unidad_bulto = 1,
                            lista_precios = "Lista de Precios",
                            codigo = fila["Producto"].ToString(),
                            precio_unitario_sin_iva = (decimal)cabecera.Rows[0]["IVA"] == 0 ? Math.Round((decimal)fila["precioSinIva"] / (decimal)1.21, 2) : (decimal)fila["precioSinIva"],
                            alicuota = (decimal)cabecera.Rows[0]["IVA"] == 00 ? 21 : (decimal)cabecera.Rows[0]["IVA"],
                            unidad_medida = 7,
                            actualiza_precio = "N",
                            rg5329 = "N"
                        }
                    };

                    if ((decimal)fila["recargo"] > 0) recargoTotal += ((decimal)fila["precioSinIva"] * ((decimal)fila["recargo"] / 100)) * (decimal)fila["cantidad"];

                    var lineaSinIva = (decimal)fila["subtotalSinIVA"] == 0 ? (decimal)cabecera.Rows[0]["TotalSInIva"] - ((decimal)cabecera.Rows[0]["TotalSInIva"] * ((decimal)cabecera.Rows[0]["descuento"] / 100)) + ((decimal)cabecera.Rows[0]["TotalSInIva"] * ((decimal)cabecera.Rows[0]["recargo"] / 100))
                                                                        : (decimal)fila["subtotalSinIVA"];

                    totalSinIva += (lineaSinIva * (decimal)fila["cantidad"]);

                    facturaRequest.comprobante.detalle.Add(item);
                }

                if (recargoTotal > 0)
                {                   

                    DetalleFactura item = new DetalleFactura
                    {
                        cantidad = 1,
                        afecta_stock = "S",
                        producto = new Producto
                        {
                            descripcion = "Recargo",
                            unidad_bulto = 1,
                            lista_precios = "Lista de Precios",
                            codigo = "1",
                            precio_unitario_sin_iva = Math.Round(recargoTotal, 2),
                            alicuota = (decimal)cabecera.Rows[0]["IVA"] == 00 ? 21 : (decimal)cabecera.Rows[0]["IVA"],
                            unidad_medida = 7,
                            actualiza_precio = "N",
                            rg5329 = "N"
                        }
                    };

                    facturaRequest.comprobante.detalle.Add(item);
                }

                

                if ((decimal)cabecera.Rows[0]["impuesto"] > 0)
                {
                    
                    facturaRequest.comprobante.tributos = new List<Tributo>();
                    Tributo unTributo = new Tributo
                    {
                        tipo = tributoIIBB,
                        regimen = regimenIIBB,
                        base_imponible = totalSinIva,
                        alicuota = Math.Round((decimal)cabecera.Rows[0]["impuesto"], 2),
                        total = Math.Round(totalSinIva * ((decimal)cabecera.Rows[0]["impuesto"] / 100),2)
                    };

                    facturaRequest.comprobante.tributos.Add(unTributo);
                }


                var (ok, jsonRespuesta, error) = await emitirComprobante(facturaRequest);

                if (!ok)
                {
                    if (!string.IsNullOrEmpty(jsonRespuesta))
                    {
                        // error HTTP
                        string errorHttp = "Error API: " + jsonRespuesta;
                        return false;
                    }
                    else
                    {
                        // error técnico (timeout, red, etc.)
                        string errorHttp = "Error técnico: " + error;
                    }

                    return false;
                }

                FacturaResponse respuesta = JsonConvert.DeserializeObject<FacturaResponse>(jsonRespuesta);

                Fiscal unTk = new Fiscal();

                if (respuesta.error == "N")
                {
                    string cae = respuesta.cae.Trim();
                    string vencimientoCAE = respuesta.vencimiento_cae;
                    string numero = respuesta.comprobante_nro;
                    string pdf = respuesta.comprobante_pdf_url;
                    string qr = respuesta.afip_qr;


                    ComprobanteFiscal unComprobante = new ComprobanteFiscal();

                    unComprobante.TipoComprobante = "Factura";
                    unComprobante.Letra = cabecera.Rows[0]["letra"].ToString();
                    unComprobante.PuntoVenta = puntoVenta;
                    unComprobante.Numero = numero.Split('-')[1].TrimStart('0');
                    unComprobante.FechaEmision = DateTime.Now;
                    unComprobante.CreatedAt = DateTime.Now;
                    unComprobante.NroReferencia = int.Parse(unaVenta.ToString());
                    unComprobante.FkCliente = int.Parse(cabecera.Rows[0]["Cliente"].ToString());
                    unComprobante.RazonSocial = cabecera.Rows[0]["razonSocial"].ToString();
                    unComprobante.Cuit = cabecera.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "99999999" : cabecera.Rows[0]["cuil"].ToString();
                    unComprobante.ImporteTotal = decimal.Parse(cabecera.Rows[0]["totalVenta"].ToString());
                    unComprobante.Estado = "Emitido";
                    unComprobante.Cae = cae;
                    unComprobante.FechaVencimientoCae = DateTime.ParseExact(vencimientoCAE, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    unComprobante.urlComprobante = pdf;
                    unComprobante.qrAfip = qr;

                    // guardar en base de datos
                    unTk.almacenarComprobanteFiscal(unComprobante);

                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = pdf,
                        UseShellExecute = true
                    });

                    return true;
                }
                else
                {
                    string errores = string.Join(" | ", respuesta.errores);

                    // guardar error en log
                    unTk.AddErrorFE(unaVenta, errores);

                    MessageBox.Show(errores, "FACTURACION", MessageBoxButton.OK, MessageBoxImage.Error);

                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private ComprobanteAsociado CrearComprobanteAsociado (string tipoComp, int numeroAsoc, string fechaAsociado, long cuitAsoc)
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
        private Cliente CrearCliente(DataRow row)
        {
            bool esConsumidorFinal = row["Cliente"].ToString() == clienteConsumidorFinal.ToString();

            if (!Enum.TryParse(row["Provincia"].ToString().Replace(" ", "_"), out ProvinciasEnum provincia))
                throw new Exception("Provincia inválida");

            return new Cliente
            {
                documento_tipo = esConsumidorFinal ? "DNI" : "CUIT",
                documento_nro = esConsumidorFinal ? "99999999" : row["cuil"].ToString(),
                razon_social = row["razonSocial"].ToString(),
                domicilio = row["Direccion"].ToString(),
                provincia = ((int)provincia).ToString(),
                codigo = "Clie" + row["Cliente"].ToString(),
                envia_por_mail = enviarFacturaMail,
                email = enviarFacturaMail == "S" ? row["email"].ToString() : null,
                condicion_pago = "201",
                condicion_iva = row["abrevFE"].ToString(),
                rg5329 = "N"                
            };
        }

        private Comprobante CrearComprobante(string tipo, decimal total)
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
                rubro_grupo_contable = $"{now.Month}/{now.Year}",
                total = total
            };
        }
    }
}
