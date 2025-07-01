using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Payments.Payment;

public class PaymentForShortResultDto
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long GroupId { get; set; }
    public long TeacherId { get; set; }
    public long RegistryId { get; set; }
    public double Amount { get; set; }
    public double Discount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentType Type { get; set; }
    public string? ReceiptNumber { get; set; }
}
