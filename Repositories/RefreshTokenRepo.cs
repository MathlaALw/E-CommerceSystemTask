using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;

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


        // Adds a new refresh token to the database
        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);   // Add entity to DbSet
            _context.SaveChanges();                     // Save changes to database
        }
}
