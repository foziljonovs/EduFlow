using AutoMapper;
using EduFlow.BLL.DTOs.Companies.Logo;
using EduFlow.BLL.Interfaces.Companies;
using EduFlow.DAL.Interfaces;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.Logging;

namespace EduFlow.BLL.Services.Companies;

public class LogoService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<LogoService> logger) : ILogoService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<LogoService> _logger = logger;
    public Task<bool> AddAsync(LogoForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LogoForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<LogoForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, LogoForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
