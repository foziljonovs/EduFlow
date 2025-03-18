using EduFlow.BLL.DTOs.Users.User;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Users.User;
using EduFlow.Desktop.Integrated.Servers.Repositories.Users.User;
using EduFlow.Desktop.Integrated.Services.Users.User.Interfaces;

namespace EduFlow.Desktop.Integrated.Services.Users.User.Services;

public class UserService : IUserService
{
    private readonly IUserServer _server;
    public UserService()
    {
        this._server = new UserServer();
    }
    public Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserForResultDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserForResultDto> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RegisterAsync(UserForCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, UserForUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyPasswordAsync(long id, string password)
    {
        throw new NotImplementedException();
    }
}
