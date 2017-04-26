using System.Collections.Generic;
using ProductApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private ILogger<ProductsController> _logger;
        private IProductRepository _productRepo;
        
        // Constructor to use the logger
        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
        }

        [HttpGet()]
        public IActionResult GetProducts()
        {
            //Get the products from the DB. NOTE: You don't return the entities directly, but use the DTO classes instead
            var productEntities = _productRepo.GetProducts();

            //Map the product entities to the DTO classes using automapper
            var allProducts = AutoMapper.Mapper.Map<IEnumerable<Product>>(productEntities);

            return Ok(allProducts);
        }

        [HttpGet("{id}")]
        //id is automatically set with the id parameters coming from the request URL
        public IActionResult GetProduct(int id)
        {
            var productToReturn = _productRepo.GetProduct(id);

            if (productToReturn == null)
            {
                return NotFound();
            }

            var productResult = AutoMapper.Mapper.Map<CityWithoutPointsOfInterestDto>(productToReturn);
            return Ok(productResult);
        }

        [HttpGet("getproductbyname")]
        //id is automatically set with the id parameters coming from the request URL
        public IActionResult GetProductByName(string name)
        {
            var productToReturn = _productRepo.GetProductByName(name);

            if (productToReturn == null)
            {
                return NotFound();
            }

            var productResult = AutoMapper.Mapper.Map<Product>(productToReturn);
            return Ok(productResult);
        }

        //USE POST TO CREATE. we use the same url route, but just make it a post. Also, we use ProductForCreated (so we can use validation attributes)
        //[FromBody] says to get the Product from the post body and try to de-serialize it to a ProductntsOfInterestForCreation object
        //[HttpPost("{productId}/product")]
        //public IActionResult CreateProduct(int productId, [FromBody] ProductForCreation product)
        //{
        //    //If the data sent cannot be de-serialized into a PointsOfInterestForCreation, the POI will be null
        //    if (product == null)
        //    {
        //        return BadRequest(); //The consuming app messed up when sending data, so let them know it was a bad request.
        //    }

        //    if (product.Description == product.Name)
        //    {
        //        ModelState.AddModelError("Description", "Description and Name can not be the same");
        //    }

        //    //Check for things like the Required or MaxLength Attributes, which can make the model state invalid
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // See if the city that was passed in exists in the database
        //    var theProductExists = _productRepo.ProductExists(productId);

        //    if (!theProductExists)
        //    {
        //        return NotFound();
        //    }

        //    //The product comes from the post body and is of type productForCreation.
        //    //Since we created a mapping for this in the mapper (see startup.cs), we can create a Product entity by mapping it from the ProductForCreation.
        //    var finalproduct = AutoMapper.Mapper.Map<Entities.ProductEntity>(product);

        //    _productRepo.AddproductForCity(productId,finalproduct);

        //    //Attempt to save the new Product to the repo. If it doesn't return an object, then it fails
        //    if (!_productRepo.Save())
        //    {
        //        _logger.LogInformation("There was a problem when trying to save the new Product to the database.");
        //        return StatusCode(500, "A problem occured while handling your request");
        //    }

        //    var newlyCreatedProduct = AutoMapper.Mapper.Map<Models.productDto>(finalproduct);

        //    //This returns a 201 (created) response with the URL where the newly created resource can be found.
        //    //Since we use the Get request to get a Product, we're saying that in order to get to this resource, they'll
        //    //need to use the Getproduct Route (passing in a productId and Product Id that the Get Route needs).
        //    //IMPORTANT: The GET route MUST have a name that matches the name in CreatedAtRoute here (e.g. "Getproduct")
        //    return CreatedAtRoute("Getproduct", 
        //                           new { productId = productId, poiId = newlyCreatedPoi.Id }, 
        //                           newlyCreatedPoi);
        //}

        //USE PUT TO UPDATE
        [HttpPut("{productId}/pointsOfInterest/{poiId}", Name = "Updateproduct")]
        //public IActionResult Updateproduct(int productId, int poiId, 
        //    [FromBody] productForUpdate product)
        //{
        //    //If the data sent cannot be de-serialized into a PointsOfInterestForUpdate, the POI will be null
        //    if (product == null)
        //    {
        //        return BadRequest(); //The consuming app messed up when sending data, so let them know it was a bad request.
        //    }

        //    if (product.Description == product.Name)
        //    {
        //        ModelState.AddModelError("Description", "Description and Name can not be the same");
        //    }

        //    //Check for things like the Required or MaxLength Attributes, which can make the model state invalid
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // See if the city that was passed in exists in the collection
        //    var doesCityExist = _productRepo.CityExists(productId);

        //    if (doesCityExist == false)
        //    {
        //        return NotFound();
        //    }

        //    //Get the POI entity that's in the DB, so we can update it.
        //    var productEntity = _productRepo.GetproductForCity(productId, poiId);

        //    if (productEntity == null)
        //    {
        //        return NotFound();
        //    }

        //    // The first argument is the new Object you're using to overwrite the existing entity. The second argument is the existing entity in the DB
        //    AutoMapper.Mapper.Map(product, productEntity); //This will overwrite the productEntity with the new product

        //    if (!_productRepo.Save())
        //    {
        //        _logger.LogInformation("There was a problem when trying to update an existing POI in the database.");
        //        return StatusCode(500, "A problem occured while handling your request");
        //    }

        //    //Since we're just updating
        //    return NoContent();
        //}

        //USE PATCH TO MAKE PARTIAL UPDATES
        //[HttpPatch("{productId}/pointsOfInterest/{poiId}")]
        //public IActionResult PartiallyUpdateproduct(int productId, int poiId,
        //    [FromBody] JsonPatchDocument<productForUpdate> patchDoc )
        //{

        //    //If the data sent cannot be de-serialized into a PointsOfInterestForUpdate, the POI will be null
        //    if (patchDoc == null)
        //    {
        //        return BadRequest(); //The consuming app messed up when sending data, so let them know it was a bad request.
        //    }

        //    // See if the city that was passed in exists in the collection
        //    var doesCityExist = _productRepo.CityExists(productId);

        //    if (doesCityExist == false)
        //    {
        //        return NotFound();
        //    }

        //    //Get the POI entity that's in the DB, so we can update it.
        //    var productEntity = _productRepo.GetproductForCity(productId, poiId);

        //    if (productEntity == null)
        //    {
        //        return NotFound();
        //    }

        //    //Map the entitiy to a DTO, so we can then change the DTO properties
        //    var poiToPatch = AutoMapper.Mapper.Map<productForUpdate>(productEntity);

        //    //Apply the patch document to the poiToPatch. We pass in the ModelState to make sure it is valid before attempting to patch
        //    patchDoc.ApplyTo(poiToPatch, ModelState);

        //    //This ModelState actually refers to the patchDoc, NOT to the POI, so we need to validate that seperately below
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //Rechecks validation rules on the POI now that the POI has been patched. 
        //    TryValidateModel(poiToPatch);

        //    //If there were any errors when re-validation the poi (after the patch is applied), they will be added to the model state.
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //Now that poiToPatch has been updated to the new values from the patchDoc, 
        //    //overwirte the poi entity in our DB (i.e. the source is the poiToPatch DTO, whichc overwrites the destination (existing) entity (productEntity))
        //    AutoMapper.Mapper.Map(poiToPatch, productEntity);

        //    //Since we just changed an entity, save changes on the repo to ensure the change persists.
        //    if (!_productRepo.Save())
        //    {
        //        _logger.LogInformation("There was a problem when trying to update an existing POI in the database.");
        //        return StatusCode(500, "A problem occured while handling your request");
        //    }

        //    return NoContent();
        //}

        // DELETE
        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            //See if the product that was passed in exists in the collection
            var productExists = _productRepo.ProductExists(productId);

            if (productExists == false)
            {
                return NotFound();
            }

            //Get the POI entity that's in the DB, so we can delete it.
            var toDeleteEntity = _productRepo.GetProduct(productId);

            if (toDeleteEntity == null)
            {
                return NotFound();
            }

            //Remove the Product
            _productRepo.DeleteProduct(toDeleteEntity);

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
