using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class Order
    {

        //List of navigation properties to access each OrderItem
        public List<OrderItem> OrderItems { get; set; }

        //Try with default EF Id, but I may want to define OrderId (instead of just ID)
        //Caution, might need to change this to OrderId so the foreign key works!
        public int Id { get; set; }

        public string UserId { get; set; }

        public ShippingAddress ShippingAddress { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentMethod PaymentMethodUsed { get; set; }
        //public int ShippingMethodType { get; set; }

        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public OrderStatus OrderStatus { get; set; }

        //Base sales tax rate used when calculating the final cost of the order
        public Decimal SalesTaxRate { get; set; }
        public Decimal TotalTax { get; set; }
        public Decimal TotalShippingCost { get; set; }

        //Special discount applied at checkout.
        public Decimal CheckoutDiscount { get; set; }

        public Decimal TotalCost { get; set; }
    }
}
