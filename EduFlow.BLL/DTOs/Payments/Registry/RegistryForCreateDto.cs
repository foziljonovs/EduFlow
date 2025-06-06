using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Payments.Registry;

public class RegistryForCreateDto
{
    public double Debit { get; set; }
    public double Credit { get; set; }
    public PaymentType Type { get; set; }
    public string? Description { get; set; }
    public bool IsConfirmed { get; set; }
}
