using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario_Amour.Model
{
    [FirestoreData]
    public class Entradas
    {
        [FirestoreProperty]
        public int IdEntrada { get; set; }
        [FirestoreProperty("Nombre")]
        public string Nombre { get; set; }
        [FirestoreProperty]
        public int Cantidad { get; set; }
    }
}
