namespace Comercial.Formularios.Configuracion
{
    partial class frmABMrubros
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmABMrubros));
            this.tvRubros = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboPadre = new System.Windows.Forms.ComboBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.gbDatos = new System.Windows.Forms.GroupBox();
            this.lblRuta = new System.Windows.Forms.Label();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbDatos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // tvRubros
            //
            this.tvRubros.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvRubros.BackColor = System.Drawing.Color.White;
            this.tvRubros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvRubros.HideSelection = false;
            this.tvRubros.ItemHeight = 22;
            this.tvRubros.Location = new System.Drawing.Point(7, 8);
            this.tvRubros.Name = "tvRubros";
            this.tvRubros.ShowNodeToolTips = true;
            this.tvRubros.Size = new System.Drawing.Size(446, 404);
            this.tvRubros.TabIndex = 0;
            this.tvRubros.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvRubros_AfterSelect);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Descripcion:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(9, 42);
            this.txtDescripcion.MaxLength = 150;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(275, 23);
            this.txtDescripcion.TabIndex = 0;
            this.txtDescripcion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescripcion_KeyPress);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rubro padre:";
            //
            // cboPadre
            //
            this.cboPadre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPadre.DropDownWidth = 420;
            this.cboPadre.FormattingEnabled = true;
            this.cboPadre.Location = new System.Drawing.Point(9, 96);
            this.cboPadre.Name = "cboPadre";
            this.cboPadre.Size = new System.Drawing.Size(275, 23);
            this.cboPadre.TabIndex = 1;
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.Silver;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = global::Comercial.Properties.Resources.back_arrow;
            this.btnCancelar.Location = new System.Drawing.Point(173, 138);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(111, 35);
            this.btnCancelar.TabIndex = 3;
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
            this.btnGrabar.Location = new System.Drawing.Point(9, 138);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(111, 35);
            this.btnGrabar.TabIndex = 2;
            this.btnGrabar.Text = "Grabar [F5]";
            this.btnGrabar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGrabar.UseVisualStyleBackColor = false;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            //
            // gbDatos
            //
            this.gbDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDatos.BackColor = System.Drawing.Color.Silver;
            this.gbDatos.Controls.Add(this.label2);
            this.gbDatos.Controls.Add(this.cboPadre);
            this.gbDatos.Controls.Add(this.label1);
            this.gbDatos.Controls.Add(this.txtDescripcion);
            this.gbDatos.Controls.Add(this.btnCancelar);
            this.gbDatos.Controls.Add(this.btnGrabar);
            this.gbDatos.Location = new System.Drawing.Point(459, 8);
            this.gbDatos.Name = "gbDatos";
            this.gbDatos.Size = new System.Drawing.Size(293, 186);
            this.gbDatos.TabIndex = 4;
            this.gbDatos.TabStop = false;
            this.gbDatos.Text = "Datos del rubro";
            //
            // lblRuta
            //
            this.lblRuta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRuta.AutoEllipsis = true;
            this.lblRuta.Location = new System.Drawing.Point(459, 204);
            this.lblRuta.Name = "lblRuta";
            this.lblRuta.Size = new System.Drawing.Size(293, 120);
            this.lblRuta.TabIndex = 5;
            this.lblRuta.Text = "Ubicación:";
            //
            // btnEliminar
            //
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEliminar.BackColor = System.Drawing.Color.Silver;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.Image = global::Comercial.Properties.Resources.rubbish_bin__1_;
            this.btnEliminar.Location = new System.Drawing.Point(251, 420);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(111, 35);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar [F4]";
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // btnEditar
            //
            this.btnEditar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditar.BackColor = System.Drawing.Color.Silver;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Image = global::Comercial.Properties.Resources.pencil_edit_button__1_;
            this.btnEditar.Location = new System.Drawing.Point(129, 420);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(111, 35);
            this.btnEditar.TabIndex = 2;
            this.btnEditar.Text = "  Editar [F3]";
            this.btnEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            //
            // btnAgregar
            //
            this.btnAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAgregar.BackColor = System.Drawing.Color.Silver;
            this.btnAgregar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.Image = global::Comercial.Properties.Resources.plus;
            this.btnAgregar.Location = new System.Drawing.Point(7, 420);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(111, 35);
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
            // frmABMrubros
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(760, 465);
            this.Controls.Add(this.tvRubros);
            this.Controls.Add(this.lblRuta);
            this.Controls.Add(this.gbDatos);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnAgregar);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 420);
            this.Name = "frmABMrubros";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABM Rubros";
            this.Load += new System.EventHandler(this.frmABMrubros_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmABMrubros_KeyDown);
            this.gbDatos.ResumeLayout(false);
            this.gbDatos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvRubros;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboPadre;
        private System.Windows.Forms.Label lblRuta;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.GroupBox gbDatos;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
