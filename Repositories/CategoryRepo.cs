using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        public ApplicationDbContext _context;
        public CategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        //Add Categories
        public void AddCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Update Category
        public void UpdateCategory(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Delete Category
        public void DeleteCategory(int categoryId)
        {
            try
            {
                var category = GetCategoryById(categoryId);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Get Category by Id
        public Category GetCategoryById(int categoryId)
        {
            try
            {
                return _context.Categories.Find(categoryId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Get All Categories
        public IEnumerable<Category> GetAllCategories()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        // Get Category by Name
        public Category GetCategoryByName(string name)
        {
            try
            {
                return _context.Categories.FirstOrDefault(c => c.Name == name);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

    }
}
