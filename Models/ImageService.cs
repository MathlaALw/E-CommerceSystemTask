using System;

namespace E_CommerceSystem.Models
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        private static readonly HashSet<string> AllowedExtensions =
            new(new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" }, StringComparer.OrdinalIgnoreCase);

        private static readonly HashSet<string> AllowedContentTypes =
            new(new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" }, StringComparer.OrdinalIgnoreCase);

        private const long MaxBytes = 5 * 1024 * 1024; // 5 MB

        public ImageService(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        // ✅ Correct signature: IFormFile, not string
        public async Task<string> SaveImageAsync(IFormFile imageFile, string subFolder = "")
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("No image file provided.", nameof(imageFile));

            if (!IsValidImage(imageFile))
                throw new ArgumentException("Invalid image file type or size.", nameof(imageFile));

            // Ensure webroot
            var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // /wwwroot/uploads/{subFolder}
            var uploadsFolder = Path.Combine(webRoot, "uploads", subFolder ?? string.Empty);
            Directory.CreateDirectory(uploadsFolder);

            // Unique filename with original extension
            var ext = Path.GetExtension(imageFile.FileName);
            var uniqueFileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save asynchronously
            await using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true))
            {
                await imageFile.CopyToAsync(stream); // 
                                                     // Return a web-relative URL: /uploads[/subFolder]/file
                var sub = string.IsNullOrWhiteSpace(subFolder) ? "" : $"{subFolder.Trim('/')}/";
                return $"/uploads/{sub}{uniqueFileName}";
            }
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return;

            var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            // Convert URL (/uploads/...) to physical path
            var relative = imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(webRoot, relative);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public bool IsValidImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return false;
            if (imageFile.Length > MaxBytes) return false;

            var ext = Path.GetExtension(imageFile.FileName);
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext)) return false;

            var contentType = imageFile.ContentType ?? "";
            if (!AllowedContentTypes.Contains(contentType)) return false;

            return true;
        }
    }



}





    
