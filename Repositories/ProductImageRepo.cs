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
            catch (Exception ex) // Catch any exceptions that occur during the database operation
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public void DeleteProductImage(int imageId) // Method to delete a product image by its ID
        {
            try
            {
                var image = GetProductImageById(imageId); // Retrieve the image from the database
                if (image != null)
                {
                    _context.ProductImages.Remove(image); // Remove the image from the context
                    _context.SaveChanges();

                    // Delete the physical file
                    DeleteImageFile(image.ImageUrl);
                }
            }
            catch (Exception ex) // Catch any exceptions that occur during the database operation
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public ProductImage GetProductImageById(int imageId) // Method to retrieve a product image by its ID
        {
            try // Try-catch block to handle potential exceptions
            {
                return _context.ProductImages.FirstOrDefault(pi => pi.ImageId == imageId); // Retrieve the image from the database
            }
            catch (Exception ex) // Catch any exceptions that occur during the database operation
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public IEnumerable<ProductImage> GetProductImages(int productId) // Method to retrieve all images for a specific product
        {
            try // Try-catch block to handle potential exceptions
            {
                return _context.ProductImages
                    .Where(pi => pi.PID == productId)
                    .OrderBy(pi => pi.DisplayOrder)
                    .ToList();
            }






    }
    }
