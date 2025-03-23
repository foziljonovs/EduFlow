using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Courses.Category;
using EduFlow.Desktop.Integrated.Servers.Repositories.Courses.Category;

namespace EduFlow.Desktop.Integrated.Services.Courses.Category;

public class CategoryService : ICategoryService
{
    private readonly ICategoryServer _server;
    public CategoryService()
    {
        this._server = new CategoryServer();
    }
    public async Task<bool> AddAsync(CategoryForCraeteDto dto)
    {
        try
        {
            var result = await _server.AddAsync(dto);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var result = await _server.DeleteAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<List<CategoryForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch (Exception ex)
        {
            return new List<CategoryForResultDto>();
        }
    }

    public async Task<CategoryForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch (Exception ex)
        {
            return new CategoryForResultDto();
        }
    }

    public async Task<bool> UpdateAsync(long id, CategoryForUpdateDto dto)
    {
        try
        {
            var result = await _server.UpdateAsync(id, dto);
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
