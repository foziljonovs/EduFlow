using EduFlow.BLL.DTOs.Companies.Logo;

namespace EduFlow.BLL.Interfaces.Companies;

public interface ILogoService
{
    Task<IEnumerable<LogoForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LogoForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<long> AddAsync(LogoForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, LogoForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
