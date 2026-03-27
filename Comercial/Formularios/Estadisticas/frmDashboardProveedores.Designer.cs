
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
            this.cartesianChartComprasPorDia = new LiveCharts.WinForms.CartesianChart();
            this.btnMesPasado = new System.Windows.Forms.Button();
            this.btnHoy = new System.Windows.Forms.Button();
            this.btn7Dias = new System.Windows.Forms.Button();
            this.btn30dias = new System.Windows.Forms.Button();
            this.btnMes = new System.Windows.Forms.Button();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.btnFiltro = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pieChartRubros = new LiveCharts.WinForms.PieChart();
            this.pieChartProveedores = new LiveCharts.WinForms.PieChart();
            this.lblComprasProm = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblComprasMax = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblComprasCant = new System.Windows.Forms.Label();
            this.lblCompras = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.cbProveedor = new System.Windows.Forms.CheckBox();
            this.panelFiltros.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cartesianChartComprasPorDia
            // 
            this.cartesianChartComprasPorDia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cartesianChartComprasPorDia.Location = new System.Drawing.Point(10, 279);
            this.cartesianChartComprasPorDia.Name = "cartesianChartComprasPorDia";
            this.cartesianChartComprasPorDia.Size = new System.Drawing.Size(995, 264);
            this.cartesianChartComprasPorDia.TabIndex = 21;
            this.cartesianChartComprasPorDia.Text = "cartesianChart1";
            // 
            // btnMesPasado
            // 
            this.btnMesPasado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMesPasado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMesPasado.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMesPasado.Location = new System.Drawing.Point(762, 9);
            this.btnMesPasado.Name = "btnMesPasado";
            this.btnMesPasado.Size = new System.Drawing.Size(139, 38);
            this.btnMesPasado.TabIndex = 0;
            this.btnMesPasado.Text = "Mes Pasado";
            this.btnMesPasado.UseVisualStyleBackColor = false;
            this.btnMesPasado.Click += new System.EventHandler(this.btnMesPasado_Click);
            // 
            // btnHoy
            // 
            this.btnHoy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnHoy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoy.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHoy.Location = new System.Drawing.Point(210, 9);
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
            this.btn7Dias.Location = new System.Drawing.Point(348, 9);
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
            this.btn30dias.Location = new System.Drawing.Point(486, 9);
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
            this.btnMes.Location = new System.Drawing.Point(624, 9);
            this.btnMes.Name = "btnMes";
            this.btnMes.Size = new System.Drawing.Size(139, 38);
            this.btnMes.TabIndex = 1;
            this.btnMes.Text = "Este Mes";
            this.btnMes.UseVisualStyleBackColor = false;
            this.btnMes.Click += new System.EventHandler(this.btnMes_Click);
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
            this.panelFiltros.Location = new System.Drawing.Point(23, 12);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Size = new System.Drawing.Size(967, 58);
            this.panelFiltros.TabIndex = 14;
            // 
            // btnFiltro
            // 
            this.btnFiltro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnFiltro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltro.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFiltro.Location = new System.Drawing.Point(77, 9);
            this.btnFiltro.Name = "btnFiltro";
            this.btnFiltro.Size = new System.Drawing.Size(139, 38);
            this.btnFiltro.TabIndex = 5;
            this.btnFiltro.Text = "Filtro";
            this.btnFiltro.UseVisualStyleBackColor = false;
            this.btnFiltro.Click += new System.EventHandler(this.btnFiltro_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = ((System.Drawing.Image)(resources.GetObject("label4.Image")));
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(841, 103);
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
            this.label11.Location = new System.Drawing.Point(19, 554);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(160, 20);
            this.label11.TabIndex = 29;
            this.label11.Text = "Compras x Proveedor";
            // 
            // pieChartRubros
            // 
            this.pieChartRubros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartRubros.Location = new System.Drawing.Point(524, 576);
            this.pieChartRubros.Name = "pieChartRubros";
            this.pieChartRubros.Size = new System.Drawing.Size(481, 300);
            this.pieChartRubros.TabIndex = 26;
            this.pieChartRubros.Text = "pieChart2";
            // 
            // pieChartProveedores
            // 
            this.pieChartProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pieChartProveedores.Location = new System.Drawing.Point(10, 577);
            this.pieChartProveedores.Name = "pieChartProveedores";
            this.pieChartProveedores.Size = new System.Drawing.Size(481, 300);
            this.pieChartProveedores.TabIndex = 24;
            this.pieChartProveedores.Text = "pieChart2";
            // 
            // lblComprasProm
            // 
            this.lblComprasProm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblComprasProm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprasProm.Location = new System.Drawing.Point(825, 16);
            this.lblComprasProm.Name = "lblComprasProm";
            this.lblComprasProm.Size = new System.Drawing.Size(155, 74);
            this.lblComprasProm.TabIndex = 6;
            this.lblComprasProm.Text = "Ventas Hoy";
            this.lblComprasProm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpDesde
            // 
            this.dtpDesde.CustomFormat = "\"dd/MM/yyyy\"";
            this.dtpDesde.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpDesde.Location = new System.Drawing.Point(499, 65);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(228, 21);
            this.dtpDesde.TabIndex = 10;
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(739, 65);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(228, 21);
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
            this.label12.Location = new System.Drawing.Point(530, 553);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(230, 20);
            this.label12.TabIndex = 30;
            this.label12.Text = "Compras de Productos x Rubro";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblComprasProm);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.lblComprasMax);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblComprasCant);
            this.panel1.Controls.Add(this.lblCompras);
            this.panel1.Location = new System.Drawing.Point(10, 126);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(995, 147);
            this.panel1.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(304, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "     Cantidad";
            // 
            // lblComprasMax
            // 
            this.lblComprasMax.BackColor = System.Drawing.Color.Lime;
            this.lblComprasMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprasMax.Location = new System.Drawing.Point(553, 16);
            this.lblComprasMax.Name = "lblComprasMax";
            this.lblComprasMax.Size = new System.Drawing.Size(155, 74);
            this.lblComprasMax.TabIndex = 5;
            this.lblComprasMax.Text = "Ventas Hoy";
            this.lblComprasMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.label3.Location = new System.Drawing.Point(573, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "      Pickit Dia";
            // 
            // lblComprasCant
            // 
            this.lblComprasCant.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblComprasCant.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprasCant.Location = new System.Drawing.Point(281, 16);
            this.lblComprasCant.Name = "lblComprasCant";
            this.lblComprasCant.Size = new System.Drawing.Size(155, 74);
            this.lblComprasCant.TabIndex = 3;
            this.lblComprasCant.Text = "Ventas Hoy";
            this.lblComprasCant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCompras
            // 
            this.lblCompras.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblCompras.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompras.Location = new System.Drawing.Point(9, 16);
            this.lblCompras.Name = "lblCompras";
            this.lblCompras.Size = new System.Drawing.Size(155, 74);
            this.lblCompras.TabIndex = 1;
            this.lblCompras.Text = "Ventas Hoy";
            this.lblCompras.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.cboProveedor);
            this.panel2.Controls.Add(this.cbProveedor);
            this.panel2.Controls.Add(this.dtpDesde);
            this.panel2.Controls.Add(this.dtpHasta);
            this.panel2.Location = new System.Drawing.Point(10, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(995, 108);
            this.panel2.TabIndex = 20;
            // 
            // cboProveedor
            // 
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(167, 64);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(306, 23);
            this.cboProveedor.TabIndex = 21;
            // 
            // cbProveedor
            // 
            this.cbProveedor.AutoSize = true;
            this.cbProveedor.Location = new System.Drawing.Point(23, 66);
            this.cbProveedor.Name = "cbProveedor";
            this.cbProveedor.Size = new System.Drawing.Size(138, 19);
            this.cbProveedor.TabIndex = 20;
            this.cbProveedor.Text = "Filtrar Por Proveedor";
            this.cbProveedor.UseVisualStyleBackColor = true;
            // 
            // frmDashboardProveedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1015, 888);
            this.Controls.Add(this.panelFiltros);
            this.Controls.Add(this.cartesianChartComprasPorDia);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pieChartRubros);
            this.Controls.Add(this.pieChartProveedores);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDashboardProveedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard Proveedores";
            this.Load += new System.EventHandler(this.frmDashboardProveedores_Load);
            this.panelFiltros.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChartComprasPorDia;
        private System.Windows.Forms.Button btnMesPasado;
        private System.Windows.Forms.Button btnHoy;
        private System.Windows.Forms.Button btn7Dias;
        private System.Windows.Forms.Button btn30dias;
        private System.Windows.Forms.Button btnMes;
        private System.Windows.Forms.Panel panelFiltros;
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