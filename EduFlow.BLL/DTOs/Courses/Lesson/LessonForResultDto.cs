using EduFlow.BLL.DTOs.Courses.Attendance;
using Et = EduFlow.Domain.Entities.Courses;

namespace EduFlow.BLL.DTOs.Courses.Lesson;

public class LessonForResultDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public int LessonNumber { get; set; }
    public DateTime Date { get; set; }
    public long GroupId { get; set; }
    public List<AttendanceForShortResultDto> Attendances { get; set; }
}
