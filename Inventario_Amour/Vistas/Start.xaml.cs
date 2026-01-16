using Google.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventario_Amour.Vistas
{
    /// <summary>
    /// Lógica de interacción para Star.xaml
    /// </summary>
    public partial class Start : UserControl
    {
        public Start()
        {
            InitializeComponent();
        }
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {

            MainWindow inicio = new MainWindow();
            inicio.ShowDialog();
            Application.Current.Windows[0].Close();


        }
    }
}
