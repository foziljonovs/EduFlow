using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Payments;

public class Payment : BaseEntity
{
    [Column("student_id")]
    public required long StudentId { get; set; }
    [Column("student")]
    public Student Student { get; set; }
    [Column("group_id")]
    public required long GroupId { get; set; }
    [Column("group")]
    public Group Group { get; set; }
    [Column("registry_id")]
    public required long RegistryId { get; set; }
    [Column("registry")]
    public Registry Registry { get; set; }
    [Column("amount")]
    public required double Amount { get; set; }
    [Column("payment_date")]
    public DateTime PaymentDate { get; set; }
}
