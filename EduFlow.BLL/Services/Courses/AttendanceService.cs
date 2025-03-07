using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.Interfaces.Courses;

namespace EduFlow.BLL.Services.Courses;

public class AttendanceService : IAttendanceService
{
    public Task<bool> AddAsync(AttendanceForCraeteDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AttendanceForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AttendanceForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AttendanceForResultDto>> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AttendanceForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, AttendanceForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
