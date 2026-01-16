using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario_Amour.Model
{
    [FirestoreData]
    public class Movimientos
    {
        [FirestoreProperty]
        public int IdMovimiento { get; set; }
        [FirestoreProperty]
        public int IdProducto { get; set; }
        [FirestoreProperty("Tipo")]
        public string Tipo { get; set; }
        [FirestoreProperty]
        public int Cantidad { get; set; }
    }
}
