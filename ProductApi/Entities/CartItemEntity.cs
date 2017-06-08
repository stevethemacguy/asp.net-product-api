using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ProductApi.Models;

namespace ProductApi.Entities
{
    public class CartItemEntity
    {
        //[Key] //Use itemId as the key, instead of just ID
        //public string ItemId { get; set; }
        
        //The item's id
        public int Id { get; set; }

        //Reference to the Shopping Cart that contains this item. CartItems do not exist without a cart!
        public string ShoppingCartId { get; set; }

        //How many of this CartItem are in the Cart
        public int Quantity { get; set; }

        //Create a foreign key (i.e. navigation property) that points to the related produc that this CartItem represents. 
        public ProductEntity Product { get; set; }
        public int ProductId { get; set; }

        //Does it need a nav property to the cart? I don't know
        //public ShoppingCart Cart { get; set; }
    }
}
