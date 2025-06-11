using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Group : BaseEntity
{
    [Column("name"), MaxLength(60)]
    public required string Name { get; set; }
    [Column("course_id")]
    public required long CourseId { get; set; }
    public Course Course { get; set; }
    [Column("teacher_id")]
    public required long TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
    public List<Lesson> Lessons { get; set; } = new List<Lesson>();
    [Column("status")]
    public Status IsStatus { get; set; }
}
