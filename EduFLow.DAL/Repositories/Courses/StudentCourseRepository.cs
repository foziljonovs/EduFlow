using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Courses;
using EduFlow.Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Courses;

public class StudentCourseRepository : Repository<StudentCourse>, IStudentCourseRepository
{
    private readonly AppDbContext _context;
    private DbSet<StudentCourse> _dbSet;
    public StudentCourseRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<StudentCourse>();
    }
}
