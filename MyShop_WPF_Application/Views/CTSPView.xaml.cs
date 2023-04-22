using Microsoft.Win32;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
<<<<<<< HEAD
<<<<<<< HEAD
using System.Diagnostics;
=======
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
=======
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
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
    /// <summary>
    /// Interaction logic for CTSPView.xaml
    /// </summary>
<<<<<<< HEAD
<<<<<<< HEAD
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
=======
=======
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
    public partial class CTSPView : UserControl
    {
        FileInfo _selectImage = null;
        CTSPViewModel _viewModel = null;
        private int? productID;


        public CTSPView(int? pID)
        {
            InitializeComponent();

            productID = pID;
            _viewModel = new CTSPViewModel(pID);
            base.DataContext = _viewModel;
<<<<<<< HEAD
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
=======
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
        }

        private void ListBill_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnAddBill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            //_viewModel.CategoryID = int.Parse(editProductCategory.Text);
            _viewModel._product.ProductName = editProductName.Text;
            _viewModel._product.ProductPrice = int.Parse(editProductPrice.Text);
            _viewModel._product.ProductPriceOriginal = int.Parse(editProductPriceOriginal.Text);
            _viewModel._product.ProductQuantity = int.Parse(editProductQuantity.Text);
            _viewModel._product.CategoryID = _currentCategoryCombobox;

            if(_selected)
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

            var edit = _viewModel.EditProduct(_viewModel._product);
            if (edit)
            {

                
=======
=======
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
            _viewModel.CategoryID = int.Parse(editProductCategory.Text);
            _viewModel.ProductName = editProductName.Text;
            _viewModel.ProductPrice = int.Parse(editProductPrice.Text);
            _viewModel.ProductPriceOriginal = int.Parse(editProductPriceOriginal.Text);
            _viewModel.ProductQuantity = int.Parse(editProductQuantity.Text);

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                string newPath = $"{folder}img/{_selectImage.Name}";

                if (!File.Exists(newPath))
                {
                    File.Copy(_selectImage.FullName, newPath);
                }
                _viewModel.ProductAvatar = $"img/{_selectImage.Name}";
            }
            catch (Exception ex)
            {

<<<<<<< HEAD
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
=======
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
            }
        }
    
        private void BtnStatistic_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            var remove = _viewModel.RemoveProduct(_viewModel._product.ProductID);
            if (remove)
            {


            }
=======

>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
=======

>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
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
<<<<<<< HEAD
<<<<<<< HEAD

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
=======
            }
        }
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
=======
            }
        }
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
    }
}
