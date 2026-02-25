
namespace Comercial.Formularios.Facturacion
{
    partial class frmReporteFacturacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReporteFacturacion));
            this.tbReportes = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvResumenGeneral = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvResumenDiario = new System.Windows.Forms.DataGridView();
            this.btnExportar = new System.Windows.Forms.Button();
            this.tbReportes.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumenGeneral)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumenDiario)).BeginInit();
            this.SuspendLayout();
            // 
            // tbReportes
            // 
            this.tbReportes.Controls.Add(this.tabPage1);
            this.tbReportes.Controls.Add(this.tabPage2);
            this.tbReportes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReportes.Location = new System.Drawing.Point(0, 0);
            this.tbReportes.Name = "tbReportes";
            this.tbReportes.SelectedIndex = 0;
            this.tbReportes.Size = new System.Drawing.Size(994, 519);
            this.tbReportes.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(986, 491);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Resúmenes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(925, 491);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Detalles";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.btnExportar);
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.dtpDesde);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dtpHasta);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(959, 70);
            this.groupBox1.TabIndex = 13;
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
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Silver;
            this.groupBox2.Controls.Add(this.dgvResumenGeneral);
            this.groupBox2.Location = new System.Drawing.Point(8, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(959, 105);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            // 
            // dgvResumenGeneral
            // 
            this.dgvResumenGeneral.AllowUserToAddRows = false;
            this.dgvResumenGeneral.AllowUserToDeleteRows = false;
            this.dgvResumenGeneral.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResumenGeneral.Location = new System.Drawing.Point(16, 20);
            this.dgvResumenGeneral.Name = "dgvResumenGeneral";
            this.dgvResumenGeneral.ReadOnly = true;
            this.dgvResumenGeneral.Size = new System.Drawing.Size(937, 70);
            this.dgvResumenGeneral.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Silver;
            this.groupBox3.Controls.Add(this.dgvResumenDiario);
            this.groupBox3.Location = new System.Drawing.Point(8, 193);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(959, 290);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            // 
            // dgvResumenDiario
            // 
            this.dgvResumenDiario.AllowUserToAddRows = false;
            this.dgvResumenDiario.AllowUserToDeleteRows = false;
            this.dgvResumenDiario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResumenDiario.Location = new System.Drawing.Point(16, 20);
            this.dgvResumenDiario.Name = "dgvResumenDiario";
            this.dgvResumenDiario.ReadOnly = true;
            this.dgvResumenDiario.Size = new System.Drawing.Size(937, 264);
            this.dgvResumenDiario.TabIndex = 0;
            // 
            // btnExportar
            // 
            this.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportar.Image = ((System.Drawing.Image)(resources.GetObject("btnExportar.Image")));
            this.btnExportar.Location = new System.Drawing.Point(892, 19);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(49, 28);
            this.btnExportar.TabIndex = 10;
            this.btnExportar.UseVisualStyleBackColor = true;
            // 
            // frmReporteFacturacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(994, 519);
            this.Controls.Add(this.tbReportes);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReporteFacturacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reportes Facturacion";
            this.tbReportes.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumenGeneral)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumenDiario)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbReportes;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvResumenGeneral;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvResumenDiario;
        private System.Windows.Forms.Button btnExportar;
    }
}