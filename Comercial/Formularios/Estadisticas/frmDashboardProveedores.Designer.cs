
namespace Comercial.Formularios.Estadisticas
{
    partial class frmDashboardProveedores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDashboardProveedores));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.cbProveedor = new System.Windows.Forms.CheckBox();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.panelFiltros = new System.Windows.Forms.TableLayoutPanel();
            this.btnFiltro = new System.Windows.Forms.Button();
            this.btnHoy = new System.Windows.Forms.Button();
            this.btn7Dias = new System.Windows.Forms.Button();
            this.btn30dias = new System.Windows.Forms.Button();
            this.btnMes = new System.Windows.Forms.Button();
            this.btnMesPasado = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tlpKpi = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTile1 = new System.Windows.Forms.Panel();
            this.lblCompras = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTile2 = new System.Windows.Forms.Panel();
            this.lblComprasCant = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlTile3 = new System.Windows.Forms.Panel();
            this.lblComprasMax = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlTile4 = new System.Windows.Forms.Panel();
            this.lblComprasProm = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tlpCharts = new System.Windows.Forms.TableLayoutPanel();
            this.cartesianChartComprasPorDia = new LiveCharts.WinForms.CartesianChart();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
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
            this.tlpCharts.SuspendLayout();
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
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(1040, 760);
            this.tlpMain.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.cboProveedor);
            this.panel2.Controls.Add(this.cbProveedor);
            this.panel2.Controls.Add(this.dtpDesde);
            this.panel2.Controls.Add(this.dtpHasta);
            this.panel2.Controls.Add(this.panelFiltros);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(13, 13);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1014, 105);
            this.panel2.TabIndex = 20;
            // 
            // cboProveedor
            // 
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(167, 68);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(306, 23);
            this.cboProveedor.TabIndex = 21;
            // 
            // cbProveedor
            // 
            this.cbProveedor.AutoSize = true;
            this.cbProveedor.Location = new System.Drawing.Point(23, 70);
            this.cbProveedor.Name = "cbProveedor";
            this.cbProveedor.Size = new System.Drawing.Size(134, 19);
            this.cbProveedor.TabIndex = 20;
            this.cbProveedor.Text = "Filtrar Por Proveedor";
            this.cbProveedor.UseVisualStyleBackColor = true;
            // 
            // dtpDesde
            // 
            this.dtpDesde.CustomFormat = "\"dd/MM/yyyy\"";
            this.dtpDesde.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpDesde.Location = new System.Drawing.Point(499, 68);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(228, 23);
            this.dtpDesde.TabIndex = 10;
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(739, 68);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(228, 23);
            this.dtpHasta.TabIndex = 12;
            // 
            // panelFiltros
            // 
            this.panelFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panelFiltros.ColumnCount = 6;
            this.panelFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.panelFiltros.Controls.Add(this.btnFiltro, 0, 0);
            this.panelFiltros.Controls.Add(this.btnHoy, 1, 0);
            this.panelFiltros.Controls.Add(this.btn7Dias, 2, 0);
            this.panelFiltros.Controls.Add(this.btn30dias, 3, 0);
            this.panelFiltros.Controls.Add(this.btnMes, 4, 0);
            this.panelFiltros.Controls.Add(this.btnMesPasado, 5, 0);
            this.panelFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFiltros.Location = new System.Drawing.Point(0, 0);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Padding = new System.Windows.Forms.Padding(4);
            this.panelFiltros.RowCount = 1;
            this.panelFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelFiltros.Size = new System.Drawing.Size(1014, 58);
            this.panelFiltros.TabIndex = 14;
            // 
            // btnFiltro
            // 
            this.btnFiltro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnFiltro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFiltro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltro.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltro.Location = new System.Drawing.Point(7, 7);
            this.btnFiltro.Name = "btnFiltro";
            this.btnFiltro.Size = new System.Drawing.Size(161, 44);
            this.btnFiltro.TabIndex = 5;
            this.btnFiltro.Text = "Filtro";
            this.btnFiltro.UseVisualStyleBackColor = false;
            this.btnFiltro.Click += new System.EventHandler(this.btnFiltro_Click);
            // 
            // btnHoy
            // 
            this.btnHoy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnHoy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHoy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoy.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHoy.Location = new System.Drawing.Point(174, 7);
            this.btnHoy.Name = "btnHoy";
            this.btnHoy.Size = new System.Drawing.Size(161, 44);
            this.btnHoy.TabIndex = 4;
            this.btnHoy.Text = "Hoy";
            this.btnHoy.UseVisualStyleBackColor = false;
            this.btnHoy.Click += new System.EventHandler(this.btnHoy_Click);
            // 
            // btn7Dias
            // 
            this.btn7Dias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn7Dias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn7Dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn7Dias.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7Dias.Location = new System.Drawing.Point(341, 7);
            this.btn7Dias.Name = "btn7Dias";
            this.btn7Dias.Size = new System.Drawing.Size(161, 44);
            this.btn7Dias.TabIndex = 3;
            this.btn7Dias.Text = "Ult. 7 dias";
            this.btn7Dias.UseVisualStyleBackColor = false;
            this.btn7Dias.Click += new System.EventHandler(this.btn7Dias_Click);
            // 
            // btn30dias
            // 
            this.btn30dias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn30dias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn30dias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn30dias.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn30dias.Location = new System.Drawing.Point(508, 7);
            this.btn30dias.Name = "btn30dias";
            this.btn30dias.Size = new System.Drawing.Size(161, 44);
            this.btn30dias.TabIndex = 2;
            this.btn30dias.Text = "Ult. 30 dias";
            this.btn30dias.UseVisualStyleBackColor = false;
            this.btn30dias.Click += new System.EventHandler(this.btn30dias_Click);
            // 
            // btnMes
            // 
            this.btnMes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMes.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMes.Location = new System.Drawing.Point(675, 7);
            this.btnMes.Name = "btnMes";
            this.btnMes.Size = new System.Drawing.Size(161, 44);
            this.btnMes.TabIndex = 1;
            this.btnMes.Text = "Este Mes";
            this.btnMes.UseVisualStyleBackColor = false;
            this.btnMes.Click += new System.EventHandler(this.btnMes_Click);
            // 
            // btnMesPasado
            // 
            this.btnMesPasado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMesPasado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMesPasado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMesPasado.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMesPasado.Location = new System.Drawing.Point(842, 7);
            this.btnMesPasado.Name = "btnMesPasado";
            this.btnMesPasado.Size = new System.Drawing.Size(165, 44);
            this.btnMesPasado.TabIndex = 0;
            this.btnMesPasado.Text = "Mes Pasado";
            this.btnMesPasado.UseVisualStyleBackColor = false;
            this.btnMesPasado.Click += new System.EventHandler(this.btnMesPasado_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.tlpKpi);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(13, 127);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1014, 141);
            this.panel1.TabIndex = 19;
            // 
            // tlpKpi
            // 
            this.tlpKpi.ColumnCount = 4;
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpKpi.Controls.Add(this.pnlTile1, 0, 0);
            this.tlpKpi.Controls.Add(this.pnlTile2, 1, 0);
            this.tlpKpi.Controls.Add(this.pnlTile3, 2, 0);
            this.tlpKpi.Controls.Add(this.pnlTile4, 3, 0);
            this.tlpKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKpi.Location = new System.Drawing.Point(0, 0);
            this.tlpKpi.Name = "tlpKpi";
            this.tlpKpi.RowCount = 1;
            this.tlpKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKpi.Size = new System.Drawing.Size(1014, 141);
            this.tlpKpi.TabIndex = 0;
            // 
            // pnlTile1
            // 
            this.pnlTile1.Controls.Add(this.lblCompras);
            this.pnlTile1.Controls.Add(this.label16);
            this.pnlTile1.Controls.Add(this.label1);
            this.pnlTile1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile1.Location = new System.Drawing.Point(8, 8);
            this.pnlTile1.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile1.Name = "pnlTile1";
            this.pnlTile1.Size = new System.Drawing.Size(237, 125);
            this.pnlTile1.TabIndex = 0;
            // 
            // lblCompras
            // 
            this.lblCompras.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblCompras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCompras.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompras.Location = new System.Drawing.Point(0, 0);
            this.lblCompras.Name = "lblCompras";
            this.lblCompras.Size = new System.Drawing.Size(237, 95);
            this.lblCompras.TabIndex = 1;
            this.lblCompras.Text = "Ventas Hoy";
            this.lblCompras.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Image = ((System.Drawing.Image)(resources.GetObject("label16.Image")));
            this.label16.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label16.Location = new System.Drawing.Point(0, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 23);
            this.label16.TabIndex = 0;
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
            this.label1.Size = new System.Drawing.Size(237, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile2
            // 
            this.pnlTile2.Controls.Add(this.lblComprasCant);
            this.pnlTile2.Controls.Add(this.label2);
            this.pnlTile2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile2.Location = new System.Drawing.Point(261, 8);
            this.pnlTile2.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile2.Name = "pnlTile2";
            this.pnlTile2.Size = new System.Drawing.Size(237, 125);
            this.pnlTile2.TabIndex = 1;
            // 
            // lblComprasCant
            // 
            this.lblComprasCant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblComprasCant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComprasCant.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprasCant.Location = new System.Drawing.Point(0, 0);
            this.lblComprasCant.Name = "lblComprasCant";
            this.lblComprasCant.Size = new System.Drawing.Size(237, 95);
            this.lblComprasCant.TabIndex = 3;
            this.lblComprasCant.Text = "Ventas Hoy";
            this.lblComprasCant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(0, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cantidad";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile3
            // 
            this.pnlTile3.Controls.Add(this.lblComprasMax);
            this.pnlTile3.Controls.Add(this.label3);
            this.pnlTile3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile3.Location = new System.Drawing.Point(514, 8);
            this.pnlTile3.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile3.Name = "pnlTile3";
            this.pnlTile3.Size = new System.Drawing.Size(237, 125);
            this.pnlTile3.TabIndex = 2;
            // 
            // lblComprasMax
            // 
            this.lblComprasMax.BackColor = System.Drawing.Color.Lime;
            this.lblComprasMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComprasMax.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprasMax.Location = new System.Drawing.Point(0, 0);
            this.lblComprasMax.Name = "lblComprasMax";
            this.lblComprasMax.Size = new System.Drawing.Size(237, 95);
            this.lblComprasMax.TabIndex = 5;
            this.lblComprasMax.Text = "Ventas Hoy";
            this.lblComprasMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(0, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(237, 30);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pickit Dia";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTile4
            // 
            this.pnlTile4.Controls.Add(this.lblComprasProm);
            this.pnlTile4.Controls.Add(this.label4);
            this.pnlTile4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTile4.Location = new System.Drawing.Point(767, 8);
            this.pnlTile4.Margin = new System.Windows.Forms.Padding(8);
            this.pnlTile4.Name = "pnlTile4";
            this.pnlTile4.Size = new System.Drawing.Size(239, 125);
            this.pnlTile4.TabIndex = 3;
            // 
            // lblComprasProm
            // 
            this.lblComprasProm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblComprasProm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComprasProm.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprasProm.Location = new System.Drawing.Point(0, 0);
            this.lblComprasProm.Name = "lblComprasProm";
            this.lblComprasProm.Size = new System.Drawing.Size(239, 95);
            this.lblComprasProm.TabIndex = 6;
            this.lblComprasProm.Text = "Ventas Hoy";
            this.lblComprasProm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(0, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(239, 30);
            this.label4.TabIndex = 7;
            this.label4.Text = "Promedio";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpCharts
            // 
            this.tlpCharts.ColumnCount = 2;
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.Controls.Add(this.cartesianChartComprasPorDia, 0, 0);
            this.tlpCharts.Controls.Add(this.label11, 0, 1);
            this.tlpCharts.Controls.Add(this.label12, 1, 1);
            this.tlpCharts.Controls.Add(this.pieChartProveedores, 0, 2);
            this.tlpCharts.Controls.Add(this.pieChartRubros, 1, 2);
            this.tlpCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCharts.Location = new System.Drawing.Point(13, 277);
            this.tlpCharts.Name = "tlpCharts";
            this.tlpCharts.RowCount = 3;
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tlpCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCharts.Size = new System.Drawing.Size(1014, 470);
            this.tlpCharts.TabIndex = 21;
            // 
            // cartesianChartComprasPorDia
            // 
            this.cartesianChartComprasPorDia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tlpCharts.SetColumnSpan(this.cartesianChartComprasPorDia, 2);
            this.cartesianChartComprasPorDia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChartComprasPorDia.Location = new System.Drawing.Point(3, 3);
            this.cartesianChartComprasPorDia.Name = "cartesianChartComprasPorDia";
            this.cartesianChartComprasPorDia.Size = new System.Drawing.Size(1008, 216);
            this.cartesianChartComprasPorDia.TabIndex = 21;
            this.cartesianChartComprasPorDia.Text = "cartesianChart1";
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 222);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(501, 26);
            this.label11.TabIndex = 29;
            this.label11.Text = "Compras x Proveedor";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(510, 222);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(501, 26);
            this.label12.TabIndex = 30;
            this.label12.Text = "Compras de Productos x Rubro";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pieChartProveedores
            // 
            this.pieChartProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartProveedores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieChartProveedores.Location = new System.Drawing.Point(3, 251);
            this.pieChartProveedores.Name = "pieChartProveedores";
            this.pieChartProveedores.Size = new System.Drawing.Size(501, 216);
            this.pieChartProveedores.TabIndex = 24;
            this.pieChartProveedores.Text = "pieChart2";
            // 
            // pieChartRubros
            // 
            this.pieChartRubros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartRubros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pieChartRubros.Location = new System.Drawing.Point(510, 251);
            this.pieChartRubros.Name = "pieChartRubros";
            this.pieChartRubros.Size = new System.Drawing.Size(501, 216);
            this.pieChartRubros.TabIndex = 26;
            this.pieChartRubros.Text = "pieChart2";
            // 
            // frmDashboardProveedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(1000, 700);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1040, 760);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "frmDashboardProveedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard Proveedores";
            this.Load += new System.EventHandler(this.frmDashboardProveedores_Load);
            this.tlpMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelFiltros.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tlpKpi.ResumeLayout(false);
            this.pnlTile1.ResumeLayout(false);
            this.pnlTile2.ResumeLayout(false);
            this.pnlTile3.ResumeLayout(false);
            this.pnlTile4.ResumeLayout(false);
            this.tlpCharts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpKpi;
        private System.Windows.Forms.TableLayoutPanel tlpCharts;
        private System.Windows.Forms.Panel pnlTile1;
        private System.Windows.Forms.Panel pnlTile2;
        private System.Windows.Forms.Panel pnlTile3;
        private System.Windows.Forms.Panel pnlTile4;
        private LiveCharts.WinForms.CartesianChart cartesianChartComprasPorDia;
        private System.Windows.Forms.Button btnMesPasado;
        private System.Windows.Forms.Button btnHoy;
        private System.Windows.Forms.Button btn7Dias;
        private System.Windows.Forms.Button btn30dias;
        private System.Windows.Forms.Button btnMes;
        private System.Windows.Forms.TableLayoutPanel panelFiltros;
        private System.Windows.Forms.Button btnFiltro;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private LiveCharts.WinForms.PieChart pieChartRubros;
        private LiveCharts.WinForms.PieChart pieChartProveedores;
        private System.Windows.Forms.Label lblComprasProm;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblComprasMax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblComprasCant;
        private System.Windows.Forms.Label lblCompras;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboProveedor;
        private System.Windows.Forms.CheckBox cbProveedor;
    }
}


