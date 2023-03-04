using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProgrammerTaskAPI.Data;
using ProgrammerTaskAPI.Models;
using ProgrammerTaskAPI.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProgrammerTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<AuthOptions> authOptions;

        public AuthController(ApplicationDbContext applicationDbContext, IOptions<AuthOptions> authOptions)
        {
            _context = applicationDbContext;
            this.authOptions = authOptions;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login loginRequest)
        {
            var user = AuthenticateUser(loginRequest.Email, loginRequest.Password);

            if(user != null)
            {
                var token = GenerateJwt(user);

                return Ok(new
                {
                    access_token = token
                });
            }

            return Unauthorized();
        }

        private User AuthenticateUser(string email, string password)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        private string GenerateJwt(User user)
        {
            var authParams = authOptions.Value;
            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            foreach(var role in _context.Roles)
            {
                if (user.RoleId.Equals(role.Id))
                {
                    claims.Add(new Claim("role", role.Name.ToString()));
                }
            }

            var token = new JwtSecurityToken(authParams.Issuer, authParams.Audience, claims, 
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
