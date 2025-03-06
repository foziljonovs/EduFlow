using EduFlow.BLL.DTOs.Users.Teacher;

namespace EduFlow.BLL.Interfaces.Users;

public interface ITeacherService
{
    Task<IEnumerable<TeacherForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TeacherForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(TeacherForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TeacherForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
