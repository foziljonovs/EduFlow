using EduFlow.DAL.Interfaces.Courses;
using EduFlow.DAL.Interfaces.Messages;
using EduFlow.DAL.Interfaces.Payments;
using EduFlow.DAL.Interfaces.Users;

namespace EduFlow.DAL.Interfaces;

public interface IUnitOfWork
{
    public IUserRepository User { get; set; }
    public ITeacherRepository Teacher { get; set; }
    public IStudentRepository Student { get; set; }
    public ICourseRepository Course { get; set; }
    public IGroupRepository Group { get; set; }
    public ICategoryRepository Category { get; set; }
    public IAttendanceRepository Attendance { get; set; }
    public ILessonRepository Lesson { get; set; }
    public IRegistryRepository Registry { get; set; }
    public IPaymentRepository Payment { get; set; }
    public IMessageRepository Message { get; set; }
    public IStudentCourseRepository StudentCourse { get; set; }
    public Task<int> SaveAsync(CancellationToken cancellation = default);
}
