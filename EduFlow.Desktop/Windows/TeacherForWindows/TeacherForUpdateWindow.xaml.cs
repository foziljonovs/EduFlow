using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.TeacherForWindows;

/// <summary>
/// Interaction logic for TeacherForUpdateWindow.xaml
/// </summary>
public partial class TeacherForUpdateWindow : Window
{
    private readonly ITeacherService _service;
    private readonly ICourseService _courseService;
    private long Id { get; set; }
    private TeacherForResultDto Teacher { get; set; } = new TeacherForResultDto();
    public TeacherForUpdateWindow()
    {
        InitializeComponent();
        this._service = new TeacherService();
        this._courseService = new CourseService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 50,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    Notifier notifierThis = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive),
            corner: Corner.TopRight,
            offsetX: 50,
            offsetY: 20);

        cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            notificationLifetime: TimeSpan.FromSeconds(3),
            maximumNotificationCount: MaximumNotificationCount.FromCount(2));

        cfg.Dispatcher = Application.Current.Dispatcher;
    });

    public void setId(long id)
        => this.Id = id;

    private async Task GetTeacher()
    {
        if (this.Id > 0)
        {
            var teacher = await _service.GetByIdAsync(this.Id);

            if(teacher is not null)
            {
                this.Teacher = teacher;
                fullNameTxt.Text = teacher.User.Firstname + " " + teacher.User.Lastname;
                phoneNumberTxt.Text = teacher.User.PhoneNumber;
                passwordTxt.Text = teacher.User.Password;
                skillTxt.Text = string.Join(", ", teacher.Skills);
            }
        }
        else
            notifierThis.ShowError("Xatolik yuz berdi, iltimos qayta yuklang!");
    }

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Tag = course.Id,
                    Content = course.Name
                };
                courseComboBox.Items.Add(item);
            }
        }
        else
            notifierThis.ShowWarning("Kurslar topilmadi, iltimos qayta yuklang!");
    }
}
