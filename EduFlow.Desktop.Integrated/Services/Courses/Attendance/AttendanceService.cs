using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Attendance;
using EduFlow.Desktop.Integrated.Servers.Repositories.Courses.Attendance;

namespace EduFlow.Desktop.Integrated.Services.Courses.Attendance;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceServer _server;
    public AttendanceService()
    {
        this._server = new AttendanceServer();
    }
    public async Task<bool> AddAsync(AttendanceForCraeteDto dto)
    {
        try
        {
            var result = await _server.AddAsync(dto);
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
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<List<AttendanceForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<AttendanceForResultDto>();
        }
    }

    public async Task<List<AttendanceForResultDto>> GetAllByCourseIdAsync(long courseId)
    {
        try
        {
            var result = await _server.GetAllByCourseIdAsync(courseId);
            return result;
        }
        catch (Exception ex)
        {
            return new List<AttendanceForResultDto>();
        }
    }

    public async Task<List<AttendanceForResultDto>> GetAllByStudentIdAsync(long studentId)
    {
        try
        {
            var result = await _server.GetAllByStudentIdAsync(studentId);
            return result;
        }
        catch (Exception ex)
        {
            return new List<AttendanceForResultDto>();
        }
    }

    public async Task<AttendanceForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            return new AttendanceForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, AttendanceForUpdateDto dto)
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

    public async Task<bool> UpdateRangeAsync(List<AttendanceForUpdateRangeDto> dtos)
    {
        try
        {
            var result = await _server.UpdateRangeAsync(dtos);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}
