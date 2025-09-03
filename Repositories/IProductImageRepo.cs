using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IProductImageRepo
    {
        void AddProductImage(ProductImage productImage);
        void DeleteProductImage(int imageId);
        ProductImage GetProductImageById(int imageId);
        IEnumerable<ProductImage> GetProductImages(int productId);
        void SetMainImage(int productId, int imageId);
        void UpdateProductImage(ProductImage productImage);
    }
}