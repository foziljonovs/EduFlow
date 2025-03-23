using EduFlow.BLL.DTOs.Courses.Category;

namespace EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Category;

public interface ICategoryServer
{
    Task<List<CategoryForResultDto>> GetAllAsync();
    Task<CategoryForResultDto> GetByIdAsync(long id);
    Task<bool> AddAsync(CategoryForCraeteDto dto);
    Task<bool> UpdateAsync(long id, CategoryForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
}
