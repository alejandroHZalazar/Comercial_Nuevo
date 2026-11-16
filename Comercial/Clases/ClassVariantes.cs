using MySqlConnector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Comercial.Clases
{
    /// <summary>
    /// Productos con variantes (medidas y/o colores), mismo modelo que LitoralMarket / Comercial Web:
    /// cada variante ES una fila de Productos (con su propio stock, costo y precio) enlazada a una ficha principal
    /// (productosPrincipales / productosVariantes). Las medidas y los colores salen de los maestros
    /// variantesMedidas / variantesColores.
    /// </summary>
    /// <summary>Iconos de los botones de los formularios de variantes (recursos del proyecto, escalados a 20x20).</summary>
    static class IconosVariantes
    {
        public static void Aplicar(System.Windows.Forms.Button boton, System.Drawing.Icon icono)
        {
            Aplicar(boton, (System.Drawing.Image)icono.ToBitmap());
        }

        public static void Aplicar(System.Windows.Forms.Button boton, System.Drawing.Image icono)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(20, 20);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(icono, 0, 0, 20, 20);
            }
            boton.Image = bmp;
            boton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            boton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            boton.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
        }
    }

    class ClassVariantes
    {
        public const int MaxNombreAtributo = 40;
        public const int MaxDescripcion = 150;   // Productos.descripcion es VARCHAR(150)
        public const int MaxCodigo = 45;         // Productos.codBarras / codProveedor son VARCHAR(45)

        // Cómo propagar un cambio del costo/precio común a las variantes existentes
        public const int PropagarNinguna = 0;
        public const int PropagarSoloQuienesSiguen = 1;
        public const int PropagarTodas = 2;

        classDatos instDatos = new classDatos();

        // ── Modelos ───────────────────────────────────────────────────────────
        public class VarianteFila
        {
            public int? ProductoId { get; set; }        // null = variante nueva
            public int MedidaId { get; set; }
            public int ColorId { get; set; }
            public string MedidaNombre { get; set; }
            public string ColorNombre { get; set; }
            public bool Habilitada { get; set; } = true;
            public string CodBarras { get; set; }
            public string CodProveedor { get; set; }
            public decimal Costo { get; set; }
            public decimal Precio { get; set; }
            public decimal Stock { get; set; }          // stock inicial (solo variantes nuevas)
            public int Orden { get; set; }
            // Valores cargados de la base (para decidir qué variantes "siguen" al valor común)
            public decimal CostoOriginal { get; set; }
            public decimal PrecioOriginal { get; set; }
        }

        public class FichaVariantes
        {
            public bool EsFicha { get; set; }
            public bool UsaMedidas { get; set; }
            public bool UsaColores { get; set; }
            public bool PreciosIguales { get; set; } = true;
            public List<VarianteFila> Variantes { get; set; } = new List<VarianteFila>();
        }

        /// <summary>Datos de la ficha tomados del formulario de productos.</summary>
        public class FichaDatos
        {
            public int ProductoId { get; set; }          // 0 = ficha nueva
            public string CodProveedor { get; set; }
            public string CodBarras { get; set; }
            public int RubroId { get; set; }
            public string Descripcion { get; set; }
            public int ProveedorId { get; set; }
            public decimal Costo { get; set; }
            public decimal Precio { get; set; }
            public decimal CantidadMinima { get; set; }
            public decimal PrecioProveedor { get; set; }
            public bool Fraccionado { get; set; }
            public bool Dolarizado { get; set; }
            public bool EsPromocion { get; set; }
            public decimal? Ganancia { get; set; }
            public decimal? Descuento { get; set; }
            public decimal CantidadMinimaVenta { get; set; }
        }

        public class ResultadoGuardado
        {
            public bool Ok { get; set; }
            public int ProductoId { get; set; }
            public List<string> Errores { get; set; } = new List<string>();
        }

        // ── Estado ────────────────────────────────────────────────────────────
        /// <summary>true con el parámetro en '1' y las 4 tablas de variantes existentes.</summary>
        public static bool habilitado()
        {
            if (!ClassProductos.variantesHabilitadas()) return false;

            classDatos datos = new classDatos();
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "select count(*) from information_schema.TABLES where TABLE_SCHEMA = DATABASE() " +
                    "and TABLE_NAME in ('variantesMedidas','variantesColores','productosPrincipales','productosVariantes')", datos.abrirConexion());
                return Convert.ToInt32(cmd.ExecuteScalar()) == 4;
            }
            catch
            {
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // ── Maestros: medidas y colores ───────────────────────────────────────
        public DataTable traeMedidas(bool soloActivas)
        {
            string sql = "select m.id, m.nombre, m.orden, m.activa, " +
                         "(select count(*) from productosVariantes v where v.fk_medida = m.id) as enUso " +
                         "from variantesMedidas m " + (soloActivas ? "where m.activa = 1 " : "") + "order by m.orden, m.nombre";
            return consulta(sql);
        }

        public DataTable traeColores(bool soloActivas)
        {
            string sql = "select c.id, c.nombre, c.codigoHex, c.orden, c.activa, " +
                         "(select count(*) from productosVariantes v where v.fk_color = c.id) as enUso " +
                         "from variantesColores c " + (soloActivas ? "where c.activa = 1 " : "") + "order by c.orden, c.nombre";
            return consulta(sql);
        }

        private DataTable consulta(string sql, params MySqlParameter[] parametros)
        {
            try
            {
                MySqlDataAdapter a = new MySqlDataAdapter(sql, instDatos.abrirConexion());
                if (parametros != null) a.SelectCommand.Parameters.AddRange(parametros);
                DataTable t = new DataTable();
                a.Fill(t);
                return t;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public static bool esLatin1(string s)
        {
            return s.All(c => c <= 'ÿ');
        }

        private static string validarNombreAtributo(ref string nombre, string que)
        {
            nombre = (nombre ?? string.Empty).Trim();
            if (nombre.Length == 0) return "Escriba el nombre de la " + que + ".";
            if (nombre.Length > MaxNombreAtributo) return "El nombre admite hasta " + MaxNombreAtributo + " caracteres.";
            // El nombre se escribe en Productos.descripcion (latin1): sin emojis ni caracteres fuera de ese juego.
            if (!esLatin1(nombre)) return "El nombre solo admite letras, números y símbolos comunes (sin emojis).";
            return null;
        }

        /// <summary>Alta (id = 0) o modificación de una medida. Devuelve null si salió bien, o el mensaje de error.</summary>
        public string guardarMedida(int id, string nombre, int orden, bool activa, out int idGuardado)
        {
            idGuardado = id;
            string error = validarNombreAtributo(ref nombre, "medida");
            if (error != null) return error;
            return guardarAtributo(true, id, nombre, null, orden, activa, out idGuardado);
        }

        /// <summary>Alta (id = 0) o modificación de un color (codigoHex opcional, formato #RRGGBB).</summary>
        public string guardarColor(int id, string nombre, string codigoHex, int orden, bool activa, out int idGuardado)
        {
            idGuardado = id;
            string error = validarNombreAtributo(ref nombre, "color");
            if (error != null) return error;

            codigoHex = string.IsNullOrWhiteSpace(codigoHex) ? null : codigoHex.Trim();
            if (codigoHex != null && !Regex.IsMatch(codigoHex, "^#[0-9A-Fa-f]{6}$"))
                return "El color debe tener el formato #RRGGBB (por ejemplo #1F2937).";

            return guardarAtributo(false, id, nombre, codigoHex, orden, activa, out idGuardado);
        }

        private string guardarAtributo(bool esMedida, int id, string nombre, string hex, int orden, bool activa, out int idGuardado)
        {
            idGuardado = id;
            string tabla = esMedida ? "variantesMedidas" : "variantesColores";
            string campoFk = esMedida ? "fk_medida" : "fk_color";
            string que = esMedida ? "medida" : "color";

            MySqlConnection cn = instDatos.abrirConexion();
            MySqlTransaction tx = null;
            try
            {
                MySqlCommand dup = new MySqlCommand("select count(*) from " + tabla + " where id <> @id and lower(nombre) = lower(@nombre)", cn);
                dup.Parameters.AddWithValue("@id", id);
                dup.Parameters.AddWithValue("@nombre", nombre);
                if (Convert.ToInt32(dup.ExecuteScalar()) > 0)
                    return "Ya existe " + (esMedida ? "una medida" : "un color") + " con ese nombre.";

                tx = cn.BeginTransaction();
                bool renombrado = false;

                if (id > 0)
                {
                    MySqlCommand actual = new MySqlCommand("select nombre from " + tabla + " where id = @id", cn, tx);
                    actual.Parameters.AddWithValue("@id", id);
                    object nombreActual = actual.ExecuteScalar();
                    if (nombreActual == null || nombreActual == DBNull.Value)
                    {
                        tx.Rollback();
                        return "La " + que + " no existe.";
                    }
                    renombrado = nombreActual.ToString() != nombre;

                    string sqlUpd = esMedida
                        ? "update variantesMedidas set nombre = @nombre, orden = @orden, activa = @activa where id = @id"
                        : "update variantesColores set nombre = @nombre, codigoHex = @hex, orden = @orden, activa = @activa where id = @id";
                    MySqlCommand upd = new MySqlCommand(sqlUpd, cn, tx);
                    upd.Parameters.AddWithValue("@nombre", nombre);
                    if (!esMedida) upd.Parameters.AddWithValue("@hex", hex == null ? (object)DBNull.Value : hex);
                    upd.Parameters.AddWithValue("@orden", orden);
                    upd.Parameters.AddWithValue("@activa", activa ? 1 : 0);
                    upd.Parameters.AddWithValue("@id", id);
                    upd.ExecuteNonQuery();
                }
                else
                {
                    string sqlIns = esMedida
                        ? "insert into variantesMedidas (nombre, orden, activa) values (@nombre, @orden, @activa)"
                        : "insert into variantesColores (nombre, codigoHex, orden, activa) values (@nombre, @hex, @orden, @activa)";
                    MySqlCommand ins = new MySqlCommand(sqlIns, cn, tx);
                    ins.Parameters.AddWithValue("@nombre", nombre);
                    if (!esMedida) ins.Parameters.AddWithValue("@hex", hex == null ? (object)DBNull.Value : hex);
                    ins.Parameters.AddWithValue("@orden", orden);
                    ins.Parameters.AddWithValue("@activa", activa ? 1 : 0);
                    ins.ExecuteNonQuery();
                    idGuardado = (int)ins.LastInsertedId;
                }

                // Renombrar una medida/color regenera la descripción de las variantes que la usan
                if (renombrado)
                    regenerarDescripcionesDeAtributo(cn, tx, campoFk, idGuardado);

                tx.Commit();
                return null;
            }
            catch (Exception ex)
            {
                try { if (tx != null) tx.Rollback(); } catch { }
                return "No se pudo guardar: " + ex.Message;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        private static void regenerarDescripcionesDeAtributo(MySqlConnection cn, MySqlTransaction tx, string campoFk, int atributoId)
        {
            // Todas las fichas que usan el atributo
            List<int> fichas = new List<int>();
            MySqlCommand cf = new MySqlCommand("select distinct fk_principal from productosVariantes where " + campoFk + " = @id", cn, tx);
            cf.Parameters.AddWithValue("@id", atributoId);
            using (MySqlDataReader rd = cf.ExecuteReader())
                while (rd.Read()) fichas.Add(Convert.ToInt32(rd[0]));

            foreach (int ficha in fichas)
            {
                bool usaMed = false, usaCol = false;
                string nombreBase = null;
                MySqlCommand cp = new MySqlCommand(
                    "select pp.usaMedidas, pp.usaColores, p.descripcion from productosPrincipales pp join Productos p on p.id = pp.fk_producto where pp.fk_producto = @f", cn, tx);
                cp.Parameters.AddWithValue("@f", ficha);
                using (MySqlDataReader rd = cp.ExecuteReader())
                    if (rd.Read())
                    {
                        usaMed = Convert.ToInt32(rd[0]) == 1;
                        usaCol = Convert.ToInt32(rd[1]) == 1;
                        nombreBase = rd[2] == DBNull.Value ? null : rd[2].ToString();
                    }
                if (string.IsNullOrWhiteSpace(nombreBase)) continue;

                List<Tuple<int, string, string>> variantes = new List<Tuple<int, string, string>>();
                MySqlCommand cv = new MySqlCommand(
                    "select v.fk_producto, m.nombre, c.nombre from productosVariantes v " +
                    "left join variantesMedidas m on m.id = v.fk_medida left join variantesColores c on c.id = v.fk_color " +
                    "where v.fk_principal = @f", cn, tx);
                cv.Parameters.AddWithValue("@f", ficha);
                using (MySqlDataReader rd = cv.ExecuteReader())
                    while (rd.Read())
                        variantes.Add(Tuple.Create(Convert.ToInt32(rd[0]),
                                                   rd[1] == DBNull.Value ? null : rd[1].ToString(),
                                                   rd[2] == DBNull.Value ? null : rd[2].ToString()));

                foreach (var v in variantes)
                {
                    string desc = generarDescripcion(nombreBase, usaMed, usaCol, v.Item2, v.Item3);
                    if (desc.Length > MaxDescripcion || !esLatin1(desc)) continue;
                    MySqlCommand up = new MySqlCommand("update Productos set descripcion = @d where id = @id", cn, tx);
                    up.Parameters.AddWithValue("@d", desc);
                    up.Parameters.AddWithValue("@id", v.Item1);
                    up.ExecuteNonQuery();
                }
            }
        }

        // ── Fichas ────────────────────────────────────────────────────────────
        /// <summary>"NOMBRE BASE - MEDIDA - COLOR" (solo las dimensiones que usa la ficha).</summary>
        public static string generarDescripcion(string nombreBase, bool usaMedidas, bool usaColores, string medida, string color)
        {
            List<string> partes = new List<string> { (nombreBase ?? string.Empty).Trim() };
            if (usaMedidas && !string.IsNullOrWhiteSpace(medida)) partes.Add(medida.Trim());
            if (usaColores && !string.IsNullOrWhiteSpace(color)) partes.Add(color.Trim());
            return string.Join(" - ", partes);
        }

        public static string etiqueta(string medida, string color)
        {
            List<string> p = new List<string>();
            if (!string.IsNullOrWhiteSpace(medida)) p.Add(medida);
            if (!string.IsNullOrWhiteSpace(color)) p.Add(color);
            return p.Count == 0 ? "(sin atributos)" : string.Join(" / ", p);
        }

        /// <summary>Un código de barras "real": ni vacío ni solo ceros.</summary>
        public static bool tieneCodigoDeBarras(string codigo)
        {
            return !string.IsNullOrWhiteSpace(codigo) && codigo.Any(c => c != '0');
        }

        /// <summary>¿El producto es una ficha principal? Con variantes apagadas siempre false.</summary>
        public static bool esFicha(int productoId)
        {
            return productoId > 0 && ClassProductos.fichasPrincipales(new[] { productoId }).Contains(productoId);
        }

        /// <summary>Carga la ficha con todas sus variantes (habilitadas o no). EsFicha = false si no es una ficha.</summary>
        public FichaVariantes obtenerFicha(int productoId)
        {
            FichaVariantes ficha = new FichaVariantes();
            DataTable cab = consulta("select usaMedidas, usaColores, preciosIguales from productosPrincipales where fk_producto = @id",
                                     new MySqlParameter("@id", productoId));
            if (cab.Rows.Count == 0) return ficha;

            ficha.EsFicha = true;
            ficha.UsaMedidas = Convert.ToInt32(cab.Rows[0]["usaMedidas"]) == 1;
            ficha.UsaColores = Convert.ToInt32(cab.Rows[0]["usaColores"]) == 1;
            ficha.PreciosIguales = Convert.ToInt32(cab.Rows[0]["preciosIguales"]) == 1;

            DataTable v = consulta(
                "select v.fk_producto, v.fk_medida, v.fk_color, v.activa, v.orden, p.codBarras, p.codProveedor, " +
                "m.nombre as medida, c.nombre as color, s.cantidad, co.costo, pr.precio " +
                "from productosVariantes v " +
                "join Productos p on p.id = v.fk_producto " +
                "left join variantesMedidas m on m.id = v.fk_medida " +
                "left join variantesColores c on c.id = v.fk_color " +
                "left join stockProductos s on s.fk_producto = v.fk_producto " +
                "left join costosProductos co on co.fk_producto = v.fk_producto " +
                "left join preciosProductos pr on pr.fk_producto = v.fk_producto " +
                "where v.fk_principal = @id order by v.orden, v.fk_producto",
                new MySqlParameter("@id", productoId));

            foreach (DataRow r in v.Rows)
            {
                decimal costo = r["costo"] == DBNull.Value ? 0m : Convert.ToDecimal(r["costo"]);
                decimal precio = r["precio"] == DBNull.Value ? 0m : Convert.ToDecimal(r["precio"]);
                ficha.Variantes.Add(new VarianteFila
                {
                    ProductoId = Convert.ToInt32(r["fk_producto"]),
                    MedidaId = Convert.ToInt32(r["fk_medida"]),
                    ColorId = Convert.ToInt32(r["fk_color"]),
                    MedidaNombre = r["medida"] == DBNull.Value ? null : r["medida"].ToString(),
                    ColorNombre = r["color"] == DBNull.Value ? null : r["color"].ToString(),
                    Habilitada = Convert.ToInt32(r["activa"]) == 1,
                    CodBarras = r["codBarras"] == DBNull.Value ? null : r["codBarras"].ToString(),
                    CodProveedor = r["codProveedor"] == DBNull.Value ? null : r["codProveedor"].ToString(),
                    Costo = costo,
                    Precio = precio,
                    CostoOriginal = costo,
                    PrecioOriginal = precio,
                    Stock = r["cantidad"] == DBNull.Value ? 0m : Convert.ToDecimal(r["cantidad"]),
                    Orden = Convert.ToInt32(r["orden"])
                });
            }
            return ficha;
        }

        private static string limpiar(string s)
        {
            s = (s ?? string.Empty).Trim();
            return s.Length == 0 ? null : s;
        }

        /// <summary>
        /// Valida y guarda la ficha con sus variantes (todo o nada, vía sp_ProductosVariantesGuardar).
        /// Mismas reglas que Comercial Web. Si hay errores no se escribe nada y vienen en Errores.
        /// </summary>
        public ResultadoGuardado guardarFicha(FichaDatos p, bool usaMedidas, bool usaColores, bool preciosIguales,
                                              int modoPropagacion, List<VarianteFila> filas)
        {
            ResultadoGuardado res = new ResultadoGuardado();
            List<string> errores = res.Errores;
            string nombreBase = (p.Descripcion ?? string.Empty).Trim();

            if (nombreBase.Length == 0) errores.Add("Escriba la descripción del producto.");
            if (p.RubroId <= 0) errores.Add("Seleccione el rubro.");
            if (p.ProveedorId <= 0) errores.Add("Seleccione el proveedor.");
            if (!usaMedidas && !usaColores) errores.Add("Elija si el producto varía por medidas, colores o ambos.");
            if (filas == null || filas.Count == 0) errores.Add("Agregue al menos una variante.");
            if (errores.Count > 0) return res;

            // Estado actual de la ficha (si ya existe)
            FichaVariantes actual = p.ProductoId > 0 ? obtenerFicha(p.ProductoId) : new FichaVariantes();
            if (p.ProductoId > 0 && !actual.EsFicha)
                errores.Add("Este producto ya existe como producto simple. Para usar variantes, cree un producto nuevo.");
            if (actual.EsFicha && actual.Variantes.Count > 0 && (actual.UsaMedidas != usaMedidas || actual.UsaColores != usaColores))
                errores.Add("No se pueden cambiar las dimensiones (medidas/colores) de un producto que ya tiene variantes.");

            Dictionary<int, VarianteFila> existentes = actual.Variantes.ToDictionary(v => v.ProductoId.Value);
            HashSet<string> combosExistentes = new HashSet<string>(actual.Variantes.Select(v => v.MedidaId + "|" + v.ColorId));

            // Maestros
            Dictionary<int, DataRow> medidas = traeMedidas(false).AsEnumerable().ToDictionary(r => Convert.ToInt32(r["id"]));
            Dictionary<int, DataRow> colores = traeColores(false).AsEnumerable().ToDictionary(r => Convert.ToInt32(r["id"]));

            HashSet<string> combosFilas = new HashSet<string>();
            Dictionary<VarianteFila, string> descripciones = new Dictionary<VarianteFila, string>();

            foreach (VarianteFila f in filas)
            {
                if (!usaMedidas) f.MedidaId = 0;
                else if (f.MedidaId <= 0) { errores.Add("Hay una fila sin medida."); continue; }
                if (!usaColores) f.ColorId = 0;
                else if (f.ColorId <= 0) { errores.Add("Hay una fila sin color."); continue; }

                DataRow medida = null, color = null;
                medidas.TryGetValue(f.MedidaId, out medida);
                colores.TryGetValue(f.ColorId, out color);
                string nomM = medida == null ? null : medida["nombre"].ToString();
                string nomC = color == null ? null : color["nombre"].ToString();
                string et = etiqueta(nomM, nomC);

                if (usaMedidas && medida == null) { errores.Add("La medida de la fila '" + et + "' no existe."); continue; }
                if (usaColores && color == null) { errores.Add("El color de la fila '" + et + "' no existe."); continue; }

                if (!combosFilas.Add(f.MedidaId + "|" + f.ColorId))
                    errores.Add("La combinación " + et + " está repetida.");

                if (f.ProductoId.HasValue)
                {
                    VarianteFila ex;
                    if (!existentes.TryGetValue(f.ProductoId.Value, out ex))
                        errores.Add("La variante " + et + " no pertenece a este producto.");
                    else if (ex.MedidaId != f.MedidaId || ex.ColorId != f.ColorId)
                        errores.Add("No se puede cambiar la medida o el color de la variante existente " + et + ".");
                }
                else
                {
                    if (combosExistentes.Contains(f.MedidaId + "|" + f.ColorId))
                        errores.Add("La combinación " + et + " ya existe en este producto.");
                    if (f.Habilitada && medida != null && Convert.ToInt32(medida["activa"]) == 0) errores.Add("La medida " + nomM + " está deshabilitada.");
                    if (f.Habilitada && color != null && Convert.ToInt32(color["activa"]) == 0) errores.Add("El color " + nomC + " está deshabilitado.");
                }

                string desc = generarDescripcion(nombreBase, usaMedidas, usaColores, nomM, nomC);
                if (desc.Length > MaxDescripcion)
                    errores.Add("La descripción '" + desc + "' supera los " + MaxDescripcion + " caracteres: acorte el nombre del producto, la medida o el color.");
                else if (!esLatin1(desc))
                    errores.Add("La descripción '" + desc + "' tiene caracteres que la base no admite (por ejemplo emojis).");
                descripciones[f] = desc;

                f.CodBarras = limpiar(f.CodBarras);
                f.CodProveedor = limpiar(f.CodProveedor);
                if (f.CodBarras != null && f.CodBarras.Length > MaxCodigo) errores.Add("El código de barras de " + et + " admite hasta " + MaxCodigo + " caracteres.");
                if (!f.ProductoId.HasValue && f.CodBarras != null && !f.CodBarras.All(char.IsDigit)) errores.Add("El código de barras de " + et + " debe contener solo números.");
                if (f.CodProveedor != null && f.CodProveedor.Length > MaxCodigo) errores.Add("El código de proveedor de " + et + " admite hasta " + MaxCodigo + " caracteres.");

                // Stock inicial: entero, solo variantes nuevas
                if (f.Stock < 0 || f.Stock != decimal.Truncate(f.Stock))
                    if (!f.ProductoId.HasValue)
                        errores.Add("El stock inicial de " + et + " debe ser un número entero mayor o igual a cero.");
                if (f.ProductoId.HasValue) f.Stock = 0;

                if (f.Costo < 0 || f.Precio < 0)
                    errores.Add("El costo y el precio de " + et + " no pueden ser negativos.");
                if (!f.ProductoId.HasValue && f.Habilitada && f.Precio <= 0)
                    errores.Add("Falta el precio de venta de " + et + ".");
            }

            // Códigos repetidos: dentro de la grilla y contra el resto de los productos
            foreach (var g in filas.Where(f => tieneCodigoDeBarras(f.CodBarras)).GroupBy(f => f.CodBarras.ToLowerInvariant()).Where(g => g.Count() > 1))
                errores.Add("El código de barras " + g.First().CodBarras + " está repetido en la grilla.");
            foreach (var g in filas.Where(f => f.CodProveedor != null).GroupBy(f => f.CodProveedor.ToLowerInvariant()).Where(g => g.Count() > 1))
                errores.Add("El código de proveedor " + g.First().CodProveedor + " está repetido en la grilla.");

            List<int> idsPropios = new List<int>(existentes.Keys);
            if (p.ProductoId > 0) idsPropios.Add(p.ProductoId);

            chequearCodigos(filas.Where(f => tieneCodigoDeBarras(f.CodBarras)).Select(f => f.CodBarras).Distinct().ToList(),
                            "codBarras", null, idsPropios, errores, "El código de barras {0} ya lo usa el producto '{1}'.");
            chequearCodigos(filas.Where(f => f.CodProveedor != null).Select(f => f.CodProveedor).Distinct().ToList(),
                            "codProveedor", p.ProveedorId, idsPropios, errores, "El código de proveedor {0} ya lo usa el producto '{1}' (mismo proveedor).");

            // Precios
            if (preciosIguales)
            {
                if (p.Precio <= 0) errores.Add("Cargue el precio de venta común (mayor a cero).");
                if (p.Costo < 0) errores.Add("El costo común no puede ser negativo.");
            }

            if (errores.Count > 0) { res.Errores = errores.Distinct().ToList(); return res; }

            // Valor común anterior (para decidir qué variantes "siguen" al cambiar el común)
            decimal? viejoCosto = null, viejoPrecio = null;
            if (actual.EsFicha)
            {
                DataTable c = consulta("select (select costo from costosProductos where fk_producto = @id limit 1) as costo, " +
                                       "(select precio from preciosProductos where fk_producto = @id limit 1) as precio",
                                       new MySqlParameter("@id", p.ProductoId));
                if (c.Rows.Count > 0)
                {
                    if (c.Rows[0]["costo"] != DBNull.Value) viejoCosto = Convert.ToDecimal(c.Rows[0]["costo"]);
                    if (c.Rows[0]["precio"] != DBNull.Value) viejoPrecio = Convert.ToDecimal(c.Rows[0]["precio"]);
                }
            }

            // Costo/precio final de cada variante existente: valor editado en la grilla, o regla de propagación del común
            if (preciosIguales)
            {
                foreach (VarianteFila f in filas.Where(f => f.ProductoId.HasValue))
                {
                    bool costoEditado = f.Costo != f.CostoOriginal;
                    bool precioEditado = f.Precio != f.PrecioOriginal;
                    if (!costoEditado && debePropagar(modoPropagacion, viejoCosto, f.CostoOriginal)) f.Costo = p.Costo;
                    if (!precioEditado && debePropagar(modoPropagacion, viejoPrecio, f.PrecioOriginal)) f.Precio = p.Precio;
                }
            }

            // Sin precios iguales, la ficha guarda como referencia el menor costo/precio de las variantes habilitadas
            if (!preciosIguales)
            {
                var habilitadas = filas.Where(f => f.Habilitada).ToList();
                p.Precio = habilitadas.Count > 0 ? habilitadas.Min(f => f.Precio) : 0m;
                p.Costo = habilitadas.Count > 0 ? habilitadas.Min(f => f.Costo) : 0m;
            }

            JArray json = new JArray();
            int orden = 0;
            foreach (VarianteFila f in filas)
            {
                orden++;
                json.Add(new JObject
                {
                    { "productoId", f.ProductoId ?? 0 },
                    { "medidaId", f.MedidaId },
                    { "colorId", f.ColorId },
                    { "habilitada", f.Habilitada ? 1 : 0 },
                    { "codBarras", f.CodBarras },
                    { "codProveedor", f.CodProveedor },
                    { "descripcion", descripciones[f] },
                    { "costo", f.Costo },
                    { "precio", f.Precio },
                    { "stock", f.Stock },
                    { "orden", f.Orden > 0 ? f.Orden : orden }
                });
            }

            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_ProductosVariantesGuardar", instDatos.abrirConexion());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("unProducto", p.ProductoId);
                cmd.Parameters.AddWithValue("unCodProveedor", p.CodProveedor ?? string.Empty);
                cmd.Parameters.AddWithValue("unCodBarras", p.CodBarras ?? string.Empty);
                cmd.Parameters.AddWithValue("unRubro", p.RubroId);
                cmd.Parameters.AddWithValue("unaDescripcion", nombreBase);
                cmd.Parameters.AddWithValue("unProveedor", p.ProveedorId);
                cmd.Parameters.AddWithValue("unCosto", p.Costo);
                cmd.Parameters.AddWithValue("unPrecio", p.Precio);
                cmd.Parameters.AddWithValue("unaCMinima", p.CantidadMinima);
                cmd.Parameters.AddWithValue("unPrecioProveedor", p.PrecioProveedor);
                cmd.Parameters.AddWithValue("esFraccionado", p.Fraccionado);
                cmd.Parameters.AddWithValue("esDolarizado", p.Dolarizado);
                cmd.Parameters.AddWithValue("esUnaPromocion", p.EsPromocion);
                cmd.Parameters.AddWithValue("unGanancia", p.Ganancia.HasValue ? (object)p.Ganancia.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("unDescuento", p.Descuento.HasValue ? (object)p.Descuento.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("unaCantidadMinimaVenta", p.CantidadMinimaVenta);
                cmd.Parameters.AddWithValue("usaMed", usaMedidas);
                cmd.Parameters.AddWithValue("usaCol", usaColores);
                cmd.Parameters.AddWithValue("preciosIg", preciosIguales);
                cmd.Parameters.AddWithValue("unasVariantes", json.ToString(Newtonsoft.Json.Formatting.None));

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32) { Direction = ParameterDirection.Output };
                MySqlParameter mensaje = new MySqlParameter("mensaje", MySqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(salida);
                cmd.Parameters.Add(mensaje);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();

                int codigo = Convert.ToInt32(salida.Value);
                if (codigo > 0)
                {
                    res.Ok = true;
                    res.ProductoId = codigo;
                }
                else
                {
                    string m = mensaje.Value == null || mensaje.Value == DBNull.Value ? string.Empty : mensaje.Value.ToString();
                    res.Errores.Add(m.Length > 0 ? m : "Ha ocurrido un error durante el procedimiento (código " + codigo + ").");
                }
            }
            catch (Exception ex)
            {
                res.Errores.Add("No se pudo guardar: " + ex.Message);
            }
            finally
            {
                instDatos.cerrarConexion();
            }
            return res;
        }

        // Modo "todas": sí. "Solo quienes siguen": solo si su valor actual es igual al común anterior. "Ninguna": no.
        private static bool debePropagar(int modo, decimal? comunAnterior, decimal valorActual)
        {
            switch (modo)
            {
                case PropagarTodas: return true;
                case PropagarSoloQuienesSiguen: return comunAnterior.HasValue && valorActual == comunAnterior.Value;
                default: return false;
            }
        }

        // Busca productos activos (que no sean de la propia ficha) que ya usen alguno de los códigos
        private void chequearCodigos(List<string> codigos, string columna, int? proveedor, List<int> idsPropios,
                                     List<string> errores, string formato)
        {
            if (codigos.Count == 0) return;

            List<MySqlParameter> ps = new List<MySqlParameter>();
            List<string> nombres = new List<string>();
            for (int i = 0; i < codigos.Count; i++)
            {
                ps.Add(new MySqlParameter("@c" + i, codigos[i]));
                nombres.Add("@c" + i);
            }

            string sql = "select " + columna + ", descripcion from Productos where " + columna + " in (" + string.Join(",", nombres) + ") " +
                         "and IFNULL(baja,0) = 0";
            if (proveedor.HasValue)
            {
                sql += " and fk_proveedor = @prov";
                ps.Add(new MySqlParameter("@prov", proveedor.Value));
            }
            if (idsPropios.Count > 0)
                sql += " and id not in (" + string.Join(",", idsPropios) + ")";
            sql += " limit 10";

            DataTable t = consulta(sql, ps.ToArray());
            foreach (DataRow r in t.Rows)
                errores.Add(string.Format(formato, r[0], r[1]));
        }
    }
}
