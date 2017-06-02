using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using Microsoft.Extensions.Logging;
using ProductApi.Entities;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Authorize]
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

        [HttpGet("{cartId}")]
        public IActionResult GetShoppingCart(int cartId)
        {
            //Get the Shopping Cart from the DB.
            //Multiple carts aren't currently supported, so we return all cartItems that match the cartID (instead of finding a shopping cart entity)
            var cartItemEntities = _productRepo.GetShoppingCartItems(cartId);

            //Map the CartItem entities to the DTO classes using automapper
            var allCartItems= AutoMapper.Mapper.Map<IEnumerable<CartItem>>(cartItemEntities);

            return Ok(allCartItems);
        }

        [HttpGet("{cartId}/getproducts", Name = "GetShoppingCartProducts")]
        public IActionResult GetShoppingCartProducts(int cartId)
        {
            //Get the cart items
            var cartItemEntities = _productRepo.GetShoppingCartItems(cartId);

            //Map the CartItem entities to the DTO classes using automapper
            var allCartItems = AutoMapper.Mapper.Map<IEnumerable<CartItem>>(cartItemEntities);

            //Build a list of products from the cart items and return that to the FE
            List<Product> allProducts = new List<Product>();

            foreach (var cartItem in allCartItems)
            {
                //Get the associated product
                var productEntity = _productRepo.GetProduct(cartItem.ProductId);

                //Convert the productEntity into a DTO that the FE can use.
                var productToReturn = AutoMapper.Mapper.Map<Product>(productEntity);

                //Add it to the list to return
                allProducts.Add(productToReturn);
            }

            return Ok(allProducts);
        }

        //[HttpGet("{cartId}", Name = "GetShoppingCart")]
        //public IActionResult GetShoppingCart(int cartId)
        //{
        //    //Get the Shopping Cart from the DB. NOTE: You don't return the entities directly, but use the DTO classes instead
        //    var shoppingCartToReturn = _productRepo.GetShoppingCart(cartId);

        //    //Map the entity from the DB to a DTO that the front-end can use
        //    var cartToReturn = AutoMapper.Mapper.Map<ShoppingCart>(shoppingCartToReturn);

        //    return Ok(cartToReturn);
        //}

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

        //[HttpGet("{cartId}/addproduct/{productId}", Name = "AddProductToCart")]
        //public IActionResult AddProductToCart(int cartId, int productId)
        //{
        //    //Check that the cart exists
        //    var cartExists = _productRepo.ShoppingCartExists(cartId);

        //    if (cartExists == false)
        //    {
        //        return BadRequest("The shopping cart with id" + cartId + "does not exists");
        //    }

        //    //Get the product from the DB
        //    var productEntity = _productRepo.GetProduct(productId);

        //    //Map the entity to a DTO, so we can created one from the Api request
        //    var productDto = AutoMapper.Mapper.Map<Product>(productEntity);

        //    //var productEntity = _productRepo.GetProducts()
        //    //    .SingleOrDefault(p => p.Id == productId);

        //    //var productDto = AutoMapper.Mapper.Map<Product>(productEntity);
        //    //Create an ItemDTO. Should maybe be a for creation one
        //    var cartItemToAdd = new CartItem
        //    {
        //        ShoppingCartId = cartId,
        //        Quantity = 1,
        //        ProductId = productId,
        //        Product = productDto,
        //    };

        //    //Create a cartItem entity to the DB..don't add it to the cart yet
        //    var finalCartItem = AutoMapper.Mapper.Map<Entities.CartItemEntity>(cartItemToAdd);
        //    _productRepo.CreateCartItem(finalCartItem);

        //    //Attempt to save the DB. Todo: redundent?
        //    if (!_productRepo.Save())
        //    {
        //        _logger.LogInformation("There was a problem when trying to add the product to the ShoppingCart");
        //        return StatusCode(500, "A problem occured while handling your request");
        //    }

        //    //Add the newly created item to the cart. Is this needed? I'm not sure
        //    _productRepo.AddItemToCart(cartId, finalCartItem);

        //    //Get the shoppingCart that we just updated, so we can overwrite the old cart 
        //    //Get the POI entity that's in the DB, so we can update it.
        //    //var cartToUpdate = _productRepo.GetShoppingCart(cartId);

        //    //var temp = new ShoppingCartEntity();
        //    //temp.CartItems.Add(finalCartItem);
        //    //// The first argument is the new Object you're using to overwrite the existing entity. The second argument is the existing entity in the DB
        //    //_productRepo.CreateShoppingCart(temp);

        //    //Attempt to save the DB
        //    if (!_productRepo.Save())
        //    {
        //        _logger.LogInformation("There was a problem when trying to add the product to the ShoppingCart");
        //        return StatusCode(500, "A problem occured while handling your request");
        //    }

        //    return Ok();
        //}


        [HttpPost("{cartId}/addproduct/{productId}", Name = "AddProductToSingleCart")]
        public IActionResult AddProductToSingleCart(int cartId, int productId)
        {

            // See if the product that was passed exists in the database
            var theProductExists = _productRepo.ProductExists(productId);

            if (!theProductExists)
            {
                ModelState.AddModelError("Invalid Product ID", "A Product with this ID does not exists.");
                return BadRequest(ModelState);
            }

            var currentProductEntity = _productRepo.GetProduct(productId);

            //Create an ItemDTO from the productEntity.
            var cartItemToAdd = new CartItemEntity()
            {
                ShoppingCartId = cartId,
                Quantity = 1,
                ProductId = currentProductEntity.Id,
            };

            //Add the cartItem entity to the DB. Current there is no concept of a "shopping cart".
            //However, the CartEntity is aware of it's cart id (we only need one cart, so the cart ID should always be one)
            _productRepo.CreateCartItem(cartItemToAdd);

            if (!_productRepo.Save())
            {
                _logger.LogInformation("There was a problem when trying to add the product to the ShoppingCart");
                return StatusCode(500, "A problem occured while handling your request");
            }

            return Ok();
        }


        [HttpDelete("{cartId}/removeproduct/{productId}")]
        public IActionResult RemoveProductFromSingleCart(int cartId, int productId)
        {
            //See if the productID matches a cartItem (i.e. whether the product is "in the cart")
            var cartItemEntities = _productRepo.GetShoppingCartItems(cartId);

            var itemInCart = cartItemEntities.FirstOrDefault(c => c.ProductId == productId);

            if (itemInCart == null)
            {
                return NotFound();
            }

            //Delete the CartItemEntity
            _productRepo.DeleteCartItem(itemInCart);

            //Since we just removed an entity form the DB, save changes on the repo to ensure the change persists.
            if (!_productRepo.Save())
            {
                _logger.LogInformation("The attempt to delete a Product from the database FAILED.");
                return StatusCode(500, "A problem occured while handling your request");
            }

            return NoContent();
        }

        //Works, but doesn't add to the shopping cart in SQL fro some reason
        //[HttpGet("{cartId}/addproduct/{productId}", Name = "AddProductToCart")]
        //public IActionResult AddProductToCart(int cartId, int productId)
        //{
        //    //Check that the cart exists
        //    var cartExists = _productRepo.ShoppingCartExists(cartId);

        //    if (cartExists == false)
        //    {
        //        return BadRequest("The shopping cart with id" + cartId + "does not exists");
        //    }

        //    //Add the item to the cart
        //    _productRepo.AddItemToCart(cartId, productId);

        //    //Attempt to save the DB
        //    if (!_productRepo.Save())
        //    {
        //        _logger.LogInformation("There was a problem when trying to add the product to the ShoppingCart");
        //        return StatusCode(500, "A problem occured while handling your request");
        //    }

        //    return Ok();
        //}

    }
}
