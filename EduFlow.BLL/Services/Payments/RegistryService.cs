﻿using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Payments.Interfaces;
using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.BLL.Interfaces.Payments;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Payments;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Payments;

public class RegistryService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<RegistryService> logger,
    IRegistryValidator validator) : IRegistryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<RegistryService> _logger = logger;
    private readonly IRegistryValidator _validator = validator;

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            var registry = await _unitOfWork.Registry.GetAsync(id);

            if(registry is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registry not found.");

            if (registry.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This registry has been deleted.");

            registry.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellation) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the registry. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<RegistryForResultDto>> FilterAsync(RegistryForFilterDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var registries = await _unitOfWork.Registry
                .GetAllAsync()
                .Where(x => x.IsDeleted == false)
                .ToListAsync(cancellationToken);

            if(!registries.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registries not found.");

            if(dto.StartedDate.HasValue &&
                dto.FinishedDate.HasValue)
            {
                var startedDate = DateTime.SpecifyKind(dto.StartedDate.Value, DateTimeKind.Utc);
                var finishedDate = DateTime.SpecifyKind(dto.FinishedDate.Value, DateTimeKind.Utc);

                registries = registries
                    .Where(x => x.CreatedAt >= startedDate &&
                                x.CreatedAt <= finishedDate)
                    .ToList();
            }

            if(!registries.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Registries not found with the given filter.");

            return _mapper.Map<IEnumerable<RegistryForResultDto>>(registries);

        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while filter the registries. {ex}");
            throw;
        }
    }

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

    public async Task<long> IncomeAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var registry = _mapper.Map<Registry>(dto);
            registry.Type = dto.Type;

            var res = await _unitOfWork.Registry.AddAsync(registry);
            return res;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while income the registry. {ex}");
            throw;
        }
    }

    public async Task<long> OutlayAsync(RegistryForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);
            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var registry = _mapper.Map<Registry>(dto);
            registry.Type = dto.Type;

            var res = await _unitOfWork.Registry.AddAsync(registry);
            return res;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while outley the registry. {ex}");
            throw;
        }
    }
}
