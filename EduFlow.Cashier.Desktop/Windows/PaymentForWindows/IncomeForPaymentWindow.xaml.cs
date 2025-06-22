using EduFlow.BLL.DTOs.Users.Teacher;
using EduFlow.Desktop.Integrated.Services.Courses.Group;
using EduFlow.Desktop.Integrated.Services.Payments.Payment;
using EduFlow.Desktop.Integrated.Services.Users.Student;
using EduFlow.Desktop.Integrated.Services.Users.Teacher;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace EduFlow.Cashier.Desktop.Windows.PaymentForWindows;

/// <summary>
/// Interaction logic for IncomeForPaymentWindow.xaml
/// </summary>
public partial class IncomeForPaymentWindow : Window
{
    private readonly IPaymentService _paymentService;
    private readonly IGroupService _groupService;
    private readonly ITeacherService _teacherService;
    private readonly IStudentService _studentService;
    public IncomeForPaymentWindow()
    {
        InitializeComponent();
        this._paymentService = new PaymentService();
        this._groupService = new GroupService();
        this._teacherService = new TeacherService();
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

    private void CloseBtn_Click(object sender, RoutedEventArgs e)
        => this.Close();

    private void MinButton_Click(object sender, RoutedEventArgs e)
        => this.WindowState = WindowState.Minimized;

    private void NormalButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Normal;

        if(this.WindowState == WindowState.Normal)
        {
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
        }
        else
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
    }

    private void MaxButton_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Maximized;

        if(this.WindowState == WindowState.Maximized)
        {
            MaxButton.Visibility = Visibility.Collapsed;
            NormalButton.Visibility = Visibility.Visible;
        }
        else
        {
            MaxButton.Visibility = Visibility.Visible;
            NormalButton.Visibility = Visibility.Collapsed;
        }
    }

    private async Task GetAllTeacher()
    {
        var teachers = await Task.Run(async () => await _teacherService.GetAllAsync());

        ShowTeachers(teachers);
    }

    private void ShowTeachers(List<TeacherForResultDto> teachers)
    {
        if (teachers.Any())
        {
            teacherComboBox.Items.Clear();

            teacherComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Barcha",
                IsSelected = true,
                IsEnabled = false
            });

            foreach (var teacher in teachers)
            {
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = $"{teacher.User?.Firstname} {teacher.User?.Lastname}",
                    Tag = teacher.Id
                };

                teacherComboBox.Items.Add(item);
            }
        }
    }

    private void EmptyDataForLoaded()
    {
        //groups
        stGroups.Children.Clear();
        groupLoader.Visibility = Visibility.Collapsed;
        emptyDataForGroups.Visibility = Visibility.Visible;

        //students
        stStudents.Children.Clear();
        studentLoader.Visibility = Visibility.Collapsed;
        emptyDataForStudents.Visibility = Visibility.Visible;
    }

    private async Task LoadWindow()
    {
        EmptyDataForLoaded();
        await GetAllTeacher();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadWindow();
    }
}
