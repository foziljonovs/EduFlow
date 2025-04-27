using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Courses;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Courses;

public class AttendanceService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<AttendanceService> logger,
    IAttendanceValidator validator) : IAttendanceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<AttendanceService> _logger = logger;
    private readonly IAttendanceValidator _validator = validator;
    public async Task<bool> AddAsync(AttendanceForCraeteDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsLesson = await _unitOfWork.Lesson.GetAsync(dto.LessonId);
            if(existsLesson is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lesson not found.");

            var existsStudent = await _unitOfWork.Student.GetAsync(dto.StudentId);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            var savedAttendance = _mapper.Map<Attendance>(dto);
            return await _unitOfWork.Attendance.AddConfirmAsync(savedAttendance);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while added the attendance. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var attendance = await _unitOfWork.Attendance.GetAsync(id);
            if (attendance is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Attendance not found.");

            attendance.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the attendance id: {id}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<AttendanceForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var attendances = await _unitOfWork.Attendance
                .GetAllAsync()
                .Where(x => x.IsDeleted == false)
                .ToListAsync();

            if (!attendances.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Attendances not found.");

            return _mapper.Map<IEnumerable<AttendanceForResultDto>>(attendances);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all attendance. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<AttendanceForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var attendances = await _unitOfWork.Attendance
                .GetAllAsync()
                .Where(x => x.StudentId == studentId && x.IsDeleted == false)
                .ToListAsync();

            if (!attendances.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Attendaces not found.");

            return _mapper.Map<IEnumerable<AttendanceForResultDto>>(attendances);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all attendance by student id: {studentId}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<AttendanceForResultDto>> GetAllByLessonIdAsync(long lessonId, CancellationToken cancellationToken = default)
    {
        try
        {
            var attendances = await _unitOfWork.Attendance
                .GetAllAsync()
                .Where(x => x.LessonId == lessonId)
                .ToListAsync();

            if (!attendances.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Attendances not found.");

            return _mapper.Map<IEnumerable<AttendanceForResultDto>>(attendances);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all attendance by course id: {lessonId}. {ex}");
            throw;
        }
    }

    public async Task<AttendanceForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var attendance = await _unitOfWork.Attendance.GetAsync(id);
            if (attendance is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Attendance not found.");

            if (attendance.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This attendance has been deleted.");

            return _mapper.Map<AttendanceForResultDto>(attendance);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get attendance by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, AttendanceForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateUpdate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsAttendance = await _unitOfWork.Attendance.GetAsync(id);
            if (existsAttendance is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Attendance not found.");

            var existsLesson = await _unitOfWork.Lesson.GetAsync(dto.LessonId);
            if (existsLesson is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lesson not found.");

            var existsStudent = await _unitOfWork.Student.GetAsync(dto.StudentId);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            _mapper.Map(dto, existsAttendance);
            existsAttendance.Id = id;
            existsAttendance.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Attendance.UpdateAsync(existsAttendance);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update attendance id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateRangeAsync(List<AttendanceForUpdateDto> dtos, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!dtos.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Attendance not found.");

            var existsAttendances = await _unitOfWork.Attendance
                .GetAllAsync()
                .Where(x => dtos.Select(x => x.Id).Contains(x.Id))
                .ToListAsync();

            if (!existsAttendances.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Attendance not found.");

            var existsLessons = await _unitOfWork.Lesson
                .GetAllAsync()
                .Where(x => dtos.Select(x => x.LessonId).Contains(x.Id))
                .ToListAsync();

            if (!existsLessons.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Lesson not found.");

            var savedAttendances = _mapper.Map<List<Attendance>>(dtos);
            foreach (var attendance in existsAttendances)
            {
                var updatedAttendance = savedAttendances.FirstOrDefault(x => x.Id == attendance.Id);
                if (updatedAttendance != null)
                {
                    attendance.IsActived = updatedAttendance.IsActived;
                    attendance.UpdatedAt = DateTime.UtcNow.AddHours(5);
                }
            }

            var result = await _unitOfWork.Attendance.UpdateRangeAsync(existsAttendances);

            return result;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update range attendance. {ex}");
            throw;
        }
    }
}
