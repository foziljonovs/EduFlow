using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Group : BaseEntity
{
    public List<Student> Students { get; set; } = new List<Student>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
    [Column("status")]
    public Status IsStatus { get; set; }
}
