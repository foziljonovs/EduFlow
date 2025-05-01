using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Lesson;
using EduFlow.Desktop.Integrated.Servers.Repositories.Courses.Lesson;

namespace EduFlow.Desktop.Integrated.Services.Courses.Lesson;

public class LessonService : ILessonService
{
    private readonly ILessonServer _server;
    public LessonService()
    {
        this._server = new LessonServer();
    }
    public async Task<bool> AddAsync(LessonForCreateDto lesson)
    {
        try
        {
            var result = await _server.AddAsync(lesson);
            return result;
        }
        catch (Exception ex)
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
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<List<LessonForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<LessonForResultDto>();
        }
    }

    public async Task<List<LessonForResultDto>> GetByGroupIdAsync(long groupId)
    {
        try
        {
            var result = await _server.GetByGroupIdAsync(groupId);
            return result;
        }
        catch(Exception ex)
        {
            return new List<LessonForResultDto>();
        }
    }

    public async Task<LessonForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return new LessonForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, LessonForUpdateDto lesson)
    {
        try
        {
            var result = await _server.UpdateAsync(id, lesson);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
