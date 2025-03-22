using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Users;
using EduFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Users;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    private readonly AppDbContext _context;
    private DbSet<Student> _dbSet;
    public StudentRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<Student>();
    }

    public IQueryable<Student> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.Payments)
            .Include(x => x.Courses);
}
