using EduFlow.BLL.DTOs.Courses.Lesson;

namespace EduFlow.Desktop.Integrated.Services.Courses.Lesson;

public interface ILessonService
{
    Task<List<LessonForResultDto>> GetAllAsync();
    Task<LessonForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(LessonForCreateDto lesson);
    Task<bool> UpdateAsync(long id, LessonForUpdateDto lesson);
    Task<bool> DeleteAsync(long id);
    Task<List<LessonForResultDto>> GetByGroupIdAsync(long groupId);
}
