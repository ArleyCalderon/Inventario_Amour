using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario_Amour.Model
{
    [FirestoreData]
    public class Salidas
    {
        [FirestoreProperty]
        public int IdSalida { get; set; }
        [FirestoreProperty]
        public int IdProducto { get; set; }
        [FirestoreProperty]
        public int Cantidad { get; set; }
    }
}
