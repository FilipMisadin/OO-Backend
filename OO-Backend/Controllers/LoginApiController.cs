using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OO_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace OO_Backend.Controllers
{
    [Route("api")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _database;

        public LoginApiController(IConfiguration config, DatabaseContext context)
        {
            _config = config;
            _database = context;
        }

        [HttpGet]
        [Route("auth")]
        public IActionResult Login(string username, string pass)
        {
            var login = new User
            {
                Username = username,
                Password = pass
            };
            IActionResult response = Unauthorized();

            var user = AuthenticateUser(login);

            if (user == null) return response;
            var tokenStr = GenerateJsonWebToken(user);
            response = Ok(new { token = tokenStr });

            return response;
        }

        private User AuthenticateUser(User login)
        {
            User user = null;
            if (_database.UsernameExists(login.Username))
            {
                user = _database.GetUser(login.Username);
                if (user.Password != login.Password)
                    return null;
            }
            return user;
        }

        private string GenerateJsonWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(180),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }
    }
}