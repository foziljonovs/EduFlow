using EduFlow.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Users;

public class Teacher : BaseEntity
{
    [Column("skills")]
    public string[] Skills { get; set; }
    [Column("user_id")]
    public required long UserId { get; set; }
    public User User { get; set; }
}
