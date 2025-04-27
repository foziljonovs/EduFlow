using EduFlow.BLL.DTOs.Courses.Lesson;

namespace EduFlow.BLL.Interfaces.Courses;

public interface ILessonService
{
    Task<IEnumerable<LessonForResultDto>> GetAllAsync(CancellationToken cancellation = default);
    Task<LessonForResultDto> GetByIdAsync(long id, CancellationToken cancellation = default);
    Task<bool> AddAsync(LessonForCreateDto lesson, CancellationToken cancellation = default);
    Task<bool> UpdateAsync(LessonForUpdateDto lesson, CancellationToken cancellation = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellation = default);
}
