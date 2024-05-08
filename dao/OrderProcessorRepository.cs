using Ecommerce.entity;
using System.Collections.Generic;
using System.Data;

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
}
