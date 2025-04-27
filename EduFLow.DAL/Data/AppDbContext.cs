using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Messaging;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.DAL.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Registry> Registry { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }
}
