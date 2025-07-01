using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Category;

public class CategoryForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public Status Status { get; set; }
}
