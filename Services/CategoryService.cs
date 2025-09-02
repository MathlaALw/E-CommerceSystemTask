using AutoMapper;
using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;

namespace E_CommerceSystem.Services
{
    public class CategoryService : ICategoryService
    {
        // inject repository
        private readonly ICategoryRepo _categoryRepo;

        // AutoMapper
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepo categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        // Add Category
        public void AddCategory(CategoryDTO categoryDTO)
        {
            // Map DTO to Entity
            var category = _mapper.Map<Category>(categoryDTO);
            _categoryRepo.AddCategory(category);
        }

        // Update Category
        public void UpdateCategory(int categoryId, CategoryDTO categoryDTO)
        {
            // Check if category exists
            var category = _categoryRepo.GetCategoryById(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
            // Map updated fields from DTO to Entity
            _mapper.Map(categoryDTO, category);
            _categoryRepo.UpdateCategory(category);
        }

        // Delete Category
        public void DeleteCategory(int categoryId)
        {
            // Check if category exists
            var category = _categoryRepo.GetCategoryById(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found.");
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
            // Check if category exists
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
