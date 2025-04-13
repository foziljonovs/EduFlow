using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Users;
using EduFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Users;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    private readonly AppDbContext _context;
    private DbSet<Teacher> _dbSet;
    public TeacherRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<Teacher>();
    }

    public IQueryable<Teacher> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.User)
            .Include(x => x.Groups)
            .Include(x => x.Course);
}
