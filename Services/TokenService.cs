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

            // Retrieves the "JwtSettings" section from the application's configuration.
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Gets the secret key from the configuration, used for signing the token.
            var secretKey = jwtSettings["SecretKey"];

            // Creates an array of claims to be included in the token.
            var claims = new[]
            {
                 // Adds the user's unique identifier (UID) as the 'sub' (subject) claim.
            new Claim(JwtRegisteredClaimNames.Sub, user.UID.ToString()),

             // Adds the user's name as the 'name' claim.
            new Claim(JwtRegisteredClaimNames.Name, user.UName),

             // Adds the user's role as the 'role' claim.
            new Claim(ClaimTypes.Role, user.Role),

             // Adds a unique JWT ID ('jti') to prevent token replay attacks.
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
            // Creates a cryptographic key from the secret string using UTF8 encoding.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Creates signing credentials using the key and the HMAC SHA256 algorithm.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // Creates a new JWT token instance.
            var token = new JwtSecurityToken(
            // Sets the claims for the token.
            claims: claims,
            // Sets the token's expiration time based on the configured ExpiryInMinutes.
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
            // Sets the signing credentials.
            signingCredentials: creds
        );

            // Uses a JwtSecurityTokenHandler to serialize and return the token as a string.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Method to generate a cryptographically secure RefreshToken.
        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            // Uses a secure random number generator. Note: RNGCryptoServiceProvider is obsolete.
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                // Creates a byte array to hold the random token data.
                var randomBytes = new byte[64];

                // Fills the byte array with cryptographically strong random data.
                rngCryptoServiceProvider.GetBytes(randomBytes);

                // Creates and returns a new RefreshToken object.
                return new RefreshToken
                {
                    // Converts the random bytes to a Base64 string for the token value.
                    Token = Convert.ToBase64String(randomBytes),
                    // Sets the token's expiration to 7 days from the current UTC time.
                    Expires = DateTime.UtcNow.AddDays(7),
                    // Records the creation time of the token in UTC.
                    Created = DateTime.UtcNow,
                    // Stores the IP address from which the token was created.
                    CreatedByIp = ipAddress
                };
            }

}
}
