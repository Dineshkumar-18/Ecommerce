using System;

namespace Ecommerce.entity
{
    public class Customer
    {
        public int CustomerID { get; set; };
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Customer() { }
        public Customer(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
