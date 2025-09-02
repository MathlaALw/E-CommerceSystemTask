using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoryController
    {
        // inject service
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Add Category
        [HttpPost("AddCategory")]
        public IActionResult AddCategory([FromBody] Category category)
        {
            try
            {
                if (category == null || string.IsNullOrWhiteSpace(category.Name))
                {
                    return new BadRequestObjectResult("Category data is invalid.");
                }
                _categoryService.AddCategory(category);
                return new OkObjectResult("Category added successfully.");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"An error occurred while adding the category. {ex.Message}") { StatusCode = 500 };
            }
        }

        // get all categories
        [HttpGet("GetAllCategories")]
        public IActionResult Get()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
                return new OkObjectResult(categories);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"An error occurred while retrieving categories. {ex.Message}") { StatusCode = 500 };
            }

        }


        }
}
