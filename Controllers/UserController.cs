using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using System.Security.Cryptography;
using E_CommerceSystem.Repositories;


namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRefreshTokenRepo _refreshTokenRepo; // A private, read-only field that holds a reference to the refresh token repository.
                                                              // The 'readonly' keyword ensures this reference can only be set in the constructor.
        private readonly ITokenService _tokenService; // A private, read-only field that holds a reference to the token service.
                                                      // This is used to call methods for generating, setting, or removing tokens.


        public UserController(IUserService userService, IConfiguration configuration, IMapper mapper, IRefreshTokenRepo refreshTokenRepo,ITokenService tokenService)
        {
            _userService = userService;
            _configuration = configuration;
            _mapper = mapper;
            _refreshTokenRepo = refreshTokenRepo; // Assigns the injected IRefreshTokenRepo instance to the private, read-only field.
            _tokenService = tokenService; // Assigns the injected ITokenService instance to the private, read-only field

        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserDTO InputUser)
        {
            try
            {
                if (InputUser == null)
                    return BadRequest("User data is required");

                //var user = new User
                //{
                //    UName = InputUser.UName,
                //    Email = InputUser.Email,
                //    Password = InputUser.Password,
                //    Role = InputUser.Role,
                //    Phone = InputUser.Phone,
                //    CreatedAt = DateTime.Now
                //};
                var user = _mapper.Map<User>(InputUser);
                _userService.AddUser(user);

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while adding the user. {ex.Message} ");
            }
        }


        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login(string email, string password)
        {
            try
            {
                var user = _userService.GetUSer(email, password);
                string token = GenerateJwtToken(user.UID.ToString(), user.UName, user.Role);
                return Ok(token);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while login. {(ex.Message)}");
            }

        }


        [HttpGet("GetUserById/{UserID}")]
        public IActionResult GetUserById(int UserID)
        {
            try
            {
                var user = _userService.GetUserById(UserID);
                return Ok(user);

            }
            catch (Exception ex)
            {
                // Return a generic error response
                return StatusCode(500, $"An error occurred while retrieving user. {(ex.Message)}");
            }
        }

        [NonAction]
        public string GenerateJwtToken(string userId, string username, string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.UniqueName, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

       // Add method to generate refresh token

        // [NonAction] attribute indicates that this method is not a web action and cannot be called directly via a URL.
        [NonAction]
        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            // A secure way to generate cryptographic random numbers.
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                // Creates a byte array to hold the random data for the token.
                var randomBytes = new byte[64];

                // Fills the byte array with cryptographically strong random bytes.
                rngCryptoServiceProvider.GetBytes(randomBytes);

                // Creates and returns a new RefreshToken object.
                return new RefreshToken
                {
                    // Converts the random bytes to a URL-safe Base64 string for the token value.
                    Token = Convert.ToBase64String(randomBytes),

                    // Sets the token's expiration date to 7 days from the current UTC time.
                    Expires = DateTime.UtcNow.AddDays(7),

                    // Records the exact time the token was created in UTC.
                    Created = DateTime.UtcNow,

                    // Stores the IP address from which the token was created.
                    CreatedByIp = ipAddress
                };

            }
        }
        [HttpPost("Logout")] // Defines an HTTP POST endpoint for the "Logout" action
        public IActionResult Logout()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];   // Tries to retrieve the "refreshToken" from the incoming request's cookies
                if (!string.IsNullOrEmpty(refreshToken)) // Checks if the refresh token cookie exists and is not empty
                {
                    var storedToken = _refreshTokenRepo.GetRefreshToken(refreshToken);  // Gets the refresh token record from the repository based on the token string.
                    if (storedToken != null) // Checks if a matching token was found in the repository
                    {
                        // Revokes the refresh token, marking it as invalid in the database.
                        // It also records the IP address and the reason for revocation ("Logout").
                        _refreshTokenRepo.RevokeRefreshToken(storedToken, GetIpAddress(), "Logout");
                    }
                }

                _tokenService.RemoveTokenCookies(Response); // Deletes both the access token and refresh token cookies from the user's browser.
                return Ok(new { message = "Logout successful" }); // Returns a 200 OK status with a success message
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while logging out. {ex.Message}");// Catches any exceptions that occur during the logout process.
                                                                                             // Returns a 500 Internal Server Error status with a descriptive error message.
            }
        }

        private string GetIpAddress() // A private helper method to get the client's IP address
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))  // Checks if the "X-Forwarded-For" header is present. This is common when using proxies or load balancers.
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
        }



    }
}
