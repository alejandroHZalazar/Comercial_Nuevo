using Comercial.Formularios;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comercial
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RegisterAppForToast();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmLogin login = new frmLogin();

            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new frmPrincipal());
            }
            else
            {
                Application.Exit();
            }

        }

        static void RegisterAppForToast()
        {
            string appId = "SistemaComercial";
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(
                @"Software\Classes\AppUserModelId\" + appId))
            {
                key.SetValue("DisplayName", "Sistema Comercial");
                key.SetValue("IconUri", exePath);
            }
        }


    }
}
