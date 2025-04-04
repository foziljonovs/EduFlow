using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Course : BaseEntity
{
    [Column("name")]
    public required string Name { get; set; }
    [Column("price")]
    public required double Price { get; set; }
    [Column("term")]
    public DateTime Term { get; set; }
    [Column("archived")]
    public Status Archived { get; set; }
    [Column("category_id")]
    public long CategoryId { get; set; }
    [Column("category")]
    public Category Category { get; set; }
    public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    public List<Group> Groups { get; set; } = new List<Group>();
}
