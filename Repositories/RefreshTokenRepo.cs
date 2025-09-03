namespace E_CommerceSystem.Repositories
{
    public class RefreshTokenRepo
    {
        private readonly ApplicationDbContext _context;  // EF Core DbContext for database access

       // Constructor injecting ApplicationDbContext dependency
        public RefreshTokenRepo(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
