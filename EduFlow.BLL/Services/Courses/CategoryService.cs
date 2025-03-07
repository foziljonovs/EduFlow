using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.BLL.Interfaces.Courses;

namespace EduFlow.BLL.Services.Courses;

public class CategoryService : ICategoryService
{
    public Task<bool> AddAsync(CategoryForCraeteDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CategoryForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CategoryForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(long id, CategoryForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
