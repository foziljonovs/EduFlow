using EduFlow.BLL.DTOs.Users.Student;
using EduFlow.Cashier.Desktop.Components.StudentForComponents;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Domain.Enums;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Pages.StudentForPages;

/// <summary>
/// Interaction logic for StudentPage.xaml
/// </summary>
public partial class StudentPage : Page
{
    private readonly IStudentService _studentService;
    public StudentPage()
    {
        InitializeComponent();
        this._studentService = new StudentService();
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

    private async Task GetAllStudent()
    {
        studentLoader.Visibility = Visibility.Visible;

        var students = await Task.Run(async () => await _studentService.GetAllAsync());

        ShowStudents(students);
    }


    private void ShowStudents(List<StudentForResultDto> students)
    {
        int count = 1;

        if (students.Any())
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Collapsed;

            foreach(var student in students)
            {
                StudentForSecondComponent component = new StudentForSecondComponent();
                component.SetValues(
                    student.Id,
                    count,
                    student.Fullname,
                    student.Age,
                    student.Address ?? "Nomalum",
                    student.PhoneNumber,
                    student.Groups.Any() ? student.Groups.First(x => x.IsStatus == Status.Active).Name : "Nomalum");

                stStudents.Children.Add(component);
                count++;
            }
        }
        else
        {
            studentLoader.Visibility = Visibility.Collapsed;
            emptyDataForStudent.Visibility = Visibility.Visible;
        }
    }

    private async Task LoadedPage()
    {
        await GetAllStudent();
    }

    private bool IsLoaded = false;
    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        if(!IsLoaded)
        {
            IsLoaded = true;
            LoadedPage();
        }
    }
}
