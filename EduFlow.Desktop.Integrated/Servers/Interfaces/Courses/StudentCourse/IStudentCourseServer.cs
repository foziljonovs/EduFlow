using EduFlow.BLL.DTOs.Courses.StudentCourse;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.StudentCourse;

public interface IStudentCourseServer
{
    Task<List<StudentCourseForResultDto>> GetAllAsync();
    Task<StudentCourseForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(StudentCourseForCreateDto studentCourse);
    Task<bool> UpdateAsync(long id, StudentCourseForUpdateDto studentCourse);
    Task<bool> DeleteAsync(long id);
    Task<bool> ExistsAsync(long id);
}
