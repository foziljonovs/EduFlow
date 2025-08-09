using EduFlow.BLL.DTOs.Companies.CompanySubscription;

namespace EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;

public interface ICompanySubscriptionServer
{
    Task<List<CompanySubscriptionForResultDto>> GetAllAsync();
    Task<CompanySubscriptionForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(CompanySubscriptionForCreateDto dto);
    Task<bool> UpdateAsync(long id, CompanySubscriptionForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<CompanySubscriptionForResultDto>> GetAllByCompanyIdAsync(long companyId);
    Task<List<CompanySubscriptionForResultDto>> GetAllIsActive();
}
