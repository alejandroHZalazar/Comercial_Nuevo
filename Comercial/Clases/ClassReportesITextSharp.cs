using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;

namespace Comercial.Clases
{
    public class ClassReportesITextSharp
    {


        public void GenerarYMostrarRecibo(string unIdRecibo, string logoPath, string nombreEmpresa, string direccionEmpresa, string telEmpresa, string cuitEmpresa, DateTime unaFechaRecibo,
                                            string nombreCliente, string cuitCliente, string observCobro, decimal importe)

        {
            string rutaPdf = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Recibo_No_Fiscal_" + unIdRecibo + ".pdf"
            );

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

    }
}
