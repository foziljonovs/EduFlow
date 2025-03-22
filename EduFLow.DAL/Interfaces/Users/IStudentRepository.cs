using EduFlow.Domain.Entities.Users;

namespace EduFlow.DAL.Interfaces.Users;

public interface IStudentRepository : IRepository<Student>
{
    Task<List<Student>> GetAllFullInformationAsync();
}
