namespace Comercial.Formularios.Ventas
{
    partial class frmVentasReportes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVentasReportes));
            System.Windows.Forms.DataGridViewCellStyle dgvCabeceraHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvCabeceraAltStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvDetalleHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvDetalleAltStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitGrillas = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbCliente = new System.Windows.Forms.CheckBox();
            this.cboCliente = new System.Windows.Forms.ComboBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvVentasCabecera = new System.Windows.Forms.DataGridView();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasCabecera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrillas)).BeginInit();
            this.splitGrillas.Panel1.SuspendLayout();
            this.splitGrillas.Panel2.SuspendLayout();
            this.splitGrillas.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Text = "Filtros";
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.cbCliente);
            this.groupBox1.Controls.Add(this.cboCliente);
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.dtpDesde);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpHasta);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(833, 113);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // cbCliente
            // 
            this.cbCliente.AutoSize = true;
            this.cbCliente.Location = new System.Drawing.Point(13, 71);
            this.cbCliente.Name = "cbCliente";
            this.cbCliente.Size = new System.Drawing.Size(117, 19);
            this.cbCliente.TabIndex = 10;
            this.cbCliente.Text = "Filtrar Por Cliente";
            this.cbCliente.UseVisualStyleBackColor = true;
            // 
            // cboCliente
            // 
            this.cboCliente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCliente.FormattingEnabled = true;
            this.cboCliente.Location = new System.Drawing.Point(152, 69);
            this.cboCliente.Name = "cboCliente";
            this.cboCliente.Size = new System.Drawing.Size(550, 23);
            this.cboCliente.TabIndex = 9;
            //
            // btnBuscar
            //
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscar.BackColor = System.Drawing.Color.Gray;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.Image = global::Comercial.Properties.Resources.musica_searcher;
            this.btnBuscar.Location = new System.Drawing.Point(710, 62);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 36);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dtpDesde
            // 
            this.dtpDesde.Location = new System.Drawing.Point(54, 23);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(235, 23);
            this.dtpDesde.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Desde";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(332, 23);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(235, 23);
            this.dtpHasta.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(293, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hasta";
            // 
            // dgvVentasCabecera
            // 
            this.dgvVentasCabecera.AllowUserToAddRows = false;
            this.dgvVentasCabecera.AllowUserToDeleteRows = false;
            this.dgvVentasCabecera.AlternatingRowsDefaultCellStyle = dgvCabeceraAltStyle;
            this.dgvVentasCabecera.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvVentasCabecera.BackgroundColor = System.Drawing.Color.White;
            this.dgvVentasCabecera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dgvCabeceraAltStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            dgvCabeceraHeaderStyle.BackColor = System.Drawing.Color.DimGray;
            dgvCabeceraHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvCabeceraHeaderStyle.ForeColor = System.Drawing.Color.White;
            dgvCabeceraHeaderStyle.SelectionBackColor = System.Drawing.Color.DimGray;
            dgvCabeceraHeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvVentasCabecera.ColumnHeadersDefaultCellStyle = dgvCabeceraHeaderStyle;
            this.dgvVentasCabecera.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentasCabecera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVentasCabecera.EnableHeadersVisualStyles = false;
            this.dgvVentasCabecera.Location = new System.Drawing.Point(0, 0);
            this.dgvVentasCabecera.MultiSelect = false;
            this.dgvVentasCabecera.Name = "dgvVentasCabecera";
            this.dgvVentasCabecera.ReadOnly = true;
            this.dgvVentasCabecera.RowHeadersVisible = false;
            this.dgvVentasCabecera.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVentasCabecera.Size = new System.Drawing.Size(951, 300);
            this.dgvVentasCabecera.TabIndex = 11;
            this.dgvVentasCabecera.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVentasCabecera_CellEnter);
            // 
            // dgvDetalle
            // 
            this.dgvDetalle.AllowUserToAddRows = false;
            this.dgvDetalle.AllowUserToDeleteRows = false;
            this.dgvDetalle.AlternatingRowsDefaultCellStyle = dgvDetalleAltStyle;
            this.dgvDetalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDetalle.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dgvDetalleAltStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            dgvDetalleHeaderStyle.BackColor = System.Drawing.Color.Gray;
            dgvDetalleHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvDetalleHeaderStyle.ForeColor = System.Drawing.Color.White;
            dgvDetalleHeaderStyle.SelectionBackColor = System.Drawing.Color.Gray;
            dgvDetalleHeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvDetalle.ColumnHeadersDefaultCellStyle = dgvDetalleHeaderStyle;
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalle.EnableHeadersVisualStyles = false;
            this.dgvDetalle.Location = new System.Drawing.Point(0, 0);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.ReadOnly = true;
            this.dgvDetalle.RowHeadersVisible = false;
            this.dgvDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalle.Size = new System.Drawing.Size(951, 233);
            this.dgvDetalle.TabIndex = 12;
            //
            // splitGrillas
            //
            this.splitGrillas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitGrillas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.splitGrillas.Location = new System.Drawing.Point(12, 131);
            this.splitGrillas.Name = "splitGrillas";
            this.splitGrillas.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitGrillas.Panel1.Controls.Add(this.dgvVentasCabecera);
            this.splitGrillas.Panel1MinSize = 100;
            this.splitGrillas.Panel2.Controls.Add(this.dgvDetalle);
            this.splitGrillas.Panel2MinSize = 100;
            this.splitGrillas.Size = new System.Drawing.Size(951, 537);
            this.splitGrillas.SplitterDistance = 300;
            this.splitGrillas.TabIndex = 13;
            //
            // btnImprimir
            //
            this.btnImprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImprimir.BackColor = System.Drawing.Color.Silver;
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Image = global::Comercial.Properties.Resources.printer_1;
            this.btnImprimir.Location = new System.Drawing.Point(853, 12);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(110, 40);
            this.btnImprimir.TabIndex = 14;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // frmVentasReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(975, 680);
            this.Controls.Add(this.splitGrillas);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(760, 560);
            this.Name = "frmVentasReportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reportes de Ventas";
            this.Load += new System.EventHandler(this.frmVentasReportes_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentasCabecera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.splitGrillas.Panel1.ResumeLayout(false);
            this.splitGrillas.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrillas)).EndInit();
            this.splitGrillas.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbCliente;
        private System.Windows.Forms.ComboBox cboCliente;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvVentasCabecera;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.SplitContainer splitGrillas;
    }
}