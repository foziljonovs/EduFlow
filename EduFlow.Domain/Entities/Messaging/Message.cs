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
    [Column("course_id")]
    public required long CourseId { get; set; }
    [Column("course")]
    public Course Course { get; set; }
    [Column("text")]
    public required string Text { get; set; }
    [Column("phone_number"), MaxLength(13)]
    public string PhoneNumber { get; set; }
}
