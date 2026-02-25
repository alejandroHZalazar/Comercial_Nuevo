using MySqlConnector;
using System;
using System.Data;

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
    }
}
