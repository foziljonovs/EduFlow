using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Group;
using EduFlow.Desktop.Integrated.Servers.Repositories.Courses.Group;

namespace EduFlow.Desktop.Integrated.Services.Courses.Group;

public class GroupService : IGroupService
{
    private readonly IGroupServer _server;
    public GroupService()
    {
        this._server = new GroupServer();
    }
    public async Task<bool> AddAsync(GroupForCreateDto dto)
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

    public async Task<bool> AddStudentsToGroupAsync(long groupId, List<long> studentIds)
    {
        try
        {
            var res = await _server.AddStudentsToGroupAsync(groupId, studentIds);
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

    public async Task<bool> DeleteForStudentAsync(long id, long studentId)
    {
        try
        {
            var res = await _server.DeleteForStudentAsync(id, studentId);
            return res;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<List<GroupForResultDto>> FilterAsync(GroupForFilterDto dto)
    {
        try
        {
            var res = await _server.FilterAsync(dto);
            return res;
        }
        catch (Exception ex)
        {
            return new List<GroupForResultDto>();
        }
    }

    public async Task<List<GroupForResultDto>> GetAllAsync()
    {
        try
        {
            var res = await _server.GetAllAsync();
            return res;
        }
        catch (Exception ex)
        {
            return new List<GroupForResultDto>();
        }
    }

    public async Task<List<GroupForResultDto>> GetAllByCourseIdAsync(long courseId)
    {
        try
        {
            var res = await _server.GetAllByCourseIdAsync(courseId);
            return res;
        }
        catch (Exception ex)
        {
            return new List<GroupForResultDto>();
        }
    }

    public async Task<List<GroupForResultDto>> GetAllByTeacherIdAsync(long teacherId)
    {
        try
        {
            var res = await _server.GetAllByTeacherIdAsync(teacherId);
            return res;
        }
        catch(Exception ex)
        {
            return new List<GroupForResultDto>();
        }
    }

    public async Task<GroupForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var res = await _server.GetByIdAsync(id);
            return res;
        }
        catch (Exception ex)
        {
            return new GroupForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, GroupForUpdateDto dto)
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
