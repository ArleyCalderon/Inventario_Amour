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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario_Amour.Helpers
{
    public class FirebaseRepository<T> where T : class
    {
        private readonly FirestoreDb _db;
        private readonly string _collectionName;

        public FirebaseRepository(string jsonPath, string projectId, string collectionName)
        {
            // Configura las credenciales explícitamente
            var builder = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                CredentialsPath = jsonPath
            };

            _db = builder.Build();
            _collectionName = collectionName;
        }

        public async Task<string> AddAsync(T item)
        {
            DocumentReference docRef = await _db.Collection(_collectionName).AddAsync(item);
            return docRef.Id;
        }

        public async Task UpdateAsync(string id, T item)
        {
            DocumentReference docRef = _db.Collection(_collectionName).Document(id);
            await docRef.SetAsync(item, SetOptions.MergeAll);
        }

        public async Task DeleteAsync(string id)
        {
            DocumentReference docRef = _db.Collection(_collectionName).Document(id);
            await docRef.DeleteAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            DocumentReference docRef = _db.Collection(_collectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            return snapshot.Exists ? snapshot.ConvertTo<T>() : null;
        }
        public interface IHasId
        {
            string IdProducto { get; set; }
        }
        public async Task<List<T>> GetAllAsync(int limit = 0)
        {
            Query query = _db.Collection(_collectionName);

            if (limit > 0)
                query = query.Limit(limit);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            //QuerySnapshot querySnapshot = await _db.Collection(_collectionName).GetSnapshotAsync();
            List<T> items = new List<T>();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    T item = documentSnapshot.ConvertTo<T>();
                    // Si implementas IHasId
                    var prop = item.GetType().GetProperty("IdDocumento");
                    if (prop != null)
                        prop.SetValue(item, documentSnapshot.Id);
                    items.Add(item);
                }
            }
            return items;
        }


        public async Task<int> ObtenerTotalEntradas()
        {

            try
            {
                var query = _db.Collection(_collectionName);
                var snapshot = await query.GetSnapshotAsync();
                return snapshot.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener total de entradas: {ex.Message}");
                return 0;
            }
        }
    }
}
