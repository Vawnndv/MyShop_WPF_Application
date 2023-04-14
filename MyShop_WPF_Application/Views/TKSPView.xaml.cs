using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Models;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TKSPView.xaml
    /// </summary>
    public partial class TKSPView : UserControl
    {
        TKSPViewModel _viewModel = new TKSPViewModel();
        public TKSPView()
        {


            InitializeComponent();

            ObservableCollection<CategoryTypeStatistic> viewModels = new ObservableCollection<CategoryTypeStatistic>();

            viewModels = _viewModel.getAllCategory();
            categoryChart1.ItemsSource = viewModels;
            categoryChart.ItemsSource = viewModels;
            categoryChart2.ItemsSource = viewModels;
        }
    }
}
