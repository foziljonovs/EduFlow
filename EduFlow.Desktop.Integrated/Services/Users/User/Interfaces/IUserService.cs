using EduFlow.BLL.DTOs.Users.User;

namespace EduFlow.Desktop.Integrated.Services.Users.User.Interfaces;

public interface IUserService
{
    Task<List<UserForResultDto>> GetAllAsync();
    Task<UserForResultDto> GetByIdAsync(long id);
    Task<bool> RegisterAsync(UserForCreateDto dto);
    Task<bool> UpdateAsync(long id, UserForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto);
    Task<bool> VerifyPasswordAsync(long id, string password);
}
