using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using Microsoft.Extensions.Logging;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Route("api/cart")]
    public class ShoppingCartController : Controller
    {
        private ILogger<ProductsController> _logger;
        private IProductRepository _productRepo;

        // Constructor to use the logger
        public ShoppingCartController(ILogger<ProductsController> logger, IProductRepository productRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
        }

        [HttpGet("{cartId}", Name = "GetShoppingCart")]
        public IActionResult GetShoppingCart(int cartId)
        {
            //Get the S from the DB. NOTE: You don't return the entities directly, but use the DTO classes instead
            var shoppingCartToReturn = _productRepo.GetShoppingCart(cartId);

            //Map the entity from the DB to a DTO that the front-end can use
            var cartToReturn = AutoMapper.Mapper.Map<ShoppingCart>(shoppingCartToReturn);

            return Ok(cartToReturn);
        }

        [HttpPost("createCart")]
        public IActionResult CreateShoppingCart([FromBody] ShoppingCart cartToAdd)
        {
            //If the data sent cannot be de-serialized into a ShoppingCart, then the cartToAdd will be null
            if (cartToAdd == null)
            {
                return BadRequest(); //The consuming app messed up when sending data, so let them know it was a bad request.
            }

            // See if the ShoppingCart already exists in the DB
            //Todo: Should this just be ID instead?
            var cartExists = _productRepo.ShoppingCartExists(cartToAdd.Id);

            if (cartExists)
            {
                return BadRequest();
            }

            //The product comes from the post body and is of type productForCreation.
            //Since we created a mapping for this in the mapper (see startup.cs), we can create a ShoppingCart entity by mapping it from the ShoppingCart
            var finalCartToAdd = AutoMapper.Mapper.Map<Entities.ShoppingCartEntity>(cartToAdd);
            _productRepo.CreateShoppingCart(finalCartToAdd);

            //Attempt to save the new ShoppingCart to the repo. If it doesn't return an object, then it fails
            if (!_productRepo.Save())
            {
                _logger.LogInformation("There was a problem when trying to create the ShoppingCart");
                return StatusCode(500, "A problem occured while handling your request");
            }

            //Send back a route where the shopping cart can be retrieved
            var newlyCreatedShoppingCart = AutoMapper.Mapper.Map<Models.ShoppingCart>(finalCartToAdd);

            //This returns a 201 (created) response with the URL where the newly created resource can be found.
            //Since we use the Get request to get a ShoppingCart, we're saying that in order to get to this resource, they'll
            //need to use the GetShoppingCart Route (passing in shoppingCart ID)
            //IMPORTANT: The GET route MUST have a name that matches the name in CreatedAtRoute here (e.g. "GetPointOfInterest")
            
            //Todo: Should this be Id instead of cartId?
            return CreatedAtRoute("GetShoppingCart",
                                   new { cartId = newlyCreatedShoppingCart.Id },
                                   newlyCreatedShoppingCart);
        }
    }
}
