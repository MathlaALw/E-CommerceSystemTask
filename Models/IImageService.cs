
namespace E_CommerceSystem.Models
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string subFolder = "");
        void DeleteImage(string imageUrl);
        bool IsValidImage(IFormFile imageFile);
    }
}