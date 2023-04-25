using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.WindowScreen;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MyShop_WPF_Application.Views;

namespace MyShop_WPF_Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            string _screen = System.Configuration.ConfigurationManager.AppSettings["Screen"]!;
            if (_screen == null || _screen.Equals("Login"))
            {
                new Login().Show();
            }
            else {
                try
                {
                    var connection = new SqlConnection(Global.ConnectionString);
                    connection.Open();
                    System.Threading.Thread.Sleep(2000);
                    Global.Connection = connection;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                Debug.WriteLine(_screen);
                var screen = new Dashboard();
                screen.Show();
            }
        }
    }
}
