using Microsoft.AspNetCore.Mvc;
using lab_06.Models;
using lab_06.Models.Dtos;
using lab_06.Services;

namespace lab_06.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _userService.RegisterAsync(user);

        if (!success)
            return BadRequest("El usuario ya existe o hubo un error.");

        return Ok(new { message = "Usuario registrado exitosamente" });
    }
}