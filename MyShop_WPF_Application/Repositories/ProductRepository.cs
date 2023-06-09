﻿using Microsoft.Data.SqlClient;
using MyShop_WPF_Application.Model;
using MyShop_WPF_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_WPF_Application.Repositories
{
    class ProductRepository
    {
        public ObservableCollection<ProductModel> getAllProduct()
        {
            ObservableCollection<ProductModel> result = new ObservableCollection<ProductModel>();
            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = $"select * from Product";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int? pId = (int)reader["Product_ID"];
                    int? cId = (int)reader["Category_ID"];
                    string pName = (string)reader["Product_Name"];
                    string pAvatar = (string)reader["Avatar"];
                    int pQuantiry = (int)reader["Quantity"];
                    double pPrice = (double)reader["Price"];
                    double pPriceOriginal = (double)reader["Price_Original"];


                    // add products from DB to collection
                    result.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });
                }

                reader.Close();
            }

            //Global.Connection?.Close();
            return result;
        }

        public ObservableCollection<ProductModel> getCategoryProduct(int? _CategoryID)
        {
            ObservableCollection<ProductModel> result = new ObservableCollection<ProductModel>();
            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = $"select * from Product Where Category_ID = @CategoryID";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@CategoryID", _CategoryID);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int? pId = (int)reader["Product_ID"];
                    int? cId = (int)reader["Category_ID"];
                    string pName = (string)reader["Product_Name"];
                    string pAvatar = (string)reader["Avatar"];
                    int pQuantiry = (int)reader["Quantity"];
                    double pPrice = (double)reader["Price"];
                    double pPriceOriginal = (double)reader["Price_Original"];


                    // add products from DB to collection
                    result.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });
                }

                reader.Close();
            }

            //Global.Connection?.Close();
            return result;
        }


        public ProductModel getProductWithId(int? _productID)
        {
            ObservableCollection<ProductModel> result = new ObservableCollection<ProductModel>();
            ProductModel product = new ProductModel();

            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                var sql = $"select * from Product where Product_ID = @pID";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@pID", _productID);

                var reader = command.ExecuteReader();
                reader.Read();
                product = new ProductModel()

                {
                    ProductID = (int)reader["Product_ID"],
                    CategoryID = (int)reader["Category_ID"],
                    ProductName = (string)reader["Product_Name"],
                    ProductAvatar = (string)reader["Avatar"],
                    ProductQuantity = (int)reader["Quantity"],
                    ProductPrice = (double)reader["Price"],
                    ProductPriceOriginal = (double)reader["Price_Original"],
                };


                reader.Close();
            }

            //Global.Connection?.Close();
            return product;
        }

        public bool addProduct(ProductModel product)
        {
            bool result = false;
            int lastId = 0;

            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();

            if (Global.Connection != null)
            {
                var getLastIdSql = "SELECT TOP 1 Product_ID FROM Product ORDER BY Product_ID DESC";
                var getLastIdCommand = new SqlCommand(getLastIdSql, Global.Connection);
                var lastIdReader = getLastIdCommand.ExecuteReader();

                if (lastIdReader.Read())
                {
                    lastId = lastIdReader.GetInt32(0);
                }

                lastIdReader.Close();

                // Turn on IDENTITY_INSERT for the Category table
                var setIdInsertSql = "SET IDENTITY_INSERT Product ON";
                var setIdInsertCommand = new SqlCommand(setIdInsertSql, Global.Connection);
                setIdInsertCommand.ExecuteNonQuery();

                var sql = "INSERT INTO Product(Product_ID, Category_ID, Product_Name, Avatar, Quantity, Price, Price_Original) " +
                          "VALUES(@productId, @categoryID, @productName, @avatar, @quantity, @price, @priceOriginal)";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@productId", lastId + 1);
                command.Parameters.AddWithValue("@categoryID", product.CategoryID);
                command.Parameters.AddWithValue("@productName", product.ProductName);
                command.Parameters.AddWithValue("@avatar", product.ProductAvatar);
                command.Parameters.AddWithValue("@quantity", product.ProductQuantity);
                command.Parameters.AddWithValue("@price", product.ProductPrice);
                command.Parameters.AddWithValue("@priceOriginal", product.ProductPriceOriginal);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }

                // Turn off IDENTITY_INSERT for the Category table
                var unsetIdInsertSql = "SET IDENTITY_INSERT Product OFF";
                var unsetIdInsertCommand = new SqlCommand(unsetIdInsertSql, Global.Connection);
                unsetIdInsertCommand.ExecuteNonQuery();
            }

            //Global.Connection?.Close();
            return result;
        }


        public bool editProduct(ProductModel product)
        {
            bool result = false;

            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();

            if (Global.Connection != null)
            {
                var sql = "UPDATE Product SET Category_ID = @categoryID, Product_Name = @productName, Avatar = @productAvatar, Quantity = @productQuantity, Price = @productPrice, Price_Original = @productPriceOriginal WHERE Product_ID = @productID";

                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@categoryID", product.CategoryID);
                command.Parameters.AddWithValue("@productName", product.ProductName);
                command.Parameters.AddWithValue("@productAvatar", product.ProductAvatar);
                command.Parameters.AddWithValue("@productQuantity", product.ProductQuantity);
                command.Parameters.AddWithValue("@productPrice", product.ProductPrice);
                command.Parameters.AddWithValue("@productPriceOriginal", product.ProductPriceOriginal);
                command.Parameters.AddWithValue("@productID", product.ProductID);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            //Global.Connection?.Close();
            return result;
        }


        public bool removeProduct(int? productID)
        {
            bool result = false;

            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();

            if (Global.Connection != null)
            {
                // delete related records in PurchaseDetail table
                var relatedSql = "DELETE FROM PurchaseDetail WHERE Product_ID = @productID";
                var relatedCommand = new SqlCommand(relatedSql, Global.Connection);
                relatedCommand.Parameters.AddWithValue("@productID", productID);
                relatedCommand.ExecuteNonQuery();

                // delete the product
                var sql = "DELETE FROM Product WHERE Product_ID = @productID";
                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@productID", productID);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            //Global.Connection?.Close();
            return result;
        }

        public int getNumOfProductsAvailable()
        {
            int quantity = 0;
            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = "SELECT COUNT(*) AS QUANTITY\r\nFROM Product p\r\nWHERE p.Quantity > 0";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();
                reader.Read();

                quantity = (int)reader["QUANTITY"];

                reader.Close();
            }

            //Global.Connection?.Close();
            return quantity;
        }

        public int getNumOfPurchaseSold(DateTime start, DateTime end)
        {
            int quantity = 0;
            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = string.Format("SELECT COUNT(*) AS QUANTITY\r\nFROM Purchase p\r\nWHERE p.Centered_At BETWEEN  '{0}' AND '{1}'", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();
                reader.Read();

                quantity = (int)reader["QUANTITY"];

                reader.Close();
            }

            //Global.Connection?.Close();
            return quantity;
        }

        public ObservableCollection<ProductModel> getTop5Product()
        {
            ObservableCollection<ProductModel> result = new ObservableCollection<ProductModel>();
            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = "SELECT TOP(5) *\r\nFROM Product p\r\nWHERE p.Quantity < 5\r\nORDER BY p.Quantity";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int? pId = (int)reader["Product_ID"];
                    int? cId = (int)reader["Category_ID"];
                    string pName = (string)reader["Product_Name"];
                    string pAvatar = (string)reader["Avatar"];
                    int pQuantiry = (int)reader["Quantity"];
                    double pPrice = (double)reader["Price"];
                    double pPriceOriginal = (double)reader["Price_Original"];


                    // add products from DB to collection
                    result.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });
                }

                reader.Close();
            }

            //Global.Connection?.Close();
            return result;
        }

        public ObservableCollection<ProductBestSellModel> getTop10ProductBestSelling(DateTime start, DateTime end)
        {
            ObservableCollection<ProductBestSellModel> result = new ObservableCollection<ProductBestSellModel>();

            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = string.Format("SELECT TOP(10) p.Product_Name, SUM(pd.Quantity) as QUANTITY \r\nFROM PurchaseDetail pd\r\n\tJOIN Purchase pur on pur.Purchase_ID = pd.Purchase_ID\r\n\t" +
                    "JOIN Product p on p.Product_ID = pd.Product_ID\r\nWHERE pur.Centered_At BETWEEN '{0}' AND '{1}'\r\nGROUP BY p.Product_Name\r\nORDER BY QUANTITY DESC", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string pName = (string)reader["Product_Name"];
                    int quantity = (int)reader["QUANTITY"];

                    // add products from DB to collection
                    result.Add(new ProductBestSellModel()
                    {
                        name = pName,
                        numOfSellProduct = quantity,
                    });
                }

                reader.Close();
            }

            double total = 0;
            for (int i = 0; i < result.Count; i++)
            {
                total += result[i].numOfSellProduct;
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i].percentage = Math.Round((result[i].numOfSellProduct / total) * 100, 2);
            }
          return result;
        }
    }
}
