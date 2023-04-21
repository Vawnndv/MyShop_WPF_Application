using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Repositories
{
    class ProductRepository
    {
        public ObservableCollection<ProductModel> getAllProduct()
        {
            ObservableCollection<ProductModel> result = new ObservableCollection<ProductModel>();
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = $"select * from Product";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int? pId = (int)reader["Product_ID"];
                    int? cId = (int)reader["Category_ID"];
                    string pName = (string)reader["Product_Name"];
                    string pAvatar = (string)reader["Avatar"];
                    int pQuantiry = (int)reader["Quantity"];
                    double pPrice = (double)reader["Price"];
                    double pPriceOriginal = (double)reader["Price_Original"];


                    // add products from DB to collection
                    result.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });
                }

                reader.Close();
            }

            Global.Connection?.Close();
            return result;
        }

        public ObservableCollection<ProductModel> getCategoryProduct(int _CategoryID)
        {
            ObservableCollection<ProductModel> result = new ObservableCollection<ProductModel>();
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = $"select * from Product";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int? pId = (int)reader["Product_ID"];
                    int? cId = (int)reader["Category_ID"];
                    string pName = (string)reader["Product_Name"];
                    string pAvatar = (string)reader["Avatar"];
                    int pQuantiry = (int)reader["Quantity"];
                    double pPrice = (double)reader["Price"];
                    double pPriceOriginal = (double)reader["Price_Original"];


                    // add products from DB to collection
                    result.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });
                }

                reader.Close();
            }

            Global.Connection?.Close();
            return result;
        }

        public int getNumOfProductsAvailable()
        {
            int quantity = 0;
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = "SELECT COUNT(*) AS QUANTITY\r\nFROM Product p\r\nWHERE p.Quantity > 0";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();
                reader.Read();

                quantity = (int)reader["QUANTITY"];

                reader.Close();
            }

            Global.Connection?.Close();
            return quantity;
        }

        public int getNumOfPurchaseSold(DateTime start, DateTime end)
        {
            int quantity = 0;
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = string.Format("SELECT COUNT(*) AS QUANTITY\r\nFROM Purchase p\r\nWHERE p.Centered_At BETWEEN  '{0}' AND '{1}'", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();
                reader.Read();

                quantity = (int)reader["QUANTITY"];

                reader.Close();
            }

            Global.Connection?.Close();
            return quantity;
        }

        public ObservableCollection<ProductModel> getTop5Product()
        {
            ObservableCollection<ProductModel> result = new ObservableCollection<ProductModel>();
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = "SELECT TOP(5) *\r\nFROM Product p\r\nWHERE p.Quantity < 5\r\nORDER BY p.Quantity";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int? pId = (int)reader["Product_ID"];
                    int? cId = (int)reader["Category_ID"];
                    string pName = (string)reader["Product_Name"];
                    string pAvatar = (string)reader["Avatar"];
                    int pQuantiry = (int)reader["Quantity"];
                    double pPrice = (double)reader["Price"];
                    double pPriceOriginal = (double)reader["Price_Original"];


                    // add products from DB to collection
                    result.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });
                }

                reader.Close();
            }

            Global.Connection?.Close();
            return result;
        }
    }
}
