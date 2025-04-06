using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Group;

public class GroupForFilterDto
{
    public DateTime? StartedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public string? Name { get; set; }
    public Status? IsStatus { get; set; }
}
