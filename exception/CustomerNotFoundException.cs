using System;


namespace Ecommerce.exception
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string message) : base(message)
        {

        }
    }
}
