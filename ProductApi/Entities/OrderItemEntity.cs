using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Entities
{
    public class OrderItemEntity
    {
        //Let EF create the ID

        //Create a navigation property that points to a product. Required by EF
        public ProductEntity Product { get; set; }

        //Create a foreign key that points to the product. Might not be needed by EF?
        public int ProductId { get; set; }

        //A cached description of the product at the time of purchase. Might not be needed.
        public string SavedProductDescription { get; set; }

        //Quantity of the product that are on the order
        public int Quantity { get; set; }

        //Base Price of the product at the time of purchase. Excludes any discounts
        //Decimal should be used for money, but use double if you need to
        public Decimal BasePrice { get; set; }

        //Dollar value discount if any on this order item.Should this be a percent?
        public Decimal Discount { get; set; }

        //Final cost of the product at the time of purchase after applying the discount
        public Decimal FinalCost { get; set; }

        //EF Documentation:
        /*https://docs.microsoft.com/en-us/ef/core/modeling/relationships
         The most common pattern for relationships is to have navigation properties defined on both 
         ends of the relationship and a foreign key property defined in the dependent entity class.
         This is what I'm doing for OrderItem. 
         
         However, technically neither the FK or Navigation property are needed on the dependent/child entity.
         A single navigation property on the parent (e.g. Order) is suffecient for a simple relationship.
         The child (e.g. OrderItem) does not need a navigation property (no inverse navigation) or FK.
        */
        
        //Foreign key to reference the Order and Navigation property to access the OrderEntity
        //This is a "fully defined relationship" that allows for navigation both ways, but technically
        public int OrderId { get; set; }
        public OrderEntity Order{ get; set; }

    }
}
