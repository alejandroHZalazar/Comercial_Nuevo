
namespace Comercial.Formularios.Contable
{
    partial class frmPagoProveedores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPagoProveedores));
            this.cboProveedor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudPagoProveedor = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbObservacion = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPagoProveedores = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudPagoProveedor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboProveedor
            // 
            this.cboProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboProveedor.FormattingEnabled = true;
            this.cboProveedor.Location = new System.Drawing.Point(104, 12);
            this.cboProveedor.Name = "cboProveedor";
            this.cboProveedor.Size = new System.Drawing.Size(279, 23);
            this.cboProveedor.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Proveedor:";
            // 
            // nudPagoProveedor
            // 
            this.nudPagoProveedor.DecimalPlaces = 2;
            this.nudPagoProveedor.Location = new System.Drawing.Point(147, 53);
            this.nudPagoProveedor.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudPagoProveedor.Name = "nudPagoProveedor";
            this.nudPagoProveedor.Size = new System.Drawing.Size(120, 21);
            this.nudPagoProveedor.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Dinero a Ingresar:";
            // 
            // rtbObservacion
            // 
            this.rtbObservacion.Location = new System.Drawing.Point(13, 110);
            this.rtbObservacion.MaxLength = 100;
            this.rtbObservacion.Name = "rtbObservacion";
            this.rtbObservacion.Size = new System.Drawing.Size(370, 66);
            this.rtbObservacion.TabIndex = 2;
            this.rtbObservacion.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Observaciones:";
            // 
            // btnPagoProveedores
            // 
            this.btnPagoProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnPagoProveedores.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagoProveedores.Image = ((System.Drawing.Image)(resources.GetObject("btnPagoProveedores.Image")));
            this.btnPagoProveedores.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPagoProveedores.Location = new System.Drawing.Point(238, 191);
            this.btnPagoProveedores.Name = "btnPagoProveedores";
            this.btnPagoProveedores.Size = new System.Drawing.Size(145, 38);
            this.btnPagoProveedores.TabIndex = 3;
            this.btnPagoProveedores.Text = "Pagar Proveedor";
            this.btnPagoProveedores.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPagoProveedores.UseVisualStyleBackColor = false;
            this.btnPagoProveedores.Click += new System.EventHandler(this.btnPagoProveedores_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmPagoProveedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(394, 234);
            this.Controls.Add(this.btnPagoProveedores);
            this.Controls.Add(this.rtbObservacion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudPagoProveedor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboProveedor);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPagoProveedores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pago Proveedores";
            this.Load += new System.EventHandler(this.frmPagoProveedores_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudPagoProveedor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboProveedor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudPagoProveedor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtbObservacion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPagoProveedores;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}