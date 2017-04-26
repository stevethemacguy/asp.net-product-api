using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class CartItemForCreation
    {
        //Reference to the Shopping Cart that contains this item. CartItems do not exist without a cart!
        public int ShoppingCartId { get; set; }

        public int ProductId { get; set; }

        //How many of this CartItem are in the Cart
        public int Quantity { get; set; }
    }
}
