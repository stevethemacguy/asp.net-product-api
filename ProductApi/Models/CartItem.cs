using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class CartItem
    {
        //The item's ID
        public int Id { get; set; }

        //Reference to the Shopping Cart that contains this item. CartItems do not exist without a cart!
        public int ShoppingCartId { get; set; }

        //How many of this CartItem are in the Cart
        public int Quantity { get; set; }

        //Needed? Each cart item has one product (or an id to it..not sure which it needs)
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
