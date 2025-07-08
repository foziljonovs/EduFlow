using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Courses;
using EduFlow.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Courses;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    private readonly AppDbContext _context;
    private DbSet<Group> _dbSet;
    public GroupRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<Group>();
    }

    public IQueryable<Group> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.Course)
            .Include(x => x.Students)
            .Include(x => x.Teacher)
            .OrderByDescending(x => x.CreatedAt);
}
