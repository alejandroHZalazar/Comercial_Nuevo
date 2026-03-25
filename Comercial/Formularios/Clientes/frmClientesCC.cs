using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Clientes
{
    public partial class frmClientesCC : Form
    {
        int _cliente;
        public frmClientesCC(int cliente)
        {
            _cliente = cliente;
            InitializeComponent();
        }

        private void frmClientesCC_Load(object sender, EventArgs e)
        {
            cargarGrilla();
        }

        private void cargarGrilla()
        {
            Clases.ClassClientes instClie = new Clases.ClassClientes();
            dgvCC.DataSource = instClie.traerCC(_cliente);
            dgvCC.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCC.Columns["Debe"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCC.Columns["Debe"].DefaultCellStyle.Format = "C2";
            dgvCC.Columns["Haber"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCC.Columns["Haber"].DefaultCellStyle.Format = "C2";
            dgvCC.Columns["Saldo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCC.Columns["Saldo"].DefaultCellStyle.Format = "C2";
            txtSaldo.Text = calcularTotal().ToString("C");
        }

        private decimal calcularTotal()
        {
            decimal saldo = 0;

            foreach (DataGridViewRow fila in dgvCC.Rows)
            {
                if (decimal.Parse(fila.Cells["Saldo"].Value.ToString()) > 0)
                {
                    if (decimal.Parse(fila.Cells["Debe"].Value.ToString()) > 0)
                    {
                        saldo += decimal.Parse(fila.Cells["Saldo"].Value.ToString());
                    }
                    else
                    {
                        saldo -= decimal.Parse(fila.Cells["Saldo"].Value.ToString());
                    }
                }
            }

            return saldo;
        }

        private void imprimirCobro(DataTable recibo)
        {
            Clases.ClassReportesITextSharp instReport = new Clases.ClassReportesITextSharp();
            Clases.ClassClientes instClie = new Clases.ClassClientes();
            var logo = Clases.ClassParametros.buscarParametro("login", "logo");
            var nombreEmpresa = Clases.ClassParametros.buscarParametro("empresa", "nombre");
            var direccionEmpresa = Clases.ClassParametros.buscarParametro("empresa", "direccion");
            var telEmpresa = Clases.ClassParametros.buscarParametro("empresa", "telefono");
            var cuilEmpresa = Clases.ClassParametros.buscarParametro("empresa", "cuit");
            var saldo = ObtenerSaldoCliente(instClie.traerCC(_cliente));
            instReport.GenerarYMostrarRecibo(recibo.Rows[0]["Recibo"].ToString(), logo, nombreEmpresa, direccionEmpresa, telEmpresa, cuilEmpresa, DateTime.Parse(recibo.Rows[0]["Fecha"].ToString()), recibo.Rows[0]["Cliente"].ToString(), recibo.Rows[0]["cuil"].ToString(), recibo.Rows[0]["Observaciones"].ToString(), decimal.Parse(recibo.Rows[0]["ImporteTotal"].ToString()), saldo);
        }

        private decimal ObtenerSaldoCliente(DataTable dt)
        {
            decimal totalDebe = 0;
            decimal totalHaber = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalDebe += Convert.ToDecimal(row["Debe"]);
                totalHaber += Convert.ToDecimal(row["Haber"]);
            }

            return totalDebe - totalHaber;
        }
        private void btnCobrar_Click(object sender, EventArgs e)
        {
            BindingList<Clases.ClassVentas.CobroFormasPago> dtFormasPAgo = new BindingList<Clases.ClassVentas.CobroFormasPago>();
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            Clases.ClassClientes instClie = new Clases.ClassClientes();
            var planPagoDT = instConfig.traerPlanesPagoPorId(int.Parse(Clases.ClassParametros.buscarParametro("Cobros", "idPlanEfectivo")));
            int tieneCaja = Clases.ClassParametros.buscarParametro("caja", "haceCaja") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("caja", "haceCaja"));
            int CajaId = 0;
            if (tieneCaja == 1)
            {
                Clases.ClassCaja instCaja = new Clases.ClassCaja();
                DataTable cajaEstado = instCaja.traerEstadoCaja(int.Parse(Environment.GetEnvironmentVariable("idUser")));

                bool cajaAbierta = cajaEstado.Rows.Count == 0 ? false : (cajaEstado.Rows[0]["estado"].ToString() == "ABIERTA" ? true : false);
                CajaId = cajaEstado.Rows.Count == 0 ? 0 : int.Parse(cajaEstado.Rows[0]["caja_id"].ToString());
                if (!cajaAbierta)
                {

                    MessageBox.Show(this, "Debe Abrir Caja", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
            }

            if (planPagoDT.Rows.Count == 0) return;
            var planPagoId = int.Parse(planPagoDT.Rows[0]["id"].ToString());
            Formularios.Ventas.frmImputacionVenta unFrmImputacion = new Formularios.Ventas.frmImputacionVenta(calcularTotal() <= 0 ? 0 : calcularTotal(), planPagoId);
            unFrmImputacion.ShowDialog();
            if (unFrmImputacion.DialogResult == DialogResult.OK)
            {
                dtFormasPAgo = unFrmImputacion.unDT;
                var imputacion = dtFormasPAgo.Sum(x => x.Importe);
                if (imputacion <= 0) return;
                var salida = instClie.CobrarCliente(_cliente, imputacion, tieneCaja, CajaId, dtFormasPAgo[0].idMedio);

                if (salida != -1)
                {
                    MessageBox.Show(this, "Cobro Registrado con éxito!!", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    var recibo = instClie.traerDatosRecibo(salida);
                    if (recibo.Rows.Count > 0)
                    {
                        imprimirCobro(recibo);
                    }
                    cargarGrilla();
                }
                else
                {
                    MessageBox.Show(this, "Problemas para registrar cobro", "CLIENTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNC_Click(object sender, EventArgs e)
        {
            frmNC unFrmNC = new frmNC(_cliente, calcularTotal() <= 0 ? 0 : calcularTotal());
            unFrmNC.ShowDialog();
            cargarGrilla();
        }

        private void btnND_Click(object sender, EventArgs e)
        {
            frmAddND unFrmNC = new frmAddND(_cliente);
            unFrmNC.ShowDialog();
            cargarGrilla();
        }

        private void frmClientesCC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F2)
            {
                btnCobrar_Click(null, null);
            }

            if (e.KeyData == Keys.F3)
            {
                btnNC_Click(null, null);
            }

            if (e.KeyData == Keys.F4)
            {
                btnND_Click(null, null);
            }

            if (e.KeyData == Keys.F5)
            {
                btnImprimirCC_Click(null, null);
            }
        }

        private void dgvCC_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // evita encabezados
            {
                var fila = dgvCC.Rows[e.RowIndex];

                if (fila.Cells["Movimiento"].Value.ToString() != "Cobro") return;

                var id = int.Parse(fila.Cells["Numero Referencia"].Value.ToString());

                Clases.ClassClientes instClie = new Clases.ClassClientes();
                var recibo = instClie.traerDatosRecibo(id);
                if (recibo.Rows.Count > 0)
                {
                    imprimirCobro(recibo);
                }
            }
        }

        private void btnImprimirCC_Click(object sender, EventArgs e)
        {
            GenerarEstadoCuentaPDF();
        }

        private void GenerarEstadoCuentaPDF()
        {
            var instClie = new Clases.ClassClientes();
            DataTable dt = instClie.traerCC(_cliente);
            DataTable cliente = instClie.traeTodosDatos(_cliente);
            if (cliente.Rows.Count == 0) return;

            // 📅 Fecha desde (primer día del mes - 2 meses)
            DateTime fechaDesde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-2);

            // Filtrar
            var rowsFiltradas = dt.AsEnumerable()
                .Where(r => Convert.ToDateTime(r["Fecha"]) >= fechaDesde);

            if (!rowsFiltradas.Any())
            {
                MessageBox.Show("No hay movimientos en el período.");
                return;
            }

            DataTable dtFiltrado = rowsFiltradas.CopyToDataTable();

            // 🧾 Crear PDF
            string downloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            string nombreSeguro = string.Join("_", cliente.Rows[0]["nombreComercial"].ToString().Split(Path.GetInvalidFileNameChars()));

            string path = Path.Combine(downloads, $"EstadoCuenta_{nombreSeguro}_{DateTime.Now:yyyyMMdd}.pdf");

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // 🔹 HEADER
                AgregarHeader(doc);

                // 🔹 SUBTITULO
                doc.Add(new Paragraph($"Estado de cuenta desde {fechaDesde:dd/MM/yyyy}"));
                doc.Add(new Paragraph($"Sres: {cliente.Rows[0]["nombreComercial"]}"));
                doc.Add(new Paragraph(" "));

                // 🔹 TABLA
                // 🔹 FUENTES
                var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                // 🔹 TABLA (SIN REFERENCIA)
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2, 2.25f, 2, 2, 2 });

                // 🔹 HEADER
                PdfPCell HeaderCell(string text)
                {
                    var cell = new PdfPCell(new Phrase(text, fontHeader));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    return cell;
                }

                table.AddCell(HeaderCell("Fecha"));
                table.AddCell(HeaderCell("Movimiento"));
                table.AddCell(HeaderCell("Debe"));
                table.AddCell(HeaderCell("Haber"));
                table.AddCell(HeaderCell("Saldo"));

                // 🔹 FUNCIONES CELDAS
                PdfPCell CellLeft(string text)
                {
                    var cell = new PdfPCell(new Phrase(text, fontNormal));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    return cell;
                }

                PdfPCell CellRight(string text)
                {
                    var cell = new PdfPCell(new Phrase(text, fontNormal));
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    return cell;
                }

                // 🔹 TOTALES
                decimal totalDebe = 0;
                decimal totalHaber = 0;
                decimal totalSaldo = 0;

                // 🔹 DATOS
                foreach (DataRow row in dtFiltrado.Rows)
                {
                    decimal debe = Convert.ToDecimal(row["Debe"]);
                    decimal haber = Convert.ToDecimal(row["Haber"]);
                    decimal saldo = Convert.ToDecimal(row["Saldo"]);

                    table.AddCell(CellLeft(Convert.ToDateTime(row["Fecha"]).ToString("dd/MM/yyyy")));
                    table.AddCell(CellLeft(row["Movimiento"].ToString()));
                    table.AddCell(CellRight(debe.ToString("C2")));
                    table.AddCell(CellRight(haber.ToString("C2")));
                    table.AddCell(CellRight(saldo.ToString("C2")));

                    totalDebe += debe;
                    totalHaber += haber;
                    totalSaldo += saldo; // 🔥 SUMATORIA
                }

                // 🔹 FILA DE TOTALES
                PdfPCell totalLabel = new PdfPCell(new Phrase("TOTALES", fontHeader));
                totalLabel.Colspan = 2;
                totalLabel.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalLabel.BackgroundColor = BaseColor.LIGHT_GRAY;

                table.AddCell(totalLabel);
                table.AddCell(CellRight(totalDebe.ToString("C2")));
                table.AddCell(CellRight(totalHaber.ToString("C2")));
                table.AddCell(CellRight(totalSaldo.ToString("C2")));

                doc.Add(table);

                // 🔹 SALDO FINAL
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Saldo Total: {totalSaldo.ToString("C2")}", fontHeader));

                doc.Close();

                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
        }

        private void AgregarHeader(Document doc)
        {
            byte[] logoBytes = Clases.ClassParametros.traerImagenLogotipo();

            PdfPTable header = new PdfPTable(2);
            header.WidthPercentage = 100;
            header.SetWidths(new float[] { 1, 3 });

            // 🖼 LOGO
            if (logoBytes != null)
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoBytes);
                logo.ScaleToFit(80f, 80f);
                PdfPCell cellLogo = new PdfPCell(logo);
                cellLogo.Border = 0;
                header.AddCell(cellLogo);
            }
            else
            {
                header.AddCell("");
            }

            // 🏢 DATOS EMPRESA


            var NombreEmpresa = Clases.ClassParametros.buscarParametro("empresa", "nombre");
            var telEmpresa = Clases.ClassParametros.buscarParametro("empresa", "telefono");
            var dirEmpresa = Clases.ClassParametros.buscarParametro("empresa", "direccion") + " " + Clases.ClassParametros.buscarParametro("empresa", "localidad");
            var mailEmpresa = Clases.ClassParametros.buscarParametro("empresa", "mail");

            Paragraph datos = new Paragraph();
            datos.Add(new Chunk(NombreEmpresa + "\n", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            datos.Add(new Chunk($"Tel: {telEmpresa}\n"));
            datos.Add(new Chunk($"Dirección: {dirEmpresa}\n"));
            datos.Add(new Chunk($"Email: {mailEmpresa}\n"));

            PdfPCell cellDatos = new PdfPCell(datos);
            cellDatos.Border = 0;
            header.AddCell(cellDatos);

            doc.Add(header);
            doc.Add(new Paragraph(" "));
        }
    }
}
