using EduFlow.BLL.DTOs.Courses.Course;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Course;

public interface ICourseServer
{
    Task<List<CourseForResultDto>> GetAllAsync();
    Task<CourseForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(CourseForCreateDto dto);
    Task<bool> UpdateAsync(long id, CourseForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<CourseForResultDto>> GetAllByTeacherIdAsync(long teacherId);
    Task<List<CourseForResultDto>> GetAllByCategoryIdAsync(long categoryId);
    Task<List<CourseForResultDto>> FilterAsync(CourseForFilterDto dto);
    Task<bool> AddStudentsAsync(long id, List<long> studentIds);
}
