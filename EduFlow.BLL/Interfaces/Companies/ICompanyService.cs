using EduFlow.BLL.DTOs.Companies.Company;

namespace EduFlow.BLL.Interfaces.Companies;

public interface ICompanyService
{
    Task<IEnumerable<CompanyForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CompanyForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(CompanyForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, CompanyForUpdateDto dto, CancellationToken cancellation = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
