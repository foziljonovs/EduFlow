using EduFlow.Domain.Entities.Companies;

namespace EduFlow.DAL.Interfaces.Companies;

public interface ICompanySubscriptionRepository : IRepository<CompanySubscription>
{
    IQueryable<CompanySubscription> GetAllFullInformation();
}
