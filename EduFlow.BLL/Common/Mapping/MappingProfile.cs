using AutoMapper;
using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.BLL.DTOs.Courses.Course;
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
        CreateMap<Teacher, TeacherForCreateDto>();
        CreateMap<Teacher, TeacherForUpdateDto>();

        /*------------------------------------Student--------------------------------------*/
        CreateMap<Student, StudentForResultDto>();
        CreateMap<StudentForResultDto, Student>();
        CreateMap<Student, StudentForCreateDto>();
        CreateMap<Student, StudentForUpdateDto>();

        /*--------------------------------------Course---------------------------------------*/
        CreateMap<Course, CourseForResultDto>();
        CreateMap<CourseForResultDto, Course>();
        CreateMap<Course, CourseForCreateDto>();
        CreateMap<Course, CourseForUpdateDto>();

        /*-------------------------------------Category-----------------------------------------*/
        CreateMap<Category, CategoryForResultDto>();
        CreateMap<CategoryForResultDto, Category>();
        CreateMap<Category, CourseForCreateDto>();
        CreateMap<Category, CategoryForUpdateDto>();

        /*--------------------------------------Attendance----------------------------------------*/
        CreateMap<Attendance, AttendanceForResultDto>();
        CreateMap<AttendanceForResultDto, Attendance>();
        CreateMap<Attendance, AttendanceForCraeteDto>();
        CreateMap<Attendance, AttendanceForUpdateDto>();

        /*--------------------------------------Message------------------------------------------*/
        CreateMap<Message, MessageForResultDto>();
        CreateMap<MessageForResultDto, Message>();
        CreateMap<Message, MessageForCreateDto>();
        CreateMap<Message, MessageForUpdateDto>();

        /*-------------------------------------Payment-----------------------------------------*/
        CreateMap<Payment, PaymentForResultDto>();
        CreateMap<PaymentForResultDto, Payment>();
        CreateMap<Payment, PaymentForCreateDto>();
        CreateMap<Payment, PaymentForUpdateDto>();

        /*--------------------------------------Registry---------------------------------------*/
        CreateMap<Registry, RegistryForResultDto>();
        CreateMap<RegistryForResultDto, Registry>();
        CreateMap<Registry, RegistryForCreateDto>();
    }
}
