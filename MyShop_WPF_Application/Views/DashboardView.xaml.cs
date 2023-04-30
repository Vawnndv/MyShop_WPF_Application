using MyShop_WPF_Application.UserControls;
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
