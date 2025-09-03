using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;


namespace E_CommerceSystem.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;


        public ProductController(IProductService productService, IConfiguration configuration, IMapper mapper)
        {
            _productService = productService;
            _configuration = configuration;
            _mapper = mapper;
           
        }

        [HttpPost("AddProduct")]
        [Authorize(Policy = "AdminOnly")] // [Authorize] is an attribute that restricts access to the action method or controller.
                                          // It ensures that only authenticated users can access the resource.
                                          // The Policy = "AdminOnly" part specifies that the authenticated user must also
                                          // have the "AdminOnly" policy applied to their account, which typically means they have the 'Admin' role.

        public async Task<IActionResult> AddNewProduct([FromForm] ProductDTO productInput) // Add product with images
        {
            try // Try-catch block to handle potential exceptions
            {
                // Authorization check
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""); // Retrieve the Authorization header from the request
                var userRole = GetUserRoleFromToken(token); // Decode the token to check user role

                if (userRole != "admin") // Only allow Admin users to add products
                {
                    return BadRequest("You are not authorized to perform this action."); // Return a 400 Bad Request response if the user is not authorized
                }

                if (productInput == null) // Check if input data is null
                {
                    return BadRequest("Product data is required.");
                }

                await _productService.AddProductWithImages(productInput);
                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the product: {ex.Message}");
            }
        }


        public IActionResult AddNewProduct(ProductDTO productInput, int supplierId , int categoryId)
        {
            try
            {
                // Retrieve the Authorization header from the request
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Decode the token to check user role
                var userRole = GetUserRoleFromToken(token);

                // Only allow Admin users to add products
                if (userRole != "admin")
                {
                    return BadRequest("You are not authorized to perform this action.");
                }

                // Check if input data is null
                if (productInput == null)
                {
                    return BadRequest("Product data is required.");
                }

                // Create a new product
                //var product = new Product
                //{
                //    ProductName = productInput.ProductName,
                //    Price = productInput.Price,
                //    Description = productInput.Description,
                //    Stock = productInput.Stock,
                //    OverallRating = 0
                //};

                // Map DTO to Product entity
                var product = _mapper.Map<Product>(productInput);
                product.SupplierId = supplierId;
                product.CategoryId = categoryId;
                product.OverallRating = 0;
                // Add the new product to the database/service layer
                _productService.AddProduct(productInput);

                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while adding the product: {ex.Message}");
            }
        }

        [HttpPut("UpdateProduct/{productId}")]
        public IActionResult UpdateProduct(int productId, ProductDTO productInput)
        {
            try
            {
                // Retrieve the Authorization header from the request
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Decode the token to check user role
                var userRole = GetUserRoleFromToken(token);

                // Only allow Admin users to add products
                if (userRole != "admin")
                {
                    return BadRequest("You are not authorized to perform this action.");
                }

                if (productInput == null)
                    return BadRequest("Product data is required.");

                //var product = _productService.GetProductById(productId);
                
                //product.ProductName = productInput.ProductName;
                //product.Price = productInput.Price;
                //product.Description = productInput.Description;
                //product.Stock = productInput.Stock;
                 
                _productService.UpdateProduct(productId , productInput);

                return Ok("Product updated successfully.");
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while updte product. {(ex.Message)}");
            }
        }

       
        [AllowAnonymous]
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts(
        [FromQuery] string? name,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            try
            {
                // Validate pagination parameters
                if (pageNumber < 1 || pageSize < 1)
                {
                    return BadRequest("PageNumber and PageSize must be greater than 0.");
                }

                // Call the service to get the paged and filtered products
                var products = _productService.GetAllProducts(pageNumber, pageSize, name, minPrice, maxPrice);

                if (products == null || !products.Any())
                {
                    return NotFound("No products found matching the given criteria.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving products. {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet("GetProductByID/{ProductId}")]
        public IActionResult GetProductById(int ProductId)
        {
            try
            {
                var product = _productService.GetProductById(ProductId);
                if (product == null)
                    return NotFound("No product found.");

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving product. {(ex.Message)}");

            }
        }
        private string? GetUserRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                // Extract the 'role' claim
                var roleClaim = jwtToken.Claims.FirstOrDefault (c => c.Type == "role" || c.Type == "unique_name" );
                

                return roleClaim?.Value; // Return the role or null if not found
            }

            throw new UnauthorizedAccessException("Invalid or unreadable token.");
        }
    }
}
