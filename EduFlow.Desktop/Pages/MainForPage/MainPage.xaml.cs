using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.Desktop.Components.MainForComponents;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Pages.MainForPage;

/// <summary>
/// Interaction logic for MainPage.xaml
/// </summary>
public partial class MainPage : Page
{
    private readonly ICourseService _courseService;
    private readonly IteacherService _teacherService;

    public MainPage()
    {
        InitializeComponent();
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

    private async Task GetAllTeacherCourses(long teacherId)
    {
        stCourses.Children.Clear();
        courseForLoader.Visibility = Visibility.Visible;

        var courses = await Task.Run(async () => await _courseService.GetAllByTeacherIdAsync(teacherId));
        ShowCourse(courses);
    }

    private async Task GetAllCourse()
    {
        stCourses.Children.Clear();
        courseForLoader.Visibility = Visibility.Visible;

        var courses = await Task.Run(async () => await _courseService.GetAllAsync());
        ShowCourse(courses);
    }

    private void ShowCourse(List<CourseForResultDto> courses)
    {
        int count = 1;

        if (courses.Any())
        {
            courseForLoader.Visibility = Visibility.Collapsed;
            foreach (var course in courses)
            {
                MainForCourseComponent component = new MainForCourseComponent();
                component.Tag = course;
                component.SetValues(
                    count,
                    course.Id,
                    course.Name,
                    course.Students?.Count ?? 0,
                    course.Teacher?.User.Firstname ?? "Topilmadi");

                stCourses.Children.Add(component);
                count++;
            }
        }
        else
        {
            courseForLoader.Visibility = Visibility.Collapsed;
            emptyDataForCourse.Visibility = Visibility.Visible;
        }
    }

    private async Task LoadPage()
    {
        var id = IdentitySingelton.GetInstance().Id;
        var role = IdentitySingelton.GetInstance().Role;
        var fullName = IdentitySingelton.GetInstance().Name;

        if (role is Domain.Enums.UserRole.Teacher)
        {
            await GetAllTeacherCourses(id);
            categoryComboBox.Visibility = Visibility.Collapsed;
            teacherComboBox.Visibility = Visibility.Collapsed;
            notifier.ShowInformation($"{fullName} xush kelibsiz ustoz!");
        }
        else if(role is Domain.Enums.UserRole.Administrator)
        {
            await GetAllCourse();
            notifier.ShowInformation($"{fullName} xush kelibsiz!");
        }
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        LoadPage();
    }
}
