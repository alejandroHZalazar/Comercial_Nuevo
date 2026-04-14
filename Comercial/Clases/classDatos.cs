using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace Comercial.Clases
{
    class classDatos
    {
        MySqlConnection miCOnexion = new MySqlConnection("Server=72.61.47.240;Database=pañalera;User ID=remoto;Password=0315061;SslMode=None;AllowPublicKeyRetrieval=True;");
        
        public MySqlConnection abrirConexion()
        {
            try
            {
                miCOnexion.Open();
                return miCOnexion;
            }
            catch (Exception)
            {
                return miCOnexion;
            }

         }

        public void cerrarConexion()
        {
            miCOnexion.Close();
        }
    }
}

 //bool forzarError = true;

 //var service = forzarError
 //    ? throw new CDEmitterCommandErrorException(_logger, GetType().Name)
 //    : servicesScope.ServiceProvider.GetService<ICarStockService>()
 //        ?? throw new CDEmitterCommandErrorException(_logger, GetType().Name);
