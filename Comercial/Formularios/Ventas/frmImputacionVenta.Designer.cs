
namespace Comercial.Formularios.Ventas
{
    partial class frmImputacionVenta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImputacionVenta));
            this.label5 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.dgvFormasPago = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.nudImputar = new System.Windows.Forms.NumericUpDown();
            this.btnCobrar = new System.Windows.Forms.Button();
            this.btnAgregarFormasPago = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormasPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImputar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 18);
            this.label5.TabIndex = 44;
            this.label5.Text = "Total:";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(61, 17);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(45, 18);
            this.lblTotal.TabIndex = 45;
            this.lblTotal.Text = "Total:";
            // 
            // dgvFormasPago
            // 
            this.dgvFormasPago.AllowUserToAddRows = false;
            this.dgvFormasPago.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvFormasPago.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFormasPago.Location = new System.Drawing.Point(15, 50);
            this.dgvFormasPago.Name = "dgvFormasPago";
            this.dgvFormasPago.ReadOnly = true;
            this.dgvFormasPago.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFormasPago.Size = new System.Drawing.Size(704, 150);
            this.dgvFormasPago.TabIndex = 1;
            this.dgvFormasPago.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFormasPago_CellDoubleClick);
            this.dgvFormasPago.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvFormasPago_RowsRemoved);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 20);
            this.label2.TabIndex = 47;
            this.label2.Text = "Total a imputar:";
            // 
            // nudImputar
            // 
            this.nudImputar.DecimalPlaces = 2;
            this.nudImputar.Enabled = false;
            this.nudImputar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudImputar.Location = new System.Drawing.Point(139, 224);
            this.nudImputar.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudImputar.Name = "nudImputar";
            this.nudImputar.Size = new System.Drawing.Size(156, 24);
            this.nudImputar.TabIndex = 48;
            // 
            // btnCobrar
            // 
            this.btnCobrar.BackColor = System.Drawing.Color.Silver;
            this.btnCobrar.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCobrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCobrar.Image")));
            this.btnCobrar.Location = new System.Drawing.Point(565, 206);
            this.btnCobrar.Name = "btnCobrar";
            this.btnCobrar.Size = new System.Drawing.Size(154, 60);
            this.btnCobrar.TabIndex = 2;
            this.btnCobrar.Text = "Cobrar [F3]";
            this.btnCobrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCobrar.UseVisualStyleBackColor = false;
            this.btnCobrar.Click += new System.EventHandler(this.btnCobrar_Click);
            // 
            // btnAgregarFormasPago
            // 
            this.btnAgregarFormasPago.BackColor = System.Drawing.Color.Silver;
            this.btnAgregarFormasPago.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarFormasPago.Image = global::Comercial.Properties.Resources.plus;
            this.btnAgregarFormasPago.Location = new System.Drawing.Point(532, 9);
            this.btnAgregarFormasPago.Name = "btnAgregarFormasPago";
            this.btnAgregarFormasPago.Size = new System.Drawing.Size(187, 35);
            this.btnAgregarFormasPago.TabIndex = 0;
            this.btnAgregarFormasPago.Text = "Agregar Formas Pago [F2]";
            this.btnAgregarFormasPago.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgregarFormasPago.UseVisualStyleBackColor = false;
            this.btnAgregarFormasPago.Click += new System.EventHandler(this.btnAgregarFormasPago_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmImputacionVenta
            // 
            this.AcceptButton = this.btnCobrar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(731, 278);
            this.Controls.Add(this.btnAgregarFormasPago);
            this.Controls.Add(this.btnCobrar);
            this.Controls.Add(this.nudImputar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvFormasPago);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label5);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImputacionVenta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cobros";
            this.Load += new System.EventHandler(this.frmImputacionVenta_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmImputacionVenta_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormasPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImputar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.DataGridView dgvFormasPago;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudImputar;
        private System.Windows.Forms.Button btnCobrar;
        private System.Windows.Forms.Button btnAgregarFormasPago;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}