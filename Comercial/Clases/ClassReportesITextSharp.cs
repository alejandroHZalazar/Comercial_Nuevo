using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;
using System.Data;
using iTextSharp.text.pdf.draw;
using System.Globalization;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;

namespace Comercial.Clases
{
    public class ClassReportesITextSharp
    {


        public void GenerarYMostrarRecibo(string unIdRecibo, string logoPath, string nombreEmpresa, string direccionEmpresa, string telEmpresa, string cuitEmpresa, DateTime unaFechaRecibo,
                                            string nombreCliente, string cuitCliente, string observCobro, decimal importe, decimal saldoCC)

        {
            string downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            string rutaPdf = Path.Combine(downloads, "Recibo_No_Fiscal_" + unIdRecibo + ".pdf");

            string rutaLogo = logoPath;

            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(rutaPdf, FileMode.Create));

            doc.Open();

            // Fuentes
            Font titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Font negrita = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            // ================= LOGO =================
            if (File.Exists(rutaLogo))
            {
                Image logo = Image.GetInstance(rutaLogo);
                logo.ScaleToFit(120, 60);
                logo.Alignment = Image.ALIGN_LEFT;
                doc.Add(logo);
            }

            // ================= TITULO =================
            Paragraph pTitulo = new Paragraph("RECIBO NO FISCAL", titulo);
            pTitulo.Alignment = Element.ALIGN_CENTER;
            pTitulo.SpacingBefore = 10;
            pTitulo.SpacingAfter = 20;
            doc.Add(pTitulo);

            // ================= EMPRESA =================
            doc.Add(new Paragraph("Empresa: " + nombreEmpresa, negrita));
            doc.Add(new Paragraph("Dirección: " + direccionEmpresa, normal));
            doc.Add(new Paragraph("Tel: " + telEmpresa, normal));
            doc.Add(new Paragraph("CUIT:" + cuitEmpresa, normal));

            doc.Add(new Paragraph(" "));

            // ================= DATOS RECIBO =================
            doc.Add(new Paragraph("Recibo N°: " + unIdRecibo.PadLeft(7, '0'), normal));
            doc.Add(new Paragraph("Fecha: " + unaFechaRecibo.ToString("dd/MM/yyyy"), normal));

            doc.Add(new Paragraph("--------------------------------------------------"));

            // ================= CLIENTE =================
            doc.Add(new Paragraph("Cliente: " + nombreCliente, normal));
            doc.Add(new Paragraph("DNI / CUIT: " + cuitCliente, normal));

            doc.Add(new Paragraph(" "));

            // ================= CONCEPTO =================
            doc.Add(new Paragraph("Concepto:", negrita));
            doc.Add(new Paragraph(observCobro, normal));

            doc.Add(new Paragraph(" "));

            // ================= PAGO =================
            doc.Add(new Paragraph("Total recibido: " + importe.ToString("C"), negrita));

            doc.Add(new Paragraph("Saldo pendiente: " + saldoCC.ToString("C"), negrita));

            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));

            // ================= FIRMA =================
            doc.Add(new Paragraph("Firma: ________________________________", normal));
            doc.Add(new Paragraph("Aclaración: ___________________________", normal));

            doc.Close();

            // ================= ABRIR PDF =================
            Process.Start(new ProcessStartInfo()
            {
                FileName = rutaPdf,
                UseShellExecute = true
            });
        }

        public class ProductoEtiqueta
        {
            public string Descripcion { get; set; }
            public decimal Precio { get; set; }
            public string CodigoBarras { get; set; }
        }


        public void GenerarEtiquetasPDF(List<ProductoEtiqueta> productos, string pathArchivo, Action<int> reportarProgreso)
        {
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pathArchivo, FileMode.Create));
            doc.Open();

            PdfContentByte cb = writer.DirectContent;

            float etiquetaAncho = 170f;   // 6 cm
            float etiquetaAlto = 113f;    // 4 cm

            float margenIzq = 40f;
            float margenSup = 800f;

            int columna = 0;
            int fila = 0;

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            int procesados = 0;
            foreach (var p in productos)
            {
                float x = margenIzq + (columna * etiquetaAncho);
                float y = margenSup - (fila * etiquetaAlto);

                // ----- Rectángulo punteado -----
                cb.SetLineDash(3f, 3f);
                cb.Rectangle(x, y - etiquetaAlto, etiquetaAncho, etiquetaAlto);
                cb.Stroke();
                cb.SetLineDash(0);

                // ----- Descripción -----
                ColumnText ct = new ColumnText(cb);

                Phrase descripcion = new Phrase(
                    p.Descripcion,
                    new Font(bf, 9, Font.BOLD)
                );

                // Definimos el área donde puede escribir
                ct.SetSimpleColumn(
                    descripcion,
                    x + 5,                         // izquierda
                    y - 40,                        // abajo
                    x + etiquetaAncho - 5,         // derecha
                    y - 10,                        // arriba
                    12,                            // interlineado
                    Element.ALIGN_CENTER
                );

                ct.Go();

                // ----- Precio grande -----
                ColumnText.ShowTextAligned(
                    cb,
                    Element.ALIGN_CENTER,
                    new Phrase("$ " + p.Precio.ToString("N2"),
                    new Font(bf, 22, Font.BOLD, BaseColor.MAGENTA)),
                    x + etiquetaAncho / 2,
                    y - 65,
                    0);

                // ----- Fecha izquierda -----
                ColumnText.ShowTextAligned(
                    cb,
                    Element.ALIGN_LEFT,
                    new Phrase(DateTime.Now.ToString("dd/MM/yyyy"),
                    new Font(bf, 7)),
                    x + 5,
                    y - etiquetaAlto + 5,
                    0);

                // ----- Código de barras -----
                Barcode128 barcode = new Barcode128();
                barcode.Code = p.CodigoBarras;
                barcode.CodeType = Barcode128.CODE128;

                Image barcodeImg = barcode.CreateImageWithBarcode(cb, null, null);
                barcodeImg.ScalePercent(70);
                barcodeImg.SetAbsolutePosition(x + etiquetaAncho - 100, y - etiquetaAlto + 5);
                doc.Add(barcodeImg);

                columna++;

                if (columna == 3)
                {
                    columna = 0;
                    fila++;
                }

                // Nueva página si no entra más
                if ((margenSup - ((fila + 1) * etiquetaAlto)) < 50)
                {
                    doc.NewPage();
                    columna = 0;
                    fila = 0;
                }

                procesados++;
                reportarProgreso?.Invoke(procesados);
            }

            doc.Close();

            // Abrir automáticamente
            System.Diagnostics.Process.Start(pathArchivo);
        }

        public void GenerarCodBarrasPDF(
                                        List<ProductoEtiqueta> productos,
                                        string pathArchivo,
                                        Action<int> reportarProgreso = null)
        {
            using (FileStream fs = new FileStream(pathArchivo, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                PdfContentByte cb = writer.DirectContent;

                // Tamaño etiqueta (6x4 cm aprox)
                float etiquetaAncho = 170f;
                float etiquetaAlto = 113f;

                float margenIzq = 40f;
                float margenSup = 800f;

                int columna = 0;
                int fila = 0;

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                int procesados = 0;
                int total = productos.Count;

                foreach (var p in productos)
                {
                    float x = margenIzq + (columna * etiquetaAncho);
                    float y = margenSup - (fila * etiquetaAlto);

                    // ===============================
                    // RECTÁNGULO
                    // ===============================
                    cb.SetLineDash(3f, 3f);
                    cb.Rectangle(x, y - etiquetaAlto, etiquetaAncho, etiquetaAlto);
                    cb.Stroke();
                    cb.SetLineDash(0);

                    // ===============================
                    // DESCRIPCIÓN DINÁMICA
                    // ===============================
                    ColumnText ct = new ColumnText(cb);

                    Phrase descripcion = new Phrase(
                        p.Descripcion,
                        new Font(bf, 9, Font.BOLD)
                    );

                    float descTop = y - 10;
                    float descBottom = y - 45; // área máxima para descripción

                    ct.SetSimpleColumn(
                        descripcion,
                        x + 5,
                        descBottom,
                        x + etiquetaAncho - 5,
                        descTop,
                        11,
                        Element.ALIGN_CENTER
                    );

                    ct.Go();

                    // ===============================
                    // CÓDIGO DE BARRAS
                    // ===============================
                    Barcode128 barcode = new Barcode128();
                    barcode.Code = p.CodigoBarras;
                    barcode.CodeType = Barcode128.CODE128;
                    barcode.BarHeight = 35f;
                    barcode.X = 1.1f;

                    // ocultar texto automático
                    barcode.Font = null;

                    Image barcodeImg = barcode.CreateImageWithBarcode(cb, null, null);
                    barcodeImg.ScalePercent(90);

                    float barcodeX = x + (etiquetaAncho - barcodeImg.ScaledWidth) / 2;
                    float barcodeY = y - etiquetaAlto + 35;

                    barcodeImg.SetAbsolutePosition(barcodeX, barcodeY);
                    doc.Add(barcodeImg);

                    // ===============================
                    // TEXTO DEBAJO DEL CÓDIGO
                    // ===============================
                    ColumnText.ShowTextAligned(
                        cb,
                        Element.ALIGN_CENTER,
                        new Phrase(p.CodigoBarras, new Font(bf, 8)),
                        x + etiquetaAncho / 2,
                        barcodeY - 10,
                        0);

                    // ===============================
                    // CONTROL DE GRILLA
                    // ===============================
                    columna++;

                    if (columna == 3)
                    {
                        columna = 0;
                        fila++;
                    }

                    // Nueva página
                    if ((margenSup - ((fila + 1) * etiquetaAlto)) < 50)
                    {
                        doc.NewPage();
                        columna = 0;
                        fila = 0;
                    }

                    // ===============================
                    // PROGRESO
                    // ===============================
                    procesados++;
                    reportarProgreso?.Invoke(procesados);
                }

                doc.Close();
            }

            System.Diagnostics.Process.Start(pathArchivo);
        }

        public void GenerarVentasPDF(long unaVenta)
        {
            var instVentas = new Clases.ClassVentas();
            DataTable dt = instVentas.imprimirVenta(unaVenta);

            if (dt.Rows.Count == 0) return;

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            string path = Path.Combine(downloads, $"Venta_{unaVenta}_{DateTime.Now:ddMMyyyy_HHmmss}.pdf");

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                Font bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 8);

                DataRow cab = dt.Rows[0];

                // ================= HEADER =================
                AgregarHeaderVenta(doc, cab);

                // ================= CLIENTE =================
                Paragraph p1 = new Paragraph();
                p1.Add(new Chunk("Sres: ", bold));
                p1.Add(new Chunk(cab["nombreComercial"].ToString(), normal));
                doc.Add(p1);

                Chunk glue = new Chunk(new VerticalPositionMark());

                Paragraph p2 = new Paragraph();
                p2.Add(new Chunk("Razón Social: ", bold));
                p2.Add(new Chunk(cab["razonSocial"].ToString(), normal));
                p2.Add(glue);
                p2.Add(new Chunk("CUIT: ", bold));
                p2.Add(new Chunk(cab["cuil"].ToString(), normal));
                doc.Add(p2);

                Paragraph p3 = new Paragraph();
                p3.Add(new Chunk("Dirección: ", bold));
                p3.Add(new Chunk(cab["Direccion"].ToString(), normal));
                p3.Add(glue);
                p3.Add(new Chunk("Tel: ", bold));
                p3.Add(new Chunk(cab["Tel"].ToString(), normal));
                doc.Add(p3);

                Paragraph p4 = new Paragraph();
                p4.Add(new Chunk("Cond. IVA: ", bold));
                p4.Add(new Chunk(cab["Cond_IVA"].ToString(), normal));
                doc.Add(p4);

                doc.Add(new Paragraph(" "));

                // ================= TABLA =================
                Font bold8 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
                Font normal8 = FontFactory.GetFont(FontFactory.HELVETICA, 8);

                PdfPTable table = new PdfPTable(9);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 3, 2, 8, 2, 2, 2, 2, 2, 2 });

                string[] headers = {
            "C. Barras","C. Prov","Descripción","P Lista","%",
            "P S/IVA","P C/IVA","Cant","Subtotal"
        };

                foreach (var h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, bold8));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                var culture = new System.Globalization.CultureInfo("es-AR");

                foreach (DataRow row in dt.Rows)
                {
                    decimal precioSinIva = Convert.ToDecimal(row["precioSinIva"]);
                    decimal desc = Convert.ToDecimal(row["descuento_Linea"]);
                    decimal rec = Convert.ToDecimal(row["recargo_linea"]);
                    decimal porcentaje = rec > 0 ? rec : -desc;
                    decimal precioAjustado = Convert.ToDecimal(row["subtotalSinIVA"]);

                    table.AddCell(new Phrase(row["codBarras"].ToString(), normal8));
                    table.AddCell(new Phrase(row["codProveedor"].ToString(), normal8));
                    table.AddCell(new Phrase(row["descripcion"].ToString(), normal8));

                    table.AddCell(new PdfPCell(new Phrase(precioSinIva.ToString("N2", culture), normal8)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(porcentaje.ToString("N2", culture), normal8)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(precioAjustado.ToString("N2", culture), normal8)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(row["precioConIva"]).ToString("N2", culture), normal8)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(row["cantidad"]).ToString("N2", culture), normal8)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(row["subtotalIVA"]).ToString("N2", culture), normal8)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));

                // ================= TOTALES =================
                decimal totalSinIva = 0;
                decimal totalConIva = 0;
                decimal impuesto = 0;
                decimal IVA = 0;
                decimal subtotalBase = 0;

                foreach (DataRow row in dt.Rows)
                {
                    subtotalBase += Convert.ToDecimal(row["precioSinIva"]) * Convert.ToDecimal(row["cantidad"]);
                    totalSinIva += Convert.ToDecimal(row["subtotalSinIVA"]) * Convert.ToDecimal(row["cantidad"]);
                    totalConIva += Convert.ToDecimal(row["precioConIva"]) * Convert.ToDecimal(row["cantidad"]);
                    impuesto = Convert.ToDecimal(row["impuesto"]);
                    IVA = Convert.ToDecimal(row["IVA"]);
                }

                decimal ivaCalculado = IVA == 0 ? 0 : totalConIva - totalSinIva;
                decimal percepcion = impuesto == 0 ? 0 : totalSinIva * (impuesto / 100);
                decimal totalGeneral = totalSinIva + ivaCalculado + percepcion;

                PdfPTable tablaTotales = new PdfPTable(3);
                tablaTotales.WidthPercentage = 100;
                tablaTotales.SetWidths(new float[] { 6, 1, 3 });

                BaseColor grisClaro = new BaseColor(230, 230, 230);
                Font bold9 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                Font normal9 = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                tablaTotales.AddCell(new PdfPCell(new Phrase("Subtotal Sin IVA", bold9)) { BackgroundColor = grisClaro });
                tablaTotales.AddCell(new PdfPCell(new Phrase(subtotalBase.ToString("N2", culture), normal9)){Colspan = 2,HorizontalAlignment = Element.ALIGN_RIGHT,BackgroundColor = grisClaro});

                tablaTotales.AddCell(new PdfPCell(new Phrase("Total Sin IVA", bold9)) { BackgroundColor = grisClaro });
                tablaTotales.AddCell(new PdfPCell(new Phrase(totalSinIva.ToString("N2", culture), normal9)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_RIGHT, BackgroundColor = grisClaro });

                tablaTotales.AddCell(new PdfPCell(new Phrase("IVA", bold9)) { BackgroundColor = grisClaro });
                tablaTotales.AddCell(new PdfPCell(new Phrase(IVA.ToString("N2", culture), normal9)) { BackgroundColor = grisClaro });
                tablaTotales.AddCell(new PdfPCell(new Phrase(ivaCalculado.ToString("N2", culture), normal9)) { BackgroundColor = grisClaro, HorizontalAlignment = Element.ALIGN_RIGHT });

                tablaTotales.AddCell(new PdfPCell(new Phrase("Percepción IIBB", bold9)) { BackgroundColor = grisClaro });
                tablaTotales.AddCell(new PdfPCell(new Phrase(impuesto.ToString("N2", culture), normal9)) { BackgroundColor = grisClaro });
                tablaTotales.AddCell(new PdfPCell(new Phrase(percepcion.ToString("N2", culture), normal9)) { BackgroundColor = grisClaro, HorizontalAlignment = Element.ALIGN_RIGHT });

                tablaTotales.AddCell(new PdfPCell(new Phrase("TOTAL GENERAL", bold9)) { BackgroundColor = grisClaro });
                tablaTotales.AddCell(new PdfPCell(new Phrase("")) { BackgroundColor = grisClaro });
                tablaTotales.AddCell(new PdfPCell(new Phrase(totalGeneral.ToString("N2", culture), bold9)) { BackgroundColor = grisClaro, HorizontalAlignment = Element.ALIGN_RIGHT });

                // ================= MEDIOS DE PAGO =================
                Font boldTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                Font normalTexto = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                string mp1 = cab["Medio_Pago_1"]?.ToString();
                string mp2 = cab["Medio_Pago_2"]?.ToString();
                string mp3 = cab["Medio_Pago_3"]?.ToString();

                List<string> mediosPago = new List<string>();

                if (!string.IsNullOrEmpty(mp1) && mp1 != "Sin Especificar") mediosPago.Add(mp1);
                if (!string.IsNullOrEmpty(mp2) && mp2 != "Sin Especificar") mediosPago.Add(mp2);
                if (!string.IsNullOrEmpty(mp3) && mp3 != "Sin Especificar") mediosPago.Add(mp3);

                PdfPCell cellMP = new PdfPCell { Border = Rectangle.NO_BORDER };

                Paragraph tituloMP = new Paragraph("Medios de Pago:", boldTitulo);
                tituloMP.SpacingAfter = 5f;
                cellMP.AddElement(tituloMP);

                foreach (var mp in mediosPago)
                {
                    cellMP.AddElement(new Paragraph("• " + mp, normalTexto));
                }

                if (mediosPago.Count == 0)
                {
                    cellMP.AddElement(new Paragraph("Sin Medios de Pago", normalTexto));
                }

                // ================= LAYOUT FINAL =================
                PdfPTable layout = new PdfPTable(2);
                layout.WidthPercentage = 100;
                layout.SetWidths(new float[] { 1, 1 });

                PdfPCell left = new PdfPCell(cellMP) { Border = Rectangle.NO_BORDER };
                PdfPCell right = new PdfPCell(tablaTotales) { Border = Rectangle.NO_BORDER };

                layout.AddCell(left);   // 👈 Medios de Pago IZQUIERDA
                layout.AddCell(right);  // 👉 Totales DERECHA

                doc.Add(new Paragraph(" "));
                doc.Add(layout);

                doc.Close();
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        private void AgregarHeaderVenta(Document doc, DataRow cab)
        {
            Font bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            PdfPTable mainTable = new PdfPTable(3);
            mainTable.WidthPercentage = 100;
            mainTable.SetWidths(new float[] { 4, 1, 5 });
            mainTable.DefaultCell.Border = Rectangle.NO_BORDER;

            // ================= IZQUIERDA (LOGO + EMPRESA) =================
            PdfPTable left = new PdfPTable(2);
            left.WidthPercentage = 100;
            left.SetWidths(new float[] { 1, 3 });

            byte[] imgBytes = (byte[])cab["imagen"];

            using (var ms = new MemoryStream(imgBytes))
            {
                System.Drawing.Image test = System.Drawing.Image.FromStream(ms);
            }

            if (cab["imagen"] != DBNull.Value)
            {
                Image logo = Image.GetInstance((byte[])cab["imagen"]);
                logo.ScaleToFit(90,60);

                PdfPCell logoCell = new PdfPCell(logo);
                logoCell.Border = 0;
                logoCell.PaddingTop = 8f;
                logoCell.PaddingLeft = 5f;
                logoCell.PaddingRight = 5f;
                logoCell.VerticalAlignment = Element.ALIGN_TOP;

                left.AddCell(logoCell);
            }

            Paragraph empresa = new Paragraph();
            empresa.Alignment = Element.ALIGN_CENTER;
            empresa.SetLeading(0f, 1.2f); // 🔥 probá entre 1.1 y 1.5

            empresa.Add(new Chunk(Clases.ClassValidacion.traerEmpresa(), bold));
            empresa.Add(Chunk.NEWLINE);
            empresa.Add(Chunk.NEWLINE);

            empresa.Add(new Chunk(Clases.ClassValidacion.traerRazonSocial(), normal));
            empresa.Add(Chunk.NEWLINE);
            empresa.Add(new Chunk("Tel: " + Clases.ClassValidacion.traerEmpresaTelefono(), normal));
            empresa.Add(Chunk.NEWLINE);
            empresa.Add(new Chunk(Clases.ClassValidacion.traerEmpresaDireccion(), normal));
            empresa.Add(Chunk.NEWLINE);
            empresa.Add(new Chunk(Clases.ClassValidacion.traerEmpresaCiudad(), normal));

            PdfPCell empresaCell = new PdfPCell(empresa);
            empresaCell.Border = 0;
            empresaCell.VerticalAlignment = Element.ALIGN_TOP;
            empresaCell.PaddingLeft = 10f;
            empresaCell.HorizontalAlignment = Element.ALIGN_CENTER;

            left.AddCell(empresaCell);

            // 🔹 Contenedor SOLO con borde inferior
            PdfPCell leftContainer = new PdfPCell(left);
            leftContainer.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            //leftContainer.FixedHeight = 70f;

            mainTable.AddCell(leftContainer);

            // ================= CENTRO (X EN RECUADRO) =================
            PdfPTable tablaX = new PdfPTable(1);
            tablaX.WidthPercentage = 100;

            // 🔲 Cuadro de la X
            PdfPCell cellX = new PdfPCell(new Phrase("X", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)));
            cellX.HorizontalAlignment = Element.ALIGN_CENTER;
            cellX.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellX.Border = Rectangle.BOX;
            cellX.FixedHeight = 30;
            tablaX.AddCell(cellX);

            // 🔽 Línea central + base
            PdfPCell cellLinea = new PdfPCell(new Phrase(""));
            cellLinea.Border = Rectangle.NO_BORDER;
            cellLinea.FixedHeight = 40;
            cellLinea.CellEvent = new LineaVerticalConBase();

            tablaX.AddCell(cellLinea);

            // 🔹 Contenedor sin bordes
            PdfPCell contenedorX = new PdfPCell(tablaX);
            contenedorX.Border = Rectangle.NO_BORDER;
            contenedorX.FixedHeight = 70f;

            mainTable.AddCell(contenedorX);
            // ================= DERECHA (DATOS VENTA) =================
            PdfPTable right = new PdfPTable(1);
            right.WidthPercentage = 100;

            Paragraph datosVenta = new Paragraph();
            datosVenta.Alignment = Element.ALIGN_LEFT;

            datosVenta.Add(new Chunk($"NRO. VENTA: {cab["nroVenta"]}", bold));
            datosVenta.Add(Chunk.NEWLINE);
            datosVenta.Add(Chunk.NEWLINE);

            datosVenta.Add(new Chunk($"FECHA: {Convert.ToDateTime(cab["fecha"]):dd/MM/yyyy}", bold));
            datosVenta.Add(Chunk.NEWLINE);
            datosVenta.Add(Chunk.NEWLINE);

            datosVenta.Add(new Chunk($"CUIT: {cab["cuil"]}", normal));
            datosVenta.Add(Chunk.NEWLINE);
            datosVenta.Add(new Chunk("DOCUMENTO NO VALIDO COMO FACTURA", normal));

            PdfPCell rightCell = new PdfPCell(datosVenta);
            rightCell.Border = 0;

            right.AddCell(rightCell);

            // 🔹 SOLO borde inferior
            PdfPCell rightContainer = new PdfPCell(right);
            rightContainer.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
            rightContainer.FixedHeight = 70f;
            rightContainer.PaddingLeft = 15f;

            mainTable.AddCell(rightContainer);


            doc.Add(mainTable);
            doc.Add(new Paragraph(" "));
        }

        class LineaVerticalConBase : IPdfPCellEvent
        {
            public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
            {
                PdfContentByte canvas = canvases[PdfPTable.LINECANVAS];

                float xCentro = (position.Left + position.Right) / 2;

                // 🔹 Línea vertical (centro)
                canvas.MoveTo(xCentro, position.Top);
                canvas.LineTo(xCentro, position.Bottom);

                // 🔹 Línea horizontal (abajo)
                canvas.MoveTo(position.Left, position.Bottom);
                canvas.LineTo(position.Right, position.Bottom);

                canvas.Stroke();
            }
        }
        public void GenerarVentasExcel(long unaVenta)
        {
            var instVentas = new Clases.ClassVentas();
            DataTable dt = instVentas.imprimirVenta(unaVenta);

            if (dt.Rows.Count == 0) return;

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            string path = Path.Combine(downloads,
                $"Venta_{unaVenta}_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx");

            var culture = new CultureInfo("es-AR");

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Venta");

                int fila = 1;
                DataRow cab = dt.Rows[0];

                // ================= HEADER =================
                if (cab["imagen"] != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])cab["imagen"];
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        ws.AddPicture(ms).MoveTo(ws.Cell("A1"), 5, 5).WithSize(80, 40);
                    }
                }

                ws.Column(1).Width = 20;
                ws.Column(2).Width = 5;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;

                ws.Range("A1:C5").Merge().Value =
                    Clases.ClassValidacion.traerEmpresa() + "\n\n" +
                    Clases.ClassValidacion.traerRazonSocial() + "\n" +
                    "Tel: " + Clases.ClassValidacion.traerEmpresaTelefono() + "\n" +
                    Clases.ClassValidacion.traerEmpresaDireccion();

                ws.Range("D1:D2").Merge().Value = "X";
                ws.Range("E1:I5").Merge().Value =
                    $"NRO. VENTA: {cab["nroVenta"]}\n\nFECHA: {Convert.ToDateTime(cab["fecha"]):dd/MM/yyyy}";

                fila = 7;

                // ================= TABLA =================
                string[] headers = {
            "C. Barras","C. Prov","Descripción",
            "P Lista","%","P S/IVA","P C/IVA","Cant","Subtotal"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(fila, i + 1).Value = headers[i];
                    ws.Cell(fila, i + 1).Style.Font.Bold = true;
                }

                fila++;

                decimal totalSinIva = 0;
                decimal totalConIva = 0;
                decimal impuesto = 0;
                decimal IVA = 0;

                foreach (DataRow row in dt.Rows)
                {
                    decimal precioSinIva = Convert.ToDecimal(row["precioSinIva"]);
                    decimal desc = Convert.ToDecimal(row["descuento_Linea"]);
                    decimal rec = Convert.ToDecimal(row["recargo_linea"]);
                    decimal cantidad = Convert.ToDecimal(row["cantidad"]);
                    decimal precioConIva = Convert.ToDecimal(row["precioConIva"]);

                    decimal porcentaje = rec > 0 ? rec : -desc;
                    decimal precioAjustado = Convert.ToDecimal(row["subtotalSinIVA"]);

                    ws.Cell(fila, 1).Value = row["codBarras"].ToString();
                    ws.Cell(fila, 2).Value = row["codProveedor"].ToString();
                    ws.Cell(fila, 3).Value = row["descripcion"].ToString();
                    ws.Cell(fila, 4).Value = precioSinIva;
                    ws.Cell(fila, 5).Value = porcentaje;
                    ws.Cell(fila, 6).Value = precioAjustado;
                    ws.Cell(fila, 7).Value = precioConIva;
                    ws.Cell(fila, 8).Value = cantidad;
                    ws.Cell(fila, 9).Value = Convert.ToDecimal(row["subtotalIVA"]);

                    for (int col = 4; col <= 9; col++)
                    {
                        ws.Cell(fila, col).Style.NumberFormat.Format = "#,##0.00";
                        ws.Cell(fila, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }

                    totalSinIva += precioAjustado * cantidad;
                    totalConIva += precioConIva * cantidad;
                    impuesto = Convert.ToDecimal(row["impuesto"]);
                    IVA = Convert.ToDecimal(row["IVA"]);

                    fila++;
                }

                fila += 2;

                // ================= MEDIOS DE PAGO (IZQUIERDA) =================
                int filaBase = fila;

                ws.Cell(filaBase, 1).Value = "Medios de Pago:";
                ws.Cell(filaBase, 1).Style.Font.Bold = true;

                string mp1 = cab["Medio_Pago_1"]?.ToString();
                string mp2 = cab["Medio_Pago_2"]?.ToString();
                string mp3 = cab["Medio_Pago_3"]?.ToString();

                List<string> mediosPago = new List<string>();

                if (!string.IsNullOrEmpty(mp1) && mp1 != "Sin Especificar") mediosPago.Add(mp1);
                if (!string.IsNullOrEmpty(mp2) && mp2 != "Sin Especificar") mediosPago.Add(mp2);
                if (!string.IsNullOrEmpty(mp3) && mp3 != "Sin Especificar") mediosPago.Add(mp3);

                int filaMP = filaBase + 1;

                foreach (var mp in mediosPago)
                {
                    ws.Cell(filaMP, 1).Value = "• " + mp;
                    filaMP++;
                }

                if (mediosPago.Count == 0)
                {
                    ws.Cell(filaMP, 1).Value = "Sin Medios de Pago";
                }

                // ================= TOTALES (DERECHA) =================
                int colInicio = 6;
                int filaTot = filaBase;

                decimal ivaCalculado = IVA == 0 ? 0 : totalConIva - totalSinIva;
                decimal percepcion = impuesto == 0 ? 0 : totalSinIva * (impuesto / 100);
                decimal totalGeneral = totalSinIva + ivaCalculado + percepcion;

                void SetTotal(string label, decimal v1, decimal v2, bool bold = false)
                {
                    ws.Cell(filaTot, colInicio).Value = label;
                    ws.Cell(filaTot, colInicio).Style.Font.Bold = true;

                    ws.Cell(filaTot, colInicio + 1).Value = v1;
                    ws.Cell(filaTot, colInicio + 2).Value = v2;

                    ws.Range(filaTot, colInicio, filaTot, colInicio + 2)
                      .Style.Fill.BackgroundColor = XLColor.LightGray;

                    ws.Range(filaTot, colInicio + 1, filaTot, colInicio + 2)
                      .Style.NumberFormat.Format = "#,##0.00";

                    ws.Range(filaTot, colInicio + 1, filaTot, colInicio + 2)
                      .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    if (bold)
                        ws.Range(filaTot, colInicio, filaTot, colInicio + 2).Style.Font.Bold = true;

                    filaTot++;
                }

                ws.Cell(filaTot, colInicio).Value = "Total Sin IVA";
                ws.Cell(filaTot, colInicio + 1).Value = totalSinIva;
                ws.Range(filaTot, colInicio + 1, filaTot, colInicio + 2).Merge();
                filaTot++;

                SetTotal("IVA", IVA, ivaCalculado);
                SetTotal("Percepción IIBB", impuesto, percepcion);
                SetTotal("TOTAL GENERAL", 0, totalGeneral, true);

                ws.Columns().AdjustToContents();

                wb.SaveAs(path);
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        public void GenerarDevolucionPDF(long unaDevolucion)
        {
            var instVentas = new Clases.ClassVentas();
            DataTable dt = instVentas.imprimirDevolucion(unaDevolucion);

            if (dt.Rows.Count == 0) return;

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            string path = Path.Combine(downloads,$"Devolucion_{unaDevolucion}_{DateTime.Now:ddMMyyyy_HHmmss}.pdf");

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                Font bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 8);

                DataRow cab = dt.Rows[0];

                // ================= HEADER =================

                AgregarHeaderDevolucion(doc, cab);

                // ================= CLIENTE =================
                // 🔹 Sres
                Paragraph p1 = new Paragraph();
                p1.Add(new Chunk("Sres: ", bold));
                p1.Add(new Chunk(cab["nombreComercial"].ToString(), normal));
                doc.Add(p1);

                Chunk glue = new Chunk(new VerticalPositionMark());

                // 🔹 Razón Social + CUIT
                Paragraph p2 = new Paragraph();

                p2.Add(new Chunk("Razón Social: ", bold));
                p2.Add(new Chunk(cab["razonSocial"].ToString(), normal));

                p2.Add(glue); // 🔥 empuja lo siguiente a la derecha

                p2.Add(new Chunk("CUIT: ", bold));
                p2.Add(new Chunk(cab["cuil"].ToString(), normal));

                doc.Add(p2);

                // 🔹 Dirección + Teléfono
                Paragraph p3 = new Paragraph();

                p3.Add(new Chunk("Dirección: ", bold));
                p3.Add(new Chunk(cab["Direccion"].ToString(), normal));

                p3.Add(glue); // 🔥 alinea a la derecha

                p3.Add(new Chunk("Tel: ", bold));
                p3.Add(new Chunk(Clases.ClassValidacion.traerEmpresaTelefono(), normal));

                doc.Add(p3);

                // 🔹 Condición IVA
                Paragraph p4 = new Paragraph();
                p4.Add(new Chunk("Cond. IVA: ", bold));
                p4.Add(new Chunk(cab["IVA"].ToString(), normal));
                doc.Add(p4);

                // 🔹 Espacio
                doc.Add(new Paragraph(" "));

                // ================= TABLA =================
                // 🔹 Fuentes
                Font bold8 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
                Font normal8 = FontFactory.GetFont(FontFactory.HELVETICA, 8);

                // 🔹 Tabla
                PdfPTable table = new PdfPTable(9);
                table.WidthPercentage = 100;

                // 🔹 Anchos (ajustables)
                table.SetWidths(new float[] { 3, 2, 8, 2, 2, 2, 2, 2, 2 });

                // 🔹 Encabezados
                string[] headers = {
                                    "C. Barras",
                                    "C. Prov",
                                    "Descripción",
                                    "P Lista",
                                    "%",
                                    "P S/IVA",
                                    "P C/IVA",
                                    "Cant",
                                    "Subtotal"
                                };

                foreach (var h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, bold8));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.Padding = 3f;

                    table.AddCell(cell);
                }

                // 🔹 Detalle
                decimal total = 0;
                var culture = new System.Globalization.CultureInfo("es-AR");
                foreach (DataRow row in dt.Rows)
                {
                    decimal precioSinIva = Convert.ToDecimal(row["precioSinIva"]);
                    decimal desc = Convert.ToDecimal(row["descuento_Linea"]);
                    decimal rec = Convert.ToDecimal(row["recargo_linea"]);
                    decimal porcentaje = rec > 0 ? rec : -desc;

                    decimal precioAjustado = Convert.ToDecimal(row["subtotalSinIVA"]);

                    // C. Barras
                    table.AddCell(new PdfPCell(new Phrase(row["codBarras"].ToString(), normal8)));

                    // C. Prov
                    table.AddCell(new PdfPCell(new Phrase(row["codProveedor"].ToString(), normal8)));

                    // Descripción
                    table.AddCell(new PdfPCell(new Phrase(row["descripcion"].ToString(), normal8)));

                    // 🔹 P Lista
                    PdfPCell c1 = new PdfPCell(new Phrase(precioSinIva.ToString("N2", culture), normal8));
                    c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(c1);

                    // 🔹 %
                    PdfPCell c2 = new PdfPCell(new Phrase(porcentaje.ToString("N2", culture), normal8));
                    c2.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(c2);

                    // 🔹 P S/IVA
                    PdfPCell c3 = new PdfPCell(new Phrase(precioAjustado.ToString("N2", culture), normal8));
                    c3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(c3);

                    // 🔹 P C/IVA
                    PdfPCell c4 = new PdfPCell(new Phrase(Convert.ToDecimal(row["precioConIva"]).ToString("N2", culture), normal8));
                    c4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(c4);

                    // 🔹 Cantidad (solo alineado, sin miles obligatorio)
                    PdfPCell c5 = new PdfPCell(new Phrase(Convert.ToDecimal(row["cantidad"]).ToString("N2", culture), normal8));
                    c5.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(c5);

                    // 🔹 Subtotal
                    PdfPCell c6 = new PdfPCell(new Phrase(Convert.ToDecimal(row["subtotalIVA"]).ToString("N2", culture), normal8));
                    c6.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(c6);
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));

                // ================= TOTALES =================
                decimal totalSinIva = 0;
                decimal totalConIva = 0;
                decimal impuesto = 0;
                decimal IVA = 0;
                // 🔹 recorrer nuevamente o acumular en el foreach anterior
                foreach (DataRow row in dt.Rows)
                {
                    decimal precioSinIva = Convert.ToDecimal(row["subtotalSinIVA"]);
                    decimal precioConIva = Convert.ToDecimal(row["precioConIva"]);
                    decimal cantidad = Convert.ToDecimal(row["cantidad"]);
                    decimal desc = Convert.ToDecimal(row["descuento_Linea"]);
                    decimal rec = Convert.ToDecimal(row["recargo_linea"]);


                    totalSinIva += precioSinIva * cantidad;
                    totalConIva += precioConIva * cantidad;

                    impuesto = Convert.ToDecimal(row["impuesto"]); // toma uno (si es el mismo para todos)
                    IVA = Convert.ToDecimal(row["IVA"]);
                }



                decimal ivaCalculado = IVA == 0 ? 0 : totalConIva - totalSinIva;
                decimal percepcion = impuesto == 0 ? 0 : totalSinIva * (impuesto / 100);

                decimal totalGeneral = totalSinIva + ivaCalculado + percepcion;

                PdfPTable tablaTotales = new PdfPTable(3);
                tablaTotales.WidthPercentage = 50; // 🔹 mitad de la hoja
                tablaTotales.HorizontalAlignment = Element.ALIGN_RIGHT;

                tablaTotales.SetWidths(new float[] { 6, 1, 3 });

                // 🔹 Colores
                BaseColor grisClaro = new BaseColor(230, 230, 230);

                // 🔹 Fuentes
                Font bold9 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
                Font normal9 = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                PdfPCell f1c1 = new PdfPCell(new Phrase("Total Sin IVA", bold9));
                f1c1.BackgroundColor = grisClaro;

                PdfPCell f1c2 = new PdfPCell(new Phrase(totalSinIva.ToString("N2", culture), normal9));
                f1c2.HorizontalAlignment = Element.ALIGN_RIGHT;
                f1c2.BackgroundColor = grisClaro;
                f1c2.Colspan = 2;

                tablaTotales.AddCell(f1c1);
                tablaTotales.AddCell(f1c2);

                tablaTotales.AddCell(new PdfPCell(new Phrase("IVA", bold9)) { BackgroundColor = grisClaro });

                tablaTotales.AddCell(new PdfPCell(new Phrase(IVA.ToString("N2", culture), normal9))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = grisClaro
                });

                tablaTotales.AddCell(new PdfPCell(new Phrase(ivaCalculado.ToString("N2", culture), normal9))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = grisClaro
                });

                tablaTotales.AddCell(new PdfPCell(new Phrase("Percep. IIBB PCIA. Chaco - Misiones", bold9)) { BackgroundColor = grisClaro });

                tablaTotales.AddCell(new PdfPCell(new Phrase(impuesto.ToString("N2", culture), normal9))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = grisClaro
                });

                tablaTotales.AddCell(new PdfPCell(new Phrase(percepcion.ToString("N2", culture), normal9))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = grisClaro
                });

                tablaTotales.AddCell(new PdfPCell(new Phrase("TOTAL GENERAL", bold9)) { BackgroundColor = grisClaro });

                tablaTotales.AddCell(new PdfPCell(new Phrase("", normal9))
                {
                    BackgroundColor = grisClaro
                });

                tablaTotales.AddCell(new PdfPCell(new Phrase(totalGeneral.ToString("N2", culture), bold9))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = grisClaro
                });

                doc.Add(new Paragraph(" "));
                doc.Add(tablaTotales);

                doc.Close();
            }

            // 🔥 ABRIR AUTOMÁTICAMENTE
            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        private void AgregarHeaderDevolucion(Document doc, DataRow cab)
        {
            Font bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            PdfPTable mainTable = new PdfPTable(3);
            mainTable.WidthPercentage = 100;
            mainTable.SetWidths(new float[] { 4, 1, 5 });
            mainTable.DefaultCell.Border = Rectangle.NO_BORDER;

            // ================= IZQUIERDA (LOGO + EMPRESA) =================
            PdfPTable left = new PdfPTable(2);
            left.WidthPercentage = 100;
            left.SetWidths(new float[] { 1, 3 });

            if (cab["imagen"] != DBNull.Value)
            {
                Image logo = Image.GetInstance((byte[])cab["imagen"]);
                logo.ScaleToFit(70, 40);

                PdfPCell logoCell = new PdfPCell(logo);
                logoCell.Border = 0;
                logoCell.PaddingTop = 8f;
                logoCell.PaddingLeft = 5f;
                logoCell.PaddingRight = 5f;
                logoCell.VerticalAlignment = Element.ALIGN_TOP;

                left.AddCell(logoCell);
            }

            Paragraph empresa = new Paragraph();
            empresa.Alignment = Element.ALIGN_CENTER;
            empresa.SetLeading(0f, 1.2f); // 🔥 probá entre 1.1 y 1.5

            empresa.Add(new Chunk(Clases.ClassValidacion.traerEmpresa(), bold));
            empresa.Add(Chunk.NEWLINE);
            empresa.Add(Chunk.NEWLINE);

            empresa.Add(new Chunk(Clases.ClassValidacion.traerRazonSocial(), normal));
            empresa.Add(Chunk.NEWLINE);
            empresa.Add(new Chunk("Tel: " + Clases.ClassValidacion.traerEmpresaTelefono(), normal));
            empresa.Add(Chunk.NEWLINE);
            empresa.Add(new Chunk(Clases.ClassValidacion.traerEmpresaDireccion(), normal));
            empresa.Add(Chunk.NEWLINE);
            empresa.Add(new Chunk(Clases.ClassValidacion.traerEmpresaCiudad(), normal));

            PdfPCell empresaCell = new PdfPCell(empresa);
            empresaCell.Border = 0;
            empresaCell.VerticalAlignment = Element.ALIGN_TOP;
            empresaCell.PaddingLeft = 10f;
            empresaCell.HorizontalAlignment = Element.ALIGN_CENTER;

            left.AddCell(empresaCell);

            // 🔹 Contenedor SOLO con borde inferior
            PdfPCell leftContainer = new PdfPCell(left);
            leftContainer.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER;
            leftContainer.FixedHeight = 70f;

            mainTable.AddCell(leftContainer);

            // ================= CENTRO (X EN RECUADRO) =================
            PdfPTable tablaX = new PdfPTable(1);
            tablaX.WidthPercentage = 100;

            // 🔲 Cuadro de la X
            PdfPCell cellX = new PdfPCell(new Phrase("X", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18)));
            cellX.HorizontalAlignment = Element.ALIGN_CENTER;
            cellX.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellX.Border = Rectangle.BOX;
            cellX.FixedHeight = 30;
            tablaX.AddCell(cellX);

            // 🔽 Línea central + base
            PdfPCell cellLinea = new PdfPCell(new Phrase(""));
            cellLinea.Border = Rectangle.NO_BORDER;
            cellLinea.FixedHeight = 40;
            cellLinea.CellEvent = new LineaVerticalConBase();

            tablaX.AddCell(cellLinea);

            // 🔹 Contenedor sin bordes
            PdfPCell contenedorX = new PdfPCell(tablaX);
            contenedorX.Border = Rectangle.NO_BORDER;
            contenedorX.FixedHeight = 70f;

            mainTable.AddCell(contenedorX);
            // ================= DERECHA (DATOS VENTA) =================
            PdfPTable right = new PdfPTable(1);
            right.WidthPercentage = 100;

            Paragraph datosVenta = new Paragraph();
            datosVenta.Alignment = Element.ALIGN_LEFT;

            datosVenta.Add(new Chunk($"DEVOLUCIÓN NRO: {cab["nroDev"]}", bold));
            datosVenta.Add(Chunk.NEWLINE);
            datosVenta.Add(Chunk.NEWLINE);

            datosVenta.Add(new Chunk($"FECHA: {Convert.ToDateTime(cab["fecha"]):dd/MM/yyyy}", bold));
            datosVenta.Add(Chunk.NEWLINE);
            datosVenta.Add(Chunk.NEWLINE);

            datosVenta.Add(new Chunk($"CUIT: {cab["cuil"]}", normal));
            datosVenta.Add(Chunk.NEWLINE);
            datosVenta.Add(new Chunk("DOCUMENTO NO VALIDO COMO FACTURA", normal));

            PdfPCell rightCell = new PdfPCell(datosVenta);
            rightCell.Border = 0;

            right.AddCell(rightCell);

            // 🔹 SOLO borde inferior
            PdfPCell rightContainer = new PdfPCell(right);
            rightContainer.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
            rightContainer.FixedHeight = 70f;
            rightContainer.PaddingLeft = 15f;

            mainTable.AddCell(rightContainer);


            doc.Add(mainTable);
            doc.Add(new Paragraph(" "));
        }

        public void GenerarDevolucionExcel(long unaDevolucion)
        {
            var instVentas = new Clases.ClassVentas();
            DataTable dt = instVentas.imprimirDevolucion(unaDevolucion); // 🔥 CAMBIO

            if (dt.Rows.Count == 0) return;

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            string path = Path.Combine(downloads,
                $"Devolucion_{unaDevolucion}_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx");

            var culture = new CultureInfo("es-AR");

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Devolución");

                int fila = 1;
                DataRow cab = dt.Rows[0];

                // ================= LOGO =================
                if (cab["imagen"] != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])cab["imagen"];

                    using (var ms = new MemoryStream(imageBytes))
                    {
                        var picture = ws.AddPicture(ms)
                            .MoveTo(ws.Cell("A1"), 5, 5)
                            .WithSize(80, 40);

                        picture.WithPlacement(XLPicturePlacement.FreeFloating);
                    }
                }

                // ================= COLUMNAS =================
                ws.Column(1).Width = 20;
                ws.Column(2).Width = 5;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 15;
                ws.Column(8).Width = 15;
                ws.Column(9).Width = 15;

                // ================= IZQUIERDA =================
                ws.Range("A1:C5").Merge().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                var leftCell = ws.Cell("A1");
                leftCell.Value =
                    Clases.ClassValidacion.traerEmpresa() + "\n\n" +
                    Clases.ClassValidacion.traerRazonSocial() + "\n" +
                    "Tel: " + Clases.ClassValidacion.traerEmpresaTelefono() + "\n" +
                    Clases.ClassValidacion.traerEmpresaDireccion() + "\n" +
                    Clases.ClassValidacion.traerEmpresaCiudad();

                leftCell.Style.Alignment.WrapText = true;
                leftCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                leftCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                leftCell.Style.Font.Bold = true;

                // ================= CENTRO =================
                ws.Range("D1:D2").Merge().Value = "X";
                ws.Range("D1:D2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("D1:D2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("D1:D2").Style.Font.Bold = true;
                ws.Range("D1:D2").Style.Font.FontSize = 16;
                ws.Range("D1:D2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                ws.Range("D3:D5").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("D3:D5").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Range("D5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                // ================= DERECHA =================
                ws.Range("E1:I5").Merge().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                var rightCell = ws.Cell("E1");
                rightCell.Value =
                    $"DEVOLUCIÓN NRO: {cab["nroDev"]}\n\n" + // 🔥 CAMBIO TEXTO
                    $"FECHA: {Convert.ToDateTime(cab["fecha"]):dd/MM/yyyy}\n\n" +
                    $"CUIT: {cab["cuil"]}\n" +
                    "DOCUMENTO NO VALIDO COMO FACTURA";

                rightCell.Style.Alignment.WrapText = true;
                rightCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                rightCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                fila = 7;

                // ================= TABLA =================
                string[] headers = {
            "C. Barras","C. Prov","Descripción",
            "P Lista","%","P S/IVA","P C/IVA","Cant","Subtotal"
        };

                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(fila, i + 1).Value = headers[i];
                    ws.Cell(fila, i + 1).Style.Font.Bold = true;
                    ws.Cell(fila, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                fila++;

                decimal totalSinIva = 0;
                decimal totalConIva = 0;
                decimal impuesto = 0;
                decimal IVA = 0;

                foreach (DataRow row in dt.Rows)
                {
                    decimal precioSinIva = Convert.ToDecimal(row["precioSinIva"]);
                    decimal desc = Convert.ToDecimal(row["descuento_Linea"]);
                    decimal rec = Convert.ToDecimal(row["recargo_linea"]);
                    decimal cantidad = Convert.ToDecimal(row["cantidad"]);
                    decimal precioConIva = Convert.ToDecimal(row["precioConIva"]);

                    decimal porcentaje = rec > 0 ? rec : -desc;
                    decimal precioAjustado = Convert.ToDecimal(row["subtotalSinIVA"]);

                    ws.Cell(fila, 1).Value = row["codBarras"].ToString();
                    ws.Cell(fila, 2).Value = row["codProveedor"].ToString();
                    ws.Cell(fila, 3).Value = row["descripcion"].ToString();

                    ws.Cell(fila, 4).Value = precioSinIva;
                    ws.Cell(fila, 5).Value = porcentaje;
                    ws.Cell(fila, 6).Value = precioAjustado;
                    ws.Cell(fila, 7).Value = precioConIva;
                    ws.Cell(fila, 8).Value = cantidad;
                    ws.Cell(fila, 9).Value = Convert.ToDecimal(row["subtotalIVA"]);

                    for (int col = 4; col <= 9; col++)
                    {
                        ws.Cell(fila, col).Style.NumberFormat.Format = "#,##0.00";
                        ws.Cell(fila, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }

                    totalSinIva += precioAjustado * cantidad;
                    totalConIva += precioConIva * cantidad;

                    impuesto = Convert.ToDecimal(row["impuesto"]);
                    IVA = Convert.ToDecimal(row["IVA"]);

                    fila++;
                }

                fila++;

                // ================= TOTALES =================
                int colInicio = 6;

                decimal ivaCalculado = IVA == 0 ? 0 : totalConIva - totalSinIva;
                decimal percepcion = impuesto == 0 ? 0 : totalSinIva * (impuesto / 100);
                decimal totalGeneral = totalSinIva + ivaCalculado + percepcion;

                void SetTotalRow(string label, decimal value1, decimal value2, bool bold = false)
                {
                    ws.Cell(fila, colInicio).Value = label;
                    ws.Cell(fila, colInicio).Style.Font.Bold = true;

                    ws.Cell(fila, colInicio + 1).Value = value1;
                    ws.Cell(fila, colInicio + 2).Value = value2;

                    ws.Range(fila, colInicio, fila, colInicio + 2).Style.Fill.BackgroundColor = XLColor.LightGray;

                    ws.Range(fila, colInicio + 1, fila, colInicio + 2)
                      .Style.NumberFormat.Format = "#,##0.00";

                    ws.Range(fila, colInicio + 1, fila, colInicio + 2)
                      .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    if (bold)
                        ws.Range(fila, colInicio, fila, colInicio + 2).Style.Font.Bold = true;

                    fila++;
                }

                ws.Cell(fila, colInicio).Value = "Total Sin IVA";
                ws.Cell(fila, colInicio).Style.Font.Bold = true;
                ws.Cell(fila, colInicio + 1).Value = totalSinIva;
                ws.Range(fila, colInicio + 1, fila, colInicio + 2).Merge();
                ws.Range(fila, colInicio, fila, colInicio + 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                fila++;

                SetTotalRow("IVA", IVA, ivaCalculado);
                SetTotalRow("Percep. IIBB PCIA. Chaco - Misiones", impuesto, percepcion);
                SetTotalRow("TOTAL GENERAL", 0, totalGeneral, true);

                ws.Columns().AdjustToContents();

                wb.SaveAs(path);
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        // ════════════════════════════════════════════════════════════════════════
        // PEDIDO — PDF visualmente equivalente al reporte web
        // C:\Desarrollos\Comercial-master\Comercial Web\ComercialWeb\Pages\Ventas\ReportePedidos\Imprimir
        // Data: sp_PedidosPrintPedido(unPedido) — cabecera+detalle aplanados.
        // Teléfono/Contacto se obtienen de ClassClientes.traerDatosVenta (no están en el SP).
        // ════════════════════════════════════════════════════════════════════════
        public void GenerarPedidoPDF(int unPedido, int unCliente)
        {
            var instPed = new Clases.ClassPedidos();
            DataTable dt = instPed.traerImpresionPedido(unPedido);
            if (dt.Rows.Count == 0) return;

            // Datos extra del cliente que no vienen en el SP
            var instClie = new Clases.ClassClientes();
            DataTable cli = instClie.traerDatosVenta(" and c.id = " + unCliente);

            string clienteTelefono   = cli.Rows.Count > 0 ? cli.Rows[0]["Tel"].ToString()      : "";
            string clienteContacto   = cli.Rows.Count > 0 ? cli.Rows[0]["contacto"].ToString() : "";
            string clienteDireccion  = "";
            if (cli.Rows.Count > 0)
            {
                clienteDireccion = cli.Rows[0]["Dir"].ToString()
                                 + (string.IsNullOrEmpty(cli.Rows[0]["Localidad"].ToString())  ? "" : ". " + cli.Rows[0]["Localidad"].ToString())
                                 + (string.IsNullOrEmpty(cli.Rows[0]["Provincia"].ToString())  ? "" : ". " + cli.Rows[0]["Provincia"].ToString());
            }

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            string path = Path.Combine(downloads, $"Pedido_{unPedido}_{DateTime.Now:ddMMyyyy_HHmmss}.pdf");

            var culture   = new CultureInfo("es-AR");
            int cantStock = Clases.ClassProductos.cantDecimalesStock();

            // ─── Paleta (matchea CSS web) ───────────────────────────────────
            BaseColor cAzul       = new BaseColor(26, 58, 92);     // #1a3a5c
            BaseColor cGrisFondo  = new BaseColor(248, 249, 252);  // #f8f9fc
            BaseColor cGrisBorde  = new BaseColor(227, 230, 240);  // #e3e6f0
            BaseColor cGrisLabel  = new BaseColor(108, 117, 125);  // #6c757d
            BaseColor cBlanco     = BaseColor.WHITE;
            BaseColor cTextoOsc   = new BaseColor(44, 62, 80);     // #2c3e50
            BaseColor cTextoSec   = new BaseColor(85, 85, 85);     // #555
            BaseColor cVerde      = new BaseColor(26, 122, 69);    // #1a7a45
            BaseColor cRojo       = new BaseColor(192, 57, 43);    // #c0392b
            BaseColor cTextoNeg   = new BaseColor(34, 34, 34);     // #222

            // ─── Fuentes ────────────────────────────────────────────────────
            Font fEmpNombre  = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, cAzul);
            Font fEmpRazon   = FontFactory.GetFont(FontFactory.HELVETICA,      9, cTextoOsc);
            Font fEmpSub     = FontFactory.GetFont(FontFactory.HELVETICA,      8, cTextoSec);
            Font fEmpSubLbl  = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, cTextoSec);
            Font fPedTitulo  = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, cBlanco);
            Font fPedFecha   = FontFactory.GetFont(FontFactory.HELVETICA,       9, cBlanco);
            Font fCabLabel   = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  7, cGrisLabel);
            Font fCabValor   = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  9, cTextoOsc);
            Font fObsLabel   = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  7, cGrisLabel);
            Font fObsValor   = FontFactory.GetFont(FontFactory.HELVETICA,       9, cTextoNeg);
            // Tabla detalle: tipografía reducida para que entren los headers ("Precio s/IVA", "Subt. s/IVA", etc.)
            Font fThead      = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  7.5f, cBlanco);
            Font fTbody      = FontFactory.GetFont(FontFactory.HELVETICA,       7.5f, cTextoNeg);
            Font fTbodySmall = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 6.5f, cRojo);
            Font fTbodyBold  = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  7.5f, cTextoNeg);
            Font fTotLab     = FontFactory.GetFont(FontFactory.HELVETICA,       9, cTextoNeg);
            Font fTotVal     = FontFactory.GetFont(FontFactory.HELVETICA,       9, cTextoNeg);
            Font fTotTotLab  = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, cBlanco);
            Font fTotTotVal  = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, cBlanco);
            Font fFirma      = FontFactory.GetFont(FontFactory.HELVETICA,       8, cTextoSec);

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                // 1.5cm de margen como el CSS web (1cm = 28.35pt)
                Document doc = new Document(PageSize.A4, 42.5f, 42.5f, 42.5f, 42.5f);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                DataRow cab = dt.Rows[0];

                // ═════════════════════════════════════════════════════════════
                // ENCABEZADO EMPRESA (logo + datos, separado por línea azul)
                // ═════════════════════════════════════════════════════════════
                PdfPTable empHeader = new PdfPTable(2);
                empHeader.WidthPercentage = 100;
                empHeader.SetWidths(new float[] { 1.2f, 4f });

                PdfPCell logoCell;
                if (cab.Table.Columns.Contains("imagen") && cab["imagen"] != DBNull.Value)
                {
                    try
                    {
                        Image logo = Image.GetInstance((byte[])cab["imagen"]);
                        logo.ScaleToFit(120, 60);
                        logoCell = new PdfPCell(logo);
                    }
                    catch
                    {
                        logoCell = new PdfPCell(new Phrase(""));
                    }
                }
                else
                {
                    logoCell = new PdfPCell(new Phrase(""));
                }
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.PaddingBottom = 8f;
                logoCell.VerticalAlignment = Element.ALIGN_TOP;
                empHeader.AddCell(logoCell);

                // Datos de empresa (parametros: nombre, razonSocial, direccion, localidad, cuit, telefono)
                string empNombre   = Clases.ClassValidacion.traerEmpresa();
                string empRazon    = Clases.ClassValidacion.traerRazonSocial();
                string empDir      = Clases.ClassValidacion.traerEmpresaDireccion();
                string empLocal    = Clases.ClassValidacion.traerEmpresaCiudad();
                string empCuit     = Clases.ClassValidacion.traerEmpresaCuit();
                string empTel      = Clases.ClassValidacion.traerEmpresaTelefono();
                const string SEP = "   ·   ";

                Paragraph empDatos = new Paragraph();
                empDatos.SetLeading(0f, 1.35f); // espaciado entre líneas más aireado

                // Línea 1: nombre comercial (grande, azul)
                empDatos.Add(new Chunk(empNombre, fEmpNombre));
                empDatos.Add(Chunk.NEWLINE);

                // Línea 2: razón social
                if (!string.IsNullOrEmpty(empRazon))
                {
                    empDatos.Add(new Chunk(empRazon, fEmpRazon));
                    empDatos.Add(Chunk.NEWLINE);
                }

                // Línea 3: dirección · localidad (espaciado horizontal)
                bool tieneDir   = !string.IsNullOrEmpty(empDir);
                bool tieneLocal = !string.IsNullOrEmpty(empLocal);
                if (tieneDir || tieneLocal)
                {
                    if (tieneDir)   empDatos.Add(new Chunk(empDir, fEmpSub));
                    if (tieneDir && tieneLocal) empDatos.Add(new Chunk(SEP, fEmpSub));
                    if (tieneLocal) empDatos.Add(new Chunk(empLocal, fEmpSub));
                    empDatos.Add(Chunk.NEWLINE);
                }

                // Línea 4: CUIT · Tel (con label en negrita)
                bool tieneCuit = !string.IsNullOrEmpty(empCuit);
                bool tieneTel  = !string.IsNullOrEmpty(empTel);
                if (tieneCuit || tieneTel)
                {
                    if (tieneCuit)
                    {
                        empDatos.Add(new Chunk("CUIT: ", fEmpSubLbl));
                        empDatos.Add(new Chunk(empCuit, fEmpSub));
                    }
                    if (tieneCuit && tieneTel) empDatos.Add(new Chunk(SEP, fEmpSub));
                    if (tieneTel)
                    {
                        empDatos.Add(new Chunk("Tel: ", fEmpSubLbl));
                        empDatos.Add(new Chunk(empTel, fEmpSub));
                    }
                }

                PdfPCell empDatCell = new PdfPCell(empDatos);
                empDatCell.Border = Rectangle.NO_BORDER;
                empDatCell.PaddingLeft = 12f;
                empDatCell.PaddingBottom = 8f;
                empDatCell.VerticalAlignment = Element.ALIGN_TOP;
                empHeader.AddCell(empDatCell);

                doc.Add(empHeader);

                // Línea azul gruesa que separa el header empresa
                LineSeparator lineaAzul = new LineSeparator(2f, 100, cAzul, Element.ALIGN_CENTER, -1);
                doc.Add(new Chunk(lineaAzul));
                doc.Add(new Paragraph(" ") { SpacingAfter = 4f });

                // ═════════════════════════════════════════════════════════════
                // TITULO PEDIDO (banner azul: "PEDIDO N.° X" + fecha)
                // ═════════════════════════════════════════════════════════════
                PdfPTable banner = new PdfPTable(2);
                banner.WidthPercentage = 100;
                banner.SetWidths(new float[] { 3, 1 });

                PdfPCell bcIzq = new PdfPCell(new Phrase("PEDIDO N.° " + cab["Pedido"].ToString(), fPedTitulo));
                bcIzq.BackgroundColor = cAzul;
                bcIzq.Border = Rectangle.NO_BORDER;
                bcIzq.HorizontalAlignment = Element.ALIGN_LEFT;
                bcIzq.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcIzq.PaddingTop = 6f; bcIzq.PaddingBottom = 6f; bcIzq.PaddingLeft = 10f;

                string fechaTxt = cab["fecha"] != DBNull.Value
                    ? Convert.ToDateTime(cab["fecha"]).ToString("dd/MM/yyyy")
                    : "";
                PdfPCell bcDer = new PdfPCell(new Phrase(fechaTxt, fPedFecha));
                bcDer.BackgroundColor = cAzul;
                bcDer.Border = Rectangle.NO_BORDER;
                bcDer.HorizontalAlignment = Element.ALIGN_RIGHT;
                bcDer.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcDer.PaddingTop = 6f; bcDer.PaddingBottom = 6f; bcDer.PaddingRight = 10f;

                banner.AddCell(bcIzq);
                banner.AddCell(bcDer);
                banner.SpacingAfter = 10f;
                doc.Add(banner);

                // ═════════════════════════════════════════════════════════════
                // DATOS CABECERA (grid 3 columnas, fondo gris claro)
                // ═════════════════════════════════════════════════════════════
                decimal ivaPct       = cab.Table.Columns.Contains("iva")       && cab["iva"]       != DBNull.Value ? Convert.ToDecimal(cab["iva"])       : 0m;
                decimal descuentoCab = cab.Table.Columns.Contains("descuento") && cab["descuento"] != DBNull.Value ? Convert.ToDecimal(cab["descuento"]) : 0m;
                decimal recargoCab   = cab.Table.Columns.Contains("recargo")   && cab["recargo"]   != DBNull.Value ? Convert.ToDecimal(cab["recargo"])   : 0m;
                string  nombreClie   = cab["nombreComercial"].ToString();
                string  observacion  = cab.Table.Columns.Contains("observacion") ? cab["observacion"].ToString() : "";

                bool mostrarDescRecGlobal = descuentoCab != 0 || recargoCab != 0;

                PdfPTable cabGrid = new PdfPTable(3);
                cabGrid.WidthPercentage = 100;
                cabGrid.DefaultCell.Border = Rectangle.NO_BORDER;
                cabGrid.SpacingAfter = 8f;

                Action<string, string> addCabItem = (label, valor) =>
                {
                    Paragraph p = new Paragraph();
                    p.Add(new Chunk(label.ToUpper(), fCabLabel));
                    p.Add(Chunk.NEWLINE);
                    p.Add(new Chunk(string.IsNullOrEmpty(valor) ? " " : valor, fCabValor));
                    PdfPCell c = new PdfPCell(p);
                    c.BackgroundColor = cGrisFondo;
                    c.BorderColor = cGrisBorde;
                    c.Border = Rectangle.BOX;
                    c.Padding = 6f;
                    cabGrid.AddCell(c);
                };

                addCabItem("Cliente",  nombreClie);
                addCabItem("Telefono", clienteTelefono);
                addCabItem("Contacto", clienteContacto);
                addCabItem("Direccion", clienteDireccion);
                addCabItem("IVA", ivaPct.ToString("N0", culture) + "%");
                if (mostrarDescRecGlobal)
                {
                    string txt = "";
                    if (descuentoCab != 0) txt += "Dto " + descuentoCab.ToString("N2", culture) + "%";
                    if (recargoCab   != 0) txt += "  Rec " + recargoCab.ToString("N2", culture) + "%";
                    addCabItem("Desc / Rec global", txt);
                }
                else
                {
                    // Para que el grid tenga celdas pares (3 columnas, completar fila)
                    PdfPCell empty = new PdfPCell(new Phrase(" "));
                    empty.BackgroundColor = cGrisFondo;
                    empty.BorderColor = cGrisBorde;
                    empty.Border = Rectangle.BOX;
                    cabGrid.AddCell(empty);
                }

                doc.Add(cabGrid);

                // ═════════════════════════════════════════════════════════════
                // OBSERVACION (si hay)
                // ═════════════════════════════════════════════════════════════
                if (!string.IsNullOrWhiteSpace(observacion))
                {
                    PdfPTable obsT = new PdfPTable(1);
                    obsT.WidthPercentage = 100;
                    Paragraph p = new Paragraph();
                    p.Add(new Chunk("OBSERVACION", fObsLabel));
                    p.Add(Chunk.NEWLINE);
                    p.Add(new Chunk(observacion, fObsValor));
                    PdfPCell c = new PdfPCell(p);
                    c.BorderColor = cGrisBorde;
                    c.Border = Rectangle.BOX;
                    c.Padding = 8f;
                    obsT.AddCell(c);
                    obsT.SpacingAfter = 8f;
                    doc.Add(obsT);
                }

                // ═════════════════════════════════════════════════════════════
                // TABLA DETALLE
                // ═════════════════════════════════════════════════════════════
                // Columnas: Cód.Prov | Cód.Barras | Descripción | Stock | Cantidad | Subtotal
                PdfPTable det = new PdfPTable(6);
                det.WidthPercentage = 100;
                det.SetWidths(new float[] { 1.3f, 2.0f, 3.8f, 1.0f, 1.0f, 1.6f });

                string[] heads = { "Cód. Prov.", "Cód. Barras", "Descripción", "Stock", "Cantidad", "Subtotal" };
                for (int i = 0; i < heads.Length; i++)
                {
                    PdfPCell c = new PdfPCell(new Phrase(heads[i], fThead));
                    c.BackgroundColor = cAzul;
                    c.Border = Rectangle.NO_BORDER;
                    c.HorizontalAlignment = i <= 2 ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
                    c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                    c.NoWrap = true;
                    c.PaddingTop = 5f; c.PaddingBottom = 5f;
                    c.PaddingLeft = 3f; c.PaddingRight = 3f;
                    det.AddCell(c);
                }

                // Acumuladores totales
                decimal totPrecioSIva = 0;
                decimal totDescRec    = 0;
                decimal totSubtSIva   = 0;
                decimal totIva        = 0;
                decimal totTotal      = 0;

                decimal ivaRate = ivaPct / 100m;

                // descRecGlobal: web aplica (descuento*-1)+recargo si la cabecera tiene global.
                // En este SP descuento llega a nivel línea; recargo puede no existir.
                // Sin manera fiable de distinguir si la cabecera tiene global, replicamos
                // el cálculo por línea (idéntico a la lógica web cuando no hay global).
                int idx = 0;
                foreach (DataRow l in dt.Rows)
                {
                    decimal precioSIva = l["Precio_S_IVA"] != DBNull.Value ? Convert.ToDecimal(l["Precio_S_IVA"]) : 0m;
                    decimal cant       = l["Cantidad"]    != DBNull.Value ? Convert.ToDecimal(l["Cantidad"])    : 0m;
                    decimal dLinea     = l.Table.Columns.Contains("descuento") && l["descuento"] != DBNull.Value ? Convert.ToDecimal(l["descuento"]) : 0m;
                    decimal rLinea     = l.Table.Columns.Contains("recargo")   && l["recargo"]   != DBNull.Value ? Convert.ToDecimal(l["recargo"])   : 0m;
                    decimal descRecPct = (dLinea * -1m) + rLinea;

                    decimal subSIva    = precioSIva * (1m + descRecPct / 100m);
                    decimal conIva     = subSIva * (1m + ivaRate);

                    totPrecioSIva += precioSIva * cant;
                    totDescRec    += (subSIva - precioSIva) * cant;
                    totSubtSIva   += subSIva * cant;
                    totIva        += (conIva - subSIva) * cant;
                    totTotal      += conIva * cant;

                    BaseColor bgFila = (idx % 2 == 0) ? cBlanco : cGrisFondo;

                    // Helpers de celda
                    Action<string, Font, bool> addCell = (txt, fnt, right) =>
                    {
                        PdfPCell c = new PdfPCell(new Phrase(txt, fnt));
                        c.BackgroundColor     = bgFila;
                        c.BorderColor         = cGrisBorde;
                        c.Border              = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = right ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft = 3f; c.PaddingRight = 3f;
                        det.AddCell(c);
                    };

                    // Cód. Prov.
                    string codProv  = l.Table.Columns.Contains("Cod_Proveedor") && l["Cod_Proveedor"] != DBNull.Value
                                      ? l["Cod_Proveedor"].ToString() : "";
                    addCell(codProv, fTbody, false);

                    // Cód. Barras
                    string codBarras = l.Table.Columns.Contains("Cod_Barras") && l["Cod_Barras"] != DBNull.Value
                                       ? l["Cod_Barras"].ToString() : "";
                    addCell(codBarras, fTbody, false);

                    // Descripción (con observ debajo, gris pequeño)
                    string desc = l["Descripcion"].ToString();
                    string obs  = l.Table.Columns.Contains("Observ") ? l["Observ"].ToString() : "";
                    Paragraph pDesc = new Paragraph();
                    pDesc.Add(new Chunk(desc, fTbody));
                    if (!string.IsNullOrWhiteSpace(obs))
                    {
                        pDesc.Add(Chunk.NEWLINE);
                        pDesc.Add(new Chunk(obs, fTbodySmall));
                    }
                    PdfPCell cDesc = new PdfPCell(pDesc);
                    cDesc.BackgroundColor = bgFila;
                    cDesc.BorderColor     = cGrisBorde;
                    cDesc.Border          = Rectangle.BOTTOM_BORDER;
                    cDesc.PaddingTop = 3f; cDesc.PaddingBottom = 3f;
                    cDesc.PaddingLeft = 4f; cDesc.PaddingRight = 3f;
                    det.AddCell(cDesc);

                    // Stock
                    decimal stock = l.Table.Columns.Contains("Stock") && l["Stock"] != DBNull.Value
                                    ? Convert.ToDecimal(l["Stock"]) : 0m;
                    addCell(stock.ToString("N" + cantStock, culture), fTbody, true);

                    // Cantidad
                    addCell(cant.ToString("N2", culture), fTbody, true);

                    // Subtotal (precio con IVA × cantidad — misma lógica que antes)
                    addCell("$" + (conIva * cant).ToString("N2", culture), fTbodyBold, true);

                    idx++;
                }

                det.SpacingAfter = 8f;
                doc.Add(det);

                // ═════════════════════════════════════════════════════════════
                // TOTALES (cuadro a la derecha, mitad del ancho)
                // ═════════════════════════════════════════════════════════════
                PdfPTable wrap = new PdfPTable(2);
                wrap.WidthPercentage = 100;
                wrap.SetWidths(new float[] { 1.4f, 1f });

                PdfPCell hueco = new PdfPCell(new Phrase(" "));
                hueco.Border = Rectangle.NO_BORDER;
                wrap.AddCell(hueco);

                PdfPTable tot = new PdfPTable(2);
                tot.WidthPercentage = 100;
                tot.SetWidths(new float[] { 1.4f, 1f });

                Action<string, string, BaseColor, Font, Font> totRow = (lbl, val, bg, fl, fv) =>
                {
                    PdfPCell lc = new PdfPCell(new Phrase(lbl, fl));
                    lc.BackgroundColor = bg;
                    lc.BorderColor = cGrisBorde;
                    lc.Border = Rectangle.NO_BORDER;
                    lc.PaddingTop = 4f; lc.PaddingBottom = 4f; lc.PaddingLeft = 8f;
                    tot.AddCell(lc);

                    PdfPCell vc = new PdfPCell(new Phrase(val, fv));
                    vc.BackgroundColor = bg;
                    vc.BorderColor = cGrisBorde;
                    vc.Border = Rectangle.NO_BORDER;
                    vc.HorizontalAlignment = Element.ALIGN_RIGHT;
                    vc.PaddingTop = 4f; vc.PaddingBottom = 4f; vc.PaddingRight = 8f;
                    tot.AddCell(vc);
                };

                totRow("Precio s/IVA",     "$" + totPrecioSIva.ToString("N2", culture), cBlanco,    fTotLab, fTotVal);
                totRow("Desc / Rec",       "$" + totDescRec.ToString("N2", culture),    cGrisFondo, fTotLab, fTotVal);
                totRow("Subtotal s/IVA",   "$" + totSubtSIva.ToString("N2", culture),   cBlanco,    fTotLab, fTotVal);
                totRow("IVA " + ivaPct.ToString("N0", culture) + "%",
                                           "$" + totIva.ToString("N2", culture),        cGrisFondo, fTotLab, fTotVal);
                totRow("TOTAL",            "$" + totTotal.ToString("N2", culture),      cAzul,      fTotTotLab, fTotTotVal);

                PdfPCell totCell = new PdfPCell(tot);
                totCell.Border = Rectangle.BOX;
                totCell.BorderColor = cGrisBorde;
                totCell.Padding = 0f;
                wrap.AddCell(totCell);
                doc.Add(wrap);

                // ═════════════════════════════════════════════════════════════
                // FIRMA (a la derecha)
                // ═════════════════════════════════════════════════════════════
                doc.Add(new Paragraph(" ") { SpacingAfter = 20f });

                PdfPTable firma = new PdfPTable(2);
                firma.WidthPercentage = 100;
                firma.SetWidths(new float[] { 2.5f, 1f });

                PdfPCell huecoF = new PdfPCell(new Phrase(" "));
                huecoF.Border = Rectangle.NO_BORDER;
                firma.AddCell(huecoF);

                PdfPCell firmaCell = new PdfPCell(new Phrase("Firma y aclaracion", fFirma));
                firmaCell.HorizontalAlignment = Element.ALIGN_CENTER;
                firmaCell.Border = Rectangle.TOP_BORDER;
                firmaCell.BorderColor = cTextoSec;
                firmaCell.PaddingTop = 6f;
                firmaCell.PaddingBottom = 4f;
                firma.AddCell(firmaCell);

                doc.Add(firma);

                doc.Close();
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        // ════════════════════════════════════════════════════════════════════════
        // ESTADÍSTICAS DE VENTAS — PDF reemplaza ReportVentasEstadisticas.rdlc
        // Data: sp_VentasBuscarEntreFechayFiltro(filtro, filtroDev, tipo)
        // ════════════════════════════════════════════════════════════════════════
        public void GenerarVentasEstadisticasPDF(string filtro, string filtroDev, string subtitulo, int tipo)
        {
            var instEstad = new ClassEstadisticas();
            DataTable dt = instEstad.traeEstadisticasVentas(filtro, filtroDev, tipo);

            if (dt.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                    "No hay datos para el período seleccionado.", "Sin datos",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string path = Path.Combine(downloads,
                $"VentasEstadisticas_{DateTime.Now:ddMMyyyy_HHmmss}.pdf");

            var culture = new CultureInfo("es-AR");

            // ─── Paleta ────────────────────────────────────────────────────────
            BaseColor cAzul      = new BaseColor(26,  58,  92);
            BaseColor cGrisFondo = new BaseColor(248, 249, 252);
            BaseColor cGrisBorde = new BaseColor(227, 230, 240);
            BaseColor cGrisTotal = new BaseColor(220, 220, 230);
            BaseColor cBlanco    = BaseColor.WHITE;
            BaseColor cTextoOsc  = new BaseColor(44,  62,  80);
            BaseColor cTextoNeg  = new BaseColor(34,  34,  34);
            BaseColor cRojo      = new BaseColor(192, 57,  43);

            // ─── Fuentes ───────────────────────────────────────────────────────
            Font fTitulo    = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, cBlanco);
            Font fSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA,       9, cBlanco);
            Font fThead     = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7f, cBlanco);
            Font fTbody     = FontFactory.GetFont(FontFactory.HELVETICA,      7f, cTextoNeg);
            Font fTbodyNeg  = FontFactory.GetFont(FontFactory.HELVETICA,      7f, cRojo);
            Font fTbodyBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7f, cTextoNeg);
            Font fTotLab    = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  8, cTextoOsc);
            Font fTotVal    = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  8, cTextoNeg);

            // ─── Logo y datos empresa (para el header repetido) ────────────────
            byte[] logoBytes = null;
            try { logoBytes = ClassParametros.traerImagenLogotipo(); } catch { }

            string empNombre = Clases.ClassValidacion.traerEmpresa();
            string empRazon  = Clases.ClassValidacion.traerRazonSocial();
            string empDir    = Clases.ClassValidacion.traerEmpresaDireccion();
            string empLocal  = Clases.ClassValidacion.traerEmpresaCiudad();
            string empCuit   = Clases.ClassValidacion.traerEmpresaCuit();
            string empTel    = Clases.ClassValidacion.traerEmpresaTelefono();

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                // Margen superior amplio para que el page-event dibuje el header sin pisar contenido
                const float TOP_MARGIN = 92f;
                Document doc = new Document(PageSize.A4.Rotate(), 36, 36, TOP_MARGIN, 36);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                // Header repetido en todas las páginas via page event
                writer.PageEvent = new EmpresaPageHeader(
                    logoBytes, empNombre, empRazon, empDir, empLocal, empCuit, empTel);
                doc.Open();

                // ══ BANNER TÍTULO (sólo página 1) ════════════════════════════
                PdfPTable banner = new PdfPTable(2);
                banner.WidthPercentage = 100;
                banner.SetWidths(new float[] { 3, 2 });

                PdfPCell bcIzq = new PdfPCell(new Phrase("RESUMEN DE VENTAS", fTitulo));
                bcIzq.BackgroundColor = cAzul;
                bcIzq.Border = Rectangle.NO_BORDER;
                bcIzq.HorizontalAlignment = Element.ALIGN_LEFT;
                bcIzq.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcIzq.PaddingTop = 6f; bcIzq.PaddingBottom = 6f; bcIzq.PaddingLeft = 10f;

                PdfPCell bcDer = new PdfPCell(new Phrase(subtitulo, fSubtitulo));
                bcDer.BackgroundColor = cAzul;
                bcDer.Border = Rectangle.NO_BORDER;
                bcDer.HorizontalAlignment = Element.ALIGN_RIGHT;
                bcDer.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcDer.PaddingTop = 6f; bcDer.PaddingBottom = 6f; bcDer.PaddingRight = 10f;

                banner.AddCell(bcIzq);
                banner.AddCell(bcDer);
                banner.SpacingAfter = 10f;
                doc.Add(banner);

                // ══ TABLA DETALLE ════════════════════════════════════════════
                PdfPTable det = new PdfPTable(10);
                det.WidthPercentage = 100;
                det.SetWidths(new float[] { 1f, 1.5f, 3.5f, 2.5f, 1.8f, 1.2f, 1.8f, 1.8f, 1f, 1.8f });
                det.HeaderRows = 1; // repetir fila de encabezado en cada página

                string[] heads = { "Nro", "Fecha", "Cliente", "Vendedor",
                                   "Total C/IVA", "IVA", "Total S/IVA", "Costo", "% Com", "Comisión" };
                foreach (var h in heads)
                {
                    PdfPCell c = new PdfPCell(new Phrase(h, fThead));
                    c.BackgroundColor = cAzul;
                    c.Border = Rectangle.NO_BORDER;
                    c.HorizontalAlignment = h == "Nro" || h == "Fecha" || h == "Cliente" || h == "Vendedor"
                        ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
                    c.VerticalAlignment = Element.ALIGN_MIDDLE;
                    c.NoWrap = true;
                    c.PaddingTop = 4f; c.PaddingBottom = 4f;
                    c.PaddingLeft = 3f; c.PaddingRight = 3f;
                    det.AddCell(c);
                }

                decimal sumTotalCIVA = 0, sumIVA = 0, sumTotalSIVA = 0, sumCosto = 0, sumComision = 0;
                int idx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal totalCIVA  = row["TotalCIVA"]  != DBNull.Value ? Convert.ToDecimal(row["TotalCIVA"])  : 0m;
                    decimal iva        = row["IVA"]        != DBNull.Value ? Convert.ToDecimal(row["IVA"])        : 0m;
                    decimal totalSIVA  = row["totalSIVA"]  != DBNull.Value ? Convert.ToDecimal(row["totalSIVA"])  : 0m;
                    decimal costo      = row["Costo"]      != DBNull.Value ? Convert.ToDecimal(row["Costo"])      : 0m;
                    decimal pCom       = row["P_Com"]      != DBNull.Value ? Convert.ToDecimal(row["P_Com"])      : 0m;
                    decimal comision   = row["Comision"]   != DBNull.Value ? Convert.ToDecimal(row["Comision"])   : 0m;

                    sumTotalCIVA += totalCIVA;
                    sumIVA       += iva;
                    sumTotalSIVA += totalSIVA;
                    sumCosto     += costo;
                    sumComision  += comision;

                    BaseColor bg = (idx % 2 == 0) ? cBlanco : cGrisFondo;

                    string fecha = row["Fecha"] != DBNull.Value
                        ? Convert.ToDateTime(row["Fecha"]).ToString("dd/MM/yyyy") : "";

                    Action<string, bool, bool> addTxt = (txt, right, bold) =>
                    {
                        Font f = bold ? fTbodyBold : fTbody;
                        PdfPCell c = new PdfPCell(new Phrase(txt, f));
                        c.BackgroundColor = bg;
                        c.BorderColor = cGrisBorde;
                        c.Border = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = right ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft = 3f; c.PaddingRight = 3f;
                        det.AddCell(c);
                    };

                    Action<decimal, bool> addNum = (val, isNegSign) =>
                    {
                        Font f = isNegSign && val < 0 ? fTbodyNeg : fTbody;
                        string txt = val.ToString("N2", culture);
                        PdfPCell c = new PdfPCell(new Phrase(txt, f));
                        c.BackgroundColor = bg;
                        c.BorderColor = cGrisBorde;
                        c.Border = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = Element.ALIGN_RIGHT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft = 3f; c.PaddingRight = 3f;
                        det.AddCell(c);
                    };

                    addTxt(row["Nro"].ToString(),      false, false);
                    addTxt(fecha,                      false, false);
                    addTxt(row["Cliente"].ToString(),  false, false);
                    addTxt(row["Vendedor"].ToString(), false, false);
                    addNum(totalCIVA,  true);
                    addNum(iva,        false);
                    addNum(totalSIVA,  true);
                    addNum(costo,      false);
                    addTxt(pCom.ToString("N2", culture) + "%", true, false);
                    addNum(comision,   false);

                    idx++;
                }

                det.SpacingAfter = 0f;
                doc.Add(det);

                // ══ FILA DE TOTALES ══════════════════════════════════════════
                PdfPTable totT = new PdfPTable(10);
                totT.WidthPercentage = 100;
                totT.SetWidths(new float[] { 1f, 1.5f, 3.5f, 2.5f, 1.8f, 1.2f, 1.8f, 1.8f, 1f, 1.8f });
                totT.KeepTogether = true;  // no partir el total entre páginas
                totT.SpacingBefore = 0f;

                Action<string, bool> addTotCell = (txt, isNum) =>
                {
                    PdfPCell c = new PdfPCell(new Phrase(txt, isNum ? fTotVal : fTotLab));
                    c.BackgroundColor = cGrisTotal;
                    c.Border = Rectangle.TOP_BORDER;
                    c.BorderColor = cAzul;
                    c.HorizontalAlignment = isNum ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                    c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                    c.PaddingTop = 5f; c.PaddingBottom = 5f;
                    c.PaddingLeft = 3f; c.PaddingRight = 3f;
                    totT.AddCell(c);
                };

                // "TOTALES" abarca las 3 primeras columnas para que no se corte la etiqueta
                PdfPCell cTotLbl = new PdfPCell(new Phrase("TOTALES", fTotLab));
                cTotLbl.Colspan = 3;
                cTotLbl.BackgroundColor = cGrisTotal;
                cTotLbl.Border = Rectangle.TOP_BORDER;
                cTotLbl.BorderColor = cAzul;
                cTotLbl.HorizontalAlignment = Element.ALIGN_LEFT;
                cTotLbl.VerticalAlignment = Element.ALIGN_MIDDLE;
                cTotLbl.NoWrap = true;
                cTotLbl.PaddingTop = 5f; cTotLbl.PaddingBottom = 5f;
                cTotLbl.PaddingLeft = 3f; cTotLbl.PaddingRight = 3f;
                totT.AddCell(cTotLbl);
                addTotCell(idx.ToString() + " registros", false);
                addTotCell(sumTotalCIVA.ToString("N2", culture), true);
                addTotCell(sumIVA.ToString("N2", culture),       true);
                addTotCell(sumTotalSIVA.ToString("N2", culture), true);
                addTotCell(sumCosto.ToString("N2", culture),     true);
                addTotCell("",                                    false);
                addTotCell(sumComision.ToString("N2", culture),  true);

                doc.Add(totT);
                doc.Close();
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        // ════════════════════════════════════════════════════════════════════════
        // STOCK DE PRODUCTOS — PDF reemplaza ReportProductosStock.rdlc
        // Data: sp_ProductosTraerStock(unFiltro)
        // ════════════════════════════════════════════════════════════════════════
        public void GenerarStockPDF(string filtro, int cantDec, int cantStock)
        {
            var instProd = new ClassProductos();
            DataTable dt = instProd.traeProductosStock(filtro);

            if (dt.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                    "No hay productos para los filtros seleccionados.", "Sin datos",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string path = Path.Combine(downloads,
                $"Stock_{DateTime.Now:ddMMyyyy_HHmmss}.pdf");

            var culture = new CultureInfo("es-AR");

            // ─── Paleta ────────────────────────────────────────────────────────
            BaseColor cAzul      = new BaseColor(26,  58,  92);
            BaseColor cGrisFondo = new BaseColor(248, 249, 252);
            BaseColor cGrisBorde = new BaseColor(227, 230, 240);
            BaseColor cGrisTotal = new BaseColor(220, 220, 230);
            BaseColor cBlanco    = BaseColor.WHITE;
            BaseColor cTextoOsc  = new BaseColor(44,  62,  80);
            BaseColor cTextoNeg  = new BaseColor(34,  34,  34);
            BaseColor cRojo      = new BaseColor(192, 57,  43);

            // ─── Fuentes ───────────────────────────────────────────────────────
            Font fTitulo    = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, cBlanco);
            Font fSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA,       9, cBlanco);
            Font fThead     = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7.5f, cBlanco);
            Font fTbody     = FontFactory.GetFont(FontFactory.HELVETICA,      7.5f, cTextoNeg);
            Font fTbodyNeg  = FontFactory.GetFont(FontFactory.HELVETICA,      7.5f, cRojo);
            Font fTotLab    = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  8.5f, cTextoOsc);
            Font fTotVal    = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  8.5f, cTextoNeg);

            // ─── Logo y datos empresa (para el header repetido) ────────────────
            byte[] logoBytes = null;
            try { logoBytes = ClassParametros.traerImagenLogotipo(); } catch { }

            string empNombre = Clases.ClassValidacion.traerEmpresa();
            string empRazon  = Clases.ClassValidacion.traerRazonSocial();
            string empDir    = Clases.ClassValidacion.traerEmpresaDireccion();
            string empLocal  = Clases.ClassValidacion.traerEmpresaCiudad();
            string empCuit   = Clases.ClassValidacion.traerEmpresaCuit();
            string empTel    = Clases.ClassValidacion.traerEmpresaTelefono();

            string fmtDec   = "N" + cantDec.ToString();
            string fmtStock = "N" + cantStock.ToString();

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                const float TOP_MARGIN = 92f;
                Document doc = new Document(PageSize.A4.Rotate(), 36, 36, TOP_MARGIN, 36);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                writer.PageEvent = new EmpresaPageHeader(
                    logoBytes, empNombre, empRazon, empDir, empLocal, empCuit, empTel);
                doc.Open();

                // ══ BANNER TÍTULO (sólo página 1) ════════════════════════════
                PdfPTable banner = new PdfPTable(2);
                banner.WidthPercentage = 100;
                banner.SetWidths(new float[] { 3, 2 });

                PdfPCell bcIzq = new PdfPCell(new Phrase("STOCK DE PRODUCTOS", fTitulo));
                bcIzq.BackgroundColor = cAzul;
                bcIzq.Border = Rectangle.NO_BORDER;
                bcIzq.HorizontalAlignment = Element.ALIGN_LEFT;
                bcIzq.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcIzq.PaddingTop = 6f; bcIzq.PaddingBottom = 6f; bcIzq.PaddingLeft = 10f;

                PdfPCell bcDer = new PdfPCell(new Phrase("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), fSubtitulo));
                bcDer.BackgroundColor = cAzul;
                bcDer.Border = Rectangle.NO_BORDER;
                bcDer.HorizontalAlignment = Element.ALIGN_RIGHT;
                bcDer.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcDer.PaddingTop = 6f; bcDer.PaddingBottom = 6f; bcDer.PaddingRight = 10f;

                banner.AddCell(bcIzq);
                banner.AddCell(bcDer);
                banner.SpacingAfter = 10f;
                doc.Add(banner);

                // ══ TABLA DETALLE ════════════════════════════════════════════
                // Columnas: Cod_Prov | Descripcion | Stock | C_Min | Costo | P_Prov | P_Lista
                PdfPTable det = new PdfPTable(7);
                det.WidthPercentage = 100;
                det.SetWidths(new float[] { 1.6f, 5.5f, 1.4f, 1.3f, 1.6f, 1.6f, 1.6f });
                det.HeaderRows = 1; // repetir encabezado en cada página

                string[] heads = { "Cód. Prov.", "Descripción", "Stock", "C. Mín.", "Costo", "P. Prov.", "P. Lista" };
                for (int i = 0; i < heads.Length; i++)
                {
                    PdfPCell c = new PdfPCell(new Phrase(heads[i], fThead));
                    c.BackgroundColor = cAzul;
                    c.Border = Rectangle.NO_BORDER;
                    c.HorizontalAlignment = i <= 1 ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
                    c.VerticalAlignment = Element.ALIGN_MIDDLE;
                    c.NoWrap = true;
                    c.PaddingTop = 4f; c.PaddingBottom = 4f;
                    c.PaddingLeft = 4f; c.PaddingRight = 4f;
                    det.AddCell(c);
                }

                decimal sumStock = 0, sumCosto = 0, sumPProv = 0, sumPLista = 0;
                int idx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal stock   = row["Stock"]   != DBNull.Value ? Convert.ToDecimal(row["Stock"])   : 0m;
                    decimal cMin    = row["C_Min"]   != DBNull.Value ? Convert.ToDecimal(row["C_Min"])   : 0m;
                    decimal costo   = row["Costo"]   != DBNull.Value ? Convert.ToDecimal(row["Costo"])   : 0m;
                    decimal pProv   = row["P_Prov"]  != DBNull.Value ? Convert.ToDecimal(row["P_Prov"])  : 0m;
                    decimal pLista  = row["P_Lista"] != DBNull.Value ? Convert.ToDecimal(row["P_Lista"]) : 0m;

                    sumStock  += stock;
                    sumCosto  += costo  * stock;
                    sumPProv  += pProv  * stock;
                    sumPLista += pLista * stock;

                    BaseColor bg = (idx % 2 == 0) ? cBlanco : cGrisFondo;

                    Action<string, bool> addTxt = (txt, right) =>
                    {
                        PdfPCell c = new PdfPCell(new Phrase(txt, fTbody));
                        c.BackgroundColor = bg;
                        c.BorderColor = cGrisBorde;
                        c.Border = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = right ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft = 4f; c.PaddingRight = 4f;
                        det.AddCell(c);
                    };

                    Action<decimal, string, bool> addNum = (val, fmt, alertaBajoMin) =>
                    {
                        Font f = alertaBajoMin ? fTbodyNeg : fTbody;
                        PdfPCell c = new PdfPCell(new Phrase(val.ToString(fmt, culture), f));
                        c.BackgroundColor = bg;
                        c.BorderColor = cGrisBorde;
                        c.Border = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = Element.ALIGN_RIGHT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft = 4f; c.PaddingRight = 4f;
                        det.AddCell(c);
                    };

                    addTxt(row["Cod_Prov"].ToString(),    false);
                    addTxt(row["Descripcion"].ToString(), false);
                    addNum(stock,  fmtStock, stock < cMin);  // resaltar stock bajo mínimo
                    addNum(cMin,   fmtStock, false);
                    addNum(costo,  fmtDec,   false);
                    addNum(pProv,  fmtDec,   false);
                    addNum(pLista, fmtDec,   false);

                    idx++;
                }

                det.SpacingAfter = 0f;
                doc.Add(det);

                // ══ FILA DE TOTALES ══════════════════════════════════════════
                PdfPTable totT = new PdfPTable(7);
                totT.WidthPercentage = 100;
                totT.SetWidths(new float[] { 1.6f, 5.5f, 1.4f, 1.3f, 1.6f, 1.6f, 1.6f });
                totT.KeepTogether = true;
                totT.SpacingBefore = 0f;

                Action<string, bool, int> addTotCell = (txt, isNum, colspan) =>
                {
                    PdfPCell c = new PdfPCell(new Phrase(txt, isNum ? fTotVal : fTotLab));
                    if (colspan > 1) c.Colspan = colspan;
                    c.BackgroundColor = cGrisTotal;
                    c.Border = Rectangle.TOP_BORDER;
                    c.BorderColor = cAzul;
                    c.HorizontalAlignment = isNum ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                    c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                    c.NoWrap = true;
                    c.PaddingTop = 5f; c.PaddingBottom = 5f;
                    c.PaddingLeft = 4f; c.PaddingRight = 4f;
                    totT.AddCell(c);
                };

                addTotCell("TOTALES (" + idx.ToString() + " productos)", false, 2);
                addTotCell(sumStock.ToString(fmtStock, culture), true, 1);
                addTotCell("", false, 1);
                addTotCell(sumCosto.ToString(fmtDec, culture),  true, 1);
                addTotCell(sumPProv.ToString(fmtDec, culture),  true, 1);
                addTotCell(sumPLista.ToString(fmtDec, culture), true, 1);

                doc.Add(totT);
                doc.Close();
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        // ════════════════════════════════════════════════════════════════════════
        // LISTA DE PRECIOS — PDF reemplaza ReportListaDePreciosPorRubro.rdlc
        // Data: sp_ProductosListaPrecios(unFiltro)
        // ════════════════════════════════════════════════════════════════════════
        public void GenerarListaPreciosPDF(string filtro, int cantDec)
        {
            var instProd = new ClassProductos();
            DataTable dt = instProd.traerListaPrecios(filtro);

            if (dt.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                    "No hay productos para los filtros seleccionados.", "Sin datos",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string path = Path.Combine(downloads,
                $"ListaPrecios_{DateTime.Now:ddMMyyyy_HHmmss}.pdf");

            var culture = new CultureInfo("es-AR");

            // ─── Paleta ────────────────────────────────────────────────────────
            BaseColor cAzul      = new BaseColor(26,  58,  92);
            BaseColor cGrisFondo = new BaseColor(248, 249, 252);
            BaseColor cGrisBorde = new BaseColor(227, 230, 240);
            BaseColor cBlanco    = BaseColor.WHITE;
            BaseColor cTextoNeg  = new BaseColor(34,  34,  34);

            // ─── Fuentes ───────────────────────────────────────────────────────
            Font fTitulo    = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, cBlanco);
            Font fSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA,       9, cBlanco);
            Font fThead     = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8f, cBlanco);
            Font fTbody     = FontFactory.GetFont(FontFactory.HELVETICA,      8f, cTextoNeg);
            Font fTbodyBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8f, cTextoNeg);

            // ─── Logo y datos empresa (para el header repetido) ────────────────
            byte[] logoBytes = null;
            try { logoBytes = ClassParametros.traerImagenLogotipo(); } catch { }

            string empNombre = Clases.ClassValidacion.traerEmpresa();
            string empRazon  = Clases.ClassValidacion.traerRazonSocial();
            string empDir    = Clases.ClassValidacion.traerEmpresaDireccion();
            string empLocal  = Clases.ClassValidacion.traerEmpresaCiudad();
            string empCuit   = Clases.ClassValidacion.traerEmpresaCuit();
            string empTel    = Clases.ClassValidacion.traerEmpresaTelefono();

            string fmtDec = "N" + cantDec.ToString();

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                const float TOP_MARGIN = 92f;
                Document doc = new Document(PageSize.A4.Rotate(), 36, 36, TOP_MARGIN, 36);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                writer.PageEvent = new EmpresaPageHeader(
                    logoBytes, empNombre, empRazon, empDir, empLocal, empCuit, empTel);
                doc.Open();

                // ══ BANNER TÍTULO (sólo página 1) ════════════════════════════
                PdfPTable banner = new PdfPTable(2);
                banner.WidthPercentage = 100;
                banner.SetWidths(new float[] { 3, 2 });

                PdfPCell bcIzq = new PdfPCell(new Phrase("LISTA DE PRECIOS", fTitulo));
                bcIzq.BackgroundColor = cAzul;
                bcIzq.Border = Rectangle.NO_BORDER;
                bcIzq.HorizontalAlignment = Element.ALIGN_LEFT;
                bcIzq.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcIzq.PaddingTop = 6f; bcIzq.PaddingBottom = 6f; bcIzq.PaddingLeft = 10f;

                PdfPCell bcDer = new PdfPCell(new Phrase("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"), fSubtitulo));
                bcDer.BackgroundColor = cAzul;
                bcDer.Border = Rectangle.NO_BORDER;
                bcDer.HorizontalAlignment = Element.ALIGN_RIGHT;
                bcDer.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcDer.PaddingTop = 6f; bcDer.PaddingBottom = 6f; bcDer.PaddingRight = 10f;

                banner.AddCell(bcIzq);
                banner.AddCell(bcDer);
                banner.SpacingAfter = 10f;
                doc.Add(banner);

                // ══ TABLA DETALLE ════════════════════════════════════════════
                // Columnas: Cod. Prov. | Cod. Barras | Descripción | Precio S/IVA | Precio C/IVA
                PdfPTable det = new PdfPTable(5);
                det.WidthPercentage = 100;
                det.SetWidths(new float[] { 1.6f, 2.2f, 7f, 1.8f, 1.8f });
                det.HeaderRows = 1; // repetir encabezado en cada página

                string[] heads = { "Cód. Prov.", "Cód. Barras", "Descripción", "Precio S/IVA", "Precio C/IVA" };
                for (int i = 0; i < heads.Length; i++)
                {
                    PdfPCell c = new PdfPCell(new Phrase(heads[i], fThead));
                    c.BackgroundColor = cAzul;
                    c.Border = Rectangle.NO_BORDER;
                    c.HorizontalAlignment = i <= 2 ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
                    c.VerticalAlignment = Element.ALIGN_MIDDLE;
                    c.NoWrap = true;
                    c.PaddingTop = 5f; c.PaddingBottom = 5f;
                    c.PaddingLeft = 5f; c.PaddingRight = 5f;
                    det.AddCell(c);
                }

                int idx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal precio    = row["precio"]    != DBNull.Value ? Convert.ToDecimal(row["precio"])    : 0m;
                    decimal precioIva = row["PrecioIVA"] != DBNull.Value ? Convert.ToDecimal(row["PrecioIVA"]) : 0m;

                    BaseColor bg = (idx % 2 == 0) ? cBlanco : cGrisFondo;

                    Action<string, bool, bool> addTxt = (txt, right, bold) =>
                    {
                        Font f = bold ? fTbodyBold : fTbody;
                        PdfPCell c = new PdfPCell(new Phrase(txt, f));
                        c.BackgroundColor = bg;
                        c.BorderColor = cGrisBorde;
                        c.Border = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = right ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop = 4f; c.PaddingBottom = 4f;
                        c.PaddingLeft = 5f; c.PaddingRight = 5f;
                        det.AddCell(c);
                    };

                    addTxt(row["codProveedor"].ToString(), false, false);
                    addTxt(row["codBarras"].ToString(),    false, false);
                    addTxt(row["descripcion"].ToString(),  false, false);
                    addTxt("$ " + precio.ToString(fmtDec, culture),     true, false);
                    addTxt("$ " + precioIva.ToString(fmtDec, culture),  true, true);

                    idx++;
                }

                doc.Add(det);
                doc.Close();
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        // ════════════════════════════════════════════════════════════════════════
        // PRODUCTOS A PEDIR — PDF reemplaza ReportProductoPedir.rdlc
        // Data: sp_ProveedoresListarProductosAPedir(unProveedor, desde, hasta)
        // ════════════════════════════════════════════════════════════════════════
        public void GenerarProductosAPedirPDF(int proveedor, DateTime desde, DateTime hasta,
                                              string subtitulo, int cantDec, int cantStock)
        {
            var instProv = new ClassProveedores();
            DataTable dt = instProv.traerListaProdAPedir(proveedor, desde, hasta);

            if (dt.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                    "No hay productos para los filtros seleccionados.", "Sin datos",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string path = Path.Combine(downloads,
                $"ProductosAPedir_{DateTime.Now:ddMMyyyy_HHmmss}.pdf");

            var culture = new CultureInfo("es-AR");

            // ─── Paleta ────────────────────────────────────────────────────────
            BaseColor cAzul      = new BaseColor(26,  58,  92);
            BaseColor cGrisFondo = new BaseColor(248, 249, 252);
            BaseColor cGrisBorde = new BaseColor(227, 230, 240);
            BaseColor cBlanco    = BaseColor.WHITE;
            BaseColor cTextoNeg  = new BaseColor(34,  34,  34);
            BaseColor cRojo      = new BaseColor(192, 57,  43);

            // ─── Fuentes ───────────────────────────────────────────────────────
            Font fTitulo    = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, cBlanco);
            Font fSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA,       9, cBlanco);
            Font fThead     = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7.5f, cBlanco);
            Font fTbody     = FontFactory.GetFont(FontFactory.HELVETICA,      7.5f, cTextoNeg);
            Font fTbodyNeg  = FontFactory.GetFont(FontFactory.HELVETICA,      7.5f, cRojo);

            // ─── Logo y datos empresa (para el header repetido) ────────────────
            byte[] logoBytes = null;
            try { logoBytes = ClassParametros.traerImagenLogotipo(); } catch { }

            string empNombre = Clases.ClassValidacion.traerEmpresa();
            string empRazon  = Clases.ClassValidacion.traerRazonSocial();
            string empDir    = Clases.ClassValidacion.traerEmpresaDireccion();
            string empLocal  = Clases.ClassValidacion.traerEmpresaCiudad();
            string empCuit   = Clases.ClassValidacion.traerEmpresaCuit();
            string empTel    = Clases.ClassValidacion.traerEmpresaTelefono();

            string fmtDec   = "N" + cantDec.ToString();
            string fmtStock = "N" + cantStock.ToString();

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                const float TOP_MARGIN = 92f;
                Document doc = new Document(PageSize.A4.Rotate(), 36, 36, TOP_MARGIN, 36);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                writer.PageEvent = new EmpresaPageHeader(
                    logoBytes, empNombre, empRazon, empDir, empLocal, empCuit, empTel);
                doc.Open();

                // ══ BANNER TÍTULO (sólo página 1) ════════════════════════════
                PdfPTable banner = new PdfPTable(2);
                banner.WidthPercentage = 100;
                banner.SetWidths(new float[] { 2, 3 });

                PdfPCell bcIzq = new PdfPCell(new Phrase("PRODUCTOS A PEDIR", fTitulo));
                bcIzq.BackgroundColor = cAzul;
                bcIzq.Border = Rectangle.NO_BORDER;
                bcIzq.HorizontalAlignment = Element.ALIGN_LEFT;
                bcIzq.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcIzq.PaddingTop = 6f; bcIzq.PaddingBottom = 6f; bcIzq.PaddingLeft = 10f;

                PdfPCell bcDer = new PdfPCell(new Phrase(subtitulo ?? string.Empty, fSubtitulo));
                bcDer.BackgroundColor = cAzul;
                bcDer.Border = Rectangle.NO_BORDER;
                bcDer.HorizontalAlignment = Element.ALIGN_RIGHT;
                bcDer.VerticalAlignment   = Element.ALIGN_MIDDLE;
                bcDer.PaddingTop = 6f; bcDer.PaddingBottom = 6f; bcDer.PaddingRight = 10f;

                banner.AddCell(bcIzq);
                banner.AddCell(bcDer);
                banner.SpacingAfter = 10f;
                doc.Add(banner);

                // ══ TABLA DETALLE ════════════════════════════════════════════
                // Columnas: Cod.Prov | Descripción | Stock | Ingreso | Ventas | P.Prov | Costo | P.Lista
                PdfPTable det = new PdfPTable(8);
                det.WidthPercentage = 100;
                det.SetWidths(new float[] { 1.5f, 5f, 1.2f, 1.2f, 1.2f, 1.5f, 1.5f, 1.5f });
                det.HeaderRows = 1; // repetir encabezado en cada página

                string[] heads = { "Cód. Prov.", "Descripción", "Stock", "Ingreso", "Ventas", "P. Prov.", "Costo", "P. Lista" };
                for (int i = 0; i < heads.Length; i++)
                {
                    PdfPCell c = new PdfPCell(new Phrase(heads[i], fThead));
                    c.BackgroundColor = cAzul;
                    c.Border = Rectangle.NO_BORDER;
                    c.HorizontalAlignment = i <= 1 ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
                    c.VerticalAlignment = Element.ALIGN_MIDDLE;
                    c.NoWrap = true;
                    c.PaddingTop = 4f; c.PaddingBottom = 4f;
                    c.PaddingLeft = 4f; c.PaddingRight = 4f;
                    det.AddCell(c);
                }

                int idx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal stock   = row["Stock"]   != DBNull.Value ? Convert.ToDecimal(row["Stock"])   : 0m;
                    decimal ingreso = row["Ingreso"] != DBNull.Value ? Convert.ToDecimal(row["Ingreso"]) : 0m;
                    decimal ventas  = row["Ventas"]  != DBNull.Value ? Convert.ToDecimal(row["Ventas"])  : 0m;
                    decimal pProv   = row["P_Prov"]  != DBNull.Value ? Convert.ToDecimal(row["P_Prov"])  : 0m;
                    decimal costo   = row["Costo"]   != DBNull.Value ? Convert.ToDecimal(row["Costo"])   : 0m;
                    decimal pLista  = row["P_Lista"] != DBNull.Value ? Convert.ToDecimal(row["P_Lista"]) : 0m;

                    BaseColor bg = (idx % 2 == 0) ? cBlanco : cGrisFondo;

                    Action<string, bool> addTxt = (txt, right) =>
                    {
                        PdfPCell c = new PdfPCell(new Phrase(txt, fTbody));
                        c.BackgroundColor = bg;
                        c.BorderColor = cGrisBorde;
                        c.Border = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = right ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft = 4f; c.PaddingRight = 4f;
                        det.AddCell(c);
                    };

                    Action<decimal, string, bool> addNum = (val, fmt, alertar) =>
                    {
                        Font f = alertar ? fTbodyNeg : fTbody;
                        PdfPCell c = new PdfPCell(new Phrase(val.ToString(fmt, culture), f));
                        c.BackgroundColor = bg;
                        c.BorderColor = cGrisBorde;
                        c.Border = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = Element.ALIGN_RIGHT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft = 4f; c.PaddingRight = 4f;
                        det.AddCell(c);
                    };

                    addTxt(row["Cod_Prov"].ToString(),    false);
                    addTxt(row["Descripcion"].ToString(), false);
                    addNum(stock,   fmtStock, stock <= 0); // resaltar stock 0 o negativo
                    addNum(ingreso, fmtStock, false);
                    addNum(ventas,  fmtStock, false);
                    addNum(pProv,   fmtDec,   false);
                    addNum(costo,   fmtDec,   false);
                    addNum(pLista,  fmtDec,   false);

                    idx++;
                }

                doc.Add(det);
                doc.Close();
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        public void GenerarOrdenCompraPDF(long idOrden, int cantDec, int cantStock)
        {
            var instProv = new ClassProveedores();
            DataTable dt = instProv.traerOrdenCompra(idOrden);

            if (dt.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                    "No se encontraron datos para la orden seleccionada.", "Sin datos",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string path = Path.Combine(downloads,
                $"OrdenCompra_{idOrden}_{DateTime.Now:ddMMyyyy_HHmmss}.pdf");

            var culture = new CultureInfo("es-AR");

            // ─── Paleta ────────────────────────────────────────────────────────
            BaseColor cAzul      = new BaseColor(26,  58,  92);
            BaseColor cAzulClaro = new BaseColor(44,  84, 130);
            BaseColor cGrisFondo = new BaseColor(248, 249, 252);
            BaseColor cGrisBorde = new BaseColor(227, 230, 240);
            BaseColor cGrisTotal = new BaseColor(220, 220, 230);
            BaseColor cBlanco    = BaseColor.WHITE;
            BaseColor cTextoNeg  = new BaseColor(34,  34,  34);
            BaseColor cTextoSec  = new BaseColor(85,  85,  85);

            // ─── Fuentes ───────────────────────────────────────────────────────
            Font fNombreEmp = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13f, cAzul);
            Font fDatosEmp  = FontFactory.GetFont(FontFactory.HELVETICA,       8f, cTextoSec);
            Font fDatosLbl  = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  8f, cTextoSec);
            Font fTitOC     = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14f, cBlanco);
            Font fNroOC     = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11f, cBlanco);
            Font fFecOC     = FontFactory.GetFont(FontFactory.HELVETICA,       8f, cBlanco);
            Font fSeccion   = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  9f, cBlanco);
            Font fThead     = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8.5f, cBlanco);
            Font fTbody     = FontFactory.GetFont(FontFactory.HELVETICA,      8.5f, cTextoNeg);
            Font fProvNom   = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  9f, cTextoNeg);
            Font fProvDir   = FontFactory.GetFont(FontFactory.HELVETICA,      8.5f, cTextoNeg);

            DataRow cab     = dt.Rows[0];
            string nroOrden = cab["id"].ToString();
            string fecha    = Convert.ToDateTime(cab["fecha"]).ToString("dd/MM/yyyy");
            string proveedor = cab["nombreComercial"].ToString();
            string dirProv   = cab["direccion"] != DBNull.Value ? cab["direccion"].ToString() : string.Empty;
            decimal total    = cab["total"]    != DBNull.Value ? Convert.ToDecimal(cab["total"])    : 0m;
            decimal iva      = cab["iva"]      != DBNull.Value ? Convert.ToDecimal(cab["iva"])      : 0m;
            decimal recargo  = cab["recargo"]  != DBNull.Value ? Convert.ToDecimal(cab["recargo"])  : 0m;
            decimal descuento= cab["descuento"]!= DBNull.Value ? Convert.ToDecimal(cab["descuento"]): 0m;

            string fmtDec   = "N" + cantDec.ToString();
            string fmtStock = "N" + cantStock.ToString();

            string empNombre = Clases.ClassValidacion.traerEmpresa();
            string empRazon  = Clases.ClassValidacion.traerRazonSocial();
            string empDir    = Clases.ClassValidacion.traerEmpresaDireccion();
            string empLocal  = Clases.ClassValidacion.traerEmpresaCiudad();
            string empCuit   = Clases.ClassValidacion.traerEmpresaCuit();
            string empTel    = Clases.ClassValidacion.traerEmpresaTelefono();

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4, 36, 36, 36, 36);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // ══ HEADER: logo | empresa | bloque orden ═════════════════════
                PdfPTable hdr = new PdfPTable(3);
                hdr.WidthPercentage = 100;
                hdr.SetWidths(new float[] { 1.5f, 4f, 2.5f });
                hdr.SpacingAfter = 14f;

                // Logo
                PdfPCell logoCell;
                if (cab["imagen"] != DBNull.Value)
                {
                    try
                    {
                        Image logo = Image.GetInstance((byte[])cab["imagen"]);
                        logo.ScaleToFit(90, 55);
                        logoCell = new PdfPCell(logo);
                        logoCell.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    catch { logoCell = new PdfPCell(new Phrase("")); }
                }
                else { logoCell = new PdfPCell(new Phrase("")); }
                logoCell.Border       = Rectangle.NO_BORDER;
                logoCell.PaddingRight = 10f;
                hdr.AddCell(logoCell);

                // Datos empresa
                Paragraph pEmp = new Paragraph();
                pEmp.SetLeading(0f, 1.35f);
                pEmp.Add(new Chunk(empNombre, fNombreEmp));
                pEmp.Add(Chunk.NEWLINE);
                if (!string.IsNullOrEmpty(empRazon))
                    { pEmp.Add(new Chunk(empRazon, fDatosEmp)); pEmp.Add(Chunk.NEWLINE); }
                if (!string.IsNullOrEmpty(empDir))
                    { pEmp.Add(new Chunk(empDir, fDatosEmp)); pEmp.Add(Chunk.NEWLINE); }
                if (!string.IsNullOrEmpty(empLocal))
                    { pEmp.Add(new Chunk(empLocal, fDatosEmp)); pEmp.Add(Chunk.NEWLINE); }
                if (!string.IsNullOrEmpty(empCuit))
                    { pEmp.Add(new Chunk("CUIT: ", fDatosLbl)); pEmp.Add(new Chunk(empCuit, fDatosEmp)); pEmp.Add(Chunk.NEWLINE); }
                if (!string.IsNullOrEmpty(empTel))
                    { pEmp.Add(new Chunk("Tel: ", fDatosLbl));  pEmp.Add(new Chunk(empTel, fDatosEmp)); }

                PdfPCell empCell = new PdfPCell(pEmp);
                empCell.Border            = Rectangle.NO_BORDER;
                empCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                hdr.AddCell(empCell);

                // Bloque azul: título + nro + fecha
                PdfPTable ocBox = new PdfPTable(1);
                ocBox.WidthPercentage = 100;

                PdfPCell ocTitCell = new PdfPCell(new Phrase("ORDEN DE COMPRA", fTitOC));
                ocTitCell.BackgroundColor     = cAzul;
                ocTitCell.Border              = Rectangle.NO_BORDER;
                ocTitCell.HorizontalAlignment = Element.ALIGN_CENTER;
                ocTitCell.PaddingTop          = 8f; ocTitCell.PaddingBottom = 4f;
                ocBox.AddCell(ocTitCell);

                PdfPCell ocNroCell = new PdfPCell(new Phrase("N°  " + nroOrden, fNroOC));
                ocNroCell.BackgroundColor     = cAzul;
                ocNroCell.Border              = Rectangle.NO_BORDER;
                ocNroCell.HorizontalAlignment = Element.ALIGN_CENTER;
                ocNroCell.PaddingBottom       = 6f;
                ocBox.AddCell(ocNroCell);

                PdfPCell ocFecCell = new PdfPCell(new Phrase("Fecha:  " + fecha, fFecOC));
                ocFecCell.BackgroundColor     = cAzulClaro;
                ocFecCell.Border              = Rectangle.NO_BORDER;
                ocFecCell.HorizontalAlignment = Element.ALIGN_CENTER;
                ocFecCell.PaddingTop          = 4f; ocFecCell.PaddingBottom = 4f;
                ocBox.AddCell(ocFecCell);

                PdfPCell ocBoxCell = new PdfPCell(ocBox);
                ocBoxCell.Border      = Rectangle.NO_BORDER;
                ocBoxCell.PaddingLeft = 10f;
                hdr.AddCell(ocBoxCell);

                doc.Add(hdr);

                // ══ SECCIÓN PROVEEDOR ════════════════════════════════════════
                PdfPTable secProv = new PdfPTable(1);
                secProv.WidthPercentage = 100;
                secProv.SpacingAfter = 10f;

                PdfPCell secProvHdr = new PdfPCell(new Phrase("PROVEEDOR", fSeccion));
                secProvHdr.BackgroundColor = cAzul;
                secProvHdr.Border          = Rectangle.NO_BORDER;
                secProvHdr.PaddingTop      = 4f; secProvHdr.PaddingBottom = 4f;
                secProvHdr.PaddingLeft     = 8f;
                secProv.AddCell(secProvHdr);

                Paragraph pProv = new Paragraph();
                pProv.SetLeading(0f, 1.35f);
                pProv.Add(new Chunk(proveedor, fProvNom));
                if (!string.IsNullOrEmpty(dirProv))
                    { pProv.Add(Chunk.NEWLINE); pProv.Add(new Chunk(dirProv, fProvDir)); }

                PdfPCell provCell = new PdfPCell(pProv);
                provCell.BackgroundColor = cGrisFondo;
                provCell.BorderColor     = cGrisBorde;
                provCell.Border          = Rectangle.BOX;
                provCell.PaddingTop      = 6f; provCell.PaddingBottom = 6f;
                provCell.PaddingLeft     = 8f;
                secProv.AddCell(provCell);

                doc.Add(secProv);

                // ══ TABLA DETALLE ════════════════════════════════════════════
                // Cód.Prov | Descripción | Cantidad | P.Proveedor | Subtotal
                PdfPTable det = new PdfPTable(5);
                det.WidthPercentage = 100;
                det.SetWidths(new float[] { 1.5f, 5f, 1.2f, 1.8f, 1.8f });
                det.HeaderRows   = 1;
                det.SpacingAfter = 0f;

                string[] heads = { "Cód. Prov.", "Descripción", "Cantidad", "P. Proveedor", "Subtotal" };
                for (int i = 0; i < heads.Length; i++)
                {
                    PdfPCell c = new PdfPCell(new Phrase(heads[i], fThead));
                    c.BackgroundColor     = cAzul;
                    c.Border              = Rectangle.NO_BORDER;
                    c.HorizontalAlignment = i <= 1 ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT;
                    c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                    c.NoWrap              = true;
                    c.PaddingTop          = 5f; c.PaddingBottom = 5f;
                    c.PaddingLeft         = 4f; c.PaddingRight  = 4f;
                    det.AddCell(c);
                }

                int rowIdx = 0;
                foreach (DataRow row in dt.Rows)
                {
                    decimal cantidad   = row["cantidad"]       != DBNull.Value ? Convert.ToDecimal(row["cantidad"])       : 0m;
                    decimal precioProv = row["precioProveedor"]!= DBNull.Value ? Convert.ToDecimal(row["precioProveedor"]) : 0m;
                    decimal subtotal   = row["subtotal"]       != DBNull.Value ? Convert.ToDecimal(row["subtotal"])       : 0m;

                    BaseColor bg = (rowIdx % 2 == 0) ? cBlanco : cGrisFondo;

                    Action<string, bool> addTxt = (txt, right) =>
                    {
                        PdfPCell c = new PdfPCell(new Phrase(txt, fTbody));
                        c.BackgroundColor     = bg;
                        c.BorderColor         = cGrisBorde;
                        c.Border              = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = right ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop          = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft         = 4f; c.PaddingRight  = 4f;
                        det.AddCell(c);
                    };

                    Action<decimal, string> addNum = (val, fmt) =>
                    {
                        PdfPCell c = new PdfPCell(new Phrase(val.ToString(fmt, culture), fTbody));
                        c.BackgroundColor     = bg;
                        c.BorderColor         = cGrisBorde;
                        c.Border              = Rectangle.BOTTOM_BORDER;
                        c.HorizontalAlignment = Element.ALIGN_RIGHT;
                        c.VerticalAlignment   = Element.ALIGN_MIDDLE;
                        c.PaddingTop          = 3f; c.PaddingBottom = 3f;
                        c.PaddingLeft         = 4f; c.PaddingRight  = 4f;
                        det.AddCell(c);
                    };

                    addTxt(row["codProveedor"].ToString(), false);
                    addTxt(row["descripcion"].ToString(),  false);
                    addNum(cantidad,   fmtStock);
                    addNum(precioProv, fmtDec);
                    addNum(subtotal,   fmtDec);

                    rowIdx++;
                }

                doc.Add(det);

                // ══ TOTALES (alineado a la derecha, 45% del ancho) ═════════
                Font fTotLbl  = FontFactory.GetFont(FontFactory.HELVETICA,      8.5f, cTextoNeg);
                Font fTotBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD,  9f,  cBlanco);

                PdfPTable totT = new PdfPTable(2);
                totT.WidthPercentage      = 45;
                totT.HorizontalAlignment  = Element.ALIGN_RIGHT;
                totT.SetWidths(new float[] { 2.5f, 2f });
                totT.KeepTogether   = true;
                totT.SpacingBefore  = 0f;

                Action<string, decimal, bool> addTot = (lbl, val, bold) =>
                {
                    Font fL = bold ? fTotBold : fTotLbl;
                    Font fV = bold ? fTotBold : fTotLbl;
                    BaseColor bg2 = bold ? cAzul : cGrisTotal;

                    PdfPCell cL = new PdfPCell(new Phrase(lbl, fL));
                    cL.BackgroundColor     = bg2;
                    cL.Border              = Rectangle.NO_BORDER;
                    cL.HorizontalAlignment = Element.ALIGN_LEFT;
                    cL.PaddingTop          = bold ? 5f : 3f;
                    cL.PaddingBottom       = bold ? 5f : 3f;
                    cL.PaddingLeft         = 8f;
                    totT.AddCell(cL);

                    PdfPCell cV = new PdfPCell(new Phrase(val.ToString(fmtDec, culture), fV));
                    cV.BackgroundColor     = bg2;
                    cV.Border              = Rectangle.NO_BORDER;
                    cV.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cV.PaddingTop          = bold ? 5f : 3f;
                    cV.PaddingBottom       = bold ? 5f : 3f;
                    cV.PaddingRight        = 8f;
                    totT.AddCell(cV);
                };

                if (descuento != 0) addTot("Descuento:", descuento, false);
                if (recargo   != 0) addTot("Recargo:",   recargo,   false);
                if (iva       != 0) addTot("IVA:",       iva,       false);
                addTot("TOTAL:", total, true);

                doc.Add(totT);
                doc.Close();
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        // Header empresa repetido en cada página via PdfPageEventHelper.
        // Reutilizado por reportes tipo listado (estadísticas de ventas, stock, lista de precios, productos a pedir, etc.).
        private class EmpresaPageHeader : PdfPageEventHelper
        {
            private readonly byte[] _logoBytes;
            private readonly string _empNombre, _empRazon, _empDir, _empLocal, _empCuit, _empTel;

            public EmpresaPageHeader(byte[] logoBytes,
                string empNombre, string empRazon, string empDir,
                string empLocal, string empCuit, string empTel)
            {
                _logoBytes = logoBytes;
                _empNombre = empNombre; _empRazon  = empRazon;
                _empDir    = empDir;    _empLocal  = empLocal;
                _empCuit   = empCuit;   _empTel    = empTel;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                BaseColor cAzul     = new BaseColor(26, 58, 92);
                BaseColor cTextoOsc = new BaseColor(44, 62, 80);
                BaseColor cTextoSec = new BaseColor(85, 85, 85);

                Font fNombre = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13f, cAzul);
                Font fRazon  = FontFactory.GetFont(FontFactory.HELVETICA,      8.5f, cTextoOsc);
                Font fSub    = FontFactory.GetFont(FontFactory.HELVETICA,      7.5f, cTextoSec);
                Font fSubLbl = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7.5f, cTextoSec);

                float usableWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

                PdfPTable hdr = new PdfPTable(2);
                hdr.TotalWidth   = usableWidth;
                hdr.LockedWidth  = true;
                hdr.SetWidths(new float[] { 1.4f, 5f });

                // Columna logo
                PdfPCell logoCell;
                if (_logoBytes != null)
                {
                    try
                    {
                        Image logo = Image.GetInstance(_logoBytes);
                        logo.ScaleToFit(100, 50);
                        logoCell = new PdfPCell(logo);
                        logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    }
                    catch { logoCell = new PdfPCell(new Phrase("")); }
                }
                else { logoCell = new PdfPCell(new Phrase("")); }
                logoCell.Border       = Rectangle.NO_BORDER;
                logoCell.PaddingTop   = 5f;
                logoCell.PaddingBottom = 5f;
                logoCell.PaddingRight  = 16f;
                hdr.AddCell(logoCell);

                // Columna datos empresa
                const string SEP = "   ·   ";
                Paragraph p = new Paragraph();
                p.SetLeading(0f, 1.32f);
                p.Add(new Chunk(_empNombre, fNombre));
                p.Add(Chunk.NEWLINE);
                if (!string.IsNullOrEmpty(_empRazon))
                {
                    p.Add(new Chunk(_empRazon, fRazon));
                    p.Add(Chunk.NEWLINE);
                }
                bool tieneDir   = !string.IsNullOrEmpty(_empDir);
                bool tieneLocal = !string.IsNullOrEmpty(_empLocal);
                if (tieneDir || tieneLocal)
                {
                    if (tieneDir)             p.Add(new Chunk(_empDir,   fSub));
                    if (tieneDir && tieneLocal) p.Add(new Chunk(SEP,     fSub));
                    if (tieneLocal)           p.Add(new Chunk(_empLocal, fSub));
                    p.Add(Chunk.NEWLINE);
                }
                bool tieneCuit = !string.IsNullOrEmpty(_empCuit);
                bool tieneTel  = !string.IsNullOrEmpty(_empTel);
                if (tieneCuit || tieneTel)
                {
                    if (tieneCuit) { p.Add(new Chunk("CUIT: ", fSubLbl)); p.Add(new Chunk(_empCuit, fSub)); }
                    if (tieneCuit && tieneTel) p.Add(new Chunk(SEP, fSub));
                    if (tieneTel)  { p.Add(new Chunk("Tel: ",  fSubLbl)); p.Add(new Chunk(_empTel,  fSub)); }
                }

                PdfPCell datosCell = new PdfPCell(p);
                datosCell.Border            = Rectangle.NO_BORDER;
                datosCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                datosCell.PaddingLeft       = 8f;
                datosCell.PaddingTop        = 5f;
                datosCell.PaddingBottom     = 5f;
                hdr.AddCell(datosCell);

                // Dibujar la tabla en la zona del margen superior (fuera del flujo del documento)
                float startY = document.PageSize.Height - 8f;
                hdr.WriteSelectedRows(0, -1, document.LeftMargin, startY, writer.DirectContent);

                // Línea azul separadora justo debajo del header
                float lineY = startY - hdr.TotalHeight - 2f;
                PdfContentByte cb = writer.DirectContent;
                cb.SaveState();
                cb.SetLineWidth(2f);
                cb.SetColorStroke(cAzul);
                cb.MoveTo(document.LeftMargin, lineY);
                cb.LineTo(document.PageSize.Width - document.RightMargin, lineY);
                cb.Stroke();
                cb.RestoreState();
            }
        }

    }
}
