using EduFlow.Domain.Entities.Users;
using Et = EduFlow.Domain.Entities.Courses;

namespace EduFlow.BLL.DTOs.Courses.Attendance;

public class AttendanceForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long StudentId { get; set; }
    public Student Student { get; set; }
    public long GroupId { get; set; }
    public Et.Group Group { get; set; }
    public DateTime Date { get; set; }
    public bool IsActived { get; set; }
}
