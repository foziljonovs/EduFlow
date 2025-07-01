namespace EduFlow.BLL.DTOs.Users.Teacher;

public class TeacherForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string[] Skills { get; set; }
    public long UserId { get; set; }
    public long CourseId { get; set; }
}
