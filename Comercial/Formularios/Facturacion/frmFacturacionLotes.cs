using Comercial.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Facturacion
{
    public partial class frmFacturacionLotes : Form
    {
        int cantDec = Clases.ClassProductos.cantDecimales();
        int cantStock = Clases.ClassProductos.cantDecimalesStock();
        int facturaFiscal = Clases.ClassParametros.buscarParametro("ventas", "facturaFiscal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaFiscal"));
        string marcaFiscal = Clases.ClassParametros.buscarParametro("ventas", "marcaFiscal");
        int facturaElectronica = Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaElectronica"));
        public frmFacturacionLotes()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Clases.ClassVentas instVentas = new Clases.ClassVentas();
            var ventas = instVentas.TraerVentasNoFacturadas(dtpDesde.Value, dtpHasta.Value);
            if (ventas.Rows.Count == 0) return;

            // 1️⃣ Agregar nueva columna bool
            ventas.Columns.Add("SelBool", typeof(bool));

            // 2️⃣ Copiar valores (0 → false)
            foreach (DataRow row in ventas.Rows)
            {
                row["SelBool"] = Convert.ToBoolean(row["Sel"]);
            }

            // 3️⃣ Eliminar columna original
            ventas.Columns.Remove("Sel");

            // 4️⃣ Renombrar
            ventas.Columns["SelBool"].ColumnName = "Sel";

            dgvVentas.DataSource = ventas;
            dgvVentas.Columns["Nro"].ReadOnly = true;
            dgvVentas.Columns["Fecha"].ReadOnly = true;
            dgvVentas.Columns["Total Venta"].ReadOnly = true;
            dgvVentas.Columns["Nombre Comercial"].ReadOnly = true;
            dgvVentas.Columns["Cajero"].ReadOnly = true;
            dgvVentas.Columns["IVA"].ReadOnly = true;
            dgvVentas.Columns["Descuento"].ReadOnly = true;
            dgvVentas.Columns["Recargo"].ReadOnly = true;
            dgvVentas.Columns["Impuesto"].ReadOnly = true;
            dgvVentas.Columns["Sel"].DisplayIndex = 0;
            calcularTotales();
        }

        private void dgvVentas_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVentas.Rows.Count == 0) return;
            Clases.ClassVentas instVentas = new Clases.ClassVentas();
            dgvDetalle.DataSource = instVentas.traerTodosDetalles(long.Parse(dgvVentas.CurrentRow.Cells["Nro"].Value.ToString()));
            redondearDetalle();
        }

        private void redondearDetalle()
        {
            if (dgvDetalle.RowCount > 0)
            {
                dgvDetalle.Columns["Precio_S_IVA"].DefaultCellStyle.Format = "N" + cantDec.ToString();
                dgvDetalle.Columns["Precio_C_IVA"].DefaultCellStyle.Format = "N" + cantDec.ToString();
                dgvDetalle.Columns["Cantidad"].DefaultCellStyle.Format = "N" + cantStock.ToString();
                dgvDetalle.Columns["Subtotal"].DefaultCellStyle.Format = "N" + cantDec.ToString();
            }
        }

        private void dgvVentas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            calcularTotales();
        }

        private void calcularTotales()
        {
            var total = (decimal)0;
            foreach (DataGridViewRow fila in dgvVentas.Rows)
            {
                if ((bool)fila.Cells["Sel"].Value)
                {
                    total += (decimal)fila.Cells["Total Venta"].Value;
                }
            }

            nudTotal.Value = total;
        }

        private void dgvVentas_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvVentas.IsCurrentCellDirty)
            {
                dgvVentas.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            if (dgvVentas.RowCount == 0) return;

            foreach (DataGridViewRow fila in dgvVentas.Rows)
            {
                fila.Cells["Sel"].Value = true;
            }
        }

        private void btnNinguno_Click(object sender, EventArgs e)
        {
            if (dgvVentas.RowCount == 0) return;

            foreach (DataGridViewRow fila in dgvVentas.Rows)
            {
                fila.Cells["Sel"].Value = false;
            }
        }

        private async void btnFacturar_Click(object sender, EventArgs e)
        {
            btnFacturar.Enabled = false;
            dgvVentas.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            pbFacturar.Value = 0;
            rtbProceso.Clear();

            var filasSeleccionadas = dgvVentas.Rows
            .Cast<DataGridViewRow>()
            .Where(r => r.Cells["Sel"].Value != null && (bool)r.Cells["Sel"].Value)
            .ToList();

            pbFacturar.Maximum = filasSeleccionadas.Count;

            Clases.Fiscal tk = new Clases.Fiscal();
            await Task.Run(() =>
            {
                int progreso = 0;

                foreach (var fila in filasSeleccionadas)
                {

                    var venta = (Int64)fila.Cells["Nro"].Value;

                    this.Invoke(new Action(() =>
                    {
                        rtbProceso.AppendText($"Procesando venta N° {venta}...\n");
                    }));

                    if (facturaFiscal == 1)
                    {
                        ComprobanteFiscal status;
                        if (marcaFiscal.ToUpper() == "EPSON")
                        {
                            status = tk.imprimirFacturaEpson(venta);
                        }
                        else
                        {
                            status = tk.imprimirFacturaHasar(venta);
                        }
                        if (status == null)
                        {
                            this.Invoke(new Action(() =>
                            {
                                rtbProceso.AppendText($"❌ Error al facturar venta {venta}\n");
                            }));
                            continue;
                        }

                        var salida_Fiscal = tk.almacenarComprobanteFiscal(status);

                        if (salida_Fiscal == -1)
                        {
                            this.Invoke(new Action(() =>
                            {
                                rtbProceso.AppendText($"❌ Error al almacenar comprobante venta {venta}\n");
                            }));
                            continue;
                        }
                    }
                    else if (facturaElectronica == 1)
                    {
                        ClassFacturacionElectronica instFactElect = new ClassFacturacionElectronica();
                        var status = instFactElect.emitirFacturaElectronica(venta);
                        if (!status.Result)
                        {
                            this.Invoke(new Action(() =>
                            {
                                rtbProceso.AppendText($"❌ Error al almacenar comprobante venta {venta}\n");
                            }));
                            continue;
                        }
                    }

                    progreso++;

                    this.Invoke(new Action(() =>
                    {
                        pbFacturar.Value = progreso;
                        rtbProceso.AppendText($"✅ Venta {venta} facturada correctamente\n");
                    }));
                }
            });

            MessageBox.Show(this, "Proceso de facturación por lotes terminado con éxito", "FACTURACION LOTES", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnBuscar_Click(null, null);
        }

        private void frmFacturacionLotes_Load(object sender, EventArgs e)
        {
            if (facturaFiscal == 0 && facturaElectronica == 0)
            {
                MessageBox.Show(this, "Debe habilitar la Facturacion Fiscal o Electronica", "FACTURACION LOTES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
