using MyShop_WPF_Application.Commands;
using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
    /// Interaction logic for QLSPView.xaml
    /// </summary>
    public partial class QLSPView : UserControl
    {
        QLSPViewModel _viewModel = new QLSPViewModel();

        public QLSPView()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
            base.DataContext = _viewModel;

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    btn.Visibility = Visibility.Collapsed;
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProductListView.ItemsSource = _viewModel._productList;
        }
    }
}
