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

namespace Comercial.Formularios.Estadisticas
{
    public partial class frmDashboardProveedores : Form
    {
        public frmDashboardProveedores()
        {
            InitializeComponent();
        }

        private void frmDashboardProveedores_Load(object sender, EventArgs e)
        {
            cargarCombos();
            btnHoy_Click(btnHoy, null);
        }

        private void cargarCombos()
        {
            Clases.ClassProveedores instProv = new Clases.ClassProveedores();
            cboProveedor.DataSource = instProv.traeProveedores();
            cboProveedor.ValueMember = "id";
            cboProveedor.DisplayMember = "nombreComercial";
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

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var desde = dtpDesde.Value.Date;
            var hasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1);
            MostrarValores(desde, hasta,cbProveedor.Checked?(int?)cboProveedor.SelectedValue:null);
        }

        private void btnHoy_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var desde = hoy.Date;
            var hasta = hoy.Date.AddDays(1).AddSeconds(-1);
            MostrarValores(desde, hasta, cbProveedor.Checked ? (int?)cboProveedor.SelectedValue : null);
        }

        private void btn7Dias_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var desde = hoy.Date.AddDays(-6); // 🔥 clave
            var hasta = hoy.Date.AddDays(1).AddSeconds(-1);
            MostrarValores(desde, hasta, cbProveedor.Checked ? (int?)cboProveedor.SelectedValue : null);
        }

        private void btn30dias_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var desde = hoy.Date.AddDays(-29);
            var hasta = hoy.Date.AddDays(1).AddSeconds(-1);
            MostrarValores(desde, hasta, cbProveedor.Checked ? (int?)cboProveedor.SelectedValue : null);
        }

        private void btnMes_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var desde = new DateTime(hoy.Year, hoy.Month, 1);
            var hasta = desde.AddMonths(1).AddSeconds(-1);
            MostrarValores(desde, hasta, cbProveedor.Checked ? (int?)cboProveedor.SelectedValue : null);
        }

        private void btnMesPasado_Click(object sender, EventArgs e)
        {
            MarcarBotonSeleccionado((Button)sender);
            var hoy = DateTime.Now;
            var primerDiaMesActual = new DateTime(hoy.Year, hoy.Month, 1);

            var desde = primerDiaMesActual.AddMonths(-1);   // 🔥 inicio mes pasado
            var hasta = primerDiaMesActual.AddSeconds(-1);  // 🔥 fin mes pasado
            MostrarValores(desde, hasta, cbProveedor.Checked ? (int?)cboProveedor.SelectedValue : null);
        }

        private void MostrarValores(DateTime desde, DateTime hasta, int? proveedorId)
        {
            mostrarMetricas(desde, hasta, proveedorId);
            mostrarGraficos(desde, hasta, proveedorId);
        }

        private void mostrarMetricas(DateTime desde, DateTime hasta, int? proveedorId)
        {
            lblCompras.Text = "0,00";
            lblComprasCant.Text = "0";
            lblComprasMax.Text = "0,00";
            lblComprasProm.Text = "0,00";
           
            Clases.ClassEstadisticas instEst = new Clases.ClassEstadisticas();
            DataTable metricas = instEst.traerDashboardMetricasProveedores(desde, hasta, proveedorId);

            if (metricas.Rows.Count == 0) return;

            lblCompras.Text = metricas.Rows[0]["TotalCompras"].ToString();
            lblComprasCant.Text = metricas.Rows[0]["CantidadCompras"].ToString();
            lblComprasMax.Text = metricas.Rows[0]["MejorDia"].ToString();
            lblComprasProm.Text = metricas.Rows[0]["PromedioCompras"].ToString();
            
        }

        private void mostrarGraficos(DateTime desde, DateTime hasta, int? proveedorId)
        {
            LimpiarGraficos(this);
            Clases.ClassEstadisticas instEst = new Clases.ClassEstadisticas();
            DataSet DatosGrafios = instEst.traerDashGraficosCompras(desde, hasta, proveedorId);

            var ComprasPorDia = DatosGrafios.Tables[0];
            var ComprasPorProveedor = DatosGrafios.Tables[1];
            var ComprasPorRubro = DatosGrafios.Tables[2];

            if (ComprasPorDia.Rows.Count > 0)
            {
                dibujarComprasPorDia(ComprasPorDia);
            }

            if (ComprasPorProveedor.Rows.Count > 0)
            {
                dibujarComprasPorProveedor(ComprasPorProveedor);
            }

            if (ComprasPorRubro.Rows.Count > 0)
            {
                dibujarComprasPorRubro(ComprasPorRubro);
            }
        }

        private void dibujarComprasPorRubro(DataTable ComprasPorRubro)
        {
            pieChartRubros.Series.Clear();

            var series = new LiveCharts.SeriesCollection();

            foreach (DataRow row in ComprasPorRubro.Rows)
            {
                decimal valor = Convert.ToDecimal(row["TotalCompras"]);
                string rubro = row["Rubro"].ToString();

                // 🔥 evitar valores 0 o negativos
                if (valor <= 0) continue;

                series.Add(new LiveCharts.Wpf.PieSeries
                {
                    Title = rubro,
                    Values = new LiveCharts.ChartValues<decimal> { valor },

                    DataLabels = false, // limpio tipo dashboard

                    // 🔥 tooltip formato argentino
                    LabelPoint = p => p.Y.ToString("N2", new CultureInfo("es-AR"))
                });
            }

            pieChartRubros.Series = series;

            // 🔹 leyenda a la derecha
            pieChartRubros.LegendLocation = LiveCharts.LegendLocation.Right;

            // 🔥 texto de leyenda en blanco (para fondo oscuro)
            pieChartRubros.DefaultLegend.Foreground =
                System.Windows.Media.Brushes.White;
        }
        private void dibujarComprasPorProveedor(DataTable ComprasPorProveedor) 
        {
            pieChartProveedores.Series.Clear();

            var series = new LiveCharts.SeriesCollection();

            foreach (DataRow row in ComprasPorProveedor.Rows)
            {
                decimal valor = Convert.ToDecimal(row["TotalCompras"]);
                string proveedor = row["Proveedor"].ToString();

                // 🔥 evitar valores 0 o negativos
                if (valor <= 0) continue;

                series.Add(new LiveCharts.Wpf.PieSeries
                {
                    Title = proveedor,
                    Values = new LiveCharts.ChartValues<decimal> { valor },

                    DataLabels = false, // limpio tipo dashboard

                    // 🔥 opcional: tooltip con formato argentino
                    LabelPoint = p => p.Y.ToString("N2", new CultureInfo("es-AR"))
                });
            }

            pieChartProveedores.Series = series;

            // 🔹 leyenda a la derecha
            pieChartProveedores.LegendLocation = LiveCharts.LegendLocation.Right;

            // 🔥 texto de leyenda en blanco (para fondo oscuro)
            pieChartProveedores.DefaultLegend.Foreground =
                System.Windows.Media.Brushes.White;
        }
        private void dibujarComprasPorDia(DataTable ComprasPorDia)
        {
            cartesianChartComprasPorDia.Series.Clear();

            var series = new LiveCharts.SeriesCollection();

            // 🔹 agrupar por proveedor
            var proveedores = ComprasPorDia.AsEnumerable()
                .Select(r => r["Proveedor"].ToString())
                .Distinct();

            // 🔹 eje X (fechas únicas ordenadas)
            var fechas = ComprasPorDia.AsEnumerable()
                .Select(r => Convert.ToDateTime(r["Fecha"]))
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            foreach (var proveedor in proveedores)
            {
                var valores = new LiveCharts.ChartValues<decimal>();

                foreach (var fecha in fechas)
                {
                    var row = ComprasPorDia.AsEnumerable()
                        .FirstOrDefault(r =>
                            r["Proveedor"].ToString() == proveedor &&
                            Convert.ToDateTime(r["Fecha"]) == fecha);

                    decimal total = row != null
                        ? Convert.ToDecimal(row["TotalCompras"])
                        : 0;

                    valores.Add(total);
                }

                series.Add(new LiveCharts.Wpf.LineSeries
                {
                    Title = proveedor,
                    Values = valores,
                    PointGeometry = null // 🔥 sin puntos, más limpio
                });
            }

            cartesianChartComprasPorDia.Series = series;

            // 🔹 etiquetas del eje X
            cartesianChartComprasPorDia.AxisX.Clear();
            cartesianChartComprasPorDia.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Labels = fechas.Select(f => f.ToString("dd/MM")).ToArray(),
                Foreground = System.Windows.Media.Brushes.White
            });

            // 🔹 eje Y formato
            cartesianChartComprasPorDia.AxisY.Clear();
            cartesianChartComprasPorDia.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                LabelFormatter = value => value.ToString("N2", new CultureInfo("es-AR")),
                Foreground = System.Windows.Media.Brushes.White
            });

            // 🔥 leyenda
            cartesianChartComprasPorDia.LegendLocation = LiveCharts.LegendLocation.Right;
            cartesianChartComprasPorDia.DefaultLegend.Foreground =
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
