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

        //This is actually just the userID that purchased the item. No intermediate ShoppingCart object exists.
        public string ShoppingCartId { get; set; }

        //How many of this CartItem are in the Cart
        public int Quantity { get; set; }

        //Create a navigation property that points to a product. Not sure if this is needed.
        public ProductEntity Product { get; set; }

        //Create a foreign key that points to the related product that this CartItem represents. 
        public int ProductId { get; set; }
    }
}
