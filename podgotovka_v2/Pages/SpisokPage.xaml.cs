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
using podgotovka_v2.Model;

namespace podgotovka_v2.Pages
{
    /// <summary>
    /// Interaction logic for SpisokPage.xaml
    /// </summary>
    public partial class SpisokPage : Page
    {
        List<Product> products = new List<Product>();
        public SpisokPage()
        {
            InitializeComponent();
            var products = App.Db.Product.ToList();
            LVprod.ItemsSource = products;
            var allTypes = App.Db.Manufacturer.ToList();
            allTypes.Insert(0 , new Manufacturer{Name = "Все"});
            DiscountSortCb.SelectedIndex = 0;

            CBproiz.ItemsSource = allTypes;
            CBproiz.SelectedIndex = 0;
            Refresh();
        }
        public void Refresh()
        {
            var filterProduct = App.Db.Product.ToList();
            if (DiscountSortCb.SelectedIndex == 1)
                filterProduct = filterProduct.OrderBy(x => x.Cost).ToList();
            else if (DiscountSortCb.SelectedIndex == 2)
                filterProduct =
                    filterProduct.OrderByDescending(x => x.Cost).ToList(); //сортировка по убывания и возрастанию

            if (CBproiz.SelectedIndex > 0)
            {
                var a = CBproiz.SelectedIndex.ToString();
                filterProduct = filterProduct.Where(x => x.ManufacturerID.ToString().Contains(a.ToLower()))
                    .ToList(); //фильтрация по производителю
            }

            if (TitleDescriptionTb.Text.Length > 0)
            {
                filterProduct = filterProduct.Where(x =>
                        x.Title.ToLower().StartsWith(TitleDescriptionTb.Text.ToLower()) ||
                        x.Description.ToLower().StartsWith(TitleDescriptionTb.Text.ToLower()))
                    .ToList(); //шаблон рабоч поисковой строки
            }

            LVprod.ItemsSource = filterProduct.ToList(); //поисковая строка

        }
        private void CBproiz_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }
        private void TitleDescriptionTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }
        private void DiscountSortCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }
        private void BTNhist_OnClick(object sender, RoutedEventArgs e)
        {
            var selProduct = LVprod.SelectedItem as Product;
            if (selProduct == null)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }

            NavigationService.Navigate(new HistoryPage(selProduct));
        }
        private void BTNedt_OnClick(object sender, RoutedEventArgs e)
        {
            var select = LVprod.SelectedItem as Product;
            if (select == null)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }

            NavigationService.Navigate(new AddEditPage(select));
        }
        private void ADDbtn_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage());
        }
        private void Dekbtn_OnClick(object sender, RoutedEventArgs e)
        {
            var product = LVprod.SelectedItem as Product;
            if (product != null)
            {
                if (product.ProductSale.Count != 0)
                {
                    MessageBox.Show("Продукт не может быть удален, поскольку у продукта есть история продаж",
                        "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("Вы действительно хотите удалить эту запись?", "Уведомление",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    App.Db.Product.Remove(product);
                    App.Db.SaveChanges();
                    LVprod.ItemsSource = App.Db.Product.ToList();
                }
            }
        }
    }
}