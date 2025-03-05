using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Users.User;

public class UserForResultDto
{
    public long Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Age { get; set; }
    public UserRole Role { get; set; }
}
