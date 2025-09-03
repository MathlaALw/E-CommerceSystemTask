using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user); // Declares a method to create a new JWT (JSON Web Token) for a user.
                                            // It takes a User object and returns the generated JWT as a string
        RefreshToken GenerateRefreshToken(string ipAddress);
        void RemoveTokenCookies(HttpResponse response);
        void SetTokenCookies(HttpResponse response, string jwtToken, RefreshToken refreshToken);
    }
}