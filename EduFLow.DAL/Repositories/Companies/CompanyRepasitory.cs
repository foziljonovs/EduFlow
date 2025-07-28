using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces.Companies;
using EduFlow.Domain.Entities.Companies;

namespace EduFlow.DAL.Repositories.Companies
{
    public class CompanyRepasitory : Repository<Company>, ICompnayRepasitory
    {
        public CompanyRepasitory(AppDbContext context) : base(context)
        {
        }
    }
}
