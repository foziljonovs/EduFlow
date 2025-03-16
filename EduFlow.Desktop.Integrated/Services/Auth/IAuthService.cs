using EduFlow.BLL.DTOs.Users.User;

namespace EduFlow.Desktop.Integrated.Services.Auth;

public interface IAuthService
{
    Task<(bool result, string token)> LoginAsync(UserForLoginDto dto);
}
