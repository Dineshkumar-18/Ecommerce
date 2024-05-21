﻿using Ecommerce.dao;
using Ecommerce.entity;
using Ecommerce.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.main
{
    public class EcommApp
    {
        private static OrderProcessorRepository _orderProcessorRepository=new OrderProcessorRepositoryImpl();
        static void Main(string[] args)
        {
            Console.WriteLine("---------------------------------------------Welcome To Ecommerce Service------------------------------------------------");
            while (true)
            {
                Console.WriteLine("1. Register Customer");
                Console.WriteLine("2. Create Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. Add to cart");
                Console.WriteLine("5. View cart");
                Console.WriteLine("6. Place order");
                Console.WriteLine("7. View Customer Order");
                Console.WriteLine("8. Remove From Cart");
                Console.Write("Enter your choice :");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        RegisterCustomer();
                        break;
                    case "2":
                        CreateProduct();
                        break;
                    case "3":
                        DeleteProduct();
                        break;
                    case "4":
                        AddToCart();
                        break;
                    case "5":
                        ViewCart();
                        break;
                    case "6":
                        PlaceOrder();
                        break;
                    case "7":
                        ViewCustomerOrder();
                        break;
                    case "8":
                        RemoveFromCart();
                        break;
                    default:
                        Console.WriteLine("invalid choice!!!!");
                        break;
                }

            }
        }

        public static void RegisterCustomer()
        {
            Console.WriteLine("Register Here");
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            Customer customer = new Customer
            {
                Name = name,
                Email = email,
                Password = password
            };
            if (_orderProcessorRepository.createCustomer(customer))
            {
                Console.WriteLine("Register Successsfully......!!!!!!");
            }
            else { Console.WriteLine("Register unsuccessful....."); }
                
        }
        public static void CreateProduct()
        {
            Console.WriteLine("Create Product Here....");
            Console.Write("Produt Name: ");
            string name = Console.ReadLine();
            Console.Write("Produt Price (Ex.100.00):");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Product Description: ");
            string description = Console.ReadLine();
            Console.Write("StockQuantity: ");
            int stockQuantity = Convert.ToInt32(Console.ReadLine());
            Products product = new Products
            {
                Name = name,
                Price = price,
                Description = description,
                StockQuantity = stockQuantity
            };
            if(_orderProcessorRepository.createProduct(product))
            {
                Console.WriteLine("Product created successfully.....");
            }
            else
            {
                Console.WriteLine("Product creating is failed");
            }
        }
        public static void DeleteProduct()
        {
            Console.Write("Enter Product  ID: ");
            int proId = Convert.ToInt32(Console.ReadLine());
            if(_orderProcessorRepository.deleteProduct(proId))
            {
                Console.WriteLine("Product deleted Successfully");
            }
            else {
                Console.WriteLine("Product deleting is not done");
            }
        }
        public static void AddToCart()
        {
            Console.Write("Enter Customer ID to Login into your account: ");
            int cusId = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter ProductID to Add to cart: ");
            int proId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());
            Products ProductInfo = DataAccessLayer.GetProductInfo(proId);
            Customer CustomerInfo = DataAccessLayer.GetCustomerInfo(cusId);
            if(_orderProcessorRepository.addToCart(CustomerInfo,ProductInfo,quantity))
            {
                Console.WriteLine("Cart added Successfully");
            }
            else { Console.WriteLine("Error while adding to the cart"); }
        }
        public static void ViewCart()
        {
            Console.Write("Enter Customer ID to Login into your account: ");
            int cusId = Convert.ToInt32(Console.ReadLine());
            Customer CustomerInfo = DataAccessLayer.GetCustomerInfo(cusId);
            List<Dictionary<Products,int>> allProductsView = _orderProcessorRepository.getAllFromCart(CustomerInfo);
            if (allProductsView.Capacity < 1)
            {
                Console.WriteLine("\nYour cart is empty!!!..... Add new item to the cart for placing order\n");
                return;
            }
            foreach (var products in allProductsView)
            {
                foreach (var item in products)
                {
                    Products product = item.Key;
                    int quantity = item.Value;
                    decimal totalPrice = product.Price * quantity;
                    Console.WriteLine(product.Name + "            " + product.Price +"        "+ quantity+"        " +totalPrice+ "               " +product.Description); 
                }
            }
        }
        public static void PlaceOrder()
        {
            Console.Write("Enter Customer ID: ");
            int cusId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Shipping Address: ");
            string shippingAddress = Console.ReadLine();
            Customer cusInfo = DataAccessLayer.GetCustomerInfo(cusId);
       
            List<Dictionary<Products,int>> pro = DataAccessLayer.GetCartItemsFromDatabase(cusId);
            bool orderPlaced = _orderProcessorRepository.PlaceOrder(cusInfo, pro, shippingAddress);
            if (orderPlaced)
            {
                Console.WriteLine("Order placed successfully!");
                Console.WriteLine("Customer ID: " + cusInfo.CustomerID);
                Console.WriteLine("Shipping Address: " + shippingAddress);
                foreach (var item in pro)
                {

                    foreach (var entry in item)
                    {
                        Products product = entry.Key;
                        int quantity = entry.Value;
                        Console.WriteLine("Product ID: " + product.ProductID + ", Product Name: "+product.Name+" Product Description: "+product.Description+", Quantity: " + quantity);
                        bool removeCart = _orderProcessorRepository.removeFromCart(cusInfo, product);
                        if (removeCart) continue;
                        else
                        {
                            Console.WriteLine("Error while removing....");
                        }
                    }
                }
                
            }
            else
            {
                Console.WriteLine("Failed to place order.");
            }
        }
        public static void ViewCustomerOrder()
        {
            Console.Write("Customer ID: ");
            int CustomerID = Convert.ToInt32(Console.ReadLine());
            List<Dictionary<Products, int>> Orders = _orderProcessorRepository.GetOrdersByCustomer(CustomerID);
     
            foreach(var entry in Orders)
            {
                foreach(var item in entry)
                {
                    Products product = item.Key;
                    int quatity = item.Value;
                    Console.WriteLine(product.Name+"   "+product.Price+"  "+product.Description+"  "+quatity);
                }
            }
        }
        public static void RemoveFromCart()
        {
            Console.Write("Customer ID: ");
            int CustomerID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter ProductID to remove from cart: ");
            int ProductID = Convert.ToInt32(Console.ReadLine());
            Products ProductInfo = DataAccessLayer.GetProductInfo(ProductID);
            Customer CustomerInfo = DataAccessLayer.GetCustomerInfo(CustomerID);
            if (_orderProcessorRepository.removeFromCart(CustomerInfo, ProductInfo))
            {
                Console.WriteLine("Cart removed Successfully");
            }
            else { Console.WriteLine("Error while removing to the cart"); }
        }

    }
}
