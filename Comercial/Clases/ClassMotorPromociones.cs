using System;
using System.Collections.Generic;
using System.Linq;

namespace Comercial.Clases
{
    /// <summary>Tipos de regla soportados por el motor de promociones por cantidad / porcentaje.</summary>
    static class TipoPromo
    {
        public const string Porcentaje = "PORC";
        public const string NxM = "NXM";
    }

    /// <summary>Regla de promoción ya cargada en memoria (sin acceso a datos).</summary>
    class ReglaPromo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = TipoPromo.Porcentaje;
        public decimal? Porcentaje { get; set; }
        public int? Lleva { get; set; }
        public int? Paga { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public bool Activa { get; set; } = true;
        public HashSet<int> ProductoIds { get; set; } = new HashSet<int>();

        /// <summary>Vigente = activa y 'ahora' dentro de [Desde, Hasta] (inclusive en ambos extremos).</summary>
        public bool EstaVigente(DateTime ahora) { return Activa && ahora >= Desde && ahora <= Hasta; }
    }

    /// <summary>Línea de entrada al motor. 'Clave' es un identificador opaco que el motor devuelve intacto.</summary>
    class LineaPromoEntrada
    {
        public int Clave { get; set; }
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioSinIva { get; set; }
    }

    /// <summary>Promoción aplicada a una línea.</summary>
    class LineaPromoResultado
    {
        public int Clave { get; set; }
        public int ProductoId { get; set; }
        public int ReglaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal? Porcentaje { get; set; }
        public int? Lleva { get; set; }
        public int? Paga { get; set; }
        /// <summary>Unidades bonificadas en ESTA línea (solo NxM; 0 para porcentaje).</summary>
        public decimal CantBonificada { get; set; }
        /// <summary>Descuento efectivo de la línea en % (0-100): lo que debe verse en la columna Desc/Rec.</summary>
        public decimal DescuentoPct { get; set; }
        /// <summary>Importe sin IVA que se deja de cobrar en esta línea.</summary>
        public decimal Ahorro { get; set; }
    }

    /// <summary>Producto que está a una unidad de completar un grupo NxM (aviso informativo).</summary>
    class LineaPromoProxima
    {
        public int Clave { get; set; }
        public int ProductoId { get; set; }
        public int ReglaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Lleva { get; set; }
        public int Paga { get; set; }
        /// <summary>Cantidad total actual del producto en la operación.</summary>
        public decimal CantidadActual { get; set; }
    }

    /// <summary>
    /// Motor puro (sin base de datos, sin hora del sistema) que decide qué promoción aplicar a cada producto.
    /// Port 1:1 de MotorPromociones de Comercial Web (misma lógica, mismos desempates).
    ///
    /// Reglas:
    ///   * Se agrupan las líneas por producto y se evalúa la CANTIDAD TOTAL del producto.
    ///   * Candidatas: reglas vigentes que incluyen el producto y cuyas condiciones se cumplen
    ///     (NxM requiere cantidad >= N; porcentaje siempre).
    ///   * Importe a pagar (en unidades de precio): PORC = q × (1 − %/100); NxM = q − grupos × (N − M),
    ///     con grupos = floor(q / N). Las unidades que no completan un grupo conservan su precio.
    ///   * Se elige la candidata de MENOR importe final. No se acumulan promociones: una sola por producto.
    ///   * Desempate determinístico: a igual importe gana la regla de menor Id (la más antigua).
    ///   * Si un producto tiene varias líneas, las unidades bonificadas (NxM) se asignan en orden de línea.
    /// </summary>
    static class MotorPromociones
    {
        public static List<LineaPromoResultado> Evaluar(IEnumerable<LineaPromoEntrada> lineas, IEnumerable<ReglaPromo> reglas, DateTime ahora)
        {
            List<LineaPromoResultado> resultado = new List<LineaPromoResultado>();
            List<ReglaPromo> vigentes = reglas.Where(r => r.EstaVigente(ahora)).OrderBy(r => r.Id).ToList();
            if (vigentes.Count == 0) return resultado;

            List<LineaPromoEntrada> validas = lineas.Where(l => l.Cantidad > 0 && l.PrecioSinIva > 0 && l.ProductoId > 0).ToList();

            foreach (IGrouping<int, LineaPromoEntrada> grupo in validas.GroupBy(l => l.ProductoId))
            {
                List<LineaPromoEntrada> prodLineas = grupo.ToList();
                decimal q = prodLineas.Sum(l => l.Cantidad);

                decimal mejorBon;
                ReglaPromo mejor = Mejor(grupo.Key, q, vigentes, out mejorBon);
                if (mejor == null) continue;

                // Reparto entre las líneas del producto
                decimal bonifRestante = mejorBon;
                foreach (LineaPromoEntrada l in prodLineas)
                {
                    decimal bonifLinea = 0m, pct, ahorro;
                    if (mejor.Tipo == TipoPromo.NxM)
                    {
                        bonifLinea = Math.Min(l.Cantidad, bonifRestante);
                        bonifRestante -= bonifLinea;
                        pct = l.Cantidad > 0 ? Math.Round(bonifLinea / l.Cantidad * 100m, 8) : 0m;
                        ahorro = bonifLinea * l.PrecioSinIva;
                    }
                    else
                    {
                        pct = mejor.Porcentaje.Value;
                        ahorro = l.Cantidad * l.PrecioSinIva * pct / 100m;
                    }

                    if (pct <= 0m) continue;         // línea sin beneficio (p. ej. NxM cuyo grupo cae en otra línea)

                    resultado.Add(new LineaPromoResultado
                    {
                        Clave = l.Clave,
                        ProductoId = l.ProductoId,
                        ReglaId = mejor.Id,
                        Nombre = mejor.Nombre,
                        Tipo = mejor.Tipo,
                        Porcentaje = mejor.Tipo == TipoPromo.Porcentaje ? mejor.Porcentaje : null,
                        Lleva = mejor.Tipo == TipoPromo.NxM ? mejor.Lleva : null,
                        Paga = mejor.Tipo == TipoPromo.NxM ? mejor.Paga : null,
                        CantBonificada = bonifLinea,
                        DescuentoPct = pct,
                        Ahorro = ahorro
                    });
                }
            }

            return resultado;
        }

        /// <summary>
        /// Mejor regla para UN producto con la cantidad total q (la de menor importe final; a igualdad, la de menor Id,
        /// porque 'vigentes' viene ordenada por Id). Devuelve también las unidades bonificadas (solo NxM).
        /// </summary>
        private static ReglaPromo Mejor(int productoId, decimal q, IReadOnlyList<ReglaPromo> vigentes, out decimal bonificadas)
        {
            ReglaPromo mejor = null;
            decimal mejorPag = q;            // unidades "equivalentes" a pagar; solo aplica si es MENOR que q
            decimal mejorBon = 0m;

            foreach (ReglaPromo r in vigentes)   // orden por Id: el empate lo gana la primera encontrada
            {
                if (!r.ProductoIds.Contains(productoId)) continue;

                decimal aPagar, bonif = 0m;
                if (r.Tipo == TipoPromo.NxM)
                {
                    if (!r.Lleva.HasValue || !r.Paga.HasValue || r.Lleva.Value < 2 ||
                        r.Paga.Value < 1 || r.Paga.Value >= r.Lleva.Value) continue;
                    if (q < r.Lleva.Value) continue;
                    decimal grupos = Math.Floor(q / r.Lleva.Value);
                    bonif = grupos * (r.Lleva.Value - r.Paga.Value);
                    aPagar = q - bonif;
                }
                else if (r.Tipo == TipoPromo.Porcentaje)
                {
                    if (!r.Porcentaje.HasValue || r.Porcentaje.Value <= 0m || r.Porcentaje.Value > 100m) continue;
                    aPagar = q * (1m - r.Porcentaje.Value / 100m);
                }
                else continue;

                if (aPagar < mejorPag)           // estrictamente menor: a igualdad se conserva la de menor Id
                {
                    mejor = r; mejorPag = aPagar; mejorBon = bonif;
                }
            }
            bonificadas = mejorBon;
            return mejor;
        }

        /// <summary>
        /// Avisos de proximidad (solo informativos, no modifican importes): productos a UNA unidad de completar un grupo NxM.
        ///   * Se evalúa la cantidad total del producto (q) y la cantidad q + 1 con el mismo criterio que Evaluar.
        ///   * Hay aviso solo si con q + 1 la ganadora es una NxM que bonifica MÁS unidades que con q y q + 1 completa
        ///     un grupo ((q + 1) múltiplo de N).
        ///   * Solo con cantidades enteras.
        ///   * Con varias líneas del mismo producto se avisa sobre la primera línea.
        /// </summary>
        public static List<LineaPromoProxima> Proximas(IEnumerable<LineaPromoEntrada> lineas, IEnumerable<ReglaPromo> reglas, DateTime ahora)
        {
            List<LineaPromoProxima> res = new List<LineaPromoProxima>();
            List<ReglaPromo> vigentes = reglas.Where(r => r.EstaVigente(ahora)).OrderBy(r => r.Id).ToList();
            if (vigentes.Count == 0) return res;

            List<LineaPromoEntrada> validas = lineas.Where(l => l.Cantidad > 0 && l.PrecioSinIva > 0 && l.ProductoId > 0).ToList();
            foreach (IGrouping<int, LineaPromoEntrada> grupo in validas.GroupBy(l => l.ProductoId))
            {
                decimal q = grupo.Sum(l => l.Cantidad);
                if (q != Math.Floor(q)) continue;

                decimal bonActual, bonSig;
                ReglaPromo actual = Mejor(grupo.Key, q, vigentes, out bonActual);
                ReglaPromo sig = Mejor(grupo.Key, q + 1m, vigentes, out bonSig);
                if (sig == null || sig.Tipo != TipoPromo.NxM) continue;
                if (bonSig <= (actual == null ? 0m : bonActual)) continue;
                if ((q + 1m) % sig.Lleva.Value != 0m) continue;

                res.Add(new LineaPromoProxima
                {
                    Clave = grupo.First().Clave,
                    ProductoId = grupo.Key,
                    ReglaId = sig.Id,
                    Nombre = sig.Nombre,
                    Lleva = sig.Lleva.Value,
                    Paga = sig.Paga.Value,
                    CantidadActual = q
                });
            }
            return res;
        }
    }
}
