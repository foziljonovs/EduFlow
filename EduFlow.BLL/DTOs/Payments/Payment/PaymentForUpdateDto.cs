namespace EduFlow.BLL.DTOs.Payments.Payment;

public class PaymentForUpdateDto
{
    public long StudentId { get; set; }
    public long GroupId { get; set; }
    public long RegistryId { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}
