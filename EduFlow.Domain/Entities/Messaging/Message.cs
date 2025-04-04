using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Messaging;

public class Message : BaseEntity
{
    [Column("student_id")]
    public required long StudentId { get; set; }
    [Column("student")]
    public Student Student { get; set; }
    [Column("group_id")]
    public required long GroupId { get; set; }
    [Column("group")]
    public Group Group { get; set; }
    [Column("text")]
    public required string Text { get; set; }
    [Column("phone_number"), MaxLength(13)]
    public string PhoneNumber { get; set; }
}
