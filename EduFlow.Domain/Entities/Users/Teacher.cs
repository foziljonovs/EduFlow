using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Courses;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Users;

public class Teacher : BaseEntity
{
    [Column("skills")]
    public string[] Skills { get; set; }
    [Column("user_id")]
    public required long UserId { get; set; }
    public User User { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();
}
