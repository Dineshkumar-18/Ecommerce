using System;
using System.Data.SqlClient;
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
        static void Main(string[] args)
        {
            Customer c = new Customer
            {
                Name = "Dineshkumar",
                Email = "dineshmathan2@gmail.com",
                Password = "123@Abc"
            };
            DataAccessLayer da = new DataAccessLayer();
            Console.WriteLine("ID of Dineshkumar is"+da.InsertStudent(c));
        }
    }

}
