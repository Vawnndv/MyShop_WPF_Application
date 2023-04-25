using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using MyShop_WPF_Application.Commands;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

            Global.SaveScreen("QLSP");
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    btn.Visibility = Visibility.Collapsed;
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboboxCategory.Items.Add("Tất cả");
            displayRowPerPageTextBox.Text = rowsPerPage.ToString();

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

        private void updatePage(int category, int page, string keyword = "")
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

            if (category == 0)
            {
                _productListCategory = _viewModel._productList.ToList();
            }
            else
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
            else
            {
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
            int index = -1;
            if (_currentPage > 0)
            {
                index = (_currentPage - 1) * rowsPerPage + ProductListView.SelectedIndex;
            }
            else
            {
                index = ProductListView.SelectedIndex;

            }

            //Trace.WriteLine("san pham thu " + index + ", " + _viewModel._productList[index].ProductID);

            nextPage.Content = new CTSPView(_viewModel._productList[index].ProductID);
        }

        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            nextPage.Content = new ThemSPView();
        }

        private void importProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại mở tập tin
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;
                // Do something with the selected file path

                try
                {
                    var document = SpreadsheetDocument.Open(filename, false);
                    var wbPart = document.WorkbookPart!;
                    var sheets = wbPart.Workbook.Descendants<Sheet>()!;
                    var sheet = sheets.FirstOrDefault(
                        s => s.Name == "Product");
                    var wsPart = (WorksheetPart)(wbPart!.GetPartById(sheet.Id!));
                    var cells = wsPart.Worksheet.Descendants<Cell>();

                    int row = 2;
                    Cell idCategoryCell;
                    int countAdd = 0;

                    do
                    {
                        idCategoryCell = cells.FirstOrDefault(
                            c => c?.CellReference == $"A{row}"
                        )!;

                        if (idCategoryCell?.InnerText.Length > 0)
                        {
                            string id = idCategoryCell.InnerText;

                            //ProductName
                            Cell nameCell = cells.FirstOrDefault(
                                c => c?.CellReference == $"B{row}"
                            )!;
                            string stringId = nameCell!.InnerText;
                            var stringTable = wbPart
                            .GetPartsOfType<SharedStringTablePart>()
                                 .FirstOrDefault()!;
                            string name = stringTable.SharedStringTable
                                 .ElementAt(int.Parse(stringId)).
                            InnerText;

                            //Avatar
                            Cell avatarCell = cells.FirstOrDefault(
                                c => c?.CellReference == $"C{row}"
                            )!;
                            string stringIdAvatar = avatarCell!.InnerText;
                            var stringTableAvatar = wbPart
                            .GetPartsOfType<SharedStringTablePart>()
                                 .FirstOrDefault()!;
                            string avatar = stringTableAvatar.SharedStringTable
                                 .ElementAt(int.Parse(stringIdAvatar)).
                            InnerText;

                            //Quantiny
                            Cell quantinyCell = cells.FirstOrDefault(
                                c => c?.CellReference == $"D{row}"
                            )!;
                            string quantiny = quantinyCell!.InnerText;

                            //Price
                            Cell priceCell = cells.FirstOrDefault(
                               c => c?.CellReference == $"E{row}"
                           )!;
                            string price = priceCell!.InnerText;

                            //Price_Orginal
                            Cell priceOrginalCell = cells.FirstOrDefault(
                                    c => c?.CellReference == $"F{row}"
                                )!;
                            string priceOrginal = priceCell!.InnerText;

                            bool productExists = _viewModel._productList
                                .Where(p => p.CategoryID != null && p.ProductName != null && p.ProductAvatar != null && p.ProductQuantity != null && p.ProductPrice != null && p.ProductPriceOriginal != null) // Filter products based on criteria
                                .Any(p => p.CategoryID == int.Parse(id) && p.ProductName == name && p.ProductAvatar == avatar && p.ProductQuantity == int.Parse(quantiny) && p.ProductPrice == double.Parse(price) && p.ProductPriceOriginal == double.Parse(priceOrginal)); // Check if desired product exists in filtered array

                            if (!productExists)
                            {
                                _viewModel._product.CategoryID = int.Parse(id);
                                _viewModel._product.ProductName = name;
                                _viewModel._product.ProductAvatar = avatar;
                                _viewModel._product.ProductQuantity = int.Parse(quantiny);
                                _viewModel._product.ProductPrice = double.Parse(price);
                                _viewModel._product.ProductPriceOriginal = double.Parse(priceOrginal);

                                var add = _viewModel.AddNewProduct(_viewModel._product);
                                if (add)
                                {
                                    countAdd++;
                                }
                            }
                            Trace.WriteLine($"{id} - {name} - {avatar} - {quantiny} - {price} - {priceOrginal}");
                        }
                        row++;

                    } while (idCategoryCell?.InnerText.Length > 0);

                    Console.ReadLine();
                    ProductListView.ItemsSource = _viewModel.getProductList();

                    if (countAdd > 0)
                    {
                        string title = "Import category từ Excel" + countAdd;
                        string message = "Đã thêm thành công " + countAdd + " loại sản phẩm mới";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string title = "Import category từ Excel";
                        string message = "Không có dữ liệu hoặc dữ liệu đã tồn tại trong cơ sở dữ liệu";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    string title = "Import category từ Excel";
                    string message = "Import không thành công";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            updatePage(_currentCategoryCombobox, _currentPage);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }

        private void nextPage_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void displayRowPerPageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if (Int16.Parse(displayRowPerPageTextBox.Text) > rowsPerPage)
                //{
                //    displayRowPerPageTextBox.Text = rowsPerPage.ToString();
                //}
                //else
                //{
                rowsPerPage = Int16.Parse(displayRowPerPageTextBox.Text);
                //}
                updatePage(_currentCategoryCombobox, _currentPage);
                updateTotalPage();
                currentPageComboBox.SelectedIndex = _currentPage - 1;

            }
            catch { }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
