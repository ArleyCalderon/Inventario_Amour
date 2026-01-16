using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using FirebaseAdmin;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using Inventario_Amour.Helpers;

namespace Inventario_Amour
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ruta al archivo JSON de tu cuenta de servicio
            string credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Credenciales", "databaseamourcaldas-firebase-adminsdk-fbsvc-e5ce4c1ebb.json");

            ConnectionString.Initialize(credentialPath);
        }
    }
}