namespace ProductApi.Models
{
    public class ShippingAddress
    {
        //Use the default EF Id. Don't need to specify

        //FK pointing to the User. This shouldn't be required, but we may need it later
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        //Assume a basic US State for now
        public string State { get; set; }

        //Assume a basic country
        public string Country { get; set; }

        public int ZipCode { get; set; }
    }
}
