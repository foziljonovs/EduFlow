using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Courses;
using EduFlow.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Courses;

public class LessonRepository : Repository<Lesson>, ILessonRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Lesson> _dbSet;
    public LessonRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbSet = context.Set<Lesson>();
    }

    public IQueryable<Lesson> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.Group)
            .Include(x => x.Attendances)
                .ThenInclude(a => a.Student);
}
