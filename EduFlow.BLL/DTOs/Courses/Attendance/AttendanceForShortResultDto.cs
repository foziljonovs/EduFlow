using EduFlow.Domain.Entities.Users;

namespace EduFlow.BLL.DTOs.Courses.Attendance;

public class AttendanceForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long StudentId { get; set; }
    public long LessonId { get; set; }
    public DateTime Date { get; set; }
    public bool IsActived { get; set; }
}
