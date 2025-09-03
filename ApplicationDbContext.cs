using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; } // DbSet for ProductImage entity
        public DbSet<Supplier> Suppliers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();
            // Category
            modelBuilder.Entity<Category>()
              .HasIndex(c => c.Name).IsUnique();
            // Product-Category (Many-to-One)
            modelBuilder.Entity<Product>()
              .HasOne(p => p.Categoty)
              .WithMany(c => c.Products)
              .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Order>()
                        .Property(o => o.Status)
                        .HasConversion<string>();
        }
    }
}
