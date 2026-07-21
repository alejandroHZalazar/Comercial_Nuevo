using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Comercial.Clases
{
    class ClassClientes
    {
        Clases.classDatos instDatos = new classDatos();

        public DataTable traeClientesPpal(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ClientesConsultaPpal", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable buscarAVender()
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_clientesBuscarAVender", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
           

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerConSaldo(int? unaProvincia, int? unaLocalidad, int? unaZona, int? unVendedor)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Clientes_SaldoPendiente", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unVendedorId", unVendedor);
            a1.SelectCommand.Parameters.AddWithValue("unaLocalidadId", unaLocalidad);
            a1.SelectCommand.Parameters.AddWithValue("unaProvinciaId", unaProvincia);
            a1.SelectCommand.Parameters.AddWithValue("unaZonaId", unaZona);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerDatosVenta(string unFiltro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ClientesTraerDatosVentas", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unFiltro", unFiltro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerDatosRecibo(int unCobro)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Clientes_PrintRecibo", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unCobro", unCobro);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public int traerFiltroDefecto()
        {
            MySqlCommand nComando = new MySqlCommand("select valor from parametros where modulo = 'clientes' and parametro = 'indiceBusqueda'", instDatos.abrirConexion());
            int valor = int.Parse(nComando.ExecuteScalar().ToString());
            instDatos.cerrarConexion();
            return valor;
        }

        public DataTable traerParaCombos()
        {
            MySqlDataAdapter nComando = new MySqlDataAdapter("select id, nombreComercial  from Clientes order by nombreComercial", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            nComando.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerProvincias()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id, nombre from Provincias where baja = 0 order by nombre", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerZonas()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id, nombre from ClientesZonas where baja = 0 order by nombre", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerLocalidad(int unaProvincia)
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select id, nombre from Localidades where fk_Provincia = " + unaProvincia + " order by nombre", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traerCondIVA()
        {
            MySqlDataAdapter rows = new MySqlDataAdapter("select * from condIVA", instDatos.abrirConexion());
            DataTable dt = new DataTable();
            rows.Fill(dt);
            instDatos.cerrarConexion();
            return dt;
        }

        public DataTable traeTodosDatos(int unCliente)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_ClientesTraerTodos", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unCliente", unCliente );

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerVendedores()
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_CLientesTraerVendedores", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            
            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public DataTable traerClientesExportarCSV()
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_CLientes_exportar_CSV", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public int traerUltimoCliente()
        {
            MySqlCommand nComando = new MySqlCommand("select max(id) from Clientes", instDatos.abrirConexion());
            int valor = int.Parse(nComando.ExecuteScalar().ToString());
            instDatos.cerrarConexion();
            return valor;
        }

        public string traerDetalleCobroFormaPago(int cobroId)
        {
            MySqlCommand nComando = new MySqlCommand(
                "SELECT fn_CobrosDetalleTexto(@unCobroId)",
                instDatos.abrirConexion()
            );

            nComando.Parameters.AddWithValue("@unCobroId", cobroId);

            object res = nComando.ExecuteScalar();

            string valor = "";

            if (res != null && res != DBNull.Value)
                valor = res.ToString();

            instDatos.cerrarConexion();

            return valor;
        }


        public int ABMClientes(int unId, string unNombreComercial, string unaRazonSocial, string unCuil, string unaDireccion, string unEMail, string unTelefono, string unCelular, string unContacto, int unaCondIva, int unVendedor, int unaAccion,int unaZona, int unaLocaldiad)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_ClientesABM";

                cmd.Parameters.AddWithValue("unId", unId );
                cmd.Parameters.AddWithValue("unNombreComercial", unNombreComercial );
                cmd.Parameters.AddWithValue("unaRazonSocial", unaRazonSocial );
                cmd.Parameters.AddWithValue("unCuil", unCuil);
                cmd.Parameters.AddWithValue("unaDireccion", unaDireccion );
                cmd.Parameters.AddWithValue("unEMail", unEMail );
                cmd.Parameters.AddWithValue("unTelefono", unTelefono );
                cmd.Parameters.AddWithValue("unCelular", unCelular);
                cmd.Parameters.AddWithValue("unContacto", unContacto );
                cmd.Parameters.AddWithValue("unaCondIva", unaCondIva );
                cmd.Parameters.AddWithValue("unVendedor", unVendedor );
                cmd.Parameters.AddWithValue("unaAccion", unaAccion );
                cmd.Parameters.AddWithValue("unaZona", unaZona );
                cmd.Parameters.AddWithValue("unaLocalidad", unaLocaldiad );
                

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32);
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
        // ── Password del cliente (misma lógica que ClienteService.ResetPasswordAsync de la Web) ──
        /// <summary>
        /// Genera un nuevo password seguro, lo hashea en el mismo formato que
        /// Microsoft.AspNetCore.Identity.PasswordHasher (Identity V3) y lo guarda en
        /// Clientes.passwordHash. Devuelve el password en texto plano una sola vez
        /// para mostrarlo al operador. El hash resultante es verificable por el login web.
        /// </summary>
        public string ResetPasswordCliente(int clienteId)
        {
            string password = _GenerarPasswordSeguro();
            string hash     = _HashPasswordIdentityV3(password);

            MySqlCommand cmd = new MySqlCommand(
                "update Clientes set passwordHash = @hash where id = @id",
                instDatos.abrirConexion());
            cmd.Parameters.AddWithValue("@hash", hash);
            cmd.Parameters.AddWithValue("@id", clienteId);
            cmd.ExecuteNonQuery();
            instDatos.cerrarConexion();

            return password;   // se devuelve solo una vez para mostrar al operador
        }

        /// <summary>
        /// Genera un password criptográficamente seguro de 12 caracteres:
        /// al menos 1 mayúscula, 1 minúscula, 1 dígito y 1 símbolo.
        /// (Copia exacta de ClienteService._GenerarPasswordSeguro de la versión Web.)
        /// </summary>
        private static string _GenerarPasswordSeguro()
        {
            const string mayus    = "ABCDEFGHJKLMNPQRSTUVWXYZ";   // sin I, O (confusos)
            const string minus    = "abcdefghjkmnpqrstuvwxyz";    // sin i, l, o
            const string digitos  = "23456789";                    // sin 0, 1
            const string simbolos = "!@#$%&*+-?";
            const string todos    = mayus + minus + digitos + simbolos;
            const int    longitud = 12;

            using (var rng = RandomNumberGenerator.Create())
            {
                var buf   = new byte[longitud * 4];   // extra para descarte
                var chars = new char[longitud];

                // Garantizar al menos uno de cada categoría en posiciones fijas
                chars[0] = _RndChar(mayus,    rng);
                chars[1] = _RndChar(minus,    rng);
                chars[2] = _RndChar(digitos,  rng);
                chars[3] = _RndChar(simbolos, rng);

                // Rellenar el resto aleatoriamente
                for (int i = 4; i < longitud; i++)
                    chars[i] = _RndChar(todos, rng);

                // Mezclar para que los obligatorios no estén siempre al inicio
                rng.GetBytes(buf);
                for (int i = longitud - 1; i > 0; i--)
                {
                    int j = (int)(BitConverter.ToUInt32(buf, i * 4) % (uint)(i + 1));
                    var tmp = chars[i]; chars[i] = chars[j]; chars[j] = tmp;
                }

                return new string(chars);
            }
        }

        private static char _RndChar(string charset, RandomNumberGenerator rng)
        {
            var b = new byte[4];
            rng.GetBytes(b);
            return charset[(int)(BitConverter.ToUInt32(b, 0) % (uint)charset.Length)];
        }

        /// <summary>
        /// Reproduce el formato de hash de Microsoft.AspNetCore.Identity.PasswordHasher
        /// (Identity V3): [0x01][prf][iter][saltLen][salt][subkey] en Base64.
        /// PRF = HMACSHA512, 100000 iteraciones, salt 128 bits, subkey 256 bits.
        /// Idéntico byte a byte al que produce PasswordHasher&lt;object&gt;.HashPassword,
        /// verificable por PasswordHasher.VerifyHashedPassword del login web.
        /// </summary>
        private static string _HashPasswordIdentityV3(string password)
        {
            const int prf          = 2;       // KeyDerivationPrf.HMACSHA512
            const int iterCount    = 100000;
            const int saltSize     = 16;      // 128 bits
            const int subkeyLength = 32;      // 256 bits

            byte[] salt = new byte[saltSize];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            // PBKDF2-HMACSHA512 (net461 no soporta el ctor de Rfc2898DeriveBytes con
            // HashAlgorithmName). El password se codifica en UTF8, igual que
            // KeyDerivation.Pbkdf2 que usa PasswordHasher internamente.
            byte[] subkey = _Pbkdf2HmacSha512(password, salt, iterCount, subkeyLength);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // marcador de formato V3
            _WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            _WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
            _WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt,   0, outputBytes, 13,               salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + salt.Length, subkey.Length);

            return Convert.ToBase64String(outputBytes);
        }

        private static void _WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        /// <summary>
        /// PBKDF2 (RFC 2898) con HMAC-SHA512 usando la BCL. Salida idéntica a
        /// KeyDerivation.Pbkdf2(password, salt, HMACSHA512, iterations, outputBytes).
        /// </summary>
        private static byte[] _Pbkdf2HmacSha512(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(password)))
            {
                int hashLength = hmac.HashSize / 8;                       // 64
                int blockCount = (outputBytes + hashLength - 1) / hashLength;
                byte[] output  = new byte[blockCount * hashLength];
                int    offset  = 0;

                byte[] saltAndIndex = new byte[salt.Length + 4];
                Buffer.BlockCopy(salt, 0, saltAndIndex, 0, salt.Length);

                for (int block = 1; block <= blockCount; block++)
                {
                    saltAndIndex[salt.Length + 0] = (byte)(block >> 24);
                    saltAndIndex[salt.Length + 1] = (byte)(block >> 16);
                    saltAndIndex[salt.Length + 2] = (byte)(block >> 8);
                    saltAndIndex[salt.Length + 3] = (byte)(block);

                    byte[] u = hmac.ComputeHash(saltAndIndex);
                    byte[] t = (byte[])u.Clone();
                    for (int it = 1; it < iterations; it++)
                    {
                        u = hmac.ComputeHash(u);
                        for (int k = 0; k < t.Length; k++)
                            t[k] ^= u[k];
                    }
                    Buffer.BlockCopy(t, 0, output, offset, hashLength);
                    offset += hashLength;
                }

                byte[] result = new byte[outputBytes];
                Buffer.BlockCopy(output, 0, result, 0, outputBytes);
                return result;
            }
        }

        public DataTable traerCC(int unCliente)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_Cientes_VerCC", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unCliente", unCliente);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

        public int CobrarCliente(int unCliente, decimal importeCobrar, int haceCaja, int idCaja, string detallePlanPago)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_clientes_Cobrar";

                cmd.Parameters.AddWithValue("unCliente", unCliente);
                cmd.Parameters.AddWithValue("ImporteCobro", importeCobrar);
                cmd.Parameters.AddWithValue("haceCaja", haceCaja);
                cmd.Parameters.AddWithValue("idCaja", idCaja);
                cmd.Parameters.AddWithValue("detallePlanPago", detallePlanPago);

                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32);
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

        public int NC_Cliente(int unCliente, decimal importeCobrar, string unaObserv, int haceCaja, int CajaId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_clientes_ADD_NC";

                cmd.Parameters.AddWithValue("unCliente", unCliente);
                cmd.Parameters.AddWithValue("ImporteCobro", importeCobrar);
                cmd.Parameters.AddWithValue("observ", unaObserv);                
                cmd.Parameters.AddWithValue("haceCaja", haceCaja);
                cmd.Parameters.AddWithValue("CajaId", CajaId);


                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32);
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

        public int Add_ND_Cliente(int unCliente, decimal importeDebito, string unaObserv)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = instDatos.abrirConexion();
                cmd.CommandText = "sp_clientes_AddND";

                cmd.Parameters.AddWithValue("ClienteId", unCliente);
                cmd.Parameters.AddWithValue("Importe", importeDebito);
                cmd.Parameters.AddWithValue("observ", unaObserv);


                MySqlParameter salida = new MySqlParameter("salida", MySqlDbType.Int32);
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

        public void ExportarListaClientesCsv(DataTable lista)
        {

            string carpetaDescargas = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            string archivo = $"Clientes_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            string path = Path.Combine(carpetaDescargas, archivo);

            var sb = new StringBuilder();


            // Columnas
            sb.AppendLine("Nro;Nombre Comercial;Razon Social;CUIL/CUIT;Direccion;Email;Telefono;Celular;Contacto;Condicion IVA;Vendedor;Localidad;Provincias;Zona");


            // Detalle
            foreach (DataRow row in lista.Rows)
            {
                sb.AppendLine($"{row["Nro"]};" +
              $"{row["Nombre Comercial"]};" +
              $"{row["Razon Social"]};" +
              $"{row["CUIL/CUIT"]};" +
              $"{row["Direccion"]};" +
              $"{row["Email"]};" +
              $"{row["Telefono"]};" +
              $"{row["Celular"]};" +
              $"{row["Contacto"]};" +
              $"{row["Condicion IVA"]};" +
              $"{row["Vendedor"]};" +
              $"{row["Localidad"]};" +
              $"{row["Provincias"]};" +
              $"{row["Zona"]}");
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        public DataTable traerDatosFiscales(int unCliente)
        {
            MySqlDataAdapter a1 = new MySqlDataAdapter("sp_clientes_TraerDatosFiscal", instDatos.abrirConexion());
            a1.SelectCommand.CommandType = CommandType.StoredProcedure;
            a1.SelectCommand.Parameters.AddWithValue("unCliente", unCliente);

            DataTable t2 = new DataTable();
            a1.Fill(t2);
            return t2;
        }

    }
}
