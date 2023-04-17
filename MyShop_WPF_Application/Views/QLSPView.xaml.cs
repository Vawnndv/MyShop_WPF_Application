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
            updateTotalPage();
            updatePage(1);

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

        private void updatePage(int page, string keyword = "")
        {
            _currentPage = page;
            List<ProductModel> _newProductList = new List<ProductModel>();
            List<ProductModel> _newProductItem = new List<ProductModel>();
            if (keyword.Length > 0)
            {
                _newProductList = _viewModel._productList.Where(x => x.ProductName.Contains(keyword)).ToList();
                if (_newProductList.Count > 0)
                {
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();
                }
                else
                {
                    //var dialog = new Dialog() { Message = "Không tìm thấy sản phẩm" };
                    //dialog.Owner = Window.GetWindow(this);
                    //dialog.ShowDialog();
                }

                searchProductInput.Clear();
            }
            else
            {
                if (isFiltering)
                {
                    _newProductList = _viewModel._productList.Where(x => x.ProductPrice >= double.Parse(fromPrice.Text) && x.ProductPrice <= double.Parse(toPrice.Text)).ToList();
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

                }
                else
                {
                    _newProductList = _viewModel._productList.ToList();
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

            updatePage(1, keyword);
            updateTotalPage();

            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }

        private void previosButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                currentPageComboBox.SelectedIndex = _currentPage - 1;
                updatePage(_currentPage);
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPage)
            {
                _currentPage++;
                currentPageComboBox.SelectedIndex = _currentPage - 1;
                updatePage(_currentPage);
            }
        }

        private void currentPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentPageComboBox.SelectedIndex >= 0)
            {
                _currentPage = currentPageComboBox.SelectedIndex + 1;

                updatePage(_currentPage);
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
            updatePage(1);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;
            ProductListView.ItemsSource = _viewModel._productList.Where(x => x.ProductPrice >= double.Parse(fromPriceString) && x.ProductPrice <= double.Parse(toPriceString)).Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
        }

        private void removeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            isFiltering = false;
            fromPrice.Clear();
            toPrice.Clear();    
            updatePage(_currentPage);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;

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
