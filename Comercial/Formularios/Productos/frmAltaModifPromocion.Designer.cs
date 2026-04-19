namespace Comercial.Formularios.Productos
{
    partial class frmAltaModifPromocion
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAltaModifPromocion));
            this.lblProducto = new System.Windows.Forms.Label();
            this.cboProducto = new System.Windows.Forms.ComboBox();
            this.chkActiva = new System.Windows.Forms.CheckBox();
            this.chkFechas = new System.Windows.Forms.CheckBox();
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.grpSlots = new System.Windows.Forms.GroupBox();
            this.dgvSlots = new System.Windows.Forms.DataGridView();
            this.btnAgregarSlot = new System.Windows.Forms.Button();
            this.btnQuitarSlot = new System.Windows.Forms.Button();
            this.grpProductosSlot = new System.Windows.Forms.GroupBox();
            this.dgvSlotProductos = new System.Windows.Forms.DataGridView();
            this.btnAgregarProductoSlot = new System.Windows.Forms.Button();
            this.btnQuitarProductoSlot = new System.Windows.Forms.Button();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.grpSlots.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSlots)).BeginInit();
            this.grpProductosSlot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSlotProductos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblProducto
            // 
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(10, 13);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(53, 13);
            this.lblProducto.TabIndex = 0;
            this.lblProducto.Text = "Producto:";
            // 
            // cboProducto
            // 
            this.cboProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProducto.Location = new System.Drawing.Point(69, 10);
            this.cboProducto.Name = "cboProducto";
            this.cboProducto.Size = new System.Drawing.Size(301, 21);
            this.cboProducto.TabIndex = 0;
            // 
            // chkActiva
            // 
            this.chkActiva.AutoSize = true;
            this.chkActiva.Checked = true;
            this.chkActiva.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActiva.Location = new System.Drawing.Point(381, 12);
            this.chkActiva.Name = "chkActiva";
            this.chkActiva.Size = new System.Drawing.Size(56, 17);
            this.chkActiva.TabIndex = 1;
            this.chkActiva.Text = "Activa";
            // 
            // chkFechas
            // 
            this.chkFechas.AutoSize = true;
            this.chkFechas.Location = new System.Drawing.Point(10, 39);
            this.chkFechas.Name = "chkFechas";
            this.chkFechas.Size = new System.Drawing.Size(70, 17);
            this.chkFechas.TabIndex = 2;
            this.chkFechas.Text = "Vigencia:";
            this.chkFechas.CheckedChanged += new System.EventHandler(this.chkFechas_CheckedChanged);
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(84, 41);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(41, 13);
            this.lblDesde.TabIndex = 3;
            this.lblDesde.Text = "Desde:";
            // 
            // dtpDesde
            // 
            this.dtpDesde.Enabled = false;
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(129, 37);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(95, 20);
            this.dtpDesde.TabIndex = 3;
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(228, 41);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(38, 13);
            this.lblHasta.TabIndex = 4;
            this.lblHasta.Text = "Hasta:";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Enabled = false;
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(270, 37);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(95, 20);
            this.dtpHasta.TabIndex = 4;
            // 
            // grpSlots
            // 
            this.grpSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpSlots.Controls.Add(this.dgvSlots);
            this.grpSlots.Controls.Add(this.btnAgregarSlot);
            this.grpSlots.Controls.Add(this.btnQuitarSlot);
            this.grpSlots.Location = new System.Drawing.Point(10, 65);
            this.grpSlots.Name = "grpSlots";
            this.grpSlots.Size = new System.Drawing.Size(317, 329);
            this.grpSlots.TabIndex = 5;
            this.grpSlots.TabStop = false;
            this.grpSlots.Text = "Slots (grupos de productos)";
            // 
            // dgvSlots
            // 
            this.dgvSlots.AllowUserToAddRows = false;
            this.dgvSlots.AllowUserToDeleteRows = false;
            this.dgvSlots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSlots.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSlots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSlots.Location = new System.Drawing.Point(7, 19);
            this.dgvSlots.MultiSelect = false;
            this.dgvSlots.Name = "dgvSlots";
            this.dgvSlots.RowHeadersVisible = false;
            this.dgvSlots.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSlots.Size = new System.Drawing.Size(303, 269);
            this.dgvSlots.TabIndex = 0;
            this.dgvSlots.SelectionChanged += new System.EventHandler(this.dgvSlots_SelectionChanged);
            // 
            // btnAgregarSlot
            // 
            this.btnAgregarSlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgregarSlot.Location = new System.Drawing.Point(7, 296);
            this.btnAgregarSlot.Name = "btnAgregarSlot";
            this.btnAgregarSlot.Size = new System.Drawing.Size(94, 24);
            this.btnAgregarSlot.TabIndex = 1;
            this.btnAgregarSlot.Text = "+ Agregar Slot";
            this.btnAgregarSlot.UseVisualStyleBackColor = true;
            this.btnAgregarSlot.Click += new System.EventHandler(this.btnAgregarSlot_Click);
            // 
            // btnQuitarSlot
            // 
            this.btnQuitarSlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQuitarSlot.Location = new System.Drawing.Point(110, 296);
            this.btnQuitarSlot.Name = "btnQuitarSlot";
            this.btnQuitarSlot.Size = new System.Drawing.Size(94, 24);
            this.btnQuitarSlot.TabIndex = 2;
            this.btnQuitarSlot.Text = "- Quitar Slot";
            this.btnQuitarSlot.UseVisualStyleBackColor = true;
            this.btnQuitarSlot.Click += new System.EventHandler(this.btnQuitarSlot_Click);
            // 
            // grpProductosSlot
            // 
            this.grpProductosSlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpProductosSlot.Controls.Add(this.dgvSlotProductos);
            this.grpProductosSlot.Controls.Add(this.btnAgregarProductoSlot);
            this.grpProductosSlot.Controls.Add(this.btnQuitarProductoSlot);
            this.grpProductosSlot.Location = new System.Drawing.Point(336, 65);
            this.grpProductosSlot.Name = "grpProductosSlot";
            this.grpProductosSlot.Size = new System.Drawing.Size(317, 329);
            this.grpProductosSlot.TabIndex = 6;
            this.grpProductosSlot.TabStop = false;
            this.grpProductosSlot.Text = "Productos elegibles (slot seleccionado)";
            // 
            // dgvSlotProductos
            // 
            this.dgvSlotProductos.AllowUserToAddRows = false;
            this.dgvSlotProductos.AllowUserToDeleteRows = false;
            this.dgvSlotProductos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSlotProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSlotProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSlotProductos.Location = new System.Drawing.Point(7, 19);
            this.dgvSlotProductos.MultiSelect = false;
            this.dgvSlotProductos.Name = "dgvSlotProductos";
            this.dgvSlotProductos.ReadOnly = true;
            this.dgvSlotProductos.RowHeadersVisible = false;
            this.dgvSlotProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSlotProductos.Size = new System.Drawing.Size(303, 269);
            this.dgvSlotProductos.TabIndex = 0;
            // 
            // btnAgregarProductoSlot
            // 
            this.btnAgregarProductoSlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgregarProductoSlot.Location = new System.Drawing.Point(7, 296);
            this.btnAgregarProductoSlot.Name = "btnAgregarProductoSlot";
            this.btnAgregarProductoSlot.Size = new System.Drawing.Size(120, 24);
            this.btnAgregarProductoSlot.TabIndex = 1;
            this.btnAgregarProductoSlot.Text = "+ Agregar Producto";
            this.btnAgregarProductoSlot.UseVisualStyleBackColor = true;
            this.btnAgregarProductoSlot.Click += new System.EventHandler(this.btnAgregarProductoSlot_Click);
            // 
            // btnQuitarProductoSlot
            // 
            this.btnQuitarProductoSlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnQuitarProductoSlot.Location = new System.Drawing.Point(135, 296);
            this.btnQuitarProductoSlot.Name = "btnQuitarProductoSlot";
            this.btnQuitarProductoSlot.Size = new System.Drawing.Size(103, 24);
            this.btnQuitarProductoSlot.TabIndex = 2;
            this.btnQuitarProductoSlot.Text = "- Quitar Producto";
            this.btnQuitarProductoSlot.UseVisualStyleBackColor = true;
            this.btnQuitarProductoSlot.Click += new System.EventHandler(this.btnQuitarProductoSlot_Click);
            // 
            // btnGrabar
            // 
            this.btnGrabar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGrabar.Location = new System.Drawing.Point(490, 406);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(77, 26);
            this.btnGrabar.TabIndex = 7;
            this.btnGrabar.Text = "Grabar (F5)";
            this.btnGrabar.UseVisualStyleBackColor = true;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(576, 406);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(77, 26);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.Text = "Cancelar (F6)";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmAltaModifPromocion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 442);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.cboProducto);
            this.Controls.Add(this.chkActiva);
            this.Controls.Add(this.chkFechas);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.grpSlots);
            this.Controls.Add(this.grpProductosSlot);
            this.Controls.Add(this.btnGrabar);
            this.Controls.Add(this.btnCancelar);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(602, 439);
            this.Name = "frmAltaModifPromocion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alta / Modificación de Promoción";
            this.Load += new System.EventHandler(this.frmAltaModifPromocion_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAltaModifPromocion_KeyDown);
            this.grpSlots.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSlots)).EndInit();
            this.grpProductosSlot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSlotProductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.ComboBox cboProducto;
        private System.Windows.Forms.CheckBox chkActiva;
        private System.Windows.Forms.CheckBox chkFechas;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.GroupBox grpSlots;
        private System.Windows.Forms.DataGridView dgvSlots;
        private System.Windows.Forms.Button btnAgregarSlot;
        private System.Windows.Forms.Button btnQuitarSlot;
        private System.Windows.Forms.GroupBox grpProductosSlot;
        private System.Windows.Forms.DataGridView dgvSlotProductos;
        private System.Windows.Forms.Button btnAgregarProductoSlot;
        private System.Windows.Forms.Button btnQuitarProductoSlot;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
