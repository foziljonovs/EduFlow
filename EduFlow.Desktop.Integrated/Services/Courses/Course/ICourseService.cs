using EduFlow.BLL.DTOs.Courses.Course;

namespace EduFlow.Desktop.Integrated.Services.Courses.Course;

public interface ICourseService
{
    Task<List<CourseForResultDto>> GetAllAsync();
    Task<CourseForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(CourseForCreateDto dto);
    Task<bool> UpdateAsync(long id, CourseForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<CourseForResultDto>> GetAllByTeacherIdAsync(long teacherId);
    Task<List<CourseForResultDto>> GetAllByCategoryIdAsync(long categoryId);
}
