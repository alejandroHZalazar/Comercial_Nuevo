using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Comercial.Formularios.Ventas
{
    public partial class frmImputacionVenta : Form
    {
        // ── Estado ─────────────────────────────────────────────────────────
        private readonly decimal _total;
        private readonly int     _defaultPlanId;
        private readonly bool    _llevaCC;

        private DataTable _planes;          // cols: Nro, Nombre, Recargo
        private int?      _planSelId;
        private string    _planSelNombre  = string.Empty;
        private decimal   _planSelRecargo;

        // Resultado público (consumido por frmVentas)
        // idPlan == 0 identifica el ítem "Cuenta Corriente"
        public BindingList<Clases.ClassVentas.CobroFormasPago> unDT =
            new BindingList<Clases.ClassVentas.CobroFormasPago>();

        // ── Controles programáticos ─────────────────────────────────────────
        private Label           _lblTotalValor;
        private FlowLayoutPanel _pnlPlanes;
        private Label           _lblPlanSel;
        private TextBox         _txtImporte;
        private Label           _lblBalance;
        private Button          _btnAgregar;
        private Button          _btnCC;       // solo visible si llevaCC
        private Button          _btnCancelar;

        // Datos del pago (in-form, reemplazan a frmImputacionFormasPagos)
        private Label   _lblDatosTit;
        private Label   _lblDato1, _lblDato2, _lblDato3;
        private TextBox _txtDato1, _txtDato2, _txtDato3;
        private bool    _planSelNecesitaDatos;
        // dgvFormasPago y btnCobrar vienen del Designer

        // ── Constructor ─────────────────────────────────────────────────────
        public frmImputacionVenta(decimal total, int plan, int llevaCC = 0)
        {
            _total         = total;
            _defaultPlanId = plan;
            _llevaCC       = llevaCC == 1;
            InitializeComponent();
        }

        // ── Load ─────────────────────────────────────────────────────────────
        private void frmImputacionVenta_Load(object sender, EventArgs e)
        {
            construirUI();
            estadoInicial();
        }

        // ── Construcción programática de la UI ───────────────────────────────
        private void construirUI()
        {
            this.ClientSize = new Size(730, 478);

            // — Título total —
            var lblTotalTit = new Label
            {
                Text     = "Total a cobrar:",
                Font     = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(12, 12),
                AutoSize = true
            };
            _lblTotalValor = new Label
            {
                Text      = "$ " + _total.ToString("N2", new CultureInfo("es-AR")),
                Font      = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location  = new Point(185, 10),
                AutoSize  = true
            };

            // — Panel de planes (izquierda) —
            var lblPlanesTit = new Label
            {
                Text     = "Planes de pago  (presione el número del plan):",
                Font     = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(12, 50),
                AutoSize = true
            };
            _pnlPlanes = new FlowLayoutPanel
            {
                Location      = new Point(12, 70),
                Size          = new Size(340, 130),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = true,
                BorderStyle   = BorderStyle.FixedSingle,
                AutoScroll    = true
            };

            // — Área de entrada (derecha) —
            _lblPlanSel = new Label
            {
                Text      = "Plan: —",
                Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                Location  = new Point(368, 50),
                AutoSize  = true
            };

            var lblImp = new Label
            {
                Text     = "Importe:",
                Font     = new Font("Segoe UI", 10, FontStyle.Regular),
                Location = new Point(368, 82),
                AutoSize = true
            };
            _txtImporte = new TextBox
            {
                Font      = new Font("Segoe UI", 14, FontStyle.Bold),
                Location  = new Point(448, 78),
                Size      = new Size(268, 32),
                TextAlign = HorizontalAlignment.Right
            };
            _txtImporte.KeyDown += txtImporte_KeyDown;

            // — Datos del pago (3 campos inline, ocultos cuando el plan no los requiere) —
            _lblDatosTit = new Label
            {
                Text      = "Datos del pago:",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                Location  = new Point(368, 122),
                AutoSize  = true,
                Visible   = false
            };

            int yDatos = 144;
            _lblDato1 = new Label { Text = "Ref. 1:", Font = new Font("Segoe UI", 9), Location = new Point(368, yDatos + 3), AutoSize = true, Visible = false };
            _txtDato1 = new TextBox { Font = new Font("Segoe UI", 10), Location = new Point(425, yDatos), Size = new Size(291, 24), Visible = false };

            _lblDato2 = new Label { Text = "Ref. 2:", Font = new Font("Segoe UI", 9), Location = new Point(368, yDatos + 33), AutoSize = true, Visible = false };
            _txtDato2 = new TextBox { Font = new Font("Segoe UI", 10), Location = new Point(425, yDatos + 30), Size = new Size(291, 24), Visible = false };

            _lblDato3 = new Label { Text = "Ref. 3:", Font = new Font("Segoe UI", 9), Location = new Point(368, yDatos + 63), AutoSize = true, Visible = false };
            _txtDato3 = new TextBox { Font = new Font("Segoe UI", 10), Location = new Point(425, yDatos + 60), Size = new Size(291, 24), Visible = false };

            // Navegación con Enter entre los campos de datos:
            // Ref1 → Ref2, Ref2 → Ref3, Ref3 → confirma agregando el pago.
            _txtDato1.KeyDown += datoCampo_KeyDown;
            _txtDato2.KeyDown += datoCampo_KeyDown;
            _txtDato3.KeyDown += datoCampo_KeyDown;

            _btnAgregar = new Button
            {
                Text      = "Agregar  [Enter]",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                Location  = new Point(368, 232),
                Size      = new Size(348, 34),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnAgregar.Click += (s, ev) => agregarPago();

            _lblBalance = new Label
            {
                Text      = "",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DarkOrange,
                Location  = new Point(368, 274),
                AutoSize  = true
            };

            // — Grid (viene del Designer, se reposiciona) —
            dgvFormasPago.Location = new Point(12, 300);
            dgvFormasPago.Size     = new Size(706, 90);

            var lblGridTip = new Label
            {
                Text      = "Doble clic para editar   |   [Supr] para eliminar la fila seleccionada",
                Font      = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location  = new Point(12, 394),
                AutoSize  = true
            };

            // — Botones inferiores —
            _btnCancelar = new Button
            {
                Text      = "Cancelar  [Esc]",
                Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                Location  = new Point(12, 420),
                Size      = new Size(180, 44),
                BackColor = Color.IndianRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _btnCancelar.Click += (s, ev) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Botón CC — solo si el cliente lleva cuenta corriente
            if (_llevaCC)
            {
                _btnCC = new Button
                {
                    Text      = "Saldo a Cuenta Corriente  [F5]",
                    Font      = new Font("Segoe UI", 10, FontStyle.Bold),
                    Location  = new Point(200, 420),
                    Size      = new Size(280, 44),
                    BackColor = Color.DarkOrange,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                _btnCC.Click += (s, ev) => agregarSaldoCC();
                Controls.Add(_btnCC);
            }

            // btnCobrar viene del Designer — reposicionar y estilizar
            btnCobrar.Location  = new Point(490, 420);
            btnCobrar.Size      = new Size(228, 44);
            btnCobrar.Text      = "Cobrar  [F3]";
            btnCobrar.Font      = new Font("Segoe UI", 11, FontStyle.Bold);
            btnCobrar.BackColor = Color.MediumSeaGreen;
            btnCobrar.ForeColor = Color.White;
            btnCobrar.FlatStyle = FlatStyle.Flat;
            btnCobrar.Enabled   = false;

            // Si no lleva CC, alinear Cobrar a la derecha del Cancelar
            if (!_llevaCC)
            {
                btnCobrar.Location = new Point(200, 420);
                btnCobrar.Size     = new Size(518, 44);
            }

            // — Agregar controles —
            Controls.Add(lblTotalTit);
            Controls.Add(_lblTotalValor);
            Controls.Add(lblPlanesTit);
            Controls.Add(_pnlPlanes);
            Controls.Add(_lblPlanSel);
            Controls.Add(lblImp);
            Controls.Add(_txtImporte);
            Controls.Add(_lblDatosTit);
            Controls.Add(_lblDato1); Controls.Add(_txtDato1);
            Controls.Add(_lblDato2); Controls.Add(_txtDato2);
            Controls.Add(_lblDato3); Controls.Add(_txtDato3);
            Controls.Add(_btnAgregar);
            Controls.Add(_lblBalance);
            Controls.Add(lblGridTip);
            Controls.Add(_btnCancelar);
        }

        // ── Estado inicial ────────────────────────────────────────────────────
        private void estadoInicial()
        {
            // 1. Cargar planes
            Clases.ClassVentas instVentas = new Clases.ClassVentas();
            _planes = instVentas.traerPlanesPago();   // cols: Nro, Nombre, Recargo

            // 2. Crear botones de planes
            _pnlPlanes.Controls.Clear();
            foreach (DataRow row in _planes.Rows)
            {
                int    nro    = int.Parse(row["Nro"].ToString());
                string nombre = row["Nombre"].ToString();
                var btn = new Button
                {
                    Text      = $"[{nro}]  {nombre}",
                    Tag       = row,
                    AutoSize  = false,
                    Size      = new Size(155, 36),
                    BackColor = Color.Silver,
                    ForeColor = Color.Black,
                    Font      = new Font("Segoe UI", 9, FontStyle.Regular),
                    FlatStyle = FlatStyle.Flat,
                    Margin    = new Padding(3)
                };
                int capturedNro = nro;
                btn.Click += (s, ev) => seleccionarPlan(capturedNro);
                _pnlPlanes.Controls.Add(btn);
            }

            // 3. Bind grid — vacía al inicio
            dgvFormasPago.DataSource = unDT;

            // 4. Balance inicial
            actualizarBalance();

            // 5. Foco en panel para que las teclas numéricas funcionen
            _pnlPlanes.Focus();
        }

        // ── Seleccionar plan ──────────────────────────────────────────────────
        private void seleccionarPlan(int id)
        {
            DataRow planRow = _planes?.AsEnumerable()
                .FirstOrDefault(r => int.Parse(r["Nro"].ToString()) == id);
            if (planRow == null) return;

            _planSelId      = id;
            _planSelNombre  = planRow["Nombre"].ToString();
            _planSelRecargo = decimal.Parse(planRow["Recargo"].ToString());
            _planSelNecesitaDatos = obtenerNecesitaDatos(id);

            _lblPlanSel.Text = "Plan: " + _planSelNombre;

            // Resaltar botón activo
            foreach (Button b in _pnlPlanes.Controls.OfType<Button>())
            {
                bool activo = b.Text.StartsWith($"[{id}]");
                b.BackColor = activo ? Color.SteelBlue : Color.Silver;
                b.ForeColor = activo ? Color.White     : Color.Black;
            }

            // Mostrar/ocultar campos de datos según el plan
            mostrarCamposDatos(_planSelNecesitaDatos);
            limpiarCamposDatos();

            // Pre-cargar el saldo faltante (excluyendo ítem CC) y seleccionarlo todo
            decimal faltanteReal = _total - unDT.Where(x => x.idPlan != 0).Sum(x => x.Importe);
            _txtImporte.Text = faltanteReal > 0
                ? faltanteReal.ToString("N2", new CultureInfo("es-AR"))
                : string.Empty;
            _txtImporte.Focus();
            _txtImporte.SelectAll();
        }

        // ── Mostrar/ocultar los 3 campos de datos del pago ────────────────────
        private void mostrarCamposDatos(bool visible)
        {
            _lblDatosTit.Visible = visible;
            _lblDato1.Visible    = _txtDato1.Visible = visible;
            _lblDato2.Visible    = _txtDato2.Visible = visible;
            _lblDato3.Visible    = _txtDato3.Visible = visible;
        }

        private void limpiarCamposDatos()
        {
            _txtDato1.Text = string.Empty;
            _txtDato2.Text = string.Empty;
            _txtDato3.Text = string.Empty;
        }

        // ── Actualizar balance ────────────────────────────────────────────────
        private void actualizarBalance()
        {
            decimal imputadoReal = unDT.Where(x => x.idPlan != 0).Sum(x => x.Importe);
            decimal imputadoCC   = unDT.Where(x => x.idPlan == 0).Sum(x => x.Importe);
            decimal imputadoTotal = imputadoReal + imputadoCC;
            decimal faltante     = _total - imputadoTotal;
            var     cultura      = new CultureInfo("es-AR");

            string texto = $"Efectivo/Tarjeta: ${imputadoReal.ToString("N2", cultura)}";
            if (imputadoCC > 0)
                texto += $"   |   Cuenta Corriente: ${imputadoCC.ToString("N2", cultura)}";
            if (faltante > 0)
                texto += $"   |   Faltante: ${faltante.ToString("N2", cultura)}";

            _lblBalance.Text = texto;

            if (faltante <= 0 && imputadoTotal > 0)
            {
                _lblBalance.ForeColor = Color.DarkGreen;
                btnCobrar.Enabled     = true;
            }
            else
            {
                _lblBalance.ForeColor = faltante > 0 ? Color.DarkOrange : Color.Red;
                btnCobrar.Enabled     = false;
            }

            // Ocultar columnas internas del grid
            if (dgvFormasPago.Columns.Contains("idMedio"))       dgvFormasPago.Columns["idMedio"].Visible       = false;
            if (dgvFormasPago.Columns.Contains("idPlan"))        dgvFormasPago.Columns["idPlan"].Visible        = false;
            if (dgvFormasPago.Columns.Contains("necesitaDatos")) dgvFormasPago.Columns["necesitaDatos"].Visible = false;
            if (dgvFormasPago.Columns.Contains("Recargo"))       dgvFormasPago.Columns["Recargo"].Visible       = false;
        }

        // ── Parsear decimal (acepta '.' o ',' como separador decimal) ─────────
        private static decimal ParsearImporte(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return 0;
            valor = valor.Trim();

            int puntos = valor.Count(c => c == '.');
            int comas  = valor.Count(c => c == ',');

            if (puntos == 1 && comas == 0)
                valor = valor.Replace('.', ',');

            if (puntos > 0 && comas == 1)
                valor = valor.Replace(".", "");

            if (decimal.TryParse(valor, NumberStyles.Any,
                    new CultureInfo("es-AR"), out decimal resultado))
                return resultado;

            valor = valor.Replace(',', '.');
            decimal.TryParse(valor, NumberStyles.Any,
                CultureInfo.InvariantCulture, out resultado);
            return resultado;
        }

        // ── Agregar saldo a Cuenta Corriente ──────────────────────────────────
        private void agregarSaldoCC()
        {
            decimal faltante = _total - unDT.Sum(x => x.Importe);
            if (faltante <= 0)
            {
                MessageBox.Show(this, "No hay saldo pendiente para asignar a Cuenta Corriente.",
                    "Cobros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Reemplazar ítem CC anterior si ya existe
            var ccExistente = unDT.FirstOrDefault(x => x.idPlan == 0);
            if (ccExistente != null)
                unDT.Remove(ccExistente);

            unDT.Add(new Clases.ClassVentas.CobroFormasPago
            {
                idMedio       = 0,
                Medio         = "Cuenta Corriente",
                idPlan        = 0,
                Plan          = "Cuenta Corriente",
                Importe       = faltante,
                Recargo       = 0,
                Referencia1   = string.Empty,
                Referencia2   = string.Empty,
                Referencia3   = string.Empty,
                necesitaDatos = false
            });

            actualizarBalance();
            _pnlPlanes.Focus();
        }

        // ── Agregar pago (interactivo) ────────────────────────────────────────
        private void agregarPago()
        {
            if (_planSelId == null)
            {
                MessageBox.Show(this, "Seleccione un plan de pago presionando su número.",
                    "Cobros", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _pnlPlanes.Focus();
                return;
            }

            decimal monto = ParsearImporte(_txtImporte.Text);
            if (monto <= 0)
            {
                MessageBox.Show(this, "Ingrese un importe válido mayor a cero.",
                    "Cobros", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtImporte.Focus();
                _txtImporte.SelectAll();
                return;
            }

            // Faltante sin contar ítem CC (que se ajusta automáticamente)
            decimal faltanteReal = _total - unDT.Where(x => x.idPlan != 0).Sum(x => x.Importe);
            if (monto > faltanteReal)
                monto = faltanteReal;

            bool necesita = _planSelNecesitaDatos;
            string ref1 = string.Empty, ref2 = string.Empty, ref3 = string.Empty;

            // Los datos se ingresan directamente en los TextBoxes inline.
            // Si el plan los requiere y aún no se ingresó nada, llevamos foco al primer campo.
            if (necesita)
            {
                ref1 = _txtDato1.Text.Trim();
                ref2 = _txtDato2.Text.Trim();
                ref3 = _txtDato3.Text.Trim();

                if (string.IsNullOrEmpty(ref1) &&
                    string.IsNullOrEmpty(ref2) &&
                    string.IsNullOrEmpty(ref3))
                {
                    _txtDato1.Focus();
                    _txtDato1.SelectAll();
                    return;
                }
            }

            agregarPagoInterno(_planSelId.Value, _planSelNombre, _planSelRecargo,
                               necesita, monto, ref1, ref2, ref3);

            // Si había un ítem CC, ajustar su importe al nuevo faltante
            ajustarItemCC();

            _txtImporte.Text = string.Empty;
            limpiarCamposDatos();
            mostrarCamposDatos(false);
            actualizarBalance();
            _pnlPlanes.Focus();
        }

        // ── Navegación con Enter entre los campos de datos ────────────────────
        private void datoCampo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (sender == _txtDato1)
                {
                    _txtDato2.Focus();
                    _txtDato2.SelectAll();
                }
                else if (sender == _txtDato2)
                {
                    _txtDato3.Focus();
                    _txtDato3.SelectAll();
                }
                else // _txtDato3
                {
                    agregarPago();
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                limpiarCamposDatos();
                _txtImporte.Focus();
                _txtImporte.SelectAll();
            }
        }

        // ── Ajustar ítem CC tras agregar un pago real ─────────────────────────
        private void ajustarItemCC()
        {
            var ccItem = unDT.FirstOrDefault(x => x.idPlan == 0);
            if (ccItem == null) return;

            decimal nuevoFaltante = _total - unDT.Where(x => x.idPlan != 0).Sum(x => x.Importe);
            if (nuevoFaltante <= 0)
                unDT.Remove(ccItem);
            else
                ccItem.Importe = nuevoFaltante;
        }

        // ── Agregar pago interno (sin interacción de usuario) ─────────────────
        private void agregarPagoInterno(int planId, string planNombre, decimal recargo,
                                        bool necesitaDatos, decimal monto,
                                        string ref1, string ref2, string ref3)
        {
            Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
            var planDB = instConfig.traerPlanesPagoPorId(planId);
            if (planDB == null || planDB.Rows.Count == 0) return;
            int medioId = int.Parse(planDB.Rows[0]["fk_medioPago"].ToString());
            var medioDB = instConfig.traerMediosDePagoPorId(medioId);
            if (medioDB == null || medioDB.Rows.Count == 0) return;

            unDT.Add(new Clases.ClassVentas.CobroFormasPago
            {
                idMedio       = medioId,
                Medio         = medioDB.Rows[0]["Nombre"].ToString(),
                idPlan        = planId,
                Plan          = planNombre,
                Importe       = monto,
                Recargo       = recargo,
                Referencia1   = ref1,
                Referencia2   = ref2,
                Referencia3   = ref3,
                necesitaDatos = necesitaDatos
            });
        }

        // ── Verificar si el medio del plan necesita datos adicionales ─────────
        private bool obtenerNecesitaDatos(int planId)
        {
            try
            {
                Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                var planDB = instConfig.traerPlanesPagoPorId(planId);
                if (planDB == null || planDB.Rows.Count == 0) return false;
                int medioId = int.Parse(planDB.Rows[0]["fk_medioPago"].ToString());
                var medioDB = instConfig.traerMediosDePagoPorId(medioId);
                if (medioDB == null || medioDB.Rows.Count == 0) return false;
                return bool.Parse(medioDB.Rows[0]["conDatos"].ToString());
            }
            catch { return false; }
        }

        // ── Confirmar cobro ───────────────────────────────────────────────────
        private void btnCobrar_Click(object sender, EventArgs e)
        {
            if (!validarFormulario()) return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ── Validación ───────────────────────────────────────────────────────
        private bool validarFormulario()
        {
            errorProvider1.Clear();

            decimal imputadoTotal = unDT.Sum(x => x.Importe);
            if (imputadoTotal < _total)
            {
                decimal faltante = _total - imputadoTotal;
                errorProvider1.SetError(dgvFormasPago,
                    "Falta imputar $" + faltante.ToString("N2", new CultureInfo("es-AR")));
                return false;
            }

            foreach (Clases.ClassVentas.CobroFormasPago item in unDT)
            {
                if (item.idPlan == 0) continue;  // ítem CC no necesita referencias
                if (item.necesitaDatos &&
                    string.IsNullOrEmpty(item.Referencia1) &&
                    string.IsNullOrEmpty(item.Referencia2) &&
                    string.IsNullOrEmpty(item.Referencia3))
                {
                    errorProvider1.SetError(dgvFormasPago,
                        "Faltan datos de cobros en algunas formas de pago seleccionadas.");
                    return false;
                }
            }
            return true;
        }

        // ── Tecla Enter / Escape en _txtImporte ───────────────────────────────
        private void txtImporte_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                // Si el plan requiere datos y aún están vacíos, saltamos al primer
                // campo de referencia para que el operador los ingrese antes de confirmar.
                if (_planSelNecesitaDatos &&
                    string.IsNullOrWhiteSpace(_txtDato1.Text) &&
                    string.IsNullOrWhiteSpace(_txtDato2.Text) &&
                    string.IsNullOrWhiteSpace(_txtDato3.Text))
                {
                    _txtDato1.Focus();
                    _txtDato1.SelectAll();
                }
                else
                {
                    agregarPago();
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                _txtImporte.Text   = string.Empty;
                _pnlPlanes.Focus();
            }
        }

        // ── ProcessCmdKey — captura teclas globales ───────────────────────────
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Si el foco está en cualquier campo de entrada de texto, dejamos que la
            // tecla viaje normal (caso contrario se seleccionarían planes al tipear datos).
            bool enCampoTexto = _txtImporte.Focused
                             || _txtDato1.Focused
                             || _txtDato2.Focused
                             || _txtDato3.Focused;

            // 1-9 (teclado superior) → seleccionar plan
            if (keyData >= Keys.D1 && keyData <= Keys.D9 && !enCampoTexto)
            {
                seleccionarPlan((int)keyData - (int)Keys.D0);
                return true;
            }
            // Numpad 1-9
            if (keyData >= Keys.NumPad1 && keyData <= Keys.NumPad9 && !enCampoTexto)
            {
                seleccionarPlan((int)keyData - (int)Keys.NumPad0);
                return true;
            }

            if (keyData == Keys.F3)
            {
                btnCobrar_Click(null, null);
                return true;
            }
            if (keyData == Keys.F5 && _llevaCC)
            {
                agregarSaldoCC();
                return true;
            }
            if (keyData == Keys.Escape && !enCampoTexto)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return true;
            }
            if (keyData == Keys.Delete && dgvFormasPago.SelectedRows.Count > 0
                                       && !enCampoTexto)
            {
                var item = (Clases.ClassVentas.CobroFormasPago)
                    dgvFormasPago.SelectedRows[0].DataBoundItem;
                if (item != null)
                {
                    unDT.Remove(item);
                    actualizarBalance();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ── Doble clic en grid → editar fila ──────────────────────────────────
        private void dgvFormasPago_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var pago = (Clases.ClassVentas.CobroFormasPago)
                dgvFormasPago.Rows[e.RowIndex].DataBoundItem;

            if (pago == null) return;

            // El ítem CC no se edita con el formulario de referencias
            if (pago.idPlan == 0)
            {
                MessageBox.Show(this,
                    "El ítem Cuenta Corriente se ajusta automáticamente al saldo restante.\n" +
                    "Para modificarlo, elimínelo con [Supr] y vuelva a presionar [F5].",
                    "Cuenta Corriente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var f = new frmImputacionFormasPagos(
                pago.Importe, pago.idPlan,
                pago.Referencia1, pago.Referencia2, pago.Referencia3, true))
            {
                if (f.ShowDialog() != DialogResult.OK || !f._edicion) return;

                Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
                var planDB = instConfig.traerPlanesPagoPorId(f._planPago ?? 0);
                if (planDB == null || planDB.Rows.Count == 0) return;
                var medioDB = instConfig.traerMediosDePagoPorId(
                    int.Parse(planDB.Rows[0]["fk_medioPago"].ToString()));
                if (medioDB == null || medioDB.Rows.Count == 0) return;

                pago.Importe     = f._total;
                pago.idMedio     = int.Parse(medioDB.Rows[0]["id"].ToString());
                pago.Medio       = medioDB.Rows[0]["Nombre"].ToString();
                pago.idPlan      = f._planPago ?? pago.idPlan;
                pago.Plan        = planDB.Rows[0]["nombre"].ToString();
                pago.Referencia1 = f._dato1;
                pago.Referencia2 = f._dato2;
                pago.Referencia3 = f._dato3;

                // Ajustar CC si existía
                ajustarItemCC();
                dgvFormasPago.Refresh();
                actualizarBalance();
            }
        }

        // ── Filas eliminadas del grid ─────────────────────────────────────────
        private void dgvFormasPago_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            actualizarBalance();
        }
    }
}
