using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Repositories
{
    class OrderDetailsRepository
    {
        public ObservableCollection<OrderDetailsProductModel> getAllProductOfOrder(int orderId)
        {
            // result list (all products of a specific order)
            ObservableCollection<OrderDetailsProductModel> productResultList = new ObservableCollection<OrderDetailsProductModel>();

            if(Global.Connection !=  null)
            {
                string sql = $"select p.*, pd.Quantity as orderQuantity from Product as p, PurchaseDetail as pd where pd.Purchase_ID = @oId and pd.Product_ID = p.Product_ID";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@oId", orderId);

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    productResultList.Add(new OrderDetailsProductModel()
                    {
                        ProductID = (int)reader["Product_ID"],
                        CategoryID = (int)reader["Category_ID"],
                        ProductName = (string)reader["Product_Name"],
                        ProductAvatar = (string)reader["Avatar"],
                        ProductQuantity = (int)reader["Quantity"],
                        ProductPrice = (double)reader["Price"],
                        orderQuantity = (int)reader["orderQuantity"]
                    });
                }

                reader.Close();
            }


            return productResultList;
        }

        public void removeProductFromOrder(int productId, int orderId)
        {
            if (Global.Connection == null)
            {
                Global.Connection = new SqlConnection(Global.ConnectionString);
            }

            if(Global.Connection != null)
            {
                var sql = $"delete from PurchaseDetail where Purchase_ID = @ID and Product_ID = @pID";
                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@ID", orderId );
                command.Parameters.AddWithValue("@pID", productId);

                command.ExecuteNonQuery();
            }
        }
    }
}
