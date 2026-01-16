using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Threading.Tasks;

namespace Inventario_Amour.Helpers
{
    public class FirestoreConnectionValidator
    {
        private readonly FirestoreDb _db;

        public FirestoreConnectionValidator(FirestoreDb db)
        {
            _db = db;
        }

        public async Task<bool> ValidateConnectionAsync()
        {
            try
            {
                // Intentamos una operación simple de lectura
                Query query = _db.Collection("Productos").Limit(1);
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

                // Alternativa: Obtener un documento de configuración
                // DocumentReference docRef = _db.Collection("config").Document("connection_test");
                // DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error de conexión a Firestore: {ex.Message}",
                              "Error de Conexión",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                return false;
            }
        }
    }
}
