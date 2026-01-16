using Inventario_Amour.ViewModel;
using System.Windows.Controls;


namespace Inventario_Amour.Vistas
{
    /// <summary>
    /// Lógica de interacción para Products.xaml
    /// </summary>
    public partial class Products : UserControl
    {
        public Products()
        {
            InitializeComponent();
            DataContext = new ProductoViewModel();
        }

        private void datagridinventario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
