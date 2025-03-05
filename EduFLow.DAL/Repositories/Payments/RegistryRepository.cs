using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Payments;
using EduFlow.Domain.Entities.Payments;

namespace EduFlow.DAL.Repositories.Payments;

public class RegistryRepository : Repository<Registry>, IRegistryRepository
{
    public RegistryRepository(AppDbContext context) : base(context)
    {
        
    }
}
