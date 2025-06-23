using EduFlow.BLL.DTOs.Payments.Registry;

namespace EduFlow.BLL.Interfaces.Payments;

public interface IRegistryService
{
    Task<IEnumerable<RegistryForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RegistryForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<long> IncomeAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default);
    Task<long> OutlayAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellation = default);
}
