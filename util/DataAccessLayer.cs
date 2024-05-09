using System;
using System.Data.SqlClient;
using System.Text;
using Ecommerce.entity;


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
      
    }

}
