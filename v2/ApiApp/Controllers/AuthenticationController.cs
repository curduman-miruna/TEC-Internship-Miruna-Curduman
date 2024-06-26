﻿using Internship.Model;
using Internship.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ApiApp;
using NLog;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginViewModel login)
        {
            var db = new APIDbContext();
            var user = db.Users.FirstOrDefault(x => x.Email == login.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                _logger.Warn($"Failed login attempt for {login.Email}");
                return Unauthorized();
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            string token = GenerateToken(authClaims);
            _logger.Info($"User {login.Email} logged in");
            return Ok(token);
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid == false)
            {
                _logger.Warn("Invalid user registration attempt");
                return BadRequest();
            }
                
            var db = new APIDbContext();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            db.Users.Add(user);
            db.SaveChanges();

            _logger.Info($"User {user.Email} registered");
            return Ok(user);
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"]!,
                Audience = _configuration["JWT:ValidAudience"]!,
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
