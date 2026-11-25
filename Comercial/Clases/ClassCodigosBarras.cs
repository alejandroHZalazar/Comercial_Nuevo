using MySqlConnector;
using System;

namespace Comercial.Clases
{
    /// <summary>
    /// Códigos de barras y de proveedor automáticos (misma lógica que Comercial Web). La reserva la hacen los
    /// procedimientos almacenados dentro de la transacción del alta (scripts/codigo_barras_automatico_escritorio.sql);
    /// acá solo se consulta si la opción está activa e instalada.
    /// </summary>
    static class ClassCodigosBarras
    {
        public const string AvisoPantalla =
            "Si deja vacíos el código de barras y el de proveedor, se generan automáticamente (el mismo valor en ambos).";

        public const string FaltaScript =
            "La generación automática de códigos está activa pero faltan sus procedimientos en la base de datos. " +
            "Ejecute scripts/codigo_barras_automatico_escritorio.sql (y, si usa variantes, variantes_escritorio_01_sp_guardar.sql).";

        /// <summary>true solo con parametros(productos, generaCodigoBarras) = '1'.</summary>
        public static bool habilitado()
        {
            object valor = escalar("select valor from parametros where modulo = 'productos' and parametro = 'generaCodigoBarras' limit 1");
            return valor != null && valor != DBNull.Value && valor.ToString().Trim() == "1";
        }

        public static bool procedimientosInstalados()
        {
            return Convert.ToInt32(escalar(
                "select count(*) from information_schema.ROUTINES where ROUTINE_SCHEMA = DATABASE() " +
                "and ROUTINE_NAME = 'sp_CodigoBarras_Reservar'")) > 0;
        }

        private static object escalar(string sql)
        {
            classDatos datos = new classDatos();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, datos.abrirConexion()))
                    return cmd.ExecuteScalar();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
