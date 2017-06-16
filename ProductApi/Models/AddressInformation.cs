using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Models
{
    public class AddressInformation
    {

        public AddressInformation()
        {
            
        }

        public ShippingAddress ShippingAddress { get; set; }
        public BillingAddress BillingAddress { get; set; }

    }
}
