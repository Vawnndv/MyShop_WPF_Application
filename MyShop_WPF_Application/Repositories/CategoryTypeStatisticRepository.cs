using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop_WPF_Application.Repositories
{
    class CategoryTypeStatisticRepository
    {
        public ObservableCollection<CategoryTypeStatistic> getAllCategory()
        {
            ObservableCollection<CategoryTypeStatistic> result = new ObservableCollection<CategoryTypeStatistic>();
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {
                string sql = "SELECT p.Category_ID, Count(*) as NumOfProduct, c.Category_Name\r\nFROM Product p, Category c\r\nwhere p.Category_ID = c.Category_ID\r\nGROUP BY p.Category_ID, c.Category_Name";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();
               
                while (reader.Read())
                {
                    int id = (int)reader["Category_ID"];
                    string name = (string)reader["Category_Name"];
                    int num = (int)reader["NumOfProduct"];

                    result.Add(new CategoryTypeStatistic()
                    {
                        id = id,
                        name = name,
                        numOfProduct = num
                    });
                }

                reader.Close();
            }
            Global.Connection.Close();
            return result;
        }
    }
}
