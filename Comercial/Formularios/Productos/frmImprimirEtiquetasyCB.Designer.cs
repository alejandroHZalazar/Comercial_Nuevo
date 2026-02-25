
namespace Comercial.Formularios.Productos
{
    partial class frmImprimirEtiquetasyCB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImprimirEtiquetasyCB));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.txtFiltro = new System.Windows.Forms.TextBox();
            this.cboFiltro = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboRubros = new System.Windows.Forms.ComboBox();
            this.cboProveedores = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.cbTodos = new System.Windows.Forms.CheckBox();
            this.cbNinguno = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnImprimirEtiqueta = new System.Windows.Forms.Button();
            this.btnImprimirCodBarras = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Silver;
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnBuscar);
            this.groupBox1.Controls.Add(this.txtFiltro);
            this.groupBox1.Controls.Add(this.cboFiltro);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboRubros);
            this.groupBox1.Controls.Add(this.cboProveedores);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(947, 108);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtro";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Image = global::Comercial.Properties.Resources.musica_searcher;
            this.btnBuscar.Location = new System.Drawing.Point(452, 56);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(50, 34);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // txtFiltro
            // 
            this.txtFiltro.Location = new System.Drawing.Point(249, 63);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.Size = new System.Drawing.Size(187, 21);
            this.txtFiltro.TabIndex = 7;
            // 
            // cboFiltro
            // 
            this.cboFiltro.FormattingEnabled = true;
            this.cboFiltro.Items.AddRange(new object[] {
            "Cod. Proveedor",
            "Cod. Barras",
            "Descripcion"});
            this.cboFiltro.Location = new System.Drawing.Point(62, 62);
            this.cboFiltro.Name = "cboFiltro";
            this.cboFiltro.Size = new System.Drawing.Size(171, 23);
            this.cboFiltro.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Filtro:";
            // 
            // cboRubros
            // 
            this.cboRubros.FormattingEnabled = true;
            this.cboRubros.Location = new System.Drawing.Point(367, 24);
            this.cboRubros.Name = "cboRubros";
            this.cboRubros.Size = new System.Drawing.Size(223, 23);
            this.cboRubros.TabIndex = 2;
            // 
            // cboProveedores
            // 
            this.cboProveedores.FormattingEnabled = true;
            this.cboProveedores.Location = new System.Drawing.Point(79, 24);
            this.cboProveedores.Name = "cboProveedores";
            this.cboProveedores.Size = new System.Drawing.Size(223, 23);
            this.cboProveedores.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Proveedor";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(320, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "Rubro";
            // 
            // dgvProductos
            // 
            this.dgvProductos.AllowUserToAddRows = false;
            this.dgvProductos.AllowUserToDeleteRows = false;
            this.dgvProductos.AllowUserToOrderColumns = true;
            this.dgvProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductos.Location = new System.Drawing.Point(5, 126);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.Size = new System.Drawing.Size(947, 372);
            this.dgvProductos.TabIndex = 3;
            // 
            // cbTodos
            // 
            this.cbTodos.AutoSize = true;
            this.cbTodos.Location = new System.Drawing.Point(12, 504);
            this.cbTodos.Name = "cbTodos";
            this.cbTodos.Size = new System.Drawing.Size(128, 19);
            this.cbTodos.TabIndex = 4;
            this.cbTodos.Text = "Seleccionar Todos";
            this.cbTodos.UseVisualStyleBackColor = true;
            this.cbTodos.CheckedChanged += new System.EventHandler(this.cbTodos_CheckedChanged);
            // 
            // cbNinguno
            // 
            this.cbNinguno.AutoSize = true;
            this.cbNinguno.Location = new System.Drawing.Point(160, 504);
            this.cbNinguno.Name = "cbNinguno";
            this.cbNinguno.Size = new System.Drawing.Size(160, 19);
            this.cbNinguno.TabIndex = 5;
            this.cbNinguno.Text = "No Seleccionar Ninguno";
            this.cbNinguno.UseVisualStyleBackColor = true;
            this.cbNinguno.CheckedChanged += new System.EventHandler(this.cbNinguno_CheckedChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(341, 504);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(236, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // btnImprimirEtiqueta
            // 
            this.btnImprimirEtiqueta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirEtiqueta.Image = ((System.Drawing.Image)(resources.GetObject("btnImprimirEtiqueta.Image")));
            this.btnImprimirEtiqueta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimirEtiqueta.Location = new System.Drawing.Point(601, 504);
            this.btnImprimirEtiqueta.Name = "btnImprimirEtiqueta";
            this.btnImprimirEtiqueta.Size = new System.Drawing.Size(153, 28);
            this.btnImprimirEtiqueta.TabIndex = 7;
            this.btnImprimirEtiqueta.Text = "Imprimir Etiqueta";
            this.btnImprimirEtiqueta.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImprimirEtiqueta.UseVisualStyleBackColor = true;
            this.btnImprimirEtiqueta.Click += new System.EventHandler(this.btnImprimirEtiqueta_Click);
            // 
            // btnImprimirCodBarras
            // 
            this.btnImprimirCodBarras.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirCodBarras.Image = ((System.Drawing.Image)(resources.GetObject("btnImprimirCodBarras.Image")));
            this.btnImprimirCodBarras.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimirCodBarras.Location = new System.Drawing.Point(777, 504);
            this.btnImprimirCodBarras.Name = "btnImprimirCodBarras";
            this.btnImprimirCodBarras.Size = new System.Drawing.Size(175, 28);
            this.btnImprimirCodBarras.TabIndex = 8;
            this.btnImprimirCodBarras.Text = "Imprimir Cod. Barras";
            this.btnImprimirCodBarras.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImprimirCodBarras.UseVisualStyleBackColor = true;
            this.btnImprimirCodBarras.Click += new System.EventHandler(this.btnImprimirCodBarras_Click);
            // 
            // frmImprimirEtiquetasyCB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(960, 536);
            this.Controls.Add(this.btnImprimirCodBarras);
            this.Controls.Add(this.btnImprimirEtiqueta);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cbNinguno);
            this.Controls.Add(this.cbTodos);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImprimirEtiquetasyCB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impresión de Etiquetas y Códigos de Barra";
            this.Load += new System.EventHandler(this.frmImprimirEtiquetasyCB_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox txtFiltro;
        private System.Windows.Forms.ComboBox cboFiltro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboRubros;
        private System.Windows.Forms.ComboBox cboProveedores;
        private System.Windows.Forms.DataGridView dgvProductos;
        private System.Windows.Forms.CheckBox cbTodos;
        private System.Windows.Forms.CheckBox cbNinguno;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnImprimirEtiqueta;
        private System.Windows.Forms.Button btnImprimirCodBarras;
    }
}