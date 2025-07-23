using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.StudentCourse;

public class StudentCourseForUpdateDto
{
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public TimeOnly? PreferredTime { get; set; }
    public Day PreferredDay { get; set; }
}
