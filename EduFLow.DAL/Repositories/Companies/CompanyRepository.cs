using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Companies;
using EduFlow.Domain.Entities.Companies;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Companies;

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    private readonly AppDbContext _context;
    private DbSet<Company> _dbSet;
    public CompanyRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<Company>();
    }

    public IQueryable<Company> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.Logo);
}
