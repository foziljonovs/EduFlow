using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Courses;
using EduFlow.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Courses;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    private readonly AppDbContext _context;
    private DbSet<Course> _dbSet;
    public CourseRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<Course>();
    }

    public IQueryable<Course> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.Category)
            .Include(x => x.Teacher)
                .ThenInclude(t => t.User)
            .Include(x => x.Students)
            .Include(x => x.Payments);
}
