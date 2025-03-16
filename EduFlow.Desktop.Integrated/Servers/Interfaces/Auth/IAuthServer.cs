using EduFlow.BLL.DTOs.Users.User;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Auth;

public interface IAuthServer
{
    Task<(bool result, string token)> LoginAsync(UserForLoginDto dto);
}
