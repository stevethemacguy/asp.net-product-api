using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductApi.Models;

namespace ProductApi.Entities
{
    public class OrderEntity
    {
        //Try with default EF Id, but I may want to define OrderId (instead of just ID)
        //Caution, might need to change this to OrderId so the foreign key works!
        public int Id { get; set; }

        //FK pointing to the User's ID. This is also the same as the "ShoppingCartId" in CartItem.
        public string UserId { get; set; }

        //List of navigation properties to access each OrderItem
        public List<OrderItemEntity> OrderItems { get; set; }

        //According to MS documentation, no Foreign Key should be needed on Order, since it is the parent entity.

        //Simple Relationships, so I only need to specify navigation propertys on the parent (i.e. this OrderEntity)
        public ShippingAddressEntity ShippingAddress { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentMethodEntity PaymentMethodUsed { get; set; }
        public ShippingMethodTypeEntity ShippingMethodType { get; set; }

        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public OrderStatus OrderStatus { get; set; }
        
        //Base sales tax rate used when calculating the final cost of the order
        public Decimal SalesTaxRate { get; set; }
        public Decimal TotalTax { get; set; }
        public Decimal TotalShippingCost { get; set; }
        
        //Special discount applied at checkout.
        public Decimal CheckoutDiscount { get; set; }

        //Final cost of the order, which includes the following:
        //   TotalPrice of each product (whichh includes the base price and any discount price)
        //   TotalTax
        //   TotalShippingCost
        //   Any overall special discount applied at checkout.
        public Decimal FinalCost { get; set; }
        
    }
}
