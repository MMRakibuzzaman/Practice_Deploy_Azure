using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Models;
using Auth.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext db;
        private readonly IConfiguration conf;

        public AuthController(AppDbContext _db, IConfiguration _conf)
        {
            db = _db;
            conf = _conf;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto req)
        {
            if (db.Users.Any(u => u.Email == req.Email))
                return BadRequest("User already exists");

            var user = new User()
            {
                Email = req.Email,
                PasswordHash = new PasswordHasher<User>().HashPassword(null!, req.Password),
            };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Ok("Registration successful");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto req)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == req.Email);

            if (user == null)
                return Unauthorized();

            var password = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash!, req.Password);

            if (password != PasswordVerificationResult.Success)
                return Unauthorized();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()) };

            var token = new JwtSecurityToken(
                issuer: conf["Jwt:Issuer"],
                audience: conf["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );
            
            return Ok(new{Token=new JwtSecurityTokenHandler().WriteToken(token)});
        }
    }
}