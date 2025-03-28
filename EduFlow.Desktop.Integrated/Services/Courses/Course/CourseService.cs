using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Course;
using EduFlow.Desktop.Integrated.Servers.Repositories.Courses.Course;

namespace EduFlow.Desktop.Integrated.Services.Courses.Course;

public class CourseService : ICourseService
{
    private readonly ICourseServer _server;
    public CourseService()
    {
        this._server  = new CourseServer();
    }
    public async Task<bool> AddAsync(CourseForCreateDto dto)
    {
        try
        {
            var res = await _server.AddAsync(dto);
            return res;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> AddStudentsAsync(long id, List<long> studentIds)
    {
        try
        {
            var res = await _server.AddStudentsAsync(id, studentIds);
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

    public async Task<List<CourseForResultDto>> FilterAsync(CourseForFilterDto dto)
    {
        try
        {
            var res = await _server.FilterAsync(dto);
            return res;
        }
        catch (Exception ex)
        {
            return new List<CourseForResultDto>();
        }
    }

    public async Task<List<CourseForResultDto>> GetAllAsync()
    {
        try
        {
            var res = await _server.GetAllAsync();
            return res;
        }
        catch (Exception ex)
        {
            return new List<CourseForResultDto>();
        }
    }

    public async Task<List<CourseForResultDto>> GetAllByCategoryIdAsync(long categoryId)
    {
        try
        {
            var res = await _server.GetAllByCategoryIdAsync(categoryId);
            return res;
        }
        catch (Exception ex)
        {
            return new List<CourseForResultDto>();
        }
    }

    public async Task<List<CourseForResultDto>> GetAllByTeacherIdAsync(long teacherId)
    {
        try
        {
            var res = await _server.GetAllByTeacherIdAsync(teacherId);
            return res;
        }
        catch (Exception ex)
        {
            return new List<CourseForResultDto>();
        }
    }

    public async Task<CourseForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var res = await _server.GetByIdAsync(id);
            return res;
        }
        catch (Exception ex)
        {
            return new CourseForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, CourseForUpdateDto dto)
    {
        try
        {
            var res = await _server.UpdateAsync(id, dto);
            return res;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
