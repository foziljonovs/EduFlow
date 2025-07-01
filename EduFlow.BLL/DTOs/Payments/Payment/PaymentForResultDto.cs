using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Domain.Enums;
using Et = EduFlow.Domain.Entities.Payments;

namespace EduFlow.BLL.DTOs.Payments.Payment;

public class PaymentForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long StudentId { get; set; }
    public StudentForShortResultDto Student { get; set; }
    public long GroupId { get; set; }
    public GroupForShortResultDto Group { get; set; }
    public long TeacherId { get; set; }
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
