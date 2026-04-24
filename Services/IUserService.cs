using lab_06.Models;
using lab_06.Models.Dtos;

namespace lab_06.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(UserDto user);
}