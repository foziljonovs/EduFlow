using EduFlow.BLL.DTOs.Users.Teacher;

namespace EduFlow.Desktop.Integrated.Services.Users.Teacher;

public interface ITeacherService
{
    Task<List<TeacherForResultDto>> GetAllAsync();
    Task<TeacherForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(TeacherForCreateDto dto);
    Task<bool> UpdateAsync(long id, TeacherForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<TeacherForResultDto> GetByUserIdAsync(long userId);
}
