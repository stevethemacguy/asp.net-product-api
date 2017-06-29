using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        //public Product Product { get; set; } //Is this needed in the DTO?
        public int ProductId { get; set; }

        public string SavedProductDescription { get; set; }
        public int Quantity { get; set; }

        public Decimal BasePrice { get; set; }
        public Decimal Discount { get; set; }
        public Decimal FinalCost { get; set; }

        public int OrderId { get; set; }
        //public Order Order { get; set; } //Is this needed in the DTO?
    }
}
