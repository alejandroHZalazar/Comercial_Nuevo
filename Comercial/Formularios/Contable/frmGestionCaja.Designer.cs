
namespace Comercial.Formularios.Contable
{
    partial class frmGestionCaja
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGestionCaja));
            this.label1 = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.dgvMovCaja = new System.Windows.Forms.DataGridView();
            this.btnAbrirCaja = new System.Windows.Forms.Button();
            this.btnCerrarCaja = new System.Windows.Forms.Button();
            this.btnIngresoDinero = new System.Windows.Forms.Button();
            this.btnEgresoDinero = new System.Windows.Forms.Button();
            this.btnGastos = new System.Windows.Forms.Button();
            this.btnArqueo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEfectivo = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTotalDebe = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTotalHaber = new System.Windows.Forms.Label();
            this.btnPagoProveedores = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovCaja)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuario:";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.Location = new System.Drawing.Point(84, 9);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(58, 16);
            this.lblUsuario.TabIndex = 1;
            this.lblUsuario.Text = "Usuario:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(246, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Estado:";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(313, 9);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(58, 16);
            this.lblEstado.TabIndex = 3;
            this.lblEstado.Text = "Usuario:";
            // 
            // dgvMovCaja
            // 
            this.dgvMovCaja.AllowUserToAddRows = false;
            this.dgvMovCaja.AllowUserToDeleteRows = false;
            this.dgvMovCaja.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvMovCaja.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovCaja.Location = new System.Drawing.Point(12, 49);
            this.dgvMovCaja.Name = "dgvMovCaja";
            this.dgvMovCaja.ReadOnly = true;
            this.dgvMovCaja.Size = new System.Drawing.Size(802, 383);
            this.dgvMovCaja.TabIndex = 4;
            // 
            // btnAbrirCaja
            // 
            this.btnAbrirCaja.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnAbrirCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrirCaja.Location = new System.Drawing.Point(606, 5);
            this.btnAbrirCaja.Name = "btnAbrirCaja";
            this.btnAbrirCaja.Size = new System.Drawing.Size(101, 38);
            this.btnAbrirCaja.TabIndex = 0;
            this.btnAbrirCaja.Text = "Abrir Caja";
            this.btnAbrirCaja.UseVisualStyleBackColor = false;
            this.btnAbrirCaja.Click += new System.EventHandler(this.btnAbrirCaja_Click);
            // 
            // btnCerrarCaja
            // 
            this.btnCerrarCaja.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCerrarCaja.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrarCaja.Location = new System.Drawing.Point(713, 5);
            this.btnCerrarCaja.Name = "btnCerrarCaja";
            this.btnCerrarCaja.Size = new System.Drawing.Size(101, 38);
            this.btnCerrarCaja.TabIndex = 1;
            this.btnCerrarCaja.Text = "Cerrar Caja";
            this.btnCerrarCaja.UseVisualStyleBackColor = false;
            this.btnCerrarCaja.Click += new System.EventHandler(this.btnCerrarCaja_Click);
            // 
            // btnIngresoDinero
            // 
            this.btnIngresoDinero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnIngresoDinero.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresoDinero.Image = ((System.Drawing.Image)(resources.GetObject("btnIngresoDinero.Image")));
            this.btnIngresoDinero.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIngresoDinero.Location = new System.Drawing.Point(820, 49);
            this.btnIngresoDinero.Name = "btnIngresoDinero";
            this.btnIngresoDinero.Size = new System.Drawing.Size(151, 39);
            this.btnIngresoDinero.TabIndex = 2;
            this.btnIngresoDinero.Text = "Ingreso Dinero";
            this.btnIngresoDinero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIngresoDinero.UseVisualStyleBackColor = false;
            this.btnIngresoDinero.Click += new System.EventHandler(this.btnIngresoDinero_Click);
            // 
            // btnEgresoDinero
            // 
            this.btnEgresoDinero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEgresoDinero.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEgresoDinero.Image = ((System.Drawing.Image)(resources.GetObject("btnEgresoDinero.Image")));
            this.btnEgresoDinero.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEgresoDinero.Location = new System.Drawing.Point(820, 93);
            this.btnEgresoDinero.Name = "btnEgresoDinero";
            this.btnEgresoDinero.Size = new System.Drawing.Size(151, 39);
            this.btnEgresoDinero.TabIndex = 3;
            this.btnEgresoDinero.Text = "Retiro Dinero";
            this.btnEgresoDinero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEgresoDinero.UseVisualStyleBackColor = false;
            this.btnEgresoDinero.Click += new System.EventHandler(this.btnEgresoDinero_Click);
            // 
            // btnGastos
            // 
            this.btnGastos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnGastos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGastos.Image = ((System.Drawing.Image)(resources.GetObject("btnGastos.Image")));
            this.btnGastos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGastos.Location = new System.Drawing.Point(820, 137);
            this.btnGastos.Name = "btnGastos";
            this.btnGastos.Size = new System.Drawing.Size(151, 39);
            this.btnGastos.TabIndex = 4;
            this.btnGastos.Text = "Ingresar Gastos";
            this.btnGastos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGastos.UseVisualStyleBackColor = false;
            this.btnGastos.Click += new System.EventHandler(this.btnGastos_Click);
            // 
            // btnArqueo
            // 
            this.btnArqueo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnArqueo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArqueo.Image = ((System.Drawing.Image)(resources.GetObject("btnArqueo.Image")));
            this.btnArqueo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnArqueo.Location = new System.Drawing.Point(820, 181);
            this.btnArqueo.Name = "btnArqueo";
            this.btnArqueo.Size = new System.Drawing.Size(151, 38);
            this.btnArqueo.TabIndex = 5;
            this.btnArqueo.Text = "Arqueo Caja";
            this.btnArqueo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnArqueo.UseVisualStyleBackColor = false;
            this.btnArqueo.Click += new System.EventHandler(this.btnArqueo_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 479);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Efectivo: ";
            // 
            // lblEfectivo
            // 
            this.lblEfectivo.AutoSize = true;
            this.lblEfectivo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEfectivo.Location = new System.Drawing.Point(90, 479);
            this.lblEfectivo.Name = "lblEfectivo";
            this.lblEfectivo.Size = new System.Drawing.Size(58, 16);
            this.lblEfectivo.TabIndex = 12;
            this.lblEfectivo.Text = "Usuario:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 448);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 16);
            this.label5.TabIndex = 13;
            this.label5.Text = "Total Debe:";
            // 
            // lblTotalDebe
            // 
            this.lblTotalDebe.AutoSize = true;
            this.lblTotalDebe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDebe.Location = new System.Drawing.Point(108, 448);
            this.lblTotalDebe.Name = "lblTotalDebe";
            this.lblTotalDebe.Size = new System.Drawing.Size(87, 16);
            this.lblTotalDebe.TabIndex = 14;
            this.lblTotalDebe.Text = "lblTotalDebe";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(291, 448);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Total Haber:";
            // 
            // lblTotalHaber
            // 
            this.lblTotalHaber.AutoSize = true;
            this.lblTotalHaber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalHaber.Location = new System.Drawing.Point(392, 448);
            this.lblTotalHaber.Name = "lblTotalHaber";
            this.lblTotalHaber.Size = new System.Drawing.Size(87, 16);
            this.lblTotalHaber.TabIndex = 16;
            this.lblTotalHaber.Text = "lblTotalDebe";
            // 
            // btnPagoProveedores
            // 
            this.btnPagoProveedores.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnPagoProveedores.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagoProveedores.Image = ((System.Drawing.Image)(resources.GetObject("btnPagoProveedores.Image")));
            this.btnPagoProveedores.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPagoProveedores.Location = new System.Drawing.Point(820, 225);
            this.btnPagoProveedores.Name = "btnPagoProveedores";
            this.btnPagoProveedores.Size = new System.Drawing.Size(151, 38);
            this.btnPagoProveedores.TabIndex = 17;
            this.btnPagoProveedores.Text = "Pago Proveedores";
            this.btnPagoProveedores.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPagoProveedores.UseVisualStyleBackColor = false;
            this.btnPagoProveedores.Click += new System.EventHandler(this.btnPagoProveedores_Click);
            // 
            // frmGestionCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(974, 519);
            this.Controls.Add(this.btnPagoProveedores);
            this.Controls.Add(this.lblTotalHaber);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblTotalDebe);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblEfectivo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnArqueo);
            this.Controls.Add(this.btnGastos);
            this.Controls.Add(this.btnEgresoDinero);
            this.Controls.Add(this.btnIngresoDinero);
            this.Controls.Add(this.btnCerrarCaja);
            this.Controls.Add(this.btnAbrirCaja);
            this.Controls.Add(this.dgvMovCaja);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGestionCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Caja";
            this.Load += new System.EventHandler(this.frmGestionCaja_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovCaja)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.DataGridView dgvMovCaja;
        private System.Windows.Forms.Button btnAbrirCaja;
        private System.Windows.Forms.Button btnCerrarCaja;
        private System.Windows.Forms.Button btnIngresoDinero;
        private System.Windows.Forms.Button btnEgresoDinero;
        private System.Windows.Forms.Button btnGastos;
        private System.Windows.Forms.Button btnArqueo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblEfectivo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTotalDebe;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTotalHaber;
        private System.Windows.Forms.Button btnPagoProveedores;
    }
}