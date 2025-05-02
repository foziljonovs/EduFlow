using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Services.Courses.Course;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Desktop.Windows.GroupForWindows;

/// <summary>
/// Interaction logic for GroupForAddStudentWindow.xaml
/// </summary>
public partial class GroupForAddStudentWindow : Window
{
    private List<StudentForResultDto> ChooseStudents = new List<StudentForResultDto>();

    private readonly IStudentService _studentService;
    private readonly IGroupService _groupService;
    private readonly ICourseService _courseService;
    public GroupForAddStudentWindow()
    {
        InitializeComponent();
        this._studentService = new StudentService();
        this._groupService = new GroupService();
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

    private async Task GetCourses()
    {
        var courses = await Task.Run(async () => await _courseService.GetAllAsync());

        if (courses.Any())
        {
            courseComboBox.Items.Clear();

            courseComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Kurs tanlang",
                IsSelected = true,
                IsEnabled = false
            });

            foreach (var item in courses)
            {
                courseComboBox.Items.Add(new ComboBoxItem
                {
                    Content = item.Name,
                    Tag = item.Id
                });
            }
        }
        else
            notifier.ShowWarning("kurslar topilmadi!");
    }

    private async Task GetStudents()
    {
        var students = await Task.Run(async () => await _studentService.GetAllAsync());
        
        var result = students.Where(x => x.StudentCourses
            .Any(y => y.Status == Domain.Enums.EnrollmentStatus.Pending)).ToList();

        ShowStudents(result);
    }



    private void ShowStudents(List<StudentForResultDto> students)
    {
        int count = 1;

        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Visible;

        if (students.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            studentforEmptyData.Visibility = Visibility.Collapsed;

            foreach(var item in students)
            {
                var courseName = item.StudentCourses
                    .FirstOrDefault(x => x.Status == Domain.Enums.EnrollmentStatus.Pending)
                    .Course.Name ?? "Nomalum";

                StudentForGroupComponent component = new StudentForGroupComponent();
                component.SetValues(
                    count,
                    item.Id,
                    item.Fullname,
                    courseName,
                    item.Age,
                    item.Address,
                    item.PhoneNumber);

                stStudents.Children.Add(component);
                count++;
            }
        }
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            studentforEmptyData.Visibility = Visibility.Visible;
        }
    }

    private async void LoadWindow()
    {
        await GetCourses();
        await GetStudents();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadWindow();
    }

    private void closeBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void dtEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void courseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}
