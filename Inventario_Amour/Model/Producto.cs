using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;


namespace Inventario_Amour.Model
{
    [FirestoreData]
    public class Producto:INotifyPropertyChanged
    {
        [FirestoreProperty]
        public int IdProducto { get; set; } // este es el identificador real del producto
        [FirestoreProperty("Nombre")]
        public string Nombre { get; set; }

        [FirestoreProperty]
        public double Precio { get; set; }
        public string IdDocumento { get; set; } // este almacena el ID real del doc en Firestore, por si lo necesitas

        // Propiedad solo de presentación
        public string DescripcionCombo => $"{IdProducto} - {Nombre}";

        private int _totalEntradas;
        public int TotalEntradas
        {
            get => _totalEntradas;
            set
            {
                _totalEntradas = value;
                OnPropertyChanged(nameof(TotalEntradas));
                OnPropertyChanged(nameof(Stock));
            }
        }

        private int _totalSalidas;
        public int TotalSalidas
        {
            get => _totalSalidas;
            set
            {
                _totalSalidas = value;
                OnPropertyChanged(nameof(TotalSalidas));
                OnPropertyChanged(nameof(Stock));
            }
        }

        public int Stock => TotalEntradas - TotalSalidas;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
