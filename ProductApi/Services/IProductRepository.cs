using System.Collections.Generic;
using ProductApi.Entities;

namespace ProductApi.Services
{
    public interface IProductRepository
    {
        //Get all the products from the DB
        IEnumerable<ProductEntity> GetProducts();

        //Get a single product
        ProductEntity GetProduct(int productId);

        //Get a product by it's name
        ProductEntity GetProductByName(string name);

        //Check that a Product exists
        bool ProductExists(int productId);

        //Add a Product
        void AddProduct();

        //Required to save new entities to the database context when they are created.
        bool Save();

        void DeleteProduct(ProductEntity productToDelete);
    }
}
