using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Payments;
using EduFlow.Domain.Entities.Payments;

namespace EduFlow.DAL.Repositories.Payments;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
        
    }
}
