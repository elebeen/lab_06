using lab_06.Models;
using lab_06.Models.Dtos;
using lab_06.Repositories;

namespace lab_06.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> RegisterAsync(UserDto user)
    {
        var existingUser = _unitOfWork.Repository<User>().FindByName(user.Name);
        if (existingUser != null) return false;

        // 2. Aquí deberías encriptar la contraseña (ej. BCrypt)
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        var x = new User()
        {
            Name = user.Name,
            Password = user.Password
        };
        
        // 3. Guardar
        _unitOfWork.Repository<User>().Add(x);
        await _unitOfWork.SaveChanges();
        
        return true;
    }
}