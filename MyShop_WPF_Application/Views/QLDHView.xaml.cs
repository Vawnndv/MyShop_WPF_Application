﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Repositories;
using MyShop_WPF_Application.UserControls;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for QLDHView.xaml
    /// </summary>
    public partial class QLDHView : UserControl
    {
        QLDHViewModel _viewModel = new QLDHViewModel();
        int _currentPage = 1, rowsPerPage = 10;
        int _totalPage, _listSize;
        bool isFiltering = false;
        DateTime fromDate, toDate;

        public QLDHView()
        {
            InitializeComponent();
            base.DataContext = _viewModel;
            for (int i = 0; i < Dashboard.menuBTN.Children.Count; i++)
            {
                if (Dashboard.menuBTN.Children[i] is MenuButton)
                {
                    var select = Dashboard.menuBTN.Children[i] as MenuButton;
                    if (select.btn.IsFocused == true)
                        select.isActive = true;
                    else
                        select.isActive = false;
                }
            }

            for (int i = 0; i < Dashboard.subMenuBTN.Children.Count; i++)
            {
                if (Dashboard.subMenuBTN.Children[i] is MenuButton)
                {
                    var select_ = Dashboard.subMenuBTN.Children[i] as MenuButton;
                    if (select_.btn.IsFocused == true)
                        select_.isActive = true;
                    else
                        select_.isActive = false;
                }
            }

            Global.SaveScreen("QLDH");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // set blackout date from to day --> next day available in the next day
            // according to NOW system date
            fromDatePicker.DisplayDateEnd = DateTime.Now;
            toDatePicker.DisplayDateEnd= DateTime.Now;

            displayRowPerPageTextBox.Text = rowsPerPage.ToString();
            updatePage(1);
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
            var select = Dashboard.menuBTN.Children[2] as MenuButton;
            select?.btn.Focus();

            Button button = (Button)sender;
            var currrentItem = (OrderModel)button.DataContext;

            _viewModel.removeOrder(currrentItem.OrderID);

            lst.ClearValue(ItemsControl.ItemsSourceProperty);

            updatePage(_currentPage);
        }

        // event handler
        private void displayRowPerPageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                rowsPerPage = Int16.Parse(displayRowPerPageTextBox.Text);

                updatePage(_currentPage);
            }
            catch { }
        }

        
        private void removeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            isFiltering = false;
            updatePage(_currentPage);
        }

        // filter orders by date button
        private void filterDateButton_Click(object sender, RoutedEventArgs e)
        {
            string fromDateString = fromDatePicker.Text;
            string toDateString = toDatePicker.Text;

            // check if string is null
            if (fromDateString == null || fromDateString == "" || toDateString == null || toDateString == "")
            {
                isFiltering = false;
                return;
            }

            fromDate = DateTime.Parse(fromDateString + " 12:00");
            toDate = DateTime.Parse(toDateString + " 12:00");


            //MessageBox.Show($"{fromDate.ToString()}\n{toDate.ToString()}");

            isFiltering = true;

            updatePage(1);
            lst.ItemsSource = _viewModel._orderList.Where(x => x.OrderDate >= fromDate.Date && x.OrderDate <= toDate.Date).Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
        }

        private void viewDetails_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var order = (OrderModel)button.DataContext;

            Global.selectedOrderID = order.OrderID;
            screen.Content = new OrderDetailsView();
            //selectedOrderDetails.ShowDialog();

            //_viewModel = new QLDHViewModel();

            //updatePage(_currentPage);
        }

        private void addNewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            screen.Content = new AddNewOrderView();
        }

     
        // update the paging system
        // assign new itemsource for listview
        private void updatePage(int page)
        {
            _currentPage = page;

            if (_currentPage == 1)
            {
                prevButton.IsEnabled = false;
                nextButton.IsEnabled = true;
            }

            else if (_currentPage == _totalPage)
            {
                prevButton.IsEnabled = true;
                nextButton.IsEnabled = false;
            }

            else
            {
                prevButton.IsEnabled = true;
                nextButton.IsEnabled = true;
            }

            if (isFiltering)
            {
                _listSize = _viewModel._orderList.Where(x => x.OrderDate >= fromDate.Date && x.OrderDate <= toDate.Date).ToList().Count;
                lst.ItemsSource = _viewModel._orderList.Where(x => x.OrderDate >= fromDate.Date && x.OrderDate <= toDate.Date).Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
            }
            else
            {
                _listSize = _viewModel._orderList.Count;
                lst.ItemsSource = _viewModel._orderList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
            }

            _totalPage = _listSize / rowsPerPage + ((_listSize % rowsPerPage) == 0 ? 0 : 1);
            pageCountLabel.Content = $"{_currentPage}/{_totalPage}";
        }

        // Make sure the input are all numbers
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

