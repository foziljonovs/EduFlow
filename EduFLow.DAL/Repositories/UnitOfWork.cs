using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces;
using EduFlow.DAL.Interfaces.Courses;
using EduFlow.DAL.Interfaces.Messages;
using EduFlow.DAL.Interfaces.Payments;
using EduFlow.DAL.Interfaces.Users;
using EduFlow.DAL.Repositories.Courses;
using EduFlow.DAL.Repositories.Messages;
using EduFlow.DAL.Repositories.Payments;
using EduFlow.DAL.Repositories.Users;

namespace EduFlow.DAL.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
{
    public IUserRepository User { get; set; } = new UserRepository(context);
    public ITeacherRepository Teacher { get; set; } = new TeacherRepository(context);
    public IStudentRepository Student { get; set; } = new StudentRepository(context);
    public ICourseRepository Course { get; set; } = new CourseRepository(context);
    public ICategoryRepository Category { get; set; } = new CategoryRepository(context);
    public IAttendanceRepository Attendance { get; set; } = new AttendanceRepository(context);
    public ILessonRepository Lesson { get; set; } = new LessonRepository(context);
    public IRegistryRepository Registry { get; set; } = new RegistryRepository(context);
    public IPaymentRepository Payment { get; set; } = new PaymentRepository(context);
    public IMessageRepository Message { get; set; } = new MessageRepository(context);
    public IGroupRepository Group { get; set; } = new GroupRepository(context);
    public IStudentCourseRepository StudentCourse { get; set; } = new StudentCourseRepository(context);

    public void Dispose()
        => GC.SuppressFinalize(this);

    public async Task<int> SaveAsync(CancellationToken cancellation = default)
        => await context.SaveChangesAsync(cancellation);
}
