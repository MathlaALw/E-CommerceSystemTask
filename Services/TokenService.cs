using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace E_CommerceSystem.Services
{
    public class TokenService
    {
        // Declares a method to generate a new JWT (JSON Web Token) for a given user.
        string GenerateJwtToken(User user);

        // Declares a method to generate a new RefreshToken.
        RefreshToken GenerateRefreshToken(string ipAddress);

        // Declares a method to set both JWT and RefreshToken as HTTP cookies on the response.
        void SetTokenCookies(HttpResponse response, string jwtToken, RefreshToken refreshToken);

        // Declares a method to delete the token cookies from the HTTP response.
        void RemoveTokenCookies(HttpResponse response);
    }

}
}
