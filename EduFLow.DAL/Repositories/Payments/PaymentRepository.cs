using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Payments;
using EduFlow.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Repositories.Payments;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    private readonly AppDbContext _context;
    private DbSet<Payment> _dbSet;
    public PaymentRepository(AppDbContext context) : base(context)
    {
        this._context = context;
        this._dbSet = _context.Set<Payment>();
    }

    public IQueryable<Payment> GetAllFullInformation()
        => _dbSet
            .Where(x => !x.IsDeleted)
            .Include(x => x.Student)
            .Include(x => x.Group)
            .Include(x => x.Group)
                .ThenInclude(g => g.Course)
            .Include(x => x.Teacher)
                .ThenInclude(t => t.User)
            .Include(x => x.Registry);
}
