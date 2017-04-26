using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class CartItem
    {
        public string ItemId { get; set; }

        //Reference to the Shopping Cart that contains this item. CartItems do not exist without a cart!
        public string CartId { get; set; }

        //How many of this CartItem are in the Cart
        public int Quantity { get; set; }
    }
}
