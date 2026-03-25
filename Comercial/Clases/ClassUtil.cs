using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    }
}
