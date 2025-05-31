using EduFlow.BLL.DTOs.Courses.Course;
using EduFlow.BLL.DTOs.Courses.Group;
using EduFlow.Desktop.Components.GroupForComponents;
using EduFlow.Desktop.Components.TeacherForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Domain.Entities.Courses;
using EduFlow.Domain.Entities.Users;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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
    private readonly IGroupService _groupService;
    private long CourseId { get; set; }
    public CourseForViewWindow()
    {
        InitializeComponent();
        this._courseService = new CourseService();
        this._groupService = new GroupService();
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

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if (NormalButton.Visibility == Visibility.Visible)
        {
            this.NormalButton.Visibility = Visibility.Collapsed;
            this.MaxButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.MaxButton.Visibility = Visibility.Collapsed;
            this.NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if (MaxButton.Visibility == Visibility.Visible)
        {
            this.MaxButton.Visibility = Visibility.Collapsed;
            this.NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            this.NormalButton.Visibility = Visibility.Collapsed;
            this.MaxButton.Visibility = Visibility.Visible;
        }
    }

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
            tbCategory.Content = course.Category?.Name ?? "Nomalum";

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
        stTeachers.Children.Clear();

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

                component.SelectedComponent = async () =>
                {
                    if(selectedTeacherComponent != null)
                        selectedTeacherComponent.spBorder.Background = Brushes.Transparent;

                    selectedTeacherComponent = component;
                    selectedTeacherComponent.spBorder.Background = Brushes.LightGray;

                    await ShowCourseForSelectedTeacher();
                };

                stTeachers.Children.Add(component);
            }
        }
        else
        {
            stTeachers.Visibility = Visibility.Collapsed;
            emptyDataForTeacher.Visibility = Visibility.Visible;
        }
    }

    private async Task<List<GroupForResultDto>> GetAllGroupForTeacherId(long teacherId)
    {
        var groups = await _groupService.GetAllByTeacherIdAsync(teacherId);
        if (groups.Any())
            return groups;
        else
            return new List<GroupForResultDto>();
    }

    private TeacherForMinComponent selectedTeacherComponent = null!;
    private async Task ShowCourseForSelectedTeacher()
    {
        stGroups.Children.Clear();
        groupForLoader.Visibility = Visibility.Visible;

        int count = 1;
        var groups = await GetAllGroupForTeacherId(selectedTeacherComponent.GetId());

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
            groupForLoader.Visibility = Visibility.Collapsed;
            emptyDataForGroup.Visibility = Visibility.Visible;
        }
    }

    private void ShowGroups(List<Group> groups)
    {
        stGroups.Children.Clear();
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

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ShowCourse();
    }
}
