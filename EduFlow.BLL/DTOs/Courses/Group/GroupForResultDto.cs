using EduFlow.Domain.Enums;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.BLL.DTOs.Courses.Course;

namespace EduFlow.BLL.DTOs.Courses.Group;

public class GroupForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public long CourseId { get; set; }
    public CourseForShortResultDto Course { get; set; }
    public long TeacherId { get; set; }
    public TeacherForShortResultDto Teacher { get; set; }
    public List<StudentForShortResultDto> Students { get; set; }
    public Status IsStatus { get; set; }
}
