using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Attendance : BaseEntity
{
    [Column("student_id")]
    public required long StudentId { get; set; }
    [Column("student")]
    public Student Student { get; set; }
    [Column("course_id")]
    public required long CourseId { get; set; }
    [Column("course")]
    public Course Course { get; set; }
    [Column("date")]
    public DateTime Date { get; set; }
    [Column("is_actived")]
    public bool IsActived { get; set; }
}
