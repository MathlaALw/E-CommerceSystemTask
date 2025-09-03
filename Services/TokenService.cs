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
        // A private field to hold the application's configuration settings.
        private readonly IConfiguration _configuration;

        // Constructor for the TokenService, injecting the IConfiguration dependency.
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // Method to generate a JWT token.
        public string GenerateJwtToken(User user)
        {
        }

}
}
