using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Media;

namespace Comercial.Formularios.Estadisticas
{
    public partial class frmDashboardVentas : Form
    {
        public frmDashboardVentas()
        {
            InitializeComponent();
        }

        private void frmDashboardVentas_Load(object sender, EventArgs e)
        {

            btnHoy_Click(btnHoy,null);

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


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Clases.ClassEstadisticas instEstad = new Clases.ClassEstadisticas();
            
            var meses = new List<string>();

            DataTable ventasMes = instEstad.traerDashboardVentasMes(dtpDesde.Value, dtpHasta.Value);

            

            DataTable ventasClientes = instEstad.traerDashboardVentasClientes(dtpDesde.Value, dtpHasta.Value);

            if (ventasClientes.Rows.Count == 0) return;

            LiveCharts.SeriesCollection pieSeries = new LiveCharts.SeriesCollection();

            foreach (DataRow fila in ventasClientes.Rows)
            {
                pieSeries.Add(new PieSeries
                {
                    Title = fila["cliente"].ToString(),
                    Values = new ChartValues<double>
                {
                    Convert.ToDouble(fila["total"])
                },
                    DataLabels = true
                });
            }

            pieChartVentasPorCliente.Series = pieSeries;


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

        private void MostrarValores(DateTime desde, DateTime hasta)
        {
            mostrarMetricas(desde, hasta);
            mostrarGraficos(desde, hasta);
        }

        private void mostrarMetricas (DateTime desde, DateTime hasta)
        {
            lblVentas.Text = "0,00";
            lblVentasCant.Text = "0";
            lblVentasMax.Text = "0,00";
            lblVentasProm.Text = "0,00";
            lblCosto.Text = "0,00";
            lblRentabilidad.Text = "0,00";
            lblCompras.Text = "0,00";
            Clases.ClassEstadisticas instEst = new Clases.ClassEstadisticas();
            DataTable metricas = instEst.traerDashboardMetricasVentas(desde, hasta);
            if (metricas.Rows.Count == 0) return;
            lblVentas.Text = metricas.Rows[0]["TotalVentas"].ToString();
            lblVentasCant.Text = metricas.Rows[0]["CantidadVentas"].ToString();
            lblVentasMax.Text = metricas.Rows[0]["PickitDia"].ToString();
            lblVentasProm.Text = metricas.Rows[0]["Promedio"].ToString();
            lblCosto.Text = metricas.Rows[0]["Costos"].ToString();
            lblRentabilidad.Text = metricas.Rows[0]["Ganancias"].ToString();
            lblCompras.Text = metricas.Rows[0]["Compras"].ToString();
        }

        private void mostrarGraficos(DateTime desde, DateTime hasta)
        {
            LimpiarGraficos(this);
            Clases.ClassEstadisticas instEst = new Clases.ClassEstadisticas();
            DataSet DatosGrafios = instEst.traerDashGraficosVentas(desde, hasta);            

            var VentasPorDia = DatosGrafios.Tables[0];
            var VentasPorCliente = DatosGrafios.Tables[1];
            var MediosPago = DatosGrafios.Tables[2];
            var VentasProveedor = DatosGrafios.Tables[3];
            var VentasRubro = DatosGrafios.Tables[4];

            if (VentasPorDia.Rows.Count > 0)
            {
                dibujarVentasPorDia(VentasPorDia);
            }

            if (VentasPorCliente.Rows.Count > 0)
            {
                dibujarVentasPorCliente(VentasPorCliente);
            }

            if (MediosPago.Rows.Count > 0)
            {
                dibujarBarrasMediosPago(MediosPago);
            }

            if (VentasProveedor.Rows.Count > 0)
            {
                dibujarProductosPorProveedor(VentasProveedor);
            }

            if(VentasRubro.Rows.Count > 0)
            {
                dibujarTortaVentasPorRubro(VentasRubro);
            }
        }

        private void dibujarTortaVentasPorRubro(DataTable VentasRubro)
        {
            pieChartRubros.Series.Clear();

            var series = new LiveCharts.SeriesCollection();

            foreach (DataRow row in VentasRubro.Rows)
            {
                decimal valor = Convert.ToDecimal(row["TotalVenta"]);
                string rubro = row["Rubro"].ToString();

                // 🔥 evitar valores 0 o negativos (devoluciones)
                if (valor <= 0) continue;

                series.Add(new LiveCharts.Wpf.PieSeries
                {
                    Title = rubro,
                    Values = new LiveCharts.ChartValues<decimal> { valor },
                    DataLabels = false // limpio tipo dashboard
                });
            }

            pieChartRubros.Series = series;

            // 🔥 leyenda a la derecha
            pieChartRubros.LegendLocation = LiveCharts.LegendLocation.Right;

            // 🔥 texto de leyenda en blanco (para fondo oscuro)
            pieChartRubros.DefaultLegend.Foreground =
                System.Windows.Media.Brushes.White;
        }
        private void dibujarProductosPorProveedor(DataTable VentasProveedor)
        {
            pieChartProveedores.Series.Clear();

            var series = new LiveCharts.SeriesCollection();

            foreach (DataRow row in VentasProveedor.Rows)
            {
                decimal valor = Convert.ToDecimal(row["TotalVenta"]);
                string proveedor = row["Proveedor"].ToString();

                // 🔥 evitar valores inválidos (por devoluciones)
                if (valor <= 0) continue;

                series.Add(new LiveCharts.Wpf.PieSeries
                {
                    Title = proveedor,
                    Values = new LiveCharts.ChartValues<decimal> { valor },
                    DataLabels = false // limpio tipo dashboard
                });
            }

            pieChartProveedores.Series = series;

            // 🔥 leyenda a la derecha
            pieChartProveedores.LegendLocation = LiveCharts.LegendLocation.Right;

            // 🔥 color de texto de leyenda (importante)
            pieChartProveedores.DefaultLegend.Foreground =
                System.Windows.Media.Brushes.White;
        }
        private void dibujarBarrasMediosPago(DataTable MediosPago)
        {
            chartMediosPago.Series.Clear();
            chartMediosPago.ChartAreas.Clear();
            chartMediosPago.Titles.Clear();

            // 🔥 Fondo general (negro)
            chartMediosPago.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            // 🔹 Área del gráfico
            ChartArea area = new ChartArea("Area1");
            area.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            // 🔥 Ejes en blanco
            area.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            area.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;

            area.AxisX.LineColor = System.Drawing.Color.White;
            area.AxisY.LineColor = System.Drawing.Color.White;

            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;

            chartMediosPago.ChartAreas.Add(area);

            // 🔹 Serie
            var serie = new System.Windows.Forms.DataVisualization.Charting.Series("Medios de Pago");
            serie.ChartType = SeriesChartType.Column;
            serie.IsValueShownAsLabel = true;

            // 🔥 color de etiquetas arriba de las barras
            serie.LabelForeColor = System.Drawing.Color.White;

            chartMediosPago.Series.Add(serie);

            foreach (DataRow row in MediosPago.Rows)
            {
                string medio = row["MedioPago"].ToString();
                decimal total = Convert.ToDecimal(row["TotalVenta"]);

                int pointIndex = serie.Points.AddXY(medio, total);

                serie.Points[pointIndex].Label =
                    total.ToString("N2", new CultureInfo("es-AR"));
            }

            // 🔹 Formato eje Y
            area.AxisY.LabelStyle.Format = "N2";

            // 🔹 Rotación etiquetas X
            area.AxisX.LabelStyle.Angle = -45;

            area.AxisX.MajorGrid.Enabled = false;

            chartMediosPago.Legends.Clear();
            //// 🔥 Leyenda blanca
            //chartMediosPago.Legends.Clear();
            //Legend legend = new Legend();
            //legend.ForeColor = System.Drawing.Color.White;
            //legend.BackColor = System.Drawing.Color.Black;
            //chartMediosPago.Legends.Add(legend);
        }
        private void dibujarVentasPorCliente(DataTable VentasPorCliente)
        {
            pieChartVentasPorCliente.Series.Clear();

            LiveCharts.SeriesCollection series = new LiveCharts.SeriesCollection();

            foreach (DataRow row in VentasPorCliente.Rows)
            {
                decimal valor = Convert.ToDecimal(row["TotalVenta"]);
                string cliente = row["Cliente"].ToString();

                // 🔥 evitar valores 0 o negativos (por devoluciones)
                if (valor <= 0) continue;

                series.Add(new PieSeries
                {
                    Title = cliente,
                    Values = new ChartValues<decimal> { valor },
                    DataLabels = false,                    
                    LabelPoint = chartPoint =>
                        $"{chartPoint.Participation:P1} ({chartPoint.Y.ToString("N2", new CultureInfo("es-AR"))})"
                });
            }

            pieChartVentasPorCliente.Series = series;

            // 🔥 ubicación de leyenda
            pieChartVentasPorCliente.LegendLocation = LegendLocation.Right;
            pieChartVentasPorCliente.DefaultLegend.Foreground = System.Windows.Media.Brushes.White;
        }
        private void dibujarVentasPorDia(DataTable VentasPorDia)
        {
            var valores = new ChartValues<decimal>();
            var labels = new List<string>();

            foreach (DataRow row in VentasPorDia.Rows)
            {
                valores.Add(Convert.ToDecimal(row["TotalVenta"]));
                labels.Add(Convert.ToDateTime(row["Fecha"]).ToString("dd/MM"));
            }

            // 🔹 serie de línea
            var serie = new LineSeries
            {
                Title = "Ventas",
                Values = valores,
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 8
            };

            cartesianChartVentasPorDia.Series = new LiveCharts.SeriesCollection
    {
        serie
    };

            // 🔹 eje X (fechas)
            cartesianChartVentasPorDia.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Día",
                Labels = labels
            });

            // 🔹 eje Y (valores)
            cartesianChartVentasPorDia.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = "Total",
                LabelFormatter = value => value.ToString("N2") // formato argentino
            });

            // 🔥 opcional (mejora visual)
            cartesianChartVentasPorDia.LegendLocation = LegendLocation.Right;
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

        private void btnEstadisticasProveedores_Click(object sender, EventArgs e)
        {
            frmDashboardProveedores unFrmDashProv = new frmDashboardProveedores();
            unFrmDashProv.ShowDialog();
        }

        private void btnEstadisticasCLientes_Click(object sender, EventArgs e)
        {
            frmDashboardClientes unFrmDashClientes = new frmDashboardClientes();
            unFrmDashClientes.ShowDialog();
        }
    }
}
