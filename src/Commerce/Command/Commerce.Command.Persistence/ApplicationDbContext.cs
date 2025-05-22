using System.Reflection;
using Commerce.Command.Domain.Entities.Order;
using Commerce.Command.Domain.Entities.Product;
using Commerce.Command.Domain.Entities.Shop;
using Microsoft.EntityFrameworkCore;

namespace Commerce.Command.Persistence
{
    /// <summary>
    /// Application database context 
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Application database context 
        /// </summary>
        /// <param name="options">Options for database context</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCancel> OrderCancels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Shop> Shops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}