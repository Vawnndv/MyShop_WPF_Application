using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyShop_WPF_Application.Repositories
{
    class PromotionRepository
    {
        // get all promotion in the DB and save in to observable collection
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
                        _promotionPercentage = (double)reader["Promotion_Percentage"]
                    });
                }

                reader.Close();
            }

            return promoLst;
        }

        // delete a promotion completely from the Promotion table
        public void deletePromotionFromPromotionTable(int promoID, double promoPercentage)
        {
            // re-calculate the total of each order that using this promo ID
            reCalculateTotalOfPurchase(promoID, promoPercentage, 0, false);

            // assign each order using this promoID with NULL -> no sale
            assignNullPromotionIdInPurchase(promoID);

            // delete promo ID from DB
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
        private void reCalculateTotalOfPurchase(int promoID, double oldPromoPercentage, double newPercentage, Boolean calType)
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

            // calculate and return to original total (return to non sale - 0%)
            oldPromoPercentage = (100 - oldPromoPercentage) / 100;
            for(int i = 0; i < totalList.Count; i++)
            {
                totalList[i] = (double)totalList[i] / oldPromoPercentage;
            }

            // calculate if change promotion code to another one
            newPercentage = (100 - newPercentage) / 100;
            if (calType)
            {
                for(int i = 0; i < totalList.Count; ++i)
                {
                    totalList[i] = (double)totalList[i] * newPercentage;
                }
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

        // edit a promotion 
        public void editPromoPercentageInDB(int promoID, double oldPercentage, double newPercentage)
        {
            // recalulate total of order with promo ID
            reCalculateTotalOfPurchase(promoID, oldPercentage, newPercentage, true);

            var sqlUpdatePercentage = "update Promotion set Promotion_Percentage = @newPer where Promotion_ID = @promoID";
            var command = new SqlCommand(sqlUpdatePercentage, Global.Connection);

            command.Parameters.AddWithValue("@newPer", newPercentage);
            command.Parameters.AddWithValue("@promoID", promoID);

            command.ExecuteNonQuery();
        }

        public void editPromoNameInDB(int promoID, string promoName)
        {
            var sqlUpdate = "update Promotion set Promotion_Name = @newName where Promotion_ID = @promoID";
            var command = new SqlCommand(sqlUpdate, Global.Connection);

            command.Parameters.AddWithValue("@newName", promoName);
            command.Parameters.AddWithValue("@promoID", promoID);

            command.ExecuteNonQuery();
        }

        public double getPromoPercentage(int promoID) {
            double result = 0;

            var sql = "select Promotion_Percentage from Promotion where Promotion_ID = @promoID";
            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@promoID", promoID);

            var reader = command.ExecuteReader();
            
            reader.Read();

            result = (double)reader["Promotion_Percentage"];

            reader.Close();

            return result;
        }
    }

}
