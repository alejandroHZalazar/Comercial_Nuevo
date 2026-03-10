using Comercial.Enums;
using Comercial.Properties;
using Comercial.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        string rubroFE = Clases.ClassParametros.buscarParametro("facturacionElectronica", "rubro") == "" ? "Productos Varios" : Clases.ClassParametros.buscarParametro("ventas", "rubro");
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

        }

        public class DetalleFactura
        {
            public decimal cantidad { get; set; }
            public string afecta_stock { get; set; }
            public Producto producto { get; set; }
            public decimal bonificacion_porcentaje { get; set; }
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

        public async Task<HttpResponseMessage> EmitirFactura(FacturaRequest factura)
        {
            string urlApi = "https://www.tusfacturas.app/app/api/v2/facturacion/nuevo";
            using (HttpClient client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(factura);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                return await client.PostAsync(urlApi, content);
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
                facturaRequest.cliente.documento_tipo = cabecera.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "DNI" : "CUIT";
                facturaRequest.cliente.documento_nro = cabecera.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "99999999" : cabecera.Rows[0]["cuil"].ToString();
                facturaRequest.cliente.razon_social = cabecera.Rows[0]["razonSocial"].ToString();
                facturaRequest.cliente.domicilio = cabecera.Rows[0]["Direccion"].ToString();
                var provinciaDb = cabecera.Rows[0]["Provincia"].ToString();

                ProvinciasEnum provincia;

                if (!Enum.TryParse(provinciaDb.Replace(" ", "_"), out provincia)) return false;

                var codProvincia = (int)provincia;
                facturaRequest.cliente.provincia = codProvincia.ToString();

                facturaRequest.cliente.codigo = "Clie" + cabecera.Rows[0]["Cliente"].ToString();
                facturaRequest.cliente.envia_por_mail = enviarFacturaMail;
                if (enviarFacturaMail == "S") facturaRequest.cliente.email = cabecera.Rows[0]["email"].ToString();
                facturaRequest.cliente.condicion_pago = "201";
                facturaRequest.cliente.condicion_iva = cabecera.Rows[0]["abrevFE"].ToString();
                facturaRequest.cliente.rg5329 = "N";

                //-------------------Comprobante-------------------------

                string claveFactura = $"Tipo_Comprobante_F{cabecera.Rows[0]["letra"].ToString().ToUpper()}";
                string tipoComprobante = Resource.ResourceManager.GetString(claveFactura);

                facturaRequest.comprobante.fecha = DateTime.Now.ToString("dd/MM/yyyy");
                facturaRequest.comprobante.tipo = tipoComprobante;
                facturaRequest.comprobante.operacion = "V";
                facturaRequest.comprobante.idioma = "1";
                facturaRequest.comprobante.punto_venta = puntoVenta.ToString();
                facturaRequest.comprobante.moneda = "PES";
                facturaRequest.comprobante.cotizacion = "1";
                facturaRequest.comprobante.vencimiento = DateTime.Now.ToString("dd/MM/yyyy");
                facturaRequest.comprobante.periodo_facturado_desde = DateTime.Now.ToString("dd/MM/yyyy");
                facturaRequest.comprobante.periodo_facturado_hasta = DateTime.Now.ToString("dd/MM/yyyy");
                facturaRequest.comprobante.rubro = rubroFE;
                facturaRequest.comprobante.rubro_grupo_contable = DateTime.Now.Month.ToString() + '/' + DateTime.Now.Year.ToString();
                facturaRequest.comprobante.total = (decimal)cabecera.Rows[0]["totalVenta"];

                //-------------------Detalle Comprobante-----------------------

                DataTable detalle = instVentas.TraerDetalleFactura(unaVenta);
                if (detalle.Rows.Count == 0) return false;
                facturaRequest.comprobante.detalle = new List<DetalleFactura>();

                foreach (DataRow fila in detalle.Rows)
                {
                    DetalleFactura item = new DetalleFactura
                    {
                        cantidad = Math.Round((decimal)(fila["cantidad"]), 2),
                        afecta_stock = "S",
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

                    facturaRequest.comprobante.detalle.Add(item);
                }

                var response = await EmitirFactura(facturaRequest);

                if (!response.IsSuccessStatusCode)
                {
                    // error HTTP
                    string errorHttp = await response.Content.ReadAsStringAsync();
                    return false;
                }

                string jsonRespuesta = await response.Content.ReadAsStringAsync();

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

                    return true;
                }
                else
                {
                    string errores = string.Join(" | ", respuesta.errores);

                    // guardar error en log
                    unTk.AddErrorFE(unaVenta, errores);

                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
