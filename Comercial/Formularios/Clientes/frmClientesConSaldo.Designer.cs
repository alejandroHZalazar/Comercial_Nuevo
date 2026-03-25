
namespace Comercial.Formularios.Clientes
{
    partial class frmClientesConSaldo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClientesConSaldo));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboLocalidad = new System.Windows.Forms.ComboBox();
            this.cbLocalidad = new System.Windows.Forms.CheckBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.cboZona = new System.Windows.Forms.ComboBox();
            this.cbZona = new System.Windows.Forms.CheckBox();
            this.cboProvincia = new System.Windows.Forms.ComboBox();
            this.cbProvincia = new System.Windows.Forms.CheckBox();
            this.cboVendedor = new System.Windows.Forms.ComboBox();
            this.cbVendedor = new System.Windows.Forms.CheckBox();
            this.dgvSaldos = new System.Windows.Forms.DataGridView();
            this.txtSaldo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExportar = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaldos)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.cboVendedor);
            this.groupBox1.Controls.Add(this.cbVendedor);
            this.groupBox1.Controls.Add(this.cboLocalidad);
            this.groupBox1.Controls.Add(this.cbLocalidad);
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.cboZona);
            this.groupBox1.Controls.Add(this.cbZona);
            this.groupBox1.Controls.Add(this.cboProvincia);
            this.groupBox1.Controls.Add(this.cbProvincia);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1135, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro";
            // 
            // cboLocalidad
            // 
            this.cboLocalidad.FormattingEnabled = true;
            this.cboLocalidad.Location = new System.Drawing.Point(517, 20);
            this.cboLocalidad.Name = "cboLocalidad";
            this.cboLocalidad.Size = new System.Drawing.Size(356, 23);
            this.cboLocalidad.TabIndex = 17;
            // 
            // cbLocalidad
            // 
            this.cbLocalidad.AutoSize = true;
            this.cbLocalidad.Location = new System.Drawing.Point(376, 22);
            this.cbLocalidad.Name = "cbLocalidad";
            this.cbLocalidad.Size = new System.Drawing.Size(135, 19);
            this.cbLocalidad.TabIndex = 16;
            this.cbLocalidad.Text = "Filtrar Por Localidad";
            this.cbLocalidad.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.Gray;
            this.btnBuscar.Image = global::Comercial.Properties.Resources.musica_searcher;
            this.btnBuscar.Location = new System.Drawing.Point(810, 48);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(63, 42);
            this.btnBuscar.TabIndex = 14;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // cboZona
            // 
            this.cboZona.FormattingEnabled = true;
            this.cboZona.Location = new System.Drawing.Point(151, 58);
            this.cboZona.Name = "cboZona";
            this.cboZona.Size = new System.Drawing.Size(204, 23);
            this.cboZona.TabIndex = 13;
            // 
            // cbZona
            // 
            this.cbZona.AutoSize = true;
            this.cbZona.Location = new System.Drawing.Point(10, 60);
            this.cbZona.Name = "cbZona";
            this.cbZona.Size = new System.Drawing.Size(111, 19);
            this.cbZona.TabIndex = 12;
            this.cbZona.Text = "Filtrar Por Zona";
            this.cbZona.UseVisualStyleBackColor = true;
            // 
            // cboProvincia
            // 
            this.cboProvincia.FormattingEnabled = true;
            this.cboProvincia.Location = new System.Drawing.Point(151, 20);
            this.cboProvincia.Name = "cboProvincia";
            this.cboProvincia.Size = new System.Drawing.Size(204, 23);
            this.cboProvincia.TabIndex = 11;
            this.cboProvincia.SelectedIndexChanged += new System.EventHandler(this.cboProvincia_SelectedIndexChanged);
            // 
            // cbProvincia
            // 
            this.cbProvincia.AutoSize = true;
            this.cbProvincia.Location = new System.Drawing.Point(10, 22);
            this.cbProvincia.Name = "cbProvincia";
            this.cbProvincia.Size = new System.Drawing.Size(135, 19);
            this.cbProvincia.TabIndex = 10;
            this.cbProvincia.Text = "Filtrar Por Provincia";
            this.cbProvincia.UseVisualStyleBackColor = true;
            // 
            // cboVendedor
            // 
            this.cboVendedor.FormattingEnabled = true;
            this.cboVendedor.Location = new System.Drawing.Point(517, 58);
            this.cboVendedor.Name = "cboVendedor";
            this.cboVendedor.Size = new System.Drawing.Size(275, 23);
            this.cboVendedor.TabIndex = 19;
            // 
            // cbVendedor
            // 
            this.cbVendedor.AutoSize = true;
            this.cbVendedor.Location = new System.Drawing.Point(376, 60);
            this.cbVendedor.Name = "cbVendedor";
            this.cbVendedor.Size = new System.Drawing.Size(138, 19);
            this.cbVendedor.TabIndex = 18;
            this.cbVendedor.Text = "Filtrar Por Vendedor";
            this.cbVendedor.UseVisualStyleBackColor = true;
            // 
            // dgvSaldos
            // 
            this.dgvSaldos.AllowUserToAddRows = false;
            this.dgvSaldos.AllowUserToDeleteRows = false;
            this.dgvSaldos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvSaldos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSaldos.Location = new System.Drawing.Point(12, 111);
            this.dgvSaldos.Name = "dgvSaldos";
            this.dgvSaldos.ReadOnly = true;
            this.dgvSaldos.Size = new System.Drawing.Size(1135, 393);
            this.dgvSaldos.TabIndex = 2;
            // 
            // txtSaldo
            // 
            this.txtSaldo.Location = new System.Drawing.Point(967, 510);
            this.txtSaldo.Name = "txtSaldo";
            this.txtSaldo.Size = new System.Drawing.Size(180, 21);
            this.txtSaldo.TabIndex = 3;
            this.txtSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(863, 512);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Saldo Total";
            // 
            // btnExportar
            // 
            this.btnExportar.BackColor = System.Drawing.Color.Silver;
            this.btnExportar.Image = ((System.Drawing.Image)(resources.GetObject("btnExportar.Image")));
            this.btnExportar.Location = new System.Drawing.Point(12, 508);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(49, 33);
            this.btnExportar.TabIndex = 10;
            this.btnExportar.UseVisualStyleBackColor = false;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // frmClientesConSaldo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1159, 547);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSaldo);
            this.Controls.Add(this.dgvSaldos);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClientesConSaldo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Saldos de Clientes";
            this.Load += new System.EventHandler(this.frmClientesConSaldo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaldos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboLocalidad;
        private System.Windows.Forms.CheckBox cbLocalidad;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ComboBox cboZona;
        private System.Windows.Forms.CheckBox cbZona;
        private System.Windows.Forms.ComboBox cboProvincia;
        private System.Windows.Forms.CheckBox cbProvincia;
        private System.Windows.Forms.ComboBox cboVendedor;
        private System.Windows.Forms.CheckBox cbVendedor;
        private System.Windows.Forms.DataGridView dgvSaldos;
        private System.Windows.Forms.TextBox txtSaldo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExportar;
    }
}