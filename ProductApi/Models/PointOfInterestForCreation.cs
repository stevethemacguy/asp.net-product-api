using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    public class PointOfInterestForCreation
    {
        [Required(ErrorMessage = "You must provide a name value")]
        [MaxLength(40, ErrorMessage = "Name can not be longer than 40 charactors")]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
    }
}
