
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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImputacionVenta));
            this.dgvFormasPago = new System.Windows.Forms.DataGridView();
            this.btnCobrar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormasPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // dgvFormasPago
            //
            this.dgvFormasPago.AllowUserToAddRows = false;
            this.dgvFormasPago.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvFormasPago.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFormasPago.Location = new System.Drawing.Point(12, 210);
            this.dgvFormasPago.Name = "dgvFormasPago";
            this.dgvFormasPago.ReadOnly = true;
            this.dgvFormasPago.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFormasPago.Size = new System.Drawing.Size(706, 130);
            this.dgvFormasPago.TabIndex = 1;
            this.dgvFormasPago.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFormasPago_CellDoubleClick);
            this.dgvFormasPago.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvFormasPago_RowsRemoved);
            //
            // btnCobrar
            //
            this.btnCobrar.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnCobrar.ForeColor = System.Drawing.Color.White;
            this.btnCobrar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCobrar.Location = new System.Drawing.Point(440, 370);
            this.btnCobrar.Name = "btnCobrar";
            this.btnCobrar.Size = new System.Drawing.Size(136, 44);
            this.btnCobrar.TabIndex = 2;
            this.btnCobrar.Text = "Confirmar  [F3]";
            this.btnCobrar.UseVisualStyleBackColor = false;
            this.btnCobrar.Click += new System.EventHandler(this.btnCobrar_Click);
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            //
            // frmImputacionVenta
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(730, 430);
            this.Controls.Add(this.btnCobrar);
            this.Controls.Add(this.dgvFormasPago);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImputacionVenta";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cobros";
            this.Load += new System.EventHandler(this.frmImputacionVenta_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFormasPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFormasPago;
        private System.Windows.Forms.Button btnCobrar;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
