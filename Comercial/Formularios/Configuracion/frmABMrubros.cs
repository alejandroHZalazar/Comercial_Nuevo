using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios.Configuracion
{
    public partial class frmABMrubros : Form
    {
        Clases.ClassConfiguracion instConfig = new Clases.ClassConfiguracion();
        int accion = 0;
        int idRubro;
        // Jerarquía cargada (ordenada por ruta: el padre antecede a sus hijos)
        DataTable dtRubros = new DataTable();

        public frmABMrubros()
        {
            InitializeComponent();
        }

        private void frmABMrubros_Load(object sender, EventArgs e)
        {
            estadoInicial(null);
        }

        private void estadoInicial(int? idSeleccionar)
        {
            txtDescripcion.Text = string.Empty;
            gbDatos.Enabled = false;
            tvRubros.Enabled = true;
            cargarArbol(idSeleccionar);
            verificarBotones();
            tvRubros.Focus();
        }

        // Arma el árbol a partir de fk_rubroPadre; los rubros principales (fk_rubroPadre NULL) son la raíz.
        private void cargarArbol(int? idSeleccionar)
        {
            dtRubros = instConfig.traeRubrosJerarquia();

            var nodos = new Dictionary<int, TreeNode>();
            foreach (DataRow fila in dtRubros.Rows)
            {
                int id = Convert.ToInt32(fila["id"]);
                nodos[id] = new TreeNode(fila["descripcion"].ToString()) { Tag = id, ToolTipText = fila["ruta"].ToString() };
            }

            tvRubros.BeginUpdate();
            tvRubros.Nodes.Clear();
            foreach (DataRow fila in dtRubros.Rows)
            {
                TreeNode nodo = nodos[Convert.ToInt32(fila["id"])];
                TreeNode padre;
                if (fila["fk_rubroPadre"] != DBNull.Value && nodos.TryGetValue(Convert.ToInt32(fila["fk_rubroPadre"]), out padre))
                    padre.Nodes.Add(nodo);
                else
                    tvRubros.Nodes.Add(nodo);
            }
            tvRubros.ExpandAll();
            tvRubros.EndUpdate();

            TreeNode seleccionado;
            if (idSeleccionar.HasValue && nodos.TryGetValue(idSeleccionar.Value, out seleccionado))
                tvRubros.SelectedNode = seleccionado;
            else if (tvRubros.Nodes.Count > 0)
                tvRubros.SelectedNode = tvRubros.Nodes[0];

            if (tvRubros.SelectedNode != null)
                tvRubros.SelectedNode.EnsureVisible();
            mostrarUbicacion();
        }

        private int? rubroSeleccionado()
        {
            return tvRubros.SelectedNode == null ? (int?)null : (int)tvRubros.SelectedNode.Tag;
        }

        private DataRow filaRubro(int id)
        {
            DataRow[] filas = dtRubros.Select("id = " + id);
            return filas.Length > 0 ? filas[0] : null;
        }

        // Muestra la ubicación jerárquica y el rubro padre del rubro seleccionado
        private void mostrarUbicacion()
        {
            int? id = rubroSeleccionado();
            DataRow fila = id.HasValue ? filaRubro(id.Value) : null;
            if (fila == null)
            {
                lblRuta.Text = "Ubicación:";
                return;
            }

            string padre = fila["fk_rubroPadre"] == DBNull.Value ? "(rubro principal)" : fila["rubroPadre"].ToString();
            lblRuta.Text = "Ubicación:" + Environment.NewLine + fila["ruta"].ToString()
                         + Environment.NewLine + Environment.NewLine + "Rubro padre: " + padre
                         + Environment.NewLine + "Nivel: " + (Convert.ToInt32(fila["nivel"]) + 1);
        }

        private void tvRubros_AfterSelect(object sender, TreeViewEventArgs e)
        {
            mostrarUbicacion();
            verificarBotones();
        }

        private void verificarBotones()
        {
            btnAgregar.Enabled = true;
            bool haySeleccion = tvRubros.SelectedNode != null;
            btnEditar.Enabled = haySeleccion;
            btnEliminar.Enabled = haySeleccion;
        }

        // Id del rubro y de todos sus descendientes (no pueden ser su propio padre)
        private HashSet<int> idsRama(int raiz)
        {
            var resultado = new HashSet<int> { raiz };
            bool agrego = true;
            while (agrego)
            {
                agrego = false;
                foreach (DataRow fila in dtRubros.Rows)
                {
                    if (fila["fk_rubroPadre"] != DBNull.Value
                        && resultado.Contains(Convert.ToInt32(fila["fk_rubroPadre"]))
                        && resultado.Add(Convert.ToInt32(fila["id"])))
                    {
                        agrego = true;
                    }
                }
            }
            return resultado;
        }

        // Combo de rubro padre: "(Rubro principal)" + todos los rubros con su ruta completa
        private void cargarComboPadre(int? excluirRama)
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("ruta", typeof(string));
            dt.Rows.Add(0, "(Rubro principal)");

            HashSet<int> excluidos = excluirRama.HasValue ? idsRama(excluirRama.Value) : new HashSet<int>();
            foreach (DataRow fila in dtRubros.Rows)
            {
                int id = Convert.ToInt32(fila["id"]);
                if (!excluidos.Contains(id))
                    dt.Rows.Add(id, fila["ruta"].ToString());
            }

            cboPadre.DataSource = dt;
            cboPadre.DisplayMember = "ruta";
            cboPadre.ValueMember = "id";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            estadoAM();
            accion = 1;
            idRubro = 0;
            txtDescripcion.Text = string.Empty;
            cargarComboPadre(null);
            // Por defecto el nuevo rubro se crea dentro del seleccionado; se puede cambiar a "(Rubro principal)"
            int? sel = rubroSeleccionado();
            cboPadre.SelectedValue = sel.HasValue ? sel.Value : 0;
            txtDescripcion.Focus();
        }

        private void estadoAM()
        {
            tvRubros.Enabled = false;
            gbDatos.Enabled = true;
            btnAgregar.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? sel = rubroSeleccionado();
            if (!sel.HasValue) return;

            estadoAM();
            accion = 2;
            idRubro = sel.Value;
            DataRow fila = filaRubro(idRubro);
            txtDescripcion.Text = fila["descripcion"].ToString();
            cargarComboPadre(idRubro);
            cboPadre.SelectedValue = fila["fk_rubroPadre"] == DBNull.Value ? 0 : Convert.ToInt32(fila["fk_rubroPadre"]);
            txtDescripcion.Focus();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? sel = rubroSeleccionado();
            if (!sel.HasValue) return;

            int resultado = instConfig.ABMRubrosJerarquia(sel.Value, 3, tvRubros.SelectedNode.Text, null);
            if (resultadoCorrecto(resultado))
                estadoInicial(null);
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (formularioValido())
            {
                int padre = cboPadre.SelectedValue == null ? 0 : Convert.ToInt32(cboPadre.SelectedValue);
                int resultado = instConfig.ABMRubrosJerarquia(idRubro, accion, txtDescripcion.Text.Trim(), padre == 0 ? (int?)null : padre);
                if (resultadoCorrecto(resultado))
                {
                    estadoInicial(resultado > 0 ? (int?)resultado : null);
                }
            }
        }

        // >= 0 es correcto (id del rubro). Los negativos son los códigos de sp_RubrosABM
        private bool resultadoCorrecto(int resultado)
        {
            if (resultado >= 0) return true;

            string mensaje;
            switch (resultado)
            {
                case -2: mensaje = "Un rubro no puede ubicarse dentro de sí mismo ni de sus propios subrubros."; break;
                case -3: mensaje = "No se puede eliminar el rubro porque tiene subrubros. Elimine o reubique primero los subrubros."; break;
                case -4: mensaje = "No se puede eliminar el rubro porque tiene productos asociados."; break;
                case -5: mensaje = "Ya existe un rubro con esa descripción dentro del mismo rubro padre."; break;
                case -6: mensaje = "El rubro padre seleccionado no existe."; break;
                case -7: mensaje = "Debe indicar una descripción."; break;
                default: mensaje = "Ha ocurrido un error durante el procesamiento."; break;
            }
            MessageBox.Show(this, mensaje, "RUBROS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private bool formularioValido()
        {
            errorProvider1.Clear();
            if (txtDescripcion.Text == string.Empty)
            {
                errorProvider1.SetError(txtDescripcion, "Debe indicar una descripcion");
                return false;
            }

            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            estadoInicial(rubroSeleccionado());
        }

        private void txtDescripcion_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void frmABMrubros_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.F2 & btnAgregar.Enabled == true)
            {
                btnAgregar_Click(null, null);
            }

            if (e.KeyData == Keys.F3 & btnEditar.Enabled == true)
            {
                btnEditar_Click(null, null);
            }

            if (e.KeyData == Keys.F4 & btnEliminar.Enabled == true)
            {
                btnEliminar_Click(null, null);
            }

            if (e.KeyData == Keys.F5 & btnAgregar.Enabled == false)
            {
                btnGrabar_Click(null, null);
            }

            if (e.KeyData == Keys.F6 & btnAgregar.Enabled == false)
            {
                btnCancelar_Click(null, null);
            }

        }
    }
}
