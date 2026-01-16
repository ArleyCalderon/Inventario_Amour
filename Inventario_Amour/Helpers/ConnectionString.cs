using FirebaseAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Windows;

namespace Inventario_Amour.Helpers
{
    public static class ConnectionString
    {
        private static FirebaseApp _firebaseApp;
        private static readonly object _lock = new object();
        private static bool _initialized = false;

        public static void Initialize(string jsonFilePath)
        {
            lock (_lock)
            {
                if (_initialized) return;

                try
                {
                    _firebaseApp = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(jsonFilePath)
                    });

                    _initialized = true;
                    Console.WriteLine("Firebase inicializado correctamente");
                    //MessageBox.Show("\"Firebase inicializado correctamente\"");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al inicializar Firebase: {ex.Message}");
                    MessageBox.Show($"Error al inicializar La base de datos: {ex.Message}");
                    throw;
                }
            }
        }

        public static void InitializeWithCredentials(string jsonContent)
        {
            lock (_lock)
            {
                if (_initialized) return;

                try
                {
                    _firebaseApp = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromJson(jsonContent)
                    });

                    _initialized = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al inicializar Firebase: {ex.Message}");
                    //MessageBox.Show($"Error al inicializar Firebase: {ex.Message}");

                    throw;
                }
            }
        }
    }
}
