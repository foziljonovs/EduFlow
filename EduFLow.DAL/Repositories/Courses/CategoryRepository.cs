using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Courses;
using EduFlow.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Courses;

public class CategoryRepository : Repository<Category>, ICategoryRepository 
{
    private readonly AppDbContext _context;
    private DbSet<Category> _dbSet;
    public CategoryRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<Category>();
    }

    public IQueryable<Category> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.Courses);
}
