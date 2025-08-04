using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Companies;

public class CompanySubscription : BaseEntity
{
    [Column("company_id")]
    public required long CompanyId { get; set; }
    public Company Company { get; set; }
    [Column("type")]
    public SubscriptionType Type { get; set; }
    [Column("is_paid")]
    public bool IsPaid { get; set; } = false;
    public DateTime? SubcriptionDate { get; set; }
    public bool IsActive { get; set; } = false;
}
