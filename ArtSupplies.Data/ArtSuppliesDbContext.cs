using ArtSupplies.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArtSupplies.Data
{
    public class ArtSuppliesDbContext : DbContext
    {
        public ArtSuppliesDbContext(DbContextOptions<ArtSuppliesDbContext> options)
            : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<ShoppingCart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.CartItems)
                .WithMany(p => p.Carts)
                .UsingEntity<CartItem>
                (ci => ci.HasOne<Product>().WithMany(),
                 ci => ci.HasOne<ShoppingCart>().WithMany())
                .Property(ci => ci.Quantity)
                .HasDefaultValueSql("0");
        }
    }
}
