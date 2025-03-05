namespace EduFlow.BLL.DTOs.Messages.Message;

public class MessageForUpdateDto
{
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public string Text { get; set; }
    public string PhoneNumber { get; set; }
}
