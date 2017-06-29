using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductApi.Entities
{
    public class ProductApiContext : IdentityDbContext<User>
    {
        public ProductApiContext(DbContextOptions<ProductApiContext> options): base(options)
        {
            //If the DB is not yet created, then create it with our entity objects.
            //DO NOT use this when starting a new project! Use Database.migrate instead.
            //Database.EnsureCreated();

            //This should really be used, but don't have the time to fix it given the current deadline.
            Database.Migrate();
        }

        //Used to query and save Entities to the DB
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }

        public DbSet<ShippingAddressEntity> ShippingAddresses { get; set; }
        public DbSet<BillingAddressEntity> BillingAddresses { get; set; }
        
        public DbSet<PaymentMethodEntity> PaymentMethods { get; set; }
        public DbSet<ShippingMethodTypeEntity> ShippingTypes { get; set; }

        public DbSet<ReportEntity> Reports { get; set; }

        //Todo: Remove and update the DB Scheme with a new migration.
        //Note: ShoppingCarts are no longer used. CartItems use their shoppingCartId to track which cart should be used.
        public DbSet<ShoppingCartEntity> ShoppingCarts { get; set; }
    }
}
