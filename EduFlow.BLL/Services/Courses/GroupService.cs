using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Pagination;
using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Courses;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Courses;

public class GroupService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<GroupService> logger,
    IGroupValidator validator) : IGroupService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GroupService> _logger = logger;
    private readonly IGroupValidator _validator = validator;
    public async Task<bool> AddAsync(GroupForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsCourse = await _unitOfWork.Course.GetAsync(dto.CourseId);
            if(existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            var existsTeacher = await _unitOfWork.Teacher.GetAsync(dto.TeacherId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            if(existsTeacher.CourseId != dto.CourseId)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "This teacher does not teach this course.");

            if (existsCourse.IsDeleted)
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

    public async Task<bool> AddStudentsToGroupAsync(long id, List<long> students, CancellationToken cancellationToken = default)
    {
        try
        {
            var group = await _unitOfWork.Group.GetAllFullInformation()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (group is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            foreach (var item in students)
            {
                var student = await _unitOfWork.Student.GetAsync(item);

                if (student is not null && !group.Students.Any(x => x.Id == student.Id))
                {
                    var studentCourse = await _unitOfWork.StudentCourse.GetAllAsync()
                        .Where(x => x.StudentId == student.Id && x.CourseId == group.CourseId)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (studentCourse is null)
                        throw new StatusCodeException(HttpStatusCode.NotFound, "Student course not found.");

                    studentCourse.Status = Domain.Enums.EnrollmentStatus.Active;

                    group.Students.Add(student);
                }
            }

            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while adding students to the group: {ex}");
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
            group.IsStatus = Domain.Enums.Status.Deleted;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while deleting the group: {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteForStudentAsync(long id, long studentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var group = await _unitOfWork.Group.GetAllFullInformation()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if(group is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            var student = await _unitOfWork.Student.GetAsync(studentId);
            if(student is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            var studentCourse = await _unitOfWork.StudentCourse.GetAllAsync()
                .Where(x => x.StudentId == student.Id 
                    && x.CourseId == group.CourseId
                    && x.IsDeleted == false)
                .FirstOrDefaultAsync(cancellationToken);

            if (studentCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student course not found.");

            studentCourse.Status = Domain.Enums.EnrollmentStatus.Dropped;
            studentCourse.IsDeleted = true;

            group.Students.Remove(student);

            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while deleting the group for student: {ex}");
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

            if (dto.CourseId.HasValue)
                groupQuery = groupQuery
                    .Where(x => x.CourseId == dto.CourseId);

            if (dto.TeacherId.HasValue)
                groupQuery = groupQuery
                    .Where(x => x.TeacherId == dto.TeacherId);

            if(dto.CategoryId.HasValue)
                groupQuery = groupQuery
                    .Where(x => x.Course.CategoryId == dto.CategoryId);

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

    public async Task<PagedList<GroupForResultDto>> GetAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var groups = await _unitOfWork.Group
                .GetAllFullInformation()
                .Where(x => x.IsStatus == Domain.Enums.Status.Active)
                .ToListAsync(cancellationToken);

            if (!groups.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            var mappedGroups = groups
                .Select(g => _mapper.Map<GroupForResultDto>(g))
                .ToList();

            var pagedList = new PagedList<GroupForResultDto>(
                mappedGroups,
                mappedGroups.Count,
                pageNumber,
                pageSize);
            
            return pagedList.ToPagedList(mappedGroups, pageSize, pageNumber);
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

    public async Task<IEnumerable<GroupForResultDto>> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        try
        {
            var groups = await _unitOfWork.Group.GetAllFullInformation()
                .Where(x => x.TeacherId == teacherId)
                .ToListAsync(cancellationToken);

            if (!groups.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            return _mapper.Map<IEnumerable<GroupForResultDto>>(groups);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while getting all groups by teacher id: {ex}");
            throw;
        }
    }

    public async Task<GroupForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var group = await _unitOfWork.Group
                .GetAllFullInformation()
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
            var validationResult = await _validator.ValidateUpdate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var group = await _unitOfWork.Group.GetAllAsync()
                .Where(x => x.IsDeleted == false && x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (group is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            if(group.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This group has been deleted.");

            var existsCourse = await _unitOfWork.Course.GetAsync(dto.CourseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            var existsTeacher = await _unitOfWork.Teacher.GetAsync(dto.TeacherId);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

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
