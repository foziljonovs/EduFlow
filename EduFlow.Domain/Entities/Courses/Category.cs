using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Companies;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Category : BaseEntity
{
    [Column("name")]
    public required string Name { get; set; }
    [Column("status")]
    public Status Status { get; set; }
    [Column("company_id")]
    public required long CompanyId { get; set; }
    public Company Company { get; set; }
    public List<Course> Courses { get; set; } = new List<Course>();
}
