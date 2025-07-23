using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.StudentCourse;

public class StudentCourseForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public EnrollmentStatus Status { get; set; }
    public TimeOnly? PreferredTime { get; set; }
    public Day PreferredDay { get; set; }
}
