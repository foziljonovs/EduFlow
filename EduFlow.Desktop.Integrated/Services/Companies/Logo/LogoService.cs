using EduFlow.BLL.DTOs.Companies.Logo;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Companies.Logo;
using EduFlow.Desktop.Integrated.Servers.Repositories.Companies.Logo;

namespace EduFlow.Desktop.Integrated.Services.Companies.Logo;

public class LogoService : ILogoService
{
    private readonly ILogoServer _server;
    public LogoService()
    {
        this._server = new LogoServer();
    }
    public async Task<long> AddAsync(LogoForCreateDto dto)
    {
        try
        {
            var result = await _server.AddAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return 0;
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

    public async Task<List<LogoForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<LogoForResultDto>();
        }
    }

    public async Task<LogoForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return new LogoForResultDto();
        }
    }

    public async Task<byte[]> GetLogoByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetLogoByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            return Array.Empty<byte>();
        }
    }

    public async Task<byte[]> GetLogoByNameAsync(string fileName)
    {
        try
        {
            var result = await _server.GetLogoByNameAsync(fileName);
            return result;
        }
        catch (Exception ex)
        {
            return Array.Empty<byte>();
        }
    }

    public async Task<bool> UpdateAsync(long id, LogoForUpdateDto dto)
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
