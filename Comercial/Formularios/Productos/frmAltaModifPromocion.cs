using Comercial.Clases;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Comercial.Formularios.Productos
{
    public partial class frmAltaModifPromocion : Form
    {
        public int unAccion;          // 1=nueva, 2=editar
        public int unIdPromocion;
        public int unFkProducto;

        ClassPromociones instPromo = new ClassPromociones();
        ClassProductos instProd = new ClassProductos();

        // Tabla interna de slots: columnas: idSlot (0=nuevo), numero, cantReq, descripcion
        private DataTable dtSlots = new DataTable();
        // Tabla interna de productos por slot: columnas: fk_slot_temp (indice fila), fk_producto, descripcion
        private DataTable dtSlotProductos = new DataTable();

        public frmAltaModifPromocion()
        {
            InitializeComponent();
            inicializarTablas();
        }

        private void inicializarTablas()
        {
            dtSlots.Columns.Add("idSlot", typeof(int));
            dtSlots.Columns.Add("numero", typeof(int));
            dtSlots.Columns.Add("cantidadRequerida", typeof(decimal));
            dtSlots.Columns.Add("descripcion", typeof(string));

            dtSlotProductos.Columns.Add("slotIndex", typeof(int));  // indice de fila en dtSlots
            dtSlotProductos.Columns.Add("fk_producto", typeof(int));
            dtSlotProductos.Columns.Add("descripcion", typeof(string));
            dtSlotProductos.Columns.Add("codBarras", typeof(string));
        }

        private void frmAltaModifPromocion_Load(object sender, EventArgs e)
        {
            cargarProductosCombo();
            if (unAccion == 2)
                cargarPromocion();
        }

        private void cargarProductosCombo()
        {
            DataTable dt = instProd.traeProductosPpal(" where baja = 0 and esPromocion = 1 order by descripcion");
            cboProducto.DataSource = dt;
            cboProducto.DisplayMember = "descripcion";
            cboProducto.ValueMember = "id";
            cboProducto.SelectedIndex = -1;
        }

        private void cargarPromocion()
        {
            DataSet ds = instPromo.obtenerPromocionCompleta(unFkProducto);
            if (ds.Tables.Count < 3) return;

            DataTable dtCab = ds.Tables[0];
            DataTable dtSlotsDB = ds.Tables[1];
            DataTable dtSlotProdDB = ds.Tables[2];

            if (dtCab.Rows.Count > 0)
            {
                cboProducto.SelectedValue = unFkProducto;
                cboProducto.Enabled = false;
                chkActiva.Checked = Convert.ToBoolean(dtCab.Rows[0]["activa"]);

                if (dtCab.Rows[0]["fechaDesde"] != DBNull.Value)
                {
                    chkFechas.Checked = true;
                    dtpDesde.Value = Convert.ToDateTime(dtCab.Rows[0]["fechaDesde"]);
                }
                if (dtCab.Rows[0]["fechaHasta"] != DBNull.Value)
                {
                    dtpHasta.Value = Convert.ToDateTime(dtCab.Rows[0]["fechaHasta"]);
                }
            }

            dtSlots.Clear();
            dtSlotProductos.Clear();

            for (int i = 0; i < dtSlotsDB.Rows.Count; i++)
            {
                DataRow r = dtSlotsDB.Rows[i];
                dtSlots.Rows.Add(
                    int.Parse(r["idSlot"].ToString()),
                    int.Parse(r["numero"].ToString()),
                    decimal.Parse(r["cantidadRequerida"].ToString()),
                    r["descripcion"].ToString()
                );

                int idSlot = int.Parse(r["idSlot"].ToString());
                foreach (DataRow rp in dtSlotProdDB.Rows)
                {
                    if (int.Parse(rp["fk_slot"].ToString()) == idSlot)
                    {
                        dtSlotProductos.Rows.Add(i, int.Parse(rp["fk_producto"].ToString()),
                            rp["productoDesc"].ToString(), rp["codBarras"].ToString());
                    }
                }
            }

            refrescarGridSlots();
        }

        private void refrescarGridSlots()
        {
            dgvSlots.DataSource = null;
            dgvSlots.DataSource = dtSlots;
            if (dgvSlots.Columns.Contains("idSlot")) dgvSlots.Columns["idSlot"].Visible = false;
            if (dgvSlots.Columns.Contains("numero")) dgvSlots.Columns["numero"].HeaderText = "N°";
            if (dgvSlots.Columns.Contains("cantidadRequerida")) dgvSlots.Columns["cantidadRequerida"].HeaderText = "Cant. Requerida";
            if (dgvSlots.Columns.Contains("descripcion")) dgvSlots.Columns["descripcion"].HeaderText = "Descripción";
        }

        private void refrescarGridSlotProductos(int slotIndex)
        {
            DataTable vista = dtSlotProductos.Clone();
            foreach (DataRow r in dtSlotProductos.Rows)
            {
                if (int.Parse(r["slotIndex"].ToString()) == slotIndex)
                    vista.ImportRow(r);
            }

            dgvSlotProductos.DataSource = null;
            dgvSlotProductos.DataSource = vista;
            if (dgvSlotProductos.Columns.Contains("slotIndex")) dgvSlotProductos.Columns["slotIndex"].Visible = false;
            if (dgvSlotProductos.Columns.Contains("fk_producto")) dgvSlotProductos.Columns["fk_producto"].Visible = false;
            if (dgvSlotProductos.Columns.Contains("descripcion")) dgvSlotProductos.Columns["descripcion"].HeaderText = "Producto";
            if (dgvSlotProductos.Columns.Contains("codBarras")) dgvSlotProductos.Columns["codBarras"].HeaderText = "Cód. Barras";
        }

        private void btnAgregarSlot_Click(object sender, EventArgs e)
        {
            int nuevoNumero = dtSlots.Rows.Count + 1;
            dtSlots.Rows.Add(0, nuevoNumero, 1m, $"Slot {nuevoNumero}");
            refrescarGridSlots();
            dgvSlots.CurrentCell = dgvSlots.Rows[dgvSlots.RowCount - 1].Cells["numero"];
        }

        private void btnQuitarSlot_Click(object sender, EventArgs e)
        {
            if (dgvSlots.CurrentRow == null) return;
            int idx = dgvSlots.CurrentRow.Index;

            // Eliminar productos de ese slot
            for (int i = dtSlotProductos.Rows.Count - 1; i >= 0; i--)
            {
                if (int.Parse(dtSlotProductos.Rows[i]["slotIndex"].ToString()) == idx)
                    dtSlotProductos.Rows.RemoveAt(i);
                else if (int.Parse(dtSlotProductos.Rows[i]["slotIndex"].ToString()) > idx)
                    dtSlotProductos.Rows[i]["slotIndex"] = int.Parse(dtSlotProductos.Rows[i]["slotIndex"].ToString()) - 1;
            }

            dtSlots.Rows.RemoveAt(idx);

            // Renumerar slots
            for (int i = 0; i < dtSlots.Rows.Count; i++)
                dtSlots.Rows[i]["numero"] = i + 1;

            refrescarGridSlots();
            dgvSlotProductos.DataSource = null;
        }

        private void dgvSlots_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSlots.CurrentRow == null) return;
            refrescarGridSlotProductos(dgvSlots.CurrentRow.Index);
        }

        private void btnAgregarProductoSlot_Click(object sender, EventArgs e)
        {
            if (dgvSlots.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un slot primero.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int slotIndex = dgvSlots.CurrentRow.Index;

            frmPickProducto frmBuscar = new frmPickProducto();
            frmBuscar.ShowDialog(this);

            if (frmBuscar.DialogResult != System.Windows.Forms.DialogResult.OK) return;

            int agregados = 0;
            int duplicados = 0;

            foreach (int idProd in frmBuscar.IdsSeleccionados)
            {
                // Verificar duplicado
                bool existe = false;
                foreach (DataRow r in dtSlotProductos.Rows)
                {
                    if (int.Parse(r["slotIndex"].ToString()) == slotIndex &&
                        int.Parse(r["fk_producto"].ToString()) == idProd)
                    {
                        existe = true;
                        break;
                    }
                }
                if (existe) { duplicados++; continue; }

                DataTable prod = instProd.traerProductosParaEditar(idProd);
                if (prod.Rows.Count > 0)
                {
                    dtSlotProductos.Rows.Add(slotIndex, idProd,
                        prod.Rows[0]["descripcion"].ToString(),
                        prod.Rows[0]["codBarras"].ToString());
                    agregados++;
                }
            }

            if (duplicados > 0)
                MessageBox.Show(
                    duplicados + " producto(s) ya estaban en el slot y no se agregaron.",
                    "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (agregados > 0)
                refrescarGridSlotProductos(slotIndex);
        }

        private void btnQuitarProductoSlot_Click(object sender, EventArgs e)
        {
            if (dgvSlots.CurrentRow == null || dgvSlotProductos.CurrentRow == null) return;
            int slotIndex = dgvSlots.CurrentRow.Index;

            int idProdElim = int.Parse(dgvSlotProductos.CurrentRow.Cells["fk_producto"].Value.ToString());

            for (int i = dtSlotProductos.Rows.Count - 1; i >= 0; i--)
            {
                if (int.Parse(dtSlotProductos.Rows[i]["slotIndex"].ToString()) == slotIndex &&
                    int.Parse(dtSlotProductos.Rows[i]["fk_producto"].ToString()) == idProdElim)
                {
                    dtSlotProductos.Rows.RemoveAt(i);
                    break;
                }
            }

            refrescarGridSlotProductos(slotIndex);
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (!formularioValido()) return;

            int fkProd = int.Parse(cboProducto.SelectedValue.ToString());
            bool activa = chkActiva.Checked;
            DateTime? desde = chkFechas.Checked ? (DateTime?)dtpDesde.Value.Date : null;
            DateTime? hasta = chkFechas.Checked ? (DateTime?)dtpHasta.Value.Date : null;

            int idPromo;
            if (unAccion == 1)
            {
                idPromo = instPromo.ABMPromocion(1, 0, fkProd, activa, desde, hasta);
            }
            else
            {
                idPromo = unIdPromocion;
                instPromo.ABMPromocion(2, idPromo, fkProd, activa, desde, hasta);
            }

            if (idPromo <= 0)
            {
                MessageBox.Show("Error al guardar la promoción.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Marcar producto como promo
            instProd.actualizarEsPromocion(fkProd, true);

            // Guardar slots
            string detalleSlots = construirDetalleSlots();
            int resSl = instPromo.guardarSlots(idPromo, detalleSlots);
            if (resSl == -1)
            {
                MessageBox.Show("Error al guardar los slots.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Close();
        }

        private string construirDetalleSlots()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dtSlots.Rows.Count; i++)
            {
                DataRow r = dtSlots.Rows[i];
                string num = r["numero"].ToString();
                string cant = r["cantidadRequerida"].ToString().Replace(',', '.');
                string desc = r["descripcion"].ToString();

                StringBuilder prods = new StringBuilder();
                foreach (DataRow rp in dtSlotProductos.Rows)
                {
                    if (int.Parse(rp["slotIndex"].ToString()) == i)
                    {
                        if (prods.Length > 0) prods.Append(",");
                        prods.Append(rp["fk_producto"].ToString());
                    }
                }

                if (sb.Length > 0) sb.Append(";");
                sb.Append($"{num}|{cant}|{desc}|{prods}");
            }
            return sb.ToString();
        }

        private bool formularioValido()
        {
            if (cboProducto.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar el producto base de la promoción.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dtSlots.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un slot.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            for (int i = 0; i < dtSlots.Rows.Count; i++)
            {
                bool tieneProductos = false;
                foreach (DataRow rp in dtSlotProductos.Rows)
                {
                    if (int.Parse(rp["slotIndex"].ToString()) == i) { tieneProductos = true; break; }
                }
                if (!tieneProductos)
                {
                    MessageBox.Show($"El slot {i + 1} no tiene productos elegibles.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (chkFechas.Checked && dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MessageBox.Show("La fecha desde no puede ser mayor que la fecha hasta.", "Promociones", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkFechas_CheckedChanged(object sender, EventArgs e)
        {
            dtpDesde.Enabled = chkFechas.Checked;
            dtpHasta.Enabled = chkFechas.Checked;
        }

        private void frmAltaModifPromocion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F5)
            {
                btnGrabar_Click(null, null);
            }
        }
    }
}
