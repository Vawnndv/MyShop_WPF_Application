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
        CustomerModel customer;
        PromotionModel? currentPromo;

        public OrderDetailsView()
        {
            InitializeComponent();

            _viewModel = new OrderDetailsViewModel(currentOrderId);

            // get order's customer info
            customer = _viewModel.getCustormerFromDB(currentOrderId);


            orderStatusComboBox.ItemsSource = _viewModel.orderStatusList();
            orderStatusComboBox.SelectedIndex = _viewModel.getOrderStatusKey(currentOrderId) - 1;

            List<PromotionModel> promoList = _viewModel.getPromotionList();
            promotionCombobox.ItemsSource = promoList;

            int? promoID = _viewModel.getPromotionID(currentOrderId);
            if(promoID == null)
            {
                promotionCombobox.SelectedIndex = 0;
            }
            else
            {
                for(int i = 0; i < promoList.Count; ++i)
                    if(promoID == promoList[i]._promotionId)
                    {
                        promotionCombobox.SelectedIndex = i;
                        break;
                    }
            }

            currentPromo = promotionCombobox.SelectedItem as PromotionModel;

            updateMoneyTextBlock();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst.ItemsSource = _viewModel.productList;

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

            updateMoneyTextBlock();
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
            else
            {
                updateMoneyTextBlock();
            }
        }

        private async void addNewProductButton_Click(object sender, RoutedEventArgs e)
        {
            Window addProductWindow = new OrderDetailChooseProductView();
            addProductWindow.ShowDialog();

            _viewModel = new OrderDetailsViewModel(currentOrderId);
            lst.ItemsSource = _viewModel.productList;

            updateMoneyTextBlock();
        }

        private void orderStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.updateStatus(currentOrderId, orderStatusComboBox.SelectedIndex + 1);
        }

        private void promotionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPromo = (PromotionModel)promotionCombobox.SelectedItem;

            _viewModel.updatePromo(currentOrderId, currentPromo._promotionId);
            
            // update current promo

            updateMoneyTextBlock();
        }

        private void editCustomerNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (customerNameTextBlock.Text == null || customerNameTextBlock.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống tên khách hàng");
                return;
            }

            _viewModel.updateInfo(customer.phone!, customerNameTextBlock.Text, "Customer_Name");
        }

        
        private void editCustomerAddressButton_Click(object sender, RoutedEventArgs e)
        {
            if (customerAddressTextBlock.Text == null || customerAddressTextBlock.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống địa chỉ khách hàng");
                return;
            }

            _viewModel.updateInfo(customer.phone!, customerAddressTextBlock.Text, "Address");
        }

        private void editCustomerEmailButton_Click(object sender, RoutedEventArgs e)
        {
            if (customerEmailTextBlock.Text == null || customerEmailTextBlock.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống địa chỉ email khách hàng");
                return;
            }

            _viewModel.updateInfo(customer.phone!, customerEmailTextBlock.Text, "Email");
        }

        private void oderCreateDateTextBlock_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(oderCreateDateTextBlock.Text != null)
            {
                _viewModel.updateDate(currentOrderId, oderCreateDateTextBlock.Text);
            }
        }

        private void updateMoneyTextBlock()
        {
            double newTotal = _viewModel.calculateTotalMoney(currentPromo!._promotionPercentage);
            totalMoneyTextBlock.Text = newTotal.ToString();

            _viewModel.updateTotal(currentOrderId, newTotal);
        }
    }
}
