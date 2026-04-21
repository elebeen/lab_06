namespace lab_06.Services;

public interface IAuthService
{
    string? Authenticate(string username, string password);
}