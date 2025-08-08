using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.BLL.Interfaces.Companies;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Companies;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Companies;

public class CompanySubscriptionService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CompanySubscriptionService> logger,
    ICompanySubscriptionValidator validator) : ICompanySubscriptionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CompanySubscriptionService> _logger = logger;
    private readonly ICompanySubscriptionValidator _validator = validator;
    public async Task<bool> AddAsync(CompanySubscriptionForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validation = await _validator.ValidateCreate(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var existsCompany = await _unitOfWork.Company.GetAsync(dto.CompanyId);
            if (existsCompany is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company subscription not found.");

            var savedCompanySubscription = _mapper.Map<CompanySubscription>(dto);
            savedCompanySubscription.SubcriptionDate = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.CompanySubscription.AddConfirmAsync(savedCompanySubscription);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An erroor occured while add the company subscription. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var companySubscription = await _unitOfWork.CompanySubscription.GetAsync(id);
            if (companySubscription is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company subscription not found.");

            companySubscription.IsDeleted = true;
            companySubscription.IsActive = false;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured whil delete the company subscription by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var companySubscriptions = await _unitOfWork.CompanySubscription
                .GetAllFullInformation()
                .ToListAsync(cancellationToken);

            if (!companySubscriptions.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company subscriptions not found.");

            return _mapper.Map<IEnumerable<CompanySubscriptionForResultDto>>(companySubscriptions);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all the company subscription. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllByCompanyIdAsync(long companyId, CancellationToken cancellationToken = default)
    {
        try
        {
            var companySubscriptions = await _unitOfWork.CompanySubscription
                .GetAllFullInformation()
                .Where(x => x.CompanyId == companyId)
                .ToListAsync(cancellationToken);

            if (!companySubscriptions.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company subscriptions not found.");

            return _mapper.Map<IEnumerable<CompanySubscriptionForResultDto>>(companySubscriptions);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while get all company subscriptions by company id: {companyId}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllIsActive(CancellationToken cancellationToken = default)
    {
        try
        {
            var companySubscriptions = await _unitOfWork.CompanySubscription
                .GetAllFullInformation()
                .Where(x => x.IsActive == true)
                .ToListAsync(cancellationToken);

            if (!companySubscriptions.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company subscriptions not found.");

            return _mapper.Map<IEnumerable<CompanySubscriptionForResultDto>>(companySubscriptions);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all is active. {ex}");
            throw;
        }
    }

    public async Task<CompanySubscriptionForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var companySubscription = await _unitOfWork.CompanySubscription
                .GetAllFullInformation()
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (companySubscription is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company subscription not found.");

            return _mapper.Map<CompanySubscriptionForResultDto>(companySubscription);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, CompanySubscriptionForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validation = await _validator.ValidateUpdate(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var companySubscription = await _unitOfWork.CompanySubscription.GetAsync(id);
            if (companySubscription is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company subscription not found.");

            var updatedCompanySubscription = _mapper.Map(dto, companySubscription);
            updatedCompanySubscription.Id = id;

            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the company subscription id: {id}. {ex}");
            throw;
        }
    }
}
