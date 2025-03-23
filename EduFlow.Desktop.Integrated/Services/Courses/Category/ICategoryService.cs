using EduFlow.BLL.DTOs.Courses.Category;

namespace EduFlow.Desktop.Integrated.Services.Courses.Category;

public interface ICategoryService
{
    Task<List<CategoryForResultDto>> GetAllAsync();
    Task<CategoryForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(CategoryForCraeteDto dto);
    Task<bool> UpdateAsync(long id, CategoryForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
}
