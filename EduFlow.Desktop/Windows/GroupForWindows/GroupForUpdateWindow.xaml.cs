using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.GroupForWindows;

/// <summary>
/// Interaction logic for GroupForUpdateWindow.xaml
/// </summary>
public partial class GroupForUpdateWindow : Window
{
    private readonly IGroupService _service;
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    private long Id { get; set; }
    private GroupForResultDto Group {  get; set; } = new GroupForResultDto();
    public GroupForUpdateWindow()
    {
        InitializeComponent();
        this._service = new GroupService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
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

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    public void setId(long id)
        => this.Id = id;

    private async Task GetGroup()
    {
        if (this.Id > 0)
        {
            var group = await _service.GetByIdAsync(this.Id);

            if (group is not null)
            {
                this.Group = group;
                nameTxt.Text = group.Name;
            }
            else
                notifierThis.ShowWarning("Malumotlar topilmadi, iltimos qayta yuklang!");
        }
        else
            notifierThis.ShowError("Xatolik yuz berdi!");
    }

    private async Task GetAllTeacher()
    {
        var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

        if (teachers.Any())
        {
            foreach(var item in teachers)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Tag = item.Id,
                    Content = item.User.Firstname + " " + item.User.Lastname
                };

                teacherComboBox.Items.Add(comboBoxItem);
            }
        }
        else
            notifierThis.ShowWarning("O'qituvchilar topilmadi, iltimos qayta yuklang!");
    }

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        if (courses.Any())
        {
            foreach(var item in courses)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Tag = item.Id,
                    Content = item.Name
                };

                courseComboBox.Items.Add(comboBoxItem);
            }
        }
        else
            notifierThis.ShowWarning("Kurslar topilmadi, iltimos qayta yuklang!");
    }

    private void Favourites()
    {
        if(this.Group is not null)
        {
            foreach(ComboBoxItem teacherComboBoxItem in teacherComboBox.Items)
                if(teacherComboBoxItem.Tag is long teacherId && teacherId == this.Group.TeacherId)
                {
                    teacherComboBox.SelectedItem = teacherComboBoxItem;
                    break;
                }

            foreach(ComboBoxItem courseComboBoxItem in courseComboBox.Items)
                if(courseComboBoxItem.Tag is long courseId && courseId == this.Group.CourseId)
                {
                    courseComboBox.SelectedItem = courseComboBoxItem;
                    break;
                }
        }
        else
        {
            notifierThis.ShowWarning("Guruh ma'lumotlari noto'g'ri, iltimos qayta yuklang!");
        }
    }

    private async void LoadedWindow()
    {
        await GetGroup();
        await GetAllTeacher();
        await GetAllCourse();

        Favourites();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadedWindow();
    }
}
