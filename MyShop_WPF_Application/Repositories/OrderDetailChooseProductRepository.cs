using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Repositories
{
    class OrderDetailChooseProductRepository
    {
        // get all products that not in the order (order ID) --> exclude product with this orderID 
        public ObservableCollection<ProductModel> getAllProductNotInOrder(int orderId)
        {
            var sql = "select * " +
                      "from Product " +
                      "where Product_ID not in (select Product_ID" +
                                                " from PurchaseDetail" +
                                                " where Purchase_ID = @orderID)";

            var productList = new ObservableCollection<ProductModel>();
            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("orderID", orderId);

            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                productList.Add(new ProductModel()
                {
                    ProductID = (int)reader["Product_ID"],
                    CategoryID = (int)reader["Category_ID"],
                    ProductName = (string)reader["Product_Name"],
                    ProductAvatar = (string)reader["Avatar"],
                    ProductQuantity = (int)reader["Quantity"],
                    ProductPrice = (double)reader["Price"],
                    ProductPriceOriginal = (double)reader["Price_Original"]
                });
            }
            reader.Close();

            return productList;
        }

        public void addProductToOrderDetail(int orderID, int productID, int quantity)
        {
            var sql = "insert into PurchaseDetail" +
                      " values (@orderID, @productID, @nQuantity)";

            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@orderID", orderID);
            command.Parameters.AddWithValue("productID", productID);
            command.Parameters.AddWithValue("nQuantity", quantity);

            command.ExecuteNonQuery();
        }

        public void updateProductQuantity(int productID, int quantity)
        {
            var sql = "update product set Quantity = @quan where Product_ID = @pID";
            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("quan", quantity);
            command.Parameters.AddWithValue("pID", productID);

            command.ExecuteNonQuery();
        }
    }
}
