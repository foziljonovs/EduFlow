using EduFlow.BLL.DTOs.Users.Student;

namespace EduFlow.Desktop.Integrated.Services.Users.Student;

public interface IStudentService
{
    Task<List<StudentForResultDto>> GetAllAsync();
    Task<StudentForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(StudentForCreateDto dto);
    Task<long> AddAndReturnIdAsync(StudentForCreateDto dto);
    Task<bool> UpdateAsync(long id, StudentForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<StudentForResultDto> GetByPhoneNumberAsync(string phoneNumber);
    Task<List<StudentForResultDto>> GetAllByTeacherIdAsync(long teacherId);
    Task<List<StudentForResultDto>> GetAllByCategoryIdAsync(long categoryId);
    Task<bool> AddStudentByCourseAsync(long studentId, long courseId);
    Task<List<StudentForResultDto>> GetAllByCourseIdAsync(long courseId);
    Task<List<StudentForResultDto>> FilterAsync(StudentForFilterDto dto);
}
