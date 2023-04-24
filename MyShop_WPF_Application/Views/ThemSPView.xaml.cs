﻿using Microsoft.Win32;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
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
        ThemSPViewModel _viewModel = null;
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
            addcomboboxCategory.SelectedIndex = 0;
            addProductQuantity.Clear();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thêm mới một sản phẩm không?",
                "Thêm sản phẩm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (addProductName.Text.Length == 0 || addProductPrice.Text.Length == 0 || addProductPriceOriginal.Text.Length == 0 || addProductQuantity.Text.Length == 0)
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

                    }
                }
            }
        }

        private void addQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void addProductName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
