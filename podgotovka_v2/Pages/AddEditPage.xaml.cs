using Microsoft.Win32;
using podgotovka_v2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;

namespace podgotovka_v2.Pages
{
    /// <summary>
    /// Interaction logic for AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Product _product;
        public AddEditPage(Product product = null)
        {
            InitializeComponent();
            InitializeProduct(product);
            SetProduct();
        }

        private void InitializeProduct(Product product)
        {
            if (product != null)
            {
                _product = product;
            }
            else
            {
                _product = new Product();
            }
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

            if (Validate() == false)
                return;

            _product.Title = TBname.Text;
            _product.Cost = decimal.Parse(Tbcost.Text);
            _product.Description = TBopis.Text;

            if (_product.ID == 0)
                App.Db.Product.Local.Add(_product);
            App.Db.SaveChanges();

            NavigationService.Navigate(new SpisokPage());
        }

        private void AddImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpg|*.jpg|*.jpeg|*.jpeg"
            };
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                _product.MainImagePath = openFileDialog.FileName;
                ServiceImg.Source = new BitmapImage(new Uri(_product.MainImagePath));

            }
        }
        //private void DeleteIngBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    var select = LVProd.SelectedItem as Product;
        //    if (select == null)
        //    {
        //        MessageBox.Show("Выберите продукт");
        //        return;
        //    }
        //    select.IsDelete = true;
        //    App.Db.SaveChanges();
        //    Refresh();
        //}
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }

        private bool Validate()
            => ErrorPho() == true
               && ErrorName() == true
               && ErrorCost() == true
               && ErrorDes() == true;

        private bool ErrorPho()
        {
            if (ServiceImg == null)
            {
                MessageBox.Show("Необходимо выбрать фотографию продукта");
                return false;
            }
            return true;
        }
        private bool ErrorName()
        {
            if (TBname.Text == "")
            {
                MessageBox.Show("Название продукта не может быть пустым");
                return false;
            }
            else
                return true;
        }
        private bool ErrorCost()
        {
            if (Tbcost.Text == "")
            {
                MessageBox.Show("Необхоимо назначить цену");
                return false;
            }
            else
                return true;
        }
        private bool ErrorDes()
        {
            if (TBopis.Text == "")
            {
                MessageBox.Show("Описание продукта не может быть пустым");
                return false;
            }
            return true;
        }
        private void SetProduct()
        {
            TBname.Text = _product.Title;
            Tbcost.Text = $"{_product.Cost:f}";
            TBopis.Text = _product.Description;

            if (_product.MainImagePath != null)
                ServiceImg.Source = new BitmapImage(new Uri(_product.MainImagePath));

        }

    }
}
