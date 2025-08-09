using EduFlow.BLL.DTOs.Companies.Company;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Companies.Company;

public interface ICompanyServer
{
    Task<List<CompanyForResultDto>> GetAllAsync();
    Task<CompanyForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(CompanyForCreateDto dto);
    Task<bool> UpdateAsync(long id, CompanyForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
}
