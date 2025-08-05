using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Companies;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Payments;

public class Registry : BaseEntity
{
    [Column("debit")]
    public double Debit { get; set; }
    [Column("credit")]
    public double Credit { get; set; }
    [Column("type")]
    public PaymentType Type { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    public bool IsConfirmed { get; set; } = false;
    [Column("company_id")]
    public required long CompanyId { get; set; }
    public Company Company { get; set; }
}
