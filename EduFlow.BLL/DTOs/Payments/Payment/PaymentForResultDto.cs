using EduFlow.Domain.Entities.Courses;
using Et = EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;

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
    public long RegistryId { get; set; }
    public Et.Registry Registry { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}
