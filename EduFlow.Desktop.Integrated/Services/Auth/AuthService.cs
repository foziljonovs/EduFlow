using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Auth;
using EduFlow.Desktop.Integrated.Servers.Repositories.Auth;

namespace EduFlow.Desktop.Integrated.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthServer _server;
    public AuthService()
    {
        this._server = new AuthServer();
    }
    public async Task<(bool result, string token)> LoginAsync(UserForLoginDto dto)
    {
        try
        {
            var res = await _server.LoginAsync(dto);

            if (res.result)
                return (result: res.result, token: res.token);
            else
                return (result: false, token: string.Empty);
        }
        catch(Exception ex)
        {
            return (result: false, token: string.Empty);
        }
    }
}
