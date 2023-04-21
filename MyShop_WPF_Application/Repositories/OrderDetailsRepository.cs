﻿using Microsoft.Data.SqlClient;
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
                        ProductQuantity = (int)reader["orderQuantity"],
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

        public int getProductQuantity(int productId)
        {
            int stockQuantity = 0;
            var sql = $"select Quantity from Product where Product_ID = @id"; // query to get current quantity in stock
            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@id", productId);

            // get quantity from queried
            var reader = command.ExecuteReader();

            reader.Read();
            stockQuantity = (int)reader["Quantity"];

            reader.Close();

            return stockQuantity;
        }

        public void updateProductQuantityInOrderDetail(int orderId, int productId, int quantity)
        {
            if (Global.Connection == null)
            {
                Global.Connection = new SqlConnection(Global.ConnectionString);
            }

            
            if (Global.Connection != null)
            {
                // edit quantity of product in Order
                var sqlUpdateOrderProductQuantity = "update PurchaseDetail set Quantity = @quan where Product_ID = @pId and Purchase_ID = @orderID";
                var command = new SqlCommand(sqlUpdateOrderProductQuantity, Global.Connection);

                command.Parameters.AddWithValue("@quan", quantity);
                command.Parameters.AddWithValue("@pId", productId);
                command.Parameters.AddWithValue("@orderID", orderId);

                command.ExecuteNonQuery();
            }
        }

        public void updateStockProductQuantity(int productId, int newQuantity) {
            if (Global.Connection == null)
            {
                Global.Connection = new SqlConnection(Global.ConnectionString);
            }

            // current in stock suitable for input quantity
            // update stock quantity
            var sqlUpdateStockQuantity = "update Product set Quantity = @stockQuantity where Product_ID = @pId";
            var command = new SqlCommand(sqlUpdateStockQuantity, Global.Connection);

            command.Parameters.AddWithValue("@stockQuantity", newQuantity);
            command.Parameters.AddWithValue("@pId", productId);

            command.ExecuteNonQuery();
        }

    }
}
