using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoryController : ControllerBase
    {
        // inject service
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Add Category
        [HttpPost("AddCategory")]
        public IActionResult AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
               _categoryService.AddCategory(categoryDTO);
                return Ok("Category added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the category. {ex.Message}");
            }
        }
        // Update Category
        [HttpPut("UpdateCategory")]
        public IActionResult UpdateCategory(int categoryId, CategoryDTO categoryDTO)
        {
            try
            {
                _categoryService.UpdateCategory(categoryId, categoryDTO);
                return Ok("Category updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the category. {ex.Message}");
            }
        }
        // Delete Category
        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int categoryId)
        {
            try
            {
                _categoryService.DeleteCategory(categoryId);
                return Ok("Category deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the category. {ex.Message}");
            }
        }
        // get all categories
        [HttpGet("GetAllCategories")]
        public IActionResult Get()
        {
            try
            {
                
                return Ok(_categoryService.GetAllCategories());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving categories. {ex.Message}");
            }

        }

        [HttpGet("GetCategoryById")]
        public IActionResult GetCategoryById(int categoryId)
        {
            try
            {
                return Ok(_categoryService.GetCategoryById(categoryId));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the category. {ex.Message}");
            }
        }



    }
}
