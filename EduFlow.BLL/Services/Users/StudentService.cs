using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Users.Interface;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.Interfaces.Users;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Users;
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

    public async Task<IEnumerable<StudentForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var students = await _unitOfWork.Student
                .GetAllAsync()
                .Where(x => x.IsDeleted == false)
                .ToListAsync(cancellationToken);

            if (!students.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Students not found.");

            return _mapper.Map<IEnumerable<StudentForResultDto>>(students);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while getting the students. {ex}");
            throw;
        }
    }

    public async Task<StudentForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var student = await _unitOfWork.Student.GetAsync(id);
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
                .GetAllAsync()
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
}
