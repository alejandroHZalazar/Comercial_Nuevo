
namespace Comercial.Formularios.Facturacion
{
    partial class frmFacturacionLotes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFacturacionLotes));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvVentas = new System.Windows.Forms.DataGridView();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.nudTotal = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTodos = new System.Windows.Forms.Button();
            this.btnNinguno = new System.Windows.Forms.Button();
            this.btnFacturar = new System.Windows.Forms.Button();
            this.rtbProceso = new System.Windows.Forms.RichTextBox();
            this.pbFacturar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.dtpDesde);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpHasta);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(959, 70);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.Gray;
            this.btnBuscar.Image = global::Comercial.Properties.Resources.musica_searcher;
            this.btnBuscar.Location = new System.Drawing.Point(584, 17);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(54, 36);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dtpDesde
            // 
            this.dtpDesde.Location = new System.Drawing.Point(60, 23);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(235, 21);
            this.dtpDesde.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Desde";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(343, 23);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(235, 21);
            this.dtpHasta.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(300, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Hasta";
            // 
            // dgvVentas
            // 
            this.dgvVentas.AllowUserToAddRows = false;
            this.dgvVentas.AllowUserToDeleteRows = false;
            this.dgvVentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentas.Location = new System.Drawing.Point(12, 100);
            this.dgvVentas.Name = "dgvVentas";
            this.dgvVentas.Size = new System.Drawing.Size(959, 388);
            this.dgvVentas.TabIndex = 13;
            this.dgvVentas.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVentas_CellEnter);
            this.dgvVentas.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVentas_CellValueChanged);
            this.dgvVentas.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvVentas_CurrentCellDirtyStateChanged);
            // 
            // dgvDetalle
            // 
            this.dgvDetalle.AllowUserToAddRows = false;
            this.dgvDetalle.AllowUserToDeleteRows = false;
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Location = new System.Drawing.Point(12, 504);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.ReadOnly = true;
            this.dgvDetalle.Size = new System.Drawing.Size(959, 190);
            this.dgvDetalle.TabIndex = 14;
            // 
            // nudTotal
            // 
            this.nudTotal.DecimalPlaces = 2;
            this.nudTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTotal.Location = new System.Drawing.Point(218, 708);
            this.nudTotal.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudTotal.Name = "nudTotal";
            this.nudTotal.Size = new System.Drawing.Size(198, 24);
            this.nudTotal.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 710);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Total de Importe a Facturar";
            // 
            // btnTodos
            // 
            this.btnTodos.BackColor = System.Drawing.Color.Gray;
            this.btnTodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTodos.Location = new System.Drawing.Point(449, 702);
            this.btnTodos.Name = "btnTodos";
            this.btnTodos.Size = new System.Drawing.Size(72, 36);
            this.btnTodos.TabIndex = 9;
            this.btnTodos.Text = "Todos";
            this.btnTodos.UseVisualStyleBackColor = false;
            this.btnTodos.Click += new System.EventHandler(this.btnTodos_Click);
            // 
            // btnNinguno
            // 
            this.btnNinguno.BackColor = System.Drawing.Color.Gray;
            this.btnNinguno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNinguno.Location = new System.Drawing.Point(527, 702);
            this.btnNinguno.Name = "btnNinguno";
            this.btnNinguno.Size = new System.Drawing.Size(72, 36);
            this.btnNinguno.TabIndex = 16;
            this.btnNinguno.Text = "Ninguno";
            this.btnNinguno.UseVisualStyleBackColor = false;
            this.btnNinguno.Click += new System.EventHandler(this.btnNinguno_Click);
            // 
            // btnFacturar
            // 
            this.btnFacturar.BackColor = System.Drawing.Color.Gray;
            this.btnFacturar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFacturar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacturar.Image = ((System.Drawing.Image)(resources.GetObject("btnFacturar.Image")));
            this.btnFacturar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFacturar.Location = new System.Drawing.Point(846, 702);
            this.btnFacturar.Name = "btnFacturar";
            this.btnFacturar.Size = new System.Drawing.Size(123, 36);
            this.btnFacturar.TabIndex = 9;
            this.btnFacturar.Text = "Facturar";
            this.btnFacturar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFacturar.UseVisualStyleBackColor = false;
            this.btnFacturar.Click += new System.EventHandler(this.btnFacturar_Click);
            // 
            // rtbProceso
            // 
            this.rtbProceso.Location = new System.Drawing.Point(977, 100);
            this.rtbProceso.Name = "rtbProceso";
            this.rtbProceso.Size = new System.Drawing.Size(215, 594);
            this.rtbProceso.TabIndex = 17;
            this.rtbProceso.Text = "";
            // 
            // pbFacturar
            // 
            this.pbFacturar.Location = new System.Drawing.Point(987, 705);
            this.pbFacturar.Name = "pbFacturar";
            this.pbFacturar.Size = new System.Drawing.Size(205, 23);
            this.pbFacturar.TabIndex = 18;
            // 
            // frmFacturacionLotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1200, 740);
            this.Controls.Add(this.pbFacturar);
            this.Controls.Add(this.rtbProceso);
            this.Controls.Add(this.btnFacturar);
            this.Controls.Add(this.btnNinguno);
            this.Controls.Add(this.btnTodos);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudTotal);
            this.Controls.Add(this.dgvDetalle);
            this.Controls.Add(this.dgvVentas);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFacturacionLotes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facturacion por Lotes";
            this.Load += new System.EventHandler(this.frmFacturacionLotes_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvVentas;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.NumericUpDown nudTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTodos;
        private System.Windows.Forms.Button btnNinguno;
        private System.Windows.Forms.Button btnFacturar;
        private System.Windows.Forms.RichTextBox rtbProceso;
        private System.Windows.Forms.ProgressBar pbFacturar;
    }
}