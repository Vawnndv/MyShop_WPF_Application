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
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Windows.Controls;

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
                    string username = System.Configuration.ConfigurationManager.AppSettings["Username"]!;
                    string passwordIn64 = System.Configuration.ConfigurationManager.AppSettings["Password"]!;
                    string entropyIn64 = System.Configuration.ConfigurationManager.AppSettings["Entropy"]!;
                    string password = "";
                    // decrypt password from app.config
                    if (passwordIn64.Length != 0)
                    {
                        byte[] entropyInBytes = Convert.FromBase64String(entropyIn64);
                        byte[] cypherTextInBytes = Convert.FromBase64String(passwordIn64);

                        byte[] passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                            entropyInBytes,
                            DataProtectionScope.CurrentUser
                        );

                        password = Encoding.UTF8.GetString(passwordInBytes);

                    }

                    // user secret
                    var secret = new ConfigurationBuilder().AddUserSecrets<Login>().Build();
                    string connectionString = secret.GetSection("DB")["ConnectionString"]!;

                    // assign var to connection string
                    connectionString = connectionString.Replace("@Server", secret.GetSection("DB")["Server"]!);
                    connectionString = connectionString.Replace("@Database", secret.GetSection("DB")["Database"]!);
                    connectionString = connectionString.Replace("@Username", username);
                    connectionString = connectionString.Replace("@Password", password);

                    // assign to attribute of global class
                    Global.ConnectionString = connectionString;
                    var connection = new SqlConnection(Global.ConnectionString);
                    connection.Open();
                    System.Threading.Thread.Sleep(2000);
                    Global.Connection = connection;

                    if (Global.Connection == null)
                    {
                        throw new Exception("Can't connect to DB");
                    }

                    var screen = new Dashboard();
                    screen.Show();
                }
                catch (Exception ex)
                {
                    new Login().Show();
                }
            }
        }
    }
}
