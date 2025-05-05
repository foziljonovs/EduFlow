namespace EduFlow.BLL.DTOs.Courses.Attendance;

public class AttendanceForUpdateRangeDto
{
    public long Id { get; set; }
    public long StudentId { get; set; }
    public long LessonId { get; set; }
    public DateTime Date { get; set; }
    public bool IsActived { get; set; }
}
