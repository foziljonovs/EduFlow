using EduFlow.BLL.DTOs.Courses.Attendance;

namespace EduFlow.BLL.Interfaces.Courses;

public interface IAttendanceService
{
    Task<IEnumerable<AttendanceForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AttendanceForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(AttendanceForCraeteDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, AttendanceForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateRangeAsync(List<AttendanceForUpdateRangeDto> dtos, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AttendanceForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AttendanceForResultDto>> GetAllByLessonIdAsync(long lessonId, CancellationToken cancellationToken = default);
}
