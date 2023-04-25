using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using MyShop_WPF_Application.Converters;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace MyShop_WPF_Application.Views
{
    public partial class CTSPView : Page
    {
        FileInfo _selectImage = null;
        CTSPViewModel _viewModel = null;
        int _currentCategoryCombobox = 0;
        bool _selected = false;

        public CTSPView(int? productID)
        {
            InitializeComponent();

            //productID = pID;
            _viewModel = new CTSPViewModel(productID);
            base.DataContext = _viewModel._product;

            int i = 0;
            foreach (var category in _viewModel._categoryList)
            {
                if (_viewModel._product.CategoryID == category.CategoryID)
                {
                    comboboxCategory.SelectedIndex = i;
                    _currentCategoryCombobox = i;
                }
                comboboxCategory.Items.Add(category.CategoryName);
                i++;

            }
        }

        private void ListBill_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            editProductName.IsReadOnly = false;
            editProductPrice.IsReadOnly = false;
            editProductPriceOriginal.IsReadOnly = false;
            editProductQuantity.IsReadOnly = false;
            btnAddImageProduct.IsEnabled = true;
            comboboxCategory.IsEnabled = true;
            btnRemoveProduct.IsEnabled = false;

            saveBtn.Visibility = Visibility.Visible;
            restoreBtn.Visibility = Visibility.Visible;
            btnEditProduct.Visibility = Visibility.Hidden;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn hiệu chỉnh lại sản phẩm này không?",
                "Hiệu chỉnh",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (editProductName.Text.Length == 0 || editProductPrice.Text.Length == 0 || editProductPriceOriginal.Text.Length == 0 || editProductQuantity.Text.Length == 0)
                {
                    string message = "Vui lòng điền đủ thông tin";
                    string title = "kiểm tra nhập thông tin sản phẩm";
                    MessageBox.Show(message, title);
                }
                else
                {
                    _viewModel._product.ProductName = editProductName.Text;
                    _viewModel._product.ProductPrice = double.Parse(editProductPrice.Text);
                    _viewModel._product.ProductPriceOriginal = double.Parse(editProductPriceOriginal.Text);
                    _viewModel._product.ProductQuantity = int.Parse(editProductQuantity.Text);
                    foreach (var category in _viewModel._categoryList)
                    {
                        if (category.CategoryName.Equals(comboboxCategory.Items.GetItemAt(_currentCategoryCombobox)))
                        {
                            _viewModel._product.CategoryID = category.CategoryID;
                            break;
                        }

                    }
                    Trace.WriteLine("Số id category là " + _currentCategoryCombobox + 1);

                    if (_selected)
                    {
                        var folder = AppDomain.CurrentDomain.BaseDirectory;
                        try
                        {
                            string newPath = $"{folder}img/phone/{_selectImage.Name}";

                            if (!File.Exists(newPath))
                            {
                                File.Copy(_selectImage.FullName, newPath);
                            }
                            _viewModel._product.ProductAvatar = $"img/phone/{_selectImage.Name}";
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    var edit = _viewModel.EditProduct(_viewModel._product);
                    _viewModel._restoreProduct = _viewModel._product;
                    if (edit)
                    {
                        string message = "Đã cập nhật sản phẩm thành công";
                        string title = "Sửa thông tin sản phẩm";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }

            editProductName.IsReadOnly = true;
            editProductPrice.IsReadOnly = true;
            editProductPriceOriginal.IsReadOnly = true;
            editProductQuantity.IsReadOnly = true;
            btnAddImageProduct.IsEnabled = false;
            comboboxCategory.IsEnabled = false;
            btnRemoveProduct.IsEnabled = true;

            saveBtn.Visibility = Visibility.Hidden;
            restoreBtn.Visibility = Visibility.Hidden;
            btnEditProduct.Visibility = Visibility.Visible;
        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa loại sản phẩm này không?",
                "Xóa loại sản phẩm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var remove = _viewModel.RemoveProduct(_viewModel._product.ProductID);
                if (remove)
                {
                    string message = "Đã xóa sản phẩm thành công";
                    string title = "Xóa thông tin sản phẩm";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        private void restoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn khôi phục lại thông tin sản phẩm này không?",
            "Khôi phục",
             MessageBoxButton.YesNo,
             MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                editProductName.Text = _viewModel._restoreProduct.ProductName;
                editProductPrice.Text = _viewModel._restoreProduct.ProductPrice.ToString();
                editProductPriceOriginal.Text = _viewModel._restoreProduct.ProductPriceOriginal.ToString();
                editProductQuantity.Text = _viewModel._product.ProductQuantity.ToString();
                for (int i = 0; i < _viewModel._categoryList.Count; i++)
                {
                    if (_viewModel._categoryList[i].CategoryID == _viewModel._restoreProduct.CategoryID)
                    {
                        _currentCategoryCombobox = i;
                        comboboxCategory.SelectedIndex = i;
                        break;
                    }
                }


                AbsoluteConverter absoluteConverter = new AbsoluteConverter();

                // Convert the relative path to an absolute path
                string imagePath = _viewModel._restoreProduct.ProductAvatar;
                // Relative path of the image
                string absolutePath = (string)absoluteConverter.Convert(imagePath, typeof(string), null, null);
                // Set the absolute path as the source of the Image control
                editProductAvatar.Source = new BitmapImage(new Uri(absolutePath));
            }
            editProductName.IsReadOnly = true;
            editProductPrice.IsReadOnly = true;
            editProductPriceOriginal.IsReadOnly = true;
            editProductQuantity.IsReadOnly = true;
            btnAddImageProduct.IsEnabled = false;
            comboboxCategory.IsEnabled = false;
            btnRemoveProduct.IsEnabled = true;

            saveBtn.Visibility = Visibility.Hidden;
            restoreBtn.Visibility = Visibility.Hidden;
            btnEditProduct.Visibility = Visibility.Visible;
        }

        private void editAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnAddImageProduct_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                _selectImage = new FileInfo(screen.FileName);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(screen.FileName, UriKind.Absolute);
                bitmap.EndInit();
                editProductAvatar.Source = bitmap;
                _selected = true;
            }
        }

        private void ComboPageIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentCategoryCombobox = comboboxCategory.SelectedIndex;
        }

        /// Hiệu ứng khi chọn
        private void ComboProductTypes_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.LightGray;
        }

        /// Hiệu ứng khi bỏ chọn     
        private void ComboProductTypes_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.Transparent;
        }

        private void BtnAddBill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            if (textBox.Text.Length > 0)
            {
                double value = 0;
                double.TryParse(textBox.Text, out value);
                textBox.Text = value.ToString("N0", CultureInfo.InvariantCulture);
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }
    }
}
