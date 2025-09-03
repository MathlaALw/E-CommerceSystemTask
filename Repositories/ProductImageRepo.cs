namespace E_CommerceSystem.Repositories
{
    public class ProductImageRepo
    {
        private readonly ApplicationDbContext _context; // Database context
        public ProductImageRepo(ApplicationDbContext context) // Constructor with dependency injection
        {
            _context = context;
        }


    }
}
