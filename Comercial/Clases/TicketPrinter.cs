using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comercial.Clases
{
    public class TicketPrinter
    {
        private List<ItemVenta> items;
        private string vendedor;
        private string comprador;
        private int anchoCaracteres;
        private decimal total;
        int cantDec = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();
        DateTime fecha;
        string nroVenta;
        decimal porcentajeDescuento;
        decimal porcentajeRecargo;
        string empresa = Clases.ClassParametros.buscarParametro("empresa","nombre");
        string direccion = Clases.ClassParametros.buscarParametro("empresa", "direccion") + ". " + Clases.ClassParametros.buscarParametro("empresa", "localidad");
        string telefono = Clases.ClassParametros.buscarParametro("empresa", "telefono");
        public TicketPrinter(List<ItemVenta> items, string vendedor, string comprador, decimal totalVenta, DateTime _fecha, string _nroVenta, 
                            decimal _porcentajeDescuento, decimal _porcentajeRecargo, int anchoCaracteres = 42)
        {
            this.items = items;
            this.vendedor = vendedor;
            this.comprador = comprador;
            this.anchoCaracteres = anchoCaracteres;
            fecha = _fecha;
            total = totalVenta;
            nroVenta = _nroVenta;
            porcentajeDescuento = _porcentajeDescuento;
            porcentajeRecargo = _porcentajeRecargo;
        }

        public void Imprimir()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            pd.Print(); // impresora por defecto
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            int anchoTicket;

            if (anchoCaracteres == 42)
            {
                anchoTicket = 280;   // aprox 80mm
            }
            else
            {
                anchoTicket = 200;   // aprox 58mm
            }
            float y = 0;
            float lineHeight = 18;
            Font font = new Font("Consolas", 9);
            CultureInfo culturaAR = new CultureInfo("es-AR");
            int decImporte = cantDec; // lo que venga por parámetro
            int decStock = cantStock;
            string formatoImporte = "0." + new string('0', decImporte);
            string formatoStock = "0." + new string('0', decStock);
            string nroFormateado = nroVenta.ToString();
            // Encabezado
            Font fontNormal = new Font("Consolas", 9, FontStyle.Regular);
            Font fontGrande = new Font("Consolas", 18, FontStyle.Bold);           

            // X grande centrada
            DibujarTextoCentrado(e.Graphics, "TICKET", fontGrande, anchoTicket, y);
            y += fontGrande.GetHeight(e.Graphics) + 5;

            // Nombre vendedor
            DibujarTextoCentrado(e.Graphics, empresa, fontNormal, anchoTicket, y);
            y += lineHeight;

            // Dirección
            DibujarTextoCentrado(e.Graphics, direccion, fontNormal, anchoTicket, y);
            y += lineHeight;

            // Teléfono
            DibujarTextoCentrado(e.Graphics, telefono, fontNormal, anchoTicket, y);
            y += lineHeight;

            // Línea separadora
            e.Graphics.DrawString(new string('-', anchoCaracteres), fontNormal, Brushes.Black, 0, y);
            y += lineHeight;           

            // Datos
            e.Graphics.DrawString("Venta N°: " + nroFormateado, font, Brushes.Black, 0, y);
            y += lineHeight;
            e.Graphics.DrawString("Vendedor: " + vendedor, font, Brushes.Black, 0, y);
            y += lineHeight;
            e.Graphics.DrawString("Cliente: " + comprador, font, Brushes.Black, 0, y);
            y += lineHeight;
            e.Graphics.DrawString("Fecha: " + fecha.ToString("dd/MM/yyyy HH:mm"), font, Brushes.Black, 0, y);
            y += lineHeight;

            e.Graphics.DrawString("------------------------------------------", font, Brushes.Black, 0, y);
            y += lineHeight;

            // Detalle
            foreach (var item in items)
            {
                decimal subtotal = item.subtotal;

                // Línea descripción
                var lineasDescripcion = AjustarDescripcion(item.Descripcion, anchoCaracteres);

                foreach (var lineaDesc in lineasDescripcion)
                {
                    e.Graphics.DrawString(lineaDesc, font, Brushes.Black, 0, y);
                    y += lineHeight;
                }

                // Línea precio x cant + subtotal alineado derecha
                              
                

                string izquierda = $"{item.PrecioUnit.ToString(formatoImporte, culturaAR)} x {item.Cantidad.ToString(formatoStock, culturaAR)}";
                string derecha = subtotal.ToString(formatoImporte, culturaAR);

                string linea = AlinearTexto(izquierda, derecha);
                e.Graphics.DrawString(linea, font, Brushes.Black, 0, y);
                y += lineHeight;

                y += 5;
            }

            e.Graphics.DrawString("------------------------------------------", font, Brushes.Black, 0, y);
            y += lineHeight;

            if (porcentajeDescuento > 0)
            {
                string lineaDesc = AlinearTexto(
                    "Descuento:",
                    porcentajeDescuento.ToString("0.##") + "%"                    
                );

                e.Graphics.DrawString(lineaDesc, font, Brushes.Black, 0, y);
                y += lineHeight;
            }

            if (porcentajeRecargo > 0)
            {
                string lineaDesc = AlinearTexto(
                    "Descuento:",
                    porcentajeRecargo.ToString("0.##") + "%"                    
                );

                e.Graphics.DrawString(lineaDesc, font, Brushes.Black, 0, y);
                y += lineHeight;
            }

            // Total — mismo tamaño que el detalle para que el alineado por caracteres
            // coincida exactamente con los importes de arriba (Bold solo para destacar)
            Font fontTotal = new Font("Consolas", 9, FontStyle.Bold);

            string lineaTotal = AlinearTexto("TOTAL:", total.ToString(formatoImporte, culturaAR));
            e.Graphics.DrawString(lineaTotal, fontTotal, Brushes.Black, 0, y);

            y += lineHeight;

        }

        private string AlinearTexto(string izquierda, string derecha)
        {
            int espacios = anchoCaracteres - izquierda.Length - derecha.Length;
            if (espacios < 1) espacios = 1;

            return izquierda + new string(' ', espacios) + derecha;
        }

        private List<string> AjustarDescripcion(string texto, int anchoMaximo)
        {
            List<string> lineas = new List<string>();

            while (texto.Length > anchoMaximo)
            {
                int corte = texto.LastIndexOf(' ', anchoMaximo);

                if (corte <= 0)
                    corte = anchoMaximo;

                lineas.Add(texto.Substring(0, corte));
                texto = texto.Substring(corte).Trim();
            }

            lineas.Add(texto);

            return lineas;
        }

        void DibujarTextoCentrado(Graphics g, string texto, Font font, float anchoTicket, float y)
        {
            SizeF size = g.MeasureString(texto, font);
            float x = (anchoTicket - size.Width) / 2;
            g.DrawString(texto, font, Brushes.Black, x, y);
        }
    }

    public class ItemVenta
    {
        public string Descripcion { get; set; }
        public decimal PrecioUnit { get; set; }
        public decimal Cantidad { get; set; }

        public decimal subtotal { get; set; }
    }
}
