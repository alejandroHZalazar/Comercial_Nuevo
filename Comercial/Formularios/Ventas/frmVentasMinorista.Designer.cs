namespace Comercial.Formularios.Ventas
{
    partial class frmVentasMinorista
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVentasMinorista));
            this.lblBuscarCliente = new System.Windows.Forms.Label();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.lblClienteNombre = new System.Windows.Forms.Label();
            this.btnAltaCliente = new System.Windows.Forms.Button();
            this.lbCliente = new System.Windows.Forms.ListBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.cboFiltro = new System.Windows.Forms.ComboBox();
            this.txtFiltro = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.lbDesc = new System.Windows.Forms.ListBox();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.codBarras = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precioSinIva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precioConIva = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DescRec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subtotalSIVA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pedido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fraccionado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dolarizado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDtoLbl = new System.Windows.Forms.Label();
            this.nudDescuento = new System.Windows.Forms.NumericUpDown();
            this.lblRecLbl = new System.Windows.Forms.Label();
            this.nudRecargo = new System.Windows.Forms.NumericUpDown();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.lblTotalLabel = new System.Windows.Forms.Label();
            this.txtTotGeneral = new System.Windows.Forms.TextBox();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.timerBusq = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorkerClientes = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDescuento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecargo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBuscarCliente
            // 
            this.lblBuscarCliente.AutoSize = true;
            this.lblBuscarCliente.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblBuscarCliente.Location = new System.Drawing.Point(9, 10);
            this.lblBuscarCliente.Name = "lblBuscarCliente";
            this.lblBuscarCliente.Size = new System.Drawing.Size(50, 17);
            this.lblBuscarCliente.TabIndex = 0;
            this.lblBuscarCliente.Text = "Cliente:";
            // 
            // txtCliente
            // 
            this.txtCliente.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtCliente.Location = new System.Drawing.Point(62, 8);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(172, 24);
            this.txtCliente.TabIndex = 1;
            this.txtCliente.TextChanged += new System.EventHandler(this.txtCliente_TextChanged);
            this.txtCliente.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCliente_KeyDown);
            // 
            // lblClienteNombre
            // 
            this.lblClienteNombre.AutoSize = true;
            this.lblClienteNombre.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblClienteNombre.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblClienteNombre.Location = new System.Drawing.Point(243, 10);
            this.lblClienteNombre.Name = "lblClienteNombre";
            this.lblClienteNombre.Size = new System.Drawing.Size(0, 17);
            this.lblClienteNombre.TabIndex = 2;
            // 
            // btnAltaCliente
            // 
            this.btnAltaCliente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAltaCliente.BackColor = System.Drawing.Color.Silver;
            this.btnAltaCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAltaCliente.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnAltaCliente.Location = new System.Drawing.Point(763, 5);
            this.btnAltaCliente.Name = "btnAltaCliente";
            this.btnAltaCliente.Size = new System.Drawing.Size(129, 23);
            this.btnAltaCliente.TabIndex = 3;
            this.btnAltaCliente.Text = "+ Nuevo Cliente";
            this.btnAltaCliente.UseVisualStyleBackColor = false;
            this.btnAltaCliente.Click += new System.EventHandler(this.btnAltaCliente_Click);
            // 
            // lbCliente
            // 
            this.lbCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbCliente.Location = new System.Drawing.Point(62, 29);
            this.lbCliente.Name = "lbCliente";
            this.lbCliente.Size = new System.Drawing.Size(317, 93);
            this.lbCliente.TabIndex = 4;
            this.lbCliente.Visible = false;
            this.lbCliente.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbCliente_KeyDown);
            this.lbCliente.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbCliente_MouseDoubleClick);
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblCodigo.Location = new System.Drawing.Point(9, 41);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(40, 17);
            this.lblCodigo.TabIndex = 5;
            this.lblCodigo.Text = "Filtro:";
            // 
            // cboFiltro
            // 
            this.cboFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltro.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboFiltro.Location = new System.Drawing.Point(53, 37);
            this.cboFiltro.Name = "cboFiltro";
            this.cboFiltro.Size = new System.Drawing.Size(125, 23);
            this.cboFiltro.TabIndex = 6;
            // 
            // txtFiltro
            // 
            this.txtFiltro.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtFiltro.Location = new System.Drawing.Point(186, 37);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.Size = new System.Drawing.Size(159, 24);
            this.txtFiltro.TabIndex = 7;
            this.txtFiltro.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFiltro_KeyDown);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.btnBuscar.Location = new System.Drawing.Point(353, 36);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(77, 23);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.Text = "Buscar [F2]";
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDesc.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDesc.Location = new System.Drawing.Point(439, 37);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(361, 24);
            this.txtDesc.TabIndex = 9;
            this.txtDesc.TextChanged += new System.EventHandler(this.txtDesc_TextChanged);
            this.txtDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDesc_KeyDown);
            // 
            // lbDesc
            // 
            this.lbDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbDesc.Location = new System.Drawing.Point(439, 60);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Size = new System.Drawing.Size(360, 132);
            this.lbDesc.TabIndex = 10;
            this.lbDesc.Visible = false;
            this.lbDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbDesc_KeyDown);
            this.lbDesc.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbDesc_MouseDoubleClick);
            // 
            // dgvProductos
            // 
            this.dgvProductos.AllowUserToAddRows = false;
            this.dgvProductos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.dgvProductos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProductos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProductos.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codBarras,
            this.descripcion,
            this.Cantidad,
            this.precioSinIva,
            this.precioConIva,
            this.Subtotal,
            this.DescRec,
            this.subtotalSIVA,
            this.id,
            this.pedido,
            this.costo,
            this.fraccionado,
            this.dolarizado});
            this.dgvProductos.EnableHeadersVisualStyles = false;
            this.dgvProductos.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvProductos.Location = new System.Drawing.Point(0, 63);
            this.dgvProductos.MultiSelect = false;
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.RowHeadersVisible = false;
            this.dgvProductos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductos.Size = new System.Drawing.Size(900, 468);
            this.dgvProductos.TabIndex = 11;
            this.dgvProductos.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dgvProductos_CellParsing);
            this.dgvProductos.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductos_CellValueChanged);
            this.dgvProductos.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvProductos_RowsRemoved);
            // 
            // codBarras
            // 
            this.codBarras.HeaderText = "Cód. Barras";
            this.codBarras.Name = "codBarras";
            this.codBarras.ReadOnly = true;
            this.codBarras.Width = 130;
            // 
            // descripcion
            // 
            this.descripcion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descripcion.HeaderText = "Descripción";
            this.descripcion.MinimumWidth = 200;
            this.descripcion.Name = "descripcion";
            this.descripcion.ReadOnly = true;
            // 
            // Cantidad
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Cantidad.DefaultCellStyle = dataGridViewCellStyle3;
            this.Cantidad.HeaderText = "Cant.";
            this.Cantidad.Name = "Cantidad";
            this.Cantidad.Width = 70;
            // 
            // precioSinIva
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            this.precioSinIva.DefaultCellStyle = dataGridViewCellStyle4;
            this.precioSinIva.HeaderText = "Precio Unit.";
            this.precioSinIva.Name = "precioSinIva";
            this.precioSinIva.ReadOnly = true;
            this.precioSinIva.Width = 110;
            // 
            // precioConIva
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.precioConIva.DefaultCellStyle = dataGridViewCellStyle5;
            this.precioConIva.HeaderText = "Precio Final";
            this.precioConIva.Name = "precioConIva";
            this.precioConIva.ReadOnly = true;
            this.precioConIva.Width = 110;
            // 
            // Subtotal
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            this.Subtotal.DefaultCellStyle = dataGridViewCellStyle6;
            this.Subtotal.HeaderText = "Subtotal";
            this.Subtotal.Name = "Subtotal";
            this.Subtotal.ReadOnly = true;
            this.Subtotal.Width = 120;
            // 
            // DescRec
            // 
            this.DescRec.Name = "DescRec";
            this.DescRec.Visible = false;
            // 
            // subtotalSIVA
            // 
            this.subtotalSIVA.Name = "subtotalSIVA";
            this.subtotalSIVA.Visible = false;
            // 
            // id
            // 
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // pedido
            // 
            this.pedido.Name = "pedido";
            this.pedido.Visible = false;
            // 
            // costo
            // 
            this.costo.Name = "costo";
            this.costo.Visible = false;
            // 
            // fraccionado
            // 
            this.fraccionado.Name = "fraccionado";
            this.fraccionado.Visible = false;
            // 
            // dolarizado
            // 
            this.dolarizado.Name = "dolarizado";
            this.dolarizado.Visible = false;
            // 
            // lblDtoLbl
            // 
            this.lblDtoLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDtoLbl.AutoSize = true;
            this.lblDtoLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDtoLbl.Location = new System.Drawing.Point(77, 554);
            this.lblDtoLbl.Name = "lblDtoLbl";
            this.lblDtoLbl.Size = new System.Drawing.Size(42, 15);
            this.lblDtoLbl.TabIndex = 12;
            this.lblDtoLbl.Text = "Dto. %";
            // 
            // nudDescuento
            // 
            this.nudDescuento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudDescuento.DecimalPlaces = 2;
            this.nudDescuento.Location = new System.Drawing.Point(9, 551);
            this.nudDescuento.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudDescuento.Name = "nudDescuento";
            this.nudDescuento.Size = new System.Drawing.Size(64, 20);
            this.nudDescuento.TabIndex = 13;
            this.nudDescuento.ValueChanged += new System.EventHandler(this.nudDescuento_ValueChanged);
            // 
            // lblRecLbl
            // 
            this.lblRecLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecLbl.AutoSize = true;
            this.lblRecLbl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblRecLbl.Location = new System.Drawing.Point(195, 554);
            this.lblRecLbl.Name = "lblRecLbl";
            this.lblRecLbl.Size = new System.Drawing.Size(42, 15);
            this.lblRecLbl.TabIndex = 14;
            this.lblRecLbl.Text = "Rec. %";
            // 
            // nudRecargo
            // 
            this.nudRecargo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudRecargo.DecimalPlaces = 2;
            this.nudRecargo.Location = new System.Drawing.Point(127, 551);
            this.nudRecargo.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudRecargo.Name = "nudRecargo";
            this.nudRecargo.Size = new System.Drawing.Size(64, 20);
            this.nudRecargo.TabIndex = 15;
            this.nudRecargo.ValueChanged += new System.EventHandler(this.nudRecargo_ValueChanged);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEliminar.Location = new System.Drawing.Point(244, 549);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(111, 26);
            this.btnEliminar.TabIndex = 16;
            this.btnEliminar.Text = "Eliminar [Del]";
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // lblTotalLabel
            // 
            this.lblTotalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalLabel.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblTotalLabel.Location = new System.Drawing.Point(480, 543);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new System.Drawing.Size(69, 33);
            this.lblTotalLabel.TabIndex = 17;
            this.lblTotalLabel.Text = "TOTAL";
            this.lblTotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotGeneral
            // 
            this.txtTotGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(58)))), ((int)(((byte)(92)))));
            this.txtTotGeneral.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTotGeneral.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.txtTotGeneral.ForeColor = System.Drawing.Color.White;
            this.txtTotGeneral.Location = new System.Drawing.Point(555, 536);
            this.txtTotGeneral.Name = "txtTotGeneral";
            this.txtTotGeneral.ReadOnly = true;
            this.txtTotGeneral.Size = new System.Drawing.Size(197, 40);
            this.txtTotGeneral.TabIndex = 18;
            this.txtTotGeneral.Text = "0";
            this.txtTotGeneral.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnGrabar
            // 
            this.btnGrabar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGrabar.BackColor = System.Drawing.Color.SeaGreen;
            this.btnGrabar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrabar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnGrabar.ForeColor = System.Drawing.Color.White;
            this.btnGrabar.Location = new System.Drawing.Point(763, 536);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(129, 49);
            this.btnGrabar.TabIndex = 19;
            this.btnGrabar.Text = "✓  ACEPTAR [F5]";
            this.btnGrabar.UseVisualStyleBackColor = false;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            // 
            // timerBusq
            // 
            this.timerBusq.Interval = 400;
            this.timerBusq.Tick += new System.EventHandler(this.timerBusq_Tick);
            // 
            // backgroundWorkerClientes
            // 
            this.backgroundWorkerClientes.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerClientes_DoWork);
            this.backgroundWorkerClientes.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerClientes_RunWorkerCompleted);
            // 
            // frmVentasMinorista
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 589);
            this.Controls.Add(this.lblBuscarCliente);
            this.Controls.Add(this.txtCliente);
            this.Controls.Add(this.lblClienteNombre);
            this.Controls.Add(this.btnAltaCliente);
            this.Controls.Add(this.lbCliente);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.cboFiltro);
            this.Controls.Add(this.txtFiltro);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.lbDesc);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.lblDtoLbl);
            this.Controls.Add(this.nudDescuento);
            this.Controls.Add(this.lblRecLbl);
            this.Controls.Add(this.nudRecargo);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.lblTotalLabel);
            this.Controls.Add(this.txtTotGeneral);
            this.Controls.Add(this.btnGrabar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(774, 525);
            this.Name = "frmVentasMinorista";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Venta — Kiosco";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmVentasMinorista_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmVentasMinorista_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDescuento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecargo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label           lblBuscarCliente;
        private System.Windows.Forms.TextBox         txtCliente;
        private System.Windows.Forms.Label           lblClienteNombre;
        private System.Windows.Forms.Button          btnAltaCliente;
        private System.Windows.Forms.ListBox         lbCliente;
        private System.Windows.Forms.Label           lblCodigo;
        private System.Windows.Forms.ComboBox        cboFiltro;
        private System.Windows.Forms.TextBox         txtFiltro;
        private System.Windows.Forms.Button          btnBuscar;
        private System.Windows.Forms.TextBox         txtDesc;
        private System.Windows.Forms.ListBox         lbDesc;
        private System.Windows.Forms.DataGridView    dgvProductos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodBarras;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecioConIva;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecioSinIva;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescRec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubtotalSIVA;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCosto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFraccionado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDolarizado;
        private System.Windows.Forms.Label           lblDtoLbl;
        private System.Windows.Forms.NumericUpDown   nudDescuento;
        private System.Windows.Forms.Label           lblRecLbl;
        private System.Windows.Forms.NumericUpDown   nudRecargo;
        private System.Windows.Forms.Button          btnEliminar;
        private System.Windows.Forms.Label           lblTotalLabel;
        private System.Windows.Forms.TextBox         txtTotGeneral;
        private System.Windows.Forms.Button          btnGrabar;
        private System.Windows.Forms.Timer           timerBusq;
        private System.ComponentModel.BackgroundWorker backgroundWorkerClientes;
        private System.Windows.Forms.DataGridViewTextBoxColumn codBarras;
        private System.Windows.Forms.DataGridViewTextBoxColumn descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn precioSinIva;
        private System.Windows.Forms.DataGridViewTextBoxColumn precioConIva;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescRec;
        private System.Windows.Forms.DataGridViewTextBoxColumn subtotalSIVA;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn pedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn costo;
        private System.Windows.Forms.DataGridViewTextBoxColumn fraccionado;
        private System.Windows.Forms.DataGridViewTextBoxColumn dolarizado;
    }
}
