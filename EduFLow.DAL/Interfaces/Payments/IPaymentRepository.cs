using EduFlow.Domain.Entities.Payments;

namespace EduFlow.DAL.Interfaces.Payments;

public interface IPaymentRepository : IRepository<Payment>
{
    IQueryable<Payment> GetAllFullInformation();
}
