
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblVentas = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVentasCant = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVentasMax = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEstadisticasCLientes = new System.Windows.Forms.Button();
            this.btnEstadisticasProveedores = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lblCompras = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lblRentabilidad = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblCosto = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblVentasProm = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.btnFiltro = new System.Windows.Forms.Button();
            this.btnHoy = new System.Windows.Forms.Button();
            this.btn7Dias = new System.Windows.Forms.Button();
            this.btn30dias = new System.Windows.Forms.Button();
            this.btnMes = new System.Windows.Forms.Button();
            this.btnMesPasado = new System.Windows.Forms.Button();
            this.cartesianChartVentasPorDia = new LiveCharts.WinForms.CartesianChart();
            this.pieChartVentasPorCliente = new LiveCharts.WinForms.PieChart();
            this.pieChartProveedores = new LiveCharts.WinForms.PieChart();
            this.pieChartRubros = new LiveCharts.WinForms.PieChart();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chartMediosPago = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMediosPago)).BeginInit();
            this.SuspendLayout();
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(205, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "     Cantidad";
            // 
            // lblVentasCant
            // 
            this.lblVentasCant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblVentasCant.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasCant.Location = new System.Drawing.Point(182, 16);
            this.lblVentasCant.Name = "lblVentasCant";
            this.lblVentasCant.Size = new System.Drawing.Size(155, 74);
            this.lblVentasCant.TabIndex = 3;
            this.lblVentasCant.Text = "Ventas Hoy";
            this.lblVentasCant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(377, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "      Pickit Dia";
            // 
            // lblVentasMax
            // 
            this.lblVentasMax.BackColor = System.Drawing.Color.Lime;
            this.lblVentasMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasMax.Location = new System.Drawing.Point(357, 16);
            this.lblVentasMax.Name = "lblVentasMax";
            this.lblVentasMax.Size = new System.Drawing.Size(155, 74);
            this.lblVentasMax.TabIndex = 5;
            this.lblVentasMax.Text = "Ventas Hoy";
            this.lblVentasMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.btnEstadisticasCLientes);
            this.panel1.Controls.Add(this.btnEstadisticasProveedores);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.lblCompras);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.lblRentabilidad);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.lblCosto);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.lblVentasProm);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.lblVentasMax);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblVentasCant);
            this.panel1.Controls.Add(this.lblVentas);
            this.panel1.Location = new System.Drawing.Point(12, 94);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1423, 147);
            this.panel1.TabIndex = 6;
            // 
            // btnEstadisticasCLientes
            // 
            this.btnEstadisticasCLientes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnEstadisticasCLientes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadisticasCLientes.ForeColor = System.Drawing.Color.Yellow;
            this.btnEstadisticasCLientes.Image = ((System.Drawing.Image)(resources.GetObject("btnEstadisticasCLientes.Image")));
            this.btnEstadisticasCLientes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEstadisticasCLientes.Location = new System.Drawing.Point(1246, 75);
            this.btnEstadisticasCLientes.Name = "btnEstadisticasCLientes";
            this.btnEstadisticasCLientes.Size = new System.Drawing.Size(172, 52);
            this.btnEstadisticasCLientes.TabIndex = 15;
            this.btnEstadisticasCLientes.Text = "Estadísticas Clientes";
            this.btnEstadisticasCLientes.UseVisualStyleBackColor = false;
            this.btnEstadisticasCLientes.Click += new System.EventHandler(this.btnEstadisticasCLientes_Click);
            // 
            // btnEstadisticasProveedores
            // 
            this.btnEstadisticasProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnEstadisticasProveedores.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEstadisticasProveedores.ForeColor = System.Drawing.Color.White;
            this.btnEstadisticasProveedores.Image = ((System.Drawing.Image)(resources.GetObject("btnEstadisticasProveedores.Image")));
            this.btnEstadisticasProveedores.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEstadisticasProveedores.Location = new System.Drawing.Point(1246, 16);
            this.btnEstadisticasProveedores.Name = "btnEstadisticasProveedores";
            this.btnEstadisticasProveedores.Size = new System.Drawing.Size(172, 52);
            this.btnEstadisticasProveedores.TabIndex = 14;
            this.btnEstadisticasProveedores.Text = "Estadísticas Proveedores";
            this.btnEstadisticasProveedores.UseVisualStyleBackColor = false;
            this.btnEstadisticasProveedores.Click += new System.EventHandler(this.btnEstadisticasProveedores_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Image = ((System.Drawing.Image)(resources.GetObject("label26.Image")));
            this.label26.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label26.Location = new System.Drawing.Point(1084, 103);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(116, 24);
            this.label26.TabIndex = 13;
            this.label26.Text = "      Compras";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Image = ((System.Drawing.Image)(resources.GetObject("label7.Image")));
            this.label7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Location = new System.Drawing.Point(1084, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 24);
            this.label7.TabIndex = 13;
            this.label7.Text = "      Compras";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Image = ((System.Drawing.Image)(resources.GetObject("label24.Image")));
            this.label24.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label24.Location = new System.Drawing.Point(903, 103);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(129, 24);
            this.label24.TabIndex = 11;
            this.label24.Text = "      Ganancias";
            // 
            // lblCompras
            // 
            this.lblCompras.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblCompras.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompras.Location = new System.Drawing.Point(1065, 16);
            this.lblCompras.Name = "lblCompras";
            this.lblCompras.Size = new System.Drawing.Size(155, 74);
            this.lblCompras.TabIndex = 12;
            this.lblCompras.Text = "Ventas Hoy";
            this.lblCompras.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Image = ((System.Drawing.Image)(resources.GetObject("label6.Image")));
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(903, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 24);
            this.label6.TabIndex = 11;
            this.label6.Text = "      Ganancias";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Image = ((System.Drawing.Image)(resources.GetObject("label22.Image")));
            this.label22.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label22.Location = new System.Drawing.Point(743, 103);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(97, 24);
            this.label22.TabIndex = 9;
            this.label22.Text = "      Costos";
            // 
            // lblRentabilidad
            // 
            this.lblRentabilidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.lblRentabilidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRentabilidad.Location = new System.Drawing.Point(890, 16);
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
            this.label5.Location = new System.Drawing.Point(743, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "      Costos";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Image = ((System.Drawing.Image)(resources.GetObject("label20.Image")));
            this.label20.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label20.Location = new System.Drawing.Point(549, 103);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(122, 24);
            this.label20.TabIndex = 7;
            this.label20.Text = "      Promedio";
            // 
            // lblCosto
            // 
            this.lblCosto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblCosto.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCosto.Location = new System.Drawing.Point(714, 16);
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
            this.label4.Location = new System.Drawing.Point(549, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "      Promedio";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Image = ((System.Drawing.Image)(resources.GetObject("label18.Image")));
            this.label18.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label18.Location = new System.Drawing.Point(205, 103);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(109, 24);
            this.label18.TabIndex = 2;
            this.label18.Text = "     Cantidad";
            // 
            // lblVentasProm
            // 
            this.lblVentasProm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblVentasProm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVentasProm.Location = new System.Drawing.Point(533, 16);
            this.lblVentasProm.Name = "lblVentasProm";
            this.lblVentasProm.Size = new System.Drawing.Size(155, 74);
            this.lblVentasProm.TabIndex = 6;
            this.lblVentasProm.Text = "Ventas Hoy";
            this.lblVentasProm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Image = ((System.Drawing.Image)(resources.GetObject("label15.Image")));
            this.label15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label15.Location = new System.Drawing.Point(377, 103);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(115, 24);
            this.label15.TabIndex = 4;
            this.label15.Text = "      Pickit Dia";
            // 
            // dtpDesde
            // 
            this.dtpDesde.CustomFormat = "\"dd/MM/yyyy\"";
            this.dtpDesde.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpDesde.Location = new System.Drawing.Point(19, 22);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(228, 21);
            this.dtpDesde.TabIndex = 10;
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(262, 22);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(228, 21);
            this.dtpHasta.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.panelFiltros);
            this.panel2.Controls.Add(this.dtpDesde);
            this.panel2.Controls.Add(this.dtpHasta);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1423, 66);
            this.panel2.TabIndex = 7;
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
            this.cartesianChartVentasPorDia.Location = new System.Drawing.Point(12, 276);
            this.cartesianChartVentasPorDia.Name = "cartesianChartVentasPorDia";
            this.cartesianChartVentasPorDia.Size = new System.Drawing.Size(913, 264);
            this.cartesianChartVentasPorDia.TabIndex = 8;
            this.cartesianChartVentasPorDia.Text = "cartesianChart1";
            // 
            // pieChartVentasPorCliente
            // 
            this.pieChartVentasPorCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartVentasPorCliente.ForeColor = System.Drawing.Color.White;
            this.pieChartVentasPorCliente.Location = new System.Drawing.Point(940, 276);
            this.pieChartVentasPorCliente.Name = "pieChartVentasPorCliente";
            this.pieChartVentasPorCliente.Size = new System.Drawing.Size(481, 264);
            this.pieChartVentasPorCliente.TabIndex = 9;
            this.pieChartVentasPorCliente.Text = "pieChart1";
            // 
            // pieChartProveedores
            // 
            this.pieChartProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartProveedores.Location = new System.Drawing.Point(441, 577);
            this.pieChartProveedores.Name = "pieChartProveedores";
            this.pieChartProveedores.Size = new System.Drawing.Size(481, 300);
            this.pieChartProveedores.TabIndex = 13;
            this.pieChartProveedores.Text = "pieChart2";
            // 
            // pieChartRubros
            // 
            this.pieChartRubros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartRubros.Location = new System.Drawing.Point(940, 577);
            this.pieChartRubros.Name = "pieChartRubros";
            this.pieChartRubros.Size = new System.Drawing.Size(481, 300);
            this.pieChartRubros.TabIndex = 14;
            this.pieChartRubros.Text = "pieChart2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.Location = new System.Drawing.Point(12, 253);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(186, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "Distribución Ventas x Dia";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.Location = new System.Drawing.Point(940, 253);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(264, 20);
            this.label9.TabIndex = 15;
            this.label9.Text = "Top 10 Distribución Ventas x Cliente";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label10.Location = new System.Drawing.Point(12, 554);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(246, 20);
            this.label10.TabIndex = 16;
            this.label10.Text = "Distribución Ventas x Medio Pago";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label11.Location = new System.Drawing.Point(441, 554);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(249, 20);
            this.label11.TabIndex = 17;
            this.label11.Text = "Ventas x Productos de  Proveedor";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Location = new System.Drawing.Point(940, 554);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(217, 20);
            this.label12.TabIndex = 18;
            this.label12.Text = "Ventas de Productos x Rubro";
            // 
            // chartMediosPago
            // 
            this.chartMediosPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chartMediosPago.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            this.chartMediosPago.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartMediosPago.Legends.Add(legend1);
            this.chartMediosPago.Location = new System.Drawing.Point(12, 577);
            this.chartMediosPago.Name = "chartMediosPago";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartMediosPago.Series.Add(series1);
            this.chartMediosPago.Size = new System.Drawing.Size(411, 300);
            this.chartMediosPago.TabIndex = 10;
            this.chartMediosPago.Text = "chart1";
            // 
            // frmDashboardVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1442, 888);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pieChartRubros);
            this.Controls.Add(this.pieChartProveedores);
            this.Controls.Add(this.chartMediosPago);
            this.Controls.Add(this.pieChartVentasPorCliente);
            this.Controls.Add(this.cartesianChartVentasPorDia);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDashboardVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard Ventas";
            this.Load += new System.EventHandler(this.frmDashboardVentas_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panelFiltros.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMediosPago)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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