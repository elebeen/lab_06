using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using lab_06.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace lab_06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    
    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User login)
    {
        if (login.Name.ToString() == "admin" && login.Password.ToString() == "admin")
        {
            var claims = new[]
            {
                new Claim(type: ClaimTypes.Name, login.Name.ToString()),
                new Claim(type: ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, algorithm: SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        return Unauthorized();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult GetAdmin()
    {
        return Ok("solo admins");
    }
    
}