using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductApi.Entities
{
    public class ProductApiContext : IdentityDbContext<User>
    {
        public ProductApiContext(DbContextOptions<ProductApiContext> options): base(options)
        {
            //If the DB is not yet created, then create it with our entity objects.
            //Database.EnsureCreated();
            Database.Migrate();
        }

        //Used to query and save Entities to the DB
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }
        public DbSet<ShoppingCartEntity> ShoppingCarts { get; set; }
    }
}
