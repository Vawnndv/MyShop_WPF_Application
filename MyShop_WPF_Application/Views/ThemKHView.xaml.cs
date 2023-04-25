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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for ThemKHView.xaml
    /// </summary>
    public partial class ThemKHView : Page
    {
        QLKHViewModel _viewModel;

        public ThemKHView()
        {
            InitializeComponent();
            _viewModel = new QLKHViewModel();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Bạn muốn thêm mới một khách hàng không?",
                "Thêm sản phẩm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (editName.Text.Length == 0 || editAddress.Text.Length == 0 || editPhone.Text.Length == 0 || editEmail.Text.Length == 0)
                {
                    string message = "Vui lòng điền đủ thông tin khách hàng";
                    string title = "kiểm tra nhập thông tin";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    _viewModel._customer.name = editName.Text;
                    _viewModel._customer.phone = editPhone.Text;
                    _viewModel._customer.address = editAddress.Text;
                    _viewModel._customer.email = editEmail.Text;

                    var add = _viewModel.AddCustomer(_viewModel._customer);
                    if (add)
                    {
                        string message = "Thêm khách hàng thành công";
                        string title = "Thêm khách hàng";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
            }
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            editName.Clear();
            editPhone.Clear();
            editAddress.Clear();
            editAddress.Clear();
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }
    }
}
