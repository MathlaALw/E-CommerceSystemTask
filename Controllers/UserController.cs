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

namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IConfiguration configuration, IMapper mapper)
        {
            _userService = userService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserDTO InputUser)
        {
            try
            {
                if(InputUser == null)
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
                }
        }
