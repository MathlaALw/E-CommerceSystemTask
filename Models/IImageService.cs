
namespace E_CommerceSystem.Models
{
    public interface IImageService
    {
        void DeleteImage(string imageUrl);
        bool IsValidImage(IFormFile imageFile);
        Task<string> SaveImageAsync(IFormFile imageFile, string subFolder = "");
    }
}