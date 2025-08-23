using EduFlow.BLL.DTOs.Companies.Logo;

namespace EduFlow.Desktop.Integrated.Services.Companies.Logo;

public interface ILogoService
{
    Task<List<LogoForResultDto>> GetAllAsync();
    Task<LogoForResultDto> GetByIdAsync(long id);
    Task<long> AddAsync(string filePath);
    Task<bool> UpdateAsync(long id, LogoForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<byte[]> GetLogoByNameAsync(string fileName);
    Task<byte[]> GetLogoByIdAsync(long id);
}
