using EduFlow.Domain.Entities.Courses;
using Et = EduFlow.Domain.Entities.Users;

namespace EduFlow.BLL.DTOs.Users.Teacher;

public class TeacherForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string[] Skills { get; set; }
    public long UserId { get; set; }
    public Et.User? User { get; set; }
    public long CourseId { get; set; }
    public Course Course { get; set; }
    public List<Group> Groups { get; set; }
}
