using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static MyShop_WPF_Application.Views.SanPhamBanChay;

namespace MyShop_WPF_Application.Views
{
    /// <summary>
    /// Interaction logic for SanPhamBanChay.xaml
    /// </summary>
    public partial class SanPhamBanChay : UserControl
    {
        internal class ProductStatistic
        {
            public String? name { get; set; }
            public int? quantity { get; set; }
        }

        ObservableCollection<ProductStatistic> productStatistic = new ObservableCollection<ProductStatistic>();
        public SanPhamBanChay()
        {
            InitializeComponent();
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {
                string sql = $"SELECT TOP(5) p.Product_Name, SUM(pd.Quantity) AS Quantity\r\nFROM Product p LEFT JOIN PurchaseDetail pd ON p.Product_ID = pd.Product_ID\r\n\tLEFT JOIN Purchase pur on pur.Purchase_ID = pd.Purchase_ID\r\nWHERE pur.Centered_At BETWEEN '{DateTime.Now.ToString("yyyy-MM-dd")}' AND '2024'\r\nGROUP BY p.Product_Name\r\nORDER BY Quantity DESC\r\n";
                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    String? pName = (String)reader["Product_Name"];
                    int? pQuantity = (int)reader["Quantity"];

                    // add products from DB to collection
                    productStatistic.Add(new ProductStatistic()
                        {
                            name = pName,
                            quantity = pQuantity
                        });
                }

                reader.Close();
            }
            Global.Connection?.Close();

            categoryPieChart.ItemsSource = productStatistic;
        }
    }
}
