using System.Collections.Generic;
using System.Linq;
using ProductApi.Entities;

namespace ProductApi.Services
{
    public class ProductRepository : IProductRepository
    {
        private ProductApiContext _context;

        public ProductRepository(ProductApiContext context)
        {
            _context = context;
        }

        public bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }

        public IEnumerable<ProductEntity> GetProducts()
        {
            //Order the Products by name
            return _context.Products.OrderBy(p => p.Name).ToList();
        }

        //Use a boolean to allow the consumer to decide whether to retrieve the POIs instead of always showing them.
        public ProductEntity GetProduct(int productId)
        {
            //If we're not including the POIs, then jus return the Product
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        //Used to persist changes in the SQL DB (i.e. when you create or delete something from the DB, you must call save on the DB context).
        public bool Save()
        {
            //SaveChanges returns the number of entities that were changed, if any.
            //In this case, we'll just return true if at least one entity was updated/removed.
            return (_context.SaveChanges() >= 0);
        }

        public ProductEntity GetProductByName(string name)
        {
            //If we're not including the POIs, then just return the Product
            return _context.Products.FirstOrDefault(p => p.Name == name);
        }

        public void DeleteProduct(ProductEntity productToDelete)
        {
            //PoinstOfInterest is a DBSet. To remove a POI from the set, we use Remove().
            _context.Products.Remove(productToDelete);
        }

        public void AddProduct()
        {
            throw new System.NotImplementedException();
        }
    }
}
