using Microsoft.Win32;
using MyShop_WPF_Application.Converters;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    /// Interaction logic for CTKHView.xaml
    /// </summary>
    public partial class CTKHView : Page
    {
        CTKHViewModel _viewModel;

        public CTKHView(string? tel)
        {
            InitializeComponent();

            _viewModel = new CTKHViewModel(tel);
            base.DataContext = _viewModel._customer;

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (editName.Text.Length == 0 || editPhone.Text.Length == 0 || editEmail.Text.Length == 0 || editAddress.Text.Length == 0)
            {
                string message = "Vui lòng điền đủ thông tin";
                string title = "kiểm tra nhập thông tin";
                MessageBox.Show(message, title);
            }
            else
            {
                if (MessageBox.Show("Bạn muốn hiệu chỉnh lại thông tin khách hàng này không?",
                "Hiệu chỉnh",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    _viewModel._customer.name = editName.Text;
                    _viewModel._customer.phone = editPhone.Text;
                    _viewModel._customer.email = editEmail.Text;
                    _viewModel._customer.address = editAddress.Text;

                    string message1 = _viewModel._customer.phone + " ," + _viewModel._customer.name;
                    string title1 = "Hiệu chỉnh";
                    MessageBox.Show(message1, title1, MessageBoxButton.OK, MessageBoxImage.Information);

                    var edit = _viewModel.EditCustomer(_viewModel._customer);
                    _viewModel._customerRestore = _viewModel._customer;
                    if (edit)
                    {
                        string message = "Đã cập nhật thông tin khách hàng thành công";
                        string title = "Hiệu chỉnh";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                editName.IsReadOnly = true;
                editEmail.IsReadOnly = true;
                editPhone.IsReadOnly = true;
                editAddress.IsReadOnly = true;

                saveBtn.Visibility = Visibility.Hidden;
                restoreBtn.Visibility = Visibility.Hidden;
                editBtn.Visibility = Visibility.Visible;
            }
        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin khách hàng này không?",
                "Xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var remove = _viewModel.RemoveCustomer(_viewModel._customer.phone);
                if (remove)
                {
                    string message = "Đã xóa thông tin khách hàng thành công";
                    string title = "Xóa";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        private void restoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn khôi phục lại thông tin khách hàng này không?",
            "Khôi phục",
             MessageBoxButton.YesNo,
             MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                editName.Text = _viewModel._customerRestore.name;
                editEmail.Text = _viewModel._customerRestore.email;
                editPhone.Text = _viewModel._customerRestore.phone;
                editAddress.Text = _viewModel._customerRestore.address;
            }
            editName.IsReadOnly = true;
            editEmail.IsReadOnly = true;
            editPhone.IsReadOnly = true;
            editAddress.IsReadOnly = true;

            saveBtn.Visibility = Visibility.Hidden;
            restoreBtn.Visibility = Visibility.Hidden;
            editBtn.Visibility = Visibility.Visible;
        }

        private void editAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            editName.IsReadOnly = false;
            editEmail.IsReadOnly = false;
            editPhone.IsReadOnly = true;
            editAddress.IsReadOnly = false;

            saveBtn.Visibility = Visibility.Visible;
            restoreBtn.Visibility = Visibility.Visible;
            editBtn.Visibility = Visibility.Hidden;
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
