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

        // Retrieves a refresh token by its token string, including related User
        public RefreshToken GetRefreshToken(string token)
        {
            return _context.RefreshTokens
                  .Include(rt => rt.User)                 // Eager load related User entity
                  .FirstOrDefault(rt => rt.Token == token); // Find first token that matches
        }


        // Updates an existing refresh token in the database
        public void UpdateRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken); // Mark entity as modified
            _context.SaveChanges();                      // Commit changes
        }

        // Revokes a refresh token by marking it as revoked and saving metadata
        public void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;              // Set revocation timestamp
            token.RevokedByIp = ipAddress;                // Store IP address of revoker
            token.ReplacedByToken = replacedByToken;      // If rotated, set replacement token
            _context.RefreshTokens.Update(token);         // Update token entity
            _context.SaveChanges();                       // Save changes

        // Revokes all descendant tokens in a token chain (rotation scenario)
         public void RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason)
        {
            // Check if this token has a replacement (child token)
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = GetRefreshToken(refreshToken.ReplacedByToken); // Fetch child

                if (childToken != null && childToken.IsActive)
                {
                    // If child is still active, revoke it
                    RevokeRefreshToken(childToken, ipAddress, reason);
                }
                else if (childToken != null)
                {


                }




















    }

}
