using MyShop_WPF_Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            TongSanPhamDangBan.Text = dashboardViewModel._quantityProductAvailable.ToString();
            TongDonHangTrongTuan.Text = dashboardViewModel._quantityNewPurchaseInWeek.ToString();
            TongDonHangTrongThang.Text = dashboardViewModel._quantityNewPurchaseInMonth.ToString();
            Top5Product.ItemsSource = dashboardViewModel._top5ProductSoldList;

            Global.SaveScreen("Dashboard");
        }

        private void Top5Product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
