using Ecommerce.entity;
using System.Collections.Generic;
using Ecommerce.util;
using System.Data.SqlClient;
using System;

namespace Ecommerce.dao
{
    public interface OrderProcessorRepository
    {
        bool createProduct(Products product);
        bool createCustomer(Customer customer);
        bool deleteProduct(int ProductID);
        bool deleteCustomer(int CustomerID);
        bool addToCart(Customer customer,Products product, int quantity);
        bool removeFromCart(Customer customer,Products product);
        List<Products> getAllFromCart(Customer customer);
        bool placeOrder(Customer customer, List<Dictionary<Products,int>> ProductsAndQuantity,string shippingAddress);
        List<Dictionary<Products, int>> getOrdersByCustomer(int CustomerID);
    }
    public class OrderProcessorRepositoryImpl : OrderProcessorRepository
    {

        public bool createCustomer(Customer customer)
        {
            using (var con = DBConnection.GetConnection())
            {
                string query = "insert into customers (name,email,password) values (@Name,@Email,@Password)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@Password", customer.Password);
                try
                {
                    con.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return true;
                    }
                    else return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

        }
        public bool createProduct(Products product)
        {
            using (var con = DBConnection.GetConnection())
            {
                string query = "insert into products (name,price,description,stockQuantity) values (@Name,@Price,@Description,@StockQuantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                try
                {
                    con.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        return true;
                    }
                    else return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }
       public bool deleteProduct(int ProductID)
        {
            return true;
        }

    }
       

}
