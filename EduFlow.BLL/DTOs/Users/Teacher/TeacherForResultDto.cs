using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.User;

namespace EduFlow.BLL.DTOs.Users.Teacher;

public class TeacherForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string[] Skills { get; set; }
    public long UserId { get; set; }
    public UserForResultDto? User { get; set; }
    public long CourseId { get; set; }
    public CourseForShortResultDto Course { get; set; }
    public List<GroupForShortResultDto> Groups { get; set; }
}
