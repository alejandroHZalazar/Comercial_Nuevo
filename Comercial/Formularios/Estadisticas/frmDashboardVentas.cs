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
            Clases.ClassEstadisticas instEstad = new Clases.ClassEstadisticas();

            DataTable ventasTot = instEstad.traerDashboardVentasTotales();

            if (ventasTot.Rows.Count == 0) return;

            lblVentasHoy.Text = Convert.ToDecimal(ventasTot.Rows[0]["ventas_hoy"]).ToString("C");
            lblVentasMes.Text = Convert.ToDecimal(ventasTot.Rows[0]["ventas_mes"]).ToString("C");
            lblVentasAnio.Text = Convert.ToDecimal(ventasTot.Rows[0]["ventas_anio"]).ToString("C");


        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Clases.ClassEstadisticas instEstad = new Clases.ClassEstadisticas();
            var valores = new ChartValues<double>();
            var meses = new List<string>();

            DataTable ventasMes = instEstad.traerDashboardVentasMes(dtpDesde.Value, dtpHasta.Value);

            foreach (DataRow fila in ventasMes.Rows)
            {
                valores.Add(Convert.ToDouble(fila["total"]));

                // Mes + Año para mayor claridad
                string mesNombre = CultureInfo.CurrentCulture.DateTimeFormat
                                    .GetAbbreviatedMonthName(Convert.ToInt32(fila["mes"]));
                meses.Add($"{mesNombre} {fila["anio"]}");
            }

            // Configuración de la serie
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Ventas",
                    Values = valores,
                    PointGeometry = DefaultGeometries.Circle, // Marca cada punto
                    PointGeometrySize = 10
                }
            };

            // Configuración del eje X
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisX.Add(new Axis
            {
                Labels = meses,
                Separator = new Separator { Step = 1 },
                LabelsRotation = 45 // Rotación para evitar superposición
            });

            // Configuración del eje Y con formato moneda
            cartesianChart1.AxisY.Clear();
            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = value => value.ToString("C0", CultureInfo.CurrentCulture)
            });

            DataTable ventasClientes = instEstad.traerDashboardVentasClientes(dtpDesde.Value, dtpHasta.Value);

            if (ventasClientes.Rows.Count == 0) return;

            SeriesCollection pieSeries = new SeriesCollection();

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

            pieChart1.Series = pieSeries;


        }
    }
}
