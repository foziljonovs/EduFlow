using EduFlow.BLL.DTOs.Payments.Registry;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Payments;

public interface IRegistryServer
{
    Task<List<RegistryForResultDto>> GetAllAsync();
    Task<RegistryForResultDto> GetByIdAsync(long id);
    Task<long> IncomeAsync(RegistryForCreateDto dto);
    Task<long> OutlayAsync(RegistryForCreateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<RegistryForResultDto>> FilterAsync(RegistryForFilterDto dto);
}
