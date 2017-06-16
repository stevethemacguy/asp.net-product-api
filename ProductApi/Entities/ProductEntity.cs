using System.ComponentModel.DataAnnotations;

namespace ProductApi.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }
        public string ImageUrl { get; set; }

        //A FK to the orderItem? MS documentation says this is needed since Product is a dependent
        //public int OrderItemId { get; set; }
        //public OrderItemId OrderItemId{ get; set; }
    }
}
