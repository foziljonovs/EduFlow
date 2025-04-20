using EduFlow.BLL.DTOs.Courses.StudentCourse;

namespace EduFlow.BLL.Interfaces.Courses;

public interface IStudentCourseService
{
    Task<IEnumerable<StudentCourseForResultDto>> GetAllAsync(CancellationToken cancellation = default);
    Task<StudentCourseForResultDto> GetByIdAsync(long id, CancellationToken cancellation = default);
    Task<bool> AddAsync(StudentCourseForCreateDto studentCourse, CancellationToken cancellation = default);
    Task<bool> UpdateAsync(long id, StudentCourseForUpdateDto studentCourse, CancellationToken cancellation = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellation = default);
    Task<bool> ExistsAsync(long id, CancellationToken cancellation = default);
}
