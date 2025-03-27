using EduFlow.BLL.DTOs.Users.Student;

namespace EduFlow.Desktop.Integrated.Services.Users.Student;

public interface IStudentService
{
    Task<List<StudentForResultDto>> GetAllAsync();
    Task<StudentForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(StudentForCreateDto dto);
    Task<bool> UpdateAsync(long id, StudentForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<StudentForResultDto>> GetAllByTeacherIdAsync(long teacherId);
}
