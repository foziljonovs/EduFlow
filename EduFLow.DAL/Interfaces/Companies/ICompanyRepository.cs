using EduFlow.Domain.Entities.Companies;

namespace EduFlow.DAL.Interfaces.Companies;

public interface ICompanyRepository : IRepository<Company>
{
    IQueryable<Company> GetAllFullInformation();
}
