using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.Text.Json.Serialization;
using Et = EduFlow.Domain.Entities.Payments;

namespace EduFlow.BLL.DTOs.Payments.Payment;

public class PaymentForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long StudentId { get; set; }
    public Student Student { get; set; }
    public long GroupId { get; set; }
    public Group Group { get; set; }
    public long TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public long RegistryId { get; set; }
    public Et.Registry Registry { get; set; }
    public double Amount { get; set; }
    public double Discount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentType Type { get; set; }
    public string? ReceiptNumber { get; set; }
    public string? Notes { get; set; }
}
