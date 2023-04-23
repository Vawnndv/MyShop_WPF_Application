using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

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

        public ObservableCollection<CategoryTypeStatistic> getCategoryWithProduct()
        {
            ObservableCollection<CategoryTypeStatistic> result = new ObservableCollection<CategoryTypeStatistic>();
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {
                //string sql = "SELECT p.Category_ID, Count(*) as NumOfProduct, c.Category_Name\r\nFROM Product p, Category c\r\nwhere p.Category_ID = c.Category_ID\r\nGROUP BY p.Category_ID, c.Category_Name";
                string sql = "SELECT c.Category_ID, c.Category_Name, count(p.Product_ID) as NumOfProduct, SUM(pd.Quantity * p.Price) as SumPrice\r\nFROM Category c left join Product p on c.Category_ID = p.Category_ID left join PurchaseDetail pd on p.Product_ID = pd.Product_ID left join Purchase pc on pc.Purchase_ID = pd.Purchase_ID\r\nGROUP BY c.Category_Name, c.Category_ID";
                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int cid = (int)reader["Category_ID"];
                    string cname = (string)reader["Category_Name"];
                    int num = (int)reader["NumOfProduct"];
                    double sum = 0;
                    if (reader["SumPrice"] != DBNull.Value)
                    {
                        sum = (double)reader["SumPrice"];
                    }

                    result.Add(new CategoryTypeStatistic()
                    {
                        id = cid,
                        name = cname,
                        numOfProduct = num,
                        sumPrice = Math.Round(sum),
                        percentage = 0
                    }); ;
                }

                reader.Close();
            }
            Global.Connection?.Close();
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

        public bool addCategory(CategoryModel category)
        {
            bool result = false;

            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();

            if (Global.Connection != null)
            {
                var sql = "INSERT INTO Category(Category_Name) VALUES(@categoryName)";

                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@categoryName", category.CategoryName);
             
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            Global.Connection?.Close();
            return result;
        }


        public bool editCategory(CategoryModel category)
        {
            bool result = false;

            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();

            if (Global.Connection != null)
            {
                var sql = "UPDATE Category SET Category_Name = @categoryName WHERE Category_ID = @categoryID";

                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@categoryID", category.CategoryID);
                command.Parameters.AddWithValue("@categoryName", category.CategoryName);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            Global.Connection?.Close();
            return result;
        }


        public bool removeCategory(int? cID)
        {
            bool result = false;
            ProductRepository productRepository = new ProductRepository();
            ObservableCollection<ProductModel> _productListWithCategoryId = productRepository.getCategoryProduct(cID);

            // delete related records in PurchaseDetail and Product table
            if (_productListWithCategoryId.Count > 0)
            {
                foreach (var product in _productListWithCategoryId)
                {
                    productRepository.removeProduct(product.ProductID);
                }
            }

            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();

            if (Global.Connection != null)
            {
                
                var sql = "DELETE FROM Category WHERE Category_ID = @categoryID";
                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@categoryID", cID);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            Global.Connection?.Close();
            return result;
        }
    }
}
