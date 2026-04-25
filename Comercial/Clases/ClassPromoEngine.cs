using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Comercial.Clases
{
    // ─── Estructuras de datos ────────────────────────────────────────────────

    public class PromoInfo
    {
        public int    IdPromocion   { get; set; }
        public int    IdProducto    { get; set; }   // producto "promo" que aparece en el ticket
        public string Descripcion   { get; set; }
        public decimal Precio       { get; set; }
        public decimal PrecioSinIva { get; set; }
        public decimal Iva          { get; set; }
        public bool   Dolarizado    { get; set; }
        public bool   EsFraccionado { get; set; }
        public List<SlotInfo> Slots { get; set; } = new List<SlotInfo>();
    }

    public class SlotInfo
    {
        public int    IdSlot             { get; set; }
        public decimal CantidadRequerida { get; set; }
        public string Descripcion        { get; set; }
        public List<int> ProductosElegibles { get; set; } = new List<int>();
        /// <summary>id → nombre de cada producto elegible del slot.</summary>
        public Dictionary<int, string> NombresProductos { get; set; } = new Dictionary<int, string>();
    }

    public class ComponenteUsado
    {
        public int     IdSlot     { get; set; }
        public int     IdProducto { get; set; }
        public decimal Cantidad   { get; set; }
    }

    public class PromoAplicacion
    {
        public PromoInfo            Promo       { get; set; }
        public int                  Veces       { get; set; }
        public List<ComponenteUsado> Componentes { get; set; } = new List<ComponenteUsado>();
    }

    public class AlertaFaltante
    {
        public PromoInfo     Promo                   { get; set; }
        /// <summary>Nombres de todos los productos elegibles del slot que falta cubrir.</summary>
        public List<string>  NombresProductosPosibles { get; set; } = new List<string>();
        public decimal       CantidadFaltante         { get; set; }
    }

    public class ResultadoPromos
    {
        public List<PromoAplicacion> Aplicaciones { get; set; } = new List<PromoAplicacion>();
        public List<AlertaFaltante>  Alertas      { get; set; } = new List<AlertaFaltante>();
    }

    // ─── Motor de cálculo ────────────────────────────────────────────────────

    public static class ClassPromoEngine
    {
        /// <summary>
        /// Dado un carrito (idProducto → cantidad) y la lista de promos activas,
        /// calcula cuántas veces se puede armar cada promo (máximo posible),
        /// asigna los componentes de forma greedy (mayor disponibilidad primero)
        /// y detecta alertas de "falta 1 unidad para armar una promo más".
        /// </summary>
        public static ResultadoPromos Calcular(List<PromoInfo> promos, Dictionary<int, decimal> carrito)
        {
            var resultado = new ResultadoPromos();

            // Copia de trabajo del carrito; se va decrementando al asignar promos
            var disponible = new Dictionary<int, decimal>(carrito);

            foreach (var promo in promos)
            {
                if (promo.Slots == null || promo.Slots.Count == 0)
                    continue;

                // ── Calcular cuántas veces se puede armar esta promo ──────────
                int vecesPromo = int.MaxValue;
                foreach (var slot in promo.Slots)
                {
                    if (slot.CantidadRequerida <= 0) continue;

                    decimal dispSlot = slot.ProductosElegibles
                        .Where(p => disponible.ContainsKey(p))
                        .Sum(p => disponible[p]);

                    int maxSlot = (int)Math.Floor(dispSlot / slot.CantidadRequerida);
                    if (maxSlot < vecesPromo)
                        vecesPromo = maxSlot;
                }
                if (vecesPromo == int.MaxValue) vecesPromo = 0;

                if (vecesPromo > 0)
                {
                    // ── Asignación greedy de componentes ─────────────────────
                    var aplicacion = new PromoAplicacion { Promo = promo, Veces = vecesPromo };

                    foreach (var slot in promo.Slots)
                    {
                        decimal necesario = vecesPromo * slot.CantidadRequerida;

                        // Ordenar productos del slot por cantidad disponible desc
                        var candidatos = slot.ProductosElegibles
                            .Where(p => disponible.ContainsKey(p) && disponible[p] > 0)
                            .OrderByDescending(p => disponible[p])
                            .ToList();

                        decimal restante = necesario;
                        foreach (int idProd in candidatos)
                        {
                            if (restante <= 0) break;
                            decimal tomar = Math.Min(disponible[idProd], restante);
                            aplicacion.Componentes.Add(new ComponenteUsado
                            {
                                IdSlot     = slot.IdSlot,
                                IdProducto = idProd,
                                Cantidad   = tomar
                            });
                            disponible[idProd] -= tomar;
                            restante           -= tomar;
                        }
                    }

                    resultado.Aplicaciones.Add(aplicacion);
                }

                // ── Verificar si "falta 1" para armar una promo más ──────────
                VerificarAlertaFaltante(promo, disponible, resultado.Alertas);
            }

            return resultado;
        }

        private static void VerificarAlertaFaltante(
            PromoInfo promo,
            Dictionary<int, decimal> disponible,
            List<AlertaFaltante> alertas)
        {
            // Solo se muestra alerta cuando falta exactamente 1 unidad para completar
            // UNA promo entera: todos los slots deben estar cubiertos excepto uno,
            // y ese slot debe faltar exactamente 1 unidad.
            SlotInfo slotConFalta = null;
            int cantSlotsFaltantes = 0;

            foreach (var slot in promo.Slots)
            {
                if (slot.CantidadRequerida <= 0) continue;

                decimal dispSlot = slot.ProductosElegibles
                    .Where(p => disponible.ContainsKey(p))
                    .Sum(p => disponible[p]);

                decimal falta = slot.CantidadRequerida - dispSlot;

                if (falta > 0)
                {
                    cantSlotsFaltantes++;
                    if (falta == 1)
                        slotConFalta = slot;
                }
            }

            // Alerta solo si exactamente 1 slot está corto y le falta exactamente 1 unidad
            if (cantSlotsFaltantes == 1 && slotConFalta != null)
            {
                // Armar la lista de nombres de los productos elegibles del slot limitante
                var nombres = slotConFalta.ProductosElegibles
                    .Select(p => slotConFalta.NombresProductos.ContainsKey(p)
                        ? slotConFalta.NombresProductos[p]
                        : "Producto " + p)
                    .ToList();

                alertas.Add(new AlertaFaltante
                {
                    Promo                    = promo,
                    NombresProductosPosibles = nombres,
                    CantidadFaltante         = 1
                });
            }
        }

        // ─── Mapeo desde DataSet devuelto por sp_PromocionesTraerTodas ────────

        /// <summary>
        /// Construye la lista de PromoInfo desde el DataSet de 3 tablas
        /// devuelto por ClassPromociones.obtenerTodasPromocionesActivas().
        /// Tables[0] = cabeceras, Tables[1] = slots, Tables[2] = slot_productos.
        /// </summary>
        public static List<PromoInfo> MapearDesdeDataSet(DataSet ds)
        {
            var promos = new List<PromoInfo>();
            if (ds == null || ds.Tables.Count < 3) return promos;

            DataTable dtCab      = ds.Tables[0];
            DataTable dtSlots    = ds.Tables[1];
            DataTable dtSlotsProds = ds.Tables[2];

            foreach (DataRow row in dtCab.Rows)
            {
                var promo = new PromoInfo
                {
                    IdPromocion   = Convert.ToInt32(row["idPromocion"]),
                    IdProducto    = Convert.ToInt32(row["fk_producto"]),
                    Descripcion   = row["descripcion"].ToString(),
                    Precio        = row["precio"]       == DBNull.Value ? 0 : Convert.ToDecimal(row["precio"]),
                    PrecioSinIva  = row["precioSinIva"] == DBNull.Value ? 0 : Convert.ToDecimal(row["precioSinIva"]),
                    Iva           = row["iva"]          == DBNull.Value ? 0 : Convert.ToDecimal(row["iva"]),
                    Dolarizado    = row["dolarizado"]   != DBNull.Value && Convert.ToBoolean(row["dolarizado"]),
                    EsFraccionado = row["fraccionado"]!= DBNull.Value && Convert.ToBoolean(row["fraccionado"])
                };

                // Agregar slots de esta promo
                foreach (DataRow sRow in dtSlots.Select("fk_promocion = " + promo.IdPromocion))
                {
                    int idSlot = Convert.ToInt32(sRow["idSlot"]);
                    var slot = new SlotInfo
                    {
                        IdSlot             = idSlot,
                        CantidadRequerida  = Convert.ToDecimal(sRow["cantidadRequerida"]),
                        Descripcion        = sRow["descripcion"].ToString()
                    };

                    // Agregar productos elegibles de este slot (id y nombre)
                    foreach (DataRow pRow in dtSlotsProds.Select("fk_slot = " + idSlot))
                    {
                        int idProd = Convert.ToInt32(pRow["fk_producto"]);
                        slot.ProductosElegibles.Add(idProd);
                        string nombre = pRow.Table.Columns.Contains("productoDesc") && pRow["productoDesc"] != DBNull.Value
                            ? pRow["productoDesc"].ToString()
                            : "Producto " + idProd;
                        slot.NombresProductos[idProd] = nombre;
                    }

                    promo.Slots.Add(slot);
                }

                promos.Add(promo);
            }

            return promos;
        }
    }
}
