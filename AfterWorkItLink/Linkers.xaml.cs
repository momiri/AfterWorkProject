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

namespace AfterWorkItLink
{
    /// <summary>
    /// Logique d'interaction pour Linkers.xaml
    /// </summary>
    public partial class Linkers : Page
    {
        public Linkers()
        {
            InitializeComponent();
        }

        private void back_to_home(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            this.NavigationService.Navigate(home);
        }
    }
}
