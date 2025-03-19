using EduFlow.BLL.DTOs.Users.Teacher;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Users.Teacher;

public interface ITeacherServer
{
    Task<List<TeacherForResultDto>> GetAllAsync();
    Task<TeacherForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(TeacherForCreateDto dto);
    Task<bool> UpdateAsync(long id, TeacherForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
}
