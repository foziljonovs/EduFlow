using EduFlow.BLL.Common.Pagination;
using EduFlow.BLL.DTOs.Courses.Group;

namespace EduFlow.BLL.Interfaces.Courses;

public interface IGroupService
{
    Task<PagedList<GroupForResultDto>> GetAllAsync(int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    Task<GroupForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(GroupForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, GroupForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupForResultDto>> GetAllByTeacherIdAsync(long teacherId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupForResultDto>> FilterAsync(GroupForFilterDto dto, CancellationToken cancellationToken = default);
    Task<bool> AddStudentsToGroupAsync(long id, List<long> students, CancellationToken cancellationToken = default);
    Task<bool> DeleteForStudentAsync(long id, long studentId, CancellationToken cancellationToken = default);
}
