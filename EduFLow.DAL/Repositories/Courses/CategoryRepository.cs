using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Courses;
using EduFlow.Domain.Entities.Courses;

namespace EduFlow.DAL.Repositories.Courses;

public class CategoryRepository : Repository<Category>, ICategoryRepository 
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
        
    }
}
