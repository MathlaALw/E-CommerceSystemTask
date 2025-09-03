using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user); // Declares a method to create a new JWT (JSON Web Token) for a user.
                                            // It takes a User object and returns the generated JWT as a string
        RefreshToken GenerateRefreshToken(string ipAddress);  // Declares a method to generate a new RefreshToken.
                                                              // It takes the IP address of the client and returns a RefreshToken object
        void RemoveTokenCookies(HttpResponse response);// Declares a method to remove the token cookies from the HTTP response.
                                                       // This is typically used during logout.
        void SetTokenCookies(HttpResponse response, string jwtToken, RefreshToken refreshToken);
    }
}