using Microsoft.Win32;
using MyShop_WPF_Application.Converters;
using MyShop_WPF_Application.UserControls;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ThemSPVIew.xaml
    /// </summary>
    public partial class ThemSPView : Page
    {
        FileInfo _selectImage = null;
        ThemSPViewModel _viewModel;
        int _currentCategoryCombobox = 0;
        bool _selected = false;

        public ThemSPView()
        {
            _viewModel = new ThemSPViewModel();
            InitializeComponent();

            foreach (var category in _viewModel._categoryList)
            {
                addcomboboxCategory.Items.Add(category.CategoryName);
            }
            addcomboboxCategory.SelectedIndex = 0;

            AbsoluteConverter absoluteConverter = new AbsoluteConverter();
            // Convert the relative path to an absolute path
            string imagePath = "img/newProduct.png";
            // Relative path of the image
            string absolutePath = (string)absoluteConverter.Convert(imagePath, typeof(string), null, null);
            // Set the absolute path as the source of the Image control
            editProductAvatar.Source = new BitmapImage(new Uri(absolutePath));
        }

        private void ComboPageIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentCategoryCombobox = addcomboboxCategory.SelectedIndex;
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

        private void BtnRefreshProduct_Click(object sender, RoutedEventArgs e)
        {
            addProductName.Clear();
            addProductPriceOriginal.Clear();
            addProductQuantity.Clear();
            addProductPrice.Clear();
            addcomboboxCategory.SelectedIndex = 0;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thêm mới một sản phẩm không?",
                "Thêm sản phẩm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (addProductName.Text.Length == 0 || addProductPrice.Text.Length == 0 || addProductPriceOriginal.Text.Length == 0 || addProductQuantity.Text.Length == 0 || _selected == false)
                {
                    string message = "Vui lòng điền đủ thông tin";
                    string title = "kiểm tra nhập thông tin sản phẩm";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    _viewModel._product.ProductName = addProductName.Text;
                    _viewModel._product.ProductPrice = int.Parse(addProductPrice.Text);
                    _viewModel._product.ProductPriceOriginal = int.Parse(addProductPriceOriginal.Text);
                    _viewModel._product.ProductQuantity = int.Parse(addProductQuantity.Text);

                    foreach (var category in _viewModel._categoryList)
                    {
                        if (category.CategoryName.Equals(addcomboboxCategory.Items.GetItemAt(_currentCategoryCombobox)))
                        {
                            _viewModel._product.CategoryID = category.CategoryID;
                            break;
                        }

                    }

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

                    var add = _viewModel.AddNewProduct(_viewModel._product);
                    if (add)
                    {
                        string message = "Thêm sản phẩm thành công";
                        string title = "Thêm Sản phẩm";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                        base.DataContext = new MainViewModel();
                    }
                }
            }
        }


        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("123 ko bam dc");
            base.DataContext = new MainViewModel();
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Chuyển định dạng abc,xyz cho giá cả
            if (textBox.Text.Length > 0)
            {
                double value = 0;
                double.TryParse(textBox.Text, out value);
                textBox.Text = value.ToString("N0");
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private void addProductName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
