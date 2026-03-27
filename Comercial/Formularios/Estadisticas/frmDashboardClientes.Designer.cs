
namespace Comercial.Formularios.Estadisticas
{
    partial class frmDashboardClientes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDashboardClientes));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnHoy = new System.Windows.Forms.Button();
            this.btn30dias = new System.Windows.Forms.Button();
            this.btnMes = new System.Windows.Forms.Button();
            this.btnMesPasado = new System.Windows.Forms.Button();
            this.cartesianChartVentasPorDia = new LiveCharts.WinForms.CartesianChart();
            this.btn7Dias = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblRentabilidad = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCosto = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pieChartRubros = new LiveCharts.WinForms.PieChart();
            this.pieChartProveedores = new LiveCharts.WinForms.PieChart();
            this.chartMediosPago = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblVentasProm = new System.Windows.Forms.Label();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.btnFiltro = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVentasMax = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVentasCant = new System.Windows.Forms.Label();
            this.lblVentas = new System.Windows.Forms.Label();
            this.chartSaldos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cboCliente = new System.Windows.Forms.ComboBox();
            this.cbCliente = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartMediosPago)).BeginInit();
            this.panelFiltros.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSaldos)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHoy
            // 
            this.btnHoy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnHoy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoy.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHoy.Location = new System.Drawing.Point(142, 9);
            this.btnHoy.Name = "btnHoy";
            this.btnHoy.Size = new System.Drawing.Size(139, 38);
            this.btnHoy.TabIndex = 4;
            this.btnHoy.Text = "Hoy";
            this.btnHoy.UseVisualStyleBackColor = false;
            this.btnHoy.Click += new System.EventHandler(this.btnHoy_Click);
            // 
            // btn30dias
            // 
            this.btn30dias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn30dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn30dias.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn30dias.Location = new System.Drawing.Point(418, 9);
            this.btn30dias.Name = "btn30dias";
            this.btn30dias.Size = new System.Drawing.Size(139, 38);
            this.btn30dias.TabIndex = 2;
            this.btn30dias.Text = "Ult. 30 días";
            this.btn30dias.UseVisualStyleBackColor = false;
            this.btn30dias.Click += new System.EventHandler(this.btn30dias_Click);
            // 
            // btnMes
            // 
            this.btnMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMes.Location = new System.Drawing.Point(556, 9);
            this.btnMes.Name = "btnMes";
            this.btnMes.Size = new System.Drawing.Size(139, 38);
            this.btnMes.TabIndex = 1;
            this.btnMes.Text = "Este Mes";
            this.btnMes.UseVisualStyleBackColor = false;
            this.btnMes.Click += new System.EventHandler(this.btnMes_Click);
            // 
            // btnMesPasado
            // 
            this.btnMesPasado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMesPasado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMesPasado.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMesPasado.Location = new System.Drawing.Point(694, 9);
            this.btnMesPasado.Name = "btnMesPasado";
            this.btnMesPasado.Size = new System.Drawing.Size(139, 38);
            this.btnMesPasado.TabIndex = 0;
            this.btnMesPasado.Text = "Mes Pasado";
            this.btnMesPasado.UseVisualStyleBackColor = false;
            this.btnMesPasado.Click += new System.EventHandler(this.btnMesPasado_Click);
            // 
            // cartesianChartVentasPorDia
            // 
            this.cartesianChartVentasPorDia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cartesianChartVentasPorDia.Location = new System.Drawing.Point(10, 276);
            this.cartesianChartVentasPorDia.Name = "cartesianChartVentasPorDia";
            this.cartesianChartVentasPorDia.Size = new System.Drawing.Size(913, 264);
            this.cartesianChartVentasPorDia.TabIndex = 21;
            this.cartesianChartVentasPorDia.Text = "cartesianChart1";
            // 
            // btn7Dias
            // 
            this.btn7Dias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn7Dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn7Dias.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7Dias.Location = new System.Drawing.Point(280, 9);
            this.btn7Dias.Name = "btn7Dias";
            this.btn7Dias.Size = new System.Drawing.Size(139, 38);
            this.btn7Dias.TabIndex = 3;
            this.btn7Dias.Text = "Ult. 7 días";
            this.btn7Dias.UseVisualStyleBackColor = false;
            this.btn7Dias.Click += new System.EventHandler(this.btn7Dias_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(1267, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 24);
            this.label6.TabIndex = 11;
            this.label6.Text = "      Ganancias";
            // 
            // lblRentabilidad
            // 
            this.lblRentabilidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.lblRentabilidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRentabilidad.Location = new System.Drawing.Point(1254, 16);
            this.lblRentabilidad.Name = "lblRentabilidad";
            this.lblRentabilidad.Size = new System.Drawing.Size(155, 74);
            this.lblRentabilidad.TabIndex = 10;
            this.lblRentabilidad.Text = "Ventas Hoy";
            this.lblRentabilidad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(1034, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "      Costos";
            // 
            // lblCosto
            // 
            this.lblCosto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblCosto.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCosto.Location = new System.Drawing.Point(1005, 16);
            this.lblCosto.Name = "lblCosto";
            this.lblCosto.Size = new System.Drawing.Size(155, 74);
            this.lblCosto.TabIndex = 8;
            this.lblCosto.Text = "Ventas Hoy";
            this.lblCosto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(772, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "      Promedio";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label11.Location = new System.Drawing.Point(439, 554);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(249, 20);
            this.label11.TabIndex = 29;
            this.label11.Text = "Ventas x Productos de  Proveedor";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label10.Location = new System.Drawing.Point(10, 554);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(308, 20);
            this.label10.TabIndex = 28;
            this.label10.Text = "Distribución Ventas x Cliente - Medio Pago";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.Location = new System.Drawing.Point(938, 253);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(172, 20);
            this.label9.TabIndex = 27;
            this.label9.Text = "Top 10 Saldos Clientes";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.Location = new System.Drawing.Point(10, 253);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(248, 20);
            this.label8.TabIndex = 25;
            this.label8.Text = "Distribución Ventas x Cliente - Dia";
            // 
            // pieChartRubros
            // 
            this.pieChartRubros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartRubros.Location = new System.Drawing.Point(938, 577);
            this.pieChartRubros.Name = "pieChartRubros";
            this.pieChartRubros.Size = new System.Drawing.Size(481, 300);
            this.pieChartRubros.TabIndex = 26;
            this.pieChartRubros.Text = "pieChart2";
            // 
            // pieChartProveedores
            // 
            this.pieChartProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartProveedores.Location = new System.Drawing.Point(439, 577);
            this.pieChartProveedores.Name = "pieChartProveedores";
            this.pieChartProveedores.Size = new System.Drawing.Size(481, 300);
            this.pieChartProveedores.TabIndex = 24;
            this.pieChartProveedores.Text = "pieChart2";
            // 
            // chartMediosPago
            // 
            this.chartMediosPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chartMediosPago.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            this.chartMediosPago.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartMediosPago.Legends.Add(legend1);
            this.chartMediosPago.Location = new System.Drawing.Point(10, 577);
            this.chartMediosPago.Name = "chartMediosPago";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartMediosPago.Series.Add(series1);
            this.chartMediosPago.Size = new System.Drawing.Size(411, 300);
            this.chartMediosPago.TabIndex = 23;
            this.chartMediosPago.Text = "chart1";
            // 
            // lblVentasProm
            // 
            this.lblVentasProm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblVentasProm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasProm.Location = new System.Drawing.Point(756, 16);
            this.lblVentasProm.Name = "lblVentasProm";
            this.lblVentasProm.Size = new System.Drawing.Size(155, 74);
            this.lblVentasProm.TabIndex = 6;
            this.lblVentasProm.Text = "Ventas Hoy";
            this.lblVentasProm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelFiltros
            // 
            this.panelFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panelFiltros.Controls.Add(this.btnFiltro);
            this.panelFiltros.Controls.Add(this.btnHoy);
            this.panelFiltros.Controls.Add(this.btn7Dias);
            this.panelFiltros.Controls.Add(this.btn30dias);
            this.panelFiltros.Controls.Add(this.btnMes);
            this.panelFiltros.Controls.Add(this.btnMesPasado);
            this.panelFiltros.Location = new System.Drawing.Point(576, 3);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Size = new System.Drawing.Size(844, 58);
            this.panelFiltros.TabIndex = 14;
            // 
            // btnFiltro
            // 
            this.btnFiltro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnFiltro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltro.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltro.Location = new System.Drawing.Point(9, 9);
            this.btnFiltro.Name = "btnFiltro";
            this.btnFiltro.Size = new System.Drawing.Size(139, 38);
            this.btnFiltro.TabIndex = 5;
            this.btnFiltro.Text = "Filtro";
            this.btnFiltro.UseVisualStyleBackColor = false;
            this.btnFiltro.Click += new System.EventHandler(this.btnFiltro_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.cboCliente);
            this.panel2.Controls.Add(this.cbCliente);
            this.panel2.Controls.Add(this.panelFiltros);
            this.panel2.Controls.Add(this.dtpDesde);
            this.panel2.Controls.Add(this.dtpHasta);
            this.panel2.Location = new System.Drawing.Point(10, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1423, 66);
            this.panel2.TabIndex = 20;
            // 
            // dtpDesde
            // 
            this.dtpDesde.CustomFormat = "\"dd/MM/yyyy\"";
            this.dtpDesde.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpDesde.Location = new System.Drawing.Point(19, 8);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(228, 20);
            this.dtpDesde.TabIndex = 10;
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(262, 8);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(228, 20);
            this.dtpHasta.TabIndex = 12;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Image = ((System.Drawing.Image)(resources.GetObject("label16.Image")));
            this.label16.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label16.Location = new System.Drawing.Point(51, 103);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(71, 24);
            this.label16.TabIndex = 0;
            this.label16.Text = "    Total";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Location = new System.Drawing.Point(938, 554);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(217, 20);
            this.label12.TabIndex = 30;
            this.label12.Text = "Ventas de Productos x Rubro";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lblRentabilidad);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lblCosto);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblVentasProm);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.lblVentasMax);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblVentasCant);
            this.panel1.Controls.Add(this.lblVentas);
            this.panel1.Location = new System.Drawing.Point(10, 94);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1423, 147);
            this.panel1.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(281, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "     Cantidad";
            // 
            // lblVentasMax
            // 
            this.lblVentasMax.BackColor = System.Drawing.Color.Lime;
            this.lblVentasMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasMax.Location = new System.Drawing.Point(507, 16);
            this.lblVentasMax.Name = "lblVentasMax";
            this.lblVentasMax.Size = new System.Drawing.Size(155, 74);
            this.lblVentasMax.TabIndex = 5;
            this.lblVentasMax.Text = "Ventas Hoy";
            this.lblVentasMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(51, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "    Total";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(527, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "      Pickit Dia";
            // 
            // lblVentasCant
            // 
            this.lblVentasCant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblVentasCant.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasCant.Location = new System.Drawing.Point(258, 16);
            this.lblVentasCant.Name = "lblVentasCant";
            this.lblVentasCant.Size = new System.Drawing.Size(155, 74);
            this.lblVentasCant.TabIndex = 3;
            this.lblVentasCant.Text = "Ventas Hoy";
            this.lblVentasCant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVentas
            // 
            this.lblVentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblVentas.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentas.Location = new System.Drawing.Point(9, 16);
            this.lblVentas.Name = "lblVentas";
            this.lblVentas.Size = new System.Drawing.Size(155, 74);
            this.lblVentas.TabIndex = 1;
            this.lblVentas.Text = "Ventas Hoy";
            this.lblVentas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chartSaldos
            // 
            this.chartSaldos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chartSaldos.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea2.Name = "ChartArea1";
            this.chartSaldos.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartSaldos.Legends.Add(legend2);
            this.chartSaldos.Location = new System.Drawing.Point(938, 276);
            this.chartSaldos.Name = "chartSaldos";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartSaldos.Series.Add(series2);
            this.chartSaldos.Size = new System.Drawing.Size(481, 264);
            this.chartSaldos.TabIndex = 31;
            this.chartSaldos.Text = "chart1";
            // 
            // cboCliente
            // 
            this.cboCliente.FormattingEnabled = true;
            this.cboCliente.Location = new System.Drawing.Point(130, 38);
            this.cboCliente.Name = "cboCliente";
            this.cboCliente.Size = new System.Drawing.Size(360, 21);
            this.cboCliente.TabIndex = 16;
            // 
            // cbCliente
            // 
            this.cbCliente.AutoSize = true;
            this.cbCliente.Location = new System.Drawing.Point(19, 40);
            this.cbCliente.Name = "cbCliente";
            this.cbCliente.Size = new System.Drawing.Size(105, 17);
            this.cbCliente.TabIndex = 15;
            this.cbCliente.Text = "Filtrar Por Cliente";
            this.cbCliente.UseVisualStyleBackColor = true;
            // 
            // frmDashboardClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1442, 888);
            this.Controls.Add(this.chartSaldos);
            this.Controls.Add(this.cartesianChartVentasPorDia);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pieChartRubros);
            this.Controls.Add(this.pieChartProveedores);
            this.Controls.Add(this.chartMediosPago);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDashboardClientes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard Clientes";
            this.Load += new System.EventHandler(this.frmDashboardClientes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartMediosPago)).EndInit();
            this.panelFiltros.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSaldos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHoy;
        private System.Windows.Forms.Button btn30dias;
        private System.Windows.Forms.Button btnMes;
        private System.Windows.Forms.Button btnMesPasado;
        private LiveCharts.WinForms.CartesianChart cartesianChartVentasPorDia;
        private System.Windows.Forms.Button btn7Dias;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblRentabilidad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCosto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private LiveCharts.WinForms.PieChart pieChartRubros;
        private LiveCharts.WinForms.PieChart pieChartProveedores;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMediosPago;
        private System.Windows.Forms.Label lblVentasProm;
        private System.Windows.Forms.Panel panelFiltros;
        private System.Windows.Forms.Button btnFiltro;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVentasMax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblVentasCant;
        private System.Windows.Forms.Label lblVentas;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSaldos;
        private System.Windows.Forms.ComboBox cboCliente;
        private System.Windows.Forms.CheckBox cbCliente;
    }
}