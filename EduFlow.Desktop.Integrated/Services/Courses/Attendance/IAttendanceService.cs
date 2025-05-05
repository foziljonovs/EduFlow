using EduFlow.BLL.DTOs.Courses.Attendance;

namespace EduFlow.Desktop.Integrated.Services.Courses.Attendance;

public interface IAttendanceService
{
    Task<List<AttendanceForResultDto>> GetAllAsync();
    Task<AttendanceForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(AttendanceForCraeteDto dto);
    Task<bool> UpdateAsync(long id, AttendanceForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<AttendanceForResultDto>> GetAllByStudentIdAsync(long studentId);
    Task<List<AttendanceForResultDto>> GetAllByCourseIdAsync(long courseId);
    Task<bool> UpdateRangeAsync(List<AttendanceForUpdateRangeDto> dtos);
}
