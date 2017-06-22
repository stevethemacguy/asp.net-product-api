using System;
using System.ComponentModel.DataAnnotations;
using ProductApi.Models;

namespace ProductApi.Entities
{
    public class PaymentMethodEntity
    {
        //Uniquely identifies this credit card
        public int Id { get; set; }

        //Billing adddress associated with this PM
        public BillingAddressEntity BillingAddress { get; set; }

        //FK pointing to the User's ID. This can be null if a payment method is used during checkout but not saved.
        public string UserId { get; set; }

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

        [Required]
        public DateTimeOffset ExpirationDate { get; set; }

        //Foreign key to reference list of Orders?
        //public int OrderId { get; set; }

        //Navigation property to allow access to this PaymentMethod from an Order.
        public PaymentMethod PaymentMethod{ get; set; }

    }
}
