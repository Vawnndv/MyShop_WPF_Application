using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Repositories;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for QLDHView.xaml
    /// </summary>
    public partial class QLDHView : Window
    {
        QLDHViewModel _viewModel = new QLDHViewModel();
        

        public  QLDHView()
        {
            InitializeComponent();
            base.DataContext = _viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            int listSize = _viewModel._orderList.Count;
            for(int i = 0; i < listSize; i++)
            {
                lst.Items.Add(new
                {
                    ID = _viewModel._orderList.ElementAt(i).OrderID.ToString(),
                    status = _viewModel._orderList.ElementAt(i).OrderStatus.ToString(),
                    createDate = _viewModel._orderList.ElementAt(i).OrderDate.ToString(),
                    phoneNumber = _viewModel._orderList.ElementAt(i).CustomerPhone,
                    totalValue = _viewModel._orderList.ElementAt(i).OrderTotal.ToString(),
                });
            }
            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            int i = lst.SelectedIndex;

            // No select, click button
            if (i == -1)
                return;

            MessageBox.Show(i.ToString());

            _viewModel.removeOrder(i);
            lst.Items.RemoveAt(i);
            lst.Items.Refresh();
        
        }

    }
}

