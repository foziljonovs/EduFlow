using EduFlow.BLL.DTOs.Courses.Attendance;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Attendance;

public interface IAttendanceServer
{
    Task<List<AttendanceForResultDto>> GetAllAsync();
    Task<AttendanceForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(AttendanceForCraeteDto dto);
    Task<bool> UpdateAsync(long id, AttendanceForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<AttendanceForResultDto>> GetAllByStudentIdAsync(long studentId);
    Task<List<AttendanceForResultDto>> GetAllByCourseIdAsync(long courseId);
}
