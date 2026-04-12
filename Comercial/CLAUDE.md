# CLAUDE.md — Sistema de Gestión Comercial

Guía de contexto para el desarrollo de este proyecto. Leer antes de cualquier tarea.

---

## Descripción del sistema

Sistema de gestión comercial completo para retail/mayoristas del mercado argentino. Cubre el ciclo de ventas completo, inventario, cuentas corrientes, caja, facturación fiscal y electrónica, estadísticas y reportes. Desarrollado por Alejandro ZALAZAR.

---

## Stack tecnológico

| Item | Valor |
|------|-------|
| Lenguaje | C# |
| Framework UI | Windows Forms (WinForms) |
| Target | .NET Framework 4.6.1 |
| Base de datos | MySQL remoto (Server: 72.61.47.240, DB: `ale`) |
| Conector BD | MySqlConnector 2.5.0 / MySql.Data 8.1.0 |
| Reportes | Microsoft ReportViewer (RDLC) + iTextSharp 5.5.13.5 (PDF) |
| Gráficos | LiveCharts 0.9.7 |
| Excel | ClosedXML 0.105.0 / OleDbConnection (importación) |
| JSON | Newtonsoft.Json 13.0 |
| Impresoras fiscales | EPSON (COM Interop) + Hasar SMH/P-322F (OCXFISLib/FiscalNET) |
| Cultura | es-AR (separador CSV: `;`, decimales con `,`) |

---

## Estructura de carpetas

```
Comercial/
├── Clases/          ← Lógica de negocio + acceso a datos
├── Formularios/     ← Interfaces de usuario (WinForms)
│   ├── Clientes/
│   ├── Ventas/
│   ├── Productos/
│   ├── Proveedores/
│   ├── Facturacion/
│   ├── Contable/
│   ├── Estadisticas/
│   ├── Configuracion/
│   └── Usuarios/
├── Reportes/        ← Archivos RDLC + datasets + frmReport.cs
├── Enums/           ← Enumeraciones del dominio
├── Properties/      ← Settings y recursos
└── App.config       ← Connection strings y binding redirects
```

---

## Arquitectura en capas

```
[Formularios\*.cs]          ← Capa de presentación (WinForms)
        ↓↑ DataTable
[Clases\Class*.cs]          ← Capa de negocio y datos
        ↓↑ MySqlCommand / MySqlDataAdapter
[classDatos.cs]             ← Conexión MySQL (abre/cierra por operación)
        ↓↑
[MySQL — Stored Procedures] ← Lógica pesada en SP
```

**Regla clave:** No hay ORM. Los datos siempre viajan como `DataTable` desde la BD hasta el formulario. Los grids y combos se databinden directamente al DataTable.

---

## Clases principales (Clases\)

| Clase | Responsabilidad |
|-------|----------------|
| `classDatos.cs` | Wrapper de conexión MySQL (`abrirConexion` / `cerrarConexion`) |
| `ClassVentas.cs` | Ventas: cabecera, detalle, impresión ticket/factura, devoluciones |
| `ClassPedidos.cs` | Órdenes de compra: cabecera y detalle |
| `ClassProductos.cs` | Inventario, stock, precios, movimientos, lotes |
| `ClassClientes.cs` | ABM clientes, cuentas corrientes, saldos, zonas |
| `ClassProveedores.cs` | ABM proveedores, coeficientes, reposición automática |
| `ClassCaja.cs` | Apertura/cierre de caja, movimientos, saldo |
| `ClassEstadisticas.cs` | Estadísticas, rankings, datos de dashboard |
| `ClassParametros.cs` | Lee configuración de la tabla `parametros` (key-value por módulo) |
| `ClassConfiguracion.cs` | Datos maestros: IVA, rubros, zonas, tipos de precio, medios de pago |
| `classUsuarios.cs` | Autenticación, roles, permisos de menú, vendedores |
| `ClassLocalidades.cs` | Provincias y localidades argentinas |
| `ClassColores.cs` | Colores de productos |
| `ClassNotificaciones.cs` | Alertas de stock mínimo y variación del dólar (API externa) |
| `ClassProductosBalanza.cs` | Parsing de códigos de barras de balanza (EAN-13) |
| `ClassReportesITextSharp.cs` | Generación de recibos no fiscales en PDF (iTextSharp) |
| `ClassReportesFiscal.cs` | Resumen/detalle de facturación, exportación CSV |
| `ClassFacturacionElectronica.cs` | Envío de facturas a API REST externa |
| `Fiscal.cs` | Control de impresoras fiscales EPSON y Hasar vía COM |
| `TicketPrinter.cs` | Impresión térmica de tickets (42 o 58 caracteres de ancho) |
| `ClassUtil.cs` | Exportación de DataGridView/DataTable a CSV |
| `ClassValidacion.cs` | Validaciones de datos de entrada |

---

## Formularios por módulo (Formularios\)

### Clientes
- `frmABMClientes` — Lista y búsqueda de clientes
- `frmAltaModifClientes` — Alta/modificación de cliente (CUIT, IVA, zona, vendedor)
- `frmClientesCC` — Cuenta corriente del cliente
- `frmClientesConSaldo` — Clientes con saldo pendiente
- `frmAddND` — Alta de nota de débito/crédito

### Ventas
- `frmPedidos` — Carga de pedidos (búsqueda producto, descuentos, recargos)
- `frmVenta` — Punto de venta / POS (efectivo, tarjeta, cuenta corriente)
- `frmDevolucion` — Devoluciones (selecciona venta, ítems, genera nota)
- `frmDevolucionReport` — Reporte de devoluciones
- `frmImputacionVenta` — Imputación de pagos

### Productos
- `frmAltaMasiva` — Importación masiva desde Excel
- `frmAltaModifProductos` — Alta/modificación de producto
- `frmCambioDePreciosMasivo` — Actualización masiva de precios
- `frmCambioPrecios` — Cambio de precio individual
- `frmAjusteStock` — Ajuste de inventario
- `frmListaStock` — Listado de stock con alertas de mínimo
- `frmEtiquetas` — Impresión de etiquetas y códigos de barras

### Proveedores
- `frmGestionProveedores` — Lista y búsqueda de proveedores
- `frmAltaModifProveedores` — Alta/modificación de proveedor
- `frmNotaPedidosPendientes` — Órdenes de compra pendientes
- `frmListaProductosAPedir` — Lista inteligente de reposición por historial
- `frmOrdenDeCompra` — Generación e impresión de orden de compra
- `frmResumenPagos` — Resumen de pagos a proveedor

### Facturación
- `frmFacturacionLotes` — Facturación en lote (múltiples ventas → impresora fiscal)
- `frmIngresarDatosNC` — Datos para nota de crédito
- `frmReporteFacturacion` — Reportes de facturación

### Contable / Caja
- `frmAperturaCaja` — Apertura de caja con saldo inicial
- `frmCierreCaja` — Cierre y arqueo de caja
- `frmArqueoCaja` — Auditoría de caja
- `frmAuditoriaCaja` — Trazabilidad de movimientos
- `frmEgresoDinero` — Registro de egresos
- `frmIngresoDinero` — Registro de ingresos

### Estadísticas
- `frmVentasEstadisticas` — Estadísticas por período y filtros
- `frmRankingVentas` — Ranking de productos y clientes
- `frmDashboardVentas` — Dashboard con gráficos (LiveCharts)
- `frmExportarVentas` — Exportación de datos de ventas

### Configuración
- `frmABMCondIva` — Condiciones de IVA
- `frmABMivaProcentajes` — Porcentajes de IVA (21%, 10.5%, etc.)
- `frmABMrubros` — Rubros/categorías de productos
- `frmABMTipoPrecios` — Tipos de precio (mayorista, minorista, etc.)
- `frmABMTipoUsuarios` — Roles y permisos de usuario
- `frmABMLocalidades` — Localidades y provincias
- `frmABMZonasClientes` — Zonas de clientes
- `frmABMEmpresa` — Datos de la empresa
- `frmABMMediosPago` — Medios de pago
- `frmABMConceptosCaja` — Conceptos de movimientos de caja
- `frmABMDocumentosTipos` — Tipos de documento

### Autenticación
- `frmLogin` — Login (usuario + contraseña)
- `frmPass` — Cambio de contraseña

---

## Reportes (Reportes\)

Todos los reportes se visualizan en `frmReport.cs` usando Microsoft ReportViewer.

| Archivo RDLC | Descripción |
|-------------|-------------|
| `ReportVenta.rdlc` | Comprobante de venta |
| `ReportDevolucion.rdlc` | Comprobante de devolución |
| `ReportPedidos.rdlc` | Listado de pedidos |
| `ReportOrdenCompra.rdlc` | Orden de compra a proveedor |
| `ReportProductosStock.rdlc` | Inventario de productos |
| `ReportProductoPedir.rdlc` | Lista de productos a reponer |
| `ReportListaDePreciosPorRubro.rdlc` | Lista de precios por rubro |
| `ReportVentasEstadisticas.rdlc` | Estadísticas de ventas |
| `reportVentasCom.rdlc` | Ventas por comisión |
| `Report1.rdlc` | Uso general |

Datasets: `dsComercial.xsd`, `dsComercial1.xsd`

---

## Enumeraciones (Enums\)

```csharp
// Medios de pago disponibles
enum MedioPago { Efectivo, Tarjeta_Debito, Tarjeta_Credito, Transferencia, CuentaCorriente }

// Tipos de comprobante Hasar
enum TipoComprobanteHassar { FacturaA, FacturaB, FacturaC, NotaCredito, NotaDebito }

// Provincias argentinas (24 + OTRO)
enum ProvinciasEnum { BuenosAires = 2, Cordoba = 6, SantaFe = 21, ... }
```

---

## Configuración runtime — tabla `parametros`

Toda la configuración de comportamiento del sistema se guarda en la tabla `parametros` de MySQL. Se accede así:

```csharp
string valor = ClassParametros.buscarParametro("modulo", "clave");
```

| Módulo | Claves relevantes |
|--------|------------------|
| `empresa` | razonSocial, cuit, domicilio, logo |
| `ventas` | PuertoFiscal, facturaFiscal, facturaElectronica, cantDecimales |
| `productos` | cantDecimales, cantDecimalesStock, dolarizado |
| `clientes` | llevaCC |
| `facturacionElectronica` | userToken, apiKey, apiToken, enviarFacturaPorMail |
| `PuntoVenta` | numero |
| `color` | colorPrimario, colorSecundario |
| `login` | cantUsuarios |
| `notificaciones` | activarDolar, activarStockMinimo |

---

## Sesión de usuario

La sesión activa se almacena en variables de entorno del proceso:

```csharp
Environment.SetEnvironmentVariable("nombreUser", usuario);
Environment.SetEnvironmentVariable("tipoUser", tipo);
Environment.SetEnvironmentVariable("idUser", id);
```

Los permisos de menú se aplican dinámicamente al iniciar la sesión mediante `classUsuarios.setPermisosMenu(frmPrincipal)`.

---

## Integraciones externas

| Integración | Detalle |
|------------|---------|
| Impresora fiscal EPSON | COM Interop: `EPSON_Impresora_Fiscal`. Puerto configurado en `parametros.ventas.PuertoFiscal` |
| Impresora fiscal Hasar | COM Interop: `OCXFISLib.DriverFiscal` / `FiscalNET`. Modelo: SMH/P-322F |
| Facturación electrónica | REST API externa. Auth por `userToken` + `apiKey` + `apiToken` (en tabla parametros) |
| Tipo de cambio dólar | GET `https://cdn.moneyconvert.net/api/latest.json` (usado en notificaciones) |
| Importación Excel | `OleDbConnection` sobre `.xls/.xlsx` para alta masiva de productos |

---

## Convenciones del código

- **Métodos de consulta:** retornan `DataTable` via `MySqlDataAdapter.Fill()`
- **Métodos de escritura:** usan `MySqlCommand.ExecuteNonQuery()` con stored procedures
- **Parámetros SQL:** se pasan como `cmd.Parameters.AddWithValue("@param", valor)`
- **Conexión:** siempre se abre y cierra dentro del método (`abrirConexion()` / `cerrarConexion()`)
- **Nombres:** español en todo el código (variables, métodos, formularios)
- **Stored Procedures:** prefijo `sp_` + módulo + acción (ej: `sp_VentasGrabarCabecera`)
- **Export CSV:** separador `;`, carpeta `Downloads` del usuario actual
- **Export PDF:** carpeta `Downloads`, se abre automáticamente al generar

---

## Reglas para el desarrollo

1. **Respetar la separación de capas:** la lógica de negocio y acceso a datos va en `Clases\`, nunca inline en un formulario.
2. **No usar ORM:** trabajar con `DataTable`, `MySqlCommand` y `MySqlDataAdapter` como el resto del código.
3. **Stored Procedures para operaciones complejas:** las consultas simples pueden ir inline, pero inserciones/actualizaciones complejas deben usar SP.
4. **Leer `ClassParametros` para comportamiento configurable:** no hardcodear valores que puedan variar por cliente (decimales, puertos, flags de módulos).
5. **Respetar la cultura `es-AR`:** fechas `dd/MM/yyyy`, decimales con coma, CSV con punto y coma.
6. **El menú principal es `frmPrincipal`:** los formularios hijos se abren como `MdiChild` o modal desde allí.
7. **Permisos:** antes de abrir cualquier formulario nuevo verificar si el módulo ya contempla permisos en `setPermisosMenu`.
8. **Sin rompre compatibilidad fiscal:** cualquier cambio en `Fiscal.cs`, `TicketPrinter.cs` o `ClassFacturacionElectronica.cs` requiere prueba exhaustiva — afecta documentos legales.
