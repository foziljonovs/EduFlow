using EduFlow.BLL.DTOs.Users.User;

namespace EduFlow.Desktop.Integrated.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthService _service;
    public AuthService(IAuthService service)
    {
        this._service = service;
    }
    public async Task<(bool result, string token)> LoginAsync(UserForLoginDto dto)
    {
        try
        {
            var res = await _service.LoginAsync(dto);

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
