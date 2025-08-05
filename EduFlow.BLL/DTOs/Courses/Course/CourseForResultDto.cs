using EduFlow.Domain.Enums;
using EduFlow.BLL.DTOs.Courses.Category;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.BLL.DTOs.Courses.Group;

namespace EduFlow.BLL.DTOs.Courses.Course;

public class CourseForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public byte Term { get; set; }
    public Status Archived { get; set; }
    public long CategoryId { get; set; }
    public CategoryForShortResultDto Category { get; set; }
    public List<TeacherForShortResultDto> Teachers { get; set; }
    public List<GroupForShortResultDto> Groups { get; set; }
    public long CompanyId { get; set; }
}
