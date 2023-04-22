using MyShop_WPF_Application.Commands;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for QLSPView.xaml
    /// </summary>
    public partial class QLSPView : UserControl
    {
        QLSPViewModel _viewModel = new QLSPViewModel();
        int _currentPage = 1, rowsPerPage = 12;
        int _currentCategoryCombobox = 0;

        int _totalPage = 0;
        int _listSize;
        int _productCount = 0;
        bool isFiltering = false;
        
        public QLSPView()
        {
            InitializeComponent();
            base.DataContext = _viewModel;

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    btn.Visibility = Visibility.Collapsed;
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboboxCategory.Items.Add("Tất cả");
            foreach (var category in _viewModel._categoryList)
            {
                comboboxCategory.Items.Add(category.CategoryName);
            }
            updateTotalPage();
            updatePage(_currentCategoryCombobox, 1);

            comboboxCategory.SelectedIndex = 0;
            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }

        private void updateTotalPage()
        {
            _totalPage = _listSize / rowsPerPage + ((_listSize % rowsPerPage) == 0 ? 0 : 1);
            var lines = new List<Tuple<int, int>>();
            for (int i = 1; i <= _totalPage; i++)
            {
                lines.Add(new Tuple<int, int>(i, _totalPage));
            }
            currentPageComboBox.ItemsSource = lines;

        }

        private void updatePage(int category, int page, string keyword = "", int index = -1)
        {
            _currentPage = page;

            if (_currentPage == 1)
            {
                previousButton.IsEnabled = false;
                nextButton.IsEnabled = true;
            }

            else if (_currentPage == _totalPage)
            {
                previousButton.IsEnabled = true;
                nextButton.IsEnabled = false;
            }

            else
            {
                previousButton.IsEnabled = true;
                nextButton.IsEnabled = true;
            }

            List<ProductModel> _productListCategory = new List<ProductModel>(); 
            List<ProductModel> _newProductList = new List<ProductModel>();
            List<ProductModel> _newProductItem = new List<ProductModel>();

            if(category == 0)
            {
                _productListCategory = _viewModel._productList.ToList();
            } else
            {
                _productListCategory = _viewModel._productList.Where(x => x.CategoryID == category).ToList();

            }
            if (keyword.Length > 0)
            {
                _newProductList = _productListCategory.Where(x => x.ProductName.ToLower().Contains(keyword.ToLower())).ToList();
                if (_newProductList.Count > 0)
                {
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _newProductList = _productListCategory.ToList();
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();
                }

                searchProductInput.Clear();
            }
            else { 
                if (isFiltering)
                {
                    _newProductList = _productListCategory.Where(x => x.ProductPrice >= double.Parse(fromPrice.Text) && x.ProductPrice <= double.Parse(toPrice.Text)).ToList();
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

                }
                else
                {
                    _newProductList = _productListCategory.ToList();
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();
                    
                }
            }


            ProductListView.ItemsSource = _newProductItem;
            _listSize = _newProductList.Count;
            _productCount = _newProductItem.Count;
            infoTextBlock.Text = $"Đang hiển thị {_productCount} / {_listSize} sản phẩm";
        }

        private void searchProductButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = searchProductInput.Text;

            updatePage(_currentCategoryCombobox, 1, keyword);
            updateTotalPage();

            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }

        private void previosButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                currentPageComboBox.SelectedIndex = _currentPage - 1;
                updatePage(_currentCategoryCombobox, _currentPage);
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPage)
            {
                _currentPage++;
                currentPageComboBox.SelectedIndex = _currentPage - 1;
                updatePage(_currentCategoryCombobox, _currentPage);
            }
        }

        private void currentPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentPageComboBox.SelectedIndex >= 0)
            {
                _currentPage = currentPageComboBox.SelectedIndex + 1;

                updatePage(_currentCategoryCombobox, _currentPage);
            }
        }

        private void filterPriceButton_Click(object sender, RoutedEventArgs e)
        {
            string fromPriceString = fromPrice.Text;
            string toPriceString = toPrice.Text;

            // check if string is null
            if (fromPriceString == null || fromPriceString == "" || toPriceString == null || toPriceString == "")
            {
                isFiltering = false;
                return;
            }


            isFiltering = true;
            updatePage(_currentCategoryCombobox, 1);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;
            //ProductListView.ItemsSource = _viewModel._productList.Where(x => x.ProductPrice >= double.Parse(fromPriceString) && x.ProductPrice <= double.Parse(toPriceString)).Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
        }

        private void removeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            isFiltering = false;
            fromPrice.Clear();
            toPrice.Clear();    
            updatePage(_currentCategoryCombobox, _currentPage);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;

        }

        private void ComboPageIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentCategoryCombobox = comboboxCategory.SelectedIndex;
            updatePage(_currentCategoryCombobox, _currentPage);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;
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

        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            var index = ProductListView.SelectedIndex;
            nextPage.Content = new CTSPView(_viewModel._productList[index].ProductID);
            //MessageBox.Show(_viewModel._productList[index].ProductName);

        }

        private void nextPage_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            var index = ProductListView.SelectedIndex;
            nextPage.Content = new ThemSPView();
        }

        private void importProductsButton_Click(object sender, RoutedEventArgs e)
        {
=======
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
=======
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201

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


    }
}
