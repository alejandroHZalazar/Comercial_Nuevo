using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    public partial class frmCambioPrecios : Form
    {
        bool cargado = false;

        // Controles de modo (agregados programáticamente para no tocar el Designer)
        private RadioButton rbPorcentaje;
        private RadioButton rbDirecto;

        // Indica si el worker debe procesar en modo precio directo
        private bool _modoDirecto = false;

        public frmCambioPrecios()
        {
            InitializeComponent();
        }

        private void frmCambioPrecios_Load(object sender, EventArgs e)
        {
            crearControlesModo();
            cargarCombos();
            estadoInicial();
            cargado = true;
            Control.CheckForIllegalCrossThreadCalls = false;

            // Normalizar decimales al terminar de editar una celda
            dgvProductos.CellParsing  += dgvProductos_CellParsing;
        }

        // ── Radio buttons de modo ────────────────────────────────────────────
        private void crearControlesModo()
        {
            var lblModo = new Label
            {
                Text     = "Modo:",
                Location = new Point(210, 90),
                AutoSize = true,
                Font     = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            rbPorcentaje = new RadioButton
            {
                Text     = "Por porcentaje",
                Location = new Point(260, 88),
                AutoSize = true,
                Checked  = true,
                Font     = new Font("Segoe UI", 9)
            };

            rbDirecto = new RadioButton
            {
                Text     = "Precio directo",
                Location = new Point(390, 88),
                AutoSize = true,
                Font     = new Font("Segoe UI", 9)
            };

            rbPorcentaje.CheckedChanged += rbModo_CheckedChanged;
            rbDirecto.CheckedChanged    += rbModo_CheckedChanged;

            Controls.Add(lblModo);
            Controls.Add(rbPorcentaje);
            Controls.Add(rbDirecto);
        }

        private void rbModo_CheckedChanged(object sender, EventArgs e)
        {
            bool esPorcentaje = rbPorcentaje.Checked;

            // Habilitar/deshabilitar controles de porcentaje
            cboTipo.Enabled  = esPorcentaje;
            nudValor.Enabled = esPorcentaje;

            // Hacer editables las columnas M en modo directo
            aplicarEditabilidadColumnasModo();

            if (!esPorcentaje)
            {
                // Pre-cargar las columnas M con los precios actuales de base
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    fila.Cells["P_Prov_M"].Value = fila.Cells["P_Prov"].Value;
                    fila.Cells["P_SIVA_M"].Value = fila.Cells["P_SIVA"].Value;
                    fila.Cells["Costo_M"].Value  = fila.Cells["Costo"].Value;
                }
            }
            else
            {
                calcularNuevosPrecios();
            }
        }

        private void aplicarEditabilidadColumnasModo()
        {
            if (!dgvProductos.Columns.Contains("P_Prov_M")) return;
            bool soloLectura = rbPorcentaje.Checked;
            dgvProductos.Columns["P_Prov_M"].ReadOnly = soloLectura;
            dgvProductos.Columns["P_SIVA_M"].ReadOnly = soloLectura;
            dgvProductos.Columns["Costo_M"].ReadOnly  = soloLectura;
        }

        // ── Normalización de decimales (acepta '.' o ',' como separador) ─────
        private void dgvProductos_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (rbDirecto == null || !rbDirecto.Checked) return;
            string nombre = dgvProductos.Columns[e.ColumnIndex].Name;
            if (nombre != "P_Prov_M" && nombre != "P_SIVA_M" && nombre != "Costo_M") return;

            if (e.Value is string raw)
            {
                e.Value          = ParsearDecimal(raw);
                e.ParsingApplied = true;
            }
        }

        /// <summary>
        /// Parsea un decimal aceptando tanto '.' como ',' como separador decimal.
        /// </summary>
        private static decimal ParsearDecimal(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return 0;
            valor = valor.Trim();

            int puntos = valor.Count(c => c == '.');
            int comas  = valor.Count(c => c == ',');

            // "1500.50" (un solo punto, sin coma) → tratar punto como decimal
            if (puntos == 1 && comas == 0)
                valor = valor.Replace('.', ',');

            // "1.500,50" (varios puntos + una coma) → quitar puntos de miles
            if (puntos > 0 && comas == 1)
                valor = valor.Replace(".", "");

            decimal resultado;
            if (decimal.TryParse(valor, NumberStyles.Any,
                    new CultureInfo("es-AR"), out resultado))
                return resultado;

            // Fallback: cultura invariante
            valor = valor.Replace(',', '.');
            decimal.TryParse(valor, NumberStyles.Any,
                CultureInfo.InvariantCulture, out resultado);
            return resultado;
        }

        private void cargarCombos()
        {
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();


            cboProveedores.DataSource = instProv.traeProveedoresCabecera();
            cboProveedores.ValueMember = "Cod";
            cboProveedores.DisplayMember = "Proveedor";
            cboRubros.DataSource = instConfig.traeRubros();
            cboRubros.ValueMember = "id";
            cboRubros.DisplayMember = "descripcion";

            DataTable tiposMov = new DataTable();
            tiposMov.Columns.Add("id");
            tiposMov.Columns.Add("nombre");

            DataRow fila = tiposMov.NewRow();
            fila["id"] = "1";
            fila["nombre"] = "Aumento";
            tiposMov.Rows.Add(fila);

            fila = tiposMov.NewRow();
            fila["id"] = "-1";
            fila["nombre"] = "Rebaja";
            tiposMov.Rows.Add(fila);

            cboTipo.DataSource = tiposMov;
            cboTipo.ValueMember = "id";
            cboTipo.DisplayMember = "nombre";
        }

        private void estadoInicial()
        {
            cboProveedores.SelectedIndex = 0;
            cboRubros.SelectedIndex = 0;
            dgvProductos.Rows.Clear();
            nudValor.Value = 0;
        }

        private void redondearColumnas()
        {
            int cantDec = Clases.ClassProductos.cantDecimales();
            
            dgvProductos .Columns["P_Prov"].DefaultCellStyle.Format = "N" + cantDec .ToString();
            dgvProductos.Columns["P_SIVA"].DefaultCellStyle.Format = "N" + cantDec.ToString();
            dgvProductos.Columns["Costo"].DefaultCellStyle.Format = "N" + cantDec.ToString();
            dgvProductos.Columns["P_Prov_M"].DefaultCellStyle.Format = "N" + cantDec.ToString();
            dgvProductos.Columns["P_SIVA_M"].DefaultCellStyle.Format = "N" + cantDec.ToString();
            dgvProductos.Columns["Costo_M"].DefaultCellStyle.Format = "N" + cantDec.ToString();

        }

        private void calcularNuevosPrecios()
        {
            // Solo aplica en modo porcentaje
            if (cargado && (rbPorcentaje == null || rbPorcentaje.Checked))
            {
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    decimal factor = (100 + (nudValor.Value * decimal.Parse(cboTipo.SelectedValue.ToString()))) / 100;
                    fila.Cells["P_Prov_M"].Value = decimal.Parse(fila.Cells["P_Prov"].Value.ToString()) * factor;
                    fila.Cells["P_SIVA_M"].Value = decimal.Parse(fila.Cells["P_SIVA"].Value.ToString()) * factor;
                    fila.Cells["Costo_M"].Value  = decimal.Parse(fila.Cells["Costo"].Value.ToString())  * factor;
                }
                formatoGrilla();
                redondearColumnas();
            }
        }

        private void formatoGrilla()
        {
            dgvProductos.Columns["P_Prov_M"].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvProductos.Columns["P_SIVA_M"].DefaultCellStyle.BackColor = Color.LightGreen;
            dgvProductos.Columns["Costo_M"].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            Clases.ClassProductos instProd = new Clases.ClassProductos();

            string unFiltro = string.Empty;

            if (cbProveedor.Checked)
                unFiltro = " where p.fk_proveedor = " + cboProveedores.SelectedValue.ToString();

            if (cbRubros.Checked)
                unFiltro = (unFiltro == string.Empty
                    ? " where p.fk_Rubro = " + cboRubros.SelectedValue.ToString()
                    : unFiltro + " and p.fk_Rubro = " + cboRubros.SelectedValue.ToString());

            dgvProductos.DataSource = instProd.traeParaCambiarPrecio(unFiltro);

            // Aplicar editabilidad de columnas según el modo activo
            aplicarEditabilidadColumnasModo();

            if (rbDirecto != null && rbDirecto.Checked)
            {
                // Pre-cargar las columnas M con los precios actuales de base
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    fila.Cells["P_Prov_M"].Value = fila.Cells["P_Prov"].Value;
                    fila.Cells["P_SIVA_M"].Value = fila.Cells["P_SIVA"].Value;
                    fila.Cells["Costo_M"].Value  = fila.Cells["Costo"].Value;
                }
            }
            else
            {
                calcularNuevosPrecios();
            }
        }

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            calcularNuevosPrecios();
        }

        private void nudValor_ValueChanged(object sender, EventArgs e)
        {
            calcularNuevosPrecios();
        }

        private void btnCambioPrecios_Click(object sender, EventArgs e)
        {
            // Capturar el modo antes de pasar al hilo de fondo
            _modoDirecto = rbDirecto != null && rbDirecto.Checked;
            backgroundTarea.RunWorkerAsync();
        }

        private void backgroundTarea_DoWork(object sender, DoWorkEventArgs e)
        {
            dgvProductos.Enabled = false;
            rtbObserv.Text = string.Empty;
            rtbObserv.Text = "[" + DateTime.Now + "] Proceso Iniciado";
            Clases.ClassProductos instProd = new Clases.ClassProductos();

            if (_modoDirecto)
            {
                // ── Modo precio directo ──────────────────────────────────────
                int procesados = 0, omitidos = 0;

                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    decimal pProv = ParsearDecimal(fila.Cells["P_Prov_M"].Value?.ToString() ?? "0");
                    decimal pSIVA = ParsearDecimal(fila.Cells["P_SIVA_M"].Value?.ToString() ?? "0");
                    decimal costo = ParsearDecimal(fila.Cells["Costo_M"].Value?.ToString() ?? "0");

                    // Ignorar filas donde ninguna columna tiene valor > 0
                    if (pProv <= 0 && pSIVA <= 0 && costo <= 0)
                    {
                        omitidos++;
                        continue;
                    }

                    try
                    {
                        instProd.actualizarPreciosDirectos(
                            fila.Cells["Cod_Prov"].Value.ToString(),
                            pProv, pSIVA, costo);
                        rtbObserv.Text += System.Environment.NewLine +
                            "[" + DateTime.Now + "] Procesado Prod: " + fila.Cells["Cod_Prov"].Value;
                        procesados++;
                    }
                    catch
                    {
                        rtbObserv.Text += System.Environment.NewLine +
                            "[" + DateTime.Now + "] Error Prod: " + fila.Cells["Cod_Prov"].Value;
                    }
                }

                rtbObserv.Text += System.Environment.NewLine +
                    "[" + DateTime.Now + "] Finalizado. Procesados: " + procesados +
                    " | Omitidos (sin precio): " + omitidos;
            }
            else
            {
                // ── Modo porcentaje (lógica original) ────────────────────────
                foreach (DataGridViewRow fila in dgvProductos.Rows)
                {
                    try
                    {
                        instProd.actualziarMasivaPrecios(
                            fila.Cells["Cod_Prov"].Value.ToString(),
                            decimal.Parse(fila.Cells["P_Prov_M"].Value.ToString()),
                            "", "", 0);
                        rtbObserv.Text += System.Environment.NewLine +
                            "[" + DateTime.Now + "] Procesado Prod: " + fila.Cells["Cod_Prov"].Value;
                    }
                    catch
                    {
                        rtbObserv.Text += System.Environment.NewLine +
                            "[" + DateTime.Now + "] Error Prod: " + fila.Cells["Cod_Prov"].Value;
                    }
                }

                rtbObserv.Text += System.Environment.NewLine +
                    "[" + DateTime.Now + "] Procesado Finalizado";
            }

            MessageBox.Show(this, "Cambio de Precio Finalizado. Verifique en el cuadro de resultados los errores.",
                "Cambio de Precios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            dgvProductos.Enabled = true;
        }
    }
}
