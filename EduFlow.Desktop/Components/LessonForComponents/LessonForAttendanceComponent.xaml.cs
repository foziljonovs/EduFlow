using EduFlow.BLL.DTOs.Courses.Attendance;
using EduFlow.BLL.DTOs.Courses.Lesson;
using EduFlow.Desktop.Integrated.Services.Courses.Lesson;
using EduFlow.Desktop.Windows;
using EduFlow.Domain.Entities.Courses;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Components.LessonForComponents;

/// <summary>
/// Interaction logic for LessonForAttendanceComponent.xaml
/// </summary>
public partial class LessonForAttendanceComponent : UserControl
{
    private readonly ILessonService _service;
    private Dictionary<int, long> keys = new Dictionary<int, long>();
    private LessonForResultDto lesson = new LessonForResultDto();
    public bool isChanged { get; private set; } = false;
    public event Func<Task> OnGetValues = null!;
    public LessonForAttendanceComponent()
    {
        InitializeComponent();
        this._service = new LessonService();
    }

    Notifier notifierThis = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 20,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;

        cfg.DisplayOptions.Width = 200;
        cfg.DisplayOptions.TopMost = true;
    });

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
                attendance = new AttendanceForShortResultDto
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
                Tag = attendance.Id,
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
        if (sender is CheckBox checkBox && checkBox.Tag is long attendanceId)
        {
            isChanged = true;

            var attendance = lesson.Attendances.FirstOrDefault(a => a.Id == attendanceId);
            if (attendance is not null)
            {
                attendance.IsActived = true;
                isChanged = true;
            }
        }
    }

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.Tag is long attendanceId)
        {
            var attendance = lesson.Attendances.FirstOrDefault(a => a.Id == attendanceId);
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
            if(child is CheckBox checkBox && checkBox.Tag is long attendanceId)
            {
                var attendance = lesson.Attendances.FirstOrDefault(a => a.Id == attendanceId);

                result.Add(new AttendanceForUpdateRangeDto
                {
                    Id = long.Parse(checkBox.Tag.ToString()), 
                    StudentId = attendance.StudentId,
                    LessonId = lesson.Id,
                    //Date = DateTime.ParseExact(tbDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    Date = DateTime.Parse(tbDate.Text, new CultureInfo("en-GB")),
                    IsActived = attendance.IsActived
                });
            }

        return result;
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        PrintValues(keys, lesson);
    }

    private async void mnDelete_Click(object sender, RoutedEventArgs e)
    {
        if(this.lesson is not null)
        {
            var messageBox = new MessageBoxWindow($"Darsni o'chirmoqchimisiz", MessageBoxWindow.MessageType.Confirmation, MessageBoxWindow.MessageButtons.OkCancel);
            var messageResult = messageBox.ShowDialog();

            if(messageResult is true)
            {
                var result = await _service.DeleteAsync(lesson.Id);

                if (result)
                {
                    await OnGetValues.Invoke();
                    notifierThis.ShowSuccess("Dars o'chirildi!");
                }
                else
                    notifierThis.ShowWarning("Darsni o'chirib bo'lmadi, iltimos qayta urining!");
            }
        }
        else
        {
            notifierThis.ShowError("Xatolik yuz berdi, qayta urining!");
        }
    }
}
