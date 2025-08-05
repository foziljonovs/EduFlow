using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Companies.CompanySubscription;

public class CompanySubscriptionForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long CompanyId { get; set; }
    public SubscriptionType Type { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? SubcriptionDate { get; set; }
    public bool IsActive { get; set; }
}
