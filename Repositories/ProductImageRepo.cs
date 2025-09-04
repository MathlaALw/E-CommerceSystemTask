using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public class ProductImageRepo : IProductImageRepo 
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
                return _context.ProductImages // Retrieve images for the specified product
                    .Where(pi => pi.PID == productId) // Filter by product ID
                    .OrderBy(pi => pi.DisplayOrder) // Order by display order
                    .ToList(); // Convert to list
            }
            catch (Exception ex) // Catch any exceptions that occur during the database operation
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        public void UpdateProductImage(ProductImage productImage) // Method to update an existing product image
        {
            try // Try-catch block to handle potential exceptions
            {
                _context.ProductImages.Update(productImage); // Update the product image in the context
                _context.SaveChanges();
            }
            catch (Exception ex) // Catch any exceptions that occur during the database operation
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public void SetMainImage(int productId, int imageId) // Method to set a specific image as the main image for a product
        {
            try // Try-catch block to handle potential exceptions
            {
                // Reset all images as not main
                var images = _context.ProductImages.Where(pi => pi.PID == productId).ToList();
                foreach (var image in images) // Loop through each image
                {
                    image.IsMain = (image.ImageId == imageId); // Set IsMain to true for the specified image, false for others
                }

                // Update product's main image URL
                var product = _context.Products.Find(productId); // Retrieve the product from the database
                var mainImage = _context.ProductImages.FirstOrDefault(pi => pi.ImageId == imageId); // Retrieve the main image
                if (product != null && mainImage != null) // If both product and main image exist
                {
                    product.MainImageUrl = mainImage.ImageUrl; // Update the product's main image URL
                   
                }

                _context.SaveChanges();
            }
            catch (Exception ex) // Catch any exceptions that occur during the database operation
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }
        private void DeleteImageFile(string imageUrl) // Method to delete the physical image file from the server
        {
            try // Try-catch block to handle potential exceptions
            {
                if (!string.IsNullOrEmpty(imageUrl) && File.Exists(imageUrl)) // Check if the file exists
                {
                    File.Delete(imageUrl);
                }
            }
            catch (Exception ex) // Catch any exceptions that occur during file deletion
            {
                // Log error but don't throw to avoid affecting the main operation
                Console.WriteLine($"Error deleting image file: {ex.Message}");
            }
        }
    }




}
    
