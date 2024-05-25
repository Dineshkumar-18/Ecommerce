using Ecommerce.dao;
using Ecommerce.entity;
using Ecommerce.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                Console.WriteLine();
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
            bool checkUser = DataAccessLayer.CheckUserExist(name,email);
            if(checkUser)
            {
                Console.WriteLine("User already exist!!!!!");
                return;
            }
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
            int productId = DataAccessLayer.CheckProductAlreadyExist(name,price,description);
            int ExistingQuantity = DataAccessLayer.GetQuantityOfProduct(productId);
            int UpdatedQuantity = stockQuantity + ExistingQuantity;
            if(productId>0)
            {
                Console.WriteLine("Product is already available..... ");
                DataAccessLayer.UpdateQuantity(productId,UpdatedQuantity);
                Console.WriteLine("product Quantity is updated successfully....");
                return;
            }
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
            char ch = 'y';
            Console.Write("Enter Customer ID to Login into your account: ");
            int cusId = Convert.ToInt32(Console.ReadLine());
            do
            {
                Console.Write("Enter ProductID to Add to cart: ");
                int proId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Quantity: ");
                int quantity = Convert.ToInt32(Console.ReadLine());
                int AvailableQuantity = DataAccessLayer.GetQuantityOfProduct(proId);
                if (AvailableQuantity < 1 )
                {
                    Console.WriteLine("Requested product is out of stock!!!!!");
                    Console.WriteLine("Do you want to add some other products in your cart?  press (y/n)");
                    ch = Char.ToLower(Convert.ToChar(Console.ReadLine()));
                    continue;
                }
                else if (AvailableQuantity < quantity)
                {
                    bool check = true;
                    while (true)
                    {
                        Console.WriteLine($"only {AvailableQuantity} is left");
                        Console.WriteLine($"Do you want to purchase products within {AvailableQuantity} quantity? click (y/n)");
                        char choice = Convert.ToChar(Console.ReadLine());
                        if (Char.ToLower(choice) == 'y')
                        {
                            Console.Write("Enter Quantity: ");
                            quantity = Convert.ToInt32(Console.ReadLine());
                            if (quantity < AvailableQuantity) break;
                        }
                        else check = false;
                    }
                    if (!check) continue;
                }
                bool ExistProductIDInCart = DataAccessLayer.ExistProductIDInCart(cusId, proId);
                Products ProductInfo = DataAccessLayer.GetProductInfo(proId);
                Customer CustomerInfo = DataAccessLayer.GetCustomerInfo(cusId);
                if (ExistProductIDInCart)
                {
                    int exQuantity = DataAccessLayer.GetQuantityFromCart(cusId,proId);
                    DataAccessLayer.UpdateQuantityInCart(cusId,proId,quantity+exQuantity);
                }
                else if(_orderProcessorRepository.addToCart(CustomerInfo, ProductInfo, quantity))
                {
                    Console.WriteLine("Cart added Successfully");
                }
                else { Console.WriteLine("Error while adding to the cart"); }
                Console.WriteLine("Do you want to add more products in your cart?  press (y/n)");
                ch = Char.ToLower(Convert.ToChar(Console.ReadLine()));
            } while (ch == 'y');
        }

        //overloaded function for add to cart

        public static void AddToCart(int cusId)
        {
            char ch = 'y';
            do
            {
                Console.Write("Enter ProductID to Add to cart: ");
                int proId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Quantity: ");
                int quantity = Convert.ToInt32(Console.ReadLine());
                int AvailableQuantity = DataAccessLayer.GetQuantityOfProduct(proId);
                if (AvailableQuantity < 1)
                {
                    Console.WriteLine("Requested product is out of stock!!!!!");
                    Console.WriteLine("Do you want to add some other products in your cart?  press (y/n)");
                    ch = Char.ToLower(Convert.ToChar(Console.ReadLine()));
                    continue;
                }
                else if (AvailableQuantity < quantity)
                {
                    bool check = true;
                    while (true)
                    {
                        Console.WriteLine($"only {AvailableQuantity} is left");
                        Console.WriteLine($"Do you want to purchase products within {AvailableQuantity} quantity? click (y/n)");
                        char choice = Convert.ToChar(Console.ReadLine());
                        if (Char.ToLower(choice) == 'y')
                        {
                            Console.Write("Enter Quantity: ");
                            quantity = Convert.ToInt32(Console.ReadLine());
                            if (quantity < AvailableQuantity) break;
                        }
                        else check = false;
                    }
                    if (!check) continue;
                }
                bool ExistProductIDInCart = DataAccessLayer.ExistProductIDInCart(cusId, proId);
                Products ProductInfo = DataAccessLayer.GetProductInfo(proId);
                Customer CustomerInfo = DataAccessLayer.GetCustomerInfo(cusId);
                if (ExistProductIDInCart)
                {
                    int exQuantity = DataAccessLayer.GetQuantityFromCart(cusId, proId);
                    DataAccessLayer.UpdateQuantityInCart(cusId, proId, quantity + exQuantity);
                }
                else if (_orderProcessorRepository.addToCart(CustomerInfo, ProductInfo, quantity))
                {
                    Console.WriteLine("Cart added Successfully");
                }
                else { Console.WriteLine("Error while adding to the cart"); }
                Console.WriteLine("Do you want to add more products in your cart?  press (y/n)");
                ch = Char.ToLower(Convert.ToChar(Console.ReadLine()));
            } while (ch == 'y');
        }

        public static void ViewCart()
        {
            Console.Write("Enter Customer ID to view cart: ");
            int cusId = Convert.ToInt32(Console.ReadLine());
            Customer CustomerInfo = DataAccessLayer.GetCustomerInfo(cusId);
            List<Dictionary<Products, int>> allProductsView = _orderProcessorRepository.getAllFromCart(CustomerInfo);
            if (allProductsView.Capacity < 1)
            {
                Console.WriteLine("\nYour cart is empty!!!..... Add new item to the cart for placing order\n");
                return;
            }
            decimal OverAllTotalPrice = 0.0m;
            //for formatting purpose this will be used
            int maxProductNameLength = "Product Name".Length;
            int maxQuantityLength = "Quantity".Length;
            int maxPriceLength = "Price".Length;
            int maxTotalPriceLength = "Total Price".Length;
            int maxDescriptionLength = "Description".Length;

            foreach (var products in allProductsView)
            {
                foreach (var item in products)
                {
                    Products product = item.Key;
                    int quantity = item.Value;
                    decimal totalPrice = product.Price * quantity;
                    OverAllTotalPrice += totalPrice;
                    //for setting the adapted margin which is longer length 
                    if (product.Name.Length > maxProductNameLength)
                    {
                        maxProductNameLength = product.Name.Length;
                    }
                    if (product.Price.ToString().Length > maxPriceLength)
                    {
                        maxPriceLength = product.Price.ToString().Length;
                    }
                    if (quantity.ToString().Length > maxQuantityLength)
                    {
                        maxQuantityLength = quantity.ToString().Length;
                    }
                    if (totalPrice.ToString().Length > maxTotalPriceLength)
                    {
                        maxTotalPriceLength = totalPrice.ToString().Length;
                    }
                    if (product.Description.Length > maxDescriptionLength)
                    {
                        maxDescriptionLength = product.Description.Length;
                    }
                }
            }
            if (OverAllTotalPrice.ToString().Length > maxTotalPriceLength)
            {
                maxTotalPriceLength = OverAllTotalPrice.ToString().Length;
            }
            int totalLength = maxProductNameLength + maxQuantityLength + maxPriceLength + maxTotalPriceLength + maxDescriptionLength + 10 + 6;
            Console.WriteLine(new string('-', totalLength));

            Console.WriteLine($"| {"Product Name".PadRight(maxProductNameLength)} | {"Description".PadRight(maxDescriptionLength)} | {"Quantity".PadRight(maxQuantityLength)} | {"Price".PadRight(maxPriceLength)} | {"Total Price".PadRight(maxTotalPriceLength)} |");
            Console.WriteLine(new string('-', totalLength));

            foreach (var products in allProductsView)
            {
                foreach (var item in products)
                {
                    Products product = item.Key;
                    int quantity = item.Value;
                    decimal totalPrice = product.Price * quantity;
                    Console.WriteLine($"| {product.Name.PadRight(maxProductNameLength)} | {product.Description.PadRight(maxDescriptionLength)} | {quantity.ToString().PadRight(maxQuantityLength)} | {product.Price.ToString().PadRight(maxPriceLength)} | {totalPrice.ToString().PadRight(maxTotalPriceLength)} |");
                }
            }
            Console.WriteLine(new string('-', totalLength));
            string overallTotalPriceLabel = "Overall Total Price";
            int labelStartPos = (maxProductNameLength + maxDescriptionLength + maxQuantityLength + maxPriceLength + 9 - overallTotalPriceLabel.Length) / 2;

            string centeredLabel = overallTotalPriceLabel.PadLeft(labelStartPos + overallTotalPriceLabel.Length).PadRight(maxProductNameLength + maxDescriptionLength + maxQuantityLength + maxPriceLength + 9);

            Console.WriteLine($"| {centeredLabel} | {OverAllTotalPrice.ToString().PadRight(maxTotalPriceLength)} |");

            Console.WriteLine(new string('-', totalLength));
        }

        //overloaded function for view cart
        public static void ViewCart(int cusId)
        {
            Customer CustomerInfo = DataAccessLayer.GetCustomerInfo(cusId);
            List<Dictionary<Products, int>> allProductsView = _orderProcessorRepository.getAllFromCart(CustomerInfo);
            if (allProductsView.Capacity < 1)
            {
                Console.WriteLine("\nYour cart is empty!!!..... Add new item to the cart for placing order\n");
                return;
            }
            //Display the formatting result
            DisplayResult(allProductsView);
        }

        public static void PlaceOrder()
        {
            Console.Write("Enter Customer ID to login: ");
            int cusId= Convert.ToInt32(Console.ReadLine());
            bool checkCusIdInCart = DataAccessLayer.FetchCartDetails(cusId);
            if (!checkCusIdInCart)
            {
                Console.WriteLine("You have nothing in your cart!!!");
                Console.WriteLine("Do you want to add the products in your cart? press (y/n)");
                char choice = Convert.ToChar(Console.ReadLine());
                if (Char.ToLower(choice) == 'y')
                {
                    AddToCart(cusId);
                    Console.WriteLine("Your cart Details: ");
                    ViewCart(cusId);
                    Console.WriteLine("Do you want to proceed to place order? press (y/n)");
                    char ch = Convert.ToChar(Console.ReadLine());
                    if (Char.ToLower(ch) != 'y')
                    {
                        Console.WriteLine("Your order has cancelled....!! returning to the home page");
                        return;
                    }
                    PlaceOrder(cusId);
                    return;
                }
                else
                {
                    Console.WriteLine("Not able to proceed further, because your cart is empty");
                    return;
                }
            }
            Console.Write("Enter Shipping Address: ");
            string shippingAddress = Console.ReadLine();
            Customer cusInfo = DataAccessLayer.GetCustomerInfo(cusId);

            List<Dictionary<Products, int>> pro = DataAccessLayer.GetCartItemsFromDatabase(cusId);
            foreach (var productQuantityDict in pro)
            {
                foreach (var entry in productQuantityDict.ToList())
                {
                    Products product = entry.Key;
                    int quantity = entry.Value;
                    int availQuantity = DataAccessLayer.GetQuantityOfProduct(product.ProductID);
                    if (availQuantity < 1)
                    {
                        Console.WriteLine("Product is out of stock.....");
                        Console.WriteLine("Your order has cancelled....");
                        return;
                    }
                    while (true)
                    {
                        if (quantity > availQuantity)
                        {
                            Console.WriteLine($"only {availQuantity} is left");
                            Console.WriteLine("Do you want to proceed? press(y/n)");
                            char ch = Convert.ToChar(Console.ReadLine());
                            if (Char.ToLower(ch) == 'y')
                            {
                                Console.Write("Enter Quantity: ");
                                quantity = Convert.ToInt32(Console.ReadLine());
                                if (quantity > availQuantity)
                                {
                                    continue;
                                }
                                else
                                {
                                    productQuantityDict[product] = quantity;
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Your order has cancelled....");
                                return;
                            }
                        }
                        else break;
                    }
                }
            }
            bool orderPlaced = _orderProcessorRepository.PlaceOrder(cusInfo, pro, shippingAddress);
            if (orderPlaced)
            {
                Console.WriteLine("Order placed successfully!");
                Console.WriteLine("Customer Name: " + cusInfo.Name);
                Console.WriteLine("Shipping Address " + shippingAddress);

                DisplayResult(pro);

                //addtional logic after placing order like remove from cart, update the stockQuantity

                foreach (var item in pro)
                {
                    foreach (var entry in item)
                    {
                        Products product = entry.Key;
                        int quantity = entry.Value;
                        decimal totalPrice = product.Price * quantity;

                        bool removeCart = _orderProcessorRepository.removeFromCart(cusInfo, product);
                        int existQuantity = DataAccessLayer.GetQuantityOfProduct(product.ProductID);
                        int ModifiedQuantity = existQuantity - quantity;
                        DataAccessLayer.UpdateQuantity(product.ProductID, ModifiedQuantity);
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


        //overloaded function of place order
        public static void PlaceOrder(int cusId)
        {

            bool checkCusIdInCart = DataAccessLayer.FetchCartDetails(cusId);
            if(!checkCusIdInCart)
            {
                Console.WriteLine("You have nothing in your cart!!!");
                Console.WriteLine("Do you want to add the products in your cart? press (y/n)");
                char choice =Convert.ToChar(Console.ReadLine());
                if(Char.ToLower(choice) == 'y')
                {
                    AddToCart(cusId);
                    Console.WriteLine("Your cart Details: ");
                    ViewCart(cusId);
                    Console.WriteLine("Do you want to proceed to place order? press (y/n)");
                    char ch = Convert.ToChar(Console.ReadLine());
                    if (Char.ToLower(ch) != 'y')
                    {
                        Console.WriteLine("Your order has cancelled....!! returning to the home page");
                        return;
                    }
                    PlaceOrder(cusId);
                    return;
                }
                else
                {
                    Console.WriteLine("Not able to proceed further, because your cart is empty");
                    return;
                }
            }
            Console.Write("Enter Shipping Address: ");
            string shippingAddress = Console.ReadLine();
            Customer cusInfo = DataAccessLayer.GetCustomerInfo(cusId);
       
            List<Dictionary<Products,int>> pro = DataAccessLayer.GetCartItemsFromDatabase(cusId);
            foreach (var productQuantityDict in pro)
            {
                foreach (var entry in productQuantityDict.ToList())
                {
                    Products product = entry.Key;
                    int quantity = entry.Value;
                    int availQuantity = DataAccessLayer.GetQuantityOfProduct(product.ProductID);
                    if(availQuantity<1)
                    {
                        Console.WriteLine("Product is out of stock.....");
                        Console.WriteLine("Your order has cancelled....");
                        return;
                    }
                    while (true)
                    {
                        if (quantity > availQuantity)
                        {
                            Console.WriteLine($"only {availQuantity} is left");
                            Console.WriteLine("Do you want to proceed? press(y/n)");
                            char ch = Convert.ToChar(Console.ReadLine());
                            if (Char.ToLower(ch) == 'y')
                            {
                                Console.Write("Enter Quantity: ");
                                quantity = Convert.ToInt32(Console.ReadLine());
                                if (quantity > availQuantity)
                                {
                                    continue;
                                }
                                else
                                {
                                    productQuantityDict[product] = quantity;
                                    break;
                                } 
                            }
                            else
                            {
                                Console.WriteLine("Your order has cancelled....");
                                return;
                            }
                        }
                        else break;
                    }
                }
            }
            bool orderPlaced = _orderProcessorRepository.PlaceOrder(cusInfo, pro, shippingAddress);
            if (orderPlaced)
            {
                Console.WriteLine("Order placed successfully!");
                Console.WriteLine("Customer Name: " + cusInfo.Name);
                Console.WriteLine("Shipping Address "+shippingAddress);

                //Displying result
                DisplayResult(pro);

                //addtional logic after placing order like remove from cart, update the stockQuantity
                foreach (var item in pro)
                {
                    foreach (var entry in item)
                    {
                        Products product = entry.Key;
                        int quantity = entry.Value;
                        bool removeCart = _orderProcessorRepository.removeFromCart(cusInfo, product);
                        int existQuantity = DataAccessLayer.GetQuantityOfProduct(product.ProductID);
                        int ModifiedQuantity=existQuantity-quantity;
                        DataAccessLayer.UpdateQuantity(product.ProductID,ModifiedQuantity);
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
            if(Orders.Capacity==0)
            {
                Console.WriteLine("No history of Orders for this Customer\n");
                return;
            }

            DisplayResult(Orders);
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

        //For displying result in formated manner like table structure
        public static void DisplayResult(List<Dictionary<Products, int>> pro)
        {
            int maxProductName = "Product Name".Length;
            int maxDescription = "Description".Length;
            int maxPrice = "Price".Length;
            int maxQuantity = "Quantity".Length;
            int maxTotalPrice = "Total Price".Length;

            decimal overAllToTalPrice = 0.0m;
            //for formatting purpose

            foreach (var item in pro)
            {
                foreach (var entry in item)
                {
                    Products product = entry.Key;
                    int quantity = entry.Value;
                    decimal totalPrice = product.Price * quantity;
                    if (product.Name.Length > maxProductName)
                    {
                        maxProductName = product.Name.Length;
                    }
                    if (product.Description.Length > maxDescription)
                    {
                        maxDescription = product.Description.Length;
                    }
                    if (product.Price.ToString().Length > maxPrice)
                    {
                        maxPrice = product.Price.ToString().Length;
                    }
                    if (quantity.ToString().Length > maxQuantity)
                    {
                        maxQuantity = quantity.ToString().Length;
                    }
                    if (totalPrice.ToString().Length > maxTotalPrice)
                    {
                        maxTotalPrice = totalPrice.ToString().Length;
                    }
                }
            }
            int totalLength = maxProductName + maxDescription + maxQuantity + maxPrice + maxTotalPrice + 10 + 6;
            Console.WriteLine(new string('-', totalLength));

            Console.WriteLine($"| {"Product Name".PadRight(maxProductName)} | {"Description".PadRight(maxDescription)} | {"Quantity".PadRight(maxQuantity)} | {"Price".PadRight(maxPrice)} | {"Total Price".PadRight(maxTotalPrice)} |");
            Console.WriteLine(new string('-', totalLength));

            foreach (var item in pro)
            {
                foreach (var entry in item)
                {
                    Products product = entry.Key;
                    int quantity = entry.Value;
                    decimal totalPrice = product.Price * quantity;
                    overAllToTalPrice += totalPrice;
                    Console.WriteLine($"| {product.Name.PadRight(maxProductName)} | {product.Description.PadRight(maxDescription)} | {quantity.ToString().PadRight(maxQuantity)} | {product.Price.ToString().PadRight(maxPrice)} | {totalPrice.ToString().PadRight(maxTotalPrice)} |");
                }
            }
            if (overAllToTalPrice.ToString().Length > maxTotalPrice)
            {
                maxTotalPrice = overAllToTalPrice.ToString().Length;
            }
            Console.WriteLine(new string('-', totalLength));
            string overallTotalPriceLabel = "Overall Total Price";
            int labelStartPos = (maxProductName + maxDescription + maxQuantity + maxPrice + 9 - overallTotalPriceLabel.Length) / 2;

            string centeredLabel = overallTotalPriceLabel.PadLeft(labelStartPos + overallTotalPriceLabel.Length).PadRight(maxProductName + maxDescription + maxQuantity + maxPrice + 9);

            Console.WriteLine($"| {centeredLabel} | {overAllToTalPrice.ToString().PadRight(maxTotalPrice)} |");

            Console.WriteLine(new string('-', totalLength));
        }
    }
}
