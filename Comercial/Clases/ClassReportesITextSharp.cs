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

            string path = Path.Combine(downloads, $"Venta_{unaVenta}_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.pdf");

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
                p4.Add(new Chunk(cab["Cond_IVA"].ToString(), normal));
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

                // ================= HEADER =================
                int fila = 1;

                DataRow cab = dt.Rows[0];

                if (cab["imagen"] != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])cab["imagen"];

                    using (var ms = new MemoryStream(imageBytes))
                    {
                        var picture = ws.AddPicture(ms)
                            .MoveTo(ws.Cell("A1"), 5, 5) // 🔹 leve margen
                            .WithSize(80, 40); // 🔹 tamaño tipo PDF

                        picture.WithPlacement(XLPicturePlacement.FreeFloating);
                    }
                }

                // 🔹 Definir ancho de columnas (simula proporción 4-1-5)
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
                ws.Range("A1:C5").Merge();
                ws.Range("A1:C5").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

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

                // ================= CENTRO (X) =================
                ws.Range("D1:D2").Merge();
                ws.Range("D1:D2").Value = "X";
                ws.Range("D1:D2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range("D1:D2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Range("D1:D2").Style.Font.Bold = true;
                ws.Range("D1:D2").Style.Font.FontSize = 16;
                ws.Range("D1:D2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // 🔹 Línea vertical (simulada)
                ws.Range("D3:D5").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range("D3:D5").Style.Border.RightBorder = XLBorderStyleValues.Thin;

                // 🔹 Línea horizontal inferior
                ws.Range("D5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                // ================= DERECHA =================
                ws.Range("E1:I5").Merge();
                ws.Range("E1:I5").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                var rightCell = ws.Cell("E1");
                rightCell.Value =
                    $"NRO. VENTA: {cab["nroVenta"]}\n\n" +
                    $"FECHA: {Convert.ToDateTime(cab["fecha"]):dd/MM/yyyy}\n\n" +
                    $"CUIT: {cab["cuil"]}\n" +
                    "DOCUMENTO NO VALIDO COMO FACTURA";

                rightCell.Style.Alignment.WrapText = true;
                rightCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                rightCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                // 🔹 Espacio después del header
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

                    // 🔹 formato números
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

                // Total sin IVA
                ws.Cell(fila, colInicio).Value = "Total Sin IVA";
                ws.Cell(fila, colInicio).Style.Font.Bold = true;
                ws.Cell(fila, colInicio + 1).Value = totalSinIva;
                ws.Range(fila, colInicio + 1, fila, colInicio + 2).Merge();
                ws.Range(fila, colInicio, fila, colInicio + 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                fila++;

                SetTotalRow("IVA", IVA, ivaCalculado);
                SetTotalRow("Percep. IIBB PCIA. Chaco - Misiones", impuesto, percepcion);
                SetTotalRow("TOTAL GENERAL", 0, totalGeneral, true);

                // 🔹 auto ajuste columnas
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

    }
}
