using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Repositories
{
    class OrderRepository
    {
        public ObservableCollection<OrderModel> getAllOrder()
        {
            ObservableCollection<OrderModel> result = new ObservableCollection<OrderModel>();
            if(Global.Connection != null)
            {
                List<string> statusTypeList = getStatusDisplayTextStringFromDB();

                // query to get user's role
                string sql = $"select * from Purchase";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    int? pId ;

                    // check null promotion code
                    if (reader["Promotion_ID"] == DBNull.Value)
                        pId = null;
                    else
                        pId = (int)reader["Promotion_ID"];

                    // add orders from DB to collection
                    result.Add(new OrderModel()
                    {
                        PromotionID = pId,
                        OrderID = (int)reader["Purchase_ID"],
                        OrderDate = (DateTime)reader["Centered_At"],
                        OrderStatus = (int)reader["Status"],
                        OrderTotal = (Double)reader["Total"],
                        CustomerPhone = (string)reader["Customer_Phone"],
                        OrderStatusDisplayText = statusTypeList.ElementAt((int)reader["Status"] - 1).ToString()
                    });
                }

                reader.Close();
            }

            return result;
        }

        public Boolean deleteOrderId(int id)
        {
            if (Global.Connection != null)
            {
                // delete order details first to avoid FK conflict
                deleteOrderDetails(id);
                
                // query to get user's role
                string sql = $"delete from Purchase where Purchase_ID = @ID";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();

                return true;
            }
            return false;
        }

        private void deleteOrderDetails(int id)
        {
            string sql = $"delete from PurchaseDetail where Purchase_ID = @ID";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@ID", id);
            command.ExecuteNonQuery();
        }

        // query and get status display text from SQL
        private List<string> getStatusDisplayTextStringFromDB()
        {
            string sql = $"select Display_Text from PurchasesStatusEnum";
            List<string> result = new List<string>();
            var command = new SqlCommand(sql, Global.Connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add((string)reader["Display_Text"]);
            }

            reader.Close();


            return result;
        }
    }
}
