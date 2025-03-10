using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Courses;

public class CategoryService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CategoryService> logger) : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CategoryService> _logger = logger;
    public async Task<bool> AddAsync(CategoryForCraeteDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsCategory = await _unitOfWork.Category
                .GetAllAsync()
                .FirstOrDefaultAsync(x => x.Name == dto.Name);

            if (existsCategory is not null)
                throw new StatusCodeException(HttpStatusCode.Conflict, "Category already exists.");

            var category = _mapper.Map<Category>(dto);
            return await _unitOfWork.Category.AddConfirmAsync(category);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while added the category. {ex}");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsCategory = await _unitOfWork.Category.GetAsync(id);
            if (existsCategory is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found.");

            existsCategory.IsDeleted = true;
            return await _unitOfWork.SaveAsync(cancellationToken) > 0;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while delete the category id: {id}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<CategoryForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var categories = await _unitOfWork.Category
                .GetAllAsync()
                .Where(x => x.IsDeleted == false)
                .ToListAsync(cancellationToken);

            if (!categories.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Categories not found.");

            return _mapper.Map<IEnumerable<CategoryForResultDto>>(categories);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all categories. {ex}");
            throw;
        }
    }

    public async Task<CategoryForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var category = await _unitOfWork.Category.GetAsync(id);
            if (category is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found.");

            if (category.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This category has been deleted.");

            return _mapper.Map<CategoryForResultDto>(category);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get category by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(long id, CategoryForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsCategory = await _unitOfWork.Category.GetAsync(id);
            if (existsCategory is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Category not found.");

            if (existsCategory.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This category has been deleted.");

            var updateCategory = _mapper.Map<Category>(dto);
            updateCategory.Id = id;
            updateCategory.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.Category.UpdateAsync(updateCategory);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while update the category id: {id}. {ex}");
            throw;
        }
    }
}
