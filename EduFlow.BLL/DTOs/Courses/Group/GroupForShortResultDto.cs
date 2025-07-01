using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Group;

public class GroupForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public long CourseId { get; set; }
    public long TeacherId { get; set; }
    public Status IsStatus { get; set; }
}
