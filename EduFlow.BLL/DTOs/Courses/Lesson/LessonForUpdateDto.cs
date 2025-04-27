namespace EduFlow.BLL.DTOs.Courses.Lesson;

public class LessonForUpdateDto
{
    public string? Name { get; set; }
    public int LessonNumber { get; set; }
    public DateTime Date { get; set; }
    public long GroupId { get; set; }
}
