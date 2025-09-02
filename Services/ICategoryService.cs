using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface ICategoryService
    {
        void AddCategory(Category category);
        void DeleteCategory(int categoryId);
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int categoryId);
        Category GetCategoryByName(string name);
        void UpdateCategory(Category category);
    }
}