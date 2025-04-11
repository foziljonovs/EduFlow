using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Components.GroupForComponents;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Pages.GroupForPages;

/// <summary>
/// Interaction logic for GroupPage.xaml
/// </summary>
public partial class GroupPage : Page
{
    private readonly IGroupService _groupService;
    private readonly ICourseService _courseService;
    private readonly ITeacherService _teacherService;
    private TeacherForResultDto _teacher;
    public GroupPage()
    {
        InitializeComponent();
        this._groupService = new GroupService();
        this._courseService = new CourseService();
        this._teacherService = new TeacherService();
    }

    Notifier notifier = new Notifier(cfg =>
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

    private async Task GetAllCourse()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());
        if (courses.Any())
        {
            foreach (var item in courses)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.Name;
                comboBoxItem.Tag = item.Id;
                courseComboBox.Items.Add(comboBoxItem);
            }
        }
        else
        {
            notifier.ShowWarning("Kurslar topilmadi!");
        }
    }

    private async Task GetAllTeacher()
    {
        var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());
        if (teachers.Any())
        {
            foreach (var item in teachers)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.User.Firstname + " " + item.User.Lastname;
                comboBoxItem.Tag = item.Id;
                teacherComboBox.Items.Add(comboBoxItem);
            }
        }
        else
        {
            notifier.ShowWarning("O'qituvchilar topilmadi!");
        }
    }

    private async Task GetAllGroup()
    {
        stGroups.Children.Clear();
        groupLoader.Visibility = Visibility.Visible;
        var groups = await Task.Run(async () => await _groupService.GetAllAsync());

        ShowGroups(groups);
    }

    private async Task GetAllGroupByTeacherId(long id)
    {
        stGroups.Children.Clear();
        groupLoader.Visibility = Visibility.Visible;
        var groups = await Task.Run(async () => await _groupService.GetAllByTeacherIdAsync(id));

        ShowGroups(groups);
    }

    private void ShowGroups(List<GroupForResultDto> groups)
    {
        int count = 1;
        stGroups.Children.Clear();

        if (groups.Any())
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Collapsed;

            foreach (var item in groups)
            {
                GroupForComponent component = new GroupForComponent();
                component.Tag = item;
                component.setValues(
                    item.Id,
                    count,
                    item.Name,
                    item.Course.Name,
                    "Abdulvosid", //vaqtinchalik, keyinroq guruh va o'qituvchi releashion ship qilinib togri chaqiriladi
                    item.Students.Count,
                    item.IsStatus,
                    DateTime.UtcNow.AddHours(5)); // vaqtincha, keyinroq startedDate ni chaqrib qoyiladi

                stGroups.Children.Add(component);
                count++;
            }
        }
        else
        {
            groupLoader.Visibility = Visibility.Collapsed;
            emptyData.Visibility = Visibility.Visible;
        }
    }
    private async Task<long> GetTeacher(long userId)
    {
        var teacher = await Task.Run(async () => await _teacherService.GetByUserIdAsync(userId));

        if (teacher is null)
        {
            notifier.ShowInformation("Ustoz topilmadi!");
            return 0;
        }

        _teacher = teacher;
        return teacher.Id;
    }

    private async Task LoadPage()
    {
        var id = IdentitySingelton.GetInstance().Id;
        var role = IdentitySingelton.GetInstance().Role;

        if(role == Domain.Enums.UserRole.Teacher)
        {
            courseComboBox.Visibility = Visibility.Collapsed;
            teacherComboBox.Visibility = Visibility.Collapsed;
            createGroupBtn.Visibility = Visibility.Collapsed;

            var teacherId = await GetTeacher(id);
            if (teacherId == 0)
            {
                stGroups.Children.Clear();
                emptyData.Visibility = Visibility.Visible;
                return;
            }

            await GetAllGroupByTeacherId(teacherId);
        }
        else
        {
            await GetAllGroup();
            await GetAllCourse();
            await GetAllTeacher();
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        LoadPage();
    }
}
