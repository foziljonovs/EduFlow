using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Domain.Enums;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Users.Student;

public interface IStudentServer
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
    Task<bool> UpdateStudentByGroupAsync(long id, long groupId, Status status);
    Task<List<StudentForResultDto>> GetAllByCourseIdAsync(long courseId);
    Task<List<StudentForResultDto>> FilterAsync(StudentForFilterDto dto);
    Task<PagedResponse<StudentForResultDto>> GetAllPaginationAsync(int pageSize, int pageNumber);
}
