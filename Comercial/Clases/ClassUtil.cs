using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Clases
{
    public class ClassUtil
    {
        public void ExportarDataGridViewACsv(DataGridView dgv, string nombreArchivoBase)
        {
            if (dgv.Rows.Count == 0)
                return;

            // 🔹 Carpeta Descargas
            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            // 🔹 Nombre con fecha/hora
            string fecha = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");

            string path = Path.Combine(downloads, $"{nombreArchivoBase}_{fecha}.csv");

            var sb = new StringBuilder();
            string separador = ";";

            // ================= HEADER =================
            List<string> headers = new List<string>();

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                    headers.Add(EscaparCsv(col.HeaderText));
            }

            sb.AppendLine(string.Join(separador, headers));

            // ================= FILAS =================
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                List<string> campos = new List<string>();

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (dgv.Columns[cell.ColumnIndex].Visible)
                    {
                        string valor = Convert.ToString(cell.Value, new System.Globalization.CultureInfo("es-AR")) ?? "";
                        campos.Add(EscaparCsv(valor));
                    }
                }

                sb.AppendLine(string.Join(separador, campos));
            }

            // 🔹 Guardar archivo
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

            // 🔹 Abrir automáticamente
            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        private string EscaparCsv(string valor)
        {
            if (valor.Contains("\""))
                valor = valor.Replace("\"", "\"\"");

            if (valor.Contains(";") || valor.Contains("\n") || valor.Contains("\r"))
                valor = $"\"{valor}\"";

            return valor;
        }

        public void ExportarDataTableACsv(DataTable dt, string nombreArchivoBase)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            // 🔹 Carpeta Descargas
            string downloads = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            // 🔹 Nombre con fecha
            string fecha = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            string path = Path.Combine(downloads, $"{nombreArchivoBase}_{fecha}.csv");

            var sb = new StringBuilder();
            string separador = ";"; // Excel en español

            var culture = new System.Globalization.CultureInfo("es-AR");

            // ================= HEADER =================
            List<string> headers = new List<string>();

            foreach (DataColumn col in dt.Columns)
            {
                headers.Add(EscaparCsv(col.ColumnName));
            }

            sb.AppendLine(string.Join(separador, headers));

            // ================= FILAS =================
            foreach (DataRow row in dt.Rows)
            {
                List<string> campos = new List<string>();

                foreach (DataColumn col in dt.Columns)
                {
                    object valorObj = row[col];

                    string valor = "";

                    if (valorObj != DBNull.Value)
                    {
                        if (valorObj is decimal || valorObj is double || valorObj is float)
                            valor = Convert.ToDecimal(valorObj).ToString("N2", culture);
                        else if (valorObj is DateTime)
                            valor = Convert.ToDateTime(valorObj).ToString("dd/MM/yyyy");
                        else
                            valor = valorObj.ToString();
                    }

                    campos.Add(EscaparCsv(valor));
                }

                sb.AppendLine(string.Join(separador, campos));
            }

            // 🔹 Guardar archivo
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

            // 🔹 Abrir automáticamente
            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }

        public static void CargarRichConFormato(string texto, RichTextBox rich)
        {
            rich.Clear();

            Font fontNormal = new Font(rich.Font, FontStyle.Regular);
            Font fontBold = new Font(rich.Font, FontStyle.Bold);

            int i = 0;

            while (i < texto.Length)
            {
                if (texto.Substring(i).StartsWith("<b>"))
                {
                    i += 3; // saltar <b>
                    int fin = texto.IndexOf("</b>", i);
                    if (fin == -1) break;

                    string contenido = texto.Substring(i, fin - i);

                    int start = rich.TextLength;
                    rich.AppendText(contenido);

                    // 🔹 aplicar negrita SOLO a este bloque
                    rich.Select(start, contenido.Length);
                    rich.SelectionFont = fontBold;

                    // 🔹 volver a normal (CLAVE)
                    rich.SelectionStart = rich.TextLength;
                    rich.SelectionLength = 0;
                    rich.SelectionFont = fontNormal;

                    i = fin + 4; // saltar </b>
                }
                else if (texto[i] == '\n')
                {
                    rich.AppendText(Environment.NewLine);
                    i++;
                }
                else
                {
                    int start = rich.TextLength;
                    rich.AppendText(texto[i].ToString());

                    // 🔹 asegurar que esto sea normal
                    rich.Select(start, 1);
                    rich.SelectionFont = fontNormal;

                    i++;
                }
            }

            rich.SelectionStart = rich.TextLength;
            rich.SelectionLength = 0;
        }
    }
}
