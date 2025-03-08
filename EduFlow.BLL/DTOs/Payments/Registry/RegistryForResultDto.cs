using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Payments.Registry;

public class RegistryForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public double Debit { get; set; }
    public double Credit { get; set; }
    public PaymentType Type { get; set; }
}
