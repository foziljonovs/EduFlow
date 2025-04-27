namespace EduFlow.BLL.DTOs.Courses.Attendance;

public class AttendanceForUpdateDto
{
    public long StudentId { get; set; }
    public long LessonId { get; set; }
    public DateTime Date { get; set; }
    public bool IsActived { get; set; }
}
