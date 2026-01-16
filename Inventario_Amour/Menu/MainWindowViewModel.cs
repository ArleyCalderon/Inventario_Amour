using Inventario_Amour.Model;
using Inventario_Amour.Vistas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Inventario_Amour.Menu
{
    public class MainWindowViewModel
    {
        public MenuItem[] MenuItems { get; set; } 
        
        public MainWindowViewModel()
        {
            MenuItems = new[]
            {
                new MenuItem("Inicio", new Start(),"Home"),
                new MenuItem("Productos", new Products(),"Package"),
                new MenuItem("Inventario", new InventarioView(),"Warehouse"),

            };

        }
    }
}
