using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Course : BaseEntity
{
    [Column("name")]
    public required string Name { get; set; }
    [Column("price")]
    public required double Price { get; set; }
    [Column("term")]
    public DateTime Term { get; set; }
    [Column("archived")]
    public Status Archived { get; set; }
    [Column("teacher_id")]
    public required long TeacherId { get; set; }
    [Column("teacher")]
    public Teacher Teacher { get; set; }
    [Column("category_id")]
    public long CategoryId { get; set; }
    [Column("category")]
    public Category Category { get; set; }
    public List<Student> Students { get; set; } = new List<Student>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
}
