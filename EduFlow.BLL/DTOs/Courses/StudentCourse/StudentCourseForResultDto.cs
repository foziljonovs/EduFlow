using Et = EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;

namespace EduFlow.BLL.DTOs.Courses.StudentCourse;

public class StudentCourseForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long StudentId { get; set; }
    public Student Student { get; set; }
    public long CourseId { get; set; }
    public Et.Course Course { get; set; }
}
