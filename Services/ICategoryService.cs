using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface ICategoryService
    {
        void AddCategory(CategoryDTO categoryDTO);
        void DeleteCategory(int categoryId);
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int categoryId);
        Category GetCategoryByName(string name);
        void UpdateCategory(int categoryId, CategoryDTO categoryDTO);
    }
}