using EduFlow.BLL.DTOs.Courses.StudentCourse;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.StudentCourse;
using EduFlow.Desktop.Integrated.Servers.Repositories.Courses.StudentCourse;

namespace EduFlow.Desktop.Integrated.Services.Courses.StudentCourse;

public class StudentCourseService : IStudentCourseService
{
    private readonly IStudentCourseServer _server;
    public StudentCourseService()
    {
        this._server = new StudentCourseServer();
    }
    public async Task<bool> AddAsync(StudentCourseForCreateDto studentCourse)
    {
        try
        {
            var res = await _server.AddAsync(studentCourse);
            return res;
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
            var res = await _server.DeleteAsync(id);
            return res;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> ExistsAsync(long id)
    {
        try
        {
            var res = await _server.ExistsAsync(id);
            return res;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<List<StudentCourseForResultDto>> GetAllAsync()
    {
        try
        {
            var res = await _server.GetAllAsync();
            return res;
        }
        catch (Exception ex)
        {
            return new List<StudentCourseForResultDto>();
        }
    }

    public async Task<StudentCourseForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var res = await _server.GetByIdAsync(id);
            return res;
        }
        catch(Exception ex)
        {
            return new StudentCourseForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, StudentCourseForUpdateDto studentCourse)
    {
        try
        {
            var res = await _server.UpdateAsync(id, studentCourse);
            return res;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
