using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.BLL.Interfaces.Payments;

namespace EduFlow.BLL.Services.Payments;

public class RegistryService : IRegistryService
{
    public Task<IEnumerable<RegistryForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<RegistryForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IncomeAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> OutlayAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
