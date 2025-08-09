using EduFlow.BLL.DTOs.Companies.Logo;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Companies.Logo;

public interface ILogoServer
{
    Task<List<LogoForResultDto>> GetAllAsync();
    Task<LogoForResultDto> GetByIdAsync(long id);
    Task<long> AddAsync(LogoForCreateDto dto);
    Task<bool> UpdateAsync(long id, LogoForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
}
