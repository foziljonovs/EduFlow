using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using Et = EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Group;

public class GroupForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public long CourseId { get; set; }
    public Et.Course Course { get; set; }
    public long TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public List<Student> Students { get; set; }
    public List<Payment> Payments { get; set; }
    public Status IsStatus { get; set; }
}
