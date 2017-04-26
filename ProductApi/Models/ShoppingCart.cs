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

        //This could potentially just be ids, but this seems to make sense
        public ICollection<CartItem> CartItems { get; set; }
            = new List<CartItem>();
    }
}
