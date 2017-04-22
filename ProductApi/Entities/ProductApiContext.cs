using Microsoft.EntityFrameworkCore;

namespace ProductApi.Entities
{
    public class ProductApiContext : DbContext
    {
        public ProductApiContext(DbContextOptions<ProductApiContext> options): base(options)
        {
            //If the DB is not yet created, then create it with our entity objects.
            //Database.EnsureCreated();
            Database.Migrate();
        }

        //Used to query and save Entities to the DB
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest{ get; set; }
    }
}
