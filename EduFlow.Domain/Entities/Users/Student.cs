using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Payments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Users;

public class Student : BaseEntity
{
    [Column("fullname"), MaxLength(60)]
    public required string Fullname { get; set; }
    [Column("age")]
    public required int Age { get; set; }
    [Column("address")]
    public string? Address { get; set; }
    [Column("phone_number")]
    public required string PhoneNumber { get; set; }
    public List<StudentCourse> StudentCourses { get; set; }
    public List<Payment> Payments { get; set; } = new List<Payment>();
    public List<Group> Groups { get; set; } = new List<Group>();
}
