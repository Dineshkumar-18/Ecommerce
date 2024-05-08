using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.main
{
    public class EcommApp
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Register Customer");
                Console.WriteLine("Create Product");
                Console.WriteLine("Delete Product");
                Console.WriteLine("Add to cart");
                Console.WriteLine("View cart");
                Console.WriteLine("Place order");
                Console.WriteLine("View Customer Order");
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
                    default:
                        break;
                }
            }
        }
    }
}
