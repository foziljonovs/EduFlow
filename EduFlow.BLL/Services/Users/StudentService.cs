using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Pagination;
using EduFlow.BLL.Common.Validators.Users.Interface;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.Interfaces.Users;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Users;

public class StudentService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<StudentService> logger,
    IStudentValidator validator) : IStudentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<StudentService> _logger = logger;
    private readonly IStudentValidator _validator = validator;

    public async Task<long> AddAndReturnIdAsync(StudentForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsUser = await _unitOfWork.Student
                .GetAllAsync()
                .FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);
            
            if (existsUser is not null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student already exists.");
            
            var savedStudent = _mapper.Map<Student>(dto);
            
            return await _unitOfWork.Student.AddAsync(savedStudent);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while adding the student. {ex}");
            throw;
        }
    }

    public async Task<bool> AddAsync(StudentForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsUser = await _unitOfWork.Student
                .GetAllAsync()
                .FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);

            if (existsUser is not null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student already exists.");

            var savedStudent = _mapper.Map<Student>(dto);
            return await _unitOfWork.Student.AddConfirmAsync(savedStudent);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while adding the student. {ex}");
            throw;
        }
    }

    public async Task<bool> AddStudentByGroupAsync(long studentId, long groupId, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsStudent = await _unitOfWork.Student.GetAsync(studentId);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            var existsCourse = await _unitOfWork.Group.GetAsync(groupId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            existsStudent.Groups.Add(existsCourse);
            var res = await _unitOfWork.Student.UpdateAsync(existsStudent);

            return res;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while add student by course - id: {groupId}. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsStudent = await _unitOfWork.Student.GetAsync(id);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            existsStudent.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured delete the student {id}. {ex}");
            throw;
        }
    }

    public async Task<PagedList<StudentForResultDto>> FilterAsync(StudentForFilterDto dto, int pageSize, int pageNumber, CancellationToken cancellation = default)
    {
        try
        {
            var studentsQuery = _unitOfWork.Student.GetAllFullInformation();

            if (dto.StartedDate.HasValue && dto.FinishedDate.HasValue)
            {
                var startedDateUtc = DateTime.SpecifyKind(dto.StartedDate.Value, DateTimeKind.Utc);
                var finishedDateUtc = DateTime.SpecifyKind(dto.FinishedDate.Value, DateTimeKind.Utc);

                studentsQuery = studentsQuery.Where(x =>
                    x.CreatedAt.Date >= startedDateUtc &&
                    x.CreatedAt.Date <= finishedDateUtc);
            }

            if (dto.CourseId > 0)
            {
                studentsQuery = studentsQuery
                    .Where(x => x.StudentCourses.Any(sc => sc.CourseId == dto.CourseId));
            }

            if (dto.CourseStatus.HasValue)
            {
                studentsQuery = studentsQuery
                    .Where(x => x.StudentCourses.Any(sc => sc.Status == dto.CourseStatus.Value));
            }

            if (!studentsQuery.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Students not found.");

            var mappedStudents = studentsQuery
               .Select(p => _mapper.Map<StudentForResultDto>(p))
               .ToList();

            var pagedlist = new PagedList<StudentForResultDto>(
                mappedStudents,
                mappedStudents.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedStudents, pageSize, pageNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while filtering the students. {ex}");
            throw;
        }
    }


    public async Task<PagedList<StudentForResultDto>> GetAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var students = await _unitOfWork.Student
                .GetAllFullInformation()
                .ToListAsync(cancellationToken);

            if (!students.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Students not found.");

            var mappedStudents = students
               .Select(p => _mapper.Map<StudentForResultDto>(p))
               .ToList();

            var pagedlist = new PagedList<StudentForResultDto>(
                mappedStudents,
                mappedStudents.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedStudents, pageSize, pageNumber);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while getting the students. {ex}");
            throw;
        }
    }

    public async Task<PagedList<StudentForResultDto>> GetAllByCategoryIdAsync(long categoryId, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsCategory = await _unitOfWork.Category.GetAsync(categoryId);
            if(existsCategory is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found.");

            var students = await _unitOfWork.Student
                .GetAllFullInformation()
                //.Where(x => x.Groups.Any(x => x.CategoryId == existsCategory.Id))
                .ToListAsync(cancellationToken);

            if(!students.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Students not found.");

            var mappedStudents = students
               .Select(p => _mapper.Map<StudentForResultDto>(p))
               .ToList();

            var pagedlist = new PagedList<StudentForResultDto>(
                mappedStudents,
                mappedStudents.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedStudents, pageSize, pageNumber);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all by category id: {categoryId}. {ex}");
            throw;
        }
    }

    public async Task<PagedList<StudentForResultDto>> GetAllByCourseIdAsync(long courseId, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsCourse = await _unitOfWork.Course.GetAsync(courseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            var students = await _unitOfWork.Student
                .GetAllFullInformation()
                .Where(x => x.StudentCourses.Any(x => x.CourseId == courseId))
                .ToListAsync(cancellationToken);

            if (!students.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Students not found.");

            var mappedStudents = students
                .Select(p => _mapper.Map<StudentForResultDto>(p))
                .ToList();

            var pagedlist = new PagedList<StudentForResultDto>(
                mappedStudents,
                mappedStudents.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedStudents, pageSize, pageNumber);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while getting by course id: {courseId}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<StudentForResultDto>> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsTeacher = await _unitOfWork.Teacher.GetAsync(teacherId);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            var students = await _unitOfWork.Student
                .GetAllFullInformation()
                //.Where(x => x.Courses.Any(x => x.TeacherId == existsTeacher.Id))
                .ToListAsync(cancellationToken);

            if (!students.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Students not found.");

            return _mapper.Map<IEnumerable<StudentForResultDto>>(students);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all by teacher id: {teacherId}. {ex}");
            throw;
        }
    }

    public async Task<PagedList<StudentForResultDto>> GetAllPaginationByTeacherIdAsync(long teacherId, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsTeacher = await _unitOfWork.Teacher.GetAsync(teacherId);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            var students = await _unitOfWork.Student.GetAllFullInformation()
                //.Where(x => x.StudentCourses.Any(x => x.TeacherId == existsTeacher.Id);
                .ToListAsync(cancellationToken);

            if (!students.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Students not found.");

            var mappedStudents = students
                .Select(p => _mapper.Map<StudentForResultDto>(p))
                .ToList();

            var pagedlist = new PagedList<StudentForResultDto>(
                mappedStudents,
                mappedStudents.Count,
                pageNumber,
                pageSize);

            return pagedlist.ToPagedList(mappedStudents, pageSize, pageNumber);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while getting the student by teacher id: {teacherId}");
            throw;
        }
    }

    public async Task<StudentForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _unitOfWork.Student.GetAllFullInformation()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (student is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            if (student.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This student has been deleted.");

            return _mapper.Map<StudentForResultDto>(student);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<StudentForResultDto> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _unitOfWork.Student
                .GetAllFullInformation()
                .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

            if (student is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            if (student.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This student has been deleted");

            return _mapper.Map<StudentForResultDto>(student);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while getting student by phone number: {phoneNumber}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, StudentForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateUpdate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsStudent = await _unitOfWork.Student.GetAsync(id);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            if (existsStudent.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This student has been deleted.");

            _mapper.Map(dto, existsStudent);
            existsStudent.Id = id;
            existsStudent.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Student.UpdateAsync(existsStudent);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the student id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateStudentByGroupAsync(long id, long groupId, Status status, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsStudent = await _unitOfWork.Student.GetAllFullInformation()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            var existsGroup = await _unitOfWork.Group.GetAsync(groupId);

            if (existsGroup is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Group not found.");

            existsStudent.Groups.Where(x => x.Id == groupId).FirstOrDefault().IsStatus = status;
            existsStudent.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Student.UpdateAsync(existsStudent);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update student by group id: {id}. {ex}");
            throw;
        }
    }
}
