using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Base;

public class BaseEntity
{
    [Column("id"), Key]
    public required long Id { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(5);
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(5);
    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}
