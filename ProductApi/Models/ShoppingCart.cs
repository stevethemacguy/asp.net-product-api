using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        //ID of the customer that created this cart
        //public int customerId { get; set; }

        public int NumberOfCartItems
        {
            get { return CartItems.Count(); }
        }

        // You could also set this in the constructor
        public ICollection<CartItem> CartItems { get; set; }
            = new List<CartItem>();
    }
}
