using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Entities
{
    public class ShoppingCartEntity
    {
        public int Id { get; set; }

        //ID of the customer that created this cart
        //public int customerId { get; set; }

        public int NumberOfCartItems
        {
            get { return CartItems.Count(); }
        }

        // You could also set this in the constructor
        public ICollection<CartItemEntity> CartItems { get; set; }
            = new List<CartItemEntity>();
    }
}
