using EduFlow.BLL.DTOs.Users.Student;

namespace EduFlow.BLL.Interfaces.Users;

public interface IStudentService
{
    Task<IEnumerable<StudentForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StudentForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(StudentForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, StudentForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<StudentForResultDto> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
}
