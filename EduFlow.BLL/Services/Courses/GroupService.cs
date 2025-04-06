using AutoMapper;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace EduFlow.BLL.Services.Courses;

public class GroupService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<GroupService> logger) : IGroupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupService> _logger = logger;
    public Task<bool> AddAsync(GroupForCreateDto groupForCreationDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GroupForResultDto>> FilterAsync(GroupForFilterDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GroupForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<GroupForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<GroupForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, GroupForUpdateDto groupForUpdateDto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
