using EduFlow.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Companies;

public class Logo : BaseEntity
{
    [Column("file_name")]
    public required string FileName { get; set; }
    [Column("content_type")]
    public required string ContentType { get; set; }
    [Column("file_path")]
    public required string FilePath { get; set; }
}
