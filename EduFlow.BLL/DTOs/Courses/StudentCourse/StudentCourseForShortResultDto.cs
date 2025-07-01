namespace EduFlow.BLL.DTOs.Courses.StudentCourse;

public class StudentCourseForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long StudentId { get; set; }
    public long CourseId { get; set; }
}
