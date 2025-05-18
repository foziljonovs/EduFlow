using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.GroupForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.TeacherForWindows;

/// <summary>
/// Interaction logic for TeacherForViewWindow.xaml
/// </summary>
public partial class TeacherForViewWindow : Window
{
    private readonly ITeacherService _teacherService;
    private readonly IGroupService _groupService;
    private long Id { get; set; }
    public TeacherForViewWindow()
    {
        InitializeComponent();
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

    public void SetId(long id)
        => this.Id = id;

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Normal;

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private async Task<TeacherForResultDto> GetTeacher()
    {
        var teacher = await _teacherService.GetByIdAsync(Id);

        if (teacher is not null)
            return teacher;
        else
            return new TeacherForResultDto();
    }

    private async Task<List<GroupForResultDto>> GetGroups()
    {
        var groups = await _groupService.GetAllByTeacherIdAsync(Id);

        if (groups is not null)
            return groups;
        else
            return new List<GroupForResultDto>();
    }

    private async void ShowValues()
    {
        groupsForLoader.Visibility = Visibility.Visible;
        var teacher = await GetTeacher();

        if(teacher is not null)
        {
            tbName.Text = teacher.User.Firstname + " " + teacher.User.Lastname;
            tbCourseName.Text = teacher.Course.Name;
            tbPhoneNumber.Text = teacher.User.PhoneNumber;
            tbSalary.Text = "0";
            tbSkills.Text = teacher.Skills.ToString();
            tbStudentsCount.Text = "0";
        }
        else
        {
            notifierThis.ShowWarning("O'qituvchi ma'lumotlari topilmadi!");
            EmptyValues();
            return;
        }

        var groups = await GetGroups();

        int count = 1;
        if (groups.Any())
        {
            groupsForLoader.Visibility = Visibility.Collapsed;
            groupForEmptyData.Visibility = Visibility.Collapsed;

            foreach (var group in groups)
            {
                GroupForMinComponent component = new GroupForMinComponent();
                component.SetValues(
                    count,
                    group.Id,
                    group.Name,
                    group.Students.Count(),
                    teacher.User.Firstname,
                    group.IsStatus);

                stGroups.Children.Add(component);
                count++;
            }
        }
        else
        {
            notifierThis.ShowWarning("O'qituvchi guruhlari topilmadi!");
            groupsForLoader.Visibility = Visibility.Collapsed;
            groupForEmptyData.Visibility = Visibility.Visible;
        }
    }

    private void EmptyValues()
    {
        tbName.Text = string.Empty;
        tbCourseName.Text = string.Empty;
        tbPhoneNumber.Text = "+998000000000";
        tbSalary.Text = "0";
        tbSkills.Text = string.Empty;
        tbStudentsCount.Text = "0";

        groupForEmptyData.Text = string.Empty;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ShowValues();
    }
}
