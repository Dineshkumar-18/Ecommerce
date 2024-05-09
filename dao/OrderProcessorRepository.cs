using Ecommerce.entity;
using System.Collections.Generic;
using Ecommerce.util;
using System.Data.SqlClient;
using System;
using System.Transactions;

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
        bool PlaceOrder(Customer customer, List<Dictionary<Products,int>> ProductsAndQuantity,string shippingAddress);
        List<Dictionary<Products, int>> GetOrdersByCustomer(int CustomerID);
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
                cmd.Parameters.AddWithValue("@ProductID",ProductID);
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
                string query = "insert cart(customer_id,product_id,quantity) values(@CustomerID,@ProductID,@Quantity)";
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

        public bool PlaceOrder(Customer customer, List<Dictionary<Products, int>> productsAndQuantities, string shippingAddress)
        {

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                connection.Open();

                using (TransactionScope scope = new TransactionScope())
                {
                   
                    int orderId;
                    decimal totalPrice = 0; 
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO orders (customer_id,total_price,shipping_address) VALUES (@CustomerId, @TotalPrice, @ShippingAddress); SELECT SCOPE_IDENTITY();";
                        command.Parameters.AddWithValue("@CustomerId", customer.CustomerID);
                        command.Parameters.AddWithValue("@TotalPrice", DBNull.Value);
                        command.Parameters.AddWithValue("@ShippingAddress", shippingAddress);
                        orderId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    if (orderId <= 0)
                    {
                        return false;
                    }

                    // Insert into OrderItems table
                    foreach (var productQuantityDict in productsAndQuantities)
                    {
                        foreach (var entry in productQuantityDict)
                        {
                            Products product = entry.Key;
                            int quantity = entry.Value;

                            // Calculate total price for each product
                            decimal productTotalPrice = product.Price * quantity;
                            totalPrice += productTotalPrice;

                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "INSERT INTO order_items (order_id, product_id, quantity) VALUES (@OrderId, @ProductId, @Quantity)";
                                command.Parameters.AddWithValue("@OrderId", orderId);
                                command.Parameters.AddWithValue("@ProductId", product.ProductID);
                                command.Parameters.AddWithValue("@Quantity", quantity);
                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    // Update Orders table with total price
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE orders SET total_price = @TotalPrice WHERE order_id = @OrderId";
                        command.Parameters.AddWithValue("@TotalPrice", totalPrice);
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.ExecuteNonQuery();
                    }
                    scope.Complete();
                    return true;
                }
            }
        }
        public List<Dictionary<Products, int>> GetOrdersByCustomer(int CustomerID)
        {
            
            List<Dictionary<Products, int>> orders = new List<Dictionary<Products, int>>();

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                connection.Open();

                // Query to retrieve orders by customer ID
                string query = @"SELECT O.order_id, P.product_id, P.name, P.price, OI.quantity
                             FROM orders O
                             JOIN order_items OI ON O.order_id = OI.order_id
                             JOIN products P ON OI.product_id = P.product_id
                             WHERE O.customer_id = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", CustomerID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Dictionary<Products, int> orderItems = null;
                        while (reader.Read())
                        {
                            int orderId = (int)reader["order_id"];
                            int productId = (int)reader["product_id"];
                            string productName = (string)reader["name"];
                            decimal productPrice = (decimal)reader["price"];
                            int quantity = (int)reader["quantity"];

                            if (orderItems == null || orderItems.ContainsKey(new Products { ProductID = productId, Name = productName, Price = productPrice }) == false)
                            {
                                orderItems = new Dictionary<Products, int>();
                                orders.Add(orderItems);
                            }

                            orderItems[new Products { ProductID = productId, Name = productName, Price = productPrice }] = quantity;
                        }
                    }
                }
            }

            return orders;
        }
    

    }
       

}
