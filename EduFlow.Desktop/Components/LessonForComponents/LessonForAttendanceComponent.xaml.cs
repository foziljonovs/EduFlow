using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.Domain.Entities.Courses;
using System.Windows.Controls;

namespace EduFlow.Desktop.Components.LessonForComponents;

/// <summary>
/// Interaction logic for LessonForAttendanceComponent.xaml
/// </summary>
public partial class LessonForAttendanceComponent : UserControl
{
    private Dictionary<int, long> keys = new Dictionary<int, long>();
    private LessonForResultDto lesson = new LessonForResultDto();
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

    private void PrintValues(Dictionary<int, long> keys, LessonForResultDto dto)
    {
        foreach(var tableNumber in keys.Keys)
        {
            long studentId = keys[tableNumber];

            var attendance = dto.Attendances.FirstOrDefault(a => a.StudentId == studentId);

            if (attendance is null)
                return;

            if (attendance.IsActived)
            {
                var checkBox = new CheckBox
                {
                    Content = "Keldi",
                    IsChecked = true,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };

                stAttendances.Children.Add(checkBox);
            }
            else
            {
                var checkBox = new CheckBox
                {
                    Content = "Kelmadi",
                    IsChecked = false,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center
                };
                stAttendances.Children.Add(checkBox);
            }
        }
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        PrintValues(keys, lesson);
    }
}
