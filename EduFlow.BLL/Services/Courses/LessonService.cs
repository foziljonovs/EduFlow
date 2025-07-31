using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Courses;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Courses;

public class LessonService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<LessonService> logger,
    ILessonValidator validator,
    IAttendanceService attendanceService) : ILessonService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<LessonService> _logger = logger;
    private readonly ILessonValidator _validator = validator;
    private readonly IAttendanceService _attendanceService = attendanceService;
    public async Task<bool> AddAsync(LessonForCreateDto lesson, CancellationToken cancellation = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCraete(lesson);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsGroup = await _unitOfWork.Group.GetAllFullInformation()
                .Where(x => x.Id == lesson.GroupId)
                .FirstOrDefaultAsync();

            if (existsGroup is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            var mappedLesson = _mapper.Map<Lesson>(lesson);

            var savedLessonId = await _unitOfWork.Lesson.AddAsync(mappedLesson);

            foreach(var groupStudent in existsGroup.Students)
            {
                var attendance = new AttendanceForCraeteDto
                {
                    StudentId = groupStudent.Id,
                    LessonId = savedLessonId,
                    Date = DateTime.UtcNow.AddHours(5),
                    IsActived = false
                };

                var attendanceRes = await _attendanceService.AddAsync(attendance, cancellation);
            }

            return savedLessonId > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while adding the lesson. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            var existsLesson = await _unitOfWork.Lesson.GetAsync(id);
            if (existsLesson is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lesson not found.");

            if (existsLesson.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This lesson has been deleted.");

            existsLesson.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellation) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the lesson id: {id}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<LessonForResultDto>> GetAllAsync(CancellationToken cancellation = default)
    {
        try
        {
            var lessons = await _unitOfWork.Lesson.GetAllFullInformation()
                .ToListAsync(cancellation);

            if (!lessons.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lessons not found.");

            return _mapper.Map<IEnumerable<LessonForResultDto>>(lessons);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all by lesson. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<LessonForResultDto>> GetAllByGroupIdAsync(long groupId, CancellationToken cancellation = default)
    {
        try
        {
            var lessons = await _unitOfWork.Lesson
                .GetAllFullInformation()
                .Where(x => x.GroupId == groupId)
                .ToListAsync(cancellation);

            if (!lessons.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lessons not found.");

            return _mapper.Map<IEnumerable<LessonForResultDto>>(lessons);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all by lesson. {ex}");
            throw;
        }
    }

    public async Task<LessonForResultDto> GetByIdAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            var lesson = await _unitOfWork.Lesson
                .GetAllFullInformation()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellation);

            if (lesson is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lesson not found.");

            return _mapper.Map<LessonForResultDto>(lesson);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while get by lesson id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, LessonForUpdateDto lesson, CancellationToken cancellation = default)
    {
        try
        {
            //var validationResult = await _validator.ValidateUpdate(lesson);
            //if (validationResult.IsValid)
            //    throw new ValidationException(validationResult.Errors);

            var existsLesson = await _unitOfWork.Lesson.GetAllAsync()
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellation);

            if (existsLesson is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lesson not found.");

            if (existsLesson.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This lesson has been deleted.");

            _mapper.Map(lesson, existsLesson);
            existsLesson.Id = id;
            existsLesson.UpdatedAt = DateTime.UtcNow.AddHours(5);
            existsLesson.Date = lesson.Date.Date;

            return await _unitOfWork.Lesson.UpdateAsync(existsLesson);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the lesson. {ex}");
            throw;
        }
    }
}
