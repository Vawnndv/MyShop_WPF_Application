using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Repositories
{
    class PromotionRepository
    {
        public static PromotionModel getPromotion(int id)
        {
            PromotionModel promotion = null;

            if(Global.Connection == null)
                Global.Connection = new SqlConnection(Global.ConnectionString);

            if(Global.Connection != null)
            {
                var sql = "select * from Promotion where Promotion_ID = @ID";
                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@ID", id);

                var reader = command.ExecuteReader();

                promotion = new PromotionModel() { 
                    _promotionId = (int)reader["Promotion_ID"],
                    _promotionName = (string)reader["Promotion_Name"],
                    _promotionQuantity = (int)reader["Quantity"],
                    _promotionPercentage = (double)reader["Promotion_Percentage"]
                };
            }

            return promotion;
        }
    }
}
