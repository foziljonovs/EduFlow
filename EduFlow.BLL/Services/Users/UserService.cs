using EduFlow.BLL.DTOs.Users.User;
using EduFlow.BLL.Interfaces.Users;

namespace EduFlow.BLL.Services.Users;

public class UserService : IUserService
{
    public Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> LoginAsync(UserForLoginDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RegisterAsync(UserForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, UserForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyPasswordAsync(long id, string password, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
