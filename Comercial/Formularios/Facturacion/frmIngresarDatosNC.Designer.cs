namespace Comercial.Formularios.Facturacion
{
    partial class frmIngresarDatosNC
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIngresarDatosNC));
            this.tlpDatos = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFacturaAsociada = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.nudImporte = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cboIva = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboIIBB = new System.Windows.Forms.ComboBox();
            this.btnSiguiente = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tlpDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImporte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpDatos
            // 
            this.tlpDatos.AutoSize = true;
            this.tlpDatos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpDatos.ColumnCount = 4;
            this.tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDatos.Controls.Add(this.label1, 0, 0);
            this.tlpDatos.Controls.Add(this.txtFacturaAsociada, 1, 0);
            this.tlpDatos.Controls.Add(this.label2, 2, 0);
            this.tlpDatos.Controls.Add(this.dtpFecha, 3, 0);
            this.tlpDatos.Controls.Add(this.label3, 0, 1);
            this.tlpDatos.Controls.Add(this.nudImporte, 1, 1);
            this.tlpDatos.Controls.Add(this.label4, 2, 1);
            this.tlpDatos.Controls.Add(this.cboIva, 3, 1);
            this.tlpDatos.Controls.Add(this.label5, 0, 2);
            this.tlpDatos.Controls.Add(this.cboIIBB, 1, 2);
            this.tlpDatos.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpDatos.Location = new System.Drawing.Point(0, 0);
            this.tlpDatos.Name = "tlpDatos";
            this.tlpDatos.Padding = new System.Windows.Forms.Padding(16, 12, 16, 0);
            this.tlpDatos.RowCount = 3;
            this.tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDatos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDatos.Size = new System.Drawing.Size(660, 115);
            this.tlpDatos.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nro. Factura Asociada";
            // 
            // txtFacturaAsociada
            // 
            this.txtFacturaAsociada.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFacturaAsociada.Location = new System.Drawing.Point(157, 18);
            this.txtFacturaAsociada.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtFacturaAsociada.Name = "txtFacturaAsociada";
            this.txtFacturaAsociada.Size = new System.Drawing.Size(161, 23);
            this.txtFacturaAsociada.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(335, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(14, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fecha Factura Asociada";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFecha.Location = new System.Drawing.Point(480, 18);
            this.dtpFecha.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(161, 23);
            this.dtpFecha.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 57);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Importe Nota Crédito";
            // 
            // nudImporte
            // 
            this.nudImporte.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nudImporte.DecimalPlaces = 2;
            this.nudImporte.Location = new System.Drawing.Point(157, 53);
            this.nudImporte.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.nudImporte.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudImporte.Name = "nudImporte";
            this.nudImporte.Size = new System.Drawing.Size(161, 23);
            this.nudImporte.TabIndex = 2;
            this.nudImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(335, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(14, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "IVA";
            // 
            // cboIva
            // 
            this.cboIva.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboIva.FormattingEnabled = true;
            this.cboIva.Location = new System.Drawing.Point(480, 54);
            this.cboIva.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cboIva.Name = "cboIva";
            this.cboIva.Size = new System.Drawing.Size(161, 23);
            this.cboIva.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(19, 91);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Ingresos Brutos";
            // 
            // cboIIBB
            // 
            this.cboIIBB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboIIBB.FormattingEnabled = true;
            this.cboIIBB.Location = new System.Drawing.Point(157, 88);
            this.cboIIBB.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.cboIIBB.Name = "cboIIBB";
            this.cboIIBB.Size = new System.Drawing.Size(161, 23);
            this.cboIIBB.TabIndex = 4;
            // 
            // btnSiguiente
            // 
            this.btnSiguiente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSiguiente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSiguiente.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSiguiente.Location = new System.Drawing.Point(504, 168);
            this.btnSiguiente.Name = "btnSiguiente";
            this.btnSiguiente.Size = new System.Drawing.Size(140, 40);
            this.btnSiguiente.TabIndex = 5;
            this.btnSiguiente.Text = "Siguiente [F5]";
            this.btnSiguiente.UseVisualStyleBackColor = false;
            this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmIngresarDatosNC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(660, 224);
            this.Controls.Add(this.btnSiguiente);
            this.Controls.Add(this.tlpDatos);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(620, 260);
            this.Name = "frmIngresarDatosNC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Datos Nota de Crédito";
            this.Load += new System.EventHandler(this.frmIngresarDatosNC_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmIngresarDatosNC_KeyDown);
            this.tlpDatos.ResumeLayout(false);
            this.tlpDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImporte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpDatos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFacturaAsociada;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.NumericUpDown nudImporte;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboIva;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboIIBB;
        private System.Windows.Forms.Button btnSiguiente;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
