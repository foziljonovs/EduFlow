using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class StudentCourse : BaseEntity
{
    [Column("student_id")]
    public required long StudentId { get; set; }
    public Student Student { get; set; }
    [Column("course_id")]
    public required long CourseId { get; set; }
    public Course Course { get; set; }
    [Column("status")]
    public EnrollmentStatus Status { get; set; }
    [Column("preferred_time")]
    public TimeOnly? PreferredTime { get; set; }
    [Column("preferred_days")]
    public Day PreferredDay { get; set; }
}
