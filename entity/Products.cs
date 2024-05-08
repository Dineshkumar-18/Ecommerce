using System;

namespace Ecommerce.entity
{
    public class Products
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }

        public Products() { }
        public Products(string name, decimal price, string description, int stockQuantity)
        {
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }
    }
}
