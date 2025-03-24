namespace EduFlow.BLL.DTOs.Courses.Course;

public class CourseForFilterDto
{
    public DateTime? StartedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public long? CategoryId { get; set; }
    public long? TeacherId { get; set; }
}
