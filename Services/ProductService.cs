using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using E_CommerceSystem.Exceptions;
using E_CommerceSystem.Middleware;

namespace E_CommerceSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IProductImageRepo _productImageRepo; // For managing product images
        private readonly IImageService _imageService; // For handling image file operations
        private readonly IMapper _mapper;
        private readonly ISupplierRepo _supplierRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IAppLogger<ProductService> _logger;

        public ProductService(IProductRepo productRepo, IProductImageRepo productImageRepo, IImageService imageService, IMapper mapper, ISupplierRepo supplierRepo,
            ICategoryRepo categoryRepo, IAppLogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _productImageRepo = productImageRepo;
            _imageService = imageService;
            _mapper = mapper;
            _supplierRepo = supplierRepo;
            _categoryRepo = categoryRepo;
            _logger = logger;
        }



        public IEnumerable<Product> GetAllProducts(int pageNumber, int pageSize, string? name = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            // Base query
            var query = _productRepo.GetAllProducts();

            // Apply filters
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Pagination
            var pagedProducts = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedProducts;

        }
        public Product GetProductById(int pid)
        {
            try
            {
                _logger.LogInformation("Getting product by ID: {ProductId}", pid);

                var product = _productRepo.GetProductById(pid);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", pid);
                    throw new KeyNotFoundException($"Product with ID {pid} not found.");
                }

                _logger.LogInformation("Product with ID {ProductId} found: {ProductName}", pid, product.ProductName);
                return product;
            }
            catch (Exception ex) when (ex is not NotFoundException)
            {
                _logger.LogError(ex, "Error occurred while getting product by ID: {ProductId}", pid);
                throw new AppException($"Database error: {ex.Message}", 500, false);
            }
        }
        // Add Product DTO
        public void AddProduct(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            _productRepo.AddProduct(product);
        }
        // Add Product Object
        public void AddProduct(Product product)
        {
            _productRepo.AddProduct(product);
        }
        // Update Product DTO
        public void UpdateProduct(int productId, ProductDTO productDTO)
        {

            var existingProduct = _productRepo.GetProductById(productId);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            _mapper.Map(productDTO, existingProduct);
            _productRepo.UpdateProduct(existingProduct);
        }
        // Update Product Object
        public void UpdateProduct(Product product)
        {
            try
            {
                var existingProduct = _productRepo.GetProductById(product.PID);
                if (existingProduct == null)
                    throw new KeyNotFoundException($"Product with ID {product.PID} not found.");

                // Check if the RowVersion matches (concurrency check)
                if (!existingProduct.RowVersion.SequenceEqual(product.RowVersion))
                    throw new DbUpdateConcurrencyException("The product was modified by another user. Please refresh and try again.");

                _productRepo.UpdateProduct(product);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw;
            }
        }
        public Product GetProductByName(string productName)
        {
            var product = _productRepo.GetProductByName(productName);
            if (product == null)
                throw new KeyNotFoundException($"Product with Nmae {productName} not found.");
            return product;
        }

        public async Task AddProductWithImages(ProductDTO productDTO, int supplierId, int categoryId) // Add product with images
        {
            try
            {
                var product = _mapper.Map<Product>(productDTO); // Map DTO to entity
                product.SupplierId = supplierId;
                product.CategoryId = categoryId;
                product.RowVersion = new byte[] { 0 }; // Initialize RowVersion for concurrency on new
                product.ProductName = productDTO.ProductName;
                product.Description = productDTO.Description;
                product.Price = productDTO.Price;

                // Add the product first
                _productRepo.AddProduct(product);
                // Validate relations
                var supplier = _supplierRepo.GetSupplierById(supplierId);
                if (supplier is null) throw new KeyNotFoundException($"Supplier {supplierId} not found.");

                var category = _categoryRepo.GetCategoryById(categoryId);
                if (category is null) throw new KeyNotFoundException($"Category {categoryId} not found.");


                // Handle main image
                if (!string.IsNullOrEmpty(productDTO.MainImageUrl))
                {
                    var mainImage = new ProductImage
                    {
                        PID = product.PID,
                        ImageUrl = productDTO.MainImageUrl,
                        IsMain = true,
                        DisplayOrder = 0
                    };

                    _productImageRepo.AddProductImage(mainImage); // Add image to the repository

                    // Update product's main image URL
                    product.MainImageUrl = productDTO.MainImageUrl; // Update main image URL
                    _productRepo.UpdateProduct(product); // Update product to save main image URL
                }
                // Handle additional images
                if (productDTO.AdditionalImageUrls != null && productDTO.AdditionalImageUrls.Any())
                {
                    int order = 1;
                    foreach (var imageUrl in productDTO.AdditionalImageUrls)
                    {
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            var productImage = new ProductImage
                            {
                                PID = product.PID,
                                ImageUrl = imageUrl,
                                IsMain = false,
                                DisplayOrder = order++
                            };

                            _productImageRepo.AddProductImage(productImage);
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

        }

        public async Task UpdateProductWithImages(int productId, ProductDTO productDTO, string imageUrl) // Update product with images
        {
            var existingProduct = _productRepo.GetProductById(productId);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            _mapper.Map(productDTO, existingProduct);

            // Handle new main image
            if (!string.IsNullOrEmpty(productDTO.MainImageUrl))
            {
                // Delete old main image if exists
                var oldMainImage = _productImageRepo.GetProductImages(productId)
                    .FirstOrDefault(pi => pi.IsMain);

                if (oldMainImage != null)
                {
                    _productImageRepo.DeleteProductImage(oldMainImage.ImageId);
                }

                // Add new main image
                var mainImage = new ProductImage
                {
                    PID = productId,
                    ImageUrl = productDTO.MainImageUrl,
                    IsMain = true,
                    DisplayOrder = 0
                };

                _productImageRepo.AddProductImage(mainImage);

                // Update product's main image URL
                existingProduct.MainImageUrl = productDTO.MainImageUrl;
            }

            _productRepo.UpdateProduct(existingProduct);

            // Handle additional images
            if (productDTO.AdditionalImageUrls != null && productDTO.AdditionalImageUrls.Any())
            {
                // Delete existing additional images
                var existingAdditionalImages = _productImageRepo.GetProductImages(productId)
                    .Where(pi => !pi.IsMain)
                    .ToList();

                foreach (var image in existingAdditionalImages)
                {
                    _productImageRepo.DeleteProductImage(image.ImageId);
                }

                // Add new additional images
                int order = 1;
                foreach (var imageU in productDTO.AdditionalImageUrls)
                {
                    if (!string.IsNullOrEmpty(imageU))
                    {
                        var productImage = new ProductImage
                        {
                            PID = productId,
                            ImageUrl = imageU,
                            IsMain = false,
                            DisplayOrder = order++
                        };

                        _productImageRepo.AddProductImage(productImage);
                    }
                }
            }
        }

        public void DeleteProductImages(int productId) // Delete all images associated with a product
        {
            var images = _productImageRepo.GetProductImages(productId); // Get all images for the product
            foreach (var image in images) // Loop through each image
            {
                _imageService.DeleteImage(image.ImageUrl); // Delete physical file
                _productImageRepo.DeleteProductImage(image.ImageId); // Remove from repository
            }
        }

        public IEnumerable<ProductImage> GetProductImages(int productId) // Get all images for a product
        {
            return _productImageRepo.GetProductImages(productId); // Retrieve images from the repository
        }
        public void SetMainProductImage(int productId, int imageId) // Set a specific image as the main image for a product
        {
            _productImageRepo.SetMainImage(productId, imageId); // Update the main image in the repository
        }
        public void DeleteProductImage(int imageId) // Delete a specific product image by its ID
        {
            _productImageRepo.DeleteProductImage(imageId); // Remove the image from the repository
        }






    }
}
