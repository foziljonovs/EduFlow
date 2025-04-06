using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Courses;

public class GroupService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<GroupService> logger) : IGroupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupService> _logger = logger;
    public async Task<bool> AddAsync(GroupForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsCourse = await _unitOfWork.Course.GetAsync(dto.CourseId);
            if(existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            if(existsCourse.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This course has been deleted.");

            var savedCourse = _mapper.Map<Group>(dto);
            return await _unitOfWork.Group.AddConfirmAsync(savedCourse);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while adding the group: {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var group = await _unitOfWork.Group.GetAsync(id);
            if(group is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            group.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while deleting the group: {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<GroupForResultDto>> FilterAsync(GroupForFilterDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var groupQuery = _unitOfWork.Group.GetAllFullInformation();

            if(!groupQuery.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            if(dto.StartedDate.HasValue && dto.FinishedDate.HasValue)
            {
                var startedDateUtc = DateTime.SpecifyKind(dto.StartedDate.Value, DateTimeKind.Utc);
                var finishedDateUtc = DateTime.SpecifyKind(dto.FinishedDate.Value, DateTimeKind.Utc);
                groupQuery = groupQuery.Where(x =>
                    x.CreatedAt >= startedDateUtc &&
                    x.CreatedAt <= finishedDateUtc);
            }

            if (!string.IsNullOrEmpty(dto.Name))
                groupQuery = groupQuery
                    .Where(x => x.Name.ToLower().Contains(dto.Name.ToLower()));

            if (dto.IsStatus.HasValue)
                groupQuery = groupQuery
                    .Where(x => x.IsStatus == dto.IsStatus);

            var groups = await groupQuery
                .ToListAsync(cancellationToken);

            if(!groups.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            return _mapper.Map<IEnumerable<GroupForResultDto>>(groups);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while filtering the groups: {ex}");
            throw;  
        }
    }

    public async Task<IEnumerable<GroupForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var groups = await _unitOfWork.Group.GetAllFullInformation()
                .ToListAsync(cancellationToken);

            if (!groups.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");
            
            return _mapper.Map<IEnumerable<GroupForResultDto>>(groups);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while getting all groups: {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<GroupForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var groups = await _unitOfWork.Group.GetAllFullInformation()
                .Where(x => x.CourseId == courseId)
                .ToListAsync(cancellationToken);

            if (!groups.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");
            
            return _mapper.Map<IEnumerable<GroupForResultDto>>(groups);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while getting all groups by course id: {ex}");
            throw;
        }
    }

    public async Task<GroupForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var group = await _unitOfWork.Group
                .GetAllAsync()
                .Where(x => x.IsDeleted == false && x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (group is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");
            
            return _mapper.Map<GroupForResultDto>(group);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while getting the group by id: {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, GroupForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var group = await _unitOfWork.Group.GetAllAsync()
                .Where(x => x.IsDeleted == false && x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (group is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            if(group.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This group has been deleted.");

            var updatedGroup = _mapper.Map(dto, group);
            updatedGroup.Id = id;
            updatedGroup.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Group.UpdateAsync(updatedGroup);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while updating the group: {ex}");
            throw;
        }
    }
}
