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

        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var currrentItem = (OrderDetailsProductModel)button.DataContext;

            _viewModel.removeProductFromList((int)currrentItem.ProductID, currentOrderId);
        }
    }
}
