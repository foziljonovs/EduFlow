using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Payments;
using EduFlow.Desktop.Integrated.Servers.Repositories.Payments;

namespace EduFlow.Desktop.Integrated.Services.Payments.Registry;

public class RegistryService : IRegistryService
{
    private readonly IRegistryServer _server;
    public RegistryService()
    {
        this._server = new RegistryServer();
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

    public async Task<List<RegistryForResultDto>> FilterAsync(RegistryForFilterDto dto)
    {
        try
        {
            var result = await _server.FilterAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return new List<RegistryForResultDto>();
        }
    }

    public async Task<List<RegistryForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch(Exception ex)
        {
            return new List<RegistryForResultDto>();
        }
    }

    public async Task<RegistryForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return new RegistryForResultDto();
        }
    }

    public async Task<long> IncomeAsync(RegistryForCreateDto dto)
    {
        try
        {
            var result = await _server.IncomeAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return 0;
        }
    }

    public async Task<long> OutlayAsync(RegistryForCreateDto dto)
    {
        try
        {
            var result = await _server.OutlayAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return 0;
        }
    }
}
