using EduFlow.BLL.DTOs.Companies.CompanySubscription;

namespace EduFlow.BLL.Interfaces.Companies;

public interface ICompanySubscriptionService
{
    Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CompanySubscriptionForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(CompanySubscriptionForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, CompanySubscriptionForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllByCompanyIdAsync(long companyId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllIsActive(CancellationToken cancellationToken = default);
}
