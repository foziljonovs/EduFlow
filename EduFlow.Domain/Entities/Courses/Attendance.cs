using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Attendance : BaseEntity
{
    [Column("student_id")]
    public required long StudentId { get; set; }
    public Student Student { get; set; }
    [Column("lesson_id")]
    public required long LessonId { get; set; }
    public Lesson Lesson { get; set; }
    [Column("date")]
    public DateTime Date { get; set; }
    [Column("is_actived")]
    public bool IsActived { get; set; }
}
