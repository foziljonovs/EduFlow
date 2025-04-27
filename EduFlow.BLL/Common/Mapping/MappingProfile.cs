using AutoMapper;
using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.DTOs.Courses.Category;
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
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Messaging;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;

namespace EduFlow.BLL.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        /*--------------------------------User----------------------------------*/
        CreateMap<User, UserForResultDto>();
        CreateMap<UserForResultDto, User>();
        CreateMap<UserForCreateDto, User>();
        CreateMap<UserForUpdateDto, User>();
        CreateMap<UserForLoginDto, User>();
        CreateMap<UserForChangePasswordDto, User>();

        /*----------------------------------Teacher------------------------------------*/
        CreateMap<Teacher, TeacherForResultDto>();
        CreateMap<TeacherForResultDto, Teacher>();
        CreateMap<TeacherForCreateDto, Teacher>();
        CreateMap<TeacherForUpdateDto, Teacher>();

        /*------------------------------------Student--------------------------------------*/
        CreateMap<Student, StudentForResultDto>();
        CreateMap<StudentForResultDto, Student>();
        CreateMap<StudentForCreateDto, Student>();
        CreateMap<StudentForUpdateDto, Student>();

        /*--------------------------------------Course---------------------------------------*/
        CreateMap<Course, CourseForResultDto>();
        CreateMap<CourseForResultDto, Course>();
        CreateMap<CourseForCreateDto, Course>();
        CreateMap<CourseForUpdateDto, Course>();

        /*--------------------------------------Group----------------------------------------*/
        CreateMap<Group, GroupForResultDto>();
        CreateMap<GroupForResultDto, Group>();
        CreateMap<GroupForCreateDto, Group>()
            .ForMember(dest => dest.Students, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore());

        CreateMap<GroupForUpdateDto, Group>()
            .ForMember(dest => dest.Students, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore());

        /*-------------------------------------StudentCourse----------------------------------*/
        CreateMap<StudentCourse, StudentCourseForResultDto>();
        CreateMap<StudentCourseForResultDto, StudentCourse>();
        CreateMap<StudentCourseForCreateDto, StudentCourse>();
        CreateMap<StudentCourseForUpdateDto, StudentCourse>();

        /*-------------------------------------Category-----------------------------------------*/
        CreateMap<Category, CategoryForResultDto>();
        CreateMap<CategoryForResultDto, Category>();
        CreateMap<CategoryForCraeteDto, Category>()
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

        CreateMap<CategoryForUpdateDto, Category>()
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

        /*--------------------------------------Attendance----------------------------------------*/
        CreateMap<Attendance, AttendanceForResultDto>();
        CreateMap<AttendanceForResultDto, Attendance>();
        CreateMap<AttendanceForCraeteDto, Attendance>();
        CreateMap<AttendanceForUpdateDto, Attendance>();

        /*--------------------------------------Lesson------------------------------------------*/
        CreateMap<Lesson, LessonForResultDto>();
        CreateMap<LessonForResultDto, Lesson>();
        CreateMap<LessonForCreateDto, Lesson>();
        CreateMap<LessonForUpdateDto, Lesson>();

        /*--------------------------------------Message------------------------------------------*/
        CreateMap<Message, MessageForResultDto>();
        CreateMap<MessageForResultDto, Message>();
        CreateMap<MessageForCreateDto, Message>();
        CreateMap<MessageForUpdateDto, Message>();

        /*-------------------------------------Payment-----------------------------------------*/
        CreateMap<Payment, PaymentForResultDto>();
        CreateMap<PaymentForResultDto, Payment>();
        CreateMap<PaymentForCreateDto, Payment>();
        CreateMap<PaymentForUpdateDto, Payment>();

        /*--------------------------------------Registry---------------------------------------*/
        CreateMap<Registry, RegistryForResultDto>();
        CreateMap<RegistryForResultDto, Registry>();
        CreateMap<RegistryForCreateDto, Registry>();
    }
}
