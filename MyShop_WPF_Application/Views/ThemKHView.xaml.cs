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

            if (editName.Text.Length == 0 || editAddress.Text.Length == 0 || editPhone.Text.Length == 0 || editEmail.Text.Length == 0)
            {
                string message = "Vui lòng điền đủ thông tin khách hàng";
                string title = "kiểm tra nhập thông tin";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
            {
                if(!_viewModel.getCustomerPhone(editPhone.Text))
                {
                    if (MessageBox.Show("Bạn muốn thêm mới một khách hàng không?",
                    "Thêm sản phẩm",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _viewModel._customer.name = editName.Text;
                        _viewModel._customer.phone = editPhone.Text;
                        _viewModel._customer.address = editAddress.Text;
                        _viewModel._customer.email = editEmail.Text;
                        string message1 = editPhone.Text;
                        string title1 = "Thêm khách hàng";
                        MessageBox.Show(message1, title1, MessageBoxButton.OK, MessageBoxImage.Information);

                        var add = _viewModel.AddCustomer(_viewModel._customer);
                        if (add)
                        {
                            string message = "Thêm khách hàng thành công";
                            string title = "Thêm khách hàng";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                            DataContext = new MainViewModel();
                        }
                    }
                }
                else
                {
                    string message = "Số điện thoại đã tồn tại";
                    string title = "Thêm khách hàng";
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
            TextBox textBox = sender as TextBox;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                // Remove all non-digit characters from the text
                string digitsOnly = Regex.Replace(textBox.Text, @"\D", "");

                // Format the text as a phone number
                string formattedNumber = Regex.Replace(digitsOnly, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");

                // Update the text in the TextBox
                textBox.Text = formattedNumber;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }
    }
}
