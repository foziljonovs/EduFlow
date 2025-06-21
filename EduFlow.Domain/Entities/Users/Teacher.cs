using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Payments;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Users;

public class Teacher : BaseEntity
{
    [Column("skills")]
    public string[] Skills { get; set; }
    [Column("user_id")]
    public required long UserId { get; set; }
    public User User { get; set; }
    [Column("course_id")]
    public required long CourseId { get; set; }
    public Course Course { get; set; }
    public List<Group> Groups { get; set; } = new List<Group>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
}
