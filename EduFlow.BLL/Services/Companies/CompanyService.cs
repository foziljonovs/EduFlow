using AutoMapper;
using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.BLL.Interfaces.Companies;
using EduFlow.DAL.Interfaces;
using Microsoft.Extensions.Logging;

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
    public Task<bool> AddAsync(CompanyForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CompanyForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CompanyForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, CompanyForUpdateDto dto, CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }
}
