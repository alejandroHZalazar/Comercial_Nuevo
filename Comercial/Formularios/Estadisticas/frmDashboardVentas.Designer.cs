
namespace Comercial.Formularios.Estadisticas
{
    partial class frmDashboardVentas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDashboardVentas));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.btnFiltro = new System.Windows.Forms.Button();
            this.btnHoy = new System.Windows.Forms.Button();
            this.btn7Dias = new System.Windows.Forms.Button();
            this.btn30dias = new System.Windows.Forms.Button();
            this.btnMes = new System.Windows.Forms.Button();
            this.btnMesPasado = new System.Windows.Forms.Button();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tlpKpi = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTile1 = new System.Windows.Forms.Panel();
            this.lblVentas = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTile2 = new System.Windows.Forms.Panel();
            this.lblVentasCant = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlTile3 = new System.Windows.Forms.Panel();
            this.lblVentasMax = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlTile4 = new System.Windows.Forms.Panel();
            this.lblVentasProm = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlTile5 = new System.Windows.Forms.Panel();
            this.lblCosto = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlTile6 = new System.Windows.Forms.Panel();
            this.lblRentabilidad = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlTile7 = new System.Windows.Forms.Panel();
            this.lblCompras = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tlpAcciones = new System.Windows.Forms.TableLayoutPanel();
            this.btnEstadisticasProveedores = new System.Windows.Forms.Button();
            this.btnEstadisticasCLientes = new System.Windows.Forms.Button();
            this.tlpCharts = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cartesianChartVentasPorDia = new LiveCharts.WinForms.CartesianChart();
            this.pieChartVentasPorCliente = new LiveCharts.WinForms.PieChart();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chartMediosPago = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pieChartProveedores = new LiveCharts.WinForms.PieChart();
            this.pieChartRubros = new LiveCharts.WinForms.PieChart();
            this.tlpMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelFiltros.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tlpKpi.SuspendLayout();
            this.pnlTile1.SuspendLayout();
            this.pnlTile2.SuspendLayout();
            this.pnlTile3.SuspendLayout();
            this.pnlTile4.SuspendLayout();
            this.pnlTile5.SuspendLayout();
            this.pnlTile6.SuspendLayout();
            this.pnlTile7.SuspendLayout();
            this.tlpAcciones.SuspendLayout();
            this.tlpCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMediosPago)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.panel2, 0, 0);
            this.tlpMain.Controls.Add(this.panel1, 0, 1);
            this.tlpMain.Controls.Add(this.tlpCharts, 0, 2);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.Padding = new System.Windows.Forms.Padding(10);
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(1240, 760);
            this.tlpMain.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.panelFiltros);
            this.panel2.Controls.Add(this.dtpDesde);
            this.panel2.Controls.Add(this.dtpHasta);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(13, 13);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1214, 63);
            this.panel2.TabIndex = 7;
            // 
            // panelFiltros
            // 
            this.panelFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panelFiltros.Controls.Add(this.btnFiltro);
            this.panelFiltros.Controls.Add(this.btnHoy);
            this.panelFiltros.Controls.Add(this.btn7Dias);
            this.panelFiltros.Controls.Add(this.btn30dias);
            this.panelFiltros.Controls.Add(this.btnMes);
            this.panelFiltros.Controls.Add(this.btnMesPasado);
            this.panelFiltros.Location = new System.Drawing.Point(518, 3);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Size = new System.Drawing.Size(690, 56);
            this.panelFiltros.TabIndex = 14;
            // 
            // btnFiltro
            // 
            this.btnFiltro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnFiltro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltro.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltro.Location = new System.Drawing.Point(6, 9);
            this.btnFiltro.Name = "btnFiltro";
            this.btnFiltro.Size = new System.Drawing.Size(108, 38);
            this.btnFiltro.TabIndex = 5;
            this.btnFiltro.Text = "Filtro";
            this.btnFiltro.UseVisualStyleBackColor = false;
            this.btnFiltro.Click += new System.EventHandler(this.btnFiltro_Click);
            // 
            // btnHoy
            // 
            this.btnHoy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnHoy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoy.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHoy.Location = new System.Drawing.Point(120, 9);
            this.btnHoy.Name = "btnHoy";
            this.btnHoy.Size = new System.Drawing.Size(108, 38);
            this.btnHoy.TabIndex = 4;
            this.btnHoy.Text = "Hoy";
            this.btnHoy.UseVisualStyleBackColor = false;
            this.btnHoy.Click += new System.EventHandler(this.btnHoy_Click);
            // 
            // btn7Dias
            // 
            this.btn7Dias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn7Dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn7Dias.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7Dias.Location = new System.Drawing.Point(234, 9);
            this.btn7Dias.Name = "btn7Dias";
            this.btn7Dias.Size = new System.Drawing.Size(108, 38);
            this.btn7Dias.TabIndex = 3;
            this.btn7Dias.Text = "Ult. 7 días";
            this.btn7Dias.UseVisualStyleBackColor = false;
            this.btn7Dias.Click += new System.EventHandler(this.btn7Dias_Click);
            // 
            // btn30dias
            // 
            this.btn30dias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn30dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn30dias.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn30dias.Location = new System.Drawing.Point(348, 9);
            this.btn30dias.Name = "btn30dias";
            this.btn30dias.Size = new System.Drawing.Size(108, 38);
            this.btn30dias.TabIndex = 2;
            this.btn30dias.Text = "Ult. 30 días";
            this.btn30dias.UseVisualStyleBackColor = false;
            this.btn30dias.Click += new System.EventHandler(this.btn30dias_Click);
            // 
            // btnMes
            // 
            this.btnMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMes.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMes.Location = new System.Drawing.Point(462, 9);
            this.btnMes.Name = "btnMes";
            this.btnMes.Size = new System.Drawing.Size(108, 38);
            this.btnMes.TabIndex = 1;
            this.btnMes.Text = "Este Mes";
            this.btnMes.UseVisualStyleBackColor = false;
            this.btnMes.Click += new System.EventHandler(this.btnMes_Click);
            // 
            // btnMesPasado
            // 
            this.btnMesPasado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMesPasado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMesPasado.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMesPasado.Location = new System.Drawing.Point(576, 9);
            this.btnMesPasado.Name = "btnMesPasado";
            this.btnMesPasado.Size = new System.Drawing.Size(108, 38);
            this.btnMesPasado.TabIndex = 0;
            this.btnMesPasado.Text = "Mes Pasado";
            this.btnMesPasado.UseVisualStyleBackColor = false;
            this.btnMesPasado.Click += new System.EventHandler(this.btnMesPasado_Click);
            // 
            // dtpDesde
            // 
            this.dtpDesde.CustomFormat = "\"dd/MM/yyyy\"";
            this.dtpDesde.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpDesde.Location = new System.Drawing.Point(19, 20);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(228, 23);
            this.dtpDesde.TabIndex = 10;
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(262, 20);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(228, 23);
            this.dtpHasta.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.tlpKpi);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(13, 85);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1214, 141);
            this.panel1.TabIndex = 6;
            // 
            // tlpKpi
            // 
            this.tlpKpi.ColumnCount = 8;
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 206F));
            this.tlpKpi.Controls.Add(this.pnlTile1, 0, 0);
            this.tlpKpi.Controls.Add(this.pnlTile2, 1, 0);
            this.tlpKpi.Controls.Add(this.pnlTile3, 2, 0);
            this.tlpKpi.Controls.Add(this.pnlTile4, 3, 0);
            this.tlpKpi.Controls.Add(this.pnlTile5, 4, 0);
            this.tlpKpi.Controls.Add(this.pnlTile6, 5, 0);
            this.tlpKpi.Controls.Add(this.pnlTile7, 6, 0);
            this.tlpKpi.Controls.Add(this.tlpAcciones, 7, 0);
            this.tlpKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKpi.Location = new System.Drawing.Point(0, 0);
            this.tlpKpi.Name = "tlpKpi";
            this.tlpKpi.RowCount = 1;
            this.tlpKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKpi.Size = new System.Drawing.Size(1214, 141);
            this.tlpKpi.TabIndex = 0;
            // 
            // pnlTile1
            // 
            this.pnlTile1.Controls.Add(this.lblVentas);
            this.pnlTile1.Controls.Add(this.label16);
            this.pnlTile1.Controls.Add(this.label1);
            this.pnlTile1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile1.Location = new System.Drawing.Point(8, 8);
            this.pnlTile1.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile1.Name = "pnlTile1";
            this.pnlTile1.Size = new System.Drawing.Size(127, 125);
            this.pnlTile1.TabIndex = 0;
            // 
            // lblVentas
            // 
            this.lblVentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblVentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVentas.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentas.Location = new System.Drawing.Point(0, 0);
            this.lblVentas.Name = "lblVentas";
            this.lblVentas.Size = new System.Drawing.Size(127, 95);
            this.lblVentas.TabIndex = 1;
            this.lblVentas.Text = "Ventas Hoy";
            this.lblVentas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(0, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 23);
            this.label16.TabIndex = 2;
            this.label16.Text = "Total";
            this.label16.Visible = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(0, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile2
            // 
            this.pnlTile2.Controls.Add(this.lblVentasCant);
            this.pnlTile2.Controls.Add(this.label18);
            this.pnlTile2.Controls.Add(this.label2);
            this.pnlTile2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile2.Location = new System.Drawing.Point(151, 8);
            this.pnlTile2.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile2.Name = "pnlTile2";
            this.pnlTile2.Size = new System.Drawing.Size(127, 125);
            this.pnlTile2.TabIndex = 1;
            // 
            // lblVentasCant
            // 
            this.lblVentasCant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblVentasCant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVentasCant.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasCant.Location = new System.Drawing.Point(0, 0);
            this.lblVentasCant.Name = "lblVentasCant";
            this.lblVentasCant.Size = new System.Drawing.Size(127, 95);
            this.lblVentasCant.TabIndex = 3;
            this.lblVentasCant.Text = "Ventas Hoy";
            this.lblVentasCant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(0, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(100, 23);
            this.label18.TabIndex = 4;
            this.label18.Text = "Cantidad";
            this.label18.Visible = false;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(0, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cantidad";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile3
            // 
            this.pnlTile3.Controls.Add(this.lblVentasMax);
            this.pnlTile3.Controls.Add(this.label15);
            this.pnlTile3.Controls.Add(this.label3);
            this.pnlTile3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile3.Location = new System.Drawing.Point(294, 8);
            this.pnlTile3.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile3.Name = "pnlTile3";
            this.pnlTile3.Size = new System.Drawing.Size(127, 125);
            this.pnlTile3.TabIndex = 2;
            // 
            // lblVentasMax
            // 
            this.lblVentasMax.BackColor = System.Drawing.Color.Lime;
            this.lblVentasMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVentasMax.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasMax.Location = new System.Drawing.Point(0, 0);
            this.lblVentasMax.Name = "lblVentasMax";
            this.lblVentasMax.Size = new System.Drawing.Size(127, 95);
            this.lblVentasMax.TabIndex = 5;
            this.lblVentasMax.Text = "Ventas Hoy";
            this.lblVentasMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(0, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 23);
            this.label15.TabIndex = 6;
            this.label15.Text = "Pickit Dia";
            this.label15.Visible = false;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(0, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 30);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pickit Dia";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile4
            // 
            this.pnlTile4.Controls.Add(this.lblVentasProm);
            this.pnlTile4.Controls.Add(this.label20);
            this.pnlTile4.Controls.Add(this.label4);
            this.pnlTile4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile4.Location = new System.Drawing.Point(437, 8);
            this.pnlTile4.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile4.Name = "pnlTile4";
            this.pnlTile4.Size = new System.Drawing.Size(127, 125);
            this.pnlTile4.TabIndex = 3;
            // 
            // lblVentasProm
            // 
            this.lblVentasProm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblVentasProm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVentasProm.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasProm.Location = new System.Drawing.Point(0, 0);
            this.lblVentasProm.Name = "lblVentasProm";
            this.lblVentasProm.Size = new System.Drawing.Size(127, 95);
            this.lblVentasProm.TabIndex = 6;
            this.lblVentasProm.Text = "Ventas Hoy";
            this.lblVentasProm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(0, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 23);
            this.label20.TabIndex = 7;
            this.label20.Text = "Promedio";
            this.label20.Visible = false;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(0, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 30);
            this.label4.TabIndex = 7;
            this.label4.Text = "Promedio";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile5
            // 
            this.pnlTile5.Controls.Add(this.lblCosto);
            this.pnlTile5.Controls.Add(this.label22);
            this.pnlTile5.Controls.Add(this.label5);
            this.pnlTile5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile5.Location = new System.Drawing.Point(580, 8);
            this.pnlTile5.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile5.Name = "pnlTile5";
            this.pnlTile5.Size = new System.Drawing.Size(127, 125);
            this.pnlTile5.TabIndex = 4;
            // 
            // lblCosto
            // 
            this.lblCosto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblCosto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCosto.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCosto.Location = new System.Drawing.Point(0, 0);
            this.lblCosto.Name = "lblCosto";
            this.lblCosto.Size = new System.Drawing.Size(127, 95);
            this.lblCosto.TabIndex = 8;
            this.lblCosto.Text = "Ventas Hoy";
            this.lblCosto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(0, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(100, 23);
            this.label22.TabIndex = 9;
            this.label22.Text = "Costos";
            this.label22.Visible = false;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = ((System.Drawing.Image)(resources.GetObject("label5.Image")));
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(0, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 30);
            this.label5.TabIndex = 9;
            this.label5.Text = "Costos";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile6
            // 
            this.pnlTile6.Controls.Add(this.lblRentabilidad);
            this.pnlTile6.Controls.Add(this.label24);
            this.pnlTile6.Controls.Add(this.label6);
            this.pnlTile6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile6.Location = new System.Drawing.Point(723, 8);
            this.pnlTile6.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile6.Name = "pnlTile6";
            this.pnlTile6.Size = new System.Drawing.Size(127, 125);
            this.pnlTile6.TabIndex = 5;
            // 
            // lblRentabilidad
            // 
            this.lblRentabilidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.lblRentabilidad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRentabilidad.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRentabilidad.Location = new System.Drawing.Point(0, 0);
            this.lblRentabilidad.Name = "lblRentabilidad";
            this.lblRentabilidad.Size = new System.Drawing.Size(127, 95);
            this.lblRentabilidad.TabIndex = 10;
            this.lblRentabilidad.Text = "Ventas Hoy";
            this.lblRentabilidad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(0, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(100, 23);
            this.label24.TabIndex = 11;
            this.label24.Text = "Ganancias";
            this.label24.Visible = false;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(0, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 30);
            this.label6.TabIndex = 11;
            this.label6.Text = "Ganancias";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile7
            // 
            this.pnlTile7.Controls.Add(this.lblCompras);
            this.pnlTile7.Controls.Add(this.label26);
            this.pnlTile7.Controls.Add(this.label7);
            this.pnlTile7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile7.Location = new System.Drawing.Point(866, 8);
            this.pnlTile7.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile7.Name = "pnlTile7";
            this.pnlTile7.Size = new System.Drawing.Size(127, 125);
            this.pnlTile7.TabIndex = 6;
            // 
            // lblCompras
            // 
            this.lblCompras.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblCompras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCompras.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompras.Location = new System.Drawing.Point(0, 0);
            this.lblCompras.Name = "lblCompras";
            this.lblCompras.Size = new System.Drawing.Size(127, 95);
            this.lblCompras.TabIndex = 12;
            this.lblCompras.Text = "Ventas Hoy";
            this.lblCompras.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(0, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(100, 23);
            this.label26.TabIndex = 13;
            this.label26.Text = "Compras";
            this.label26.Visible = false;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Location = new System.Drawing.Point(0, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 30);
            this.label7.TabIndex = 13;
            this.label7.Text = "Compras";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpAcciones
            // 
            this.tlpAcciones.ColumnCount = 1;
            this.tlpAcciones.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAcciones.Controls.Add(this.btnEstadisticasProveedores, 0, 0);
            this.tlpAcciones.Controls.Add(this.btnEstadisticasCLientes, 0, 1);
            this.tlpAcciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAcciones.Location = new System.Drawing.Point(1009, 8);
            this.tlpAcciones.Margin = new System.Windows.Forms.Padding(8);
            this.tlpAcciones.Name = "tlpAcciones";
            this.tlpAcciones.RowCount = 2;
            this.tlpAcciones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAcciones.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAcciones.Size = new System.Drawing.Size(197, 125);
            this.tlpAcciones.TabIndex = 7;
            // 
            // btnEstadisticasProveedores
            // 
            this.btnEstadisticasProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnEstadisticasProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEstadisticasProveedores.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadisticasProveedores.ForeColor = System.Drawing.Color.White;
            this.btnEstadisticasProveedores.Image = ((System.Drawing.Image)(resources.GetObject("btnEstadisticasProveedores.Image")));
            this.btnEstadisticasProveedores.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEstadisticasProveedores.Location = new System.Drawing.Point(0, 0);
            this.btnEstadisticasProveedores.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.btnEstadisticasProveedores.Name = "btnEstadisticasProveedores";
            this.btnEstadisticasProveedores.Size = new System.Drawing.Size(197, 59);
            this.btnEstadisticasProveedores.TabIndex = 14;
            this.btnEstadisticasProveedores.Text = "    Estadísticas Proveedores";
            this.btnEstadisticasProveedores.UseVisualStyleBackColor = false;
            this.btnEstadisticasProveedores.Click += new System.EventHandler(this.btnEstadisticasProveedores_Click);
            // 
            // btnEstadisticasCLientes
            // 
            this.btnEstadisticasCLientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnEstadisticasCLientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEstadisticasCLientes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadisticasCLientes.ForeColor = System.Drawing.Color.Yellow;
            this.btnEstadisticasCLientes.Image = ((System.Drawing.Image)(resources.GetObject("btnEstadisticasCLientes.Image")));
            this.btnEstadisticasCLientes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEstadisticasCLientes.Location = new System.Drawing.Point(0, 65);
            this.btnEstadisticasCLientes.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.btnEstadisticasCLientes.Name = "btnEstadisticasCLientes";
            this.btnEstadisticasCLientes.Size = new System.Drawing.Size(197, 60);
            this.btnEstadisticasCLientes.TabIndex = 15;
            this.btnEstadisticasCLientes.Text = "    Estadísticas Clientes";
            this.btnEstadisticasCLientes.UseVisualStyleBackColor = false;
            this.btnEstadisticasCLientes.Click += new System.EventHandler(this.btnEstadisticasCLientes_Click);
            // 
            // tlpCharts
            // 
            this.tlpCharts.ColumnCount = 6;
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpCharts.Controls.Add(this.label8, 0, 0);
            this.tlpCharts.Controls.Add(this.label9, 4, 0);
            this.tlpCharts.Controls.Add(this.cartesianChartVentasPorDia, 0, 1);
            this.tlpCharts.Controls.Add(this.pieChartVentasPorCliente, 4, 1);
            this.tlpCharts.Controls.Add(this.label10, 0, 2);
            this.tlpCharts.Controls.Add(this.label11, 2, 2);
            this.tlpCharts.Controls.Add(this.label12, 4, 2);
            this.tlpCharts.Controls.Add(this.chartMediosPago, 0, 3);
            this.tlpCharts.Controls.Add(this.pieChartProveedores, 2, 3);
            this.tlpCharts.Controls.Add(this.pieChartRubros, 4, 3);
            this.tlpCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCharts.Location = new System.Drawing.Point(13, 235);
            this.tlpCharts.Name = "tlpCharts";
            this.tlpCharts.RowCount = 4;
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.Size = new System.Drawing.Size(1214, 512);
            this.tlpCharts.TabIndex = 21;
            // 
            // label8
            // 
            this.tlpCharts.SetColumnSpan(this.label8, 4);
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(802, 26);
            this.label8.TabIndex = 14;
            this.label8.Text = "Distribución Ventas x Dia";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.tlpCharts.SetColumnSpan(this.label9, 2);
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(811, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(400, 26);
            this.label9.TabIndex = 15;
            this.label9.Text = "Top 10 Distribución Ventas x Cliente";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cartesianChartVentasPorDia
            // 
            this.cartesianChartVentasPorDia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tlpCharts.SetColumnSpan(this.cartesianChartVentasPorDia, 4);
            this.cartesianChartVentasPorDia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChartVentasPorDia.Location = new System.Drawing.Point(3, 29);
            this.cartesianChartVentasPorDia.Name = "cartesianChartVentasPorDia";
            this.cartesianChartVentasPorDia.Size = new System.Drawing.Size(802, 224);
            this.cartesianChartVentasPorDia.TabIndex = 8;
            this.cartesianChartVentasPorDia.Text = "cartesianChart1";
            // 
            // pieChartVentasPorCliente
            // 
            this.pieChartVentasPorCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tlpCharts.SetColumnSpan(this.pieChartVentasPorCliente, 2);
            this.pieChartVentasPorCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieChartVentasPorCliente.ForeColor = System.Drawing.Color.White;
            this.pieChartVentasPorCliente.Location = new System.Drawing.Point(811, 29);
            this.pieChartVentasPorCliente.Name = "pieChartVentasPorCliente";
            this.pieChartVentasPorCliente.Size = new System.Drawing.Size(400, 224);
            this.pieChartVentasPorCliente.TabIndex = 9;
            this.pieChartVentasPorCliente.Text = "pieChart1";
            // 
            // label10
            // 
            this.tlpCharts.SetColumnSpan(this.label10, 2);
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 256);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(398, 26);
            this.label10.TabIndex = 16;
            this.label10.Text = "Distribución Ventas x Medio Pago";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.tlpCharts.SetColumnSpan(this.label11, 2);
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(407, 256);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(398, 26);
            this.label11.TabIndex = 17;
            this.label11.Text = "Ventas x Productos de  Proveedor";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.tlpCharts.SetColumnSpan(this.label12, 2);
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(811, 256);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(400, 26);
            this.label12.TabIndex = 18;
            this.label12.Text = "Ventas de Productos x Rubro";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chartMediosPago
            // 
            this.chartMediosPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chartMediosPago.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            this.chartMediosPago.ChartAreas.Add(chartArea1);
            this.tlpCharts.SetColumnSpan(this.chartMediosPago, 2);
            this.chartMediosPago.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartMediosPago.Legends.Add(legend1);
            this.chartMediosPago.Location = new System.Drawing.Point(3, 285);
            this.chartMediosPago.Name = "chartMediosPago";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartMediosPago.Series.Add(series1);
            this.chartMediosPago.Size = new System.Drawing.Size(398, 224);
            this.chartMediosPago.TabIndex = 10;
            this.chartMediosPago.Text = "chart1";
            // 
            // pieChartProveedores
            // 
            this.pieChartProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tlpCharts.SetColumnSpan(this.pieChartProveedores, 2);
            this.pieChartProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieChartProveedores.Location = new System.Drawing.Point(407, 285);
            this.pieChartProveedores.Name = "pieChartProveedores";
            this.pieChartProveedores.Size = new System.Drawing.Size(398, 224);
            this.pieChartProveedores.TabIndex = 13;
            this.pieChartProveedores.Text = "pieChart2";
            // 
            // pieChartRubros
            // 
            this.pieChartRubros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tlpCharts.SetColumnSpan(this.pieChartRubros, 2);
            this.pieChartRubros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieChartRubros.Location = new System.Drawing.Point(811, 285);
            this.pieChartRubros.Name = "pieChartRubros";
            this.pieChartRubros.Size = new System.Drawing.Size(400, 224);
            this.pieChartRubros.TabIndex = 14;
            this.pieChartRubros.Text = "pieChart2";
            // 
            // frmDashboardVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1220, 720);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1240, 760);
            this.Controls.Add(this.tlpMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmDashboardVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard Ventas";
            this.Load += new System.EventHandler(this.frmDashboardVentas_Load);
            this.tlpMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panelFiltros.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tlpKpi.ResumeLayout(false);
            this.pnlTile1.ResumeLayout(false);
            this.pnlTile2.ResumeLayout(false);
            this.pnlTile3.ResumeLayout(false);
            this.pnlTile4.ResumeLayout(false);
            this.pnlTile5.ResumeLayout(false);
            this.pnlTile6.ResumeLayout(false);
            this.pnlTile7.ResumeLayout(false);
            this.tlpAcciones.ResumeLayout(false);
            this.tlpCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMediosPago)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpKpi;
        private System.Windows.Forms.TableLayoutPanel tlpCharts;
        private System.Windows.Forms.TableLayoutPanel tlpAcciones;
        private System.Windows.Forms.Panel pnlTile1;
        private System.Windows.Forms.Panel pnlTile2;
        private System.Windows.Forms.Panel pnlTile3;
        private System.Windows.Forms.Panel pnlTile4;
        private System.Windows.Forms.Panel pnlTile5;
        private System.Windows.Forms.Panel pnlTile6;
        private System.Windows.Forms.Panel pnlTile7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVentas;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVentasCant;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblVentasMax;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Panel panel2;
        private LiveCharts.WinForms.CartesianChart cartesianChartVentasPorDia;
        private LiveCharts.WinForms.PieChart pieChartVentasPorCliente;
        private System.Windows.Forms.Label lblVentasProm;
        private System.Windows.Forms.Panel panelFiltros;
        private System.Windows.Forms.Button btnFiltro;
        private System.Windows.Forms.Button btnHoy;
        private System.Windows.Forms.Button btn7Dias;
        private System.Windows.Forms.Button btn30dias;
        private System.Windows.Forms.Button btnMes;
        private System.Windows.Forms.Button btnMesPasado;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCompras;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblRentabilidad;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCosto;
        private System.Windows.Forms.Label label4;
        private LiveCharts.WinForms.PieChart pieChartProveedores;
        private LiveCharts.WinForms.PieChart pieChartRubros;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMediosPago;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnEstadisticasCLientes;
        private System.Windows.Forms.Button btnEstadisticasProveedores;
    }
}
