using Inventario_Amour.Commands;
using Inventario_Amour.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Inventario_Amour.Helpers;
using System.IO;
using Inventario_Amour.Service;

namespace Inventario_Amour.ViewModel
{
    public class CrearNuevoProductoViewModel : INotifyPropertyChanged
    {

        private readonly FirebaseRepository<Producto> _productoRepo;
        private string _nombre;
        private string _precio;
        private int _IdProducto;
        private int _totalEntradas;

        public event Action<Producto> ProductoCreado;

        // Método protegido para invocar el evento
        protected virtual void OnProductoCreado(Producto producto)
        {
            ProductoCreado?.Invoke(producto);
        }



        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(); }
        }
        public string Precio
        {
            get => _precio;
            set { _precio = value; OnPropertyChanged(); }
        }
        public ICommand InsertarCommand { get; }
        public ICommand CancelarCommand { get; }

        //public event Action<Entradas> OnProductoCreado;
        public event Action OnCancelar;

        // Constructor con inyección de dependencias
        public ICommand CargarTotalCommand { get; }

        public CrearNuevoProductoViewModel(FirebaseRepository<Producto> firebaseRepo)
        {
            _productoRepo = firebaseRepo;
            InsertarCommand = new RelayCommand(async () => await Insertar());
            CancelarCommand = new RelayCommand(Cancelar);
        }
        private async Task CargarTotalEntradas()
        {
            // Lógica para cargar datos desde Firebase
            int TotalEntradas = await _productoRepo.ObtenerTotalEntradas();
        }
        private async Task Insertar()
        {



            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacío");
                return;
            }
            if (string.IsNullOrWhiteSpace(Precio))
            {
                MessageBox.Show("El nombre no puede estar vacío");
                return;
            }

            try
            {
                // Obtener el total de entradas
                _totalEntradas = await _productoRepo.ObtenerTotalEntradas();
                var nuevoProducto = new Producto
                {
                    IdProducto = this._totalEntradas + 1,
                    Nombre = this.Nombre,
                    Precio=double.Parse( this.Precio),


                };
                var productos = await _productoRepo.GetAllAsync();
                bool nombreExiste = productos.Any(p =>
                    p.Nombre != null &&
                    p.Nombre.Equals(Nombre, StringComparison.OrdinalIgnoreCase));

                if (nombreExiste)
                {
                    MessageBox.Show($"El producto '{Nombre}' ya existe en la base de datos");
                    return;
                }

                // Invoca el evento usando el método protegido
                OnProductoCreado(nuevoProducto);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el total de entradas: {ex.Message}");
            }
        }

        private void Cancelar()
        {
            OnCancelar?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
