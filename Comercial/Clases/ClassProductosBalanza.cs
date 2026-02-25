using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comercial.Clases
{
    public class ClassProductosBalanza
    {
        public static string ExtraerPorPosicion(string codigo, string posicion)
        {
            var partes = posicion.Split(',');
            int inicio = int.Parse(partes[0]) - 1;      // pasamos a 0-based
            int fin = int.Parse(partes[1]) - inicio;

            return codigo.Substring(inicio, fin);
        }

        public static bool esCodigoBalanza(string codigo, string prefijoBalanza)
        {
            return codigo.Length == 13 && codigo.StartsWith(prefijoBalanza);
        }
    }
}
