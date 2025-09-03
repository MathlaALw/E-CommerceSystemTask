using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly IProductImageRepo _productImageRepo; // For managing product images
        private readonly IImageService _imageService; // For handling image file operations
        private readonly IMapper _mapper;

        public ProductService(IProductRepo productRepo, IProductImageRepo productImageRepo, IImageService imageService, IMapper mapper)
        {
            _productRepo = productRepo;
            _productImageRepo = productImageRepo;
            _imageService = imageService;
            _mapper = mapper;
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
            var product = _productRepo.GetProductById(pid);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {pid} not found.");
            return product;
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

        public async Task AddProductWithImages(ProductDTO productDTO) // Add product with images
        {
            var product = _mapper.Map<Product>(productDTO); // Map DTO to entity

            // Add the product first
            _productRepo.AddProduct(product);

            // Handle main image
            if (productDTO.Image != null && _imageService.IsValidImage(productDTO.Image))
            {
                var imageUrl = await _imageService.SaveImageAsync(productDTO.Image, "products"); // Save image and get URL

                var mainImage = new ProductImage
                {
                    PID = product.PID,
                    ImageUrl = imageUrl,
                    IsMain = true,
                    DisplayOrder = 0
                };

            }


        }
