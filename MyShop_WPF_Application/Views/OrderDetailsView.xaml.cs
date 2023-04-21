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
        int currentOrderId = Global.selectedOrderID;
        OrderDetailsViewModel _viewModel;

        public OrderDetailsView()
        {
            InitializeComponent();
            _viewModel = new OrderDetailsViewModel(currentOrderId);

            totalMoneyTextBlock.Text = _viewModel.calculateTotalMoney().ToString();

            orderStatusComboBox.ItemsSource = _viewModel.orderStatusList();
            orderStatusComboBox.SelectedIndex = _viewModel.getOrderStatusKey(currentOrderId) - 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst.ItemsSource = _viewModel.productList;

            CustomerModel customer = _viewModel.getCustormerFromDB(currentOrderId);

            customerNameTextBlock.Text = customer.name;
            customerPhoneTextBlock.Text = customer.phone;
            customerEmailTextBlock.Text = customer.email;
            customerAddressTextBlock.Text = customer.address;
            oderCreateDateTextBlock.Text = _viewModel.getDateFromDB(currentOrderId).ToShortDateString();
            orderIDTextBlock.Text = currentOrderId.ToString();
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

        private async void addNewProductButton_Click(object sender, RoutedEventArgs e)
        {
            Window addProductWindow = new OrderDetailChooseProductView();
            addProductWindow.ShowDialog();

            _viewModel = new OrderDetailsViewModel(currentOrderId);
            lst.ItemsSource = _viewModel.productList;

            totalMoneyTextBlock.Text = _viewModel.calculateTotalMoney().ToString();
        }

        private void orderStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.updateStatus(currentOrderId, orderStatusComboBox.SelectedIndex + 1);
        }
    }
}
