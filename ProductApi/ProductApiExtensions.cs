using ProductApi.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ProductApi
{
    //ProductApiExtensions is used to seed the local SQL DB with data. 
    //This is a "code-first" approach (i.e. we"re creating new DB entities using C# classes/objects)
    public static class ProductApiExtensions
    {
        //"this" here means that EnsureSeedDataForContext extends ProductApiContext
        public static void EnsureSeedDataForContext(this ProductApiContext context)
        {
            //If products already exists in the DB, then we don't need to seed the DB with data, so exit.
            if (context.Products.Any())
            {
                return;
            }

            //Create the seed data
            var products = new List<ProductEntity>()
            {
                new ProductEntity()
                {
                    Name = "Apple",
                    Price = 1.50,
                    ImageUrl = "/apple.png"
                },
                new ProductEntity()
                {
                    Name = "Pencil",
                    Price = .50,
                    ImageUrl = "/pencil.png"
                },
                new ProductEntity()
                {
                    Name = "Xbox",
                    Price = 199.99,
                    ImageUrl = "/xbox.png"
                },
                new ProductEntity()
                {
                    Name = "Sony Camera",
                    Price = 60.00,
                    ImageUrl = "/camera.png"
                },
                new ProductEntity()
                {
                    Name = "LOTR Trilogy; Blue Ray",
                    Price = 49.99,
                    ImageUrl = "/dvd.png"
                },
                new ProductEntity()
                {
                    Name = "Band aids",
                    Price = 2.50,
                    ImageUrl = "/band.png"
                },
                new ProductEntity()
                {
                    Name = "Apple pie",
                    Price = 4.00,
                    ImageUrl = "/pie.png"
                },
                new ProductEntity()
                {
                    Name = "Tennis ball (x10)",
                    Price = 5.49,
                    ImageUrl = "/ball.png"
                },
                new ProductEntity()
                {
                    Name = "Diamond necklace",
                    Price = 20000,
                    ImageUrl = "/diamond.png"
                },
                new ProductEntity()
                {
                    Name = "Hand grenade",
                    Price = 15.00,
                    ImageUrl = "/grenade.png"
                },
                new ProductEntity()
                {
                    Name = "Printer",
                    Price = 150.00,
                    ImageUrl = "/printer.png"
                },
                new ProductEntity()
                {
                    Name = "Monitor",
                    Price = 335.00,
                    ImageUrl = "/monitor.png"
                },
                new ProductEntity()
                {
                    Name = "Book",
                    Price = 5.00,
                    ImageUrl = "/book.png"
                },
                new ProductEntity()
                {
                    Name = "Couch",
                    Price = 110.00,
                    ImageUrl = "/couch.gif"
                },
                new ProductEntity()
                {
                    Name = "Silverware",
                    Price = 150.00,
                    ImageUrl = "/silver.png"
                },
                new ProductEntity()
                {
                    Name = "Watch",
                    Price = 189.00,
                    ImageUrl = "/watch.png"
                },
                new ProductEntity()
                {
                    Name = "Flowers",
                    Price = 15.00,
                    ImageUrl = "/flowers.png"
                },
                new ProductEntity()
                {
                    Name = "Cup",
                    Price = 40,
                    ImageUrl = "/cup.png"
                }
            };

            var shoppingCarts = new List<ShoppingCartEntity>()
            {
                new ShoppingCartEntity()
                {
                    CartItems = new List<CartItemEntity>()
                }
            };

            //Track the new entities
            context.Products.AddRange(products);
            context.ShoppingCarts.AddRange(shoppingCarts);

            //Since we just added entities to the DB, save changes so that they persist after the application closes.
            context.SaveChanges();
        }
    }
}
