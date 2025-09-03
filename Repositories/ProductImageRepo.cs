using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public class ProductImageRepo
    {
        private readonly ApplicationDbContext _context; // Database context
        public ProductImageRepo(ApplicationDbContext context) // Constructor with dependency injection
        {
            _context = context;
        }
        public void AddProductImage(ProductImage productImage) // Method to add a new product image
        {
            try
            {
                _context.ProductImages.Add(productImage); // Add the product image to the context
                _context.SaveChanges();
            }


    }
    }
