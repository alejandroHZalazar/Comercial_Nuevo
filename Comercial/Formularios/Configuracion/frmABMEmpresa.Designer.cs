
namespace Comercial.Formularios.Configuracion
{
    partial class frmABMEmpresa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmABMEmpresa));
            this.dgvParametros = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametros)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvParametros
            // 
            this.dgvParametros.AllowUserToAddRows = false;
            this.dgvParametros.AllowUserToDeleteRows = false;
            this.dgvParametros.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvParametros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParametros.Location = new System.Drawing.Point(12, 12);
            this.dgvParametros.Name = "dgvParametros";
            this.dgvParametros.Size = new System.Drawing.Size(609, 495);
            this.dgvParametros.TabIndex = 0;
            this.dgvParametros.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvParametros_CellEndEdit);
            // 
            // frmABMEmpresa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(633, 519);
            this.Controls.Add(this.dgvParametros);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmABMEmpresa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuración Parámetros";
            this.Load += new System.EventHandler(this.frmABMEmpresa_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametros)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvParametros;
    }
}