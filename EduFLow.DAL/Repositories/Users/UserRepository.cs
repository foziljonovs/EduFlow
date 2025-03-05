using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces;
using EduFlow.DAL.Interfaces.Users;
using EduFlow.Domain.Entities.Users;

namespace EduFlow.DAL.Repositories.Users;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
        
    }
}
