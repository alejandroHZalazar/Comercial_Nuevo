namespace Comercial.Formularios.Estadisticas
{
    partial class frmRankingVentas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRankingVentas));
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvAltStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRankingProductos = new System.Windows.Forms.Button();
            this.dgvRanking = new System.Windows.Forms.DataGridView();
            this.btnRankigCliente = new System.Windows.Forms.Button();
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.cbProveedor = new System.Windows.Forms.CheckBox();
            this.btnExportar = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRanking)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.btnExportar);
            this.groupBox1.Controls.Add(this.cboProveedor);
            this.groupBox1.Controls.Add(this.cbProveedor);
            this.groupBox1.Controls.Add(this.dtpDesde);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpHasta);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1076, 61);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro";
            // 
            // dtpDesde
            // 
            this.dtpDesde.Location = new System.Drawing.Point(56, 22);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(200, 23);
            this.dtpDesde.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "Desde";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(306, 22);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(200, 23);
            this.dtpHasta.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Hasta";
            // 
            // btnRankingProductos
            // 
            this.btnRankingProductos.BackColor = System.Drawing.Color.Gray;
            this.btnRankingProductos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRankingProductos.Location = new System.Drawing.Point(12, 90);
            this.btnRankingProductos.Name = "btnRankingProductos";
            this.btnRankingProductos.Size = new System.Drawing.Size(187, 32);
            this.btnRankingProductos.TabIndex = 2;
            this.btnRankingProductos.Text = "RANKING POR PRODUCTOS";
            this.btnRankingProductos.UseVisualStyleBackColor = false;
            this.btnRankingProductos.Click += new System.EventHandler(this.btnRankingProductos_Click);
            // 
            // dgvRanking
            // 
            this.dgvRanking.AllowUserToAddRows = false;
            this.dgvRanking.AllowUserToDeleteRows = false;
            this.dgvRanking.AlternatingRowsDefaultCellStyle = dgvAltStyle;
            this.dgvRanking.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.dgvRanking.BackgroundColor = System.Drawing.Color.White;
            this.dgvRanking.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dgvAltStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            dgvHeaderStyle.BackColor = System.Drawing.Color.DimGray;
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvHeaderStyle.ForeColor = System.Drawing.Color.White;
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.DimGray;
            dgvHeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvRanking.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvRanking.EnableHeadersVisualStyles = false;
            this.dgvRanking.RowHeadersVisible = false;
            this.dgvRanking.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvRanking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRanking.Location = new System.Drawing.Point(211, 90);
            this.dgvRanking.Name = "dgvRanking";
            this.dgvRanking.ReadOnly = true;
            this.dgvRanking.Size = new System.Drawing.Size(877, 498);
            this.dgvRanking.TabIndex = 3;
            // 
            // btnRankigCliente
            // 
            this.btnRankigCliente.BackColor = System.Drawing.Color.Gray;
            this.btnRankigCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRankigCliente.Location = new System.Drawing.Point(12, 130);
            this.btnRankigCliente.Name = "btnRankigCliente";
            this.btnRankigCliente.Size = new System.Drawing.Size(187, 32);
            this.btnRankigCliente.TabIndex = 4;
            this.btnRankigCliente.Text = "RANKING POR CLIENTES";
            this.btnRankigCliente.UseVisualStyleBackColor = false;
            this.btnRankigCliente.Click += new System.EventHandler(this.btnRankigCliente_Click);
            // 
            // cboProveedor
            // 
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.cboProveedor.Location = new System.Drawing.Point(664, 22);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(343, 23);
            this.cboProveedor.TabIndex = 19;
            // 
            // cbProveedor
            // 
            this.cbProveedor.AutoSize = true;
            this.cbProveedor.Location = new System.Drawing.Point(516, 24);
            this.cbProveedor.Name = "cbProveedor";
            this.cbProveedor.Size = new System.Drawing.Size(143, 19);
            this.cbProveedor.TabIndex = 18;
            this.cbProveedor.Text = "Filtrar Por Proveedor";
            this.cbProveedor.UseVisualStyleBackColor = true;
            // 
            // btnExportar
            // 
            this.btnExportar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportar.Image = ((System.Drawing.Image)(resources.GetObject("btnExportar.Image")));
            this.btnExportar.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnExportar.Location = new System.Drawing.Point(1015, 19);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(49, 28);
            this.btnExportar.TabIndex = 10;
            this.btnExportar.UseVisualStyleBackColor = false;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // frmRankingVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1100, 600);
            this.Controls.Add(this.btnRankigCliente);
            this.Controls.Add(this.dgvRanking);
            this.Controls.Add(this.btnRankingProductos);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(960, 520);
            this.Name = "frmRankingVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ranking Ventas";
            this.Load += new System.EventHandler(this.frmRankingVentas_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRanking)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRankingProductos;
        private System.Windows.Forms.DataGridView dgvRanking;
        private System.Windows.Forms.Button btnRankigCliente;
        private System.Windows.Forms.ComboBox cboProveedor;
        private System.Windows.Forms.CheckBox cbProveedor;
        private System.Windows.Forms.Button btnExportar;
    }
}
