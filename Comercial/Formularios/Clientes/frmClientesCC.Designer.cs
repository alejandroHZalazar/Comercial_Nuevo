
namespace Comercial.Formularios.Clientes
{
    partial class frmClientesCC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClientesCC));
            this.dgvCC = new System.Windows.Forms.DataGridView();
            this.btnCobrar = new System.Windows.Forms.Button();
            this.btnNC = new System.Windows.Forms.Button();
            this.txtSaldo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnND = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCC)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCC
            // 
            this.dgvCC.AllowUserToAddRows = false;
            this.dgvCC.AllowUserToDeleteRows = false;
            this.dgvCC.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvCC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCC.Location = new System.Drawing.Point(12, 12);
            this.dgvCC.Name = "dgvCC";
            this.dgvCC.ReadOnly = true;
            this.dgvCC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCC.Size = new System.Drawing.Size(678, 388);
            this.dgvCC.TabIndex = 0;
            this.dgvCC.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCC_CellMouseDoubleClick);
            // 
            // btnCobrar
            // 
            this.btnCobrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCobrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCobrar.Image = ((System.Drawing.Image)(resources.GetObject("btnCobrar.Image")));
            this.btnCobrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCobrar.Location = new System.Drawing.Point(696, 12);
            this.btnCobrar.Name = "btnCobrar";
            this.btnCobrar.Size = new System.Drawing.Size(171, 55);
            this.btnCobrar.TabIndex = 1;
            this.btnCobrar.Text = "    Cobrar [F2]";
            this.btnCobrar.UseVisualStyleBackColor = false;
            this.btnCobrar.Click += new System.EventHandler(this.btnCobrar_Click);
            // 
            // btnNC
            // 
            this.btnNC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnNC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNC.Image = ((System.Drawing.Image)(resources.GetObject("btnNC.Image")));
            this.btnNC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNC.Location = new System.Drawing.Point(696, 73);
            this.btnNC.Name = "btnNC";
            this.btnNC.Size = new System.Drawing.Size(171, 55);
            this.btnNC.TabIndex = 2;
            this.btnNC.Text = "Generar NC [F3]";
            this.btnNC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNC.UseVisualStyleBackColor = false;
            this.btnNC.Click += new System.EventHandler(this.btnNC_Click);
            // 
            // txtSaldo
            // 
            this.txtSaldo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaldo.Location = new System.Drawing.Point(539, 406);
            this.txtSaldo.Name = "txtSaldo";
            this.txtSaldo.Size = new System.Drawing.Size(151, 24);
            this.txtSaldo.TabIndex = 3;
            this.txtSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(480, 411);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Saldo:";
            // 
            // btnND
            // 
            this.btnND.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnND.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnND.Image = ((System.Drawing.Image)(resources.GetObject("btnND.Image")));
            this.btnND.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnND.Location = new System.Drawing.Point(696, 134);
            this.btnND.Name = "btnND";
            this.btnND.Size = new System.Drawing.Size(171, 55);
            this.btnND.TabIndex = 3;
            this.btnND.Text = "Generar ND [F4]";
            this.btnND.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnND.UseVisualStyleBackColor = false;
            this.btnND.Click += new System.EventHandler(this.btnND_Click);
            // 
            // frmClientesCC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(879, 440);
            this.Controls.Add(this.btnND);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSaldo);
            this.Controls.Add(this.btnNC);
            this.Controls.Add(this.btnCobrar);
            this.Controls.Add(this.dgvCC);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClientesCC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cuenta Corriente";
            this.Load += new System.EventHandler(this.frmClientesCC_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmClientesCC_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCC;
        private System.Windows.Forms.Button btnCobrar;
        private System.Windows.Forms.Button btnNC;
        private System.Windows.Forms.TextBox txtSaldo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnND;
    }
}