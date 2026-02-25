using MySqlConnector;
using System;
using System.Data;

namespace Comercial.Clases
{
    class ClassConfiguracion
    {
        classDatos instDatos = new classDatos();

        public DataTable traeCondIVA()                         
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from condIVA", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traePorcentajesIva()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from ivaPorcentajes", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }
       

        public DataTable traeZonasClientes()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id,nombre from ClientesZonas where baja = 0 order by nombre", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traeRubros()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from Rubros", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traeRubrosconTdos()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select 0 as id ,'TODOS' as descripcion union select id, descripcion from Rubros ORDER BY id = 0 DESC, descripcion", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }
        public DataTable traeTipoUsuarios()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from tipoUsuarios order by id", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traeTipoPrecios()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select t.id, t.descripcion,v.descripcion as 'Tipo',t.valor,t.fk_tipoValor from tipoPrecios as t inner join tipoValoresPrecios as v on v.id = t.fk_tipoValor" , instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traeUsuarios()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select u.id, u.nombre,t.descripcion, u.tipo from usuarios as u inner join tipoUsuarios as t on u.tipo = t.id where u.baja = 0 ", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerMenuPermisos()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from menuPermisos order by id", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerTipoUsuariosPermisos(int unTipoUsuario)
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from tipoDeUsuariosPermisos where fk_tipoUsuario = " + unTipoUsuario  , instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        } 

        public void ABMcondIva(string unaDescripcion, string unaAbrev, string unaLetra, int unId, int unTipo)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMcondIva";

                cmd.Parameters.AddWithValue("unaDescripcion", unaDescripcion);
                cmd.Parameters.AddWithValue("unaAbrev", unaAbrev );
                cmd.Parameters.AddWithValue("unaLetra", unaLetra );
                cmd.Parameters.AddWithValue("unId", unId );
                cmd.Parameters.AddWithValue("tipo", unTipo );

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
 

        }

        public void ABMZonasClientes(int unId, string unNombre, int unaAccion)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMZonasClientes";

                cmd.Parameters.AddWithValue("unId", unId );
                cmd.Parameters.AddWithValue("unNombre", unNombre );
                cmd.Parameters.AddWithValue("unaAccion", unaAccion );
                

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }


        }



        public void ABMPorcentajeIva(int unId, int unTipo, decimal unValor)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMporcentajeIva";

                cmd.Parameters.AddWithValue("unId", unId );
                cmd.Parameters.AddWithValue("unTipo", unTipo );
                cmd.Parameters.AddWithValue("unValor", unValor );
                
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
          }

        public DataTable traetipoValoresPrecios()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from tipoValoresPrecios", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerConceptosCaja()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select concepto_caja_id id, nombre Nombre, tipo_movimiento Tipo, afecta_efectivo from conceptos_caja", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerConceptosCajaPorId(int unId)
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select concepto_caja_id id, nombre Nombre, tipo_movimiento Tipo, afecta_efectivo from conceptos_caja where concepto_caja_id = " + unId, instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerMediosDePago()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select medio_pago_id id, nombre Nombre, conDatos `Necesita Datos` from medios_pago", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerTipoGastos()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id Nro, nombre Nombre from tipo_gastos", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerMediosDePagoPorId(int unId)
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select medio_pago_id id, nombre Nombre, conDatos from medios_pago where medio_pago_id = " + unId, instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerPlanesPagoPorMedio(int unMedio)
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id Nro, nombre Nombre, recargo Recargo from planes_pago where fk_medioPago = " + unMedio, instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerPlanesPagoPorId(int unId)
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from planes_pago where id = " + unId, instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerPlanesPago()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from planes_pago", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }
        public void ABMRubros(int unId, int unTipo, string unaDescripcion)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMrubros";

                cmd.Parameters.AddWithValue("unId", unId);
                cmd.Parameters.AddWithValue("unTipo", unTipo);
                cmd.Parameters.AddWithValue("unaDescripcion", unaDescripcion );

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public void ABMTipoPrecios(string unaDescrpcion, int unTipoValor, decimal unValor, int unTipo, int unId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMtipoPrecios";

                cmd.Parameters.AddWithValue("unaDescripcion", unaDescrpcion );
                cmd.Parameters.AddWithValue("unTipoValor", unTipoValor );
                cmd.Parameters.AddWithValue("unValor", unValor );
                cmd.Parameters.AddWithValue("unTipo", unTipo );
                cmd.Parameters.AddWithValue("unId", unId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public void ABMUsuarios(int unID,string unNombre, string unPass, int unTipo, int unaAccion)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMUsuarios";

                cmd.Parameters.AddWithValue("unId", unID);
                cmd.Parameters.AddWithValue("unNombre", unNombre );
                cmd.Parameters.AddWithValue("unPass", unPass );
                cmd.Parameters.AddWithValue("unTipo", unTipo );
                cmd.Parameters.AddWithValue("unaAccion", unaAccion );
                

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public int ABMtipoUsuarios(string unaDescrpcion, int unId,  int unTipo, string permisos)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMtipoUsuarios";

                cmd.Parameters.AddWithValue("unaDescripcion", unaDescrpcion);
                cmd.Parameters.AddWithValue("unTipo", unTipo);
                cmd.Parameters.AddWithValue("unId", unId);
                cmd.Parameters.AddWithValue("permisos", permisos);

                MySqlParameter salida = new MySqlParameter("salida",MySqlDbType.Int32 );
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                int valor = Int32.Parse(cmd.Parameters["salida"].Value.ToString());
                return valor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public string ABMConceptosCaja(string unNombre, string unTipoMovimiento, bool unAfectaEfectivo, int unTipo, int unId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMconceptosCaja";

                cmd.Parameters.AddWithValue("unNombre", unNombre);
                cmd.Parameters.AddWithValue("unTipoMovimiento", unTipoMovimiento);
                cmd.Parameters.AddWithValue("unAfectaEfectivo", unAfectaEfectivo);
                cmd.Parameters.AddWithValue("unId", unId);
                cmd.Parameters.AddWithValue("tipo", unTipo);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public string ABMMediosPago(string unNombre, int unTipo, int unId, bool unConDatos)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMmediosPago";

                cmd.Parameters.AddWithValue("unNombre", unNombre);                
                cmd.Parameters.AddWithValue("unConDatos", unConDatos);
                cmd.Parameters.AddWithValue("tipo", unTipo);
                cmd.Parameters.AddWithValue("unId", unId);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public string ABMPlanesPago(int unMedioPago, string unNombre, decimal unRecargo, int unTipo, int unId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMPlanesPago";

                cmd.Parameters.AddWithValue("unMedioPago", unMedioPago);
                cmd.Parameters.AddWithValue("unNombre", unNombre);
                cmd.Parameters.AddWithValue("unRecargo", unRecargo);
                cmd.Parameters.AddWithValue("unId", unId);
                cmd.Parameters.AddWithValue("tipo", unTipo);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public string ABMTiposGastos(string unNombre, int unTipo, int unId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMTiposGastos";

                cmd.Parameters.AddWithValue("unNombre", unNombre);
                cmd.Parameters.AddWithValue("unId", unId);
                cmd.Parameters.AddWithValue("tipo", unTipo);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }

        public DataTable traerTiposDocumentos()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id as Nro, Abreviatura Abrev, descripcion as Nombre from Documentos_Tipo order by descripcion", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public string ABMTiposDocumentos(int unId, string unNombre, string unAbrev, int unTipo)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ConfiguracionABMTiposDocumentos";

                cmd.Parameters.AddWithValue("unNombre", unNombre);
                cmd.Parameters.AddWithValue("unAbrev", unAbrev);                
                cmd.Parameters.AddWithValue("unId", unId);
                cmd.Parameters.AddWithValue("tipo", unTipo);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.VarChar);
                salida.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(salida);

                cmd.ExecuteScalar();
                string valor = cmd.Parameters["salida"].Value.ToString();
                return valor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                instDatos.cerrarConexion();
            }
        }
    }
}
