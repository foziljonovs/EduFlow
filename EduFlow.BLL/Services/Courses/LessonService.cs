using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Courses.Interfaces;
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
    ILessonValidator validator) : ILessonService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<LessonService> _logger = logger;
    private readonly ILessonValidator _validator = validator;
    public async Task<bool> AddAsync(LessonForCreateDto lesson, CancellationToken cancellation = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCraete(lesson);
            if (validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsGroup = await _unitOfWork.Course.GetAsync(lesson.GroupId);
            if (existsGroup is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            var savedLesson = _mapper.Map<Lesson>(lesson);

            return await _unitOfWork.Lesson.AddConfirmAsync(savedLesson);
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

    public async Task<LessonForResultDto> GetByIdAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            var lesson = await _unitOfWork.Lesson
                .GetAllAsync()
                .Where(x => x.Id == id && !x.IsDeleted)
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
            var validationResult = await _validator.ValidateUpdate(lesson);
            if (validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsLesson = await _unitOfWork.Lesson.GetAllAsync()
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellation);

            if (existsLesson is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lesson not found.");

            if (existsLesson.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This lesson has been deleted.");

            var savedLesson = _mapper.Map(lesson, existsLesson);
            savedLesson.Id = id;

            return await _unitOfWork.Lesson.UpdateAsync(savedLesson);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the lesson. {ex}");
            throw;
        }
    }
}
