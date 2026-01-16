using Google.Cloud.Firestore;
using Inventario_Amour.Commands;
using Inventario_Amour.Helpers;
using Inventario_Amour.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Inventario_Amour.ViewModel
{
    public class InventarioViewModel: INotifyPropertyChanged
    {
        private readonly FirebaseRepository<Producto> _repoProductos;
        private readonly FirebaseRepository<Movimientos> _repoMovimientos;
        private ObservableCollection<Producto> _productos;
        private string _filtroNombre;
        private ObservableCollection<Producto> _productosFiltrados;

        public ObservableCollection<Producto> Productos
        {
            get => _productosFiltrados ?? _productos;
            set
            {
                _productos = value;
                OnPropertyChanged();
            }
        }

        public string FiltroNombre
        {
            get => _filtroNombre;
            set
            {
                _filtroNombre = value;
                OnPropertyChanged(nameof(FiltroNombre));
            }
        }

        public ICommand ConsultarProductosCommand { get; }

        public InventarioViewModel()
        {
            string projectId = "databaseamourcaldas";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Credenciales", "databaseamourcaldas-firebase-adminsdk-fbsvc-e5ce4c1ebb.json");

            _repoProductos = new FirebaseRepository<Producto>(jsonPath, projectId, "Productos");
            _repoMovimientos = new FirebaseRepository<Movimientos>(jsonPath, projectId, "Movimientos");

            Productos = new ObservableCollection<Producto>();
            ConsultarProductosCommand = new RelayCommand(async () => await ConsultarProductos());

            _ = CargarDatosAsync();
        }

        public async Task ConsultarProductos()
        {
            await CargarDatosAsync(FiltroNombre);
        }

        public async Task CargarDatosAsync(string filtro = null)
        {
            try
            {
                var listaProductos = await _repoProductos.GetAllAsync();
                var listaMovimientos = await _repoMovimientos.GetAllAsync();

                var productosProcesados = new List<Producto>();

                foreach (var producto in listaProductos)
                {
                    var movimientosDelProducto = listaMovimientos.Where(m => m.IdProducto == producto.IdProducto);
                    var entradas = movimientosDelProducto.Where(m => m.Tipo.ToLower() == "entrada").Sum(m => m.Cantidad);
                    var salidas = movimientosDelProducto.Where(m => m.Tipo.ToLower() == "salida").Sum(m => m.Cantidad);

                    producto.TotalEntradas = entradas;
                    producto.TotalSalidas = salidas;
                    

                    productosProcesados.Add(producto);
                }

                // Aplicar filtro si existe
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    productosProcesados = productosProcesados
                        .Where(p => p.Nombre != null && p.Nombre.ToLower().Contains(filtro.ToLower()))
                        .ToList();
                }

                Productos = new ObservableCollection<Producto>(productosProcesados);
            }
            catch (Exception ex)
            {
                // Manejar el error adecuadamente
                Console.WriteLine($"Error al cargar datos: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
