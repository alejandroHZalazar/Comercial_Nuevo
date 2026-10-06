namespace Comercial.Formularios.Proveedores
{
    partial class frmListaProductosAPedir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListaProductosAPedir));
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvAltStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvListaProdAPedir = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCant = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPrecioProv = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrecioLista = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCosto = new System.Windows.Forms.TextBox();
            this.btnCargar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.btnImprimir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaProdAPedir)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvListaProdAPedir
            // 
            this.dgvListaProdAPedir.AllowUserToAddRows = false;
            this.dgvListaProdAPedir.AllowUserToDeleteRows = false;
            this.dgvListaProdAPedir.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvListaProdAPedir.AlternatingRowsDefaultCellStyle = dgvAltStyle;
            this.dgvListaProdAPedir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            this.dgvListaProdAPedir.BackgroundColor = System.Drawing.Color.White;
            this.dgvListaProdAPedir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dgvAltStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            dgvHeaderStyle.BackColor = System.Drawing.Color.DimGray;
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvHeaderStyle.ForeColor = System.Drawing.Color.White;
            dgvHeaderStyle.SelectionBackColor = System.Drawing.Color.DimGray;
            dgvHeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.dgvListaProdAPedir.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvListaProdAPedir.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListaProdAPedir.EnableHeadersVisualStyles = false;
            this.dgvListaProdAPedir.RowHeadersVisible = false;
            this.dgvListaProdAPedir.Location = new System.Drawing.Point(12, 84);
            this.dgvListaProdAPedir.Name = "dgvListaProdAPedir";
            this.dgvListaProdAPedir.Size = new System.Drawing.Size(1185, 531);
            this.dgvListaProdAPedir.TabIndex = 0;
            this.dgvListaProdAPedir.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListaProdAPedir_CellValueChanged);
            this.dgvListaProdAPedir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvListaProdAPedir_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.label1.Location = new System.Drawing.Point(12, 630);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cant. Prod";
            // 
            // txtCant
            // 
            this.txtCant.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.txtCant.Location = new System.Drawing.Point(85, 626);
            this.txtCant.Name = "txtCant";
            this.txtCant.ReadOnly = true;
            this.txtCant.Size = new System.Drawing.Size(100, 23);
            this.txtCant.TabIndex = 2;
            this.txtCant.Text = "0";
            this.txtCant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.label2.Location = new System.Drawing.Point(195, 630);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tot. Precio Prov.";
            // 
            // txtPrecioProv
            // 
            this.txtPrecioProv.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.txtPrecioProv.Location = new System.Drawing.Point(298, 626);
            this.txtPrecioProv.Name = "txtPrecioProv";
            this.txtPrecioProv.ReadOnly = true;
            this.txtPrecioProv.Size = new System.Drawing.Size(100, 23);
            this.txtPrecioProv.TabIndex = 4;
            this.txtPrecioProv.Text = "0";
            this.txtPrecioProv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.label3.Location = new System.Drawing.Point(408, 630);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tot. Precio Lista";
            // 
            // txtPrecioLista
            // 
            this.txtPrecioLista.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.txtPrecioLista.Location = new System.Drawing.Point(508, 626);
            this.txtPrecioLista.Name = "txtPrecioLista";
            this.txtPrecioLista.ReadOnly = true;
            this.txtPrecioLista.Size = new System.Drawing.Size(100, 23);
            this.txtPrecioLista.TabIndex = 6;
            this.txtPrecioLista.Text = "0";
            this.txtPrecioLista.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.label4.Location = new System.Drawing.Point(970, 629);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Tot. Precio Costo";
            // 
            // txtCosto
            // 
            this.txtCosto.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCosto.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.txtCosto.Location = new System.Drawing.Point(1097, 625);
            this.txtCosto.Name = "txtCosto";
            this.txtCosto.ReadOnly = true;
            this.txtCosto.Size = new System.Drawing.Size(100, 25);
            this.txtCosto.TabIndex = 8;
            this.txtCosto.Text = "0";
            this.txtCosto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCargar
            // 
            this.btnCargar.BackColor = System.Drawing.Color.Silver;
            this.btnCargar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCargar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCargar.Image = global::Comercial.Properties.Resources.play_button;
            this.btnCargar.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnCargar.Location = new System.Drawing.Point(1097, 659);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(100, 30);
            this.btnCargar.TabIndex = 9;
            this.btnCargar.Text = "Cargar [F2]";
            this.btnCargar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnCargar.UseVisualStyleBackColor = false;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.dtpDesde);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpHasta);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(695, 66);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Período de movimientos";
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.Gray;
            this.btnBuscar.Image = global::Comercial.Properties.Resources.musica_searcher;
            this.btnBuscar.Location = new System.Drawing.Point(609, 16);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(54, 36);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // dtpDesde
            // 
            this.dtpDesde.Location = new System.Drawing.Point(67, 24);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(235, 23);
            this.dtpDesde.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "Desde";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Location = new System.Drawing.Point(363, 24);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(235, 23);
            this.dtpHasta.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(313, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Hasta";
            // 
            // btnImprimir
            // 
            this.btnImprimir.BackColor = System.Drawing.Color.Gray;
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Image = global::Comercial.Properties.Resources.printer_1;
            this.btnImprimir.Location = new System.Drawing.Point(713, 30);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(75, 39);
            this.btnImprimir.TabIndex = 11;
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // frmListaProductosAPedir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1209, 700);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCargar);
            this.Controls.Add(this.txtCosto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPrecioLista);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPrecioProv);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCant);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvListaProdAPedir);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(880, 500);
            this.MinimizeBox = false;
            this.Name = "frmListaProductosAPedir";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Productos a Pedir";
            this.Load += new System.EventHandler(this.frmListaProductosAPedir_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmListaProductosAPedir_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaProdAPedir)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvListaProdAPedir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCant;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPrecioProv;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPrecioLista;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCosto;
        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnImprimir;
    }
}
