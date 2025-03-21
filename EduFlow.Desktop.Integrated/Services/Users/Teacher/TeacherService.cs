using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Users.Teacher;
using EduFlow.Desktop.Integrated.Servers.Repositories.Users.Teacher;

namespace EduFlow.Desktop.Integrated.Services.Users.Teacher;

public class TeacherService : ITeacherService
{
    private readonly ITeacherServer _server;
    public TeacherService()
    {
        this._server = new TeacherServer();
    }
    public async Task<bool> AddAsync(TeacherForCreateDto dto)
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

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var result = await   _server.DeleteAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<List<TeacherForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<TeacherForResultDto>();
        }
    }

    public async Task<TeacherForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            return new TeacherForResultDto();
        }
    }

    public async Task<TeacherForResultDto> GetByUserIdAsync(long userId)
    {
        try
        {
            var result = await _server.GetByUserIdAsync(userId);
            return result;
        }
        catch (Exception ex)
        {
            return new TeacherForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, TeacherForUpdateDto dto)
    {
        try
        {
            var result = await _server.UpdateAsync(id, dto);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
