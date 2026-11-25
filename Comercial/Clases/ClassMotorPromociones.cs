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

        /// <summary>
        /// Solo NxM. true = las cantidades de las distintas variantes de una misma ficha (entre los productos de la
        /// regla) se acumulan para completar el grupo. false = cada producto se evalúa por separado (comportamiento original).
        /// </summary>
        public bool AgrupaVariantes { get; set; }

        /// <summary>Vigente = activa y 'ahora' dentro de [Desde, Hasta] (inclusive en ambos extremos).</summary>
        public bool EstaVigente(DateTime ahora) { return Activa && ahora >= Desde && ahora <= Hasta; }

        /// <summary>Regla NxM con acumulación de variantes (la que se evalúa por ficha y no por producto).</summary>
        public bool AcumulaPorFicha { get { return AgrupaVariantes && Tipo == TipoPromo.NxM; } }
    }

    /// <summary>Línea de entrada al motor. 'Clave' es un identificador opaco que el motor devuelve intacto.</summary>
    class LineaPromoEntrada
    {
        public int Clave { get; set; }
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioSinIva { get; set; }
        /// <summary>Id de la ficha principal si el producto es una variante (0 = producto común o ficha sin variantes).</summary>
        public int FichaId { get; set; }
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
        /// <summary>true = la promoción se completó acumulando variantes de una misma ficha.</summary>
        public bool PorFicha { get; set; }
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
        /// <summary>Cantidad total actual del producto (o de la ficha, si EsGrupo) en la operación.</summary>
        public decimal CantidadActual { get; set; }
        /// <summary>true = la unidad que falta puede ser de cualquiera de las variantes de la ficha.</summary>
        public bool EsGrupo { get; set; }
        public int FichaId { get; set; }
    }

    /// <summary>
    /// Motor puro (sin base de datos, sin hora del sistema) que decide qué promoción aplicar a cada producto.
    /// Port de MotorPromociones de Comercial Web (misma lógica, mismos desempates) + acumulación opcional de variantes.
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
    ///
    /// Acumulación de variantes (solo reglas NxM con AgrupaVariantes = true, y solo para productos que son variantes):
    ///   * Primero se evalúa todo como siempre (por producto) usando solo las reglas que no acumulan.
    ///   * Después, para cada ficha, se suman las cantidades de sus variantes que están en la lista de la regla; con ese
    ///     total se calculan los grupos igual que en NxM (floor(q / N), bonifican N − M por grupo) y las unidades
    ///     bonificadas se asignan EN ORDEN DE LÍNEA (la misma forma de elegir las unidades que ya usa el motor).
    ///   * La acumulación reemplaza el resultado individual de esas líneas solo si el ahorro para el cliente es MAYOR
    ///     (a igualdad se conserva lo que ya había). Una línea nunca recibe dos promociones: las líneas usadas por un
    ///     grupo quedan fuera de los siguientes grupos.
    ///   * Un producto que no es variante de ninguna ficha se evalúa siempre por separado, aunque la regla acumule.
    /// </summary>
    static class MotorPromociones
    {
        public static List<LineaPromoResultado> Evaluar(IEnumerable<LineaPromoEntrada> lineas, IEnumerable<ReglaPromo> reglas, DateTime ahora)
        {
            List<ReglaPromo> vigentes = reglas.Where(r => r.EstaVigente(ahora)).OrderBy(r => r.Id).ToList();
            if (vigentes.Count == 0) return new List<LineaPromoResultado>();

            List<LineaPromoEntrada> validas = lineas.Where(l => l.Cantidad > 0 && l.PrecioSinIva > 0 && l.ProductoId > 0).ToList();

            List<ReglaPromo> acumulan = vigentes.Where(r => r.AcumulaPorFicha).ToList();
            List<LineaPromoResultado> resultado = EvaluarPorProducto(validas, vigentes);
            if (acumulan.Count == 0 || !validas.Any(l => l.FichaId > 0)) return resultado;

            return AplicarGruposPorFicha(validas, acumulan, resultado);
        }

        // ── Camino original: por producto ─────────────────────────────────────
        // Las variantes (FichaId > 0) no usan las reglas que acumulan: esas se resuelven por ficha más abajo.
        private static List<LineaPromoResultado> EvaluarPorProducto(List<LineaPromoEntrada> validas, List<ReglaPromo> vigentes)
        {
            List<LineaPromoResultado> resultado = new List<LineaPromoResultado>();
            List<ReglaPromo> sinAcumular = vigentes.Where(r => !r.AcumulaPorFicha).ToList();

            foreach (IGrouping<int, LineaPromoEntrada> grupo in validas.GroupBy(l => l.ProductoId))
            {
                List<LineaPromoEntrada> prodLineas = grupo.ToList();
                decimal q = prodLineas.Sum(l => l.Cantidad);
                bool esVariante = prodLineas.Any(l => l.FichaId > 0);

                decimal mejorBon;
                ReglaPromo mejor = Mejor(grupo.Key, q, esVariante ? sinAcumular : vigentes, out mejorBon);
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

                    resultado.Add(Resultado(l, mejor, bonifLinea, pct, ahorro, false));
                }
            }

            return resultado;
        }

        private static LineaPromoResultado Resultado(LineaPromoEntrada l, ReglaPromo regla, decimal bonifLinea, decimal pct, decimal ahorro, bool porFicha)
        {
            return new LineaPromoResultado
            {
                Clave = l.Clave,
                ProductoId = l.ProductoId,
                ReglaId = regla.Id,
                Nombre = regla.Nombre,
                Tipo = regla.Tipo,
                Porcentaje = regla.Tipo == TipoPromo.Porcentaje ? regla.Porcentaje : null,
                Lleva = regla.Tipo == TipoPromo.NxM ? regla.Lleva : null,
                Paga = regla.Tipo == TipoPromo.NxM ? regla.Paga : null,
                CantBonificada = bonifLinea,
                DescuentoPct = pct,
                Ahorro = ahorro,
                PorFicha = porFicha
            };
        }

        // ── Acumulación de variantes de una misma ficha ───────────────────────
        private class Candidato
        {
            public ReglaPromo Regla;
            public List<LineaPromoEntrada> Lineas;
            public List<decimal> Bonificadas;      // por línea (misma posición)
            public decimal Ahorro;
            public decimal Ganancia;               // ahorro del grupo - ahorro que ya tenían esas líneas
        }

        // Reparte las unidades bonificadas del grupo entre sus líneas EN ORDEN DE LÍNEA (igual que el camino por producto)
        private static Candidato Candidatear(ReglaPromo regla, List<LineaPromoEntrada> lineasFicha, List<LineaPromoResultado> resultadoActual)
        {
            List<LineaPromoEntrada> alcance = lineasFicha.Where(l => regla.ProductoIds.Contains(l.ProductoId)).ToList();
            if (alcance.Count == 0) return null;

            if (!regla.Lleva.HasValue || !regla.Paga.HasValue || regla.Lleva.Value < 2 ||
                regla.Paga.Value < 1 || regla.Paga.Value >= regla.Lleva.Value) return null;

            decimal q = alcance.Sum(l => l.Cantidad);
            if (q < regla.Lleva.Value) return null;

            decimal bonif = Math.Floor(q / regla.Lleva.Value) * (regla.Lleva.Value - regla.Paga.Value);
            List<decimal> porLinea = new List<decimal>();
            decimal restante = bonif, ahorro = 0m;
            foreach (LineaPromoEntrada l in alcance)
            {
                decimal b = Math.Min(l.Cantidad, restante);
                restante -= b;
                porLinea.Add(b);
                ahorro += b * l.PrecioSinIva;
            }

            HashSet<int> claves = new HashSet<int>(alcance.Select(l => l.Clave));
            decimal ahorroPrevio = resultadoActual.Where(r => claves.Contains(r.Clave)).Sum(r => r.Ahorro);

            return new Candidato { Regla = regla, Lineas = alcance, Bonificadas = porLinea, Ahorro = ahorro, Ganancia = ahorro - ahorroPrevio };
        }

        private static List<LineaPromoResultado> AplicarGruposPorFicha(
            List<LineaPromoEntrada> validas, List<ReglaPromo> acumulan, List<LineaPromoResultado> resultado)
        {
            List<LineaPromoEntrada> libres = validas.Where(l => l.FichaId > 0).ToList();   // en orden de línea

            while (libres.Count > 0)
            {
                Candidato mejor = null;
                foreach (IGrouping<int, LineaPromoEntrada> ficha in libres.GroupBy(l => l.FichaId))
                {
                    List<LineaPromoEntrada> lineasFicha = ficha.ToList();
                    foreach (ReglaPromo r in acumulan)          // ya ordenadas por Id
                    {
                        Candidato c = Candidatear(r, lineasFicha, resultado);
                        if (c == null || c.Ganancia <= 0m) continue;     // solo si mejora lo que ya había (a igualdad, se conserva)
                        if (mejor == null || c.Ganancia > mejor.Ganancia) mejor = c;   // a igual ganancia gana la primera (menor Id)
                    }
                }
                if (mejor == null) break;

                // El grupo reemplaza el resultado individual de sus líneas
                HashSet<int> claves = new HashSet<int>(mejor.Lineas.Select(l => l.Clave));
                resultado.RemoveAll(r => claves.Contains(r.Clave));
                for (int i = 0; i < mejor.Lineas.Count; i++)
                {
                    LineaPromoEntrada l = mejor.Lineas[i];
                    decimal b = mejor.Bonificadas[i];
                    if (b <= 0m) continue;     // línea sin beneficio: su grupo cae en otra línea
                    decimal pct = Math.Round(b / l.Cantidad * 100m, 8);
                    resultado.Add(Resultado(l, mejor.Regla, b, pct, b * l.PrecioSinIva, true));
                }
                libres.RemoveAll(l => claves.Contains(l.Clave));    // una línea no recibe dos promociones
            }

            // Mismo orden de entrada que el camino original
            Dictionary<int, int> orden = new Dictionary<int, int>();
            for (int i = 0; i < validas.Count; i++) orden[validas[i].Clave] = i;
            return resultado.OrderBy(r => orden.ContainsKey(r.Clave) ? orden[r.Clave] : int.MaxValue).ToList();
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
        ///   * Reglas que acumulan variantes: se avisa por ficha cuando UNA unidad más (de cualquiera de las variantes
        ///     de la regla) completaría un grupo y el motor efectivamente la aplicaría.
        /// </summary>
        public static List<LineaPromoProxima> Proximas(IEnumerable<LineaPromoEntrada> lineas, IEnumerable<ReglaPromo> reglas, DateTime ahora)
        {
            List<LineaPromoProxima> res = new List<LineaPromoProxima>();
            List<ReglaPromo> vigentes = reglas.Where(r => r.EstaVigente(ahora)).OrderBy(r => r.Id).ToList();
            if (vigentes.Count == 0) return res;

            List<LineaPromoEntrada> validas = lineas.Where(l => l.Cantidad > 0 && l.PrecioSinIva > 0 && l.ProductoId > 0).ToList();
            List<ReglaPromo> sinAcumular = vigentes.Where(r => !r.AcumulaPorFicha).ToList();

            foreach (IGrouping<int, LineaPromoEntrada> grupo in validas.GroupBy(l => l.ProductoId))
            {
                decimal q = grupo.Sum(l => l.Cantidad);
                if (q != Math.Floor(q)) continue;

                // Las variantes no usan las reglas que acumulan (esas se avisan por ficha, abajo)
                List<ReglaPromo> reglasProd = grupo.Any(l => l.FichaId > 0) ? sinAcumular : vigentes;

                decimal bonActual, bonSig;
                ReglaPromo actual = Mejor(grupo.Key, q, reglasProd, out bonActual);
                ReglaPromo sig = Mejor(grupo.Key, q + 1m, reglasProd, out bonSig);
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

            ProximasPorFicha(validas, vigentes, ahora, res);
            return res;
        }

        // Una unidad más en la ficha: se simula y se avisa solo si el motor la aplicaría con esa regla
        private static void ProximasPorFicha(List<LineaPromoEntrada> validas, List<ReglaPromo> vigentes, DateTime ahora, List<LineaPromoProxima> res)
        {
            List<ReglaPromo> acumulan = vigentes.Where(r => r.AcumulaPorFicha).ToList();
            if (acumulan.Count == 0 || !validas.Any(l => l.FichaId > 0)) return;

            List<LineaPromoResultado> actual = Evaluar(validas, vigentes, ahora);

            foreach (IGrouping<int, LineaPromoEntrada> ficha in validas.Where(l => l.FichaId > 0).GroupBy(l => l.FichaId))
            {
                foreach (ReglaPromo r in acumulan)
                {
                    if (!r.Lleva.HasValue || !r.Paga.HasValue || r.Lleva.Value < 2 || r.Paga.Value < 1 || r.Paga.Value >= r.Lleva.Value) continue;

                    List<LineaPromoEntrada> alcance = ficha.Where(l => r.ProductoIds.Contains(l.ProductoId)).ToList();
                    if (alcance.Count == 0) continue;

                    decimal q = alcance.Sum(l => l.Cantidad);
                    if (q != Math.Floor(q)) continue;
                    if ((q + 1m) % r.Lleva.Value != 0m) continue;       // la unidad que falta debe completar un grupo

                    // Misma operación con una unidad más en la primera línea del alcance
                    int claveExtra = alcance[0].Clave;
                    List<LineaPromoEntrada> simuladas = validas.Select(l => new LineaPromoEntrada
                    {
                        Clave = l.Clave, ProductoId = l.ProductoId, PrecioSinIva = l.PrecioSinIva, FichaId = l.FichaId,
                        Cantidad = l.Clave == claveExtra ? l.Cantidad + 1m : l.Cantidad
                    }).ToList();
                    List<LineaPromoResultado> luego = Evaluar(simuladas, vigentes, ahora);

                    HashSet<int> claves = new HashSet<int>(alcance.Select(l => l.Clave));
                    decimal bonAntes = actual.Where(x => claves.Contains(x.Clave) && x.ReglaId == r.Id).Sum(x => x.CantBonificada);
                    decimal bonDespues = luego.Where(x => claves.Contains(x.Clave) && x.ReglaId == r.Id).Sum(x => x.CantBonificada);
                    if (bonDespues <= bonAntes) continue;               // el motor no la aplicaría (otra promoción mejor gana)

                    res.Add(new LineaPromoProxima
                    {
                        Clave = alcance[0].Clave,
                        ProductoId = alcance[0].ProductoId,
                        FichaId = ficha.Key,
                        EsGrupo = true,
                        ReglaId = r.Id,
                        Nombre = r.Nombre,
                        Lleva = r.Lleva.Value,
                        Paga = r.Paga.Value,
                        CantidadActual = q
                    });
                }
            }
        }
    }
}
