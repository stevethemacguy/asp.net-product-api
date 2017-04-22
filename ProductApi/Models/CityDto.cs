using System.Collections.Generic;
using System.Linq;

namespace ProductApi.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfPointsOfInterest
        {
            get { return PointsOfInterest.Count(); }
        }

        // You could also set this in the constructor
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; }
            = new List<PointOfInterestDto>();
    }
}
