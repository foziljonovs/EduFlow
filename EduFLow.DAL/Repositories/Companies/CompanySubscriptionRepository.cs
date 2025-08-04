using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Companies;
using EduFlow.Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Companies;

public class CompanySubscriptionRepository : Repository<CompanySubscription>, ICompanySubscriptionRepository
{
    private readonly AppDbContext _context;
    private DbSet<CompanySubscription> _dbSet;
    public CompanySubscriptionRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<CompanySubscription>();
    }

    public IQueryable<CompanySubscription> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.Company);
}
