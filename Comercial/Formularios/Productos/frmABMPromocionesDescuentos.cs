using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    /// <summary>
    /// ABM de promociones por cantidad (NxM) y por porcentaje de descuento (equivale al módulo
    /// "Promociones por Cantidad / Descuento" de Comercial Web). El motor se aplica en Ventas y Pedidos solo con
    /// el parámetro promociones/descuentos en '1'; este ABM funciona con las tablas creadas aunque esté apagado.
    /// </summary>
    public class frmABMPromocionesDescuentos : Form
    {
        private readonly Clases.ClassPromocionesDescuentos inst = new Clases.ClassPromocionesDescuentos();

        private DataGridView dgv;
        private Label lblEstado;
        private Button btnNueva, btnEditar, btnActivar, btnEliminar;

        public frmABMPromocionesDescuentos()
        {
            Text = "Promociones por cantidad / descuento";
            Font = new Font("Segoe UI", 9F);
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 540);
            MinimumSize = new Size(760, 420);
            KeyPreview = true;

            construir();

            Load += (s, e) => recargar();
            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F4) nueva();
                else if (e.KeyCode == Keys.F5) editar();
                else if (e.KeyCode == Keys.Escape) Close();
            };
        }

        private void construir()
        {
            lblEstado = new Label { Dock = DockStyle.Top, Height = 34, Padding = new Padding(10, 9, 0, 0) };

            Panel barra = new Panel { Dock = DockStyle.Top, Height = 46, Padding = new Padding(8) };
            btnNueva = new Button { Text = "Nueva (F4)", Size = new Size(120, 32), Location = new Point(8, 7) };
            btnEditar = new Button { Text = "Editar (F5)", Size = new Size(120, 32), Location = new Point(136, 7) };
            btnActivar = new Button { Text = "Activar / Desactivar", Size = new Size(170, 32), Location = new Point(264, 7) };
            btnEliminar = new Button { Text = "Eliminar", Size = new Size(110, 32), Location = new Point(442, 7) };
            barra.Controls.AddRange(new Control[] { btnNueva, btnEditar, btnActivar, btnEliminar });
            Clases.IconosVariantes.Aplicar(btnNueva, Properties.Resources.plus);
            Clases.IconosVariantes.Aplicar(btnEditar, Properties.Resources.pencil_edit_button__1_);
            Clases.IconosVariantes.Aplicar(btnActivar, Properties.Resources.play_button);
            Clases.IconosVariantes.Aplicar(btnEliminar, Properties.Resources.rubbish_bin__1_);
            btnNueva.Click += (s, e) => nueva();
            btnEditar.Click += (s, e) => editar();
            btnActivar.Click += (s, e) => activar();
            btnEliminar.Click += (s, e) => eliminar();

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false, RowHeadersVisible = false, MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle, EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.DimGray;
            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(242, 242, 242) };
            dgv.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) editar(); };

            Controls.Add(dgv);
            Controls.Add(barra);
            Controls.Add(lblEstado);
            dgv.BringToFront();
        }

        private int idSeleccionado()
        {
            if (dgv.CurrentRow == null || dgv.CurrentRow.Cells["id"].Value == null) return 0;
            return Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value);
        }

        private string nombreSeleccionado()
        {
            return dgv.CurrentRow == null ? string.Empty : Convert.ToString(dgv.CurrentRow.Cells["nombre"].Value);
        }

        private void recargar()
        {
            bool tablas = Clases.ClassPromocionesDescuentos.tablasListas();
            bool habilitado = Clases.ClassPromocionesDescuentos.habilitado();

            if (!tablas)
            {
                lblEstado.ForeColor = Color.Firebrick;
                lblEstado.Text = "Faltan crear las tablas de promociones: ejecute scripts/promociones_reglas.sql (el mismo de Comercial Web).";
            }
            else if (!habilitado)
            {
                lblEstado.ForeColor = Color.DarkOrange;
                lblEstado.Text = "El módulo está APAGADO: las promociones no se aplican en Ventas ni Pedidos hasta que el parámetro promociones / descuentos valga '1'.";
            }
            else
            {
                lblEstado.ForeColor = Color.DarkGreen;
                lblEstado.Text = "El módulo está ACTIVO: se aplica en Ventas, Venta minorista y Pedidos.";
            }
            btnNueva.Enabled = btnEditar.Enabled = btnActivar.Enabled = btnEliminar.Enabled = tablas;

            int idSel = idSeleccionado();
            dgv.DataSource = inst.listar();

            if (dgv.Columns.Contains("id")) dgv.Columns["id"].Visible = false;
            if (dgv.Columns.Contains("tipo")) dgv.Columns["tipo"].Visible = false;
            Titulo("nombre", "Promoción", 160);
            Titulo("resumen", "Beneficio", 90);
            Titulo("desde", "Desde", 80);
            Titulo("hasta", "Hasta", 80);
            Titulo("activa", "Activa", 40);
            Titulo("vigente", "Vigente ahora", 55);
            Titulo("cantProductos", "Productos", 50);
            if (dgv.Columns.Contains("desde")) dgv.Columns["desde"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            if (dgv.Columns.Contains("hasta")) dgv.Columns["hasta"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

            foreach (DataGridViewRow fila in dgv.Rows)
            {
                if (idSel > 0 && Convert.ToInt32(fila.Cells["id"].Value) == idSel) { fila.Selected = true; dgv.CurrentCell = fila.Cells["nombre"]; }
                if (!Convert.ToBoolean(fila.Cells["vigente"].Value)) fila.DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        private void Titulo(string col, string texto, int peso)
        {
            if (!dgv.Columns.Contains(col)) return;
            dgv.Columns[col].HeaderText = texto;
            dgv.Columns[col].FillWeight = peso;
        }

        private void nueva()
        {
            if (!btnNueva.Enabled) return;
            using (frmAltaModifPromocionDescuento f = new frmAltaModifPromocionDescuento { IdRegla = 0 })
                if (f.ShowDialog(this) == DialogResult.OK) recargar();
        }

        private void editar()
        {
            int id = idSeleccionado();
            if (id == 0 || !btnEditar.Enabled) return;
            using (frmAltaModifPromocionDescuento f = new frmAltaModifPromocionDescuento { IdRegla = id })
                if (f.ShowDialog(this) == DialogResult.OK) recargar();
        }

        private void activar()
        {
            int id = idSeleccionado();
            if (id == 0) return;
            try
            {
                inst.toggleActiva(id);
                recargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo cambiar el estado: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void eliminar()
        {
            int id = idSeleccionado();
            if (id == 0) return;
            if (MessageBox.Show(this, "¿Desea eliminar la promoción '" + nombreSeleccionado() + "'?\r\n" +
                                "Las ventas y pedidos ya grabados conservan el registro de la promoción aplicada.",
                                Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                inst.eliminar(id);
                recargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "No se pudo eliminar la promoción: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
