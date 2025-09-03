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
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("No image file provided");

            if (!IsValidImage(imageFile))
                throw new ArgumentException("Invalid image file");

        }
    }
