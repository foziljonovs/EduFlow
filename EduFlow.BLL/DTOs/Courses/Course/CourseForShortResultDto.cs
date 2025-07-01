using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Course;

public class CourseForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public byte Term { get; set; }
    public Status Archived { get; set; }
    public long CategoryId { get; set; }
}
