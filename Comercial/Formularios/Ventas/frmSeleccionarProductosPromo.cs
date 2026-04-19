using Comercial.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Comercial.Formularios.Ventas
{
    /// <summary>
    /// Diálogo que se muestra cuando se agrega al carrito un producto marcado como promoción.
    /// El operador selecciona cuántas unidades de cada producto elegible cubre cada slot.
    /// </summary>
    public partial class frmSeleccionarProductosPromo : Form
    {
        public int fkProducto;

        // Resultado: lista de (idSlot, idProducto, cantidad)
        public List<ComponentePromo> componentesElegidos = new List<ComponentePromo>();

        // Datos de la promo cargados del DataSet
        private DataTable dtSlots;
        private DataTable dtSlotProductos;

        // Por cada slot (índice): lista de NumericUpDown creados para sus productos
        private Dictionary<int, List<(int fkProducto, NumericUpDown nud)>> controlsPorSlot
            = new Dictionary<int, List<(int, NumericUpDown)>>();

        // Por cada slot: cantidad requerida y label de pendiente
        private Dictionary<int, decimal> cantReqPorSlot = new Dictionary<int, decimal>();
        private Dictionary<int, Label> lblPendientePorSlot = new Dictionary<int, Label>();
        // ids de slot DB
        private Dictionary<int, int> idSlotDB = new Dictionary<int, int>();

        public frmSeleccionarProductosPromo()
        {
            InitializeComponent();
        }

        private void frmSeleccionarProductosPromo_Load(object sender, EventArgs e)
        {
            cargarPromocion();
        }

        private void cargarPromocion()
        {
            ClassPromociones instPromo = new ClassPromociones();
            DataSet ds = instPromo.obtenerPromocionCompleta(fkProducto);

            if (ds.Tables.Count < 3)
            {
                MessageBox.Show("Error al cargar la promoción.", "Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            DataTable dtCab = ds.Tables[0];
            dtSlots = ds.Tables[1];
            dtSlotProductos = ds.Tables[2];

            if (dtCab.Rows.Count > 0)
                this.Text = $"Seleccionar componentes: {dtCab.Rows[0]["producto"]}";

            construirPanelSlots();
            actualizarBotonOK();
        }

        private void construirPanelSlots()
        {
            pnlSlots.Controls.Clear();
            controlsPorSlot.Clear();
            cantReqPorSlot.Clear();
            lblPendientePorSlot.Clear();
            idSlotDB.Clear();

            int yOffset = 5;

            for (int si = 0; si < dtSlots.Rows.Count; si++)
            {
                DataRow rowSlot = dtSlots.Rows[si];
                int slotId = int.Parse(rowSlot["idSlot"].ToString());
                decimal cantReq = decimal.Parse(rowSlot["cantidadRequerida"].ToString());
                string desc = rowSlot["descripcion"].ToString();

                idSlotDB[si] = slotId;
                cantReqPorSlot[si] = cantReq;

                // Título del slot
                Label lblTitulo = new Label
                {
                    Text = $"Slot {si + 1}: {desc}  (seleccionar {cantReq})",
                    Font = new Font(this.Font, FontStyle.Bold),
                    Location = new Point(5, yOffset),
                    AutoSize = true
                };
                pnlSlots.Controls.Add(lblTitulo);
                yOffset += 22;

                var nudList = new List<(int fkProducto, NumericUpDown nud)>();

                // Buscar productos de este slot
                foreach (DataRow rp in dtSlotProductos.Rows)
                {
                    if (int.Parse(rp["fk_slot"].ToString()) != slotId) continue;

                    int idProd = int.Parse(rp["fk_producto"].ToString());
                    string descProd = rp["productoDesc"].ToString();

                    Label lblProd = new Label
                    {
                        Text = descProd,
                        Location = new Point(20, yOffset + 3),
                        Width = 300,
                        AutoSize = false
                    };
                    pnlSlots.Controls.Add(lblProd);

                    NumericUpDown nud = new NumericUpDown
                    {
                        Location = new Point(330, yOffset),
                        Width = 70,
                        Minimum = 0,
                        Maximum = cantReq * 10,
                        DecimalPlaces = 0,
                        Value = 0,
                        Tag = si   // índice del slot
                    };
                    int capturedSi = si;
                    nud.ValueChanged += (s, e) => { recalcularSlot(capturedSi); actualizarBotonOK(); };
                    pnlSlots.Controls.Add(nud);

                    nudList.Add((idProd, nud));
                    yOffset += 28;
                }

                // Label de pendiente
                Label lblPend = new Label
                {
                    Text = $"Pendiente: {cantReq}",
                    Location = new Point(20, yOffset),
                    AutoSize = true,
                    ForeColor = Color.Red
                };
                pnlSlots.Controls.Add(lblPend);
                lblPendientePorSlot[si] = lblPend;

                controlsPorSlot[si] = nudList;
                yOffset += 30;

                // Separador
                Label sep = new Label
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    Location = new Point(0, yOffset),
                    Size = new Size(pnlSlots.Width - 20, 2)
                };
                pnlSlots.Controls.Add(sep);
                yOffset += 8;
            }
        }

        private void recalcularSlot(int si)
        {
            if (!controlsPorSlot.ContainsKey(si)) return;
            decimal suma = 0;
            foreach (var (_, nud) in controlsPorSlot[si])
                suma += nud.Value;

            decimal req = cantReqPorSlot[si];
            decimal pend = req - suma;

            if (lblPendientePorSlot.ContainsKey(si))
            {
                lblPendientePorSlot[si].Text = pend == 0 ? "Completo" : $"Pendiente: {pend}";
                lblPendientePorSlot[si].ForeColor = pend == 0 ? Color.Green : Color.Red;
            }
        }

        private void actualizarBotonOK()
        {
            bool todosCompletos = true;
            foreach (var kvp in cantReqPorSlot)
            {
                decimal suma = 0;
                if (controlsPorSlot.ContainsKey(kvp.Key))
                    foreach (var (_, nud) in controlsPorSlot[kvp.Key])
                        suma += nud.Value;
                if (suma != kvp.Value) { todosCompletos = false; break; }
            }
            btnOK.Enabled = todosCompletos;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            componentesElegidos.Clear();
            foreach (var kvp in controlsPorSlot)
            {
                int si = kvp.Key;
                int slotId = idSlotDB[si];
                foreach (var (idProd, nud) in kvp.Value)
                {
                    if (nud.Value > 0)
                        componentesElegidos.Add(new ComponentePromo { IdSlot = slotId, IdProducto = idProd, Cantidad = nud.Value });
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Construye el string de detalle para sp_VentasAddPromoComponentes:
        /// "idSlot#idProducto*cantidad;..."
        /// </summary>
        public string construirDetalleComponentes()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in componentesElegidos)
            {
                if (sb.Length > 0) sb.Append(";");
                sb.Append($"{c.IdSlot}#{c.IdProducto}*{c.Cantidad.ToString().Replace(',', '.')}");
            }
            return sb.ToString();
        }
    }

    public class ComponentePromo
    {
        public int IdSlot { get; set; }
        public int IdProducto { get; set; }
        public decimal Cantidad { get; set; }
    }
}
