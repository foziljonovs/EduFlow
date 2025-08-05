using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.BLL.Interfaces.Companies;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Companies;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Companies;

public class CompanyService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CompanyService> logger,
    ICompanyValidator validator) : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CompanyService> _logger = logger;
    private readonly ICompanyValidator _validator = validator;
    public async Task<bool> AddAsync(CompanyForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validation = await _validator.ValidateCreate(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var existsLogo = await _unitOfWork.Logo.GetAsync(dto.LogoId);
            if (existsLogo is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Logo not found.");

            var savedCompany = _mapper.Map<Company>(dto);

            return await _unitOfWork.Company.AddConfirmAsync(savedCompany);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while add the course. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _unitOfWork.Company.GetAsync(id);
            if (company is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company not found.");

            company.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the company by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CompanyForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var companies = await _unitOfWork.Company
                .GetAllFullInformation()
                .ToListAsync(cancellationToken);

            if (!companies.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Companies not found.");

            return _mapper.Map<IEnumerable<CompanyForResultDto>>(companies);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all the course. {ex}");
            throw;
        }
    }

    public async Task<CompanyForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _unitOfWork.Company
                .GetAllFullInformation()
                .Where(x => x.Id == id && x.IsDeleted == false)
                .FirstOrDefaultAsync(cancellationToken);

            if (company is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company not found.");

            return _mapper.Map<CompanyForResultDto>(company);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get by company id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, CompanyForUpdateDto dto, CancellationToken cancellation = default)
    {
        try
        {
            var validation = await _validator.ValidateUpdate(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var company = await _unitOfWork.Company.GetAsync(id);
            if (company is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company not found.");

            var existsLogo = await _unitOfWork.Logo.GetAsync(dto.LogoId);
            if (existsLogo is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Company logo not found.");

            var updatedCompany = _mapper.Map(dto, company);
            updatedCompany.Id = id;

            return await _unitOfWork.Company.UpdateAsync(updatedCompany);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the company id: {id}. {ex}");
            throw;
        }
    }
}
