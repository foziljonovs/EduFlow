using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.DTOs.Companies.Logo;
using EduFlow.BLL.Interfaces.Companies;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Companies;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Companies;

public class LogoService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<LogoService> logger,
    ILogoValidator validator) : ILogoService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<LogoService> _logger = logger;
    private readonly ILogoValidator _validator = validator;
    public async Task<bool> AddAsync(LogoForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validation = await _validator.ValidateCreate(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var savedLogo = _mapper.Map<Logo>(dto);
            return await _unitOfWork.Logo.AddConfirmAsync(savedLogo);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while adding the logo. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsLogo = await _unitOfWork.Logo.GetAsync(id);
            if (existsLogo is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Logo not found.");

            existsLogo.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the logo by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<LogoForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var logos = await _unitOfWork.Logo
                .GetAllAsync()
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!logos.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Logos not found.");

            return _mapper.Map<IEnumerable<LogoForResultDto>>(logos);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all logo. {ex}");
            throw;
        }
    }

    public async Task<LogoForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var logo = await _unitOfWork.Logo.GetAsync(id);
            if (logo is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Logo not found.");

            return _mapper.Map<LogoForResultDto>(logo);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get by id: {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, LogoForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validation = await _validator.ValidateUpdate(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var logo = await _unitOfWork.Logo.GetAsync(id);
            if (logo is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Logo not found.");

            var updatedLogo = _mapper.Map(dto, logo);
            updatedLogo.Id = id;

            return await _unitOfWork.Logo.UpdateAsync(logo);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the logo by id: {id}. {ex}");
            throw;
        }
    }
}
