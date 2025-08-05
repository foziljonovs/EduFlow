using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Messaging;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Companies;

public class Company : BaseEntity
{
    [Column("name")]
    public required string Name { get; set; }
    [Column("address")]
    public required string Address { get; set; }
    [Column("about")]
    public string? About { get; set; }
    [Column("phone_number")]
    public required string PhoneNumber { get; set; }
    [Column("logo_id")]
    public required long LogoId { get; set; }
    public Logo Logo { get; set; }
    public List<CompanySubscription> Subscriptions { get; set; } = new List<CompanySubscription>();
    public List<User> Users { get; set; } = new List<User>();
    public List<Course> Courses { get; set; } = new List<Course>();
    public List<Group> Groups { get; set; } = new List<Group>();
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Teacher> Teachers { get; set; } = new List<Teacher>();
    public List<Student> Students { get; set; } = new List<Student>();
    public List<Registry> Registries { get; set; } = new List<Registry>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
    public List<Message> Messages { get; set; } = new List<Message>();
}
