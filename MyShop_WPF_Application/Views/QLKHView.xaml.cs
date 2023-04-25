﻿using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
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
    /// Interaction logic for QLKHView.xaml
    /// </summary>
    /// 
    public partial class QLKHView : UserControl
    { 
        int _currentPage = 1, rowsPerPage = 10;
        int _totalPage, _listSize;
        QLKHViewModel _viewModel = new QLKHViewModel();
        public QLKHView()
        {
            InitializeComponent();
            base.DataContext = _viewModel;
        }
            
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            displayRowPerPageTextBox.Text = rowsPerPage.ToString();
            updatePage(1);
        }

        // update the paging system
        // assign new itemsource for listview
        private void updatePage(int page)
        {


            _currentPage = page;
            _listSize = _viewModel.updateCusstomerList().Count;

            if(_viewModel.updateCusstomerList().Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList().Count == 0)
            {
                _currentPage = page - 1;
            }
            lst.ItemsSource = _viewModel.updateCusstomerList().Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
            _totalPage = _listSize / rowsPerPage + ((_listSize % rowsPerPage) == 0 ? 0 : 1);
            pageCountLabel.Content = $"{_currentPage}/{_totalPage}";
        }


        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPage)
            {
                _currentPage++;
                updatePage(_currentPage);
            }
        }

        // event handler
        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                updatePage(_currentPage);
            }
        }

        // event handler
        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var currrentItem = (CustomerModel)button.DataContext;

            if (MessageBox.Show("Bạn có muốn xóa thông tin khách hàng này không?",
               "Xóa",
               MessageBoxButton.YesNo,
               MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var remove = _viewModel.RemoveCustomer(currrentItem.phone);
                if (remove)
                {
                    string message = "Đã xóa thông tin khách hàng thành công";
                    string title = "Xóa";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            updatePage(_currentPage);
        }



        private void addCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            NextPage.Content = new ThemKHView();
            updatePage(_currentPage);
        }

        private void viewDetails_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var customer = (CustomerModel)button.DataContext;

            NextPage.Content = new CTKHView(customer.phone);

            updatePage(_currentPage);
        }


        private void displayRowPerPageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                rowsPerPage = Int16.Parse(displayRowPerPageTextBox.Text);

                updatePage(_currentPage);
            }
            catch { }
        }

        private void searchProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Make sure the input are all numbers
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}