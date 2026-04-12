using Comercial.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial.Formularios
{
    public partial class frmPrincipal : Form
    {

        private Clases.ClassNotificaciones notificacionService = new Clases.ClassNotificaciones();
        int tieneNotificaciones = Clases.ClassParametros.buscarParametro("empresa", "tieneNotificaciones") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("empresa", "tieneNotificaciones"));
        int notificaDolar = Clases.ClassParametros.buscarParametro("empresa", "notificaDolar") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("empresa", "notificaDolar"));
        int notificaCantidadMinima = Clases.ClassParametros.buscarParametro("empresa", "notificaCantidadMinima") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("empresa", "notificaCantidadMinima"));
        private Timer timerNotificaciones;
        private int notificacionActualId;
        private string notificacionAccion;
        private int? notificacionReferenciaId;
        private NotifyIcon NotifyIcon;
        int facturaFiscal = Clases.ClassParametros.buscarParametro("ventas", "facturaFiscal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "facturaFiscal"));
        int puertoFiscal = Clases.ClassParametros.buscarParametro("ventas", "PuertoFiscal") == "" ? 0 : int.Parse(Clases.ClassParametros.buscarParametro("ventas", "PuertoFiscal"));
        string marcaFiscal = Clases.ClassParametros.buscarParametro("ventas", "marcaFiscal");
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void condicionIvaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmABMCondIva unFrmCondIva = new Configuracion.frmABMCondIva();
            unFrmCondIva .ShowDialog ();
        }

        private void porcentajeIvaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmABMivaProcentajes unFrmPorcentajeIVA = new Configuracion.frmABMivaProcentajes();
            unFrmPorcentajeIVA.ShowDialog();
        }

        private void rubrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmABMrubros unFrmRubro = new Configuracion.frmABMrubros();
            unFrmRubro.ShowDialog();
        }

        private void tiposDePreciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmABMTipoPrecios unFrmTipoPrecios = new Configuracion.frmABMTipoPrecios();
            unFrmTipoPrecios.ShowDialog();
        }

        private void tipoUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmABMTipoUsuarios unFrmTipoUsuarios = new Configuracion.frmABMTipoUsuarios();
            unFrmTipoUsuarios.ShowDialog();
            Clases.classUsuarios.setPermisosMenu(int.Parse(Environment.GetEnvironmentVariable("idUser")), this, menuStrip1);
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Usuarios.frmABMUsuarios unFrmAbmUsuarios = new Usuarios.frmABMUsuarios();
            unFrmAbmUsuarios.ShowDialog();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            
            this.Text = "Sistema de Gestión Comercial - Usuario: " + Environment.GetEnvironmentVariable("nombreUser");
            Clases.classUsuarios.setPermisosMenu(int.Parse(Environment.GetEnvironmentVariable("idUser")),this,menuStrip1 );
            facturacionPorLotesToolStripMenuItem.Visible = false;
            if (tieneNotificaciones == 0) return;
            timerNotificaciones = new Timer();
            timerNotificaciones.Interval = 60000; // 15 minutos
            timerNotificaciones.Tick += TimerNotificaciones_Tick;
            timerNotificaciones.Start();

            EjecutarVerificacion();

        }

        private void TimerNotificaciones_Tick(object sender, EventArgs e)
        {
            EjecutarVerificacion();
        }

        private void EjecutarVerificacion()
        {
           if (notificaDolar == 1) notificacionService.VerificarDolarAsync();
           if(notificaCantidadMinima == 1) notificacionService.VerificarStockMinimo();

            MostrarNotificacionesPendientes();
        }

        private void MostrarNotificacionesPendientes()
        {
            DataTable notif = notificacionService.traerNotificacionesPendientes();
            if (notif.Rows.Count == 0) return;

               
                    string mensaje = notif.Rows[0]["Mensaje"].ToString();
                    int id = Convert.ToInt32(notif.Rows[0]["Id"]);
                    string accion = notif.Rows[0]["Accion"].ToString();
                    int? referenciaId = notif.Rows[0]["ReferenciaId"] as int?;

                    MostrarNotificacion(mensaje, id, accion, referenciaId);
                
            
        }

        private void MostrarNotificacion(string mensaje, int id, string accion, int? referenciaId)
        {
            if (NotifyIcon == null)
            {
                NotifyIcon = new NotifyIcon();
                NotifyIcon.Icon = SystemIcons.Information;
                NotifyIcon.Visible = true;
                NotifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClicked;
            }

            notificacionActualId = id;
            notificacionAccion = accion;
            notificacionReferenciaId = referenciaId;

            NotifyIcon.BalloonTipTitle = "Sistema";
            NotifyIcon.BalloonTipText = mensaje;
            NotifyIcon.ShowBalloonTip(5000);
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            switch (notificacionAccion)
            {
                case "ABRIR_PRODUCTO":
                    Formularios.Productos.frmListaStock frmProd = new Productos.frmListaStock();
                    frmProd.ShowDialog();
                    break;

                case "ABRIR_CONFIG_DOLAR":
                    Formularios.Configuracion.frmABMTipoPrecios frmDolar = new Formularios.Configuracion.frmABMTipoPrecios();
                    frmDolar.ShowDialog();
                    break;
            }

            notificacionService.MarcarComoLeida(notificacionActualId);
        }
        private void aBMProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Productos.frmConsultaProductos unFrmProd = new Productos.frmConsultaProductos();
            unFrmProd.ShowDialog();
        }

        private void aBMProveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Proveedores.frmGestionProveedores unFrmGestProv = new Proveedores.frmGestionProveedores();
            unFrmGestProv.ShowDialog();
        }

        private void aBMClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Clientes.frmABMClientes unFrmABMClientes = new Clientes.frmABMClientes();
            unFrmABMClientes.ShowDialog();
        }

        private void localidadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmLocalidades unFmLocalidades = new Configuracion.frmLocalidades();
            unFmLocalidades.ShowDialog();
        }

        private void zonasClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmABMclientesZonas unFrmZonas = new Configuracion.frmABMclientesZonas();
            unFrmZonas.ShowDialog();
        }

        private void notasDePedidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proveedores.frmOrdenDeCompra unFrmOrdenCompra = new Proveedores.frmOrdenDeCompra();
            unFrmOrdenCompra.ShowDialog();
        }

        private void configurarLogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmLogo unFrmLogo = new Configuracion.frmLogo();
            unFrmLogo.ShowDialog();
        }

        private void ingresoDeMercaderiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Productos.frmIngresoMercaderia unFrmIngreso = new Productos.frmIngresoMercaderia();
            unFrmIngreso.ShowDialog();
        }

        private void cargarPedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Ventas.frmPedidos unFrmPedidos = new Ventas.frmPedidos();
            unFrmPedidos.ShowDialog();
        }

        private void venderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas.frmVentas unFrmVentas = new Ventas.frmVentas();
            unFrmVentas.ShowDialog();
        }

        private void altaMasivaDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos.frmAltaMasiva unFrmAltaMasiva = new Productos.frmAltaMasiva();
            unFrmAltaMasiva.ShowDialog();
        }

        private void listaDePreciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos.frmReportesProductos unFrmReporte = new Productos.frmReportesProductos();
            unFrmReporte.ShowDialog();
        }

        private void reportesPedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas.frmPedidosReport unFrmPedidosReport = new Ventas.frmPedidosReport();
            unFrmPedidosReport.ShowDialog();
        }

        private void reportesVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas.frmVentasReportes unFrmReportVentas = new Ventas.frmVentasReportes();
            unFrmReportVentas.ShowDialog();
        }

        private void inventarioDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos.frmListaStock unFrmListaStock = new Productos.frmListaStock();
            unFrmListaStock.ShowDialog();
        }

        private void lotesIngresosDeMercaderiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos.frmReporteIngreso unFrmReporteIngreso = new Productos.frmReporteIngreso();
            unFrmReporteIngreso.ShowDialog();
        }

        private void cambioDePreciosMasivosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos.frmCambioDePreciosMasivo unFrmCambioPreciosMasivos = new Productos.frmCambioDePreciosMasivo();
            unFrmCambioPreciosMasivos.ShowDialog();
        }

        private void movimientosDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos.frmMovimientoProductos unFrmMovimientos = new Productos.frmMovimientoProductos();
            unFrmMovimientos.ShowDialog();
        }

        private void devolucionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas.frmDevolucion unFrmDevolucion = new Ventas.frmDevolucion();
            unFrmDevolucion.ShowDialog();
        }

        private void ventasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Estadisticas.frmVentasEstadisticas unFrmEstadist = new Estadisticas.frmVentasEstadisticas();
            unFrmEstadist.ShowDialog();
        }

        private void rankingDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Estadisticas.frmRankingVentas unfrRankingVentas = new Estadisticas.frmRankingVentas();
            unfrRankingVentas.ShowDialog();
        }

        private void ajustarStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos.frmAjusteStock unFrmAjusteStock = new Productos.frmAjusteStock();
            unFrmAjusteStock.ShowDialog();
        }

        private void reportesDevolucionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ventas.frmDevolucionReport unFrmDevReport = new Ventas.frmDevolucionReport();
            unFrmDevReport.ShowDialog();
        }

        private void cambioDePreciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos.frmCambioPrecios unFrmCambioPrecios = new Productos.frmCambioPrecios();
            unFrmCambioPrecios.ShowDialog();
        }

        private void conceptosDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuracion.frmABMConceptosCaja unFrmConceptosCaja = new Configuracion.frmABMConceptosCaja();
            unFrmConceptosCaja.ShowDialog();
        }

        private void mediosDePagoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuracion.frmABMMediosPago unFrmABMMediosPago = new Configuracion.frmABMMediosPago();
            unFrmABMMediosPago.ShowDialog();
        }       

        private void gestionDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string haceCaja = Clases.ClassParametros.buscarParametro("caja", "haceCaja");

            if (!string.IsNullOrWhiteSpace(haceCaja) && haceCaja == "1")
            {
                Formularios.Contable.frmGestionCaja unFrmGestionCaja = new Contable.frmGestionCaja();
                unFrmGestionCaja.ShowDialog();
            }
            else
            {
                MessageBox.Show(this, "Debe habilitar el modulo de Caja o parametrizar su valor", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void auditoriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string haceCaja = Clases.ClassParametros.buscarParametro("caja", "haceCaja");

            if (!string.IsNullOrWhiteSpace(haceCaja) && haceCaja == "1")
            {
                Formularios.Contable.frmAuditoriaCaja unFrmAuditoriaCaja = new Contable.frmAuditoriaCaja();
                unFrmAuditoriaCaja.ShowDialog();
            }
            else
            {
                MessageBox.Show(this, "Debe habilitar el modulo de Caja o parametrizar su valor", "Gestion de Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tiposDocumentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Configuracion.frmABMDocumentosTipos unFrmTiposDoc = new Configuracion.frmABMDocumentosTipos();
            unFrmTiposDoc.ShowDialog();
        }

        private void exportarVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Estadisticas.frmExportarVentas unFrmExportarVentas = new Estadisticas.frmExportarVentas();
            unFrmExportarVentas.ShowDialog();
        }

        private void imprimirEtiquetasYCodBarrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Productos.frmImprimirEtiquetasyCB unFrmImprimirEyCB = new Productos.frmImprimirEtiquetasyCB();
            unFrmImprimirEyCB.ShowDialog();
        }

        private void frmPrincipal_Shown(object sender, EventArgs e)
        {
           
        }

        private void parametrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPass unFrmPass = new frmPass();
            unFrmPass.ShowDialog();
            if (unFrmPass.DialogResult == DialogResult.OK)
            {
                Formularios.Configuracion.frmABMEmpresa unFrmABMEmpresa = new Configuracion.frmABMEmpresa();
                unFrmABMEmpresa.ShowDialog();
            }
        }

        private void cierreZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (facturaFiscal == 1)
            {
                Fiscal unTk = new Fiscal();
                ComprobanteFiscal status;
                if (marcaFiscal.ToUpper() == "EPSON")
                {
                    var estado = unTk.CierreZEpson((short)puertoFiscal);
                }
            }
        }

        private void facturacionPorLotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Facturacion.frmFacturacionLotes unFrmLotes = new Facturacion.frmFacturacionLotes();
            unFrmLotes.FormClosed += (s, args) =>
            {
                facturacionPorLotesToolStripMenuItem.Visible = false;
            };
            unFrmLotes.ShowDialog();

        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Facturacion.frmReporteFacturacion unFrmReporte = new Facturacion.frmReporteFacturacion();
            unFrmReporte.ShowDialog();
        }

        private void dashboardVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Estadisticas.frmDashboardVentas unFrmDashVentas = new Estadisticas.frmDashboardVentas();
            unFrmDashVentas.ShowDialog();
        }

        private void saldosDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Formularios.Clientes.frmClientesConSaldo unFrnClientesSaldo = new Clientes.frmClientesConSaldo();
            unFrnClientesSaldo.ShowDialog();
        }

        private void frmPrincipal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6)
            {
                facturacionPorLotesToolStripMenuItem.Visible = true;
            }
        }
    }
}
