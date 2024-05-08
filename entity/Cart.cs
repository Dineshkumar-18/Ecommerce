using System;

namespace Ecommerce.entity
{
    public class Cart
    {
        public int Quantity { get; set; }

        public Cart() { }
        public Cart(int quantity)
        {
            Quantity = quantity;
        }
    }
}
