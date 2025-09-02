using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IProductService
    {
        void AddProduct(Product product);
        void AddProduct(ProductDTO productDTO);
        IEnumerable<Product> GetAllProducts(int pageNumber, int pageSize, string? name = null, decimal? minPrice = null, decimal? maxPrice = null);
        Product GetProductById(int pid);
        Product GetProductByName(string productName);
        void UpdateProduct(int productId, ProductDTO productDTO);
        void UpdateProduct(Product product);
    }
}