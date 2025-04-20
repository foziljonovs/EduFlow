using Et = EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;

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
    public Et.Category Category { get; set; }
    public List<Teacher> Teachers { get; set; }
    public List<Et.Group> Groups { get; set; }
    public List<Et.StudentCourse> Students { get; set; }
}
