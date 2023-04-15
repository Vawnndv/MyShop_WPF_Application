using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application
{
    // this class contains everything value that needed throughout the entire app
    internal class Global
    {
        public static string ConnectionString = "Server = .\\SQLExpress;\r\nDatabase = MyShopDB;\r\nTrusted_Connection=yes;\r\nTrustServerCertificate=True";
        public static int role = -1;
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static SqlConnection? Connection;
    }
}
