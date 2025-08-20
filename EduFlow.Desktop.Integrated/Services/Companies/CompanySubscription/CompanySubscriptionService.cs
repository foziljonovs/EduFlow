using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Companies.CompanySubscription;
using EduFlow.Desktop.Integrated.Servers.Repositories.Companies.CompanySubscription;

namespace EduFlow.Desktop.Integrated.Services.Companies.CompanySubscription;

public class CompanySubscriptionService : ICompanySubscriptionService
{
    private readonly ICompanySubscriptionServer _server;
    public CompanySubscriptionService()
    {
        _server = new CompanySubscriptionServer();
    }
    public async Task<bool> AddAsync(CompanySubscriptionForCreateDto dto)
    {
        try
        {
            var result = await _server.AddAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var result = await _server.DeleteAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }   
    }

    public async Task<List<CompanySubscriptionForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<CompanySubscriptionForResultDto>();
        }
    }

    public async Task<List<CompanySubscriptionForResultDto>> GetAllByCompanyIdAsync(long companyId)
    {
        try
        {
            var result = await _server.GetAllByCompanyIdAsync(companyId);
            return result;
        }
        catch(Exception ex)
        {
            return new List<CompanySubscriptionForResultDto>();
        }
    }

    public async Task<List<CompanySubscriptionForResultDto>> GetAllIsActive()
    {
        try
        {
            var result = await _server.GetAllIsActive();
            return result;
        }
        catch (Exception ex)
        {
            return new List<CompanySubscriptionForResultDto>();
        }
    }

    public async Task<CompanySubscriptionForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return new CompanySubscriptionForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, CompanySubscriptionForUpdateDto dto)
    {
        try
        {
            var result = await _server.UpdateAsync(id, dto);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
