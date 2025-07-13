
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SampleJwtApi.Data;
using SampleJwtApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SampleJwtApi.Controllers
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AuthDbContext _dbContext;

        public AuthController(IConfiguration configuration, AuthDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            var dbUser = _dbContext.Users
                .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Claims)
                .FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (dbUser == null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dbUser.Username)
            };

            foreach (var role in dbUser.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            foreach (var claim in dbUser.Claims)
            {
                claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
            }

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
