using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Comercial.Formularios.Ventas
{
    /// <summary>
    /// Aviso flotante NO bloqueante (esquina inferior derecha de la pantalla) para las promociones por cantidad:
    /// no toma el foco, se cierra solo y un aviso con el mismo id reemplaza al anterior.
    /// </summary>
    public class frmAvisoPromo : Form
    {
        private static readonly List<frmAvisoPromo> activos = new List<frmAvisoPromo>();

        private readonly string id;
        private readonly Timer cierre = new Timer();

        private frmAvisoPromo(string id, string titulo, string texto, Color color, int ms)
        {
            this.id = id;
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            TopMost = true;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F);

            Panel barra = new Panel { Dock = DockStyle.Left, Width = 6, BackColor = color };
            Label lblCerrar = new Label
            {
                Text = "×", Dock = DockStyle.None, AutoSize = false, Size = new Size(22, 22),
                TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand, ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
            lblCerrar.Click += (s, e) => Close();

            Label lblTitulo = new Label
            {
                Text = titulo, AutoSize = false, Location = new Point(16, 8), Size = new Size(366, 20),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = color
            };
            Label lblTexto = new Label
            {
                Text = texto, AutoSize = false, Location = new Point(16, 30), Width = 366, ForeColor = Color.FromArgb(40, 40, 40)
            };
            Size medida = TextRenderer.MeasureText(texto, lblTexto.Font, new Size(366, 2000), TextFormatFlags.WordBreak);
            lblTexto.Height = Math.Max(20, medida.Height + 4);

            ClientSize = new Size(398, lblTexto.Bottom + 12);
            lblCerrar.Location = new Point(ClientSize.Width - 26, 4);

            Controls.Add(lblCerrar);
            Controls.Add(lblTexto);
            Controls.Add(lblTitulo);
            Controls.Add(barra);
            Paint += (s, e) => e.Graphics.DrawRectangle(Pens.Silver, 0, 0, Width - 1, Height - 1);

            cierre.Interval = Math.Max(2000, ms);
            cierre.Tick += (s, e) => Close();
            Load += (s, e) => cierre.Start();
            FormClosed += (s, e) =>
            {
                cierre.Stop();
                cierre.Dispose();
                activos.Remove(this);
                reubicar();
            };
        }

        // No roba el foco a la grilla mientras el operador carga productos
        protected override bool ShowWithoutActivation { get { return true; } }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
                return cp;
            }
        }

        /// <summary>Muestra un aviso. Un mismo 'id' reemplaza al aviso anterior de ese id.</summary>
        public static void Mostrar(Form propietario, string id, string titulo, string texto, bool advertencia, int ms)
        {
            if (propietario == null || propietario.IsDisposed) return;

            foreach (frmAvisoPromo a in new List<frmAvisoPromo>(activos))
                if (a.id == id && !a.IsDisposed) a.Close();

            Color color = advertencia ? Color.FromArgb(214, 140, 0) : Color.FromArgb(24, 150, 90);
            frmAvisoPromo f = new frmAvisoPromo(id, titulo, texto, color, ms);
            activos.Add(f);
            reubicar();
            f.Show();   // sin Owner: no se minimiza ni tapa al propietario, y no recibe el foco
            if (!propietario.IsDisposed) propietario.Activate();
        }

        /// <summary>Cierra todos los avisos (al limpiar el formulario o cerrarlo).</summary>
        public static void CerrarTodos()
        {
            foreach (frmAvisoPromo a in new List<frmAvisoPromo>(activos))
                if (!a.IsDisposed) a.Close();
            activos.Clear();
        }

        // Apila los avisos desde la esquina inferior derecha del área de trabajo
        private static void reubicar()
        {
            Rectangle area = Screen.PrimaryScreen.WorkingArea;
            int y = area.Bottom - 12;
            for (int i = activos.Count - 1; i >= 0; i--)
            {
                frmAvisoPromo a = activos[i];
                if (a.IsDisposed) continue;
                y -= a.Height;
                a.Location = new Point(area.Right - a.Width - 12, y);
                y -= 8;
            }
        }
    }
}
