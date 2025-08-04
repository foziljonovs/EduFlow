using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Companies;
using EduFlow.Domain.Entities.Companies;

namespace EduFlow.DAL.Repositories.Companies;

public class LogoRepository : Repository<Logo>, ILogoRepository
{
    public LogoRepository(AppDbContext context) : base(context)
    {
    }
}
