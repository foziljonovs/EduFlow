using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Payments.Payment;

public class PaymentForFilterDto
{
    public DateTime? StartedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public long? CourseId { get; set; }
    public long? TeacherId { get; set; }
    public PaymentStatus? Status { get; set; }
    public PaymentType? Type { get; set; }
}
