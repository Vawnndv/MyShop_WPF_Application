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
    class CategoryRepository
    {
        public ObservableCollection<CategoryModel> getAllCategory()
        {
            ObservableCollection<CategoryModel> result = new ObservableCollection<CategoryModel>();
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = $"select * from Category";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int? cId = (int)reader["Category_ID"];
                    string cName = (string)reader["Category_Name"];

                    // add category from DB to collection
                    result.Add(new CategoryModel()
                    {
                        CategoryID = cId,
                        CategoryName = cName,
                    });
                }

                reader.Close();
            }

            Global.Connection?.Close();
            return result;
        }

    }
}
