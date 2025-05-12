using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.Desktop.Components.GroupForComponents;
using EduFlow.Desktop.Components.TeacherForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.CourseForWindows;

/// <summary>
/// Interaction logic for CourseForViewWindow.xaml
/// </summary>
public partial class CourseForViewWindow : Window
{
    private readonly ICourseService _courseService;
    private long CourseId { get; set; }
    public CourseForViewWindow()
    {
        InitializeComponent();
        this._courseService = new CourseService();
    }

    Notifier notifier = new Notifier(cfg =>
    {
        cfg.PositionProvider = new WindowPositionProvider(
            parentWindow: Application.Current.MainWindow,
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MaxButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Maximized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Normal;

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    public void SetCourseId(long courseId)
        => this.CourseId = courseId;

    private async Task<CourseForResultDto> GetCourse()
    {
        var course = await _courseService.GetByIdAsync(this.CourseId);

        if (course is not null)
            return course;
        else
            return new CourseForResultDto();
    }

    private async Task ShowCourse()
    {
        var course = await GetCourse();

        if(course is not null)
        {
            tbName.Content = course.Name;
            tbPrice.Content = course.Price.ToString();
            tbTerm.Content = course.Term.ToString();
            tbStatus.Content = course.Archived.ToString();
            tbCategory.Content = course.Category.Name;

            ShowTeachers(course.Teachers);
            ShowGroups(course.Groups);
        }
        else
        {
            notifierThis.ShowWarning("Kurs ma'lumotlarini yuklashda xatolik yuz berdi! qayta urining");
            ThisIsEmpty();
        }
    }

    private void ShowTeachers(List<Teacher> teachers)
    {
        if (teachers.Any())
        {
            loaderForTeacher.Visibility = Visibility.Collapsed;
            emptyDataForTeacher.Visibility = Visibility.Collapsed;

            foreach(var item in teachers)
            {
                TeacherForMinComponent component = new TeacherForMinComponent();
                component.SetValues(
                    item.Id,
                    $"{item.User.Firstname} {item.User.Lastname}");

                stTeachers.Children.Add(component);
            }
        }
        else
        {
            stTeachers.Visibility = Visibility.Collapsed;
            emptyDataForTeacher.Visibility = Visibility.Visible;
        }
    }

    private void ShowGroups(List<Group> groups)
    {
        int count = 1;

        if (groups.Any())
        {
            groupForLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroup.Visibility = Visibility.Collapsed;

            foreach(var item in groups)
            {
                GroupForMinComponent component = new GroupForMinComponent();
                component.SetValues(
                    count,
                    item.Id,
                    item.Name,
                    item.Students.Count,
                    $"{item.Teacher.User.Firstname} {item.Teacher.User.Lastname}",
                    item.IsStatus);

                stGroups.Children.Add(component);
                count++;
            }
        }
        else
        {
            stGroups.Visibility = Visibility.Collapsed;
            emptyDataForGroup.Visibility = Visibility.Visible;
        }
    }

    private void ThisIsEmpty()
    {
        tbName.Content = "Nomalum";
        tbPrice.Content = "0";
        tbTerm.Content = "0";
        tbStatus.Content = "Nomalum";
        tbCategory.Content = "Nomalum";

        stGroups.Visibility = Visibility.Collapsed;
        emptyDataForGroup.Visibility = Visibility.Visible;

        stTeachers.Visibility = Visibility.Collapsed;
        emptyDataForTeacher.Visibility = Visibility.Visible;
    }
}
