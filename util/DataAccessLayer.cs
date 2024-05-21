using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Ecommerce.entity;
using Ecommerce.dao;
using System.ComponentModel.Design;


namespace Ecommerce.util
{
    public class DataAccessLayer
    {
        public int InsertStudent(Customer customer)
        {
            int generateID = 0;
            using (var connection = DBConnection.GetConnection())
            {
                string query = "INSERT INTO customers (name,email,password) VALUES (@Name,@Email,@Password); SELECT SCOPE_IDENTITY();";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Email",customer.Email);
                command.Parameters.AddWithValue("@Password", customer.Password);
                try
                {
                    connection.Open();
                    generateID = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return generateID;
        }
        public static Customer GetCustomerInfo(int CusNo)
        {
            using (var connection = DBConnection.GetConnection())
            {
                string query = "select * from customers where customer_id=@CustomerId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerId", CusNo);
                try
                {
                    connection.Open();
                    SqlDataReader sr = command.ExecuteReader();
                    if(sr.Read())
                    {
                         return new Customer
                          {
                            CustomerID = (int)sr["customer_id"],
                            Name = (string)sr["name"],
                            Email= (string)sr["email"],
                            Password = (string)sr["password"]
                          };
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return null;

        }
        public static Products GetProductInfo(int ProNum)
        {
            using (var connection = DBConnection.GetConnection())
            {
                string query = "select * from products where product_id=@ProductId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", ProNum);
                try
                {
                    connection.Open();
                    SqlDataReader sr = command.ExecuteReader();
                    if (sr.Read())
                    {
                        return new Products
                        {
                            ProductID = (int)sr["product_id"],
                            Name = (string)sr["name"],
                            Price = (decimal)sr["price"],
                            Description = (string)sr["description"],
                            StockQuantity = (int)sr["stockQuantity"]
                        };
                       
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return null;

        }
        public static List<Dictionary<Products, int>> GetCartItemsFromDatabase(int customerId)
        {
            List<Dictionary<Products, int>> cartItems = new List<Dictionary<Products, int>>();

          
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT product_id, quantity FROM cart WHERE customer_id = @CustomerId";
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int productId = Convert.ToInt32(reader["product_id"]);
                        Products productInfo = GetProductInfo(productId);
                        int quantity = Convert.ToInt32(reader["quantity"]);
                        Dictionary<Products, int> items = new Dictionary<Products, int>();
                        items.Add(productInfo,quantity);
                        cartItems.Add(items);
                    }
                }
            }
            return cartItems;
        }

        public static int GetQuantityFromCart(int cusId, int productID)
        {
            int quantities = 0;
            string query = "select quantity from cart where customer_id=@CustomerId and product_id=@ProductId";
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", cusId);
                    command.Parameters.AddWithValue("@ProductId", productID);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                       quantities=((int)reader["quantity"]);
                    }
                }
            }
            return quantities;
        }

        public static int GetQuantityOfProduct(int proId)
        {
            int quantities = 0;
            string query = "select stockQuantity from products where product_id=@ProductId";
            using (SqlConnection connection = DBConnection.GetConnection())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", proId);
                    SqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        quantities = ((int)reader["stockQuantity"]);
                    }
                }
            }
            return quantities;
        }
    }

}
