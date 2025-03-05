using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Users;
using EduFlow.Domain.Entities.Users;

namespace EduFlow.DAL.Repositories.Users;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context)
    {
        
    }
}
