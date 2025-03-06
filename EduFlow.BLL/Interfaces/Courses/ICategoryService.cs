using EduFlow.BLL.DTOs.Courses.Category;

namespace EduFlow.BLL.Interfaces.Courses;

public interface ICategoryService
{
    Task<IEnumerable<CategoryForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddAsync(CategoryForCraeteDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(long id, CategoryForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
