using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Courses.Interface;
using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Courses;
using FluentValidation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Courses;

public class CourseService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CourseService> logger,
    ICourseValidator validator) : ICourseService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CourseService> _logger = logger;
    private readonly ICourseValidator _validator = validator;
    public async Task<bool> AddAsync(CourseForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var teacherExists = await _unitOfWork.Teacher.GetAsync(dto.TeacherId);
            if (teacherExists is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            var categoryExists = await _unitOfWork.Category.GetAsync(dto.CategoryId);
            if (categoryExists is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found.");

            var savedCourse = _mapper.Map<Course>(dto);

            foreach(var item in dto.StudentIds)
            {
                var studentExists = await _unitOfWork.Student.GetAsync(item);
                if(studentExists is not null)
                    savedCourse.Students.Add(studentExists);
            }

            return await _unitOfWork.Course.AddConfirmAsync(savedCourse);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured adding the course. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var course = await _unitOfWork.Course.GetAsync(id);
            if (course is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            course.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the course id: {id}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CourseForResultDto>> FilterAsync(CourseForFilterDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var courseQuery = _unitOfWork.Course
                .GetAllFullInformation();

            if(!courseQuery.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            if(dto.StartedDate.HasValue && dto.FinishedDate.HasValue)
                courseQuery = courseQuery.Where(x => 
                    x.CreatedAt >= dto.StartedDate && 
                    x.CreatedAt <= dto.FinishedDate);

            if(dto.CategoryId > 0)
                courseQuery = courseQuery
                    .Where(x => x.CategoryId == dto.CategoryId);

            if(dto.TeacherId > 0)
                courseQuery = courseQuery
                    .Where(x => x.TeacherId == dto.TeacherId);

            var courses = await courseQuery.ToListAsync(cancellationToken);

            if(!courses.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            return _mapper.Map<IEnumerable<CourseForResultDto>>(courses);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while filter the course. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CourseForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var courses = await _unitOfWork.Course
                .GetAllFullInformation()
                .ToListAsync();

            if (!courses.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Courses not found.");

            return _mapper.Map<IEnumerable<CourseForResultDto>>(courses);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all the course. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CourseForResultDto>> GetAllByCategoryIdAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsCategory = await _unitOfWork.Category.GetAsync(categoryId);
            if (existsCategory is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found.");

            var courses = await _unitOfWork.Course
                .GetAllFullInformation()
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();

            if (!courses.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Courses not found.");

            return _mapper.Map<IEnumerable<CourseForResultDto>>(courses);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all course by category id: {categoryId}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CourseForResultDto>> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsTeacher = await _unitOfWork.Teacher.GetAsync(teacherId);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            var courses = await _unitOfWork.Course
                .GetAllFullInformation()
                .Where(x => x.TeacherId == teacherId)
                .ToListAsync();

            if (!courses.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Courses not found.");

            return _mapper.Map<IEnumerable<CourseForResultDto>>(courses);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all course by teacher id: {teacherId}. {ex}");
            throw;
        }
    }

    public async Task<CourseForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var course = await _unitOfWork.Course.GetAsync(id);
            if (course is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            if (course.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This course has been deleted.");

            return _mapper.Map<CourseForResultDto>(course);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get course by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, CourseForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateUpdate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsCourse = await _unitOfWork.Course.GetAsync(id);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            if (existsCourse.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This course has been deleted.");

            var existsTeacher = await _unitOfWork.Teacher.GetAsync(dto.TeacherId);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            var existsCategory = await _unitOfWork.Category.GetAsync(dto.CategoryId);
            if (existsCategory is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found.");

            _mapper.Map(dto, existsCourse);
            existsCourse.Id = id;
            existsCourse.UpdatedAt = DateTime.UtcNow.AddHours(5);

            foreach (var item in dto.StudentIds)
            {
                var studentExists = await _unitOfWork.Student.GetAsync(item);
                if (studentExists is not null)
                    existsCourse.Students.Add(studentExists);
            }

            return await _unitOfWork.Course.UpdateAsync(existsCourse);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the course id: {id}. {ex}");
            throw;
        }
    }
}
