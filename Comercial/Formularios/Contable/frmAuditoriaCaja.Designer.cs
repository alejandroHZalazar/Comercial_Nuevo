
namespace Comercial.Formularios.Contable
{
    partial class frmAuditoriaCaja
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAuditoriaCaja));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dgvCajas = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpHastaResumen = new System.Windows.Forms.DateTimePicker();
            this.dtpDesdeResumen = new System.Windows.Forms.DateTimePicker();
            this.cboUsuario = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelResumen = new System.Windows.Forms.Panel();
            this.lblTotalHaber = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotalDebe = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rtbObservaciones = new System.Windows.Forms.RichTextBox();
            this.dgvMovimiento = new System.Windows.Forms.DataGridView();
            this.btnExportar = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lblEgresos = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblIngresos = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnDescargarDetalle = new System.Windows.Forms.Button();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.btnBuscarDetalle = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpHastaDetalle = new System.Windows.Forms.DateTimePicker();
            this.dtpDesdeDetalle = new System.Windows.Forms.DateTimePicker();
            this.cboUserDetalle = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCajas)).BeginInit();
            this.panelResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimiento)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(933, 687);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabPage2.Controls.Add(this.btnBuscar);
            this.tabPage2.Controls.Add(this.dgvCajas);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.dtpHastaResumen);
            this.tabPage2.Controls.Add(this.dtpDesdeResumen);
            this.tabPage2.Controls.Add(this.cboUsuario);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.panelResumen);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(925, 659);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resumen";
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.Silver;
            this.btnBuscar.Location = new System.Drawing.Point(756, 29);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(105, 24);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dgvCajas
            // 
            this.dgvCajas.AllowUserToAddRows = false;
            this.dgvCajas.AllowUserToDeleteRows = false;
            this.dgvCajas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCajas.Location = new System.Drawing.Point(24, 74);
            this.dgvCajas.Name = "dgvCajas";
            this.dgvCajas.ReadOnly = true;
            this.dgvCajas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCajas.Size = new System.Drawing.Size(741, 165);
            this.dgvCajas.TabIndex = 7;
            this.dgvCajas.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCajas_CellEnter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(526, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Hasta";
            // 
            // dtpHastaResumen
            // 
            this.dtpHastaResumen.Location = new System.Drawing.Point(515, 31);
            this.dtpHastaResumen.Name = "dtpHastaResumen";
            this.dtpHastaResumen.Size = new System.Drawing.Size(226, 21);
            this.dtpHastaResumen.TabIndex = 5;
            // 
            // dtpDesdeResumen
            // 
            this.dtpDesdeResumen.Location = new System.Drawing.Point(274, 31);
            this.dtpDesdeResumen.Name = "dtpDesdeResumen";
            this.dtpDesdeResumen.Size = new System.Drawing.Size(226, 21);
            this.dtpDesdeResumen.TabIndex = 4;
            // 
            // cboUsuario
            // 
            this.cboUsuario.FormattingEnabled = true;
            this.cboUsuario.Location = new System.Drawing.Point(24, 30);
            this.cboUsuario.Name = "cboUsuario";
            this.cboUsuario.Size = new System.Drawing.Size(235, 23);
            this.cboUsuario.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(283, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Desde";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Usuario";
            // 
            // panelResumen
            // 
            this.panelResumen.Controls.Add(this.lblTotalHaber);
            this.panelResumen.Controls.Add(this.label7);
            this.panelResumen.Controls.Add(this.lblTotalDebe);
            this.panelResumen.Controls.Add(this.label5);
            this.panelResumen.Controls.Add(this.rtbObservaciones);
            this.panelResumen.Controls.Add(this.dgvMovimiento);
            this.panelResumen.Controls.Add(this.btnExportar);
            this.panelResumen.Location = new System.Drawing.Point(11, 245);
            this.panelResumen.Name = "panelResumen";
            this.panelResumen.Size = new System.Drawing.Size(909, 406);
            this.panelResumen.TabIndex = 0;
            // 
            // lblTotalHaber
            // 
            this.lblTotalHaber.AutoSize = true;
            this.lblTotalHaber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHaber.Location = new System.Drawing.Point(665, 278);
            this.lblTotalHaber.Name = "lblTotalHaber";
            this.lblTotalHaber.Size = new System.Drawing.Size(87, 16);
            this.lblTotalHaber.TabIndex = 20;
            this.lblTotalHaber.Text = "lblTotalDebe";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(564, 278);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "Total Haber:";
            // 
            // lblTotalDebe
            // 
            this.lblTotalDebe.AutoSize = true;
            this.lblTotalDebe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDebe.Location = new System.Drawing.Point(381, 278);
            this.lblTotalDebe.Name = "lblTotalDebe";
            this.lblTotalDebe.Size = new System.Drawing.Size(87, 16);
            this.lblTotalDebe.TabIndex = 18;
            this.lblTotalDebe.Text = "lblTotalDebe";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(285, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "Total Debe:";
            // 
            // rtbObservaciones
            // 
            this.rtbObservaciones.Location = new System.Drawing.Point(13, 307);
            this.rtbObservaciones.Name = "rtbObservaciones";
            this.rtbObservaciones.ReadOnly = true;
            this.rtbObservaciones.Size = new System.Drawing.Size(741, 81);
            this.rtbObservaciones.TabIndex = 10;
            this.rtbObservaciones.Text = "";
            // 
            // dgvMovimiento
            // 
            this.dgvMovimiento.AllowUserToAddRows = false;
            this.dgvMovimiento.AllowUserToDeleteRows = false;
            this.dgvMovimiento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovimiento.Location = new System.Drawing.Point(13, 17);
            this.dgvMovimiento.Name = "dgvMovimiento";
            this.dgvMovimiento.ReadOnly = true;
            this.dgvMovimiento.Size = new System.Drawing.Size(741, 245);
            this.dgvMovimiento.TabIndex = 9;
            // 
            // btnExportar
            // 
            this.btnExportar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExportar.Image = ((System.Drawing.Image)(resources.GetObject("btnExportar.Image")));
            this.btnExportar.Location = new System.Drawing.Point(857, 3);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(49, 28);
            this.btnExportar.TabIndex = 9;
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tabPage3.Controls.Add(this.lblEgresos);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.lblIngresos);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.btnDescargarDetalle);
            this.tabPage3.Controls.Add(this.dgvDetalle);
            this.tabPage3.Controls.Add(this.btnBuscarDetalle);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.dtpHastaDetalle);
            this.tabPage3.Controls.Add(this.dtpDesdeDetalle);
            this.tabPage3.Controls.Add(this.cboUserDetalle);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(925, 659);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Detalle";
            // 
            // lblEgresos
            // 
            this.lblEgresos.AutoSize = true;
            this.lblEgresos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEgresos.Location = new System.Drawing.Point(402, 619);
            this.lblEgresos.Name = "lblEgresos";
            this.lblEgresos.Size = new System.Drawing.Size(87, 16);
            this.lblEgresos.TabIndex = 24;
            this.lblEgresos.Text = "lblTotalDebe";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(289, 619);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 16);
            this.label10.TabIndex = 23;
            this.label10.Text = "Total Egresos:";
            // 
            // lblIngresos
            // 
            this.lblIngresos.AutoSize = true;
            this.lblIngresos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIngresos.Location = new System.Drawing.Point(131, 619);
            this.lblIngresos.Name = "lblIngresos";
            this.lblIngresos.Size = new System.Drawing.Size(52, 16);
            this.lblIngresos.TabIndex = 22;
            this.lblIngresos.Text = "label11";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(13, 619);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 16);
            this.label12.TabIndex = 21;
            this.label12.Text = "Total Ingresos:";
            // 
            // btnDescargarDetalle
            // 
            this.btnDescargarDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDescargarDetalle.Image = ((System.Drawing.Image)(resources.GetObject("btnDescargarDetalle.Image")));
            this.btnDescargarDetalle.Location = new System.Drawing.Point(868, 79);
            this.btnDescargarDetalle.Name = "btnDescargarDetalle";
            this.btnDescargarDetalle.Size = new System.Drawing.Size(49, 28);
            this.btnDescargarDetalle.TabIndex = 17;
            this.btnDescargarDetalle.UseVisualStyleBackColor = true;
            this.btnDescargarDetalle.Click += new System.EventHandler(this.btnDescargarDetalle_Click);
            // 
            // dgvDetalle
            // 
            this.dgvDetalle.AllowUserToAddRows = false;
            this.dgvDetalle.AllowUserToDeleteRows = false;
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Location = new System.Drawing.Point(13, 79);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.ReadOnly = true;
            this.dgvDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalle.Size = new System.Drawing.Size(837, 526);
            this.dgvDetalle.TabIndex = 16;
            // 
            // btnBuscarDetalle
            // 
            this.btnBuscarDetalle.BackColor = System.Drawing.Color.Silver;
            this.btnBuscarDetalle.Location = new System.Drawing.Point(745, 31);
            this.btnBuscarDetalle.Name = "btnBuscarDetalle";
            this.btnBuscarDetalle.Size = new System.Drawing.Size(105, 24);
            this.btnBuscarDetalle.TabIndex = 15;
            this.btnBuscarDetalle.Text = "Buscar";
            this.btnBuscarDetalle.UseVisualStyleBackColor = false;
            this.btnBuscarDetalle.Click += new System.EventHandler(this.btnBuscarDetalle_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(515, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 15);
            this.label4.TabIndex = 14;
            this.label4.Text = "Hasta";
            // 
            // dtpHastaDetalle
            // 
            this.dtpHastaDetalle.Location = new System.Drawing.Point(504, 33);
            this.dtpHastaDetalle.Name = "dtpHastaDetalle";
            this.dtpHastaDetalle.Size = new System.Drawing.Size(226, 21);
            this.dtpHastaDetalle.TabIndex = 13;
            // 
            // dtpDesdeDetalle
            // 
            this.dtpDesdeDetalle.Location = new System.Drawing.Point(263, 33);
            this.dtpDesdeDetalle.Name = "dtpDesdeDetalle";
            this.dtpDesdeDetalle.Size = new System.Drawing.Size(226, 21);
            this.dtpDesdeDetalle.TabIndex = 12;
            // 
            // cboUserDetalle
            // 
            this.cboUserDetalle.FormattingEnabled = true;
            this.cboUserDetalle.Location = new System.Drawing.Point(13, 32);
            this.cboUserDetalle.Name = "cboUserDetalle";
            this.cboUserDetalle.Size = new System.Drawing.Size(235, 23);
            this.cboUserDetalle.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(272, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "Desde";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 15);
            this.label8.TabIndex = 9;
            this.label8.Text = "Usuario";
            // 
            // frmAuditoriaCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(933, 687);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAuditoriaCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auditoria Caja";
            this.Load += new System.EventHandler(this.frmAuditoriaCaja_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCajas)).EndInit();
            this.panelResumen.ResumeLayout(false);
            this.panelResumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimiento)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cboUsuario;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelResumen;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DataGridView dgvCajas;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpHastaResumen;
        private System.Windows.Forms.DateTimePicker dtpDesdeResumen;
        private System.Windows.Forms.RichTextBox rtbObservaciones;
        private System.Windows.Forms.DataGridView dgvMovimiento;
        private System.Windows.Forms.Label lblTotalHaber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTotalDebe;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBuscarDetalle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpHastaDetalle;
        private System.Windows.Forms.DateTimePicker dtpDesdeDetalle;
        private System.Windows.Forms.ComboBox cboUserDetalle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.Button btnDescargarDetalle;
        private System.Windows.Forms.Label lblEgresos;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblIngresos;
        private System.Windows.Forms.Label label12;
    }
}