using EduFlow.BLL.Common.Validators.Companies.Company;
using EduFlow.BLL.Common.Validators.Companies.CompanySubscription;
using EduFlow.BLL.Common.Validators.Companies.Interfaces;
using EduFlow.BLL.Common.Validators.Companies.Logo;
using EduFlow.BLL.Common.Validators.Companies.Services;
using EduFlow.BLL.Common.Validators.Courses.Attendance;
using EduFlow.BLL.Common.Validators.Courses.Course;
using EduFlow.BLL.Common.Validators.Courses.Group;
using EduFlow.BLL.Common.Validators.Courses.Interface;
using EduFlow.BLL.Common.Validators.Courses.Interfaces;
using EduFlow.BLL.Common.Validators.Courses.Lesson;
using EduFlow.BLL.Common.Validators.Courses.Services;
using EduFlow.BLL.Common.Validators.Courses.StudentCourse;
using EduFlow.BLL.Common.Validators.Messages.Interfaces;
using EduFlow.BLL.Common.Validators.Messages.Message;
using EduFlow.BLL.Common.Validators.Messages.Services;
using EduFlow.BLL.Common.Validators.Payments.Interfaces;
using EduFlow.BLL.Common.Validators.Payments.Payment;
using EduFlow.BLL.Common.Validators.Payments.Registry;
using EduFlow.BLL.Common.Validators.Payments.Services;
using EduFlow.BLL.Common.Validators.Users.Interface;
using EduFlow.BLL.Common.Validators.Users.Services;
using EduFlow.BLL.Common.Validators.Users.Student;
using EduFlow.BLL.Common.Validators.Users.Teacher;
using EduFlow.BLL.Common.Validators.Users.User;
using EduFlow.BLL.DTOs.Companies.Company;
using EduFlow.BLL.DTOs.Companies.CompanySubscription;
using EduFlow.BLL.DTOs.Companies.Logo;
using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.BLL.DTOs.Courses.StudentCourse;
using EduFlow.BLL.DTOs.Messages.Message;
using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.BLL.DTOs.Users.User;
using EduFlow.BLL.Interfaces.Auth;
using EduFlow.BLL.Interfaces.Companies;
using EduFlow.BLL.Interfaces.Courses;
using EduFlow.BLL.Interfaces.Messaging;
using EduFlow.BLL.Interfaces.Payments;
using EduFlow.BLL.Interfaces.Users;
using EduFlow.BLL.Services.Auth;
using EduFlow.BLL.Services.Companies;
using EduFlow.BLL.Services.Courses;
using EduFlow.BLL.Services.Messaging;
using EduFlow.BLL.Services.Payments;
using EduFlow.BLL.Services.Users;
using EduFlow.DAL.Data;
using EduFlow.DAL.Interfaces;
using EduFlow.DAL.Interfaces.Companies;
using EduFlow.DAL.Interfaces.Courses;
using EduFlow.DAL.Interfaces.Messages;
using EduFlow.DAL.Interfaces.Payments;
using EduFlow.DAL.Interfaces.Users;
using EduFlow.DAL.Repositories;
using EduFlow.DAL.Repositories.Companies;
using EduFlow.DAL.Repositories.Courses;
using EduFlow.DAL.Repositories.Messages;
using EduFlow.DAL.Repositories.Payments;
using EduFlow.DAL.Repositories.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EduFlow.WebApi.Configurations;

public static class LayerConfiguration
{
    public static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("localhost");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }

    public static void AddSerilogConfiguration(IHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/edu-flow-log.txt",
                rollingInterval: RollingInterval.Month)
            .CreateLogger();

        host.UseSerilog();
    }

    public static IServiceCollection AddRepositoryConfiguration(
        this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
        services.AddScoped<ILessonRepository, LessonRepository>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IRegistryRepository, RegistryRepository>();

        services.AddScoped<IMessageRepository, MessageRepository>();

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ILogoRepository, LogoRepository>();
        services.AddScoped<ICompanySubscriptionRepository, CompanySubscriptionRepository>();

        return services;
    }

    public static IServiceCollection AddServiceConfiguration(
        this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<IStudentService, StudentService>();

        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IStudentCourseService, StudentCourseService>();
        services.AddScoped<ILessonService, LessonService>();

        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IRegistryService, RegistryService>();

        services.AddScoped<IMessageService, MessageService>();

        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<ILogoService, LogoService>();
        services.AddScoped<ICompanySubscriptionService, CompanySubscriptionService>();

        return services;
    }

    public static IServiceCollection AddValidationServiceConfiguration(
        this IServiceCollection services)
    {
        services.AddTransient<IUserValidator, UserValidator>();
        services.AddTransient<ITeacherValidator, TeacherValidator>();
        services.AddTransient<IStudentValidator, StudentValidator>();

        services.AddTransient<ICourseValidator, CourseValidator>();
        services.AddTransient<IGroupValidator, GroupValidator>();
        services.AddTransient<IAttendanceValidator, AttendanceValidator>();
        services.AddTransient<IStudentCourseValidator, StudentCourseValidator>();
        services.AddTransient<ILessonValidator, LessonValidator>();

        services.AddTransient<IPaymentValidator, PaymentValidator>();
        services.AddTransient<IRegistryValidator, RegistryValidator>();

        services.AddTransient<IMessageValidator, MessageValidator>();

        services.AddTransient<ICompanyValidator, CompanyValidator>();
        services.AddTransient<ILogoValidator, LogoValidator>();
        services.AddTransient<ICompanySubscriptionValidator, CompanySubscriptionValidator>();

        return services;
    }

    public static IServiceCollection AddValidationConfiguration(
        this IServiceCollection services)
    {
        services.AddTransient<IValidator<UserForCreateDto>, UserForCreateValidator>();
        services.AddTransient<IValidator<UserForUpdateDto>, UserForUpdateValidator>();
        services.AddTransient<IValidator<UserForChangePasswordDto>, UserForChangePasswordValidator>();
        services.AddTransient<IValidator<UserForLoginDto>, UserForLoginValidator>();

        services.AddTransient<IValidator<TeacherForCreateDto>, TeacherForCreateValidator>();
        services.AddTransient<IValidator<TeacherForUpdateDto>, TeacherForUpdateValidator>();

        services.AddTransient<IValidator<StudentForCreateDto>, StudentForCreateValidator>();
        services.AddTransient<IValidator<StudentForUpdateDto>, StudentForUpdateValidator>();

        services.AddTransient<IValidator<CourseForCreateDto>, CourseForCreateValidator>();
        services.AddTransient<IValidator<CourseForUpdateDto>, CourseForUpdateValidator>();

        services.AddTransient<IValidator<GroupForCreateDto>, GroupForCreateValidator>();
        services.AddTransient<IValidator<GroupForUpdateDto>, GroupForUpdateValidator>();

        services.AddTransient<IValidator<AttendanceForCraeteDto>, AttendanceForCreateValidator>();
        services.AddTransient<IValidator<AttendanceForUpdateDto>, AttendanceForUpdateValidator>();

        services.AddTransient<IValidator<LessonForCreateDto>, LessonForCraeteValidator>();
        services.AddTransient<IValidator<LessonForUpdateDto>, LessonForUpdateValidator>();

        services.AddTransient<IValidator<StudentCourseForCreateDto>, StudentCourseForCreateValidator>();
        services.AddTransient<IValidator<StudentCourseForUpdateDto>, StudentCourseForUpdateValidator>();

        services.AddTransient<IValidator<PaymentForCreateDto>, PaymentForCreateValidator>();
        services.AddTransient<IValidator<PaymentForUpdateDto>, PaymentForUpdateValidator>();

        services.AddTransient<IValidator<RegistryForCreateDto>, RegistryForCreateValidator>();
        services.AddTransient<IValidator<RegistryForUpdateDto>, RegistryForUpdateValidator>();

        services.AddTransient<IValidator<MessageForCreateDto>, MessageForCreateValidator>();

        services.AddTransient<IValidator<CompanyForCreateDto>, CompanyForCreateValidator>();
        services.AddTransient<IValidator<CompanyForUpdateDto>, CompanyForUpdateValidator>();

        services.AddTransient<IValidator<LogoForCreateDto>, LogoForCreateValidator>();
        services.AddTransient<IValidator<LogoForUpdateDto>, LogoForUpdateValidator>();

        services.AddTransient<IValidator<CompanySubscriptionForCreateDto>, CompanySubscriptionForCreateValidator>();
        services.AddTransient<IValidator<CompanySubscriptionForUpdateDto>, CompanySubscriptionForUpdateValidator>();

        return services;
    }
}
