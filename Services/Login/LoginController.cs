using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ShopNest.Services.Login.Model;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ShopNest.Services.Login.Model
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly LoginDbContext _context;

        public LoginController(IConfiguration configuration, LoginDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Call the stored procedure and fetch the results into memory
            var user = _context.UserList
                .FromSqlRaw("EXEC dbo.ValidateUserLogin @EmailOrPhone = {0}, @Password = {1}",
                             loginRequest.EmailOrPhone,
                             loginRequest.Password)
                .AsEnumerable() // Move the query results to memory
                .FirstOrDefault(); // Get the first matching user

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }


        private string GenerateJwtToken(UserList user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
