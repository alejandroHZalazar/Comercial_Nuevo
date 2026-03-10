
namespace Comercial.Formularios.Configuracion
{
    partial class frmABMConceptosCaja
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmABMConceptosCaja));
            this.dgvConceptos = new System.Windows.Forms.DataGridView();
            this.gbDatos = new System.Windows.Forms.GroupBox();
            this.cbAfectaEfectivo = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTipoMovimiento = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnAgregarTiposGastos = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cbAsocMedioPago = new System.Windows.Forms.CheckBox();
            this.lblMedioPago = new System.Windows.Forms.Label();
            this.cbMedioPago = new System.Windows.Forms.ComboBox();
            this.lblTipoOperacion = new System.Windows.Forms.Label();
            this.cbTipoOperacion = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConceptos)).BeginInit();
            this.gbDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvConceptos
            // 
            this.dgvConceptos.AllowUserToAddRows = false;
            this.dgvConceptos.AllowUserToDeleteRows = false;
            this.dgvConceptos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConceptos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConceptos.Location = new System.Drawing.Point(12, 12);
            this.dgvConceptos.MultiSelect = false;
            this.dgvConceptos.Name = "dgvConceptos";
            this.dgvConceptos.ReadOnly = true;
            this.dgvConceptos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConceptos.Size = new System.Drawing.Size(401, 254);
            this.dgvConceptos.TabIndex = 0;
            // 
            // gbDatos
            // 
            this.gbDatos.BackColor = System.Drawing.Color.Silver;
            this.gbDatos.Controls.Add(this.cbTipoOperacion);
            this.gbDatos.Controls.Add(this.lblTipoOperacion);
            this.gbDatos.Controls.Add(this.cbMedioPago);
            this.gbDatos.Controls.Add(this.lblMedioPago);
            this.gbDatos.Controls.Add(this.cbAsocMedioPago);
            this.gbDatos.Controls.Add(this.label4);
            this.gbDatos.Controls.Add(this.cbAfectaEfectivo);
            this.gbDatos.Controls.Add(this.label3);
            this.gbDatos.Controls.Add(this.cboTipoMovimiento);
            this.gbDatos.Controls.Add(this.label2);
            this.gbDatos.Controls.Add(this.label1);
            this.gbDatos.Controls.Add(this.txtNombre);
            this.gbDatos.Controls.Add(this.btnCancelar);
            this.gbDatos.Controls.Add(this.btnGrabar);
            this.gbDatos.Location = new System.Drawing.Point(419, 12);
            this.gbDatos.Name = "gbDatos";
            this.gbDatos.Size = new System.Drawing.Size(314, 254);
            this.gbDatos.TabIndex = 4;
            this.gbDatos.TabStop = false;
            // 
            // cbAfectaEfectivo
            // 
            this.cbAfectaEfectivo.AutoSize = true;
            this.cbAfectaEfectivo.Location = new System.Drawing.Point(151, 77);
            this.cbAfectaEfectivo.Name = "cbAfectaEfectivo";
            this.cbAfectaEfectivo.Size = new System.Drawing.Size(37, 19);
            this.cbAfectaEfectivo.TabIndex = 2;
            this.cbAfectaEfectivo.Text = "Sí";
            this.cbAfectaEfectivo.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Afecta Efectivo:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // cboTipoMovimiento
            // 
            this.cboTipoMovimiento.FormattingEnabled = true;
            this.cboTipoMovimiento.Items.AddRange(new object[] {
            "I",
            "E"});
            this.cboTipoMovimiento.Location = new System.Drawing.Point(110, 45);
            this.cboTipoMovimiento.Name = "cboTipoMovimiento";
            this.cboTipoMovimiento.Size = new System.Drawing.Size(182, 23);
            this.cboTipoMovimiento.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tipo Movimiento:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(110, 16);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(182, 21);
            this.txtNombre.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Silver;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = global::Comercial.Properties.Resources.back_arrow;
            this.btnCancelar.Location = new System.Drawing.Point(181, 213);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(111, 35);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar [F6]";
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGrabar
            // 
            this.btnGrabar.BackColor = System.Drawing.Color.Silver;
            this.btnGrabar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrabar.Image = global::Comercial.Properties.Resources.save__1_;
            this.btnGrabar.Location = new System.Drawing.Point(9, 213);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(111, 35);
            this.btnGrabar.TabIndex = 3;
            this.btnGrabar.Text = "Grabar [F5]";
            this.btnGrabar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGrabar.UseVisualStyleBackColor = false;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.Silver;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.Image = global::Comercial.Properties.Resources.rubbish_bin__1_;
            this.btnEliminar.Location = new System.Drawing.Point(242, 272);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(109, 35);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar [F4]";
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.Silver;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Image = global::Comercial.Properties.Resources.pencil_edit_button__1_;
            this.btnEditar.Location = new System.Drawing.Point(127, 272);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(109, 35);
            this.btnEditar.TabIndex = 2;
            this.btnEditar.Text = "  Editar [F3]";
            this.btnEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.Silver;
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.Image = global::Comercial.Properties.Resources.plus;
            this.btnAgregar.Location = new System.Drawing.Point(12, 272);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(109, 35);
            this.btnAgregar.TabIndex = 1;
            this.btnAgregar.Text = "Agregar [F2]";
            this.btnAgregar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnAgregarTiposGastos
            // 
            this.btnAgregarTiposGastos.BackColor = System.Drawing.Color.Silver;
            this.btnAgregarTiposGastos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarTiposGastos.Image = ((System.Drawing.Image)(resources.GetObject("btnAgregarTiposGastos.Image")));
            this.btnAgregarTiposGastos.Location = new System.Drawing.Point(357, 272);
            this.btnAgregarTiposGastos.Name = "btnAgregarTiposGastos";
            this.btnAgregarTiposGastos.Size = new System.Drawing.Size(182, 35);
            this.btnAgregarTiposGastos.TabIndex = 6;
            this.btnAgregarTiposGastos.Text = "Agregar Tipos Gastos [F7]";
            this.btnAgregarTiposGastos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgregarTiposGastos.UseVisualStyleBackColor = false;
            this.btnAgregarTiposGastos.Click += new System.EventHandler(this.btnAgregarTiposGastos_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Asociada Medio Pago:";
            // 
            // cbAsocMedioPago
            // 
            this.cbAsocMedioPago.AutoSize = true;
            this.cbAsocMedioPago.Location = new System.Drawing.Point(151, 107);
            this.cbAsocMedioPago.Name = "cbAsocMedioPago";
            this.cbAsocMedioPago.Size = new System.Drawing.Size(37, 19);
            this.cbAsocMedioPago.TabIndex = 9;
            this.cbAsocMedioPago.Text = "Sí";
            this.cbAsocMedioPago.UseVisualStyleBackColor = true;
            this.cbAsocMedioPago.CheckedChanged += new System.EventHandler(this.cbAsocMedioPago_CheckedChanged);
            // 
            // lblMedioPago
            // 
            this.lblMedioPago.AutoSize = true;
            this.lblMedioPago.Location = new System.Drawing.Point(6, 139);
            this.lblMedioPago.Name = "lblMedioPago";
            this.lblMedioPago.Size = new System.Drawing.Size(77, 15);
            this.lblMedioPago.TabIndex = 10;
            this.lblMedioPago.Text = "Medio Pago:";
            // 
            // cbMedioPago
            // 
            this.cbMedioPago.FormattingEnabled = true;
            this.cbMedioPago.Items.AddRange(new object[] {
            "I",
            "E"});
            this.cbMedioPago.Location = new System.Drawing.Point(110, 135);
            this.cbMedioPago.Name = "cbMedioPago";
            this.cbMedioPago.Size = new System.Drawing.Size(182, 23);
            this.cbMedioPago.TabIndex = 11;
            // 
            // lblTipoOperacion
            // 
            this.lblTipoOperacion.AutoSize = true;
            this.lblTipoOperacion.Location = new System.Drawing.Point(6, 169);
            this.lblTipoOperacion.Name = "lblTipoOperacion";
            this.lblTipoOperacion.Size = new System.Drawing.Size(94, 15);
            this.lblTipoOperacion.TabIndex = 12;
            this.lblTipoOperacion.Text = "Tipo Operación:";
            // 
            // cbTipoOperacion
            // 
            this.cbTipoOperacion.FormattingEnabled = true;
            this.cbTipoOperacion.Items.AddRange(new object[] {
            "Ventas",
            "Cobros",
            "Pagos"});
            this.cbTipoOperacion.Location = new System.Drawing.Point(110, 165);
            this.cbTipoOperacion.Name = "cbTipoOperacion";
            this.cbTipoOperacion.Size = new System.Drawing.Size(182, 23);
            this.cbTipoOperacion.TabIndex = 13;
            // 
            // frmABMConceptosCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(742, 317);
            this.Controls.Add(this.btnAgregarTiposGastos);
            this.Controls.Add(this.dgvConceptos);
            this.Controls.Add(this.gbDatos);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnAgregar);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmABMConceptosCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABM Conceptos Caja";
            this.Load += new System.EventHandler(this.frmABMConceptosCaja_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmABMConceptosCaja_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConceptos)).EndInit();
            this.gbDatos.ResumeLayout(false);
            this.gbDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvConceptos;
        private System.Windows.Forms.GroupBox gbDatos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTipoMovimiento;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbAfectaEfectivo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnAgregarTiposGastos;
        private System.Windows.Forms.ComboBox cbTipoOperacion;
        private System.Windows.Forms.Label lblTipoOperacion;
        private System.Windows.Forms.ComboBox cbMedioPago;
        private System.Windows.Forms.Label lblMedioPago;
        private System.Windows.Forms.CheckBox cbAsocMedioPago;
        private System.Windows.Forms.Label label4;
    }
}