﻿using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Users.Student;
using EduFlow.Desktop.Integrated.Servers.Repositories.Users.Student;

namespace EduFlow.Desktop.Integrated.Services.Users.Student;

public class StudentService : IStudentService
{
    private readonly IStudentServer _server;
    public StudentService()
    {
        this._server = new StudentServer();
    }
    public async Task<bool> AddAsync(StudentForCreateDto dto)
    {
        try
        {
            var result = await _server.AddAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> AddStudentByCourseAsync(long studentId, long courseId)
    {
        try
        {
            var result = await _server.AddStudentByCourseAsync(studentId, courseId);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var result = await _server.DeleteAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<List<StudentForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<StudentForResultDto>();
        }
    }

    public async Task<List<StudentForResultDto>> GetAllByCategoryIdAsync(long categoryId)
    {
        try
        {
            var res = await _server.GetAllByCategoryIdAsync(categoryId);
            return res;
        }
        catch(Exception ex)
        {
            return new List<StudentForResultDto>();
        }
    }

    public async Task<List<StudentForResultDto>> GetAllByTeacherIdAsync(long teacherId)
    {
        try
        {
            var result = await _server.GetAllByTeacherIdAsync(teacherId);
            return result;
        }
        catch (Exception ex)
        {
            return new List<StudentForResultDto>();
        }
    }

    public async Task<StudentForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return new StudentForResultDto();
        }
    }

    public async Task<StudentForResultDto> GetByPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            var result = await _server.GetByPhoneNumberAsync(phoneNumber);
            return result;
        }
        catch (Exception ex)
        {
            return new StudentForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, StudentForUpdateDto dto)
    {
        try
        {
            var result = await _server.UpdateAsync(id, dto);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}
