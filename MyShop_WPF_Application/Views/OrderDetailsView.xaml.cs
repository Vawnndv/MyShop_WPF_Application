using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
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
using System.Windows.Shapes;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for OrderDetailsView.xaml
    /// </summary>
    public partial class OrderDetailsView : Window
    {
        int currentOrderId = 2;
        OrderDetailsViewModel _viewModel;

        public OrderDetailsView()
        {
            InitializeComponent();
            currentOrderId = 2;
            _viewModel = new OrderDetailsViewModel(currentOrderId);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst.ItemsSource = _viewModel.productList;
        }

        // delete product from list + DB
        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var currentItem = (OrderDetailsProductModel)button.DataContext;

            _viewModel.removeProductFromList((int)currentItem.ProductID, currentOrderId);
        }

        // edit quantity of a product in list + DB
        private void editQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var currentItem = (OrderDetailsProductModel)button.DataContext; // get selected row data
            int id = (int)currentItem.ProductID;

            if (!_viewModel.updateProductQuantity(currentOrderId, id, currentItem.orderQuantity, currentItem.ProductQuantity))
            {
                MessageBox.Show("Not enough product in stock");
            }
        }
    }
}
