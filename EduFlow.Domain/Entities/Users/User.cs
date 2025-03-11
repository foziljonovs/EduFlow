using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Users;

public class User : BaseEntity
{
    [Column("first_name"), MaxLength(50)]
    public required string Firstname { get; set; }
    [Column("last_name"), MaxLength(50)]
    public required string Lastname { get; set; }
    [Column("password")]
    public required string Password { get; set; }
    [Column("phone_number"), MaxLength(13)]
    public required string PhoneNumber { get; set; }
    [Column("age")]
    public int Age { get; set; }
    [Column("role")]
    public UserRole Role { get; set; }
    [Column("salt")]
    public string Salt { get; set; }
}
