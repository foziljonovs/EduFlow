using EduFlow.BLL.DTOs.Payments.Registry;

namespace EduFlow.BLL.Interfaces.Payments;

public interface IRegistryService
{
    Task<IEnumerable<RegistryForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RegistryForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> IncomeAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> OutlayAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default);
}
