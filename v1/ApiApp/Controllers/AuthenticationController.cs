using Internship;
using Internship.Model;
using Internship.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginViewModel login)
        {
            var db = new APIDbContext();
            var user = db.Admins.Select(x => x).Where(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefault();
            if (user == null)
                return NotFound();
            else
            {
                var authClaims = new List<Claim>
                {
                   new Claim(ClaimTypes.Email, user.Email!),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                string token = GenerateToken(authClaims);
                return Ok(token);
            }
                
        }

        [HttpPost("register")]
        public IActionResult Register(Admin admin)
        {
            var db = new APIDbContext();
            db.Admins.Add(admin);
            db.SaveChanges();
            return Ok(admin);
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration["JWT:ValidIssuer"]!,
                Audience = configuration["JWT:ValidAudience"]!,
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }
}
