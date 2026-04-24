using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lab_06.Models.Dtos;
using lab_06.Repositories;
using lab_06.Models;

namespace lab_06.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IConfiguration configuration,  IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public string GenerateJwtToken(string username, string role, List<string>? permissions = null)
    {
        // 1. Definir los Claims básicos
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role) // IMPORTANTE: Esto habilita [Authorize(Roles = "...")]
        };

        // 2. Agregar claims personalizados (permisos específicos)
        if (permissions != null)
        {
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("Permission", permission));
            }
        }

        // 3. Configuración de seguridad
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateUser(UserDto user)
    {
        var existingUser = _unitOfWork.Repository<User>().FindByName(user.Name);
        
        if (user == null)
        {
            return false;
        }
        
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password);
        
        return isPasswordValid;
    }
}