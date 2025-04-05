using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;

namespace EduFlow.BLL.DTOs.Messages.Message;

public class MessageForResultDto
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long StudentId { get; set; }
    public Student Student { get; set; }
    public long GroupId { get; set; }
    public Group Group { get; set; }
    public string Text { get; set; }
    public string PhoneNumber { get; set; }
}
