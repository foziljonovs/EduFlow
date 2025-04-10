using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Group;

public class GroupForFilterDto
{
    public DateTime? StartedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public long? CourseId { get; set; }
    public long? TeacherId { get; set; }
    public Status? IsStatus { get; set; }
}
