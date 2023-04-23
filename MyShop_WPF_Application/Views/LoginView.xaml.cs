using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyShop_WPF_Application.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public Login()
        {

            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //
            string username = System.Configuration.ConfigurationManager.AppSettings["Username"]!;
            string passwordIn64 = System.Configuration.ConfigurationManager.AppSettings["Password"]!;
            string entropyIn64 = System.Configuration.ConfigurationManager.AppSettings["Entropy"]!;

            // decrypt password from app.config
            if (passwordIn64.Length != 0)
            {
                byte[] entropyInBytes = Convert.FromBase64String(entropyIn64);
                byte[] cypherTextInBytes = Convert.FromBase64String(passwordIn64);

                byte[] passwordInBytes = ProtectedData.Unprotect(cypherTextInBytes,
                    entropyInBytes,
                    DataProtectionScope.CurrentUser
                );

                string password = Encoding.UTF8.GetString(passwordInBytes);

                // assign to text box
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

            // assign to attribute of global class
            Global.ConnectionString = connectionString;

            // set visibility for progress bar
            progressBar.Visibility = Visibility.Visible;
            loadCanvas.Visibility = Visibility.Visible;
            progressBar.IsIndeterminate = true;

            // try connect to DB
            Global.Connection = await Task.Run(() =>
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    System.Threading.Thread.Sleep(2000);

                }
                catch 
                {
                    return null;
                }

                return connection;
            });

            progressBar.IsIndeterminate = false;
            loadCanvas.Visibility = Visibility.Hidden;

            if (Global.Connection == null)
            {
                MessageBox.Show("Login Unsuccessful");
            }
            else // connect to DB successful
            {
                MessageBox.Show("Login Successful");

                // save username, password to App.config
                if (rememberCheckBox.IsChecked == true)
                {
                    AppKeyEnum.saveToConfig(password, username);
                }

                // query to get user's role
                string sql = $"select Rolename from Account where Username = @username";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;

                var reader = command.ExecuteReader();
                reader.Read();

                string roleStr = (string)reader["Rolename"];

                // check to assign static role var
                if (roleStr == "Admin")
                    Global.role = 0;
                else if (roleStr == "Saler")
                    Global.role = 1;

                reader.Close();


                // load dashboard
                Window win2 = new Dashboard();

                win2.Show();
                this.Close();
            }

            progressBar.Visibility = Visibility.Hidden;

            
        }

       
    }
}
