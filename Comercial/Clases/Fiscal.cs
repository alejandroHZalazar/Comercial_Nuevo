using EPSON_Impresora_Fiscal;
using MySqlConnector;
using System;
using System.Data;
using System.IO.Ports;
using System.Windows.Forms;
using OCXFISLib;

namespace Comercial.Clases
{
    public class Fiscal
    {
        private readonly SerialPort _port;
        short puerto = Clases.ClassParametros.buscarParametro("ventas", "PuertoFiscal") == "" ? short.Parse("0") : short.Parse(Clases.ClassParametros.buscarParametro("ventas", "PuertoFiscal"));
        int clienteConsumidorFinal = Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "clienteConsumidorFinal"));
        string empresaCondIVA = Clases.ClassParametros.buscarParametro("empresa", "condIVA");
        int cantDec = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();
        int puntoVenta = Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName) == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("PuntoVenta", Environment.MachineName));
        public bool CierreZEpson(short unPuerto)
        {
            EPSON_Impresora_Fiscal.PrinterFiscal epson = new PrinterFiscal();

            epson.PortNumber = unPuerto;
            var status = epson.CloseJournal("Z");
            return status;            
            
        }

        public ComprobanteFiscal imprimirFacturaHasar(long unaVenta)
        {
            OCXFISLib.DriverFiscal objFiscal = new OCXFISLib.DriverFiscal();

            objFiscal.Printer = "FiscalNET";

            OCXFISLib.IHasarTickNT hasar = objFiscal.HasarTickNT;

            var status = hasar.OpenFiscalReceipt(2);

            FiscalNET.HasarTicket unHs = new FiscalNET.HasarTicket();

            unHs.Printer = "HASAR SMH/P-322F";

            var status1 = unHs.OpenFiscalReceipt("COM2", "9600");

            return null;
        }

        public ComprobanteFiscal imprimirFacturaEpson(long unaVenta)
        {                

            EPSON_Impresora_Fiscal.PrinterFiscal epson = new PrinterFiscal();
            Clases.ClassVentas instVentas = new ClassVentas();
            string letra = "";
            if (empresaCondIVA == "")
            {
                MessageBox.Show("Debe parametrizar la condicion de IVA de la empresa", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            try
                {
                DataTable cabecera = instVentas.TraerCabeceraFactura(unaVenta);
                var tipoCUIT = cabecera.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "DNI" : "T";
                var nroCUIT = cabecera.Rows[0]["Cliente"].ToString() == clienteConsumidorFinal.ToString() ? "0" :cabecera.Rows[0]["cuil"].ToString();
                var iva = Math.Round(((decimal)cabecera.Rows[0]["IVA"] == 0?21: (decimal)cabecera.Rows[0]["IVA"]) * 100,0);
                var descuento =  (100 - (decimal)cabecera.Rows[0]["descuento"])/100;
                var recargo = 1 + ((decimal)cabecera.Rows[0]["recargo"] / 100);
                epson.PortNumber = puerto;
                letra = cabecera.Rows[0]["letra"].ToString();
                var status = epson.CloseInvoice("T", letra, "");
                status = epson.OpenInvoice("T", "C", letra, "1", "P", "12", "M", cabecera.Rows[0]["abrev"].ToString(), cabecera.Rows[0]["razonSocial"].ToString(), "", tipoCUIT, nroCUIT, "N", cabecera.Rows[0]["Direccion"].ToString(), "", "", cabecera.Rows[0]["formaPago"].ToString(), "", "G");
                if (!status)
                {
                    MessageBox.Show("Problemas en la impresion del encabezado de la factura", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                           

                DataTable detalle = instVentas.TraerDetalleFactura(unaVenta);
                if (detalle.Rows.Count == 0) return null;

                foreach (DataRow fila in detalle.Rows)
                {
                    var precio = letra == "A"? Math.Round(decimal.Parse(fila["precioSinIva"].ToString()), cantDec) * 100 : Math.Round(decimal.Parse(fila["precioConIva"].ToString()), cantDec) * 100;
                    precio = precio * descuento;
                    precio = precio * recargo;
                    var precioStrig = precio.ToString("0");                    
                    var cantidad = (decimal.Parse(fila["cantidad"].ToString()) * 1000).ToString("0");
                    var descripcion = fila["descripcion"].ToString();
                    descripcion = descripcion.Length > 20
                        ? descripcion.Substring(0, 20)
                        : descripcion;
                    status = epson.SendInvoiceItem(descripcion, cantidad, precioStrig.ToString(), iva.ToString(), "M", "0", "0", descuento!= 1?"Desc. " + Math.Round((decimal)cabecera.Rows[0]["descuento"],cantDec).ToString () + "%": recargo != 1 ? "Rec. " + Math.Round((decimal)cabecera.Rows[0]["recargo"], cantDec).ToString() + "%":"", "", "", "0", "0");
                    
                    //status = epson.SendInvoiceItem(fila["descripcion"].ToString(), "1100", "20030", "2100", "M", "0", "0", "", "", "", "0", "0");
                    if (!status)
                    {
                        MessageBox.Show("Problemas en la impresion del detalle de la factura", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }           
                

                status = epson.GetInvoiceSubtotal("P", "Subtotal");
                if (!status)
                {
                    MessageBox.Show("Problemas en la impresion del subtotal de la factura", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                status = epson.CloseInvoice("T", letra, "");
                if (!status)
                {
                    MessageBox.Show("Problemas en la impresion del cierre de la factura", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                var comprobante = epson.AnswerField_3;
                
                if (comprobante == null)
                {
                    MessageBox.Show("Problemas en la obtener el numero de comprobante", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                ComprobanteFiscal salida = new ComprobanteFiscal
                {
                    TipoComprobante = "Factura",
                    Letra = letra,
                    PuntoVenta = puntoVenta,
                    Numero = comprobante,
                    FechaEmision = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    NroReferencia = int.Parse(unaVenta.ToString()),
                    FkCliente = int.Parse(cabecera.Rows[0]["Cliente"].ToString()),
                    RazonSocial = cabecera.Rows[0]["razonSocial"].ToString(),
                    Cuit = nroCUIT.ToString(),
                    ImporteTotal = decimal.Parse(cabecera.Rows[0]["totalVenta"].ToString()),
                    Estado = "Emitido"
                };

                return salida;
            }
            catch (Exception ex)
            {
                var status = epson.CloseInvoice("T", letra, "");
                MessageBox.Show("Problemas al generar comprobante Fiscal", "VENTAS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
      
        public long almacenarComprobanteFiscal(ComprobanteFiscal comprobante)
        {
            classDatos instDatos = new classDatos();
            try
            {
               
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_fiscal_Add_Comprobante ";

                cmd.Parameters.AddWithValue("unTipo", comprobante.TipoComprobante);
                cmd.Parameters.AddWithValue("unaLetra", comprobante.Letra);
                cmd.Parameters.AddWithValue("unPtoVenta", comprobante.PuntoVenta);
                cmd.Parameters.AddWithValue("unNumero", comprobante.Numero);
                cmd.Parameters.AddWithValue("unaFechaEmision", comprobante.FechaEmision);
                cmd.Parameters.AddWithValue("unaReferencia", comprobante.NroReferencia);
                cmd.Parameters.AddWithValue("unCliente", comprobante.FkCliente);
                cmd.Parameters.AddWithValue("unaRazonSocial", comprobante.RazonSocial);
                cmd.Parameters.AddWithValue("unCuit", comprobante.Cuit);
                cmd.Parameters.AddWithValue("unImporte", comprobante.ImporteTotal);
                cmd.Parameters.AddWithValue("unCae", comprobante.Cae);
                cmd.Parameters.AddWithValue("unVencimientoCae", comprobante.FechaVencimientoCae);
                cmd.Parameters.AddWithValue("unEstado", comprobante.Estado);
                cmd.Parameters.AddWithValue("unFiscal_status", comprobante.FiscalStatus);
                cmd.Parameters.AddWithValue("unaJornada", comprobante.NumeroJornada);
                cmd.Parameters.AddWithValue("unCreated_at", comprobante.CreatedAt);
                cmd.Parameters.AddWithValue("unAfip_qr", comprobante.qrAfip);
                cmd.Parameters.AddWithValue("unLinkPDF", comprobante.urlComprobante);


                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int64);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                long valor = long.Parse(cmd.Parameters["salida"].Value.ToString());
                return valor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public void AddErrorFE(long unaVenta, string unError)
        {
            classDatos instDatos = new classDatos();
            try
            {

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_fiscal_add_erroresFE ";

                cmd.Parameters.AddWithValue("unaVenta", unaVenta);
                cmd.Parameters.AddWithValue("unError", unError);    
                cmd.ExecuteScalar();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

    }

    public class ComprobanteFiscal
    {
       // Identificación del comprobante
        public string TipoComprobante { get; set; } = string.Empty;   // VARCHAR(20)
        public string Letra { get; set; } = string.Empty;             // CHAR(1)
        public int PuntoVenta { get; set; }
        public string Numero { get; set; }

        // Fechas
        public DateTime FechaEmision { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relaciones
        public int? NroReferencia { get; set; }
        public int? FkCliente { get; set; }

        // Datos del receptor
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }

        // Importes
        public decimal ImporteTotal { get; set; }

        // Datos electrónicos (si aplica)
        public string Cae { get; set; }
        public DateTime? FechaVencimientoCae { get; set; }
        public string urlComprobante { get; set; }
        public string qrAfip { get; set; }

        // Estado
        public string Estado { get; set; } = "EMITIDO";

        // Datos fiscales técnicos
        public int? FiscalStatus { get; set; }
        public int? NumeroJornada { get; set; }
    }

}
