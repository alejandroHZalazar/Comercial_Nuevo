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

            // ── Controls ────────────────────────────────────────────────────────
            this.lblBuscarCliente  = new System.Windows.Forms.Label();
            this.txtCliente        = new System.Windows.Forms.TextBox();
            this.lblClienteNombre  = new System.Windows.Forms.Label();
            this.btnAltaCliente    = new System.Windows.Forms.Button();
            this.lbCliente         = new System.Windows.Forms.ListBox();
            this.lblCodigo         = new System.Windows.Forms.Label();
            this.cboFiltro         = new System.Windows.Forms.ComboBox();
            this.txtFiltro         = new System.Windows.Forms.TextBox();
            this.btnBuscar         = new System.Windows.Forms.Button();
            this.txtDesc           = new System.Windows.Forms.TextBox();
            this.lbDesc            = new System.Windows.Forms.ListBox();
            this.dgvProductos      = new System.Windows.Forms.DataGridView();
            this.colCodBarras      = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescripcion    = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCantidad       = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecioConIva   = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubtotal       = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecioSinIva   = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescRec        = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubtotalSIVA   = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId             = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPedido         = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCosto          = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFraccionado    = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDolarizado     = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDtoLbl         = new System.Windows.Forms.Label();
            this.nudDescuento      = new System.Windows.Forms.NumericUpDown();
            this.lblRecLbl         = new System.Windows.Forms.Label();
            this.nudRecargo        = new System.Windows.Forms.NumericUpDown();
            this.btnEliminar       = new System.Windows.Forms.Button();
            this.lblTotalLabel     = new System.Windows.Forms.Label();
            this.txtTotGeneral     = new System.Windows.Forms.TextBox();
            this.btnGrabar         = new System.Windows.Forms.Button();
            this.timerBusq         = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorkerClientes = new System.ComponentModel.BackgroundWorker();

            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDescuento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecargo)).BeginInit();
            this.SuspendLayout();

            // ── lblBuscarCliente ────────────────────────────────────────────────
            this.lblBuscarCliente.AutoSize  = true;
            this.lblBuscarCliente.Font      = new System.Drawing.Font("Segoe UI", 9.5f);
            this.lblBuscarCliente.Location  = new System.Drawing.Point(10, 12);
            this.lblBuscarCliente.Text      = "Cliente:";

            // ── txtCliente ──────────────────────────────────────────────────────
            this.txtCliente.Location  = new System.Drawing.Point(72, 9);
            this.txtCliente.Size      = new System.Drawing.Size(200, 22);
            this.txtCliente.Font      = new System.Drawing.Font("Segoe UI", 9.5f);
            this.txtCliente.TextChanged += new System.EventHandler(this.txtCliente_TextChanged);
            this.txtCliente.KeyDown    += new System.Windows.Forms.KeyEventHandler(this.txtCliente_KeyDown);

            // ── lblClienteNombre ────────────────────────────────────────────────
            this.lblClienteNombre.AutoSize  = true;
            this.lblClienteNombre.Font      = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold);
            this.lblClienteNombre.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblClienteNombre.Location  = new System.Drawing.Point(284, 12);

            // ── lbCliente (floating, Visible=false) ─────────────────────────────
            this.lbCliente.Location   = new System.Drawing.Point(72, 34);
            this.lbCliente.Size       = new System.Drawing.Size(370, 120);
            this.lbCliente.Visible    = false;
            this.lbCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbCliente.KeyDown    += new System.Windows.Forms.KeyEventHandler(this.lbCliente_KeyDown);
            this.lbCliente.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbCliente_MouseDoubleClick);

            // ── btnAltaCliente ──────────────────────────────────────────────────
            this.btnAltaCliente.Location  = new System.Drawing.Point(890, 6);
            this.btnAltaCliente.Size      = new System.Drawing.Size(150, 26);
            this.btnAltaCliente.Text      = "+ Nuevo Cliente";
            this.btnAltaCliente.Font      = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold);
            this.btnAltaCliente.BackColor = System.Drawing.Color.Silver;
            this.btnAltaCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAltaCliente.Anchor    = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnAltaCliente.Click     += new System.EventHandler(this.btnAltaCliente_Click);

            // ── lblCodigo ───────────────────────────────────────────────────────
            // Texto "Filtro:" (corto) para no solapar con cboFiltro
            this.lblCodigo.AutoSize  = true;
            this.lblCodigo.Font      = new System.Drawing.Font("Segoe UI", 9.5f);
            this.lblCodigo.Location  = new System.Drawing.Point(10, 47);
            this.lblCodigo.Text      = "Filtro:";

            // ── cboFiltro ───────────────────────────────────────────────────────
            this.cboFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltro.Font      = new System.Drawing.Font("Segoe UI", 9f);
            this.cboFiltro.Location  = new System.Drawing.Point(62, 43);
            this.cboFiltro.Size      = new System.Drawing.Size(145, 24);

            // ── txtFiltro ───────────────────────────────────────────────────────
            this.txtFiltro.Location  = new System.Drawing.Point(217, 43);
            this.txtFiltro.Size      = new System.Drawing.Size(185, 22);
            this.txtFiltro.Font      = new System.Drawing.Font("Segoe UI", 9.5f);
            this.txtFiltro.KeyDown   += new System.Windows.Forms.KeyEventHandler(this.txtFiltro_KeyDown);

            // ── btnBuscar ───────────────────────────────────────────────────────
            this.btnBuscar.Location  = new System.Drawing.Point(412, 41);
            this.btnBuscar.Size      = new System.Drawing.Size(90, 26);
            this.btnBuscar.Text      = "Buscar [F2]";
            this.btnBuscar.Font      = new System.Drawing.Font("Segoe UI", 8.5f);
            this.btnBuscar.Click     += new System.EventHandler(this.btnBuscar_Click);

            // ── txtDesc ─────────────────────────────────────────────────────────
            this.txtDesc.Location    = new System.Drawing.Point(512, 43);
            this.txtDesc.Size        = new System.Drawing.Size(420, 22);
            this.txtDesc.Font        = new System.Drawing.Font("Segoe UI", 9.5f);
            this.txtDesc.Anchor      = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtDesc.TextChanged += new System.EventHandler(this.txtDesc_TextChanged);
            this.txtDesc.KeyDown     += new System.Windows.Forms.KeyEventHandler(this.txtDesc_KeyDown);

            // ── lbDesc (floating, Visible=false) ────────────────────────────────
            this.lbDesc.Location     = new System.Drawing.Point(512, 69);
            this.lbDesc.Size         = new System.Drawing.Size(420, 156);
            this.lbDesc.Visible      = false;
            this.lbDesc.Anchor       = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lbDesc.BorderStyle  = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbDesc.KeyDown      += new System.Windows.Forms.KeyEventHandler(this.lbDesc_KeyDown);
            this.lbDesc.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbDesc_MouseDoubleClick);

            // ── dgvProductos ────────────────────────────────────────────────────
            this.dgvProductos.Location             = new System.Drawing.Point(0, 73);
            this.dgvProductos.Size                 = new System.Drawing.Size(1050, 540);
            this.dgvProductos.Anchor               = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvProductos.ScrollBars           = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvProductos.AllowUserToAddRows   = false;
            this.dgvProductos.AllowUserToDeleteRows = false;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.RowHeadersVisible    = false;
            this.dgvProductos.SelectionMode        = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductos.MultiSelect          = false;
            this.dgvProductos.AutoSizeColumnsMode  = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvProductos.Font                 = new System.Drawing.Font("Segoe UI", 9.5f);
            this.dgvProductos.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold);
            this.dgvProductos.CellValueChanged     += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProductos_CellValueChanged);
            this.dgvProductos.RowsRemoved          += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvProductos_RowsRemoved);
            this.dgvProductos.CellParsing          += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dgvProductos_CellParsing);

            // ── Columnas visibles ───────────────────────────────────────────────
            this.colCodBarras.Name       = "codBarras";
            this.colCodBarras.HeaderText = "Cód. Barras";
            this.colCodBarras.ReadOnly   = true;
            this.colCodBarras.Width      = 130;

            this.colDescripcion.Name        = "descripcion";
            this.colDescripcion.HeaderText  = "Descripción";
            this.colDescripcion.ReadOnly    = true;
            this.colDescripcion.MinimumWidth = 200;
            // Fill: absorbe el ancho sobrante → nunca aparece scroll horizontal
            this.colDescripcion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            this.colCantidad.Name       = "Cantidad";
            this.colCantidad.HeaderText = "Cant.";
            this.colCantidad.ReadOnly   = false;
            this.colCantidad.Width      = 70;
            this.colCantidad.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // Precio Unit. = precio original de lista (nunca cambia con desc/rec)
            this.colPrecioSinIva.Name       = "precioSinIva";
            this.colPrecioSinIva.HeaderText = "Precio Unit.";
            this.colPrecioSinIva.ReadOnly   = true;
            this.colPrecioSinIva.Width      = 110;
            this.colPrecioSinIva.Visible    = true;
            this.colPrecioSinIva.DefaultCellStyle.Format    = "N2";
            this.colPrecioSinIva.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // Precio Final = precio unitario luego de aplicar desc/rec global
            this.colPrecioConIva.Name       = "precioConIva";
            this.colPrecioConIva.HeaderText = "Precio Final";
            this.colPrecioConIva.ReadOnly   = true;
            this.colPrecioConIva.Width      = 110;
            this.colPrecioConIva.DefaultCellStyle.Format    = "N2";
            this.colPrecioConIva.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            this.colSubtotal.Name       = "Subtotal";
            this.colSubtotal.HeaderText = "Subtotal";
            this.colSubtotal.ReadOnly   = true;
            this.colSubtotal.Width      = 120;
            this.colSubtotal.DefaultCellStyle.Format    = "N2";
            this.colSubtotal.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // ── Columnas ocultas ────────────────────────────────────────────────
            // colPrecioSinIva ya declarada arriba (ahora visible)

            this.colDescRec.Name    = "DescRec";
            this.colDescRec.Visible = false;

            this.colSubtotalSIVA.Name    = "subtotalSIVA";
            this.colSubtotalSIVA.Visible = false;

            this.colId.Name    = "id";
            this.colId.Visible = false;

            this.colPedido.Name    = "pedido";
            this.colPedido.Visible = false;

            this.colCosto.Name    = "costo";
            this.colCosto.Visible = false;

            this.colFraccionado.Name    = "fraccionado";
            this.colFraccionado.Visible = false;

            this.colDolarizado.Name    = "dolarizado";
            this.colDolarizado.Visible = false;

            this.dgvProductos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colCodBarras,    // 0
                this.colDescripcion,  // 1
                this.colCantidad,     // 2
                this.colPrecioSinIva, // 3  Precio Unit. (original)
                this.colPrecioConIva, // 4  Precio Final (ajustado por desc/rec)
                this.colSubtotal,     // 5
                this.colDescRec,      // 6  oculta
                this.colSubtotalSIVA, // 7  oculta
                this.colId,           // 8  oculta
                this.colPedido,       // 9  oculta
                this.colCosto,        // 10 oculta
                this.colFraccionado,  // 11 oculta
                this.colDolarizado    // 12 oculta
            });

            // ── nudDescuento ────────────────────────────────────────────────────
            this.nudDescuento.Location      = new System.Drawing.Point(10, 636);
            this.nudDescuento.Size          = new System.Drawing.Size(75, 24);
            this.nudDescuento.Minimum       = 0;
            this.nudDescuento.Maximum       = 99;
            this.nudDescuento.DecimalPlaces = 2;
            this.nudDescuento.Anchor        = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.nudDescuento.ValueChanged  += new System.EventHandler(this.nudDescuento_ValueChanged);

            // ── lblDtoLbl ───────────────────────────────────────────────────────
            this.lblDtoLbl.AutoSize  = true;
            this.lblDtoLbl.Font      = new System.Drawing.Font("Segoe UI", 9f);
            this.lblDtoLbl.Location  = new System.Drawing.Point(90, 639);
            this.lblDtoLbl.Text      = "Dto. %";
            this.lblDtoLbl.Anchor    = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

            // ── nudRecargo ──────────────────────────────────────────────────────
            this.nudRecargo.Location      = new System.Drawing.Point(148, 636);
            this.nudRecargo.Size          = new System.Drawing.Size(75, 24);
            this.nudRecargo.Minimum       = 0;
            this.nudRecargo.Maximum       = 99;
            this.nudRecargo.DecimalPlaces = 2;
            this.nudRecargo.Anchor        = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.nudRecargo.ValueChanged  += new System.EventHandler(this.nudRecargo_ValueChanged);

            // ── lblRecLbl ───────────────────────────────────────────────────────
            this.lblRecLbl.AutoSize  = true;
            this.lblRecLbl.Font      = new System.Drawing.Font("Segoe UI", 9f);
            this.lblRecLbl.Location  = new System.Drawing.Point(228, 639);
            this.lblRecLbl.Text      = "Rec. %";
            this.lblRecLbl.Anchor    = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;

            // ── btnEliminar ─────────────────────────────────────────────────────
            this.btnEliminar.Location  = new System.Drawing.Point(285, 633);
            this.btnEliminar.Size      = new System.Drawing.Size(130, 30);
            this.btnEliminar.Text      = "Eliminar [Del]";
            this.btnEliminar.Font      = new System.Drawing.Font("Segoe UI", 9f);
            this.btnEliminar.Anchor    = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.btnEliminar.Click     += new System.EventHandler(this.btnEliminar_Click);

            // ── lblTotalLabel ───────────────────────────────────────────────────
            this.lblTotalLabel.AutoSize  = false;
            this.lblTotalLabel.Size      = new System.Drawing.Size(80, 38);
            this.lblTotalLabel.Location  = new System.Drawing.Point(560, 626);
            this.lblTotalLabel.Text      = "TOTAL";
            this.lblTotalLabel.Font      = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold);
            this.lblTotalLabel.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblTotalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTotalLabel.Anchor    = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;

            // ── txtTotGeneral — más ancho para soportar cifras grandes ──────────
            this.txtTotGeneral.Location  = new System.Drawing.Point(648, 619);
            this.txtTotGeneral.Size      = new System.Drawing.Size(230, 52);
            this.txtTotGeneral.ReadOnly  = true;
            this.txtTotGeneral.Text      = "0";
            this.txtTotGeneral.Font      = new System.Drawing.Font("Segoe UI", 22f, System.Drawing.FontStyle.Bold);
            this.txtTotGeneral.BackColor = System.Drawing.Color.FromArgb(26, 58, 92);
            this.txtTotGeneral.ForeColor = System.Drawing.Color.White;
            this.txtTotGeneral.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotGeneral.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTotGeneral.Anchor    = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;

            // ── btnGrabar ───────────────────────────────────────────────────────
            this.btnGrabar.Location  = new System.Drawing.Point(890, 618);
            this.btnGrabar.Size      = new System.Drawing.Size(150, 56);
            this.btnGrabar.Text      = "✓  ACEPTAR [F5]";
            this.btnGrabar.Font      = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
            this.btnGrabar.BackColor = System.Drawing.Color.SeaGreen;
            this.btnGrabar.ForeColor = System.Drawing.Color.White;
            this.btnGrabar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrabar.Anchor    = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnGrabar.Click     += new System.EventHandler(this.btnGrabar_Click);

            // ── timerBusq ───────────────────────────────────────────────────────
            this.timerBusq.Interval = 400;
            this.timerBusq.Tick     += new System.EventHandler(this.timerBusq_Tick);

            // ── backgroundWorkerClientes ────────────────────────────────────────
            this.backgroundWorkerClientes.DoWork             += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerClientes_DoWork);
            this.backgroundWorkerClientes.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerClientes_RunWorkerCompleted);

            // ── Form ────────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1050, 680);
            this.MinimumSize         = new System.Drawing.Size(900, 600);
            this.KeyPreview          = true;
            this.StartPosition       = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text                = "Venta — Kiosco";
            this.FormBorderStyle     = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox         = true;
            this.WindowState         = System.Windows.Forms.FormWindowState.Maximized;

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblBuscarCliente, this.txtCliente, this.lblClienteNombre, this.btnAltaCliente,
                this.lbCliente,
                this.lblCodigo, this.cboFiltro, this.txtFiltro, this.btnBuscar,
                this.txtDesc, this.lbDesc,
                this.dgvProductos,
                this.lblDtoLbl, this.nudDescuento,
                this.lblRecLbl, this.nudRecargo,
                this.btnEliminar,
                this.lblTotalLabel, this.txtTotGeneral,
                this.btnGrabar
            });

            // lbDesc y lbCliente deben estar por encima del dgv → BringToFront
            this.lbDesc.BringToFront();
            this.lbCliente.BringToFront();

            this.Load     += new System.EventHandler(this.frmVentasMinorista_Load);
            this.KeyDown  += new System.Windows.Forms.KeyEventHandler(this.frmVentasMinorista_KeyDown);

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
    }
}
