using EduFlow.Domain.Entities.Courses;

namespace EduFlow.DAL.Interfaces.Courses;

public interface ILessonRepository : IRepository<Lesson>
{
    IQueryable<Lesson> GetAllFullInformation();
}
