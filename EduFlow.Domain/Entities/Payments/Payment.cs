using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Payments;

public class Payment : BaseEntity
{
    [Column("student_id")]
    public required long StudentId { get; set; }
    public Student Student { get; set; }
    [Column("group_id")]
    public required long GroupId { get; set; }
    public Group Group { get; set; }
    [Column("registry_id")]
    public required long RegistryId { get; set; }
    public Registry Registry { get; set; }
    [Column("amount")]
    public double Amount { get; set; }
    [Column("discount")]
    public double Discount { get; set; } = 0;
    [Column("payment_date")]
    public DateTime PaymentDate { get; set; }
    [Column("status")]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    [Column("type")]
    public PaymentType Type { get; set; }
    [Column("receipt_number")]
    public string? ReceiptNumber { get; set; }
    [Column("notes")]
    public string? Notes { get; set; }
}
