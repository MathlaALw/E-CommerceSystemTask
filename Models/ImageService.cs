namespace E_CommerceSystem.Models
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment; // To access web hosting environment details
        private readonly IConfiguration _configuration; // To access configuration settings

        public ImageService(IWebHostEnvironment environment, IConfiguration configuration) // Constructor with dependency injection
        {
            _environment = environment; // Access to web hosting environment
            _configuration = configuration; // Access to configuration settings
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string subFolder = "") // Method to save an image file asynchronously
        {
            if (imageFile == null || imageFile.Length == 0) // Check if the file is null or empty
                throw new ArgumentException("No image file provided");

            if (!IsValidImage(imageFile)) // Validate the image file type
                throw new ArgumentException("Invalid image file");

            // Create uploads directory if it doesn't exist
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", subFolder);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);



        }
    }
