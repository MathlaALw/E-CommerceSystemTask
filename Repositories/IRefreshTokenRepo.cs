using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IRefreshTokenRepo
    {
        void AddRefreshToken(RefreshToken refreshToken); // Adds a new refresh token to the repository.
        RefreshToken GetRefreshToken(string token);   // Retrieves a refresh token from the repository using its string value
        void RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason); //Revokes all descendant refresh tokens associated with a given token.
                                                                                                        // Descendant tokens are those created from a previous token in the same chain.
        void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null);
        void UpdateRefreshToken(RefreshToken refreshToken);
    }
}