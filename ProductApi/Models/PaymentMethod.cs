using System;
using ProductApi.Entities;

namespace ProductApi.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        //Billing adddress associated with this PM
        public BillingAddress BillingAddress { get; set; }

        //Not sure if this should be saved
        public int CardNumber { get; set; }

        //The type of credit card (e.g. Visa, Mastercard, etc)
        public CreditCardType CardType { get; set; }

        //Used by the user to give this payment method a "name"
        public string CustomCardName { get; set; }

        //Set to false if the credit card has expired or is no longer valid
        public string IsValid { get; set; }

        //This should be encrypted.
        public int SecurityCode { get; set; }

        public string ExpirationDate { get; set; }
    }
}
