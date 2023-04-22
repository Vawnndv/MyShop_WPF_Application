using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
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
using System.Windows.Shapes;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for OrderDetailChooseProductView.xaml
    /// </summary>
    public partial class OrderDetailChooseProductView : Window
    {
        private OrderDetailChooseProductViewModel _viewmModel = new OrderDetailChooseProductViewModel();

        public OrderDetailChooseProductView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst.ItemsSource = _viewmModel._productList;
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lst.SelectedItem != null)
            {
                ProductModel selectedProduct = lst.SelectedItem as ProductModel;

                selectedProductImage.Source = new BitmapImage(new Uri(_viewmModel.convertToAbsolute(selectedProduct!.ProductAvatar)));
                selectedProductName.Text = selectedProduct.ProductName;
                selectedProductPrice.Text = selectedProduct.ProductPrice.ToString();
                selectedProductQuantity.Text = selectedProduct.ProductQuantity.ToString();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addProductToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if(lst.SelectedItem != null) { 
                ProductModel tempModel = lst.SelectedItem as ProductModel;

                if(tempModel != null)
                {
                 
                    if(addToOrderQuantityTextBox.Text != "" && addToOrderQuantityTextBox.Text != null)
                    {
                        if(!_viewmModel.addNewProduct(tempModel.ProductID ?? default(int), Int16.Parse(addToOrderQuantityTextBox.Text), tempModel.ProductQuantity))
                        {
                            MessageBox.Show("Số lượng sản phẩm trong đơn hàng không được lớn hơn số lượng còn trong kho");
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập số lượng");
                    }

                }



            }
        }
    }
}
