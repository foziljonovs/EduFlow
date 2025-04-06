using EduFlow.BLL.DTOs.Courses.Group;

namespace EduFlow.BLL.Interfaces.Courses;

public interface IGroupService
{
    Task<List<GroupForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<GroupForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(GroupForCreateDto groupForCreationDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, GroupForUpdateDto groupForUpdateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<List<GroupForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default);
    Task<List<GroupForResultDto>> FilterAsync(GroupForFilterDto dto, CancellationToken cancellationToken = default);
}
