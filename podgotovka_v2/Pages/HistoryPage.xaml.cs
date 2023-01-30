using podgotovka_v2.Model;
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

namespace podgotovka_v2.Pages
{
    /// <summary>
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        public HistoryPage(Product product)
        {
            InitializeComponent();
            LVEmployees.ItemsSource = product.ProductSale.ToList();
        }

        private void BackBtn_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SpisokPage());
        }
    }
}
