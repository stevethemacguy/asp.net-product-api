using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ProductApi.Entities;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Route("api/checkout")]
    public class CheckoutController : Controller
    {

        private IProductRepository _productRepo;
        private readonly UserManager<User> _userManager;
        private ILogger<ProductsController> _logger;

        public CheckoutController(IProductRepository productRepo, UserManager<User> userManager, ILogger<ProductsController> logger)
        {
            _productRepo = productRepo;
            _userManager = userManager;
            _logger = logger;
        }

        //Creates a new Order (and order items) with the CartItems that match the logged in user, adds the order to the DB, and then removes the cartItems from the DB.
        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] AddressInformation addressInformation)
        {
            //Check for things like the Required or MaxLength Attributes, which can make the model state invalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the Logged in user.
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var shippingAddress = addressInformation.ShippingAddress;
            var billingAddress = addressInformation.BillingAddress;

            //Map the shipping address object received in the request to an Entity
            var finalShippingAddress = AutoMapper.Mapper.Map<Entities.ShippingAddressEntity>(shippingAddress);

            //See if the ShippingAddress passed in already exists in the database
            var shippingAddressExists = _productRepo.ShippingAddressExists(finalShippingAddress.Id);

            //If the shippingaddress doesn't yet exist, create it (otherwise use the existing shipping address)
            if (!shippingAddressExists)
            {
                //Add the user id to the entity
                finalShippingAddress.UserId = user.Id;
                _productRepo.AddShippingAddress(finalShippingAddress);
            }

            //Map the billing address object received in the request to an Entity
            var finalBillingAddress = AutoMapper.Mapper.Map<Entities.BillingAddressEntity>(billingAddress);

            //See if the villingAddress passed in already exists in the database
            var billingAddressExists = _productRepo.BillingAddressExists(finalBillingAddress.Id);

            //If the billingAddress doesn't yet exist, create it, otherwise use the existing address
            if (!billingAddressExists)
            {
                //Add the user id to the entity
                finalBillingAddress.UserId = user.Id;
                _productRepo.AddBillingAddress(finalBillingAddress);
            }

            //Todo: Create a new Payment or use an existing one

            //Create the Order (OrderItems and prices will be added/calculated later). 
            var order = new OrderEntity()
            {
                UserId = user.Id,   
                BillingAddress = finalBillingAddress,
                ShippingAddress = finalShippingAddress,
                DateUpdated = null,                     //Future enhancement
                PaymentMethodUsed = null,               //Future enhancement
                ShippingMethodType = null,              //Future enhancement
                SalesTaxRate = new decimal(0.0),        //Future enhancement
                TotalShippingCost = new decimal(0.0),   //Future enhancement
                TotalTax = new decimal(0.0),            //Future enhancement
                TotalCost = new decimal(0.0)            //Will be calculated later
            };

            //Get the cart items that match the logged in User Id
            var cartItemEntities = _productRepo.GetShoppingCartItems(user.Id).ToList();

            if (cartItemEntities == null || cartItemEntities.Any() == false)
            {
                return BadRequest(ModelState);
            }

            //For each product "in" the user's cart (i.e. each CartItem), create a new OrderItem.
            //OrderItems are used to extend and "save" a snapshot of the Product information at the time the product is purchased
            foreach (var cartItem in cartItemEntities)
            {
                //Get the associated product
                var theProduct = _productRepo.GetProduct(cartItem.ProductId);
                var basePrice = Convert.ToDecimal(theProduct.Price);

                //Future Functionality
                var discount = new decimal(0.0);

                //Calculate the Final cost of the product after applying any individual discount. For now just use the base price
                decimal finalCost = basePrice - discount;

                //Create the new OrderItem using information from the product
                var orderItemToAdd = new OrderItemEntity()
                {
                    Product = cartItem.Product,
                    ProductId = cartItem.ProductId,
                    SavedProductDescription = theProduct.Name,
                    Quantity = 1,   //Future Functionality
                    BasePrice = basePrice,
                    Discount = discount,
                    FinalCost = finalCost
                };

                //Add the OrderItem to the Order. This also sets the 'Order' navigation property on the OrderItem
                order.AddOrderItem(orderItemToAdd);

                //Remove the Cart item since the user is checking out
                _productRepo.DeleteCartItem(cartItem);
            }

            //Todo: Subtract Discount from the final price
            //Todo: Add Sales Tax
            //Todo: Calculate Shipping Cost
            order.ShippingMethodType = null; //Future functionality
            
            //Todo: Calculate The Total Overall Cost

            //Calculate the final the cost of the order item. See above todos
            order.TotalCost = order.OrderItems.Sum(x => x.FinalCost * x.Quantity);

            //Add the new order to the repo
            _productRepo.AddOrder(order);

            //Save all changes to the repo
            if (!_productRepo.Save())
            {
                _logger.LogInformation("The attempt to delete a Product from the database FAILED.");
                return StatusCode(500, "A problem occured while handling your request");
            }

            return Ok();
        }
    }
}
