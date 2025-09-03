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
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", subFolder); // Define the uploads folder path
            if (!Directory.Exists(uploadsFolder)) // Check if the directory exists
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique filename
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            // Return relative path for web access
            return $"/uploads/{subFolder}/{uniqueFileName}";
        }

        public void DeleteImage(string imageUrl) // Method to delete an image file
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            // Convert URL to physical path
            var physicalPath = imageUrl.StartsWith("/") // Check if the URL is relative
                ? Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/')) // Trim leading slash for correct path
                : imageUrl;

            if (File.Exists(physicalPath)) // Check if the file exists
            {
                File.Delete(physicalPath); // Delete the file
            }
        }

        public bool IsValidImage(IFormFile imageFile) // Method to validate image file types
        {
            if (imageFile == null) // Check if the file is null
                return false;

            // Check file size (max 5MB)
            if (imageFile.Length > 5 * 1024 * 1024)
                return false;

            // Check file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }; // Allowed image extensions
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower(); // Get file extension in lowercase
            if (!allowedExtensions.Contains(fileExtension)) // Check if the extension is allowed
                return false;

            // Check content type
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            if (!allowedContentTypes.Contains(imageFile.ContentType.ToLower()))
                return false;










        }
    }
