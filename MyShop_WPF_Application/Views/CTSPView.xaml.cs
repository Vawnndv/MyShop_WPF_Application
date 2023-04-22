using Microsoft.Win32;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            foreach (var category in _viewModel._categoryList)
            {
                comboboxCategory.Items.Add(category.CategoryName);
            }
            comboboxCategory.SelectedIndex = (int)(_viewModel._product.CategoryID - 1);
            _currentCategoryCombobox = (int)_viewModel._product.CategoryID - 1;
        }

        private void ListBill_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            //_viewModel.CategoryID = int.Parse(editProductCategory.Text);
            _viewModel._product.ProductName = editProductName.Text;
            _viewModel._product.ProductPrice = double.Parse(editProductPrice.Text);
            _viewModel._product.ProductPriceOriginal = double.Parse(editProductPriceOriginal.Text);
            _viewModel._product.ProductQuantity = int.Parse(editProductQuantity.Text);
            _viewModel._product.CategoryID = _currentCategoryCombobox;

            if (_selected)
            {
                var folder = AppDomain.CurrentDomain.BaseDirectory;
                try
                {
                    string newPath = $"{folder}img/{_selectImage.Name}";

                    if (!File.Exists(newPath))
                    {
                        File.Copy(_selectImage.FullName, newPath);
                    }
                    _viewModel._product.ProductAvatar = $"img/{_selectImage.Name}";
                }
                catch (Exception ex)
                {

                }
            }

        }

        private void BtnStatistic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            var remove = _viewModel.RemoveProduct(_viewModel._product.ProductID);
            if (remove)
            {


            }
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
            _currentCategoryCombobox = comboboxCategory.SelectedIndex + 1;
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
    }
}
