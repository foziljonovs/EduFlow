using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.DTOs.Courses.StudentCourse;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Courses;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Courses;

public class StudentCourseService(
    IUnitOfWork unitOfWork,
    ILogger<StudentCourseService> logger,
    IStudentCourseValidator validator,
    IMapper mapper) : IStudentCourseService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<StudentCourseService> _logger = logger;
    private readonly IStudentCourseValidator _validator = validator;
    private readonly IMapper _mapper = mapper;
    public async Task<bool> AddAsync(StudentCourseForCreateDto studentCourse, CancellationToken cancellation = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(studentCourse);
            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsStudent = await _unitOfWork.Student.GetAsync(studentCourse.StudentId);
            if(existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, $"Student not found with id: {studentCourse.StudentId}");

            var existsCourse = await _unitOfWork.Course.GetAsync(studentCourse.CourseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, $"Course not found with id: {studentCourse.CourseId}");

            var savedStudentCourse = _mapper.Map<StudentCourse>(studentCourse);

            return await _unitOfWork.StudentCourse.AddConfirmAsync(savedStudentCourse);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while adding a student course: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            var existsStudentCourse = await _unitOfWork.StudentCourse.GetAsync(id);
            if (existsStudentCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, $"Student course not found with id: {id}");

            existsStudentCourse.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellation) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while deleting a student course: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            var existsStudentCourse = await _unitOfWork.StudentCourse.GetAsync(id);
            if (existsStudentCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, $"Student course not found with id: {id}");

            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while checking if a student course exists: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<StudentCourseForResultDto>> GetAllAsync(CancellationToken cancellation = default)
    {
        try
        {
            var studentCourses = await _unitOfWork.StudentCourse
                .GetAllAsync()
                .Where(x => !x.IsDeleted)
                .ToListAsync();

            if (!studentCourses.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student courses not found.");

            return _mapper.Map<IEnumerable<StudentCourseForResultDto>>(studentCourses);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while getting all student courses: {ex.Message}");
            throw;
        }
    }

    public async Task<StudentCourseForResultDto> GetByIdAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            var studentCourse = await _unitOfWork.StudentCourse.GetAsync(id);
            if (studentCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, $"Student course not found with id: {id}");

            return _mapper.Map<StudentCourseForResultDto>(studentCourse);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while getting a student course by id: {id}. {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, StudentCourseForUpdateDto studentCourse, CancellationToken cancellation = default)
    {
        try
        {
            var validationResult = await _validator.ValidateUpdate(studentCourse);
            if(!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsStudentCourse = await _unitOfWork.StudentCourse.GetAsync(id);
            if (existsStudentCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, $"Student course not found with id: {id}");

            if(existsStudentCourse.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This student course has been deleted.");

            var existsStudent = await _unitOfWork.Student.GetAsync(studentCourse.StudentId);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, $"Student not found with id: {studentCourse.StudentId}");

            var existsCourse = await _unitOfWork.Course.GetAsync(studentCourse.CourseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, $"Course not found with id: {studentCourse.CourseId}");

            _mapper.Map(studentCourse, existsStudentCourse);
            existsStudentCourse.Id = id;
            existsStudentCourse.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.StudentCourse.UpdateAsync(existsStudentCourse);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while updating a student course: {ex.Message}");
            throw;
        }
    }
}
