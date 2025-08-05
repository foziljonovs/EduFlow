using AutoMapper;
using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.BLL.Interfaces.Companies;
using EduFlow.DAL.Interfaces;
using Microsoft.Extensions.Logging;

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
    public Task<bool> AddAsync(CompanySubscriptionForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllByCompanyIdAsync(long companyId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CompanySubscriptionForResultDto>> GetAllIsActive(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CompanySubscriptionForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, CompanySubscriptionForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
