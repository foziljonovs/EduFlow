namespace EduFlow.BLL.DTOs.Courses.Attendance;

public class AttendanceForCraeteDto
{
    public long StudentId { get; set; }
    public long CourseId { get; set; }
    public DateTime Date { get; set; }
    public bool IsActived { get; set; }
}
