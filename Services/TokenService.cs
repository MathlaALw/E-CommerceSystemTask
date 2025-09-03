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
    }
}
