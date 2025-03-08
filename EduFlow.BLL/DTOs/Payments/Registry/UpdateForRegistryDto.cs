using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Payments.Registry;

public class UpdateForRegistryDto
{
    public double Debit { get; set; }
    public double Credit { get; set; }
    public PaymentType Type { get; set; }
}
