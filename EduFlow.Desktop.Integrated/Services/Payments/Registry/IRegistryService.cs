using EduFlow.BLL.DTOs.Payments.Registry;

namespace EduFlow.Desktop.Integrated.Services.Payments.Registry;

public interface IRegistryService
{
    Task<List<RegistryForResultDto>> GetAllAsync();
    Task<RegistryForResultDto> GetByIdAsync(long id);
    Task<long> IncomeAsync(RegistryForCreateDto dto);
    Task<long> OutlayAsync(RegistryForCreateDto dto);
    Task<bool> DeleteAsync(long id);
}
