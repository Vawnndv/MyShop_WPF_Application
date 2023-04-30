using MyShop_WPF_Application.UserControls;
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
            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            bool isMatch = Regex.IsMatch(editEmail.Text, pattern);
            if (editName.Text.Length == 0 || editAddress.Text.Length == 0 || editPhone.Text.Length == 0 || editEmail.Text.Length == 0)
            {
                string title = "kiểm tra nhập thông tin";
                string message = "Vui lòng điền đủ thông tin khách hàng";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else if (!isMatch)
            {
                string title = "Kiểm tra";
                string message = "Vui lòng nhập đúng địa chỉ Email theo @gmail.com";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (editPhone.Text.Replace("-", "").Length != 10)
            {
                string title = "Kiểm tra";
                string message = "Vui lòng nhập đúng số điện thoại đủ 10 số";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if(!_viewModel.getCustomerPhone(editPhone.Text.Replace("-", "")))
                {
                    if (MessageBox.Show("Bạn muốn thêm mới một khách hàng không?",
                    "Thêm sản phẩm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _viewModel._customer.name = editName.Text;
                        _viewModel._customer.phone = editPhone.Text.Replace("-", "");
                        _viewModel._customer.address = editAddress.Text;
                        _viewModel._customer.email = editEmail.Text;

                        var add = _viewModel.AddCustomer(_viewModel._customer);
                        if (add)
                        {
                            string title = "Thêm khách hàng";
                            string message = "Thêm khách hàng thành công";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                            DataContext = new MainViewModel();
                        }
                    }
                }
                else
                {
                    string title = "Thêm khách hàng";
                    string message = "Số điện thoại đã tồn tại";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                    editPhone.Clear();
                }
            }
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            editName.Clear();
            editPhone.Clear();
            editAddress.Clear();
            editEmail.Clear();
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                string digitsOnly = Regex.Replace(textBox.Text, @"\D", "");

                string formattedNumber = Regex.Replace(digitsOnly, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");

                // Update the text in the TextBox
                textBox.Text = formattedNumber;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            var select = Dashboard.menuBTN.Children[1] as MenuButton;
            select?.btn.Focus();
            DataContext = new MainViewModel();
        }
    }
}
