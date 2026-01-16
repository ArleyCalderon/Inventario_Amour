using Inventario_Amour.Commands;
using Inventario_Amour.Helpers;
using Inventario_Amour.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.IO;

namespace Inventario_Amour.ViewModel
{
    public class MovimientoViewModel : INotifyPropertyChanged
    {
        private int _totalEntradas;
        private readonly FirebaseRepository<Movimientos> _productoMovi;

        private string _filtroTexto;
        public string FiltroTexto
        {
            get => _filtroTexto;
            set
            {
                _filtroTexto = value;
                OnPropertyChanged(nameof(FiltroTexto));
                FiltrarProductos();
            }
        }

        private ObservableCollection<Producto> _productosFiltrados = new();
        public ObservableCollection<Producto> ProductosFiltrados
        {
            get => _productosFiltrados;
            set
            {
                _productosFiltrados = value;
                OnPropertyChanged(nameof(ProductosFiltrados));
            }
        }

        public ObservableCollection<Producto> Productos { get; set; } = new();
        public Producto ProductoSeleccionado { get; set; }
        public int Cantidad { get; set; }
        public int IdProductoView { get; set; }

      

        public int Suma;
        public List<string> TiposMovimiento { get; } = new() { "entrada", "salida" };
        public string TipoSeleccionado { get; set; }

        public event Action OnCancelar;
        public ICommand RegistrarMovimientoCommand { get; }
        public ICommand CancelarMovimientoCommand { get; }

        public MovimientoViewModel()
        {
            RegistrarMovimientoCommand = new RelayCommand(RegistrarMovimiento);
            _ = CargarProductosAsync();
            //CancelarMovimientoCommand = new RelayCommand(Cancelar);
        }

        private async Task CargarProductosAsync()
        {
            string projectId = "databaseamourcaldas";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Credenciales", "databaseamourcaldas-firebase-adminsdk-fbsvc-e5ce4c1ebb.json");
            var repo = new FirebaseRepository<Producto>(jsonPath, projectId, "Productos");
            var CantidadRegistros = new FirebaseRepository<Movimientos>(jsonPath, projectId, "Movimientos");
            Suma = await CantidadRegistros.ObtenerTotalEntradas();
            var productos = await repo.GetAllAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Productos.Clear();
                foreach (var p in productos)
                    Productos.Add(p);
                ProductosFiltrados = new ObservableCollection<Producto>(Productos);
            });
        }

        private void FiltrarProductos()
        {
            if (string.IsNullOrWhiteSpace(FiltroTexto))
            {
                ProductosFiltrados = new ObservableCollection<Producto>(Productos);
                return;
            }

            var filtro = FiltroTexto.ToLower();
            var resultado = Productos.Where(p =>
                (!string.IsNullOrEmpty(p.Nombre) && p.Nombre.ToLower().Equals(filtro)) ||
                (!string.IsNullOrEmpty(p.IdProducto.ToString()) && p.IdProducto.ToString().ToLower().Equals(filtro))
            );

            ProductosFiltrados = new ObservableCollection<Producto>(resultado);
        }



        private async void RegistrarMovimiento(object parameter)
        {
            if (ProductoSeleccionado == null|| string.IsNullOrEmpty(TipoSeleccionado) || Cantidad <= 0)
            {
                MessageBox.Show("Por favor completa todos los campos.");
                return;
            }
            
            
            var movimiento = new Movimientos
            {
                IdMovimiento= Suma+1,
                
                IdProducto = ProductoSeleccionado.IdProducto,
                Tipo = TipoSeleccionado,   
                Cantidad = Cantidad
            };

            string projectId = "databaseamourcaldas";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Credenciales", "databaseamourcaldas-firebase-adminsdk-fbsvc-e5ce4c1ebb.json");
            var repoMovimientos = new FirebaseRepository<Movimientos>(jsonPath, projectId, "Movimientos");
            await repoMovimientos.AddAsync(movimiento);

            //MessageBox.Show("Movimiento registrado.");
            if (parameter is Window window)
            {
                window.Close();
            }
            Cantidad = 0;
            OnPropertyChanged(nameof(Cantidad));
        }
        private void Cancelar()
        {
            OnCancelar?.Invoke();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
