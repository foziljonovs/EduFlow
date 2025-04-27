namespace EduFlow.BLL.DTOs.Courses.Lesson;

public class LessonForCreateDto
{
    public string? Name { get; set; }
    public int LessonNumber { get; set; }
    public DateTime Date { get; set; }
    public long GroupId { get; set; }
}
