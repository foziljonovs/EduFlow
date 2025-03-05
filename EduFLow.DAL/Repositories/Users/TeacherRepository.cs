using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Users;
using EduFlow.Domain.Entities.Users;

namespace EduFlow.DAL.Repositories.Users;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context) : base(context)
    {
        
    }
}
