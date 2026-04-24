using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using lab_06.Models;
using lab_06.Models.Dtos;
using lab_06.Services;

namespace lab_06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    
    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto login)
    {
        if (!_auth.ValidateUser(login))
        {
            return Unauthorized();
        }

        // 2. Definir roles/permisos (esto podría venir de tu BD)
        string role = "Administrator"; 

        // 3. Generar token con claims
        var token = _auth.GenerateJwtToken(login.Name, role);

        return Ok(new { token });
    }

    [Authorize(Roles = "Administrator")]
    [HttpGet("admin")]
    public IActionResult GetAdmin()
    {
        return Ok("solo admins");
    }
}