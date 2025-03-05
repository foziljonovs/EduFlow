using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Users.User;

public class UserForUpdateDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public int Age { get; set; }
    public UserRole Role { get; set; }
}
