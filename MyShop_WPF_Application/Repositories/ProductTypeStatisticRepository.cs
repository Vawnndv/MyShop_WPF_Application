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
    class ProductTypeStatisticRepository
    {
        public ObservableCollection<ProductTypeStatisticModel> getAllProduct(DateTime start, DateTime end, int _id)
        {
            ObservableCollection<ProductTypeStatisticModel> result = new ObservableCollection<ProductTypeStatisticModel>();
            if (Global.Connection != null)
            {
                //string sql = "SELECT p.Category_ID, Count(*) as NumOfProduct, c.Category_Name\r\nFROM Product p, Category c\r\nwhere p.Category_ID = c.Category_ID\r\nGROUP BY p.Category_ID, c.Category_Name";
                string sql = string.Format("SELECT p.Product_ID, p.Product_Name, p.Quantity as NumOfProduct, SUM(pd.Quantity * p.Price) as SumPrice\r\nFROM Category c left join Product p on c.Category_ID = p.Category_ID join \r\n\tPurchaseDetail pd on p.Product_ID = pd.Product_ID join\r\n\tPurchase pc on pc.Purchase_ID = pd.Purchase_ID\r\nWHERE pc.Centered_At BETWEEN '{0}' AND '{1}' AND c.Category_ID = {2}\r\nGROUP BY p.Product_ID, p.Product_Name, p.Quantity", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"), _id);
                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = (int)reader["Product_ID"];
                    string name = (string)reader["Product_Name"];
                    int num = (int)reader["NumOfProduct"];
                    double sum = (double)reader["SumPrice"];

                    result.Add(new ProductTypeStatisticModel()
                    {
                        id = id,
                        name = name,
                        numOfProduct = num,
                        sumPrice = Math.Round(sum),
                        percentage = 0
                    }); ;
                }

                reader.Close();
            }
            double total = 0;
            for (int i = 0; i < result.Count; i++)
            {
                total += result[i].sumPrice;
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i].percentage = Math.Round((result[i].sumPrice / total) * 100, 2);
            }
            return result;
        }
    }
}
