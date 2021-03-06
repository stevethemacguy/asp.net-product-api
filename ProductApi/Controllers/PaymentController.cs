﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductApi.Entities;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Authorize]
    [Route("api/payment")]
    public class PaymentController: Controller
    {
        private IProductRepository _productRepo;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;

        public PaymentController(UserManager<User> userManager, ILogger<AccountController> logger, IProductRepository productRepo)
        {
            _userManager = userManager;
            _logger = logger;
            _productRepo = productRepo;
        }

        [HttpPost("createPaymentMethod")]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] PaymentMethod paymentMethod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            //Map the payment Method DTO to an entity
            var finalPaymentMethod= AutoMapper.Mapper.Map<Entities.PaymentMethodEntity>(paymentMethod);

            //See if the payment already exists in the database
            var paymentExists = _productRepo.PaymentMethodExists(finalPaymentMethod.Id);

            //If the paymentMethod already exists, then stop.
            if (paymentExists)
            {
                return BadRequest(ModelState);
            }

            var userHasPaymentMethods = _productRepo.GetPaymentMethods(user.Id);

            //If there are any payment methods, check for duplicate credit card numbers.
            if (userHasPaymentMethods != null)
            {
                var existingPayment = _productRepo.GetPaymentMethods(user.Id).FirstOrDefault(p => p.CardNumber == paymentMethod.CardNumber);

                //Duplicate Card number
                if (existingPayment != null)
                {
                    return BadRequest(ModelState);
                }
            }

            //Get the billing address from the Post body
            var billingAddress = paymentMethod.BillingAddress;

            //Convert the billing address into an entity
            var billingAddressEntity  = AutoMapper.Mapper.Map<Entities.BillingAddressEntity>(billingAddress);

            //Look for existing billing addresses
            var existingBillingAddresses = _productRepo.GetBillingAddresses(user.Id);

            //Check for duplicate billing addresses
            if (existingBillingAddresses != null)
            {
                billingAddressEntity = _productRepo.GetBillingAddresses(user.Id).FirstOrDefault(p => p.AddressLine1 == billingAddress.AddressLine1);

                //If it's a new address, then create it, otherwise just use the existing address
                if (billingAddressEntity == null)
                {
                    billingAddressEntity = AutoMapper.Mapper.Map<Entities.BillingAddressEntity>(billingAddress);
                    //Add the user id to the entity
                    billingAddressEntity.UserId = user.Id;
                    _productRepo.AddBillingAddress(billingAddressEntity);
                }
            }

            //Add the user Id
            billingAddressEntity.UserId = user.Id;

            //Create the payment method
            var pm = new PaymentMethodEntity()
            {
                UserId = user.Id,
                BillingAddress = billingAddressEntity,
                CardNumber = paymentMethod.CardNumber,
                CardType = paymentMethod.CardType,
                CustomCardName = paymentMethod.CustomCardName,
                IsValid = paymentMethod.IsValid,
                SecurityCode = paymentMethod.SecurityCode,
                ExpirationDate = DateTime.Parse(paymentMethod.ExpirationDate)
            };

            _productRepo.AddPaymentMethod(pm);

            //Save all changes to the repo
            if (!_productRepo.Save())
            {
                _logger.LogInformation("The attempt to delete a Product from the database FAILED.");
                return StatusCode(500, "A problem occured while handling your request");
            }

            return Ok(pm.Id);
        }

        [HttpGet("getPaymentMethods")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            //NOTE: You don't return the entities directly, but use the DTO classes instead
            var paymentMethods = _productRepo.GetPaymentMethods(user.Id);

            //Map the product entities to the DTO classes using automapper
            var allPaymentMethods= AutoMapper.Mapper.Map<List<PaymentMethod>>(paymentMethods);

            return Ok(allPaymentMethods);
        }

        [HttpDelete("{paymentMethodId}")]
        public IActionResult DeletePaymentMethod(int paymentMethodId)
        {
            //See if the payment method exists in the repor
            var exists = _productRepo.PaymentMethodExists(paymentMethodId);

            if (exists == false)
            {
                return NotFound();
            }

            //Get the POI entity that's in the DB, so we can delete it.
            var toDeleteEntity = _productRepo.GetPaymentMethod(paymentMethodId);

            if (toDeleteEntity == null)
            {
                return NotFound();
            }

            //Remove the payment method
            _productRepo.DeletePaymentMethod(toDeleteEntity);

            //Since we just removed an entity form the DB, save changes on the repo to ensure the change persists.
            if (!_productRepo.Save())
            {
                _logger.LogInformation("The attempt to delete a Product from the database FAILED.");
                return StatusCode(500, "A problem occured while handling your request");
            }

            return NoContent();
        }
    }
}
