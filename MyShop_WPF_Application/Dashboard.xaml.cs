using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Commands;
using MyShop_WPF_Application.UserControls;
using MyShop_WPF_Application.ViewModels;
using MyShop_WPF_Application.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MyShop_WPF_Application
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public static StackPanel menuBTN = null;
        public static StackPanel subMenuBTN = null;
        public Dashboard()
        {
            MainViewModel current = new MainViewModel();
            DataContext = current;
            InitializeComponent();
            
            menuBTN = menu;
            subMenuBTN = subMenu;

            var select = menu.Children[0] as MenuButton;
            //select?.btn.Focus();
            select.btn.Command = current.UpdateViewCommand;
            //select?.btn.Command.Execute("Dashboard");

            string _screen = System.Configuration.ConfigurationManager.AppSettings["Screen"]!;
            if (_screen.Equals("Dashboard"))
            {
                select = menu.Children[0] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("Dashboard");
            } else if (_screen.Equals("QLKH"))
            {
                select = menu.Children[1] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("QLKH");
            }
            else if (_screen.Equals("QLLOAISP"))
            {
                select = menu.Children[2] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("QLLOAISP");
            }
            else if (_screen.Equals("QLSP"))
            {
                select = menu.Children[3] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("QLSP");
            }
            else if (_screen.Equals("QLDH"))
            {
                select = menu.Children[4] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("QLDH");
            }
            else if (_screen.Equals("QLKM"))
            {
                select = menu.Children[5] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("QLKM");
            }
            else if (_screen.Equals("TKDTVLN"))
            {
                select = subMenu.Children[0] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("TKDTVLN");
            }
            else if (_screen.Equals("TKSP"))
            {
                select = subMenu.Children[1] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("TKSP");
            }
            else if (_screen.Equals("TKBH"))
            {
                select = subMenu.Children[2] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("TKBH");
            }
            else
            {
                select = menu.Children[0] as MenuButton;
                select?.btn.Focus();
                select?.btn.Command.Execute("Dashboard");
            }

            for (int i = 0; i < menuBTN.Children.Count; i++)
            {
                if (menu.Children[i] is MenuButton)
                {
                    var select_ = menu.Children[i] as MenuButton;
                    if (select_.btn.IsFocused == true)
                        select_.isActive = true;
                    else
                        select_.isActive = false;
                }
            }

            for (int i = 0; i < subMenuBTN.Children.Count; i++)
            {
                if (subMenuBTN.Children[i] is MenuButton)
                {
                    var select_ = subMenuBTN.Children[i] as MenuButton;
                    if (select_.btn.IsFocused == true)
                        select_.isActive = true;
                    else
                        select_.isActive = false;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1200;
                    this.Height = 750;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;

                }
            }
        }

        private void MenuButton_Loaded(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuButton;
            Debug.WriteLine(item.btn.IsFocused);
        }

    }
}
