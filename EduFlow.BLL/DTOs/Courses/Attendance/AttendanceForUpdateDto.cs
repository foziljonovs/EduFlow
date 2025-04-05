namespace EduFlow.BLL.DTOs.Courses.Attendance;

public class AttendanceForUpdateDto
{
    public long StudentId { get; set; }
    public long GroupId { get; set; }
    public DateTime Date { get; set; }
    public bool IsActived { get; set; }
}
