using EduFlow.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Lesson : BaseEntity
{
    [Column("name")]
    public string? Name { get; set; }
    [Column("lesson_number")]
    public required int LessonNumber { get; set; }
    [Column("date")]
    public required DateTime Date { get; set; }
    [Column("group_id")]
    public required long GroupId { get; set; }
    public Group Group { get; set; }
    public List<Attendance> Attendances { get; set; } = new List<Attendance>();
}
