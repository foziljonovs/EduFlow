using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.Domain.Entities.Courses;
using System.Windows;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.LessonForComponents;

/// <summary>
/// Interaction logic for LessonForAttendanceComponent.xaml
/// </summary>
public partial class LessonForAttendanceComponent : UserControl
{
    private Dictionary<int, long> keys = new Dictionary<int, long>();
    private LessonForResultDto lesson = new LessonForResultDto();
    public bool isChanged { get; private set; } = false;
    public LessonForAttendanceComponent()
    {
        InitializeComponent();
    }

    public void SetValues(int number, Dictionary<int, long> keys, LessonForResultDto dto)
    {
        this.keys = keys;
        this.lesson = dto;
        tbNumber.Text = number.ToString();
        tbDate.Text = dto.Date.ToString("dd/MM/yyyy");
    }

    public void NewValues(int number, Dictionary<int, long> keys)
    {
        this.keys = keys;
        this.lesson = new LessonForResultDto();
        tbNumber.Text = number.ToString();
        tbDate.Text = DateTime.UtcNow.AddHours(5).Date.ToString("dd/MM/yyyy");
    }

    private void PrintValues(Dictionary<int, long> keys, LessonForResultDto dto)
    {
        foreach (var tableNumber in keys.Keys)
        {
            long studentId = keys[tableNumber];
            var attendance = dto.Attendances.FirstOrDefault(a => a.StudentId == studentId);

            if (attendance is null)
            {
                attendance = new Attendance
                {
                    Id = 0,
                    StudentId = studentId,
                    LessonId = lesson.Id,
                    Date = DateTime.UtcNow.AddHours(5),
                    IsActived = false
                };
                dto.Attendances.Add(attendance);
            }

            var checkBox = new CheckBox
            {
                Width = 20,
                Height = 20,    
                Margin = new System.Windows.Thickness(0, 5, 0, 5),
                Tag = studentId,
                IsChecked = attendance.IsActived,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
            };

            checkBox.Checked += CheckBox_CheckedChanged;
            checkBox.Unchecked += CheckBox_Unchecked;

            stAttendances.Children.Add(checkBox);
        }
    }


    private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.Tag is long studentId)
        {
            isChanged = true;

            var attendance = lesson.Attendances.FirstOrDefault(a => a.StudentId == studentId);
            if (attendance is not null)
            {
                attendance.IsActived = true;
                isChanged = true;
            }
        }
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.Tag is long studentId)
        {
            var attendance = lesson.Attendances.FirstOrDefault(a => a.StudentId == studentId);
            if (attendance is not null)
            {
                attendance.IsActived = false;
                isChanged = true;
            }
        }
    }

    public void MarkAsSaved()
        => this.isChanged = false;

    public List<AttendanceForUpdateRangeDto> GetAttandanceStatus()
    {
        var result = new List<AttendanceForUpdateRangeDto>();

        foreach (var child in stAttendances.Children)
            if(child is CheckBox checkBox && checkBox.Tag is long studentId)
                result.Add(new AttendanceForUpdateRangeDto
                {
                    Id = long.Parse(checkBox.Tag.ToString()), 
                    StudentId = studentId,
                    LessonId = lesson.Id,
                    Date = DateTime.Parse(tbDate.Text),
                    IsActived = lesson.Attendances.FirstOrDefault(x => x.StudentId == studentId).IsActived
                });

        return result;
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        PrintValues(keys, lesson);
    }
}
