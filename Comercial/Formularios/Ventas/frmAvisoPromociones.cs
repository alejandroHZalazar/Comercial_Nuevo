using Comercial.Clases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Comercial.Formularios.Ventas
{
    /// <summary>
    /// Diálogo modal que muestra las promociones detectadas antes de grabar la venta.
    /// El operador puede confirmar (aplica las promos) o volver (continúa cargando productos).
    /// </summary>
    public class frmAvisoPromociones : Form
    {
        public ResultadoPromos Resultado { get; set; }

        private RichTextBox rtbDetalle;
        private Button      btnAplicar;
        private Button      btnVolver;
        private Label       lblTitulo;
        private Label       lblPregunta;

        public frmAvisoPromociones()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text            = "Promociones detectadas";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.StartPosition   = FormStartPosition.CenterParent;
            this.ClientSize      = new Size(520, 530);
            this.KeyPreview      = true;
            this.KeyDown        += frmAvisoPromociones_KeyDown;

            // Intentar aplicar el color primario del sistema
            try
            {
                string colorHex = ClassParametros.traerColorDefecto();
                if (!string.IsNullOrEmpty(colorHex))
                    this.BackColor = ColorTranslator.FromHtml(colorHex);
            }
            catch { }

            lblTitulo = new Label
            {
                Text      = "Promociones detectadas",
                Font      = new Font("Segoe UI", 12, FontStyle.Bold),
                Location  = new Point(12, 12),
                Size      = new Size(480, 28),
                ForeColor = Color.FromArgb(30, 30, 60)
            };

            rtbDetalle = new RichTextBox
            {
                Location   = new Point(12, 50),
                Size       = new Size(480, 370),
                ReadOnly   = true,
                BackColor  = Color.White,
                Font       = new Font("Consolas", 10),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            lblPregunta = new Label
            {
                Text      = "¿Desea aplicar las promociones y continuar con la venta?",
                Location  = new Point(12, 430),
                Size      = new Size(480, 20),
                ForeColor = Color.FromArgb(30, 30, 60),
                Font      = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            btnAplicar = new Button
            {
                Text         = "Aplicar y Vender  [F5]",
                Location     = new Point(12, 455),
                Size         = new Size(180, 36),
                DialogResult = DialogResult.OK,
                BackColor    = Color.FromArgb(46, 139, 87),
                ForeColor    = Color.White,
                FlatStyle    = FlatStyle.Flat,
                Font         = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            btnVolver = new Button
            {
                Text         = "Volver a la venta  [Esc]",
                Location     = new Point(312, 455),
                Size         = new Size(180, 36),
                DialogResult = DialogResult.Cancel,
                BackColor    = Color.FromArgb(178, 34, 34),
                ForeColor    = Color.White,
                FlatStyle    = FlatStyle.Flat,
                Font         = new Font("Segoe UI", 9)
            };

            this.AcceptButton = btnAplicar;
            this.CancelButton = btnVolver;

            this.Controls.AddRange(new Control[] {
                lblTitulo, rtbDetalle, lblPregunta, btnAplicar, btnVolver
            });
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            CargarDetalle();
        }

        private void CargarDetalle()
        {
            if (Resultado == null) return;

            rtbDetalle.Clear();

            // ── Promociones que se van a aplicar ────────────────────────────
            if (Resultado.Aplicaciones.Count > 0)
            {
                AppendLine("PROMOCIONES A APLICAR:", Color.DarkGreen, bold: true);
                AppendLine(string.Empty, Color.Black);

                foreach (var ap in Resultado.Aplicaciones)
                {
                    AppendLine(
                        string.Format("  ✓  {0}  ×  {1}   →   ${2:N2} c/u",
                            ap.Promo.Descripcion,
                            ap.Veces,
                            ap.Promo.Precio),
                        Color.DarkGreen);

                    // Detallar componentes usados agrupados por producto
                    var resumen = ap.Componentes
                        .GroupBy(c => c.IdProducto)
                        .Select(g => string.Format("Prod.{0} × {1}", g.Key, g.Sum(x => x.Cantidad)));
                    AppendLine("       Componentes: " + string.Join(" | ", resumen), Color.Gray);
                    AppendLine(string.Empty, Color.Black);
                }
            }

            // ── Alertas de "falta 1 unidad" ──────────────────────────────────
            if (Resultado.Alertas.Count > 0)
            {
                AppendLine(string.Empty, Color.Black);
                AppendLine("AVISOS:", Color.DarkOrange, bold: true);
                AppendLine(string.Empty, Color.Black);

                foreach (var al in Resultado.Alertas)
                {
                    AppendLine(
                        string.Format("  ⚠  Falta 1 unidad para completar otra '{0}'.",
                            al.Promo.Descripcion),
                        Color.DarkOrange, bold: true);

                    if (al.NombresProductosPosibles.Count == 1)
                    {
                        AppendLine("       Producto faltante: " + al.NombresProductosPosibles[0],
                            Color.DarkOrange);
                    }
                    else if (al.NombresProductosPosibles.Count > 1)
                    {
                        AppendLine("       Puede aportar cualquiera de estos productos:",
                            Color.DarkOrange);
                        foreach (var nombre in al.NombresProductosPosibles)
                            AppendLine("         • " + nombre, Color.DarkOrange);
                    }

                    AppendLine(string.Empty, Color.Black);
                }
            }
        }

        private void AppendLine(string texto, Color color, bool bold = false)
        {
            rtbDetalle.SelectionStart  = rtbDetalle.TextLength;
            rtbDetalle.SelectionLength = 0;
            rtbDetalle.SelectionColor  = color;
            rtbDetalle.SelectionFont   = bold
                ? new Font(rtbDetalle.Font, FontStyle.Bold)
                : rtbDetalle.Font;
            rtbDetalle.AppendText(texto + Environment.NewLine);
        }

        private void frmAvisoPromociones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmAvisoPromociones
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmAvisoPromociones";
            this.Load += new System.EventHandler(this.frmAvisoPromociones_Load);
            this.ResumeLayout(false);

        }

        private void frmAvisoPromociones_Load(object sender, EventArgs e)
        {

        }
    }
}
