using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IProductService
    {
        void AddProduct(Product product);
        void AddProduct(ProductDTO productDTO);
        Task AddProductWithImages(ProductDTO productDTO);
        void DeleteProductImage(int imageId);
        void DeleteProductImages(int productId);
        IEnumerable<Product> GetAllProducts(int pageNumber, int pageSize, string? name = null, decimal? minPrice = null, decimal? maxPrice = null);
        Product GetProductById(int pid);
        Product GetProductByName(string productName);
        IEnumerable<ProductImage> GetProductImages(int productId);
        void SetMainProductImage(int productId, int imageId);
        void UpdateProduct(int productId, ProductDTO productDTO);
        void UpdateProduct(Product product);
        Task UpdateProductWithImages(int productId, ProductDTO productDTO);
    }
}