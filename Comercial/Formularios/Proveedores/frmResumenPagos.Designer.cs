
namespace Comercial.Formularios.Proveedores
{
    partial class frmResumenPagos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResumenPagos));
            this.btnDescargarDetalle = new System.Windows.Forms.Button();
            this.dgvPagos = new System.Windows.Forms.DataGridView();
            this.btnBuscarDetalle = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpHastaDetalle = new System.Windows.Forms.DateTimePicker();
            this.dtpDesdeDetalle = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDescargarDetalle
            // 
            this.btnDescargarDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDescargarDetalle.Image = ((System.Drawing.Image)(resources.GetObject("btnDescargarDetalle.Image")));
            this.btnDescargarDetalle.Location = new System.Drawing.Point(864, 63);
            this.btnDescargarDetalle.Name = "btnDescargarDetalle";
            this.btnDescargarDetalle.Size = new System.Drawing.Size(49, 28);
            this.btnDescargarDetalle.TabIndex = 31;
            this.btnDescargarDetalle.UseVisualStyleBackColor = true;
            this.btnDescargarDetalle.Click += new System.EventHandler(this.btnDescargarDetalle_Click);
            // 
            // dgvPagos
            // 
            this.dgvPagos.AllowUserToAddRows = false;
            this.dgvPagos.AllowUserToDeleteRows = false;
            this.dgvPagos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPagos.Location = new System.Drawing.Point(12, 63);
            this.dgvPagos.Name = "dgvPagos";
            this.dgvPagos.ReadOnly = true;
            this.dgvPagos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPagos.Size = new System.Drawing.Size(837, 526);
            this.dgvPagos.TabIndex = 30;
            // 
            // btnBuscarDetalle
            // 
            this.btnBuscarDetalle.BackColor = System.Drawing.Color.Silver;
            this.btnBuscarDetalle.Location = new System.Drawing.Point(494, 19);
            this.btnBuscarDetalle.Name = "btnBuscarDetalle";
            this.btnBuscarDetalle.Size = new System.Drawing.Size(105, 24);
            this.btnBuscarDetalle.TabIndex = 29;
            this.btnBuscarDetalle.Text = "Buscar";
            this.btnBuscarDetalle.UseVisualStyleBackColor = false;
            this.btnBuscarDetalle.Click += new System.EventHandler(this.btnBuscarDetalle_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(253, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 15);
            this.label4.TabIndex = 28;
            this.label4.Text = "Hasta";
            // 
            // dtpHastaDetalle
            // 
            this.dtpHastaDetalle.Location = new System.Drawing.Point(253, 21);
            this.dtpHastaDetalle.Name = "dtpHastaDetalle";
            this.dtpHastaDetalle.Size = new System.Drawing.Size(226, 21);
            this.dtpHastaDetalle.TabIndex = 27;
            // 
            // dtpDesdeDetalle
            // 
            this.dtpDesdeDetalle.Location = new System.Drawing.Point(12, 21);
            this.dtpDesdeDetalle.Name = "dtpDesdeDetalle";
            this.dtpDesdeDetalle.Size = new System.Drawing.Size(226, 21);
            this.dtpDesdeDetalle.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 15);
            this.label6.TabIndex = 25;
            this.label6.Text = "Desde";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(657, 595);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(192, 21);
            this.txtTotal.TabIndex = 32;
            // 
            // frmResumenPagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(933, 627);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.btnDescargarDetalle);
            this.Controls.Add(this.dgvPagos);
            this.Controls.Add(this.btnBuscarDetalle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpHastaDetalle);
            this.Controls.Add(this.dtpDesdeDetalle);
            this.Controls.Add(this.label6);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmResumenPagos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Resumen Pagos";
            this.Load += new System.EventHandler(this.frmResumenPagos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDescargarDetalle;
        private System.Windows.Forms.DataGridView dgvPagos;
        private System.Windows.Forms.Button btnBuscarDetalle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpHastaDetalle;
        private System.Windows.Forms.DateTimePicker dtpDesdeDetalle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTotal;
    }
}