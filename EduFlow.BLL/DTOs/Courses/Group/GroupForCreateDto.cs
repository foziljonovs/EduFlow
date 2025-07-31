using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.BLL.DTOs.Courses.Group;

public class GroupForCreateDto
{
    public string Name { get; set; }
    public long CourseId { get; set; }
    public long TeacherId { get; set; }
    public Status IsStatus { get; set; }
    public TimeSpan PreferredTime { get; set; }
    public Day PreferredDay { get; set; }
}
