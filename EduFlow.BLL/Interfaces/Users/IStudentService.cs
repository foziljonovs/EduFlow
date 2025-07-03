using EduFlow.BLL.Common.Pagination;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Domain.Enums;

namespace EduFlow.BLL.Interfaces.Users;

public interface IStudentService
{
    Task<PagedList<StudentForResultDto>> GetAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    Task<StudentForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(StudentForCreateDto dto, CancellationToken cancellationToken = default);
    Task<long> AddAndReturnIdAsync(StudentForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, StudentForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<StudentForResultDto> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentForResultDto>> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellationToken = default);
    Task<PagedList<StudentForResultDto>> GetAllByCategoryIdAsync(long categoryId, int pageSize, int pageNumber, CancellationToken cancellationToken = default); 
    Task<bool> AddStudentByGroupAsync(long studentId, long groupId, CancellationToken cancellationToken = default);
    Task<bool> UpdateStudentByGroupAsync(long id, long groupId, Status status, CancellationToken cancellationToken = default);
    Task<PagedList<StudentForResultDto>> GetAllByCourseIdAsync(long courseId, int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<StudentForResultDto>> FilterAsync(StudentForFilterDto dto, CancellationToken cancellation = default);
}
