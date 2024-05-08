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
                finally
                {
                    con.Close();
                }
            }
        }
       public bool deleteProduct(int ProductID)
        {

           using(var con = DBConnection.GetConnection())
            {
                string query = "delete from products where product_id=@ProductID";
                SqlCommand cmd = new SqlCommand(query,con);
                try
                {
                    con.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0) return true;
                    else return false;
                }
                catch(Exception e) { Console.WriteLine(e.Message); return false; }
                finally { con.Close(); }
            }
        }

        public bool deleteCustomer(int CustomerID)
        {
            using (var con = DBConnection.GetConnection())
            {
                string query = "delete from customers where customer_id=@CustomerID";
                SqlCommand cmd = new SqlCommand(query, con);
                try
                {
                    con.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0) return true;
                    else return false;
                }
                catch (Exception e) { Console.WriteLine(e.Message); return false; }
                finally { con.Close(); }
            }
        }
        public bool addToCart(Customer customer, Products product, int quantity)
        {
            using (var con = DBConnection.GetConnection())
            {
                string query = "insert into cart(customer_id,product_id,quantity) values(@CustomerID,@ProductID,@Quantity)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerID",customer.CustomerID);
                cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                cmd.Parameters.AddWithValue("@Quantity",quantity);
                try
                {
                    con.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0) return true;
                    else return false;
                }
                catch (Exception e) { Console.WriteLine(e.Message); return false; }
                finally { con.Close(); }
            }
        }
        public bool removeFromCart(Customer customer, Products product)
        {
            using (var con = DBConnection.GetConnection())
            {
                string query = "delete from cart where customer_id=@CustomerId and product_id=@ProductId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerID);
                cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                try
                {
                    con.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0) return true;
                    else return false;
                }
                catch (Exception e) { Console.WriteLine(e.Message); return false; }
                finally { con.Close(); }
            }
        }
        public List<Products> getAllFromCart(Customer customer)
        {
            List<Products> AllCartDetails = new List<Products>();
            using (var con = DBConnection.GetConnection())
            {
                string query = "select product_id from cart where customer_id=@CustomerId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId",customer.CustomerID);
                try
                {
                    con.Open();
                    SqlDataReader sr = cmd.ExecuteReader();
                    while(sr.Read())
                    {
                        AllCartDetails.Add(DataAccessLayer.GetProductInfo((int)sr["product_id"]));
                    }
                    return AllCartDetails;
                    
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
                finally { con.Close(); }
            }
            return null;
        }

        public bool placeOrder(Customer customer, List<Dictionary<Products, int>> ProductsAndQuantity, string shippingAddress)
        {

        }

    }
       

}
