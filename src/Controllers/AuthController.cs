using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReciclagemApi.Models;
using ReciclagemApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReciclagemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly string _jwtSecret;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _jwtSecret = configuration["Jwt:SecretKey"];

            if (string.IsNullOrEmpty(_jwtSecret))
            {
                throw new ArgumentNullException("Jwt:SecretKey", "JWT Secret Key is not configured in appsettings.json");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var newUser = new UserModel
            {
                Username = request.Username,
                Role = request.Role ?? "User" // Define "User" como role padrão se não especificado
            };

            var createdUser = await _userService.RegisterUserAsync(newUser, request.Password);
            return CreatedAtAction(nameof(Register), new { id = createdUser.UserId }, createdUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authenticatedUser = await _userService.AuthenticateUserAsync(request.Username, request.Password);
            if (authenticatedUser == null) return Unauthorized();

            var token = GenerateJwtToken(authenticatedUser);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var token = new JwtSecurityToken(
                issuer: "reciclagem_api",
                audience: "reciclagem_api",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Opcional, padrão será "User"
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
