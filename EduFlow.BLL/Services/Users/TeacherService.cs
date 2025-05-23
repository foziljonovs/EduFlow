﻿using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Validators.Users.Interface;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.BLL.Interfaces.Users;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Users;

public class TeacherService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<TeacherService> logger,
    ITeacherValidator validator) : ITeacherService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<TeacherService> _logger = logger;
    private readonly ITeacherValidator _validator = validator;
    public async Task<bool> AddAsync(TeacherForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateCreate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsUser = await _unitOfWork.User.GetAsync(dto.UserId);
            if (existsUser is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "User not found.");

            var existsTeacher = await _unitOfWork.Teacher.GetAllAsync().FirstOrDefaultAsync(x => x.UserId == dto.UserId);
            if (existsTeacher is not null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "Teacher already exists.");

            var existsCourse = await _unitOfWork.Course.GetAsync(dto.CourseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            var savedTeacher = _mapper.Map<Teacher>(dto);
            return await _unitOfWork.Teacher.AddConfirmAsync(savedTeacher);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while adding the teacher. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsTeacher = await _unitOfWork.Teacher.GetAsync(id);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            existsTeacher.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured while delete the teacher. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<TeacherForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var teachers = await _unitOfWork.Teacher
                .GetAllFullInformation()
                .ToListAsync(cancellationToken);

            if (!teachers.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teachers not found.");

            return _mapper.Map<IEnumerable<TeacherForResultDto>>(teachers);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all the teacher. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<TeacherForResultDto>> GetByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var courses = await _unitOfWork.Teacher.GetAllFullInformation()
                .Where(x => x.CourseId == courseId)
                .ToListAsync(cancellationToken);

            if(!courses.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teachers not found.");

            return _mapper.Map<IEnumerable<TeacherForResultDto>>(courses);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get by course id the teacher. {ex}");
            throw;
        }
    }

    public async Task<TeacherForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var teacher = await _unitOfWork.Teacher
                .GetAllFullInformation()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (teacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            if (teacher.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This teacher has been deleted.");

            return _mapper.Map<TeacherForResultDto>(teacher);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get by id the teacher. {ex}");
            throw;
        }
    }

    public async Task<TeacherForResultDto> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var teacher = await _unitOfWork.Teacher
                .GetAllFullInformation()
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if(teacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            if (teacher.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This teacher has been deleted.");

            return _mapper.Map<TeacherForResultDto>(teacher);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get by user id the teacher. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, TeacherForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await _validator.ValidateUpdate(dto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsTeacher = await _unitOfWork.Teacher.GetAsync(id);
            if (existsTeacher is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Teacher not found.");

            if (existsTeacher.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This teacher has been deleted.");

            var existsCourse = await _unitOfWork.Course.GetAsync(dto.CourseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            if (existsCourse.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This course has been deleted.");

            var existsUser = await _unitOfWork.User.GetAsync(dto.UserId);
            if (existsUser is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "User not found.");

            _mapper.Map(dto, existsTeacher);
            existsTeacher.Id = id;
            existsTeacher.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Teacher.UpdateAsync(existsTeacher);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the teacher. {ex}");
            throw;
        }
    }
}
