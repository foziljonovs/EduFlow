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
    public async Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto)
    {
        try
        {
            var result = await _server.ChangePasswordAsync(id, dto);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var result = await _server.DeleteAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<List<UserForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch(Exception ex)
        {
            return new List<UserForResultDto>();
        }
    }

    public async Task<UserForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return new UserForResultDto();
        }
    }

    public async Task<bool> RegisterAsync(UserForCreateDto dto)
    {
        try
        {
            var result = await _server.RegisterAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(long id, UserForUpdateDto dto)
    {
        try
        {
            var result = await _server.UpdateAsync(id, dto);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> VerifyPasswordAsync(long id, string password)
    {
        try
        {
            var result = await _server.VerifyPasswordAsync(id, password);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}
