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
                    Double pPrice = (Double)reader["Price"];
                    Double pPriceOriginal = (Double)reader["Price_Original"];


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
