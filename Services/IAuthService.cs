namespace lab_06.Services;

public interface IAuthService
{
    public bool ValidateUser(string username, string password);
    string GenerateJwtToken(string username, string role, List<string>? permissions = null);
}