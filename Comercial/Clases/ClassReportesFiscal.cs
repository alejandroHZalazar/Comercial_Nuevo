using MySqlConnector;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace Comercial.Clases
{
    public class ClassReportesFiscal
    {
        Clases.classDatos instDatos = new classDatos();
        public DataTable traerResumenFacturacion(DateTime desde, DateTime hasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_fiscal_facturacion_resumen", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("p_fecha_desde", desde);
            a1.SelectCommand.Parameters.AddWithValue("p_fecha_hasta", hasta);            

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerResumenFacturacionDiario(DateTime desde, DateTime hasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_fiscal_facturacion_resumen_Diario", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("p_fecha_desde", desde);
            a1.SelectCommand.Parameters.AddWithValue("p_fecha_hasta", hasta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerDetalleFacturacion(DateTime desde, DateTime hasta)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_fiscal_facturacion_detalle", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("p_fecha_desde", desde);
            a1.SelectCommand.Parameters.AddWithValue("p_fecha_hasta", hasta);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public void ExportarResumenDiario (DataTable resumen)
        {
            string carpetaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            string archivo = $"ResumenFActuraDiario_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            string path = Path.Combine(carpetaDescargas, archivo);

            var sb = new StringBuilder();

            // Columnas
            sb.AppendLine("Fecha;Cantidad Facturas;Total Facturado;Total Costo;Ganancia Bruta;Total IVA");

            // Detalle
            foreach (DataRow row in resumen.Rows)
            {
                sb.AppendLine($"{row["Fecha"]};{row["Cantidad Facturas"]};{row["Total Facturado"]};{row["Total Costo"]};{row["Ganancia Bruta"]};{row["Total IVA"]}");
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
    }
}
