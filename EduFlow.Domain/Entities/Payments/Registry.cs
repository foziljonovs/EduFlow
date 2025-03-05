using EduFlow.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Payments;

public class Registry : BaseEntity
{
    [Column("debit")]
    public double Debit { get; set; }
    [Column("credit")]
    public double Credit { get; set; }
}
