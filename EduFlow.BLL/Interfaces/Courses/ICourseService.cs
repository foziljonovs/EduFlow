using EduFlow.BLL.DTOs.Courses.Course;

namespace EduFlow.BLL.Interfaces.Courses;

public interface ICourseService
{
    Task<IEnumerable<CourseForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CourseForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(CourseForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, CourseForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseForResultDto>> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseForResultDto>> GetAllByCategoryIdAsync(long categoryId, CancellationToken cancellationToken = default);
}
