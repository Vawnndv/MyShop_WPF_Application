using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
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

namespace MyShop_WPF_Application.WindowScreen
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        SqlConnection? conn;

        public Login()
        {

            InitializeComponent();
            string username = System.Configuration.ConfigurationManager.AppSettings["Username"]!;
            string passwordIn64 = System.Configuration.ConfigurationManager.AppSettings["Password"]!;
            string entropyIn64 = System.Configuration.ConfigurationManager.AppSettings["Entropy"]!;

            if (passwordIn64.Length != 0)
            {
                byte[] entropyInBytes = Convert.FromBase64String(entropyIn64);
                byte[] cypherTextInBytes = Convert.FromBase64String(passwordIn64);

                byte[] passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                    entropyInBytes,
                    DataProtectionScope.CurrentUser
                );

                string password = Encoding.UTF8.GetString(passwordInBytes);

                userNameTextBox.Text = username;
                passwordTextBox.Password = password;
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = userNameTextBox.Text;
            string password = passwordTextBox.Password;

            // user secret
            var secret = new ConfigurationBuilder().AddUserSecrets<Login>().Build();
            string connectionString = secret.GetSection("DB")["ConnectionString"]!;

            // assign var to connection string
            connectionString = connectionString.Replace("@Server", secret.GetSection("DB")["Server"]!);
            connectionString = connectionString.Replace("@Database", secret.GetSection("DB")["Database"]!);
            connectionString = connectionString.Replace("@Username", username);
            connectionString = connectionString.Replace("@Password", password);

            progressBar.Visibility = Visibility.Visible;
            loadCanvas.Visibility = Visibility.Visible;
            progressBar.IsIndeterminate = true;

            conn = await Task.Run(() =>
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    System.Threading.Thread.Sleep(2000);

                }
                catch (Exception ex)
                {
                    return null;
                }

                return connection;
            });

            progressBar.IsIndeterminate = false;
            loadCanvas.Visibility = Visibility.Hidden;

            if (conn == null)
            {
                MessageBox.Show("Cannot connect to database");
            }
            else
            {
                MessageBox.Show("Database is ready!");
                if (rememberCheckBox.IsChecked == true)
                {
                    var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(
                        ConfigurationUserLevel.None);
                    config.AppSettings.Settings["Username"].Value = "sale01";


                    var passwordInBytes = Encoding.UTF8.GetBytes(password);
                    var entropy = new byte[20];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(entropy);
                    }

                    var cypherText = ProtectedData.Protect(
                        passwordInBytes,
                        entropy,
                        DataProtectionScope.CurrentUser
                    );

                    string passwordIn64 = Convert.ToBase64String(cypherText);
                    string entropyIn64 = Convert.ToBase64String(entropy);
                    config.AppSettings.Settings["Password"].Value = passwordIn64;
                    config.AppSettings.Settings["Entropy"].Value = entropyIn64;

                    config.Save(ConfigurationSaveMode.Full);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
            }

            progressBar.Visibility = Visibility.Hidden;

            

        }

       
    }
}
