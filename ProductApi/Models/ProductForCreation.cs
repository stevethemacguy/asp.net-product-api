using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class ProductForCreation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a name value")]
        [MaxLength(40, ErrorMessage = "Name can not be longer than 40 charactors")]
        public string Name { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
