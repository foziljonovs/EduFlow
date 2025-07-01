namespace EduFlow.BLL.DTOs.Users.Student;

public class StudentForShortResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Fullname { get; set; }
    public int Age { get; set; }
    public string? Address { get; set; }
    public string PhoneNumber { get; set; }
}
