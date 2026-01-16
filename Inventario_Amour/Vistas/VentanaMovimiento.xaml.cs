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
    /// Lógica de interacción para VentanaMovimiento.xaml
    /// </summary>
    public partial class VentanaMovimiento : Window
    {
        public VentanaMovimiento()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (s, e) => this.Close()));
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            // Si es una pantalla grande, usa posición manual (como la que tenías)
            if (screenWidth >= 1280)
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = 675;
                this.Top = 100;
            }
            else
            {
                // En pantallas pequeñas, centra la ventana
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        
        }
    }
}
