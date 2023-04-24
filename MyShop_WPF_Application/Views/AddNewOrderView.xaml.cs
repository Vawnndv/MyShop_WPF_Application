using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.UserControls;
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
    /// Interaction logic for AddNewOrderView.xaml
    /// </summary>
    public partial class AddNewOrderView : Page
    {
        AddNewOrderViewModel _viewModel;

        public AddNewOrderView()
        {
            InitializeComponent();
            var select = Dashboard.menuBTN.Children[2] as MenuButton;
            select?.btn.Focus();
            _viewModel = new AddNewOrderViewModel();

            orderStatusComboBox.ItemsSource = _viewModel.getStatusList();

            promotionCombobox.ItemsSource = _viewModel.getPromotionList();
        }

        private void addNewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string phone = customerPhoneTextBlock.Text;


            int statusID = orderStatusComboBox.SelectedIndex;
            PromotionModel promo = (PromotionModel)promotionCombobox.SelectedItem;
            int promoID = promo._promotionId;
            string name = customerNameTextBlock.Text;
            string email = customerEmailTextBlock.Text;
            string address = customerAddressTextBlock.Text;

            DateTime? newDate = oderCreateDateTextBlock.SelectedDate;


            if (phone == null || statusID == -1 ||
                promoID == -1 || phone == "" ||
                oderCreateDateTextBlock.Text == null || oderCreateDateTextBlock.Text == "" ||
                name == null || name == "" || email == null || email == ""
                )
            {
                MessageBox.Show("Xin vui lòng điền hết tất cả các thông tin của đơn hàng ");
                return;
            }

            statusID++;

            CustomerModel newCustomer = new CustomerModel() { name = name, address = address, email = email, phone = phone };

            OrderModel newOrder = new OrderModel()
            {
                PromotionID = promoID,
                CustomerPhone = phone,
                OrderDate = newDate,
                OrderTotal = 0,
                OrderStatus = statusID
            };

            if (!_viewModel.addCustomer(newCustomer))
            {
                if (MessageBox.Show("Khách hàng đã tồn tại, bạn có muốn chỉnh sửa thông tin khách hàng giống với những gì vừa nhập không ?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _viewModel.updateCustomer(newCustomer);
                }
            }

            _viewModel.addNewOrder(newOrder);
            DataContext = new MainViewModel();
            MessageBox.Show("Thêm đơn hàng mới thành công, vui lòng vào chi tiết đơn hàng này nếu bạn muốn thêm sản phẩm vào đơn");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }
    }
}