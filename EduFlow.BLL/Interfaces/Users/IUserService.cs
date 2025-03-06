using EduFlow.BLL.DTOs.Users.User;

namespace EduFlow.BLL.Interfaces.Users;

public interface IUserService
{
    Task<IEnumerable<UserForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> RegisterAsync(UserForCreateDto dto, CancellationToken cancellationToken = default);
    Task<string> LoginAsync(UserForLoginDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, UserForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto, CancellationToken cancellationToken = default);
    Task<bool> VerifyPasswordAsync(long id, string password, CancellationToken cancellationToken = default);
}
