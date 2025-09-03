using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IRefreshTokenRepo
    {
        void AddRefreshToken(RefreshToken refreshToken);
        RefreshToken GetRefreshToken(string token);
        void RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason);
        void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null);
        void UpdateRefreshToken(RefreshToken refreshToken);
    }
}