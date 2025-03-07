using Et = EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Payments;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.BLL.DTOs.Courses.Course;

public class CourseForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public DateTime Term { get; set; }
    public Status Archived { get; set; }
    public long TeacherId { get; set; }
    public Teacher Teacher { get; set; }
    public long CategoryId { get; set; }
    public Et.Category Category { get; set; }
    public List<Student> Students { get; set; }
    public List<Payment> Payments { get; set; }
}
