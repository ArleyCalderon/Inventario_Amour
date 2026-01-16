using Inventario_Amour.Menu;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MenuItem = Inventario_Amour.Menu.MenuItem;
using Inventario_Amour.Menu;
using Inventario_Amour.Vistas;
using Inventario_Amour.Helpers;
using System.Windows.Controls.Primitives;
using Google.Cloud.Firestore;
using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Inventario_Amour.ViewModel;

namespace Inventario_Amour
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly FirestoreDb _firestoreDb;
        //private bool _isConnected;

        //public bool IsConnected
        //{
        //    get => _isConnected;
        //    set
        //    {
        //        _isConnected = value;
        //        OnPropertyChanged();
        //    }
        //}

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            //try
            //{
            //    var connectionString = new ConnectionString();
            //    _firestoreDb = connectionString.GetFirestoreDb();

            //    // Validar conexión al iniciar
            //    CheckConnection();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error Inicial", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }

        //public async void CheckConnection()
        //{
        //    var validator = new FirestoreConnectionValidator(_firestoreDb);
        //    IsConnected = await validator.ValidateConnectionAsync();

        //    if (!IsConnected)
        //    {
        //        // Puedes implementar reintentos aquí
        //        await Task.Delay(5000);
        //        CheckConnection();
        //    }
        //    else
        //    {

        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MenuItemsListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            ListBox lstBoxMenu = (ListBox)sender;
            MenuItem menuSelected = (MenuItem)lstBoxMenu.SelectedItems[0];
            Type typeOption = menuSelected.Content.GetType();
            var nameOption = typeOption.Name;
            object viewmodel = null;
            

            switch (nameOption)
            {
                case "Productos":
                    Vistas.Products searchInformation = (Vistas.Products)menuSelected.Content;
                    
                    //searchInformation.Refresh();
                    break;
                case "InventarioView":
                    Vistas.InventarioView inventario = (Vistas.InventarioView)menuSelected.Content;
                    inventario.Loaded += (s, e) =>
                    {
                        if (inventario.DataContext is InventarioViewModel viewModel)
                        {
                            _ = viewModel.CargarDatosAsync(); // Tu método para cargar registros
                        }
                    };
                    break;
                    //Vistas.Products searchInformations = (Vistas.Products)menuSelected.Content;
                    //Vistas.Administracion Agregar = (Vistas.Administracion)menuSelected.Content;
                    //Agregar.Refresh();
                    //break;
            }

            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            MenuToggleButton.IsChecked = false;

        }

        private void btnConsultar_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

    }
}