using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace PrayashStore.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private int count = 0;
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            count = count + 1;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}