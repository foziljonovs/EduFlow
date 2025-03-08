using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.BLL.Interfaces.Payments;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Payments;

public class RegistryService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<RegistryService> logger) : IRegistryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<RegistryService> _logger = logger;
    public async Task<IEnumerable<RegistryForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var registries = await _unitOfWork.Registry
                .GetAllAsync()
                .Where(x => x.IsDeleted == false)
                .ToListAsync();

            if (!registries.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registries not found.");

            return _mapper.Map<IEnumerable<RegistryForResultDto>>(registries);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all registry. {ex}");
            throw;
        }
    }

    public async Task<RegistryForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var registry = await _unitOfWork.Registry.GetAsync(id);
            if (registry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            if (registry.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This registry has been deleted.");

            return _mapper.Map<RegistryForResultDto>(registry);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get registry by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> IncomeAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var registry = _mapper.Map<Registry>(dto);
            registry.Type = Domain.Enums.PaymentType.Debit;

            return await _unitOfWork.Registry.AddConfirmAsync(registry);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while income the registry. {ex}");
            throw;
        }
    }

    public async Task<bool> OutlayAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var registry = _mapper.Map<Registry>(dto);
            registry.Type = Domain.Enums.PaymentType.Credit;

            return await _unitOfWork.Registry.AddConfirmAsync(registry);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while outley the registry. {ex}");
            throw;
        }
    }
}
