using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Group;

public class GroupForUpdateDto
{
    public string Name { get; set; }
    public long CourseId { get; set; }
    public long TeacherId { get; set; }
    public Status IsStatus { get; set; }
    public TimeOnly? PreferredTime { get; set; }
    public Day PreferredDay { get; set; }
}
