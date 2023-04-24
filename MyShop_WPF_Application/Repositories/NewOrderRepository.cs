using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Repositories
{
    class NewOrderRepository
    {
        public void addNewOrder(OrderModel newOrder)
        {
            var sql = "insert into Purchase (Promotion_ID, Total, Centered_At, Status, Customer_Phone)" +
                " values (@promoID, @total, @date, @statusID, @phone)";

            var command = new SqlCommand(sql, Global.Connection);

            if(newOrder.PromotionID == 0)
            {
                command.Parameters.AddWithValue("@promoID", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@promoID", newOrder.PromotionID);
            }

            command.Parameters.AddWithValue("@total", 0);
            command.Parameters.AddWithValue("@date", newOrder.OrderDate);
            command.Parameters.AddWithValue("@statusID", newOrder.OrderStatus);
            command.Parameters.AddWithValue("@phone", newOrder.CustomerPhone);

            command.ExecuteNonQuery();
        }

        public Boolean addNewCustomer(CustomerModel newCustomer)
        {
            try
            {
                var sql = "insert into Customer (Customer_Name, Tel, Address, Email) values (@name, @tel, @address, @email)";

                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@name", newCustomer.name);
                command.Parameters.AddWithValue("@tel", newCustomer.phone);
                command.Parameters.AddWithValue("@address", newCustomer.address);
                command.Parameters.AddWithValue("@email", newCustomer.email);

                command.ExecuteNonQuery();

                return true;
            }
            catch { return false; }
        }

        public void updateCustomer(CustomerModel newCustomer)
        {
            var sql = "update Customer set Customer_Name = @name, Address = @address, Email = @email where Tel = @tel";

            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@name", newCustomer.name);
            command.Parameters.AddWithValue("@tel", newCustomer.phone);
            command.Parameters.AddWithValue("@address", newCustomer.address);
            command.Parameters.AddWithValue("@email", newCustomer.email);

            command.ExecuteNonQuery();
        }
    }
}
