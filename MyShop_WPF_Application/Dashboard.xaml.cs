﻿using MaterialDesignThemes.Wpf;
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
        public Dashboard()
        {
            MainViewModel current = new MainViewModel();
            DataContext = current;
            InitializeComponent();
            
            menuBTN = menu;

            var select = menu.Children[0] as MenuButton;
            select?.btn.Focus();
            select.btn.Command = current.UpdateViewCommand;
            select?.btn.Command.Execute("Dashboard");
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
