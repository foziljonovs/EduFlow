namespace EduFlow.BLL.DTOs.Users.User;

public class UserForChangePasswordDto
{
    public string PhoneNumber { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
