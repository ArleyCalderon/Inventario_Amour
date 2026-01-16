using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Inventario_Amour.Service;
using Inventario_Amour.Model;
using Inventario_Amour.Helpers;
using Inventario_Amour.Commands;
using System.IO;
using Inventario_Amour.Vistas;

namespace Inventario_Amour.ViewModel
{
    public class ProductoViewModel:NotifyPropertyChanged
    {
        private readonly FirebaseRepository<Producto> _productoRepo;
        private readonly FirebaseRepository<Entradas> _productoEntradas;
        private ObservableCollection<Producto> _productos;
        private Producto _productoSeleccionado;
        private string _nombrefiltrar;
        private string _filtroNombre;
        public string FiltroNombre
        {
            get => _filtroNombre;
            set
            {
                _filtroNombre = value;
                OnPropertyChanged(nameof(FiltroNombre));
            }
        }

        private ObservableCollection<Producto> _productosFiltrados;
        public ObservableCollection<Producto> ProductosFiltrados
        {
            get => _productosFiltrados;
            set
            {
                _productosFiltrados = value;
                OnPropertyChanged(nameof(ProductosFiltrados));
            }
        }

        public ObservableCollection<Producto> Productos
        {
            get => _productos;
            set
            {
                _productos = value;
                OnPropertyChanged();
            }
        }

        public Producto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                _productoSeleccionado = value;
                OnPropertyChanged();
            }
        }
        //private ICommand cargarProductosCommand;
        //public ICommand CargarProductosCommand 
        //{
        //    get
        //    {
        //        if(cargarProductosCommand==null)
        //        {
        //            cargarProductosCommand = new RelayCommand(p => CargarProductos());
        //        }
        //        return cargarProductosCommand;
        //    }
        //}
        public ICommand AgregarProductoCommand { get; }
        public ICommand EliminarProductoCommand { get; }
        public ICommand CargarProductosCommand { get; }

        public ICommand ConsultarProductosCommand { get; }
        public ProductoViewModel()
        {
            string projectId = "databaseamourcaldas";
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Credenciales", "databaseamourcaldas-firebase-adminsdk-fbsvc-e5ce4c1ebb.json");
            _productoRepo = new FirebaseRepository<Producto>(jsonPath, projectId, "Productos");
            _productoEntradas = new FirebaseRepository<Entradas>(jsonPath, projectId, "Entradas");
            Productos = new ObservableCollection<Producto>();

            // Configurar comandos
            CargarProductosCommand = new RelayCommand(async () => await CargarProductos());
            AgregarProductoCommand = new RelayCommand(AgregarProducto);
            EliminarProductoCommand = new RelayCommand(EliminarProducto);
            ConsultarProductosCommand = new RelayCommand(async () => await ConsultarProductos());
            // Cargar productos al iniciar
            CargarProductosCommand.Execute(null);
        }

        public async Task ConsultarProductos()
        {
            try
            {
                // Primero cargamos todos los productos desde Firebase
                var todosProductos = await _productoRepo.GetAllAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Si no hay filtro o está vacío, mostramos todos los productos
                    if (string.IsNullOrWhiteSpace(FiltroNombre))
                    {
                        Productos = new ObservableCollection<Producto>(todosProductos);
                    }
                    else
                    {
                        // Filtramos los productos por nombre
                        var filtrados = todosProductos
                            .Where(p => p.Nombre != null &&
                                        p.Nombre.ToLower().Contains(FiltroNombre.ToLower()))
                            .ToList();
                        Productos = new ObservableCollection<Producto>(filtrados);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar productos: {ex.Message}");
            }
        }

        public async Task CargarProductos()
        {
            try
            {
                var productos = await _productoRepo.GetAllAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Productos.Clear();
                    foreach (var producto in productos)
                    {
                        Productos.Add(producto);
                    }
                });

                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}");
            }
        }

        private async void EliminarProducto()
        {
            var ventanaCrear = new VentanaMovimiento();
            var viewModel = new MovimientoViewModel();
            ventanaCrear.DataContext = viewModel;

            ventanaCrear.ShowDialog();

            

        
        }

        private async void AgregarProducto()
        {
            var ventanaCrear = new CrearNuevoProducto();
            var viewModel = new CrearNuevoProductoViewModel(_productoRepo);
            ventanaCrear.DataContext = viewModel;

            var completionSource = new TaskCompletionSource<Producto>();

            // Suscripción correcta al evento usando +=
            viewModel.ProductoCreado += (entrada) =>
            {
                completionSource.SetResult(entrada);
                ventanaCrear.Close();
            };

            // Para el evento de cancelar
            viewModel.OnCancelar += () =>
            {
                completionSource.SetResult(null);
                ventanaCrear.Close();
            };

            ventanaCrear.ShowDialog();

            var nuevaEntrada = await completionSource.Task;

            if (nuevaEntrada != null)
            {
                try
                {
                    // Verifica adicionalmente el repositorio
                    if (_productoRepo == null)
                    {
                        MessageBox.Show("Error: El repositorio no está inicializado");
                        return;
                    }

                    // Verifica propiedades requeridas
                    if (string.IsNullOrEmpty(nuevaEntrada.IdProducto.ToString()))
                    {
                        MessageBox.Show("Error: El ID de entrada no puede estar vacío");
                        return;
                    }

                    await _productoRepo.AddAsync(nuevaEntrada);
                    await CargarProductos();
                    //MessageBox.Show($"Entrada #{nuevaEntrada.IdEntrada} creada exitosamente");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al agregar entrada: {ex.Message}");
                }
            }
         
        }
    }
}
