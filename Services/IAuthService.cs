using lab_06.Models.Dtos;

namespace lab_06.Services;

public interface IAuthService
{
    public bool ValidateUser(UserDto user);
    string GenerateJwtToken(string username, string role, List<string>? permissions = null);
}