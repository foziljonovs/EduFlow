using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Courses.StudentCourse;
using EduFlow.BLL.DTOs.Payments.Payment;

namespace EduFlow.BLL.DTOs.Users.Student;

public class StudentForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Fullname { get; set; }
    public int Age { get; set; }
    public string? Address { get; set; }
    public string PhoneNumber { get; set; }
    public List<StudentCourseForResultDto> StudentCourses { get; set; }
    public List<PaymentForShortResultDto> Payments { get; set; }
    public List<GroupForShortResultDto> Groups { get; set; }
}
