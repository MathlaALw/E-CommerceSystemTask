using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class CategoryService
    {
        // inject repository
        private readonly ICategoryRepo _categoryRepo;
        public CategoryService(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // Add Category
        public void AddCategory(Category category)
        {
            _categoryRepo.AddCategory(category);
        }

        // Update Category
        public void UpdateCategory(Category category)
        {
            _categoryRepo.UpdateCategory(category);
        }

        // Delete Category
        public void DeleteCategory(int categoryId)
        {
            _categoryRepo.DeleteCategory(categoryId);
        }

        // Get All Categories
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepo.GetAllCategories();
        }

        // Get Category by Id
        public Category GetCategoryById(int categoryId)
        {
            var category = _categoryRepo.GetCategoryById(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
            return category;
        }

        // Get Category by Name
        public Category GetCategoryByName(string name)
        {
            var category = _categoryRepo.GetCategoryByName(name);
            if (category == null)
                throw new KeyNotFoundException($"Category with name {name} not found.");
            return category;
        }

        }
}
