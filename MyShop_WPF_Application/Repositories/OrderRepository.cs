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
                // query to get user's role
                string sql = $"select * from Purchase";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    // add orders from DB to collection
                    result.Add(new OrderModel()
                    {
                        OrderID = (int)reader["Purchase_ID"],
                        OrderDate = (DateTime)reader["Centered_At"],
                        OrderStatus = (int)reader["Status"],
                        OrderTotal = (Double)reader["Total"],
                        CustomerPhone = (string)reader["Customer_Phone"]
                    });
                }

                reader.Close();
            }

            return result;
        }
    }
}
