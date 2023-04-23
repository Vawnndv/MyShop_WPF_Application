using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MyShop_WPF_Application.Repositories
{
    class PromotionRepository
    {
        public ObservableCollection<PromotionModel> getAllPromotion()
        {
            ObservableCollection<PromotionModel > promoLst = new ObservableCollection<PromotionModel>();

            if (Global.Connection == null)
                Global.Connection = new SqlConnection(Global.ConnectionString);

            if (Global.Connection != null)
            {
                var sql = "select * from Promotion";
                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    promoLst.Add( new PromotionModel()
                    {
                        _promotionId = (int)reader["Promotion_ID"],
                        _promotionName = (string)reader["Promotion_Name"],
                        _promotionQuantity = (int)reader["Quantity"],
                        _promotionPercentage = (double)reader["Promotion_Percentage"]
                    });
                }

                reader.Close();
            }

            return promoLst;
        }

        public void deletePromotionFromPromotionTable(int promoID, double promoPercentage)
        {
            // re-calculate the total of each order that using this promo ID
            reCalculateTotalOfPurchase(promoID, promoPercentage);

            // assign each order using this promoID with NULL -> no sale
            assignNullPromotionIdInPurchase(promoID);

            // delete promo ID completely from the Promotion table in DB
            var sql = "delete from Promotion where Promotion_ID = @promoID";
            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@promoID", promoID);
            command.ExecuteNonQuery();
        }

        // assign null value to promotion Id in purchase table
        // before remove promotion from promotion table
        public void assignNullPromotionIdInPurchase(int promoID)
        {
            var sql = "update Purchase set Promotion_ID = NULL where Promotion_ID = @promo";

            var command = new SqlCommand(sql, Global.Connection);
            
            command.Parameters.AddWithValue("@promo", promoID);
            command.ExecuteNonQuery();
        }

        // re-calculate total after remove promotion code
        // from the purchase
        // default promo code will be NULL
        // --> No discount --> 0% sale
        public void reCalculateTotalOfPurchase(int promoID, double promoPercentage)
        {
            var sqlTotal = "select Purchase_ID, Total from Purchase where Promotion_ID = @promoID";
            var command = new SqlCommand(sqlTotal, Global.Connection);
            SqlDataReader reader;
            List<int> purchaseIdList = new List<int>();
            List<double> totalList = new List<double>();

            
            command.Parameters.AddWithValue("@promoID", promoID);

            reader = command.ExecuteReader();

            // get order ID and Total of each Order with the promotion_ID
            while (reader.Read())
            {
                purchaseIdList.Add((int)reader["Purchase_ID"]);
                totalList.Add((double)reader["Total"]);
            }

            reader.Close();

            // calculate new total
            promoPercentage = (100 - promoPercentage) / 100;
            for(int i = 0; i < totalList.Count; i++)
            {
                totalList[i] = totalList[i] / promoPercentage;
            }

            // update total of each selected purchase ID
            var sqlUpdateTotal = "update Purchase set Total = @newTotal where Purchase_ID = @orderID";

            command = new SqlCommand(sqlUpdateTotal, Global.Connection);

            for(int i = 0; i < purchaseIdList.Count; i++)
            {
                command.Parameters.AddWithValue("@orderID", purchaseIdList[i]);
                command.Parameters.AddWithValue("@newTotal", totalList[i]);

                command.ExecuteNonQuery();

                command.Parameters.Clear();
            }
        }
    }

}
