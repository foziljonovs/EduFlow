using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Companies.Company;
using EduFlow.Desktop.Integrated.Servers.Repositories.Companies.Company;

namespace EduFlow.Desktop.Integrated.Services.Companies.Company;

public class CompanyService : ICompanyService
{
    private readonly ICompanyServer _server;

    public CompanyService()
    {
        this._server = new CompanyServer();
    }

    public async Task<bool> AddAsync(CompanyForCreateDto dto)
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
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<List<CompanyForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<CompanyForResultDto>();
        }
    }

    public async Task<CompanyForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            return new CompanyForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, CompanyForUpdateDto dto)
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
