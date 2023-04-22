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

<<<<<<< HEAD
=======
        // query to get product quantity by Product_ID from Product table
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
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

<<<<<<< HEAD
=======
        // update product quantity of a selected product in a selected order detail
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
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

<<<<<<< HEAD
=======
        // update product quantity of a selected product in Product table
>>>>>>> a61559e37b16087ac880ed0a5a65019d6b82f201
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

        public CustomerModel getCustomer(string phone)
        {
            var sql = "select * from Customer where Tel = @phoneNum";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@phoneNum", phone);

            var reader = command.ExecuteReader();

            reader.Read();

            CustomerModel model = new CustomerModel()
            {
                name = (string)reader["Customer_Name"],
                phone = (string)reader["Tel"],
                address = (string)reader["Address"],
                email = (string)reader["Email"]
            };

            reader.Close();
            return model;
        }

        public string getCustomerPhone(int orderID)
        {
            var sql = "select Customer_Phone from Purchase where Purchase_ID = @id";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@id", orderID);

            var reader = command.ExecuteReader();

            reader.Read();

            string res = (string)reader["Customer_Phone"];

            reader.Close();

            return res;
        }

        public DateTime getOrderDate(int orderID)
        {
            var sql = "select Centered_At from Purchase where Purchase_ID = @id";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@id", orderID);

            var reader = command.ExecuteReader();

            reader.Read();

            var dtime = (DateTime)reader["Centered_At"];
       

            reader.Close ();

            return dtime;
        }

        public List<Status> getOrderStatusList()
        {
            var sql = "select Display_Text from PurchasesStatusEnum";

            var command = new SqlCommand(sql, Global.Connection);

            var reader = command.ExecuteReader();

            List<Status> res = new List<Status>();
            while (reader.Read())
            {
                res.Add(new Status(){displayText = (string)reader["Display_Text"]});
            }


            reader.Close();

            return res;
        }

        
        public int getOrderStatus(int orderID)
        {
            var sql = "select Status from Purchase where Purchase_ID = @dT";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@dT", orderID);

            var reader = command.ExecuteReader();

            reader.Read();
            int res = (int)reader["Status"];

            reader.Close();

            return res;
        }

        public void updateOrderStatus(int orderID, int newStatus)
        {
            var sql = "update Purchase set Status = @newStatus where Purchase_ID = @orderID";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@orderID", orderID);
            command.Parameters.AddWithValue("@newStatus", newStatus);

            command.ExecuteNonQuery();
        }

        public void updateCustomerInfo(string customerPhone, string newValue, string type)
        {
            var sql = "update Customer set @updateType = @newVal where Tel = @cusPhone";

            // add update field 
            sql = sql.Replace("@updateType", type);

            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@newVal", newValue);
            command.Parameters.AddWithValue("@cusPhone", customerPhone);

            command.ExecuteNonQuery();
        }

        public void updateCreateDate(int orderID, string newDate)
        {
            var sql = "update Purchase set Centered_At = @newDate where Purchase_ID = @orderID";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@orderID", orderID);
            command.Parameters.AddWithValue("@newDate", newDate);

            command.ExecuteNonQuery();
        }

        public List<PromotionModel> getPromotionListFromDB()
        {
            List<PromotionModel> list = new List<PromotionModel>();

            var sql = "select * from Promotion";
            var command = new SqlCommand(sql, Global.Connection);
            var reader = command.ExecuteReader();

            list.Add(new PromotionModel()
            {
                _promotionId = 0,
                _promotionName = "Không giảm giá",
                _promotionPercentage = 0
            });

            while (reader.Read())
            {
                list.Add(new PromotionModel()
                {
                    _promotionId = (int)reader["Promotion_ID"],
                    _promotionName = (string)reader["Promotion_Name"],
                    _promotionPercentage = (double)reader["Promotion_Percentage"]
                });
            }

            reader.Close();

           

            return list;
        } 

        public void updatePromotionInOrderDetails(int orderID, int? newPromotionID)
        {
            var sql = "update Purchase set Promotion_ID = @promo where Purchase_ID = @orderID";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@orderID", orderID);

            if(newPromotionID != null)
                command.Parameters.AddWithValue("@promo", newPromotionID);
            else
                command.Parameters.AddWithValue("@promo", DBNull.Value);

            command.ExecuteNonQuery();
        }

        public int? getPromotionID(int orderID)
        {
            int? res = null;
            var sql = "select Promotion_ID from Purchase where Purchase_ID = @dT";
            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@dT", orderID);

            var reader = command.ExecuteReader();

            reader.Read();


            if (reader["Promotion_ID"] != DBNull.Value)
            {
                res = (int)reader["Promotion_ID"];
            }

            reader.Close();

            return res;
        }

        public void updateTotalInDB(int orderID, double newTotal)
        {
            var sql = "update Purchase set Total = @total where Purchase_ID = @orderID";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@orderID", orderID);

            command.Parameters.AddWithValue("@total", newTotal);

            command.ExecuteNonQuery();
        }
    }
}
