using System;
using ProductApi.Entities;

namespace ProductApi.Models
{
    public class PaymentMethod
    {
        //Billing adddress associated with this PM
        public BillingAddressEntity BillingAddress { get; set; }

        //FK pointing to the User's ID. This can be null if a payment method is used during checkout but not saved.
        ///public string UserId { get; set; }

        //Not sure if this should be saved
        public int CardNumber { get; set; }

        //The type of credit card (e.g. Visa, Mastercard, etc)
        public CreditCardType CardType { get; set; }

        //Used by the user to give this payment method a "name"
        public string CustomCardName { get; set; }

        //Set to false if the credit card has expired or is no longer valid
        public string IsValid { get; set; }

        //This should be encrypted.
        public string SecurityCode { get; set; }

        public DateTimeOffset ExpirationDate { get; set; }
    }
}
