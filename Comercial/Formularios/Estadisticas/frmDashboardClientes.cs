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
using System.Windows.Forms.DataVisualization.Charting;

namespace Comercial.Formularios.Estadisticas
{
    public partial class frmDashboardClientes : Form
    {
        public frmDashboardClientes()
        {
            InitializeComponent();
        }

        private void frmDashboardClientes_Load(object sender, EventArgs e)
        {
            cargarCombos();
            btnHoy_Click(btnHoy, null);
        }

        private void cargarCombos()
        {
            Clases.ClassClientes instClie = new Clases.ClassClientes();

            cboCliente.DataSource = instClie.buscarAVender();
            cboCliente.ValueMember = "ID";
            cboCliente.DisplayMember = "Completo";
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var desde = dtpDesde.Value.Date;
            var hasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1);
            MostrarValores(desde, hasta);
        }

        private void btnHoy_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var desde = hoy.Date;
            var hasta = hoy.Date.AddDays(1).AddSeconds(-1);
            MostrarValores(desde, hasta);
        }

        private void btn7Dias_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var desde = hoy.Date.AddDays(-6); // 🔥 clave
            var hasta = hoy.Date.AddDays(1).AddSeconds(-1);
            MostrarValores(desde, hasta);
        }

        private void btn30dias_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var desde = hoy.Date.AddDays(-29);
            var hasta = hoy.Date.AddDays(1).AddSeconds(-1);
            MostrarValores(desde, hasta);
        }

        private void btnMes_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var desde = new DateTime(hoy.Year, hoy.Month, 1);
            var hasta = desde.AddMonths(1).AddSeconds(-1);
            MostrarValores(desde, hasta);
        }

        private void btnMesPasado_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var primerDiaMesActual = new DateTime(hoy.Year, hoy.Month, 1);

            var desde = primerDiaMesActual.AddMonths(-1);   // 🔥 inicio mes pasado
            var hasta = primerDiaMesActual.AddSeconds(-1);  // 🔥 fin mes pasado
            MostrarValores(desde, hasta);
        }

        private void MarcarBotonSeleccionado(Button botonSeleccionado)
        {
            foreach (Control ctrl in panelFiltros.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = SystemColors.Control; // normal
                    btn.ForeColor = System.Drawing.Color.Orange;
                }
            }

            // 🔥 botón seleccionado
            botonSeleccionado.BackColor = System.Drawing.Color.Blue;
            botonSeleccionado.ForeColor = System.Drawing.Color.White;
        }

        private void MostrarValores(DateTime desde, DateTime hasta)
        {
            var clienteId = cbCliente.Checked ? (int?)cboCliente.SelectedValue : null;
            mostrarMetricas(desde, hasta, clienteId);
            mostrarGraficos(desde, hasta, clienteId);
        }

        private void mostrarMetricas(DateTime desde, DateTime hasta, int? CLienteId)
        {
            lblVentas.Text = "0,00";
            lblVentasCant.Text = "0";
            lblVentasMax.Text = "0,00";
            lblVentasProm.Text = "0,00";
            lblCosto.Text = "0,00";
            lblRentabilidad.Text = "0,00";

            Clases.ClassEstadisticas instEst = new Clases.ClassEstadisticas();
            DataTable metricas = instEst.traerDashboardMetricasClientes(desde, hasta, CLienteId);

            if (metricas.Rows.Count == 0) return;
            lblVentas.Text = metricas.Rows[0]["TotalVentas"].ToString();
            lblVentasCant.Text = metricas.Rows[0]["CantidadVentas"].ToString();
            lblVentasMax.Text = metricas.Rows[0]["PickitDia"].ToString();
            lblVentasProm.Text = metricas.Rows[0]["Promedio"].ToString();
            lblCosto.Text = metricas.Rows[0]["Costos"].ToString();
            lblRentabilidad.Text = metricas.Rows[0]["Ganancias"].ToString();

        }
        private void mostrarGraficos(DateTime desde, DateTime hasta, int? clienteId)
        {
            LimpiarGraficos(this);
            Clases.ClassEstadisticas instEst = new Clases.ClassEstadisticas();
            DataSet DatosGrafios = instEst.traerDashGraficosClientes(desde, hasta, clienteId);

            var VentasPorDia = DatosGrafios.Tables[0];
            var DeudasTop10 = DatosGrafios.Tables[1];
            var MediosPago = DatosGrafios.Tables[2];
            var VentasProveedor = DatosGrafios.Tables[3];
            var VentasRubro = DatosGrafios.Tables[4];

            if (VentasPorDia.Rows.Count > 0)
            {
                dibujarVentasPorDia(VentasPorDia);
            }

            if (DeudasTop10.Rows.Count > 0)
            {
                dibujarTop10Saldos(DeudasTop10);
            }

            if(MediosPago.Rows.Count > 0)
            {
                dibujarVentasMediosPago(MediosPago);
            }

            if (VentasProveedor.Rows.Count > 0)
            {
                dibujarVentasPorProveedor(VentasProveedor);
            }

            if (VentasRubro.Rows.Count > 0)
            {
                dibujarVentasPorRubro(VentasRubro);
            }
        }

        private void dibujarVentasPorRubro(DataTable VentasRubro)
        {
            pieChartRubros.Series.Clear();

            var series = new LiveCharts.SeriesCollection();

            decimal totalGeneral = 0;

            // 🔹 calcular total general
            foreach (DataRow row in VentasRubro.Rows)
            {
                totalGeneral += Convert.ToDecimal(row["TotalVenta"]);
            }

            // 🔹 armar la torta
            foreach (DataRow row in VentasRubro.Rows)
            {
                string rubro = row["Rubro"].ToString();
                decimal valor = Convert.ToDecimal(row["TotalVenta"]);

                // 🔥 evitar valores negativos o 0
                if (valor <= 0) continue;

                series.Add(new LiveCharts.Wpf.PieSeries
                {
                    Title = rubro,
                    Values = new LiveCharts.ChartValues<decimal> { valor },

                    DataLabels = false, // limpio

                    LabelPoint = chartPoint =>
                        $"{chartPoint.Participation:P1} ({valor.ToString("N2", new CultureInfo("es-AR"))})"
                });
            }

            pieChartRubros.Series = series;

            // 🔹 leyenda a la derecha
            pieChartRubros.LegendLocation = LiveCharts.LegendLocation.Right;

            // 🔹 modo oscuro (texto blanco)
            pieChartRubros.DefaultLegend.Foreground =
               System.Windows.Media.Brushes.White;


        }
        private void dibujarVentasPorProveedor(DataTable VentasProveedor)
        {
            pieChartProveedores.Series.Clear();

            var series = new LiveCharts.SeriesCollection();

            decimal totalGeneral = 0;

            // 🔹 calcular total general
            foreach (DataRow row in VentasProveedor.Rows)
            {
                totalGeneral += Convert.ToDecimal(row["TotalVenta"]);
            }

            // 🔹 crear porciones
            foreach (DataRow row in VentasProveedor.Rows)
            {
                string proveedor = row["Proveedor"].ToString();
                decimal valor = Convert.ToDecimal(row["TotalVenta"]);

                // 🔥 evitar valores negativos o 0 (por devoluciones)
                if (valor <= 0) continue;

                series.Add(new LiveCharts.Wpf.PieSeries
                {
                    Title = proveedor,
                    Values = new LiveCharts.ChartValues<decimal> { valor },

                    DataLabels = false, // 🔥 sin etiquetas en la torta

                    // 🔹 texto del tooltip / label
                    LabelPoint = chartPoint =>
                        $"{chartPoint.Participation:P1} ({valor.ToString("N2", new CultureInfo("es-AR"))})"
                });
            }

            pieChartProveedores.Series = series;

            // 🔹 leyenda a la derecha
            pieChartProveedores.LegendLocation = LiveCharts.LegendLocation.Right;
        }
        private void dibujarVentasMediosPago(DataTable MediosPago)
        {
            chartMediosPago.Series.Clear();
            chartMediosPago.ChartAreas.Clear();

            // 🔹 Área
            ChartArea area = new ChartArea("Area1");
            chartMediosPago.ChartAreas.Add(area);

            // 🔹 Fondo oscuro
            area.BackColor = Color.FromArgb(64, 64, 64);
            chartMediosPago.BackColor = Color.FromArgb(64, 64, 64);

            // 🔹 Ejes
            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisY.LabelStyle.ForeColor = Color.White;

            area.AxisX.LineColor = Color.White;
            area.AxisY.LineColor = Color.White;

            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gray;

            // 🔹 Formato argentino
            area.AxisY.LabelStyle.Format = "N2";

            // 🔹 Serie
            var serie = new System.Windows.Forms.DataVisualization.Charting.Series("Medios de Pago")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.White
            };

            chartMediosPago.Series.Add(serie);

            // 🔹 Cargar datos
            foreach (DataRow row in MediosPago.Rows)
            {
                string medio = row["MedioPago"].ToString();
                decimal total = Convert.ToDecimal(row["TotalVenta"]);

                int index = serie.Points.AddXY(medio, total);

                // 🔥 formato argentino en etiquetas
                serie.Points[index].Label = total.ToString("N2", new CultureInfo("es-AR"));

                // 🔹 Tooltip
                serie.Points[index].ToolTip = $"{medio}: {total.ToString("N2", new CultureInfo("es-AR"))}";
            }

            // 🔹 Rotar etiquetas (muy importante)
            area.AxisX.LabelStyle.Angle = -45;

            // 🔹 Quitar leyenda (opcional)
            chartMediosPago.Legends.Clear();

            // 🔹 Quitar título
            chartMediosPago.Titles.Clear();
        }
        private void dibujarTop10Saldos(DataTable DeudasTop10)
        {
            chartSaldos.Series.Clear();
            chartSaldos.ChartAreas.Clear();

            // 🔹 Área del gráfico
            ChartArea area = new ChartArea("Area1");
            chartSaldos.ChartAreas.Add(area);

            // 🔹 Fondo oscuro (como venías usando)
            area.BackColor = Color.FromArgb(64, 64, 64);
            chartSaldos.BackColor = Color.FromArgb(64, 64, 64);

            // 🔹 Configuración ejes
            area.AxisX.LabelStyle.ForeColor = Color.White;
            area.AxisY.LabelStyle.ForeColor = Color.White;

            area.AxisX.LineColor = Color.White;
            area.AxisY.LineColor = Color.White;

            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gray;

            // 🔹 Formato argentino eje Y
            area.AxisY.LabelStyle.Format = "N2";

            // 🔹 Serie
            var serie = new System.Windows.Forms.DataVisualization.Charting.Series("Deudas")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.White // 🔥 etiquetas arriba de las barras
            };

            chartSaldos.Series.Add(serie);

            // 🔹 Cargar datos
            foreach (DataRow row in DeudasTop10.Rows)
            {
                string cliente = row["Cliente"].ToString();
                decimal saldo = Convert.ToDecimal(row["Saldo"]);

                int index = serie.Points.AddXY(cliente, saldo);

                // 🔥 formato argentino en etiquetas
                serie.Points[index].Label = saldo.ToString("N2", new CultureInfo("es-AR"));
            }

            // 🔹 Rotar nombres (clave si son largos)
            area.AxisX.LabelStyle.Angle = -45;

            // 🔹 Quitar leyenda (opcional)
            chartSaldos.Legends.Clear();

            // 🔹 Quitar título si no lo querés
            chartSaldos.Titles.Clear();
        }

        private void dibujarVentasPorDia(DataTable VentasPorDia)
        {
            cartesianChartVentasPorDia.Series.Clear();

            var seriesCollection = new LiveCharts.SeriesCollection();

            // 🔹 Agrupar por cliente
            var clientes = VentasPorDia.AsEnumerable()
                .Select(r => r.Field<string>("Cliente"))
                .Distinct();

            // 🔹 Obtener todas las fechas ordenadas
            var fechas = VentasPorDia.AsEnumerable()
                .Select(r => Convert.ToDateTime(r["Fecha"]))
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            // 🔹 Labels eje X
            cartesianChartVentasPorDia.AxisX.Clear();
            cartesianChartVentasPorDia.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = fechas.Select(f => f.ToString("dd/MM")).ToList(),
                LabelsRotation = 15,
                Foreground = System.Windows.Media.Brushes.White
            });

            // 🔹 Eje Y formato
            cartesianChartVentasPorDia.AxisY.Clear();
            cartesianChartVentasPorDia.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                LabelFormatter = value => value.ToString("N0", new CultureInfo("es-AR")),
                Foreground = System.Windows.Media.Brushes.White
            });

            // 🔹 Crear una línea por cliente
            foreach (var cliente in clientes)
            {
                var valores = new LiveCharts.ChartValues<decimal>();

                foreach (var fecha in fechas)
                {
                    var row = VentasPorDia.AsEnumerable()
                        .FirstOrDefault(r =>
                            r.Field<string>("Cliente") == cliente &&
                            Convert.ToDateTime(r["Fecha"]) == fecha);

                    valores.Add(row != null ? Convert.ToDecimal(row["TotalVenta"]) : 0);
                }

                seriesCollection.Add(new LiveCharts.Wpf.LineSeries
                {
                    Title = cliente,
                    Values = valores,
                    PointGeometry = null // 🔥 más limpio tipo dashboard
                });
            }

            cartesianChartVentasPorDia.Series = seriesCollection;

            // 🔹 Leyenda
            cartesianChartVentasPorDia.LegendLocation = LiveCharts.LegendLocation.Right;
            cartesianChartVentasPorDia.DefaultLegend.Foreground =
               System.Windows.Media.Brushes.White;
        }
        private void LimpiarGraficos(Control contenedor)
        {
            foreach (Control ctrl in contenedor.Controls)
            {
                // 🔵 Chart clásico
                if (ctrl is System.Windows.Forms.DataVisualization.Charting.Chart chart)
                {
                    foreach (var serie in chart.Series)
                    {
                        serie.Points.Clear();
                    }

                    chart.Titles.Clear();
                }

                // 🟣 LiveCharts Cartesian
                if (ctrl is LiveCharts.WinForms.CartesianChart cartesian)
                {
                    cartesian.Series.Clear();
                    cartesian.AxisX.Clear();
                    cartesian.AxisY.Clear();
                }

                // 🟣 LiveCharts Pie
                if (ctrl is LiveCharts.WinForms.PieChart pie)
                {
                    pie.Series.Clear();
                }

                // 🔁 recursivo (clave)
                if (ctrl.HasChildren)
                {
                    LimpiarGraficos(ctrl);
                }
            }
        }
    }
}
